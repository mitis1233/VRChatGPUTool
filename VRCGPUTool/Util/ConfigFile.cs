using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using VRCGPUTool.Form;

namespace VRCGPUTool.Util
{
    internal class ConfigFile
    {
        public MainForm MainObj;
        const string ConfigFileName = "config.json";

        public ConfigFile(MainForm Main_Obj)
        {
            MainObj = Main_Obj;
        }

        private class Config
        {
            public string Guid { get; set; } = "";
            public int BeginHour { get; set; } = DateTime.Now.Hour;
            public int BeginMinute { get; set; } = DateTime.Now.Minute;
            public int EndHour { get; set; } = DateTime.Now.Hour;
            public int EndMinute { get; set; } = DateTime.Now.Minute;
            public int PowerLimitSetting { get; set; } = 0;
            public int UnlimitPLSetting { get; set; } = 0;
            public bool RestoreGPUPLDefault { get; set; } = false;
            public int SelectGPUIndex { get; set; } = 0;
            public bool AllowDataProvide { get; set; } = false;
        }

        private bool isFirstCreate = false;

        private void CreateConfigFile()
        {
            var resmsg = MessageBox.Show("感謝您下載“VRChat 的 GPU 功率限制工具”\n。 \n\n你想打開發行說明嗎？", "歡迎", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (resmsg == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo { FileName = "https://github.com/njm2360/VRChatGPUTool#readme", UseShellExecute = true });
            }

            MainObj.BeginTime.Value = DateTime.Now.AddMinutes(15);
            MainObj.EndTime.Value = DateTime.Now.AddMinutes(30);

            Config config = new Config
            {
                Guid = Guid.NewGuid().ToString(),
                BeginHour = MainObj.BeginTime.Value.Hour,
                BeginMinute = MainObj.BeginTime.Value.Minute,
                EndHour = MainObj.EndTime.Value.Hour,
                EndMinute = MainObj.EndTime.Value.Minute,
                PowerLimitSetting = (int)MainObj.PowerLimitValue.Value,
                UnlimitPLSetting = (int)MainObj.SpecificPLValue.Value,
                RestoreGPUPLDefault = true,
                SelectGPUIndex = 0
            };

            resmsg = MessageBox.Show(
                "該工具提高了可用性和、\n" +
                "對於bug等系統報告 \n" +
                "獲取 GPU 使用情況並限制使用情況、\n" +
                "通過發送給開發人員以供將來開發\n" +
                "我們願意幫助您。\n\n" +
                "信息不能識別個人\n" +
                "數據將以下列形式提供\n" +
                "不會提供給\n\n" +
                "您同意提供數據嗎？",
                "【可選】提供使用數據",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button2
            );

            if (resmsg == DialogResult.Yes)
            {
                MessageBox.Show(
                    "感謝您在提供數據方面的合作。\n" +
                    "可以從設置螢幕切換數據提供。",
                    "【可選】提供使用數據",
                    MessageBoxButtons.OK
                );
                config.AllowDataProvide = true;
            }
            else
            {
                MessageBox.Show("我已選擇不提供數據。\n" +
                    "注意、可以從設置屏幕切換數據提供。",
                    "【可選】提供使用數據",
                    MessageBoxButtons.OK
                );
            }

            string confjson = JsonSerializer.Serialize(config);
            try
            {
                Directory.CreateDirectory("./powerlog");
                using (StreamWriter sw = new StreamWriter(ConfigFileName))
                {
                    sw.WriteLine(confjson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"創建配置文件時出錯\n\n{ex.Message.ToString()}",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(-1);
            }
        }


        internal void LoadConfig()
        {
            if (File.Exists(ConfigFileName))
            {
                using (FileStream fs = File.OpenRead(ConfigFileName))
                {
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                    {
                        try
                        {
                            while (!sr.EndOfStream)
                            {
                                Config config = JsonSerializer.Deserialize<Config>(sr.ReadToEnd());
                                MainObj.BeginTime.Value = new DateTime(1970, 1, 1, config.BeginHour, config.BeginMinute, 0);
                                MainObj.EndTime.Value = new DateTime(1970, 1, 1, config.EndHour, config.EndMinute, 0);
                                try
                                {
                                    MainObj.GpuIndex.SelectedIndex = config.SelectGPUIndex;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    MainObj.GpuIndex.SelectedIndex = 0;
                                }

                                if (!isFirstCreate)
                                {
                                    MainObj.PowerLimitValue.Value = config.PowerLimitSetting;
                                    MainObj.SpecificPLValue.Value = config.UnlimitPLSetting;
                                }

                                if (config.RestoreGPUPLDefault == true)
                                {
                                    MainObj.ResetGPUDefaultPL.Checked = true;
                                    MainObj.SpecificPLValue.Enabled = false;
                                }
                                else
                                {
                                    MainObj.SetGPUPLSpecific.Checked = true;
                                    MainObj.SpecificPLValue.Enabled = true;
                                }

                                MainObj.allowDataProvide = config.AllowDataProvide;
                                MainObj.guid = config.Guid;
                            }
                        }
                        catch (Exception)
                        {
                            var res = MessageBox.Show(
                                "配置文件中有錯誤。\n" +
                                "重新生成配置文件？",
                                "錯誤",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error
                            );
                            if (res == DialogResult.Yes)
                            {
                                try
                                {
                                    File.Delete(ConfigFileName);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show(
                                        "刪除配置文件失敗\n" +
                                        "手動刪除配置文件",
                                        "錯誤",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                    Environment.Exit(-1);
                                }
                                Application.Restart();
                            }
                            else
                            {
                                Application.Exit();
                            }
                        }
                    }
                }
            }
            else
            {
                isFirstCreate = true;
                CreateConfigFile();
                LoadConfig();
            }
        }

        internal void SaveConfig()
        {
            try
            {
                Config config = new Config
                {
                    BeginHour = MainObj.BeginTime.Value.Hour,
                    BeginMinute = MainObj.BeginTime.Value.Minute,
                    EndHour = MainObj.EndTime.Value.Hour,
                    EndMinute = MainObj.EndTime.Value.Minute,
                    PowerLimitSetting = (int)MainObj.PowerLimitValue.Value,
                    UnlimitPLSetting = (int)MainObj.SpecificPLValue.Value,
                    RestoreGPUPLDefault = MainObj.ResetGPUDefaultPL.Checked,
                    SelectGPUIndex = MainObj.GpuIndex.SelectedIndex,
                    AllowDataProvide = MainObj.allowDataProvide,
                    Guid = MainObj.guid
                };

                string confjson = JsonSerializer.Serialize(config);

                using (StreamWriter sw = new StreamWriter(ConfigFileName))
                {
                    sw.WriteLine(confjson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"更新配置文件時出錯\n\n{ex.Message.ToString()}",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(-1);
            }
        }
    }
}
