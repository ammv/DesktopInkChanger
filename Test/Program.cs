using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        

    static void Main(string[] args)
    {
            IntPtr hWnd = FindWindow(null, "Task Manager");
            if(hWnd == IntPtr.Zero)
            {
                hWnd = FindWindow(null, "Диспетчер задач");
            }
            if (hWnd != IntPtr.Zero) // Проверка наличия окна диспетчера задач
            {
                ShowWindow(hWnd, SW_HIDE); // Скрыть окно диспетчера задач
            }
            Console.WriteLine("Program finished!");
    }


}
}
