using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRCGPUTool.Form;

namespace VRCGPUTool
{
    internal class NvidiaSmi(MainForm main_Obj)
    {
        public MainForm MainObj = main_Obj;

        internal readonly string[] queryColumns = [
            "name", "uuid", "power.limit", "power.min_limit", "power.max_limit",
            "power.default_limit", "utilization.gpu", "temperature.gpu", "power.draw",
            "clocks.gr", "clocks.mem"
        ];

        internal static async Task<string> NvidiaSmiCommandAsync(string param)
        {
            var processStartInfo = new ProcessStartInfo("nvidia-smi.exe")
            {
                WorkingDirectory = @"C:\Windows\system32\",
                Arguments = param,
                Verb = "RunAs",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null) return string.Empty;
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output;
        }

        internal async Task FetchGpuStatusAsync()
        {
            string query = string.Join(",", queryColumns);
            string output = await NvidiaSmiCommandAsync($"--query-gpu={query} --format=csv,noheader,nounits");

            if (string.IsNullOrWhiteSpace(output)) return;

            ParseGpuData(output);

            if (MainObj.gpuStatuses.Count == 0) return;

            GpuStatus g = MainObj.gpuStatuses[MainObj.GpuIndex.SelectedIndex];
            MainObj.UpdateGpuInfoUI(g);

            await MainObj.gpuPlog.PowerLoggingAsync(DateTime.Now, g);

            if ((MainObj.PowerLimitValue.Value != g.PLimit) && MainObj.limitstatus && (MainObj.limittime > 2))
            {
                await MainObj.Limit_Action(false, true);
                MessageBox.Show("由於外部工具更改了功率限制值，限制已終止。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static void CheckNvidiaSmi()
        {
            if (!File.Exists(@"C:\Windows\system32\nvidia-smi.exe"))
            {
                MessageBox.Show("未找到 nvidia-smi。 \n確保 NVIDIA 顯卡驅動安裝正確", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        internal async Task InitGPUAsync()
        {
            string query = string.Join(",", queryColumns);
            string res = await NvidiaSmiCommandAsync($"--query-gpu={query} --format=csv,noheader,nounits");

            if (string.IsNullOrWhiteSpace(res))
            {
                MessageBox.Show("無法從 nvidia-smi 獲取 GPU 資訊。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            ParseGpuData(res);

            if (MainObj.gpuStatuses.Count == 0)
            {
                MessageBox.Show("系統中未檢測到 NVIDIA GPU。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            MainObj.GpuIndex.Items.Clear();
            foreach (GpuStatus g in MainObj.gpuStatuses)
            {
                MainObj.GpuIndex.Items.Add(g.Name);
            }
            MainObj.GpuIndex.SelectedIndex = 0;
        }

        private void ParseGpuData(string data)
        {
            MainObj.gpuStatuses.Clear();
            var lines = data.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string[] v = line.Split(',');
                if (v.Length != queryColumns.Length) continue;

                try
                {
                    MainObj.gpuStatuses.Add(new GpuStatus(
                        v[0].Trim(),
                        v[1].Trim(),
                        (int)Math.Round(decimal.Parse(v[2])),
                        (int)Math.Round(decimal.Parse(v[3])),
                        (int)Math.Round(decimal.Parse(v[4])),
                        (int)Math.Round(decimal.Parse(v[5])),
                        (int)Math.Round(decimal.Parse(v[6])),
                        (int)Math.Round(decimal.Parse(v[7])),
                        (int)Math.Round(decimal.Parse(v[8])),
                        (int)Math.Round(decimal.Parse(v[9])),
                        (int)Math.Round(decimal.Parse(v[10]))
                    ));
                }
                catch (FormatException)
                {
                    // Skip lines that cannot be parsed
                }
            }
        }
    }
}
