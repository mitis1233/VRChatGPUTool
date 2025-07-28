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
            await DrawHistoryMonth(dispDataMonth, true);
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

            for (int i = 0; i < 24; i++)
            {
                seriesColumn.Points.Add(new DataPoint(i, (double)dispdata.currentDayPowerLog[i] / 3600.0));
                usageTotalDay += dispdata.currentDayPowerLog[i];
                priceOfDay += hourOfPrice[i] * dispdata.currentDayPowerLog[i];
            }

            usageTotalDay /= (3600.0 * 1000.0); //Kwh
            priceOfDay /= (3600.0 * 1000.0);

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

        private async Task DrawHistoryMonth(DateTime dt, bool isThisMonth)
        {
            DateTime now = DateTime.Now;
            DataRefreshDate2.Text = now.ToString();

            UsageGraphMonth.Series.Clear();
            UsageGraphMonth.ChartAreas.Clear();
            UsageGraphMonth.Titles.Clear();

            string datelabel = string.Format("{0:D4}年{1}月用電記錄", dt.Year, dt.Month);
            LogMonthLabel.Text = datelabel;

            Series seriesColumn = new Series()
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            double usageTotalMonth = 0.0;
            double priceOfMonth = 0.0;

            Dictionary<int, int> monthlyData = new Dictionary<int, int>();

            using (var connection = new SQLiteConnection($"Data Source={PlogData.GetDbPath()};Version=3;"))
            {
                connection.Open();
                string sql = "SELECT SUBSTR(LogDate, 9, 2) as Day, SUM(PowerDraw) as TotalPower FROM PowerLogs WHERE SUBSTR(LogDate, 1, 7) = @month GROUP BY Day";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int day = Convert.ToInt32(reader["Day"]);
                            int totalPower = Convert.ToInt32(reader["TotalPower"]);
                            monthlyData[day] = totalPower;
                            usageTotalMonth += totalPower;
                        }
                    }
                }
            }

            int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                int power = monthlyData.ContainsKey(i) ? monthlyData[i] : 0;
                seriesColumn.Points.Add(new DataPoint(i, (double)power / 3600.0));
                // Assuming average price for month calculation for simplicity
                priceOfMonth += (power / 3600.0 / 1000.0) * hourOfPrice.Average(); 
            }

            usageTotalMonth /= (3600.0 * 1000.0); //Kwh

            priceMonth.Text = string.Format("電費:{0:f1}元", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("總計: {0:f2}kWh", usageTotalMonth);

            ChartArea area = new ChartArea("area");
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
            await DrawHistoryMonth(dispDataMonth, false);
        }

        private async void NextMonthData_Click(object sender, EventArgs e)
        {
            dispDataMonth = dispDataMonth.AddMonths(1);
            await DrawHistoryMonth(dispDataMonth, false);
        }

        private async void TabRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabRange.SelectedIndex == 0)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                await DrawHistoryMonth(dispDataMonth, true);
            }
        }

        private async void PowerPlanSetting_Click(object sender, EventArgs e)
        {
            pricesetting = new UnitPriceSetting(powerPofile);
            pricesetting.ShowDialog();

            UnitPriceRefresh();

            DrawHistoryDay(PlogData);
            await DrawHistoryMonth(dispDataMonth, true);
        }

        private void UnitPriceRefresh()
        {
            double unitP;
            int lastread = 23;
            for (int i = (powerPofile.pfData.ProfileCount - 1); i >= 0; i--)
            {
                for (int j = 23; j >= 0; j--)
                {
                    if (j == powerPofile.pfData.SplitTime[i])
                    {
                        unitP = powerPofile.pfData.Unit[i];
                        for (int k = lastread; k >= j; k--)
                        {
                            hourOfPrice[k] = unitP;
                        }
                        lastread = --j;
                        break;
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
                bool isThisMonth = (dispDataMonth.Year == DateTime.Now.Year && dispDataMonth.Month == DateTime.Now.Month);
                await logcsv.ExportCsvMonth(dispDataMonth);
            }
        }
    }
}