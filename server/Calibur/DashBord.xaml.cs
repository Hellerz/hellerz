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
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CEF.Lib;
using CEF.Lib.Helper;
using CEF.Lib.JavascriptObject;
using Fiddler;
using Newtonsoft.Json;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Calibur
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DashBord
    {
        public DashBord()
        {
            InitializeComponent();
            this.WindowState = WindowState.Minimized;
            
            Initial();
            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;
        }

        private void Initial()
        {
            AttachEventListeners();
        }

        private void AttachEventListeners()
        {
            
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
          var serializer = new XmlLayoutSerializer( dockManager );
            serializer.LayoutSerializationCallback += (s, args) =>
            {
                args.Content = args.Content;
            };

            if (File.Exists(@".\AvalonDock.config"))
            {
                serializer.Deserialize(@".\AvalonDock.config");
            }
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
          var serializer = new XmlLayoutSerializer( dockManager );
          serializer.Serialize(@".\AvalonDock.config");
        }

        #region LoadLayoutCommand
        RelayCommand _loadLayoutCommand = null;
        public ICommand LoadLayoutCommand
        {
            get
            {
                if (_loadLayoutCommand == null)
                {
                    _loadLayoutCommand = new RelayCommand(OnLoadLayout, CanLoadLayout);
                }

                return _loadLayoutCommand;
            }
        }

        private bool CanLoadLayout(object parameter)
        {
            return File.Exists(@".\AvalonDock.Layout.config");
        }

        private void OnLoadLayout(object parameter)
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            //Here I've implemented the LayoutSerializationCallback just to show
            // a way to feed layout desarialization with content loaded at runtime
            //Actually I could in this case let AvalonDock to attach the contents
            //from current layout using the content ids
            //LayoutSerializationCallback should anyway be handled to attach contents
            //not currently loaded
            layoutSerializer.LayoutSerializationCallback += (s, e) =>
                {
                    //if (e.Model.ContentId == FileStatsViewModel.ToolContentId)
                    //    e.Content = Workspace.This.FileStats;
                    //else if (!string.IsNullOrWhiteSpace(e.Model.ContentId) &&
                    //    File.Exists(e.Model.ContentId))
                    //    e.Content = Workspace.This.Open(e.Model.ContentId);
                };
            layoutSerializer.Deserialize(@".\AvalonDock.Layout.config");
        }

        #endregion 

        #region SaveLayoutCommand
        RelayCommand _saveLayoutCommand = null;
        public ICommand SaveLayoutCommand
        {
            get
            {
                if (_saveLayoutCommand == null)
                {
                    _saveLayoutCommand = new RelayCommand(OnSaveLayout, CanSaveLayout);
                }

                return _saveLayoutCommand;
            }
        }

        private bool CanSaveLayout(object parameter)
        {
            return true;
        }

        private void OnSaveLayout(object parameter)
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.Serialize(@".\AvalonDock.Layout.config");
        }

        #endregion 

        private void OnDumpToConsole(object sender, RoutedEventArgs e)
        {
            OpenDialog("Gi开源程序托管集中地了，连PHP的源码都在GitHub上面托管了", @"D:\git\Hellerz\Hellerz\server\Calibur\bin\Debug");
        }

        public static string OpenDialog(string title, string selectPath)
        {
            string selectedPath = null;
            var thread = new Thread(() =>
            {
                var dialog = new FolderBrowserDialog
                {
                    Description = title,
                    ShowNewFolderButton = true,
                    RootFolder = Environment.SpecialFolder.Desktop
                };
                if (!string.IsNullOrWhiteSpace(selectPath))
                {
                    dialog.SelectedPath = selectPath;
                }
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = dialog.SelectedPath;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return selectedPath;
        }


    }
}
