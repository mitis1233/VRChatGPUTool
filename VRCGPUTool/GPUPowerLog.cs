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
        // Store the last compression month to ensure compression happens only once per month
        private string lastCompressionMonth = "";

        public GPUPowerLog()
        {
            string logDirectory = @"D:\Program Files (x86)\TEMP\powerlog";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            dbPath = Path.Combine(logDirectory, "powerlog.db");
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
            string createTableSql = @"
                CREATE TABLE IF NOT EXISTS PowerLogs (
                    LogDate TEXT,
                    LogHour INTEGER,
                    PowerDraw INTEGER,
                    PRIMARY KEY (LogDate, LogHour)
                );
                CREATE INDEX IF NOT EXISTS idx_logdate ON PowerLogs(LogDate);
                CREATE TABLE IF NOT EXISTS DailySummary (
                    LogDate TEXT,
                    TotalPower INTEGER,
                    PRIMARY KEY (LogDate)
                );
                CREATE TABLE IF NOT EXISTS MonthlySummary (
                    LogMonth TEXT,
                    TotalPower INTEGER,
                    PRIMARY KEY (LogMonth)
                );
                CREATE TABLE IF NOT EXISTS HistoricalPowerLogs (
                    LogDate TEXT,
                    TotalPower INTEGER,
                    PRIMARY KEY (LogDate)
                );
                CREATE TABLE IF NOT EXISTS CompressionHistory (
                    Id INTEGER PRIMARY KEY,
                    LastCompressionMonth TEXT
                );
            ";
            using var command = new SQLiteCommand(createTableSql, connection);
            command.ExecuteNonQuery();
            
            // Initialize CompressionHistory with a single row if it doesn't exist
            string initSql = "INSERT OR IGNORE INTO CompressionHistory (Id, LastCompressionMonth) VALUES (1, '')";
            using var initCommand = new SQLiteCommand(initSql, connection);
            initCommand.ExecuteNonQuery();
            
            LoadCompressionHistory(connection);
        }

        private void LoadCompressionHistory(SQLiteConnection connection)
        {
            string sql = "SELECT LastCompressionMonth FROM CompressionHistory WHERE Id = 1";
            using var command = new SQLiteCommand(sql, connection);
            var result = command.ExecuteScalar();
            lastCompressionMonth = result != null ? result.ToString() : "";
        }

        private void SaveCompressionHistory(SQLiteConnection connection)
        {
            string sql = "UPDATE CompressionHistory SET LastCompressionMonth = @lastMonth WHERE Id = 1";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@lastMonth", lastCompressionMonth);
            command.ExecuteNonQuery();
        }

        public void LoadPowerLog(DateTime date)
        {
            currentLogDate = date.Date;
            Array.Clear(currentDayPowerLog, 0, currentDayPowerLog.Length);

            using var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();
            DateTime thresholdDate = DateTime.Now.AddMonths(-3).Date;
            if (date < thresholdDate)
            {
                // Load from HistoricalPowerLogs if data is old
                string sql = "SELECT TotalPower FROM HistoricalPowerLogs WHERE LogDate = @logDate";
                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@logDate", date.ToString("yyyy-MM-dd"));
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    // Cannot break down into hourly data, so just set a placeholder or handle differently
                    // For simplicity, we set all hours to 0 except the first hour as a placeholder
                    currentDayPowerLog[0] = Convert.ToInt32(result);
                }
            }
            else
            {
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
            // Update daily summary
            string dailySql = @"
                INSERT OR REPLACE INTO DailySummary (LogDate, TotalPower)
                SELECT LogDate, SUM(PowerDraw) as TotalPower
                FROM PowerLogs
                WHERE LogDate = @logDate
                GROUP BY LogDate;
            ";
            using var dailyCommand = new SQLiteCommand(dailySql, connection);
            dailyCommand.Parameters.AddWithValue("@logDate", currentLogDate.ToString("yyyy-MM-dd"));
            dailyCommand.ExecuteNonQuery();
            // Update monthly summary
            string monthSql = @"
                INSERT OR REPLACE INTO MonthlySummary (LogMonth, TotalPower)
                SELECT SUBSTR(LogDate, 1, 7) as LogMonth, SUM(PowerDraw) as TotalPower
                FROM PowerLogs
                WHERE LogDate LIKE @logMonth || '%'
                GROUP BY SUBSTR(LogDate, 1, 7);
            ";
            using var monthCommand = new SQLiteCommand(monthSql, connection);
            monthCommand.Parameters.AddWithValue("@logMonth", currentLogDate.ToString("yyyy-MM"));
            monthCommand.ExecuteNonQuery();
            transaction.Commit();
            // Check last compression month from database
            LoadCompressionHistory(connection);
            string currentMonth = currentLogDate.ToString("yyyy-MM");
            // Compress old data only once per month to reduce CPU usage
            if (string.IsNullOrEmpty(lastCompressionMonth) || lastCompressionMonth != currentMonth)
            {
                CompressOldData(connection);
                lastCompressionMonth = currentMonth;
                SaveCompressionHistory(connection);
            }
        }

        private static void CompressOldData(SQLiteConnection connection)
        {
            DateTime thresholdDate = DateTime.Now.AddMonths(-3).Date;
            string thresholdStr = thresholdDate.ToString("yyyy-MM-dd");
            // Aggregate old hourly data into daily totals in HistoricalPowerLogs
            string compressSql = @"
                INSERT OR REPLACE INTO HistoricalPowerLogs (LogDate, TotalPower)
                SELECT LogDate, SUM(PowerDraw) as TotalPower
                FROM PowerLogs
                WHERE LogDate < @threshold
                GROUP BY LogDate;
            ";
            using var compressCommand = new SQLiteCommand(compressSql, connection);
            compressCommand.Parameters.AddWithValue("@threshold", thresholdStr);
            compressCommand.ExecuteNonQuery();
            // Delete old hourly data from PowerLogs
            string deleteSql = "DELETE FROM PowerLogs WHERE LogDate < @threshold";
            using var deleteCommand = new SQLiteCommand(deleteSql, connection);
            deleteCommand.Parameters.AddWithValue("@threshold", thresholdStr);
            deleteCommand.ExecuteNonQuery();
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
