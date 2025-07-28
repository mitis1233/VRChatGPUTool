using System;
using System.IO;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace VRCGPUTool
{
    public class GPUPowerLog
    {
        internal int[] currentDayPowerLog = new int[24];
        internal DateTime currentLogDate = DateTime.Now.Date;
        private readonly string dbPath;

        public GPUPowerLog()
        {
            string logDirectory = @"D:\Program Files (x86)\TEMP\powerlog";
            dbPath = Path.Combine(logDirectory, "powerlog.sqlite");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            InitializeDatabase();
            LoadPowerLog(DateTime.Now.Date);
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();
            string sql = "CREATE TABLE IF NOT EXISTS PowerLogs (LogDate TEXT, LogHour INTEGER, PowerDraw INTEGER, PRIMARY KEY(LogDate, LogHour))";
            using var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        internal void LoadPowerLog(DateTime date)
        {
            currentLogDate = date.Date;
            Array.Clear(currentDayPowerLog, 0, currentDayPowerLog.Length);

            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();
            string sql = "SELECT LogHour, PowerDraw FROM PowerLogs WHERE LogDate = @logDate";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@logDate", date.ToString("yyyy-MM-dd"));
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int hour = Convert.ToInt32(reader["LogHour"]);
                int power = Convert.ToInt32(reader["PowerDraw"]);
                if (hour >= 0 && hour < 24)
                {
                    currentDayPowerLog[hour] = power;
                }
            }
        }

        internal void SavePowerLog()
        {
            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();
            using var transaction = connection.BeginTransaction();
            for (int i = 0; i < 24; i++)
            {
                string sql = "INSERT OR REPLACE INTO PowerLogs (LogDate, LogHour, PowerDraw) VALUES (@logDate, @logHour, @powerDraw)";
                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@logDate", currentLogDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@logHour", i);
                command.Parameters.AddWithValue("@powerDraw", currentDayPowerLog[i]);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        internal async Task PowerLoggingAsync(DateTime now, GpuStatus g)
        {
            if (now.Date != currentLogDate)
            {
                SavePowerLog();
                currentLogDate = now.Date;
                Array.Clear(currentDayPowerLog, 0, currentDayPowerLog.Length);
            }

            currentDayPowerLog[now.Hour] += g.PowerDraw;

            await Task.CompletedTask;
        }

        internal string GetDbPath()
        {
            return dbPath;
        }
    }
}

