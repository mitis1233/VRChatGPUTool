using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using VRCGPUTool.Form;

namespace VRCGPUTool.Util
{
    internal class PowerLogCsv(MainForm mainform, PowerHistory historyForm)
    {
        private readonly MainForm fm = mainform;
        private readonly PowerHistory historyForm = historyForm;

        internal void ExportCsvDay(GPUPowerLog gPLog)
        {
            var res = historyForm.saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                string fName = historyForm.saveFileDialog1.FileName;
                using FileStream fs = new(fName, FileMode.Create);
                using StreamWriter sw = new(fs, System.Text.Encoding.Unicode);
                DateTime dt = gPLog.rawdata.Logdate;

                sw.WriteLine($"{dt.Year}年{dt.Month}月{dt.Day}日");
                sw.WriteLine($"時,使用量(Wh)");

                for (int i = 0; i < 24; i++)
                {
                    sw.WriteLine($"{i},{(gPLog.rawdata.HourPowerLog[i] / 3600.0):f2}");
                }
            }
        }

        internal async Task ExportCsvMonth(DateTime dt, bool isThisMonth)
        {
            var res = historyForm.saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                string fName = historyForm.saveFileDialog1.FileName;
                using FileStream fs = new(fName, FileMode.Create);
                using StreamWriter sw = new(fs, System.Text.Encoding.Unicode);
                sw.WriteLine($"{dt.Year}年{dt.Month}月");
                sw.WriteLine($"日,時,使用量(Wh)");

                if (isThisMonth)
                {
                    for (int i = 1; i < fm.gpuPlog.rawdata.Logdate.Day; i++)
                    {
                        DateTime loadDate = new(fm.gpuPlog.rawdata.Logdate.Year, fm.gpuPlog.rawdata.Logdate.Month, i);
                        GPUPowerLog recentlog = new();
                        PowerLogFile logfile = new(recentlog);
                        int res2 = await logfile.LoadPowerLogAsync(loadDate, true);

                        if (res2 == 0)
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                sw.WriteLine($"{i},{j},{(recentlog.rawdata.HourPowerLog[j] / 3600.0)}");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                sw.WriteLine($"{i},{j},0");
                            }
                        }
                    }
                    for (int i = 0; i < 24; i++)
                    {
                        sw.WriteLine($"{fm.gpuPlog.rawdata.Logdate.Day},{i},{(fm.gpuPlog.rawdata.HourPowerLog[i] / 3600.0)}");
                    }
                }
                else
                {
                    int Days = DateTime.DaysInMonth(dt.Year, dt.Month);

                    for (int i = 1; i <= Days; i++)
                    {
                        DateTime loadDate = new(dt.Year, dt.Month, i);
                        GPUPowerLog recentlog = new();
                        PowerLogFile logfile = new(recentlog);
                        int res2 = await logfile.LoadPowerLogAsync(loadDate, true);

                        if (res2 == 0)
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                sw.WriteLine($"{i},{j},{(recentlog.rawdata.HourPowerLog[j] / 3600.0)}");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                sw.WriteLine($"{i},{j},0");
                            }
                        }
                    }
                }
            }
        }
    }
}