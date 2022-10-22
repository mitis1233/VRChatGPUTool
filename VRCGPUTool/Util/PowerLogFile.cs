﻿using System;
using System.Text.Json;
using System.IO;
using System.Windows.Forms;

namespace VRCGPUTool.Util
{
    internal class PowerLogFile
    {
        public PowerLogFile(GPUPowerLog glog)
        {
            gpupowerlog = glog;
        }

        GPUPowerLog gpupowerlog;

        internal void CreatePowerLogFile()
        {
            DateTime dt = DateTime.Now;

            string fName = string.Format("D:/Program Files (x86)/TEMP/powerlog/powerlog_{0:D4}{1:D2}{2:D2}.json", dt.Year, dt.Month, dt.Day);

            try
            {
                GPUPowerLog plog = new GPUPowerLog();

                string logjson = JsonSerializer.Serialize(plog);

                using (StreamWriter sw = new StreamWriter(fName))
                {
                    sw.WriteLine(logjson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("創建電源日誌文件時出錯\n\n{0}", ex.Message.ToString()), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        internal int LoadPowerLog(DateTime dt, bool isHistoryRead)
        {
            string fName = string.Format("D:/Program Files (x86)/TEMP/powerlog/powerlog_{0:D4}{1:D2}{2:D2}.json", dt.Year, dt.Month, dt.Day);

            if (File.Exists(fName))
            {
                using (FileStream fs = File.OpenRead(fName))
                {
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                    {
                        while (!sr.EndOfStream)
                        {
                            gpupowerlog.rawdata = JsonSerializer.Deserialize<GPUPowerLog.RawData>(sr.ReadToEnd());
                        }
                    }
                }
                return 0;
            }
            else
            {
                if (!isHistoryRead)
                {
                    CreatePowerLogFile();
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }

        internal void SavePowerLog(bool isDayBefore)
        {
            DateTime dt = DateTime.Now.Date;

            if (isDayBefore)
            {
                dt = dt.AddDays(-1);
            }

            string fName = string.Format("D:/Program Files (x86)/TEMP/powerlog/powerlog_{0:D4}{1:D2}{2:D2}.json", dt.Year, dt.Month, dt.Day);

            try
            {
                GPUPowerLog plog = new GPUPowerLog();

                string logjson = JsonSerializer.Serialize(gpupowerlog.rawdata);

                using (StreamWriter sw = new StreamWriter(fName))
                {
                    sw.WriteLine(logjson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("更新電源日誌文件時出錯\n\n{0}", ex.Message.ToString()), "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}

