using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VRCGPUTool.Util;

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

        double[] hourOfPrice = new double[24];

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

            string datelabel = string.Format("{0:D4}年{1}月{2}日の電力使用履歴", dispDataDay.Year, dispDataDay.Month, dispDataDay.Day);

            LogDateLabel.Text = datelabel;

            UsageGraphDay.Series.Clear();
            UsageGraphDay.ChartAreas.Clear();
            UsageGraphDay.Titles.Clear();

            Series seriesLine = new Series("chartArea");

            Series seriesColumn = new Series
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            double usageTotalDay = 0.0;
            double priceOfDay = 0.0;

            for (int i = 0; i < 24; i++)
            {
                seriesColumn.Points.Add(new DataPoint(i, (double)dispdata.rawdata.HourPowerLog[i] / 3600.0));
                usageTotalDay += dispdata.rawdata.HourPowerLog[i];
                priceOfDay += hourOfPrice[i] * dispdata.rawdata.HourPowerLog[i];
            }

            usageTotalDay /= (3600.0 * 1000.0); //Kwh
            priceOfDay /= (3600.0 * 1000.0);

            priceDay.Text = string.Format("電気代:{0:f1}円", priceOfDay);
            DaylyTotalPower.Text = string.Format("合計: {0:f2}kWh", usageTotalDay);

            ChartArea area = new ChartArea("area");
            area.AxisX.Title = "時間(h)";
            area.AxisY.Title = "電力量(Wh)";
            area.AxisX.LabelStyle.Interval = 1;
            area.AxisX.IsMarginVisible = true;

            UsageGraphDay.ChartAreas.Add(area);
            UsageGraphDay.Series.Add(seriesColumn);
            UsageGraphDay.ChartAreas["area"].AxisX.Minimum = 0;
            UsageGraphDay.ChartAreas["area"].AxisX.Maximum = 23;
        }

        private async void PreviousDayData_Click(object sender, EventArgs e)
        {
            dispDataDay = dispDataDay.AddDays(-1);

            if (DateTime.Now.Date == dispDataDay.Date)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                GPUPowerLog plog = new GPUPowerLog();
                PowerLogFile plogfile = new PowerLogFile(plog);
                await plogfile.LoadPowerLogAsync(dispDataDay, true);
                DrawHistoryDay(plog);
            }
        }

        private async void NextDayData_Click(object sender, EventArgs e)
        {
            dispDataDay = dispDataDay.AddDays(1);

            if (DateTime.Now.Date > PlogData.rawdata.Logdate.Date)
            {
                MessageBox.Show("日付が変わりました。表示内容を更新するにはこのウィンドウを開きなおしてください", "情報");
                return;
            }

            if (DateTime.Now.Date == dispDataDay.Date)
            {
                DrawHistoryDay(PlogData);
            }
            else
            {
                GPUPowerLog plog = new GPUPowerLog();
                PowerLogFile plogfile = new PowerLogFile(plog);
                await plogfile.LoadPowerLogAsync(dispDataDay, true);
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

            Series seriesLine = new Series("chartArea");

            Series seriesColumn = new Series
            {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = false
            };

            ChartArea area = new ChartArea("area");
            area.AxisX.Title = "日(Day)";
            area.AxisY.Title = "電力量(Wh)";
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

        private async System.Threading.Tasks.Task DataPointAddThisMonth(Series seriesColumn)
        {
            string datelabel = string.Format("{0:D4}年{1}月の電力使用履歴", PlogData.rawdata.Logdate.Year, PlogData.rawdata.Logdate.Month);
            LogMonthLabel.Text = datelabel;

            int dayUsage = 0;
            double usageTotalMonth = 0.0;
            double priceOfMonth = 0.0;

            for (int i = 0; i < 24; i++)
            {
                dayUsage += PlogData.rawdata.HourPowerLog[i];
                priceOfMonth += hourOfPrice[i] * PlogData.rawdata.HourPowerLog[i];
            }
            seriesColumn.Points.Add(new DataPoint(PlogData.rawdata.Logdate.Day, dayUsage / 3600.0));
            usageTotalMonth += dayUsage;

            int Days = DateTime.DaysInMonth(PlogData.rawdata.Logdate.Year, PlogData.rawdata.Logdate.Month);
            UsageGraphMonth.ChartAreas["area"].AxisX.Maximum = Days;

            for (int i = 1; i < PlogData.rawdata.Logdate.Day; i++)
            {
                DateTime loadDate = new DateTime(PlogData.rawdata.Logdate.Year, PlogData.rawdata.Logdate.Month, i);
                GPUPowerLog recentlog = new GPUPowerLog();
                PowerLogFile logfile = new PowerLogFile(recentlog);
                int res = await logfile.LoadPowerLogAsync(loadDate, true);

                if (res == 0)
                {
                    int recentDayUsage = 0;
                    for (int j = 0; j < 24; j++)
                    {
                        recentDayUsage += recentlog.rawdata.HourPowerLog[j];
                        priceOfMonth += hourOfPrice[j] * recentlog.rawdata.HourPowerLog[j];
                    }
                    seriesColumn.Points.Add(new DataPoint(i, recentDayUsage / 3600.0));
                    usageTotalMonth += recentDayUsage;
                }
            }
            usageTotalMonth /= (3600.0 * 1000.0);
            priceOfMonth /= (3600.0 * 1000.0);
            priceMonth.Text = string.Format("電気代:{0:f1}円", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("合計: {0:f2}kWh", usageTotalMonth);
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
                DateTime loadDate = new DateTime(dt.Year, dt.Month, i);
                GPUPowerLog recentlog = new GPUPowerLog();
                PowerLogFile logfile = new PowerLogFile(recentlog);
                int res = await logfile.LoadPowerLogAsync(loadDate, true);

                if (res == 0)
                {
                    int dayUsage = 0;
                    for (int j = 0; j < 24; j++)
                    {
                        dayUsage += recentlog.rawdata.HourPowerLog[j];
                        priceOfMonth += hourOfPrice[j] * recentlog.rawdata.HourPowerLog[j];
                    }
                    seriesColumn.Points.Add(new DataPoint(i, dayUsage / 3600.0));
                    usageTotalMonth += dayUsage;
                }
            }
            usageTotalMonth /= (3600.0 * 1000.0);
            priceOfMonth /= (3600.0 * 1000.0);
            priceMonth.Text = string.Format("電気代:{0:f1}円", priceOfMonth);
            MonthlyTotalPower.Text = string.Format("合計: {0:f2}kWh", usageTotalMonth);
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
            PowerLogCsv logcsv = new PowerLogCsv(MainObj, this);
            if (TabRange.SelectedIndex == 0)
            {
                if (DateTime.Now.Date == dispDataDay.Date)
                {
                    logcsv.ExportCsvDay(PlogData);
                }
                else
                {
                    GPUPowerLog plog = new GPUPowerLog();
                    PowerLogFile plogfile = new PowerLogFile(plog);
                    int res = await plogfile.LoadPowerLogAsync(dispDataDay, true);
                    logcsv.ExportCsvDay(plog);
                }
            }
            else
            {
                if (DateTime.Now.Year == dispDataMonth.Year && DateTime.Now.Month == dispDataMonth.Month)
                {
                    await logcsv.ExportCsvMonth(dispDataMonth, true);
                }
                else
                {
                    await logcsv.ExportCsvMonth(dispDataMonth, false);
                }
            }
        }
    }
}