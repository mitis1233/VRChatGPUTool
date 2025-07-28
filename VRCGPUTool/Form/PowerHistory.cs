using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VRCGPUTool.Util;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                seriesColumn.Points.Add(new DataPoint(i, (double)dispdata.powerLogData.HourPowerLog[i] / 3600.0));
                usageTotalDay += dispdata.powerLogData.HourPowerLog[i];
                priceOfDay += hourOfPrice[i] * dispdata.powerLogData.HourPowerLog[i];
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

        private async System.Threading.Tasks.Task DrawHistoryMonth(DateTime dispdt, bool isThisMonth)
        {
            DateTime dt = DateTime.Now;
            DataRefreshDate2.Text = dt.ToString();

            UsageGraphMonth.Series.Clear();
            UsageGraphMonth.ChartAreas.Clear();
            UsageGraphMonth.Titles.Clear();
            _ = new Series("chartArea");

            Series seriesColumn = new()
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            ChartArea area = new("area");
            area.AxisX.Title = "日(Day)";
            area.AxisY.Title = "用電量(Wh)";
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.IsMarginVisible = true;

            UsageGraphMonth.ChartAreas.Add(area);
            UsageGraphMonth.Series.Add(seriesColumn);
            UsageGraphMonth.ChartAreas["area"].AxisX.Minimum = 1;

            if (isThisMonth)
            {
                await DataPointAddThisMonth(seriesColumn);
            }
            else
            {
                await DataPointAddPreviousMonth(dispdt, seriesColumn);
            }
        }

        private async Task DataPointAddThisMonth(Series seriesColumn)
        {
            string datelabel = string.Format("{0:D4}年{1}月用電記錄", PlogData.powerLogData.LogDate.Year, PlogData.powerLogData.LogDate.Month);
            LogMonthLabel.Text = datelabel;

            var result = await Task.Run(() =>
            {
                double totalUsage = 0;
                double totalPrice = 0;
                var points = new List<DataPoint>();

                // Calculate for previous days in the month
                for (int i = 1; i < PlogData.powerLogData.LogDate.Day; i++)
                {
                    DateTime loadDate = new(PlogData.powerLogData.LogDate.Year, PlogData.powerLogData.LogDate.Month, i);
                    GPUPowerLog recentlog = new();
                    recentlog.LoadPowerLog(loadDate);

                    int dayUsage = 0;
                    for (int j = 0; j < 24; j++)
                    {
                        dayUsage += recentlog.powerLogData.HourPowerLog[j];
                        totalPrice += hourOfPrice[j] * recentlog.powerLogData.HourPowerLog[j];
                    }
                    points.Add(new DataPoint(i, dayUsage / 3600.0));
                    totalUsage += dayUsage;
                }

                // Calculate for the current day
                int currentDayUsage = 0;
                for (int i = 0; i < 24; i++)
                {
                    currentDayUsage += PlogData.powerLogData.HourPowerLog[i];
                    totalPrice += hourOfPrice[i] * PlogData.powerLogData.HourPowerLog[i];
                }
                points.Add(new DataPoint(PlogData.powerLogData.LogDate.Day, currentDayUsage / 3600.0));
                totalUsage += currentDayUsage;

                return new { Points = points, TotalUsage = totalUsage, TotalPrice = totalPrice };
            });

            foreach (var point in result.Points)
            {
                seriesColumn.Points.Add(point);
            }

            int daysInMonth = DateTime.DaysInMonth(PlogData.powerLogData.LogDate.Year, PlogData.powerLogData.LogDate.Month);
            UsageGraphMonth.ChartAreas["area"].AxisX.Maximum = daysInMonth;

            double usageTotalMonthKwh = result.TotalUsage / (3600.0 * 1000.0);
            double priceOfMonth = result.TotalPrice / (3600.0 * 1000.0);

            priceMonth.Text = string.Format("電費:{0:f1}元", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("總計: {0:f2}kWh", usageTotalMonthKwh);
        }

        private async System.Threading.Tasks.Task DataPointAddPreviousMonth(DateTime dt, Series seriesColumn)
        {
            string datelabel = string.Format("{0:D4}年{1}月の電力使用履歴", dt.Year, dt.Month);
            LogMonthLabel.Text = datelabel;

            double usageTotalMonth = 0.0;
            double priceOfMonth = 0.0;

            int Days = DateTime.DaysInMonth(dt.Year, dt.Month);
            UsageGraphMonth.ChartAreas["area"].AxisX.Maximum = Days;

            for (int i = 1; i <= Days; i++)
            {
                DateTime loadDate = new(dt.Year, dt.Month, i);
                GPUPowerLog recentlog = new();
                recentlog.LoadPowerLog(loadDate);

                int dayUsage = 0;
                for (int j = 0; j < 24; j++)
                {
                    dayUsage += recentlog.powerLogData.HourPowerLog[j];
                    priceOfMonth += hourOfPrice[j] * recentlog.powerLogData.HourPowerLog[j];
                }
                seriesColumn.Points.Add(new DataPoint(i, dayUsage / 3600.0));
                usageTotalMonth += dayUsage;
            }
            usageTotalMonth /= (3600.0 * 1000.0);
            priceOfMonth /= (3600.0 * 1000.0);
            priceMonth.Text = string.Format("電費:{0:f1}元", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("合計: {0:f2}kWh", usageTotalMonth);
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
                await logcsv.ExportCsvMonth(dispDataMonth, isThisMonth);
            }
        }
    }
}