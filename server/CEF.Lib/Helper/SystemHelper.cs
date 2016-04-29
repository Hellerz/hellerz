using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CEF.Lib.Attributes;
using Fiddler;

namespace CEF.Lib.Helper
{
    public static class SystemHelper
    {
        public static NotifyIcon Notify;

        public static void InitIcon(System.Windows.Application app,Icon icon)
        {
            Notify = new NotifyIcon
            {
                Icon = icon,
                Visible = true
            };
            //Notify.Name = "";

            var menu = new ContextMenu();

            var closeItem = new MenuItem { Text = "Exit" };
            closeItem.Click += (sender, arg) =>
            {
                app.Shutdown();
            };

            var openChromeItem = new MenuItem { Text = "Open in Chrome" };
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
        public static string Version()
        {
            return System.Windows.Forms.Application.ProductVersion;
        }
        [JSchema]
        public static int CompareVersion(string version)
        {
            var webVersion = new Version(version); 
            var serverVersion = new Version(System.Windows.Forms.Application.ProductVersion);
            serverVersion = new Version(serverVersion.Major,serverVersion.Minor,serverVersion.Build);
            return serverVersion.CompareTo(webVersion);
        }

        [JSchema]
        public static void Shutdown()
        {
            System.Windows.Forms.Application.Exit();
            Environment.Exit(0); 
        }
    }
}
