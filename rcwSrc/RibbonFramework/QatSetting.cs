using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Ribbon;
using Windows.Win32.System.Com;

namespace WinForms.Ribbon
{
    /// <summary>
    /// Handle the persistence of QAT settings.
    /// </summary>
    internal sealed class QatSetting
    {
        private string? _qatSettingsFile;
        private byte[]? _loadedQatSettings;
        private RibbonStrip _ribbon;

        public QatSetting(RibbonStrip ribbon, string? qatSettingsFile)
        {
            _qatSettingsFile = BuildQatSettingsFile(qatSettingsFile);
            _ribbon = ribbon;
        }

        private string? BuildQatSettingsFile(string? filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            if (filename!.Contains("\\"))
            {
                string path = Path.GetFullPath(filename!);
                string? directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                return path;
            }
            else
            {
                string lAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                AssemblyName? assemblyName = Assembly.GetEntryAssembly()?.GetName();
                if (assemblyName == null)
                    return null;
                string directory = Path.Combine(lAppData, assemblyName.Name!);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                return Path.Combine(directory, filename!);
            }
        }

        private bool IsEqualSetting(MemoryStream stream)
        {
            if (_loadedQatSettings == null || stream.Length != _loadedQatSettings.Length)
                return false;
            byte[] buffer = stream.ToArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (_loadedQatSettings[i] != buffer[i])
                    return false;
            }
            return true;
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_qatSettingsFile))
            {
                MemoryStream stream = new MemoryStream();
                try
                {
                    _ribbon.SaveSettingsToStreamInternal(stream);
                    stream.Position = 0;
                    if (!IsEqualSetting(stream))
                    {
                        string fileName = _qatSettingsFile!;
                        FileStream fStream = File.Create(fileName);
                        stream.CopyTo(fStream);
                        fStream.Close();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        public void Load()
        {
            if (!string.IsNullOrEmpty(_qatSettingsFile))
            {
                string fileName = _qatSettingsFile!;
                if (File.Exists(fileName))
                {
                    Stream stream = File.OpenRead(fileName);
                    try
                    {
                        _loadedQatSettings = new byte[stream.Length];
                        stream.Read(_loadedQatSettings, 0, _loadedQatSettings.Length);
                        stream.Position = 0;
                        _ribbon.LoadSettingsFromStreamInternal(stream);
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }
            }
        }

        //private void SaveSettingsToFile()
        //{
        //    if (_application.UIRibbon != null)
        //    {
        //        //Save settings first to a MemoryStream and compare to the LoadSettings byte array
        //        bool saveToFile = true;
        //        if (_loadedQatSettings != null)
        //        {
        //            MemoryStream memStream = new MemoryStream();
        //            StreamAdapter streamAdapter = new StreamAdapter(memStream);
        //            _application.UIRibbon.SaveSettingsToStream(streamAdapter);
        //            memStream.Position = 0;
        //            byte[] buffer = memStream.GetBuffer();
        //            if (buffer.Length == _loadedQatSettings.Length)
        //            {
        //                saveToFile = false;
        //                for (int i = 0; i < buffer.Length; i++)
        //                {
        //                    if (_loadedQatSettings[i] != buffer[i])
        //                    {
        //                        saveToFile = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        //if settings are changed then save it to the file
        //        if (saveToFile)
        //        {
        //            HRESULT hr = PInvoke.SHCreateStreamOnFileEx(_qatSettingsFile, (uint)(STGM.STGM_WRITE | STGM.STGM_CREATE), (uint)FileAttributes.Normal, true, null, out IStream stream);

        //            if (hr.Failed)
        //                throw new IOException();

        //            hr = _application.UIRibbon.SaveSettingsToStream(stream);
        //            if (hr.Failed)
        //            {
        //                stream.Revert();
        //                throw new IOException();
        //            }

        //            stream.Commit(STGC.STGC_DEFAULT);
        //            Marshal.ReleaseComObject(stream);
        //        }
        //    }
        //}

        //internal unsafe void LoadSettingsFromFile()
        //{
        //    if (File.Exists(_qatSettingsFile) && _application.UIRibbon != null)
        //    {
        //        //uint attr = (uint)Windows.Win32.Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL;
        //        HRESULT hr = PInvoke.SHCreateStreamOnFileEx(_qatSettingsFile, (uint)STGM.STGM_READ, (uint)FileAttributes.Normal, false, null, out IStream stream);
        //        if (hr.Failed)
        //            throw new IOException();

        //        //Read the stream to a byte array for comparing SaveSettings
        //        PInvoke.IStream_Size(stream, out ulong size);
        //        //stream.Stat(out STATSTG status, STATFLAG.STATFLAG_NONAME);
        //        _loadedQatSettings = new byte[size];
        //        uint readSize;
        //        fixed (byte* qatSettings = _loadedQatSettings)
        //            stream.Read(qatSettings, (uint)_loadedQatSettings.Length, &readSize);
        //        stream.Seek(0, SeekOrigin.Begin, null);
        //        hr = _application.UIRibbon.LoadSettingsFromStream(stream);
        //        Marshal.ReleaseComObject(stream);
        //        if (hr.Failed)
        //            throw new IOException();
        //    }
        //}
    }
}
