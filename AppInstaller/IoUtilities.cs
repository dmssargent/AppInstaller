using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace APKInstaller
{
    /// <summary>Provides utilities for performing various common IO tasks</summary>
    public class IoUtilities : IDisposable
    {
        private static IoUtilities _instance = new IoUtilities();
        private bool _disposedValue;
        private bool _isReady;
        private List<string> _sessions;
        private string _tempFilePath;

        public IoUtilities()
        {
            _isReady = false;
            _sessions = new List<string>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public static void UnzipFromFile(string file, string outFolder) {
            using (var stream = new FileStream (file, FileMode.Open)) {
                UnzipFromStream(stream, outFolder);
            }
        }


        /// <summary>Unzips a file from a file stream into a folder</summary>
        /// <param name="zipStream">the stream from a zip file</param>
        /// <param name="outFolder">the path of the destination directory</param>
        public static void UnzipFromStream(Stream zipStream, string outFolder)
        {
            var zipInputStream = new ZipInputStream(zipStream);
            var nextEntry = zipInputStream.GetNextEntry();
            var buffer = new byte[4097];
            for (; nextEntry != null; nextEntry = zipInputStream.GetNextEntry())
            {
                var path2 = nextEntry.Name.Replace("/", (Path.DirectorySeparatorChar.ToString()));
                var path = Path.Combine(outFolder, path2);
                var directoryName = Path.GetDirectoryName(path);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);
                if (!((directoryName + Path.DirectorySeparatorChar.ToString()) == (path)))
                {
                    using (var fileStream = File.Create(path))
                        StreamUtils.Copy(zipInputStream, fileStream, buffer);
                    Mono.Unix.Native.Syscall.chmod(path, Mono.Unix.Native.FilePermissions.S_IRWXU);
                }
            }
        }

        /// <summary>Prepares the IOUtils to do operations</summary>
        public static void Prepare()
        {
            if (_instance == null)
                return;
            _instance._Prepare();
        }

        private void _Prepare()
        {
            if (_isReady)
                return;
            _tempFilePath = Path.GetTempFileName();
            File.Delete(_tempFilePath);
            Directory.CreateDirectory(_tempFilePath);
            _isReady = true;
        }

        /// <summary>Creates a new temp session, and returns the path to that temp session</summary>
        /// <param name="name">the name of the temp session</param>
        /// <returns>the path of the temp session</returns>
        public static string CreateTempSession(string name)
        {
            Prepare();
            if (name == null)
                throw new ArgumentNullException("name");
            var path = Path.Combine(_instance._tempFilePath, name);
            if (_instance._sessions.Exists(test => name.Equals(test)))
                return path;
            if (File.Exists(path))
                File.Delete(path);
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>Disposes of the current IO Utilities</summary>
        public static void Cleanup()
        {
            _instance.Dispose();
        }

        /// <summary>Removes a specified temp session</summary>
        /// <param name="name">the name of a previous created temp session</param>
        public static void RemoveTempSession(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            var path = Path.Combine(_instance._tempFilePath, name);
            if (_instance._sessions.Exists(test => name.Equals(test)) && File.Exists(path))
                File.Delete(path);
            throw new ArgumentException("Session \"" + name + "\" doesn't currently exist");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                var path = _tempFilePath;
                if (disposing)
                {
                    _isReady = false;
                    _instance = null;
                    _sessions.Clear();
                    _sessions = null;
                    _tempFilePath = null;
                }
                if (File.Exists(path))
                {
                    if (Directory.Exists(path))
                        Directory.Delete(path);
                    else
                        File.Delete(path);
                }
            }
            _disposedValue = true;
        }
    }
}