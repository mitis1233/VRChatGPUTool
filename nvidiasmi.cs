using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VRCGPUTool.Form;

namespace VRCGPUTool
{
    partial class NvidiaSmi
    {
        public MainForm MainObj;
        internal BackgroundWorker NvsmiWorker;

        internal string[] queryColumns = {
            "name",
            "uuid",
            "power.limit",
            "power.min_limit",
            "power.max_limit",
            "power.default_limit",
            "utilization.gpu",
            "temperature.gpu",
            "power.draw",
            "clocks.gr",
            "clocks.mem",
        };

        public NvidiaSmi(MainForm Main_Obj)
        {
            MainObj = Main_Obj;
            InitializeNvsmiWorker();
        }

        private void InitializeNvsmiWorker()
        {
            NvsmiWorker = new BackgroundWorker();
            NvsmiWorker.DoWork += new DoWorkEventHandler(NvsmiWorker_DoWork);
            NvsmiWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(NvsmiWorker_RunWorkerCompleted);
        }

        internal string nvidia_smi(string param)
        {
            Console.WriteLine(param);

            ProcessStartInfo nvsmi = new ProcessStartInfo("nvidia-smi.exe");
            nvsmi.WorkingDirectory = @"C:\Windows\system32\";
            nvsmi.Arguments = param;
            nvsmi.Verb = "RunAs";
            nvsmi.CreateNoWindow = true;
            nvsmi.UseShellExecute = false;
            nvsmi.RedirectStandardOutput = true;

            Process p = Process.Start(nvsmi);

            string output = p.StandardOutput.ReadToEnd();

            p.Dispose();

            return output;
        }

        internal void NvsmiWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string query = string.Join(",", queryColumns);

            string output = nvidia_smi(string.Format("--query-gpu={0} --format=csv,noheader,nounits", query));

            e.Result = output;
        }

        private void NvsmiWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MainObj.gpuStatuses.Clear();
                using (var r = new StringReader(e.Result.ToString()))
                {
                    for (string l = r.ReadLine(); l != null; l = r.ReadLine())
                    {
                        string[] v = l.Split(',');
                        if (v.Length != queryColumns.Length) continue;

                        MainObj.gpuStatuses.Add(new GpuStatus(
                            v[0].Trim(),
                            v[1].Trim(),
                            (int)double.Parse(v[2]),
                            (int)double.Parse(v[3]),
                            (int)double.Parse(v[4]),
                            (int)double.Parse(v[5]),
                            (int)double.Parse(v[6]),
                            (int)double.Parse(v[7]),
                            (int)double.Parse(v[8]),
                            (int)double.Parse(v[9]),
                            (int)double.Parse(v[10])
                        ));
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("發生意外的錯誤。 \n強制終止應用程序。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            if (!MainObj.gpuStatuses.Any())
            {
                MessageBox.Show("在我的系統中不再檢測到 NVIDIA GPU。 \n請檢查支持的 GPU 是否被識別。");
                Application.Exit();
            }

            GpuStatus g = MainObj.gpuStatuses.ElementAt(MainObj.GpuIndex.SelectedIndex);
            MainObj.GPUCoreTemp.Text =         "GPU核心溫度: " + g.CoreTemp.ToString() + "℃";
            MainObj.GPUTotalPower.Text =       "GPU總功率: " + g.PowerDraw.ToString() + "W";
            MainObj.GPUCorePLValue.Text =      "GPU核心功率限制: " + g.PLimit.ToString() + "W";
            MainObj.GPUCoreClockValue.Text =   "GPU核心頻率: " + g.CoreClock.ToString() + "MHz";
            MainObj.GPUMemoryClockValue.Text = "VRAM頻率: " + g.MemoryClock.ToString() + "MHz";

            DateTime datetime_now = DateTime.Now;

            MainObj.gpuPlog.PowerLogging(datetime_now, g, MainObj.gpuPlog, MainObj);

            if ((MainObj.PowerLimitValue.Value != g.PLimit) && MainObj.limitstatus && (MainObj.limittime > 2))
            {
                if (datetime_now.Hour == MainObj.BeginTime.Value.Hour && datetime_now.Minute == MainObj.BeginTime.Value.Minute)
                {
                    MainObj.BeginTime.Value = DateTime.Now.AddMinutes(15);
                }
                MainObj.Limit_Action(false, true);
                MessageBox.Show("由於外部工具更改了功率限制值，限制已終止。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void CheckNvidiaSmi()
        {
            if (!File.Exists(@"C:\Windows\system32\nvidia-smi.exe"))
            {
                MessageBox.Show("未找到 nvidia-smi。 \n確保 NVIDIA 顯卡驅動安裝正確", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        internal void InitGPU()
        {
            string query = string.Join(",", queryColumns);
            string res = nvidia_smi(string.Format("--query-gpu={0} --format=csv,noheader,nounits", query));
            try
            {
                using (var r = new StringReader(res))
                {
                    for (string l = r.ReadLine(); l != null; l = r.ReadLine())
                    {
                        string[] v = l.Split(',');
                        if (v.Length != queryColumns.Length) continue;

                        MainObj.gpuStatuses.Add(new GpuStatus(
                            v[0].Trim(),
                            v[1].Trim(),
                            (int)double.Parse(v[2]),
                            (int)double.Parse(v[3]),
                            (int)double.Parse(v[4]),
                            (int)double.Parse(v[5]),
                            (int)double.Parse(v[6]),
                            (int)double.Parse(v[7]),
                            (int)double.Parse(v[8]),
                            (int)double.Parse(v[9]),
                            (int)double.Parse(v[10])
                        ));
                    }
                }
                foreach (GpuStatus g in MainObj.gpuStatuses)
                {
                    MainObj.GpuIndex.Items.Add(g.Name);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("此 GPU 不支持功率上限。 \n終止應用程序。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainObj.Close();
            }

            if (!MainObj.gpuStatuses.Any())
            {
                MessageBox.Show("系統中未檢測到 NVIDIA GPU。 \n請檢查是否安裝了兼容的 GPU。");
                Application.Exit();
            }
        }
    }
}
