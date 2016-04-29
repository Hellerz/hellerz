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
using System.Threading;
using System.Windows;
using Calibur.Service;
using CEF.Lib.Helper;

namespace Calibur
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            #region 判断是否重复运行
            bool createdNew;
            var mutex = new Mutex(false, "Calibur", out createdNew);

            if (!createdNew)
            {
                Process currentProcess = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if ((process.Id != currentProcess.Id) &&
                        (Assembly.GetExecutingAssembly().Location == currentProcess.MainModule.FileName))
                    {
                        if (process != null)
                        {
                            MessageBox.Show("Program is running.");
                            System.Windows.Forms.Application.Exit();
                            return;
                        }
                    }
                }
            } 
            #endregion

            base.OnStartup(e);
            SystemHelper.InitIcon(this, Calibur.Properties.Resources.calibur);
            FiddlerHelper.Start();
            WebSocketHelper.Start();
            WebSocketEntry.Start();
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
