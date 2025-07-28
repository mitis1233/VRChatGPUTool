using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace VRCGPUTool
{
    public class PowerLogData
    {
        public int[] HourPowerLog { get; set; } = new int[24];
        public DateTime LogDate { get; set; } = DateTime.Now.Date;
    }

    public class GPUPowerLog
    {
        internal PowerLogData powerLogData;
        private readonly string logDirectory = @"D:\Program Files (x86)\TEMP\powerlog";
        private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = false };

        public GPUPowerLog()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            LoadPowerLog(DateTime.Now);
        }

        private string GetLogFilePath(DateTime date)
        {
            return Path.Combine(logDirectory, $"powerlog_{date:yyyyMMdd}.json");
        }

        internal void LoadPowerLog(DateTime date)
        {
            string filePath = GetLogFilePath(date);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                powerLogData = JsonSerializer.Deserialize<PowerLogData>(json);
            }
            else
            {
                powerLogData = new PowerLogData
                {
                    LogDate = date.Date
                };
            }
        }

        internal void SavePowerLog()
        {
            string filePath = GetLogFilePath(powerLogData.LogDate);
            string json = JsonSerializer.Serialize(powerLogData, jsonOptions);
            File.WriteAllText(filePath, json);
        }

        internal async Task PowerLoggingAsync(DateTime now, GpuStatus g)
        {
            if (now.Date != powerLogData.LogDate)
            {
                SavePowerLog();
                powerLogData = new PowerLogData
                {
                    LogDate = now.Date
                };
            }

            powerLogData.HourPowerLog[now.Hour] += g.PowerDraw;

            await Task.CompletedTask;
        }
    }
}

