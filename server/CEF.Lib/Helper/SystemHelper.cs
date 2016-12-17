using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CEF.Lib.Attributes;
using Fiddler;
using System.Text.RegularExpressions;
using System.Net;

namespace CEF.Lib.Helper
{
    public static class SystemHelper
    {
        public static System.Windows.Forms.NotifyIcon Notify;

        private static readonly Process CurProcess = Process.GetCurrentProcess();

        private static readonly MethodInfo ExitInternalMethod = typeof(System.Windows.Forms.Application).GetMethod("ExitInternal", BindingFlags.Static | BindingFlags.NonPublic);

        public static void InitIcon(System.Windows.Application app, Icon icon)
        {
            Notify = new System.Windows.Forms.NotifyIcon
            {
                Icon = icon,
                Visible = true
            };
            //Notify.Name = "";

            var menu = new System.Windows.Forms.ContextMenu();

            var closeItem = new System.Windows.Forms.MenuItem { Text = "Exit" };
            closeItem.Click += (sender, arg) =>
            {
                app.Shutdown();
            };

            var openChromeItem = new System.Windows.Forms.MenuItem { Text = "Open in Chrome" };
            openChromeItem.Click += (sender, arg) =>
            {
                Utilities.RunExecutable("Chrome", "http://hellerz.github.io/hellerz/");
            };

            menu.MenuItems.Add(openChromeItem);
            menu.MenuItems.Add(closeItem);

            Notify.ContextMenu = menu;
        }

        public static void RemoveIcon()
        {
            if (Notify != null)
            {
                Notify.Visible = false;
                Notify.Dispose();
            }
        }

        [JSchema]
        public static void UpdateApplication(string filename, string url, string path)
        {
            var guid = Guid.NewGuid().ToString();
            var tmpFilePath = !string.IsNullOrWhiteSpace(path) ? path + guid + @"\" + filename : guid + @"\" + filename;
            var tmpFileFullPath = FileHelper.HttpDownloadFile(url, tmpFilePath);

            var tmpDirectory = Path.GetDirectoryName(tmpFileFullPath);
            var directory = Path.GetDirectoryName(tmpDirectory);

            var batCode = new StringBuilder();
            batCode.AppendFormat("@echo off").AppendLine();
            batCode.AppendFormat("cd \"{0}\"", directory).AppendLine();
            batCode.AppendFormat("CLS").AppendLine();
            batCode.AppendFormat("choice /t 1 /d y /n >nul").AppendLine();
            batCode.AppendFormat("CLS").AppendLine();
            batCode.AppendFormat("taskkill /f /im {0}", filename).AppendLine();
            batCode.AppendFormat("CLS").AppendLine();
            batCode.AppendFormat("move \"{0}\\{1}\" \"{1}\"", guid, filename).AppendLine();
            batCode.AppendFormat("CLS").AppendLine();
            batCode.AppendFormat("start {0} del:{1}", filename, guid).AppendLine();
            batCode.AppendFormat("exit").AppendLine();

            var batPath = tmpDirectory + @"\install.bat";
            File.WriteAllText(batPath, batCode.ToString());
            System.Diagnostics.Process.Start(tmpDirectory + @"\install.bat");
            Shutdown();
        }

        [JSchema]
        public static string Version()
        {
            return System.Windows.Forms.Application.ProductVersion;
        }
        [JSchema]
        public static int CompareVersion(string version)
        {
            var webVersion = new Version(version);
            var serverVersion = new Version(System.Windows.Forms.Application.ProductVersion);
            serverVersion = new Version(serverVersion.Major, serverVersion.Minor, serverVersion.Build);
            return serverVersion.CompareTo(webVersion);
        }


        //private readonly static  PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", CurProcess.ProcessName);
        private readonly static PerformanceCounter WorkingSetCounte = new PerformanceCounter("Process", "Working Set", CurProcess.ProcessName);
        [JSchema]
        public static long GetWorkingSet()
        {
            return (long)WorkingSetCounte.NextValue();
        }

        [JSchema]
        public static void ReStart()
        {
            var strAppFileName = CurProcess.MainModule.FileName;
            var myNewProcess = new Process
            {
                StartInfo =
                {
                    FileName = strAppFileName,
                    Arguments = "restart kill:" + CurProcess.Id,
                    CreateNoWindow = true,
                    WorkingDirectory = System.Windows.Forms.Application.ExecutablePath
                }
            };

            myNewProcess.Start();
            Shutdown();
        }

        [JSchema]
        public static void Shutdown()
        {
            SystemHelper.RemoveIcon();
            FiddlerHelper.Stop();
            WebSocketHelper.Stop();
            Exit();
        }

        public static void Exit()
        {
            
            System.Windows.Forms.Application.Exit();
            Environment.Exit(0);
        }

        [JSchema]
        public static List<string> GetRunResult(string executable, string arg, string grep)
        {
            Regex re = null;
            if (!string.IsNullOrWhiteSpace(grep))
            {
                re = new Regex(grep);
            }
            var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = arg;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            var reader = process.StandardOutput;//截取输出流
            var result = new List<string>();

            var strLine = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (re == null || re.IsMatch(strLine))
                {
                    result.Add(strLine);
                }
                Debug.WriteLine(strLine);
                strLine = reader.ReadLine();
            }
            return result;
        }
        [JSchema]
        public static string GetAddressIP() 
        { 
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    return ip.ToString();
                }
            }
            return "";
        }
    }
}
