using System;
using System.Collections.Generic;

using System.Text;
using Framework.Feature;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Framework.Legacy;
using FISCA.Presentation;

namespace Framework
{
    public static class Program
    {
        //[MainMethod("K12")]
        //[Dependency("SeniorHSBase")]
        public static void Initial()
        {
            InitializeGlobalData();

            MotherForm.Form.Icon = Properties.Resources.ischoolIcon;
            MotherForm.StartMenu.Image = Properties.Resources.Icon;
            MotherForm.Form.Text = GetTitleText();

            FISCA.Authentication.DSAServices.AutoDisplayLoadingMessageOnMotherForm();

            MotherForm.StartMenu["安全性"]["使用者管理"].Click += new EventHandler(UserManage_Click);
            MotherForm.StartMenu["安全性"]["角色權限管理"].Click += new EventHandler(RoleManage_Click);

            //Student.Instance.ReflashAll();
            //Class.Instance.ReflashAll();
            //Teacher.Instance.ReflashAll();

            //Student.Instance.SetupPresentation();
            //Class.Instance.SetupPresentation();
            //Teacher.Instance.SetupPresentation();
            //Course.Instance.SetupPresentation();
        }

        private static string GetTitleText()
        {
            string schoolName = Framework.Legacy.GlobalOld.SchoolInformation.ChineseName;
            string schoolYear = Framework.Legacy.GlobalOld.SystemConfig.DefaultSchoolYear.ToString();
            string semester = Framework.Legacy.GlobalOld.SystemConfig.DefaultSemester.ToString();
            string server = DSAServices.AccessPoint;
            string user = DSAServices.UserAccount;

            string version = "0.0.0.0";
            try
            {
                string path = Path.Combine(Application.StartupPath, "release.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                version = doc.DocumentElement.GetAttribute("Version");
            }
            catch (Exception) { }

            return string.Format("ischool 〈{0} {1} {2} 〉〈FISCA：{3}〉〈{4}〉〈{5}〉", schoolName, schoolYear, semester, version, server, user);

        }

        #region Start Menu Events

        private static void UserManage_Click(object sender, EventArgs e)
        {
            new Framework.Security.UI.UserManager().ShowDialog();
        }

        private static void RoleManage_Click(object sender, EventArgs e)
        {
            new Framework.Security.UI.RoleManager().ShowDialog();
        }
        #endregion

        private static void InitializeGlobalData()
        {
            Framework.Legacy.GlobalOld.BeginInitialize();

            //設定  Preference。
            IPreferenceProvider preference = new PreferenceProvider();
            MotherForm.PreferenceProvider = preference;
            Framework.Legacy.GlobalOld.Preference = preference;

            Framework.Legacy.GlobalOld.EndInitialize();
        }
    }
}
