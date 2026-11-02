using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 啟動流程計時記錄，寫入 %LocalAppData%\ischool\Logs\startup_yyyyMMdd.log。
    /// 任何寫入失敗都不會影響啟動。
    /// </summary>
    internal static class StartupLog
    {
        private const int KeepDays = 7;

        private static readonly object _lock = new object();
        private static string _dir;

        private static string Dir
        {
            get
            {
                if (_dir == null)
                    _dir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"ischool\Logs");
                return _dir;
            }
        }

        /// <summary>
        /// 執行指定動作並記錄耗時，例外會照常往外丟（並記錄 FAIL）。
        /// </summary>
        internal static void Time(string step, Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                action();
                Write(step, sw.ElapsedMilliseconds, null);
            }
            catch (Exception ex)
            {
                Write(step, sw.ElapsedMilliseconds, ex);
                throw;
            }
        }

        /// <summary>
        /// 執行指定函式並記錄耗時，回傳函式結果；例外會照常往外丟（並記錄 FAIL）。
        /// </summary>
        internal static T Time<T>(string step, Func<T> func)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                T result = func();
                Write(step, sw.ElapsedMilliseconds, null);
                return result;
            }
            catch (Exception ex)
            {
                Write(step, sw.ElapsedMilliseconds, ex);
                throw;
            }
        }

        /// <summary>
        /// 寫入一行記錄。格式：時間 [Tab] 步驟 [Tab] 耗時 [Tab] 執行緒 [Tab] 帳號 [Tab] 結果
        /// </summary>
        internal static void Write(string step, long elapsedMs, Exception error)
        {
            try
            {
                string result = error == null ? "OK" : "FAIL " + error.GetType().Name + ": " + OneLine(error.Message);
                string line = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}\t{1}\t{2}ms\tT{3}\t{4}\t{5}\r\n",
                    DateTime.Now, step, elapsedMs, Thread.CurrentThread.ManagedThreadId, GetUser(), result);

                lock (_lock)
                {
                    Directory.CreateDirectory(Dir);
                    File.AppendAllText(Path.Combine(Dir, "startup_" + DateTime.Now.ToString("yyyyMMdd") + ".log"), line);
                }
            }
            catch { } // 寫 log 失敗不影響啟動
        }

        /// <summary>
        /// 刪除超過保留天數的記錄檔。
        /// </summary>
        internal static void Cleanup()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Dir);
                if (!dir.Exists) return;

                DateTime limit = DateTime.Now.AddDays(-KeepDays);
                foreach (FileInfo f in dir.GetFiles("startup_*.log"))
                    if (f.LastWriteTime < limit) f.Delete();
            }
            catch { }
        }

        private static string GetUser()
        {
            try { return DSAServices.UserAccount; }
            catch { return "?"; }
        }

        private static string OneLine(string s)
        {
            return s == null ? string.Empty : s.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        }
    }
}
