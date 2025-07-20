using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRCGPUTool.Util
{
    public class PowerProfile
    {
        const string ProfileFileName = "profile.json";

        public static readonly int maxPf = 8;

        internal Profile pfData;

        public PowerProfile()
        {
            pfData = new Profile();
        }

        internal class Profile
        {
            public int ProfileCount { get; set; } = 0;
            public int[] SplitTime { get; set; } = new int[maxPf];
            public double[] Unit { get; set; } = new double[maxPf];
        }

        private static async Task CreateProfileFileAsync()
        {
            try
            {
                Profile profile = new();

                string confjson = JsonSerializer.Serialize(profile);

                await File.WriteAllTextAsync(ProfileFileName, confjson);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"創建配置文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        internal static async Task LoadProfileAsync(PowerProfile profile)
        {
            try
            {
                string json = await File.ReadAllTextAsync(ProfileFileName);
                profile.pfData = JsonSerializer.Deserialize<Profile>(json);
            }
            catch (FileNotFoundException)
            {
                await CreateProfileFileAsync();
                await LoadProfileAsync(profile);
            }
            catch (Exception)
            {
                var res = MessageBox.Show("您的個人資料中有錯誤。 \n是否要重新生成配置文件？", "錯誤", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(ProfileFileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("刪除配置文件失敗\n請手動刪除配置文件", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        internal async Task SaveProfileFileAsync()
        {
            try
            {
                string profilejson = JsonSerializer.Serialize(pfData);

                await File.WriteAllTextAsync(ProfileFileName, profilejson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新配置文件時出錯\n\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}