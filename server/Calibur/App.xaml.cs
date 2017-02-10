/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using Calibur.Service;
using CEF.Lib;
using CEF.Lib.Helper;
using Fiddler;

namespace Calibur
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            var isReStart = false;
            if (e.Args.CanBeCount())
            {
                e.Args.ForEachOfUnNone(arg=>
                {
                    if (arg.StartsWith("restart"))
                    {
                        isReStart = true;
                    }
                    if (arg.StartsWith("kill"))
                    {
                        var opt = arg.Split(':');
                        int id;
                        if (opt.Length == 2 && int.TryParse(opt[1],out id))
                        {
                            var processes = Process.GetProcesses();
                            if (processes.Any(process => process.Id == id))
                            {
                                Process.GetProcessById(id).Kill();
                            }
                        }
                    }
                    if (arg.StartsWith("del"))
                    {
                        var opt = arg.Split(':');
                        if (opt.Length == 2)
                        {
                            DirectoryHelper.DeleteRecursive(opt[1]);
                        }
                    }
                });
            }
            #region 判断是否重复运行（重启模式不再判断）

            if (!isReStart)
            {
                var currentProcess = Process.GetCurrentProcess();
                var location = Assembly.GetExecutingAssembly().Location;
                foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (process.Id != currentProcess.Id && location == currentProcess.MainModule.FileName)
                    {
                        MessageBox.Show("Program is running.");
                        SystemHelper.Exit();
                        return;
                    }
                }
            }

            #endregion  

            base.OnStartup(e);
            SystemHelper.InitIcon(this, Calibur.Properties.Resources.calibur);
            FiddlerHelper.Start();
            WebSocketHelper.Start();
            CaliburService.InitialEntry();
            if (!isReStart)
            {
                Utilities.RunExecutable("Chrome", StorageHelper.AchiveValue("pageloading", "http://hellerz.github.io/hellerz/"));
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SystemHelper.RemoveIcon();
            FiddlerHelper.Stop();
            WebSocketHelper.Stop();
        }
    }
}
