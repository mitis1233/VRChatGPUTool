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
                ParseGpuData(e.Result.ToString());
            }
            catch (FormatException)
            {
                MessageBox.Show("發生意外的錯誤。 \n強制終止應用程式。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            if (!MainObj.gpuStatuses.Any())
            {
                MessageBox.Show("在我的系統中不再檢測到 NVIDIA GPU。 \n請檢查支持的 GPU 是否被識別。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            GpuStatus g = MainObj.gpuStatuses.ElementAt(MainObj.GpuIndex.SelectedIndex);
            MainObj.UpdateGpuInfoUI(g);

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
                ParseGpuData(res);
                foreach (GpuStatus g in MainObj.gpuStatuses)
                {
                    MainObj.GpuIndex.Items.Add(g.Name);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("此 GPU 不支持功率上限。 \n終止應用程式。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainObj.Close();
            }

            if (!MainObj.gpuStatuses.Any())
            {
                MessageBox.Show("系統中未檢測到 NVIDIA GPU。 \n請檢查是否安裝了兼容的 GPU。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        
        private void ParseGpuData(string data)
        {
            MainObj.gpuStatuses.Clear();
            using (var r = new StringReader(data))
            {
                for (string l = r.ReadLine(); l != null; l = r.ReadLine())
                {
                    string[] v = l.Split(',');
                    if (v.Length != queryColumns.Length) continue;

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
            }
        }
    }
}
