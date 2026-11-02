using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Security.Cryptography;
using FISCA.DSAUtil;
using System.Xml;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using DataSynchronization;

namespace Framework
{
    public static class DSAServices
    {
        private const string NotSupportedMessage =
            "目前的 FISCA.Authentication 已不提供此方法，授權與登入由 FISCA 主程式負責。";

        /// <summary>
        /// 授權登入系統（已不支援）。
        /// </summary>
        /// <param name="fileName">授權檔路徑</param>
        /// <exception cref="NotSupportedException">一律丟出。</exception>
        [Obsolete(NotSupportedMessage)]
        public static void SetLicense(string fileName)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }
        /// <summary>
        /// 設定授權登入系統（已不支援）。
        /// </summary>
        /// <param name="stream">授權檔串流</param>
        /// <exception cref="NotSupportedException">一律丟出。</exception>
        [Obsolete(NotSupportedMessage)]
        public static void SetLicense(Stream stream)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }
        /// <summary>
        /// 登入使用者（已不支援）。
        /// 原本登入後會呼叫 <see cref="Setup"/>；若需初始化組態，請在登入完成後直接呼叫 <see cref="Setup"/>。
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <exception cref="NotSupportedException">一律丟出。</exception>
        [Obsolete(NotSupportedMessage)]
        public static void Login(string account, string password)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        public static void Setup()
        {

            Framework.BugReporter.SetRunTimeMessage("LoginUser", FISCA.Authentication.DSAServices.UserAccount);
            //IsLogined = true;
            //UserAccount = account;

            //負責儲存使用者個人設定。
            User.Configuration = new User.UserConfigManager(new ConfigProvider_User(), FISCA.Authentication.DSAServices.UserAccount);
            //負責儲存應用程式設定。
            App.Configuration = new ConfigurationManager(new ConfigProvider_App());
            //負責儲存全域設定。
            Global.Configuration = new Global.GlobalConfigManager(new ConfigProvider_Global(), true);
            //可讀寫的全域組態。
            Global.ConfigurationWritable = new Global.GlobalConfigManager(new ConfigProvider_Global(), false);
            //負責資料庫變動的通知物件。
            App.DBMonitor = new DBChangeMonitor(10, new PTChangeSetProvider());
        }
        /// <summary>
        /// 取得登入帳號是否為系統管理員帳號
        /// </summary>
        public static bool IsSysAdmin
        {
            get { return FISCA.Authentication.DSAServices.IsSysAdmin; }
        }
        /// <summary>
        /// 使用者已經成功登入系統
        /// </summary>
        public static bool IsLogined { get { return FISCA.Authentication.DSAServices.IsLogined; } }
        /// <summary>
        /// 呼叫DSAService
        /// </summary>
        /// <param name="service">ServiceName</param>
        /// <param name="req">DSRequest</param>
        /// <returns>DSResponse</returns>
        public static DSResponse CallService(string service, DSRequest req)
        {
            return FISCA.Authentication.DSAServices.CallService(service, req);
        }
        /// <summary>
        /// 登入帳號
        /// </summary>
        public static string UserAccount { get { return FISCA.Authentication.DSAServices.UserAccount; } }

        /// <summary>
        /// 登入的主機名稱。
        /// </summary>
        public static string AccessPoint { get { return FISCA.Authentication.DSAServices.AccessPoint; } }
    }

    /// <summary>
    /// 讓大家一起用吧。
    /// </summary>
    public static class PasswordHash
    {
        public static string Compute(string password)
        {
            return FISCA.Authentication.DSAServices.ComputePasswordHash(password);
        }
    }
}
