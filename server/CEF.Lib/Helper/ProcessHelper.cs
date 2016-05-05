using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF.Lib.Helper
{
    /// <summary> 概述：应用程序扩展类。
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary> 概述：表示应用程序重启的参数。
        /// </summary>
        public const string RestartArgument = "RESTARTMYSELF";

        /// <summary> 该函数设置由不同线程产生的窗口的显示状态。 
        /// </summary> 
        /// <param name="hWnd">窗口句柄</param> 
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param> 
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns> 
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary> 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary> 
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param> 
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns> 
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;
        /// <summary> 概述：判断当前应用程序是否唯一实例。倘若不是，显示已存在的实例。
        /// </summary>
        public static bool UniqueInstance() { return UniqueInstance(true); }
        /// <summary> 概述：判断当前应用程序是否唯一实例。并指示当非唯一时，是否显示已存在的实例。
        /// </summary>
        public static bool UniqueInstance(bool showAll)
        {
            var instances = RunningInstances();
            HandleRunningInstance(Process.GetCurrentProcess());
            if (!instances.CanBeCount())
            {
                if (showAll)
                {
                    instances.ForEachOfUnNone(process =>
                    {
                        HandleRunningInstance(process);
                    });
                }
                return false;
            }
            return true;
        }
        /// <summary> 概述：获取正在运行的实例，没有运行的实例返回【null】。
        /// </summary> 
        private static List<Process> RunningInstances()
        {
            Process current = Process.GetCurrentProcess();
            var processList = new List<Process>();
            foreach (Process process in Process.GetProcesses()) //查找相同名称的进程
            {
                if (process.Id != current.Id &&
                    process.ProcessName == current.ProcessName &&
                    process.MainModule.FileName == current.MainModule.FileName)
                {
                    processList.Add(process); ;
                }
            }
            return processList;
        }
        /// <summary> 概述：显示已运行的程序。 
        /// </summary> 
        private static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //显示，可以注释掉 
            SetForegroundWindow(instance.MainWindowHandle);            //放到前端 
        }
        /// <summary> 概述：重启当前应用程序。
        /// </summary>
        public static void RestartApplication()
        {
            // Get the parameters/arguments passed to program if any 
            string arguments = RestartArgument;
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++) // args[0] is always exe path/filename 
                arguments += " " + args[i];
            // Restart current application, with same arguments/parameters 
            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath, arguments);
        }
        /// <summary> 概述：判断当前应用程序是否属于重启状态。
        /// </summary>
        public static bool IsRestart(string[] args)
        {
            return (args != null && args.Length > 0 && args[0] == ProcessHelper.RestartArgument);
        }
    }
}
