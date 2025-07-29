using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VRCGPUTool.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace VRCGPUTool.Form
{
    public partial class PowerHistory : System.Windows.Forms.Form
    {
        public MainForm MainObj;

        public PowerHistory(MainForm fm)
        {
            MainObj = fm;
            InitializeComponent();
            PlogData = MainObj.gpuPlog;
            dispDataDay = DateTime.Today;
            dispDataMonth = DateTime.Today;
            powerPofile = new PowerProfile();
            LoadInitialDataAsync();
            UnitPriceRefresh();
        }

        private async void LoadInitialDataAsync()
        {
            await PowerProfile.LoadProfileAsync(powerPofile);
        }

        readonly GPUPowerLog PlogData;
        readonly PowerProfile powerPofile;
        UnitPriceSetting pricesetting;

        readonly double[] hourOfPrice = new double[24];

        private DateTime dispDataDay;
        private DateTime dispDataMonth;

        private async void PowerHistory_Load(object sender, EventArgs e)
        {
            DrawHistoryDay(PlogData);
            await DrawHistoryMonth(dispDataMonth);
        }

        private void DrawHistoryDay(GPUPowerLog dispdata)
        {
            DateTime dt = DateTime.Now;
            DataRefreshDate.Text = dt.ToString();

            string datelabel = string.Format("{0:D4}年{1}月{2}日用電記錄", dispDataDay.Year, dispDataDay.Month, dispDataDay.Day);

            LogDateLabel.Text = datelabel;

            UsageGraphDay.Series.Clear();
            UsageGraphDay.ChartAreas.Clear();
            UsageGraphDay.Titles.Clear();
            _ = new Series("chartArea");

            Series seriesColumn = new()
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            double usageTotalDay = 0.0;
            double priceOfDay = 0.0;

            DateTime thresholdDate = DateTime.Now.AddMonths(-3).Date;
            if (dispDataDay < thresholdDate)
            {
                // Use HistoricalPowerLogs for old data
                using var connection = new SQLiteConnection($"Data Source={dispdata.GetDbPath()};Version=3;");
                connection.Open();
                string sql = "SELECT TotalPower FROM HistoricalPowerLogs WHERE LogDate = @logDate";
                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@logDate", dispDataDay.ToString("yyyy-MM-dd"));
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    double totalPower = Convert.ToDouble(result);
                    // Cannot break down into hourly data, so display total in the first hour as a placeholder
                    seriesColumn.Points.Add(new DataPoint(0, totalPower / 3600.0));
                    for (int i = 1; i < 24; i++)
                    {
                        seriesColumn.Points.Add(new DataPoint(i, 0.0));
                    }
                    usageTotalDay = totalPower / (3600.0 * 1000.0); // Convert to kWh
                    priceOfDay = usageTotalDay * hourOfPrice.Average();
                }
                else
                {
                    for (int i = 0; i < 24; i++)
                    {
                        seriesColumn.Points.Add(new DataPoint(i, 0.0));
                    }
                }
            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    // The stored value is an accumulation of power readings every 1 second, which is equivalent to Watt-seconds.
                    double usageKwh = (double)dispdata.currentDayPowerLog[i] / (3600.0 * 1000.0);
                    seriesColumn.Points.Add(new DataPoint(i, usageKwh * 1000.0)); // Display in Wh
                    usageTotalDay += usageKwh;
                    priceOfDay += hourOfPrice[i] * usageKwh;
                }
            }

            priceDay.Text = string.Format("電費:{0:f1}元", priceOfDay);
            DaylyTotalPower.Text = string.Format("總計: {0:f2}kWh", usageTotalDay);

            ChartArea area = new("area");
            area.AxisX.Title = "時間(h)";
            area.AxisY.Title = "用電量(Wh)";
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.IsMarginVisible = true;

            UsageGraphDay.ChartAreas.Add(area);
            UsageGraphDay.Series.Add(seriesColumn);
            UsageGraphDay.ChartAreas["area"].AxisX.Minimum = 0;
            UsageGraphDay.ChartAreas["area"].AxisX.Maximum = 23;

            NextDayData.Enabled = (dispDataDay.Date < DateTime.Now.Date);
        }

        private void PreviousDayData_Click(object sender, EventArgs e)
        {
            dispDataDay = dispDataDay.AddDays(-1);

            if (DateTime.Now.Date == dispDataDay.Date)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                GPUPowerLog plog = new();
                plog.LoadPowerLog(dispDataDay);
                DrawHistoryDay(plog);
            }
        }

        private void NextDayData_Click(object sender, EventArgs e)
        {
            dispDataDay = dispDataDay.AddDays(1);

            if (DateTime.Now.Date == dispDataDay.Date)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                GPUPowerLog plog = new();
                plog.LoadPowerLog(dispDataDay);
                DrawHistoryDay(plog);
            }
        }

        private void DataRefresh_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Date == dispDataDay.Date)
            {
                DrawHistoryDay(PlogData);
            }
        }

        internal async Task DrawHistoryMonth(DateTime dt)
        {
            DateTime now = DateTime.Now;
            DataRefreshDate2.Text = now.ToString();

            UsageGraphMonth.Series.Clear();
            UsageGraphMonth.ChartAreas.Clear();
            UsageGraphMonth.Titles.Clear();

            string datelabel = string.Format("{0:D4}年{1}月用電記錄", dt.Year, dt.Month);
            LogMonthLabel.Text = datelabel;

            Series seriesColumn = new()
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            double usageTotalMonth = 0.0;
            double priceOfMonth = 0.0;

            Dictionary<int, int> monthlyData = [];

            using (var connection = new SQLiteConnection($"Data Source={PlogData.GetDbPath()};Version=3;"))
            {
                connection.Open();
                // First, check if there's a precomputed monthly summary
                string summarySql = "SELECT TotalPower FROM MonthlySummary WHERE LogMonth = @month";
                using var summaryCommand = new SQLiteCommand(summarySql, connection);
                summaryCommand.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                var summaryResult = summaryCommand.ExecuteScalar();
                if (summaryResult != null)
                {
                    usageTotalMonth = Convert.ToDouble(summaryResult) / (3600.0 * 1000.0); // Convert to kWh
                    // Use precomputed daily data if available
                    DateTime thresholdDate = DateTime.Now.AddMonths(-3).Date;
                    DateTime firstDayOfMonth = new(dt.Year, dt.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    if (lastDayOfMonth >= thresholdDate)
                    {
                        // Use DailySummary for recent data
                        string detailSql = "SELECT SUBSTR(LogDate, 9, 2) as Day, TotalPower FROM DailySummary WHERE LogDate LIKE @month || '%'";
                        using var detailCommand = new SQLiteCommand(detailSql, connection);
                        detailCommand.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                        using var detailReader = detailCommand.ExecuteReader();
                        while (detailReader.Read())
                        {
                            int day = Convert.ToInt32(detailReader["Day"]);
                            double power = Convert.ToDouble(detailReader["TotalPower"]);
                            monthlyData[day] = (int)power;
                        }
                    }
                    else
                    {
                        // Use HistoricalPowerLogs for old data
                        string histSql = "SELECT SUBSTR(LogDate, 9, 2) as Day, TotalPower FROM HistoricalPowerLogs WHERE LogDate LIKE @month || '%'";
                        using var histCommand = new SQLiteCommand(histSql, connection);
                        histCommand.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                        using var histReader = histCommand.ExecuteReader();
                        while (histReader.Read())
                        {
                            int day = Convert.ToInt32(histReader["Day"]);
                            double power = Convert.ToDouble(histReader["TotalPower"]);
                            monthlyData[day] = (int)power;
                        }
                    }
                }
                else
                {
                    // Fallback to original query if no summary exists
                    string sql = @"
                        SELECT SUBSTR(LogDate, 9, 2) as Day, LogHour, PowerDraw
                        FROM PowerLogs
                        WHERE LogDate LIKE @month || '%';
                    ";
                    using var command = new SQLiteCommand(sql, connection);
                    command.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int day = Convert.ToInt32(reader["Day"]);
                        double powerDraw = Convert.ToDouble(reader["PowerDraw"]);
                        if (!monthlyData.ContainsKey(day))
                        {
                            monthlyData[day] = 0;
                        }
                        monthlyData[day] += (int)powerDraw;
                    }
                }
            }

            int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
            for (int d = 1; d <= daysInMonth; d++)
            {
                double dailyUsageWh = 0.0;
                if (monthlyData.TryGetValue(d, out int value))
                {
                    dailyUsageWh = value / 3600.0;
                    usageTotalMonth += dailyUsageWh / 1000.0; // Convert Wh to kWh for total
                }
                seriesColumn.Points.Add(new DataPoint(d, dailyUsageWh));

                double dailyPrice = 0.0;
                if (hourOfPrice.Length == 24) {
                    dailyPrice = (dailyUsageWh / 1000.0) * hourOfPrice.Average();
                }
                priceOfMonth += dailyPrice;
            }

            priceMonth.Text = string.Format("電費:{0:f1}元", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("總計: {0:f2}kWh", usageTotalMonth);

            ChartArea area = new("area");
            area.AxisX.Title = "日期(d)";
            area.AxisY.Title = "用電量(Wh)";
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.IsMarginVisible = true;
            area.AxisX.Minimum = 1;
            area.AxisX.Maximum = daysInMonth;

            UsageGraphMonth.ChartAreas.Add(area);
            UsageGraphMonth.Series.Add(seriesColumn);

            NextMonthData.Enabled = (dispDataMonth.Year < DateTime.Now.Year || (dispDataMonth.Year == DateTime.Now.Year && dispDataMonth.Month < DateTime.Now.Month));
            await Task.CompletedTask;
        }

        private async void PreviousMonthData_Click(object sender, EventArgs e)
        {
            dispDataMonth = dispDataMonth.AddMonths(-1);
            await DrawHistoryMonth(dispDataMonth);
        }

        private async void NextMonthData_Click(object sender, EventArgs e)
        {
            dispDataMonth = dispDataMonth.AddMonths(1);
            await DrawHistoryMonth(dispDataMonth);
        }

        private async void TabRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabRange.SelectedIndex == 0)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                await DrawHistoryMonth(dispDataMonth);
            }
        }

        private async void PowerPlanSetting_Click(object sender, EventArgs e)
        {
            pricesetting = new UnitPriceSetting(powerPofile);
            pricesetting.ShowDialog();

            UnitPriceRefresh();

            DrawHistoryDay(PlogData);
            await DrawHistoryMonth(dispDataMonth);
        }

        private void UnitPriceRefresh()
        {
            // Initialize all prices to a default value (e.g., the first profile's price)
            if (powerPofile.pfData.ProfileCount > 0)
            {
                double initialPrice = powerPofile.pfData.Unit[0];
                for (int i = 0; i < 24; i++)
                {
                    hourOfPrice[i] = initialPrice;
                }
            }

            // Set prices based on defined time splits
            for (int i = 0; i < powerPofile.pfData.ProfileCount; i++)
            {
                int startTime = powerPofile.pfData.SplitTime[i];
                double unitPrice = powerPofile.pfData.Unit[i];

                int endTime;
                if (i + 1 < powerPofile.pfData.ProfileCount)
                {
                    endTime = powerPofile.pfData.SplitTime[i + 1];
                } else
                {
                    endTime = 24;
                }

                for (int h = startTime; h < endTime; h++)
                {
                    if (h < 24)
                    {
                        hourOfPrice[h] = unitPrice;
                    }
                }
            }
        }

        private async void SaveAction(object sender, EventArgs e)
        {
            PowerLogCsv logcsv = new(MainObj, this);
            if (TabRange.SelectedIndex == 0)
            {
                if (DateTime.Now.Date == dispDataDay.Date)
                {
                    logcsv.ExportCsvDay(PlogData);
                }
                else
                {
                    GPUPowerLog plog = new();
                    plog.LoadPowerLog(dispDataDay);
                    logcsv.ExportCsvDay(plog);
                }
            }
            else
            {
                await logcsv.ExportCsvMonth(dispDataMonth);
            }
        }
    }
}