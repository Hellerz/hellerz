using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEF.Lib.Attributes;

namespace CEF.Lib.Helper
{
    public static class DirectoryHelper
    {

        //[JSchema]
        //public static string OpenDialog(string title, string rootPath, string selectPath)
        //{
        //    string selectedPath = null;
        //    var thread = new Thread(() =>
        //    {
        //        var dialog = new FolderBrowserDialog
        //        {
        //            Description = title,
        //            ShowNewFolderButton = true,
        //            RootFolder = Environment.SpecialFolder.Desktop
        //        };
        //        if (!string.IsNullOrWhiteSpace(selectPath))
        //        {
        //            dialog.SelectedPath = selectPath;
        //        }
        //        if (dialog.ShowDialog() == DialogResult.OK)
        //        {
        //            selectedPath = dialog.SelectedPath;
        //        }
        //    });
        //    thread.SetApartmentState(ApartmentState.STA);
        //    thread.Start();
        //    thread.Join();
        //    return selectedPath;
        //}

        [JSchema]
        public static string GetBasePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// 
        /// <returns>
        /// The parent directory, or null if <paramref name="path"/> is the root directory, including the root of a UNC server or share name.
        /// </returns>
        /// <param name="path">The path for which to retrieve the parent directory. </param><exception cref="T:System.IO.IOException">The directory specified by <paramref name="path"/> is read-only. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path was not found. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static string GetParent(string path)
        {
            return Directory.GetParent(path).FullName;
        }
        /// <summary>
        /// Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An object that represents the directory for the specified path.
        /// </returns>
        /// <param name="path">The directory path to create. </param><exception cref="T:System.IO.IOException">The directory specified by <paramref name="path"/> is a file.-or-The network name is not known.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or-<paramref name="path"/> is prefixed with, or contains only a colon character (:).</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> contains a colon character (:) that is not part of a drive label ("C:\").</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// 
        /// <returns>
        /// true if <paramref name="path"/> refers to an existing directory; otherwise, false.
        /// </returns>
        /// <param name="path">The path to test. </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// Sets the creation date and time for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information. </param><param name="creationTime">An object that contains the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="creationTime"/> specifies a value outside the range of dates or times permitted for this operation. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetCreationTime(string path, DateTime creationTime)
        {
            Directory.SetCreationTime(path, creationTime);
        }
        /// <summary>
        /// Sets the creation date and time, in Coordinated Universal Time (UTC) format, for the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to set the creation date and time information. </param><param name="creationTimeUtc">An object that  contains the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="creationTime"/> specifies a value outside the range of dates or times permitted for this operation. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(path, creationTimeUtc);
        }
        /// <summary>
        /// Gets the creation date and time of a directory.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The path of the directory. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetCreationTime(string path)
        {
            return Directory.GetCreationTime(path);
        }
        /// <summary>
        /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the creation date and time for the specified directory. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The path of the directory. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetCreationTimeUtc(string path)
        {
            return Directory.GetCreationTimeUtc(path);
        }
        /// <summary>
        /// Sets the date and time a directory was last written to.
        /// </summary>
        /// <param name="path">The path of the directory. </param><param name="lastWriteTime">The date and time the directory was last written to. This value is expressed in local time.  </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastWriteTime"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            Directory.SetLastWriteTime(path, lastWriteTime);
        }
        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that a directory was last written to.
        /// </summary>
        /// <param name="path">The path of the directory. </param><param name="lastWriteTimeUtc">The date and time the directory was last written to. This value is expressed in UTC time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastWriteTimeUtc"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }
        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last written to. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain modification date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastWriteTime(string path)
        {
            return Directory.GetLastWriteTime(path);
        }
        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last written to.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last written to. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain modification date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastWriteTimeUtc(string path)
        {
            return Directory.GetLastWriteTimeUtc(path);
        }
        /// <summary>
        /// Sets the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information. </param><param name="lastAccessTime">An object that contains the value to set for the access date and time of <paramref name="path"/>. This value is expressed in local time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastAccessTime"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(path, lastAccessTime);
        }
        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to set the access date and time information. </param><param name="lastAccessTimeUtc">An object that  contains the value to set for the access date and time of <paramref name="path"/>. This value is expressed in UTC time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastAccessTimeUtc"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }
        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last accessed. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain access date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException">The <paramref name="path"/> parameter is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastAccessTime(string path)
        {
            return Directory.GetLastAccessTime(path);
        }
        /// <summary>
        /// Returns the date and time, in Coordinated Universal Time (UTC) format, that the specified file or directory was last accessed.
        /// </summary>
        /// 
        /// <returns>
        /// A structure that is set to the date and time the specified file or directory was last accessed. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain access date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException">The <paramref name="path"/> parameter is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastAccessTimeUtc(string path)
        {
            return Directory.GetLastAccessTimeUtc(path);
        }

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified directory.
        /// </returns>
        /// <param name="path">The directory from which to retrieve the files. </param><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.-or-A network error has occurred. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetFiles(string path)
        {
            return Directory.GetFiles(path).ToList();
        }
        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified directory that match the specified search pattern.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar"/> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars"/>. </param><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.-or-A network error has occurred. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="searchPattern"/> does not contain a valid pattern. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> or <paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetFilesSearch(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and option.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar"/> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars"/>. </param><param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. -or- <paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> or <paramref name="searchpattern"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.-or-A network error has occurred. </exception>
        [JSchema]
        public static List<string> GetFilesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.GetFiles(path, searchPattern, option).ToList();
            }
            return Directory.GetFiles(path, searchPattern).ToList();    
        }
        /// <summary>
        /// Gets the names of subdirectories (including their paths) in the specified directory.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) of subdirectories in the specified path.
        /// </returns>
        /// <param name="path">The path for which an array of subdirectory names is returned. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetDirectories(string path)
        {
            return Directory.GetDirectories(path).ToList();
        }
        /// <summary>
        /// Gets the names of subdirectories (including their paths) that match the specified search pattern in the current directory.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match the search pattern.
        /// </returns>
        /// <param name="path">The path to search. </param><param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar"/> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars"/>. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="searchPattern"/> does not contain a valid pattern. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> or <paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetDirectoriesSearch(string path, string searchPattern)
        {
            return Directory.GetDirectories(path, searchPattern).ToList();
        }
        /// <summary>
        /// Gets the names of the subdirectories (including their paths) that match the specified search pattern in the current directory, and optionally searches subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the full names (including paths) of the subdirectories that match the search pattern.
        /// </returns>
        /// <param name="path">The path to search. </param><param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. The parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar"/> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars"/>. </param><param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="searchPattern"/> does not contain a valid pattern. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> or <paramref name="searchPattern"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception>
        [JSchema]
        public static List<string> GetDirectoriesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.GetDirectories(path, searchPattern, option).ToList();
            }
            return Directory.GetDirectories(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns the names of all files and subdirectories in the specified directory.
        /// </summary>
        /// 
        /// <returns>
        /// An array of the names of files and subdirectories in the specified directory.
        /// </returns>
        /// <param name="path">The directory for which file and subdirectory names are returned. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetFileSystemEntries(string path)
        {
            return Directory.GetFileSystemEntries(path).ToList();
        }
        /// <summary>
        /// Returns an array of file system entries that match the specified search criteria.
        /// </summary>
        /// 
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        /// <param name="path">The path to be searched. </param><param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. The <paramref name="searchPattern"/> parameter cannot end in two periods ("..") or contain two periods ("..") followed by <see cref="F:System.IO.Path.DirectorySeparatorChar"/> or <see cref="F:System.IO.Path.AltDirectorySeparatorChar"/>, nor can it contain any of the characters in <see cref="F:System.IO.Path.InvalidPathChars"/>. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="searchPattern"/> does not contain a valid pattern. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> or <paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> GetFileSystemEntriesSearch(string path, string searchPattern)
        {
            return Directory.GetFileSystemEntries(path, searchPattern).ToList();
        }
        /// <summary>
        /// Gets an array of all the file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An array of file system entries that match the specified search criteria.
        /// </returns>
        /// <param name="path">The directory to search.</param><param name="searchPattern">The string used to search for all files or directories that match its search pattern. The default pattern is for all files and directories: "*"</param><param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly"/>.</param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> GetFileSystemEntriesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.GetFileSystemEntries(path, searchPattern, option).ToList();
            }
            return Directory.GetFileSystemEntries(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of directory names in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/>.
        /// </returns>
        /// <param name="path">The directory to search.</param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified search pattern.
        /// </returns>
        /// <param name="path">The directory to search.</param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateDirectoriesSearch(string path, string searchPattern)
        {
            return Directory.EnumerateDirectories(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified search pattern and option.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><param name="searchOption">One of the enumeration values  that specifies whether the search operation should include only the current directory or should include all subdirectories.The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly"/>.</param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateDirectoriesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.EnumerateDirectories(path, searchPattern, option).ToList();
            }
            return Directory.EnumerateDirectories(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file names in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/>.
        /// </returns>
        /// <param name="path">The directory to search. </param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified search pattern.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFilesSearch(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified search pattern and option.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly"/>.</param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFilesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.EnumerateFiles(path, searchPattern, option).ToList();
            }
            return Directory.EnumerateFiles(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file-system entries in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/>.
        /// </returns>
        /// <param name="path">The directory to search. </param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file-system entries that match a search pattern in a specified path.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified search pattern.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFileSystemEntriesSearch(string path, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern).ToList();
        }
        /// <summary>
        /// Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerable collection of file-system entries in the directory specified by <paramref name="path"/> and that match the specified search pattern and option.
        /// </returns>
        /// <param name="path">The directory to search. </param><param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.  </param><param name="searchOption">One of the enumeration values  that specifies whether the search operation should include only the current directory or should include all subdirectories.The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly"/>.</param><exception cref="T:System.ArgumentException"><paramref name="path "/>is a zero-length string, contains only white space, or contains invalid characters as defined by <see cref="M:System.IO.Path.GetInvalidPathChars"/>.- or -<paramref name="searchPattern"/> does not contain a valid pattern.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.-or-<paramref name="searchPattern"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="searchOption"/> is not a valid <see cref="T:System.IO.SearchOption"/> value.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid, such as referring to an unmapped drive. </exception><exception cref="T:System.IO.IOException"><paramref name="path"/> is a file name.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or combined exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> EnumerateFileSystemEntriesOption(string path, string searchPattern, string searchOption)
        {
            SearchOption option;
            if (Enum.TryParse(searchOption, out option))
            {
                return Directory.EnumerateFileSystemEntries(path, searchPattern, option).ToList();
            }
            return Directory.EnumerateFileSystemEntries(path, searchPattern).ToList();
        }

        /// <summary>
        /// Retrieves the names of the logical drives on this computer in the form "&lt;drive letter&gt;:\".
        /// </summary>
        /// 
        /// <returns>
        /// The logical drives on this computer.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occured (for example, a disk error). </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>

        public static List<string> GetLogicalDrives()
        {
            return Directory.GetLogicalDrives().ToList();
        }

        /// <summary>
        /// Returns the volume information, root information, or both for the specified path.
        /// </summary>
        /// 
        /// <returns>
        /// A string that contains the volume information, root information, or both for the specified path.
        /// </returns>
        /// <param name="path">The path of a file or directory. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static string GetDirectoryRoot(string path)
        {
            return Directory.GetDirectoryRoot(path);
        }
        /// <summary>
        /// Gets the current working directory of the application.
        /// </summary>
        /// 
        /// <returns>
        /// A string that contains the path of the current working directory, and does not end with a backslash (\).
        /// </returns>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException">The operating system is Windows CE, which does not have current directory functionality.This method is available in the .NET Compact Framework, but is not currently supported.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
        /// <summary>
        /// Sets the application's current working directory to the specified directory.
        /// </summary>
        /// <param name="path">The path to which the current working directory is set. </param><exception cref="T:System.IO.IOException">An I/O error occurred. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission to access unmanaged code. </exception><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified directory was not found.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>

        [JSchema]
        public static void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }
        /// <summary>
        /// Moves a file or a directory and its contents to a new location.
        /// </summary>
        /// <param name="sourceDirName">The path of the file or directory to move. </param><param name="destDirName">The path to the new location for <paramref name="sourceDirName"/>. If <paramref name="sourceDirName"/> is a file, then <paramref name="destDirName"/> must also be a file name.</param><exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume. -or- <paramref name="destDirName"/> already exists. -or- The <paramref name="sourceDirName"/> and <paramref name="destDirName"/> parameters refer to the same file or directory. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="sourceDirName"/> or <paramref name="destDirName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="sourceDirName"/> or <paramref name="destDirName"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The path specified by <paramref name="sourceDirName"/> is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }
        /// <summary>
        /// Deletes an empty directory from a specified path.
        /// </summary>
        /// <param name="path">The name of the empty directory to remove. This directory must be writable or empty. </param><exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path"/> exists.-or-The directory is the application's current working directory.-or-The directory specified by <paramref name="path"/> is not empty.-or-The directory is read-only or contains a read-only file.-or-The directory is being used by another process.-or-There is an open handle on the directory, and the operating system is Windows XP or earlier. This open handle can result from directories. For more information, see How to: Enumerate Directories and Files.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> does not exist or could not be found.-or-<paramref name="path"/> refers to a file instead of a directory.-or-The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void Delete(string path)
        {
            Directory.Delete(path);
        }

        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
        /// </summary>
        /// <param name="path">The name of the directory to remove. </param>
        /// <exception cref="T:System.IO.IOException">A file with the same name and location specified by <paramref name="path"/> exists.-or-The directory specified by <paramref name="path"/> is read-only, or <paramref name="recursive"/> is false and <paramref name="path"/> is not an empty directory. -or-The directory is the application's current working directory. -or-The directory contains a read-only file.-or-The directory is being used by another process.There is an open handle on the directory or on one of its files, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> does not exist or could not be found.-or-<paramref name="path"/> refers to a file instead of a directory.-or-The specified path is invalid (for example, it is on an unmapped drive). </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void DeleteRecursive(string path)
        {
            Directory.Delete(path, true);
        }
    }
}
