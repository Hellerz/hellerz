namespace Fiddler
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class RegistryWatcher
    {
        private bool _disposed;
        private ManualResetEvent _eventTerminate = new ManualResetEvent(false);
        private IntPtr _hiveToWatch;
        private object _lockForThread = new object();
        private RegistryEventFilter _regFilter = RegistryEventFilter.Values;
        private string _sSubKey;
        private Thread _threadWaitForChanges;
        private static readonly IntPtr HKEY_CLASSES_ROOT = ((IntPtr) (-2147483648));
        private static readonly IntPtr HKEY_CURRENT_CONFIG = ((IntPtr) (-2147483643));
        private static readonly IntPtr HKEY_CURRENT_USER = ((IntPtr) (-2147483647));
        private static readonly IntPtr HKEY_DYN_DATA = ((IntPtr) (-2147483642));
        private static readonly IntPtr HKEY_LOCAL_MACHINE = ((IntPtr) (-2147483646));
        private static readonly IntPtr HKEY_PERFORMANCE_DATA = ((IntPtr) (-2147483644));
        private static readonly IntPtr HKEY_USERS = ((IntPtr) (-2147483645));
        private const int KEY_NOTIFY = 0x10;
        private const int KEY_QUERY_VALUE = 1;
        public EventHandler KeyChanged;
        private const int STANDARD_RIGHTS_READ = 0x20000;

        

        private RegistryWatcher(RegistryHive registryHive, string subKey)
        {
            this.InitRegistryKey(registryHive, subKey);
        }

        public void Dispose()
        {
            this.Stop();
            this._disposed = true;
            GC.SuppressFinalize(this);
        }

        private void InitRegistryKey(RegistryHive hive, string name)
        {
            switch (hive)
            {
                case RegistryHive.ClassesRoot:
                    this._hiveToWatch = HKEY_CLASSES_ROOT;
                    break;

                case RegistryHive.CurrentUser:
                    this._hiveToWatch = HKEY_CURRENT_USER;
                    break;

                case RegistryHive.LocalMachine:
                    this._hiveToWatch = HKEY_LOCAL_MACHINE;
                    break;

                case RegistryHive.Users:
                    this._hiveToWatch = HKEY_USERS;
                    break;

                case RegistryHive.PerformanceData:
                    this._hiveToWatch = HKEY_PERFORMANCE_DATA;
                    break;

                case RegistryHive.CurrentConfig:
                    this._hiveToWatch = HKEY_CURRENT_CONFIG;
                    break;

                case RegistryHive.DynData:
                    this._hiveToWatch = HKEY_DYN_DATA;
                    break;

                default:
                    throw new InvalidEnumArgumentException("hive", (int) hive, typeof(RegistryHive));
            }
            this._sSubKey = name;
        }

        private void MonitorThread()
        {
            try
            {
                this.WatchAndNotify();
            }
            catch (Exception)
            {
            }
            this._threadWaitForChanges = null;
        }

        protected virtual void OnKeyChanged()
        {
            EventHandler keyChanged = this.KeyChanged;
            if (keyChanged != null)
            {
                keyChanged(this, null);
            }
        }

        [DllImport("advapi32.dll", SetLastError=true)]
        private static extern int RegCloseKey(IntPtr hKey);
        [DllImport("advapi32.dll", SetLastError=true)]
        private static extern int RegNotifyChangeKeyValue(IntPtr hKey, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, RegistryEventFilter dwNotifyFilter, IntPtr hEvent, [MarshalAs(UnmanagedType.Bool)] bool fAsynchronous);
        [DllImport("advapi32.dll", SetLastError=true)]
        private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int options, int samDesired, out IntPtr phkResult);
        private void Start()
        {
            object obj2;
            if (this._disposed)
            {
                throw new ObjectDisposedException(null, "This instance is already disposed");
            }
            bool lockTaken = false;
            try
            {
                Monitor.Enter(obj2 = this._lockForThread, ref lockTaken);
                if (!this.IsWatching)
                {
                    this._eventTerminate.Reset();
                    this._threadWaitForChanges = new Thread(new ThreadStart(this.MonitorThread));
                    this._threadWaitForChanges.IsBackground = true;
                    this._threadWaitForChanges.Start();
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lockForThread);
                }
            }
        }

        public void Stop()
        {
            object obj2;
            if (this._disposed)
            {
                throw new ObjectDisposedException(null, "This instance is already disposed");
            }
            bool lockTaken = false;
            try
            {
                Monitor.Enter(obj2 = this._lockForThread, ref lockTaken);
                Thread thread = this._threadWaitForChanges;
                if (thread != null)
                {
                    this._eventTerminate.Set();
                    thread.Join();
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lockForThread);
                }
            }
        }

        private void WatchAndNotify()
        {
            IntPtr ptr;
            int error = RegOpenKeyEx(this._hiveToWatch, this._sSubKey, 0, 0x20011, out ptr);
            if (error != 0)
            {
                throw new Win32Exception(error);
            }
            try
            {
                AutoResetEvent event2 = new AutoResetEvent(false);
                WaitHandle[] waitHandles = new WaitHandle[] { event2, this._eventTerminate };
                while (!this._eventTerminate.WaitOne(0, true))
                {
                    error = RegNotifyChangeKeyValue(ptr, false, this._regFilter, event2.SafeWaitHandle.DangerousGetHandle(), true);
                    if (error != 0)
                    {
                        throw new Win32Exception(error);
                    }
                    if (WaitHandle.WaitAny(waitHandles) == 0)
                    {
                        this.OnKeyChanged();
                    }
                }
            }
            finally
            {
                if (IntPtr.Zero != ptr)
                {
                    RegCloseKey(ptr);
                }
            }
        }

        internal static RegistryWatcher WatchKey(RegistryHive registryHive, string subKey, EventHandler oToNotify)
        {
            RegistryWatcher watcher = new RegistryWatcher(registryHive, subKey);
            watcher.KeyChanged += oToNotify;
            watcher.Start();
            return watcher;
        }

        public bool IsWatching
        {
            get
            {
                return (null != this._threadWaitForChanges);
            }
        }

        [Flags]
        private enum RegistryEventFilter : int
        {
            ACLs = 8,
            Attributes = 2,
            Key = 1,
            Values = 4
        }
    }
}

