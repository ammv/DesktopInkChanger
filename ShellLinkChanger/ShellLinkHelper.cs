﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;

namespace ShellLinkChanger
{
    public static class ShellLinkHelper
    {
        private static Shell32.Shell shell = new Shell32.Shell();
        /// <summary>
        /// Меняет путь, аргументы у ярлыка, с сохранением первоначальной иконки
        /// </summary>
        /// <param name="shellLinkFullPath">Полный путь к ярлыку</param>
        /// <param name="newPath">Новый путь, на который должен указывать ярлык</param>
        /// <param name="arguments">Аргументы ярлыка, ставятся после </param>
        public static void ChangeShellLink(string shellLinkFullPath, string newPath, string arguments = null)
        {
            // Load the shortcut.
            Shell32.ShellLinkObject currentLink = GetShellLinkObject(shellLinkFullPath);

            // Сохраняем путь, на который указывал ярлык
            string oldPath = currentLink.Path;
            currentLink.Path = newPath;

            // Указываем рабочую директорию
            currentLink.WorkingDirectory = Path.GetDirectoryName(newPath);

            // Получаем индекс иконки
            int iconIndex = currentLink.GetIconLocation(out string _);

            // Устанавливаем аргументы
            currentLink.Arguments = GetValueNotNull(arguments, currentLink.Arguments);

            // Устанавливаем иконку, так как она сбрасывается после смены пути в ярлыке
            currentLink.SetIconLocation(oldPath, iconIndex);

            // Сохраняем изменения
            currentLink.Save();
        }
        
        private static Shell32.ShellLinkObject GetShellLinkObject(string shortcutFullPath)
        {
            Shell32.Folder folder = shell.NameSpace(Path.GetDirectoryName(shortcutFullPath));
            Shell32.FolderItem folderItem = folder.Items().Item(Path.GetFileName(shortcutFullPath));
            Shell32.ShellLinkObject currentLink = (Shell32.ShellLinkObject)folderItem.GetLink;

            return currentLink;
        }

        /// <summary>
        /// Получает путь, на который указывает ярлык
        /// </summary>
        /// <param name="shortcutFullPath">Полный путь к ярлыку</param>
        /// <returns></returns>
        public static ShellLinkData GetShellLinkPath(string shortcutFullPath)
        {

            var link = GetShellLinkObject(shortcutFullPath);
            return new ShellLinkData(link.Path, link.Arguments);
        }

        /// <summary>
        /// Возвращает newValue если оно не null, иначе oldValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue">Новое значение</param>
        /// <param name="oldValue">Старое значение</param>
        /// <returns></returns>
        public static T GetValueNotNull<T>(T newValue, T oldValue)
        {
            if (newValue != null)
            {
                return newValue;
            }
            return oldValue;
        }

        /// <summary>
        /// Возвращает список файлов - ярлыков из директории
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static List<FileInfo> GetAllShellLinkFiles(string directoryPath)
        {
            //DirectoryInfo info = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            DirectoryInfo info = new DirectoryInfo(directoryPath);
            return info.GetFiles("*.lnk").ToList();
        }
    }
}
