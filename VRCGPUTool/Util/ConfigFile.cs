#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRCGPUTool.Form;

namespace VRCGPUTool.Util
{
    internal class ConfigFile(MainForm main_Obj)
    {
        public MainForm MainObj { get; } = main_Obj;
        private const string ConfigFileName = "config.json";

        private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

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
        }

        private bool isFirstCreate = false;

        private async Task CreateConfigFileAsync()
        {
            MainObj.BeginTime.Value = DateTime.Now.AddMinutes(15);
            MainObj.EndTime.Value = DateTime.Now.AddMinutes(30);

            Config config = new()
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

            try
            {
                string confjson = JsonSerializer.Serialize(config, s_jsonOptions);
                Directory.CreateDirectory(PathUtil.LogDirectory);
                await File.WriteAllTextAsync(ConfigFileName, confjson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"創建配置文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        internal async Task LoadConfigAsync()
        {
            try
            {
                string json = await File.ReadAllTextAsync(ConfigFileName);
                Config? config = JsonSerializer.Deserialize<Config>(json) ?? throw new JsonException("Config file is empty or invalid.");
                MainObj.BeginTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, config.BeginHour, config.BeginMinute, 0);
                MainObj.EndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, config.EndHour, config.EndMinute, 0);

                MainObj.GpuIndex.SelectedIndex = config.SelectGPUIndex < MainObj.GpuIndex.Items.Count ? config.SelectGPUIndex : 0;

                if (!isFirstCreate)
                {
                    MainObj.PowerLimitValue.Value = config.PowerLimitSetting;
                    MainObj.SpecificPLValue.Value = config.UnlimitPLSetting;
                }

                MainObj.ResetGPUDefaultPL.Checked = config.RestoreGPUPLDefault;
                MainObj.SetGPUPLSpecific.Checked = !config.RestoreGPUPLDefault;
                MainObj.SpecificPLValue.Enabled = !config.RestoreGPUPLDefault;

                MainObj.guid = config.Guid;
            }
            catch (FileNotFoundException)
            {
                isFirstCreate = true;
                await CreateConfigFileAsync();
                await LoadConfigAsync();
                return;
            }
            catch (Exception)
            {
                var res = MessageBox.Show("配置文件中有錯誤。\n重新生成配置文件？", "錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(ConfigFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"刪除配置文件失敗\n手動刪除配置文件\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        internal async Task SaveConfigAsync()
        {
            try
            {
                Config config = new()
                {
                    BeginHour = MainObj.BeginTime.Value.Hour,
                    BeginMinute = MainObj.BeginTime.Value.Minute,
                    EndHour = MainObj.EndTime.Value.Hour,
                    EndMinute = MainObj.EndTime.Value.Minute,
                    PowerLimitSetting = (int)MainObj.PowerLimitValue.Value,
                    UnlimitPLSetting = (int)MainObj.SpecificPLValue.Value,
                    RestoreGPUPLDefault = MainObj.ResetGPUDefaultPL.Checked,
                    SelectGPUIndex = MainObj.GpuIndex.SelectedIndex,
                    Guid = MainObj.guid
                };

                string confjson = JsonSerializer.Serialize(config, s_jsonOptions);
                await File.WriteAllTextAsync(ConfigFileName, confjson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新配置文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}
