using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEF.Lib.Attributes;
using Microsoft.Win32;

namespace CEF.Lib.Helper
{
    public static class FileHelper
    {
        private static Encoding GetEncoding(string encode)
        {
            if (string.IsNullOrWhiteSpace(encode)) return Encoding.Default;
            switch (encode.ToUpper())
            {
                case "ASCII":
                    return Encoding.ASCII;
                case "Unicode":
                    return Encoding.Unicode;
                case "BIGENDIANUNICODE":
                    return Encoding.BigEndianUnicode;
                case "UTF7":
                    return Encoding.UTF7;
                case "UTF8":
                    return Encoding.UTF8;
                case "UTF32":
                    return Encoding.UTF32;
                case "DEFAULT":
                default:
                    return Encoding.Default;
            }
        }
        
        [JSchema]
        public static List<string> OpenDialog(string title, string path, string filter, int filterIndex)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                ShowReadOnly = true,
                ReadOnlyChecked = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = title,
                InitialDirectory = path,
                Filter = filter,
                FilterIndex = filterIndex
            };
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileNames.ToList();
            }
            return null;
        }

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is not allowed.
        /// </summary>
        /// <param name="sourceFileName">The file to copy. </param>
        /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file. </param>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="sourceFileName"/> or <paramref name="destFileName"/> specifies a directory. </exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is null. </exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName"/> or <paramref name="destFileName"/> is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="T:System.IO.FileNotFoundException"><paramref name="sourceFileName"/> was not found. </exception>
        /// <exception cref="T:System.IO.IOException"><paramref name="destFileName"/> exists.-or- An I/O error has occurred. </exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is in an invalid format. </exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="sourceFileName">The file to copy. </param><param name="destFileName">The name of the destination file. This cannot be a directory. </param>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. -or-<paramref name="destFileName"/> is read-only.</exception><exception cref="T:System.ArgumentException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.-or- <paramref name="sourceFileName"/> or <paramref name="destFileName"/> specifies a directory. </exception><exception cref="T:System.ArgumentNullException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName"/> or <paramref name="destFileName"/> is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.FileNotFoundException"><paramref name="sourceFileName"/> was not found. </exception><exception cref="T:System.IO.IOException"><paramref name="destFileName"/> exists and <paramref name="overwrite"/> is false.-or- An I/O error has occurred. </exception><exception cref="T:System.NotSupportedException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void CopyWithWrite(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">The specified file is in use. -or-There is an open handle on the file, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.-or- <paramref name="path"/> is a directory.-or- <paramref name="path"/> specified a read-only file. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Delete(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Decrypts a file that was encrypted by the current account using the <see cref="M:System.IO.File.Encrypt(System.String)"/> method.
        /// </summary>
        /// <param name="path">A path that describes a file to decrypt.</param><exception cref="T:System.ArgumentException">The <paramref name="path"/> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.</exception><exception cref="T:System.ArgumentNullException">The <paramref name="path"/> parameter is null.</exception><exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception><exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path"/> parameter could not be found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. For example, the encrypted file is already open. -or-This operation is not supported on the current platform.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception><exception cref="T:System.UnauthorizedAccessException">The <paramref name="path"/> parameter specified a file that is read-only.-or- This operation is not supported on the current platform.-or- The <paramref name="path"/> parameter specified a directory.-or- The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Decrypt(string path)
        {
            File.Decrypt(path);
        }

        /// <summary>
        /// Encrypts a file so that only the account used to encrypt the file can decrypt it.
        /// </summary>
        /// <param name="path">A path that describes a file to encrypt.</param><exception cref="T:System.ArgumentException">The <paramref name="path"/> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.</exception><exception cref="T:System.ArgumentNullException">The <paramref name="path"/> parameter is null.</exception><exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception><exception cref="T:System.IO.FileNotFoundException">The file described by the <paramref name="path"/> parameter could not be found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.-or-This operation is not supported on the current platform.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.PlatformNotSupportedException">The current operating system is not Windows NT or later.</exception><exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception><exception cref="T:System.UnauthorizedAccessException">The <paramref name="path"/> parameter specified a file that is read-only.-or- This operation is not supported on the current platform.-or- The <paramref name="path"/> parameter specified a directory.-or- The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Encrypt(string path)
        {
            File.Encrypt(path);
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// 
        /// <returns>
        /// true if the caller has the required permissions and <paramref name="path"/> contains the name of an existing file; otherwise, false. This method also returns false if <paramref name="path"/> is null, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns false regardless of the existence of <paramref name="path"/>.
        /// </returns>
        /// <param name="path">The file to check. </param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Sets the date and time the file was created.
        /// </summary>
        /// <param name="path">The file for which to set the creation date and time information. </param><param name="creationTime">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in local time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="creationTime"/> specifies a value outside the range of dates, times, or both permitted for this operation. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetCreationTime(string path, DateTime creationTime)
        {
            File.SetCreationTime(path, creationTime);
        }

        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the file was created.
        /// </summary>
        /// <param name="path">The file for which to set the creation date and time information. </param><param name="creationTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the creation date and time of <paramref name="path"/>. This value is expressed in UTC time. </param><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.IOException">An I/O error occurred while performing the operation. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="creationTime"/> specifies a value outside the range of dates, times, or both permitted for this operation. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            File.SetCreationTimeUtc(path, creationTimeUtc);
        }

        /// <summary>
        /// Returns the creation date and time of the specified file or directory.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

        /// <summary>
        /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetCreationTimeUtc(string path)
        {
            return File.GetCreationTimeUtc(path);
        }

        /// <summary>
        /// Sets the date and time the specified file was last accessed.
        /// </summary>
        /// <param name="path">The file for which to set the access date and time information. </param><param name="lastAccessTime">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in local time. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastAccessTime"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            File.SetLastAccessTime(path, lastAccessTime);
        }

        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.
        /// </summary>
        /// <param name="path">The file for which to set the access date and time information. </param><param name="lastAccessTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last access date and time of <paramref name="path"/>. This value is expressed in UTC time. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastAccessTimeUtc"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain access date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain access date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastAccessTimeUtc(string path)
        {
            return File.GetLastAccessTimeUtc(path);
        }

        /// <summary>
        /// Sets the date and time that the specified file was last written to.
        /// </summary>
        /// <param name="path">The file for which to set the date and time information. </param><param name="lastWriteTime">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in local time. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastWriteTime"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            File.SetLastWriteTime(path, lastWriteTime);
        }

        /// <summary>
        /// Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.
        /// </summary>
        /// <param name="path">The file for which to set the date and time information. </param><param name="lastWriteTimeUtc">A <see cref="T:System.DateTime"/> containing the value to set for the last write date and time of <paramref name="path"/>. This value is expressed in UTC time. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.FileNotFoundException">The specified path was not found. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lastWriteTimeUtc"/> specifies a value outside the range of dates or times permitted for this operation.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain write date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in UTC time.
        /// </returns>
        /// <param name="path">The file or directory for which to obtain write date and time information. </param><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static DateTime GetLastWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

        /// <summary>
        /// Gets the <see cref="T:System.IO.FileAttributes"/> of the file on the path.
        /// </summary>
        /// 
        /// <returns>
        /// The <see cref="T:System.IO.FileAttributes"/> of the file on the path.
        /// </returns>
        /// <param name="path">The path to the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.IO.FileNotFoundException"><paramref name="path"/> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found. </exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception><exception cref="T:System.IO.IOException">This file is being used by another process.</exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static string GetAttributes(string path)
        {
            return File.GetAttributes(path).ToString();
        }

        /// <summary>
        /// Sets the specified <see cref="T:System.IO.FileAttributes"/> of the file on the specified path.
        /// </summary>
        /// <param name="path">The path to the file. </param><param name="fileAttributes">A bitwise combination of the enumeration values. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is empty, contains only white spaces, contains invalid characters, or the file attribute is invalid. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void SetAttributes(string path, string fileAttributes)
        {
            FileAttributes result;
            if (Enum.TryParse(fileAttributes, out result))
            {
                File.SetAttributes(path, result);
            }
        }


        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// 
        /// <returns>
        /// A string containing all lines of the file.
        /// </returns>
        /// <param name="path">The file to open for reading. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// 
        /// <returns>
        /// A string containing all lines of the file.
        /// </returns>
        /// <param name="path">The file to open for reading. </param><param name="encoding">The encoding applied to the contents of the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static string ReadAllTextEncoding(string path, string encoding)
        {
            return File.ReadAllText(path, GetEncoding(encoding));
        }
        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param><param name="contents">The string to write to the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null or <paramref name="contents"/> is empty.  </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param><param name="contents">The string to write to the file. </param><param name="encoding">The encoding to apply to the string.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null or <paramref name="contents"/> is empty. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void WriteAllTextEncoding(string path, string contents, string encoding)
        {
            File.WriteAllText(path, contents, GetEncoding(encoding));
        }
        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// 
        /// <returns>
        /// A byte array containing the contents of the file.
        /// </returns>
        /// <param name="path">The file to open for reading. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static string ReadAllBytes(string path)
        {
            return Convert.ToBase64String(File.ReadAllBytes(path));
        }
        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param><param name="base64">The bytes to write to the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null or the byte array is empty. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>

        [JSchema]
        public static void WriteAllBytes(string path, string base64)
        {
            File.WriteAllBytes(path, Convert.FromBase64String(base64));
        }
        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// 
        /// <returns>
        /// A string array containing all lines of the file.
        /// </returns>
        /// <param name="path">The file to open for reading. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> ReadAllLines(string path)
        {
            return File.ReadAllLines(path).ToList();
        }
        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// 
        /// <returns>
        /// A string array containing all lines of the file.
        /// </returns>
        /// <param name="path">The file to open for reading. </param><param name="encoding">The encoding applied to the contents of the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static List<string> ReadAllLinesEncoding(string path, string encoding)
        {
            return File.ReadAllLines(path, GetEncoding(encoding)).ToList();
        }
        /// <summary>
        /// Reads the lines of a file.
        /// </summary>
        /// 
        /// <returns>
        /// All the lines of the file, or the lines that are the result of a query.
        /// </returns>
        /// <param name="path">The file to read.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars"/> method.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid (for example, it is on an unmapped drive).</exception><exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path"/> was not found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception><exception cref="T:System.IO.PathTooLongException"><paramref name="path"/> exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specifies a file that is read-only.-or-This operation is not supported on the current platform.-or-<paramref name="path"/> is a directory.-or-The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> ReadLines(string path)
        {
            return File.ReadLines(path).ToList();
        }
        /// <summary>
        /// Read the lines of a file that has a specified encoding.
        /// </summary>
        /// 
        /// <returns>
        /// All the lines of the file, or the lines that are the result of a query.
        /// </returns>
        /// <param name="path">The file to read.</param><param name="encoding">The encoding that is applied to the contents of the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by the <see cref="M:System.IO.Path.GetInvalidPathChars"/> method.</exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid (for example, it is on an unmapped drive).</exception><exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path"/> was not found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception><exception cref="T:System.IO.PathTooLongException"><paramref name="path"/> exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specifies a file that is read-only.-or-This operation is not supported on the current platform.-or-<paramref name="path"/> is a directory.-or-The caller does not have the required permission.</exception>
        [JSchema]
        public static List<string> ReadLinesEncoding(string path, string encoding)
        {
            return File.ReadLines(path, GetEncoding(encoding)).ToList();
        }
        /// <summary>
        /// Creates a new file, write the specified string array to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to. </param><param name="contents">The string array to write to the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is null.  </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void WriteAllLines(string path, List<string> contents)
        {
            File.WriteAllLines(path, contents);
        }
        /// <summary>
        /// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to. </param><param name="contents">The string array to write to the file. </param><param name="encoding">An <see cref="T:System.Text.Encoding"/> object that represents the character encoding applied to the string array.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is null.  </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void WriteAllLinesEncoding(string path, List<string> contents, string encoding)
        {
            File.WriteAllLines(path, contents, GetEncoding(encoding));
        }
     
        
        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="path">The file to append the specified string to. </param><param name="contents">The string to append to the file. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents);
        }
        /// <summary>
        /// Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="path">The file to append the specified string to. </param><param name="contents">The string to append to the file. </param><param name="encoding">The character encoding to use. </param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive). </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file. </exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specified a file that is read-only.-or- This operation is not supported on the current platform.-or- <paramref name="path"/> specified a directory.-or- The caller does not have the required permission. </exception><exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="path"/> was not found. </exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void AppendAllTextEncoding(string path, string contents, string encoding)
        {
            File.AppendAllText(path, contents, GetEncoding(encoding));
        }
        /// <summary>
        /// Appends lines to a file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to append the lines to. The file is created if it does not already exist.</param><param name="contents">The lines to append to the file.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars"/> method.</exception><exception cref="T:System.ArgumentNullException">Either<paramref name=" path "/>or <paramref name="contents"/> is null.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid (for example, it is on an unmapped drive).</exception><exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path"/> was not found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception><exception cref="T:System.IO.PathTooLongException"><paramref name="path"/> exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format.</exception><exception cref="T:System.Security.SecurityException">The caller does not have permission to write to the file.</exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specifies a file that is read-only.-or-This operation is not supported on the current platform.-or-<paramref name="path"/> is a directory.</exception>
        [JSchema]
        public static void AppendAllLinesLists(string path, List<string> contents)
        {
            File.AppendAllLines(path, contents);
        }
        /// <summary>
        /// Appends lines to a file by using a specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to append the lines to.</param><param name="contents">The lines to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param><exception cref="T:System.ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one more invalid characters defined by the <see cref="M:System.IO.Path.GetInvalidPathChars"/> method.</exception><exception cref="T:System.ArgumentNullException">Either<paramref name=" path"/>, <paramref name="contents"/>, or <paramref name="encoding"/> is null.</exception><exception cref="T:System.IO.DirectoryNotFoundException"><paramref name="path"/> is invalid (for example, it is on an unmapped drive).</exception><exception cref="T:System.IO.FileNotFoundException">The file specified by <paramref name="path"/> was not found.</exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception><exception cref="T:System.IO.PathTooLongException"><paramref name="path"/> exceeds the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters and file names must be less than 260 characters.</exception><exception cref="T:System.NotSupportedException"><paramref name="path"/> is in an invalid format.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception><exception cref="T:System.UnauthorizedAccessException"><paramref name="path"/> specifies a file that is read-only.-or-This operation is not supported on the current platform.-or-<paramref name="path"/> is a directory.-or-The caller does not have the required permission.</exception>
        [JSchema]
        public static void AppendAllLinesListsEncoding(string path, List<string> contents, string encoding)
        {
            File.AppendAllLines(path, contents, GetEncoding(encoding));
        }
        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. </param><param name="destFileName">The new path for the file. </param><exception cref="T:System.IO.IOException">The destination file already exists.-or-<paramref name="sourceFileName"/> was not found. </exception><exception cref="T:System.ArgumentNullException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is a zero-length string, contains only white space, or contains invalid characters as defined in <see cref="F:System.IO.Path.InvalidPathChars"/>. </exception><exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. </exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="sourceFileName"/> or <paramref name="destFileName"/> is invalid, (for example, it is on an unmapped drive). </exception><exception cref="T:System.NotSupportedException"><paramref name="sourceFileName"/> or <paramref name="destFileName"/> is in an invalid format. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }
        /// <summary>
        /// Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file.
        /// </summary>
        /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param><param name="destinationFileName">The name of the file being replaced.</param><param name="destinationBackupFileName">The name of the backup file.</param><exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName"/> parameter was not of a legal form.-or-The path described by the <paramref name="destinationBackupFileName"/> parameter was not of a legal form.</exception><exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName"/> parameter is null.</exception><exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception><exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo"/> object could not be found.-or-The file described by the <paramref name="destinationBackupFileName"/> parameter could not be found. </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.- or -The <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> parameters specify the same file.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.PlatformNotSupportedException">The operating system is Windows 98 Second Edition or earlier and the files system is not NTFS.</exception><exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName"/> or <paramref name="destinationFileName"/> parameter specifies a file that is read-only.-or- This operation is not supported on the current platform.-or- Source or destination parameters specify a directory instead of a file.-or- The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
        }

        /// <summary>
        /// Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file and optionally ignores merge errors.
        /// </summary>
        /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName"/>.</param><param name="destinationFileName">The name of the file being replaced.</param><param name="destinationBackupFileName">The name of the backup file.</param><param name="ignoreMetadataErrors">true to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; otherwise, false. </param><exception cref="T:System.ArgumentException">The path described by the <paramref name="destinationFileName"/> parameter was not of a legal form.-or-The path described by the <paramref name="destinationBackupFileName"/> parameter was not of a legal form.</exception><exception cref="T:System.ArgumentNullException">The <paramref name="destinationFileName"/> parameter is null.</exception><exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified. </exception><exception cref="T:System.IO.FileNotFoundException">The file described by the current <see cref="T:System.IO.FileInfo"/> object could not be found.-or-The file described by the <paramref name="destinationBackupFileName"/> parameter could not be found. </exception><exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.- or -The <paramref name="sourceFileName"/> and <paramref name="destinationFileName"/> parameters specify the same file.</exception><exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception><exception cref="T:System.PlatformNotSupportedException">The operating system is Windows 98 Second Edition or earlier and the files system is not NTFS.</exception><exception cref="T:System.UnauthorizedAccessException">The <paramref name="sourceFileName"/> or <paramref name="destinationFileName"/> parameter specifies a file that is read-only.-or- This operation is not supported on the current platform.-or- Source or destination parameters specify a directory instead of a file.-or- The caller does not have the required permission.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [JSchema]
        public static void ReplaceIgnoreErrors(string sourceFileName, string destinationFileName, string destinationBackupFileName,
            bool ignoreMetadataErrors)
        {
            File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }
    }
}
