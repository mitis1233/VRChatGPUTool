#nullable enable

using System;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRCGPUTool.Util
{
    internal class PowerLogFile
    {
        private readonly GPUPowerLog gpupowerlog;
        private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

        public PowerLogFile(GPUPowerLog glog)
        {
            gpupowerlog = glog;
            Directory.CreateDirectory(PathUtil.LogDirectory);
        }

        private static string GetLogFilePath(DateTime dt)
        {
            return Path.Combine(PathUtil.LogDirectory, $"powerlog_{dt:yyyyMMdd}.json");
        }

        internal async Task CreatePowerLogFileAsync()
        {
            DateTime dt = DateTime.Now;
            string fName = GetLogFilePath(dt);

            try
            {
                string logjson = JsonSerializer.Serialize(gpupowerlog.rawdata, s_jsonOptions);
                await File.WriteAllTextAsync(fName, logjson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"創建電源日誌文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        internal async Task<int> LoadPowerLogAsync(DateTime dt, bool isHistoryRead)
        {
            string fName = GetLogFilePath(dt);

            if (File.Exists(fName))
            {
                try
                {
                    string json = await File.ReadAllTextAsync(fName);
                    gpupowerlog.rawdata = JsonSerializer.Deserialize<GPUPowerLog.RawData>(json) ?? new GPUPowerLog.RawData();
                    return 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"讀取電源日誌文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }
            else
            {
                if (!isHistoryRead)
                {
                    await CreatePowerLogFileAsync();
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }

        internal async Task SavePowerLogAsync()
        {
            DateTime dt = DateTime.Now.Date;
            string fName = GetLogFilePath(dt);

            try
            {
                string logjson = JsonSerializer.Serialize(gpupowerlog.rawdata, s_jsonOptions);
                await File.WriteAllTextAsync(fName, logjson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新電源日誌文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}

