using System;
using System.IO;
using System.Text.Json;
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

        private void CreateProfileFile()
        {
            try
            {
                Profile profile = new Profile();

                string confjson = JsonSerializer.Serialize(profile);

                using (StreamWriter sw = new StreamWriter(ProfileFileName))
                {
                    sw.WriteLine(confjson);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"創建配置文件時出錯\n\n{ex.Message.ToString()}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        internal void LoadProfile(PowerProfile profile)
        {
            if (File.Exists(ProfileFileName))
            {
                using (FileStream fs = File.OpenRead(ProfileFileName))
                {
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                    {
                        try
                        {
                            while (!sr.EndOfStream)
                            {
                                profile.pfData = JsonSerializer.Deserialize<Profile>(sr.ReadToEnd());
                            }
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
                }
            }
            else
            {
                CreateProfileFile();
                LoadProfile(profile);
            }
        }

        internal void SaveProfileFile()
        {
            try
            {
                string profilejson = JsonSerializer.Serialize(pfData);

                using (StreamWriter sw = new StreamWriter(ProfileFileName))
                {
                    sw.WriteLine(profilejson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新配置文件時出錯\n\n{ex.Message.ToString()}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}