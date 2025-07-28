using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using VRCGPUTool.Form;
using System.Data.SQLite;

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
                DateTime dt = gPLog.currentLogDate;

                sw.WriteLine($"{dt.Year}年{dt.Month}月{dt.Day}日");
                sw.WriteLine($"時,使用量(Wh)");

                for (int i = 0; i < 24; i++)
                {
                    sw.WriteLine($"{i},{(gPLog.currentDayPowerLog[i] / 3600.0):f2}");
                }
            }
        }

        internal async Task ExportCsvMonth(DateTime dt)
        {
            var res = historyForm.saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                string fName = historyForm.saveFileDialog1.FileName;
                using FileStream fs = new(fName, FileMode.Create);
                using StreamWriter sw = new(fs, System.Text.Encoding.Unicode);
                sw.WriteLine($"{dt.Year}年{dt.Month}月");
                sw.WriteLine($"日,時,使用量(Wh)");

                using (var connection = new SQLiteConnection($"Data Source={fm.gpuPlog.GetDbPath()};Version=3;"))
                {
                    connection.Open();
                    string sql = "SELECT SUBSTR(LogDate, 9, 2) as Day, LogHour, PowerDraw FROM PowerLogs WHERE SUBSTR(LogDate, 1, 7) = @month ORDER BY Day, LogHour";
                    using var command = new SQLiteCommand(sql, connection);
                    command.Parameters.AddWithValue("@month", dt.ToString("yyyy-MM"));
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string day = reader["Day"].ToString();
                        string hour = reader["LogHour"].ToString();
                        double powerDraw = Convert.ToDouble(reader["PowerDraw"]) / 3600.0;
                        sw.WriteLine($"{day},{hour},{powerDraw:f2}");
                    }
                }
                await Task.CompletedTask;
            }
        }
    }
}