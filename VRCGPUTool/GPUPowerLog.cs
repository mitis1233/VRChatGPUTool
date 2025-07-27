using System;
using System.Threading.Tasks;
using VRCGPUTool.Form;
using VRCGPUTool.Util;

namespace VRCGPUTool
{
    public class GPUPowerLog
    {
        public GPUPowerLog()
        {
            rawdata = new RawData();
        }

        internal RawData rawdata;

        internal class RawData
        {
            public int[] HourPowerLog { get; set; } = new int[24];
            public DateTime Logdate { get; set; } = DateTime.Now;
        }

        internal void ClearPowerLog()
        {
            foreach(int i in rawdata.HourPowerLog)
            {
                rawdata.HourPowerLog[i] = 0;
            }
        }

        internal async Task PowerLoggingAsync(DateTime now, GpuStatus g)
        {
            if (now.Hour != rawdata.Logdate.Hour)
            {
                rawdata.Logdate = now;
                for (int i = 0; i < 24; i++)
                {
                    rawdata.HourPowerLog[i] = 0;
                }
            }

            rawdata.HourPowerLog[now.Hour] += g.PowerDraw;

            // 移除即時儲存至硬碟的邏輯，改為在程式關閉時統一儲存
            // 數據將在運行時保存於記憶體中
            await Task.CompletedTask;
        }

        private void AddPowerDeltaData(int hour,int value)
        {
            rawdata.HourPowerLog[hour] += value;
        }
    }
}
