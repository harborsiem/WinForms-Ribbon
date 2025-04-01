using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Reflection;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.LibraryLoader;

namespace WinForms.Ribbon
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class MarkupHandler : IDisposable
    {
        private const string MarkupAsFileMarker = "file://";
        private const string DefaultResourceIdent = "APPLICATION_RIBBON";
        private const string ResourceIdentExtension = "_RIBBON";

        private string? _tempDllFilename;
        private RibbonStrip _ribbonStrip;
        private readonly string MarkupResourceIdent; //for Exceptions comment

        public string? ResourceIdentifier { get; private set; }

        public HMODULE MarkupDllHandle { get; private set; }

        public MarkupHandler(Assembly executingAssembly, RibbonStrip ribbonStrip)
        {
            MarkupResourceIdent = $"{nameof(RibbonStrip)}.{nameof(ribbonStrip.MarkupResource)}";
            _ribbonStrip = ribbonStrip;
            ResourceIdentifier = ribbonStrip.ResourceIdentifier;
            MarkupDllHandle = HMODULE.Null;
            InitFramework(ribbonStrip.MarkupResource, executingAssembly);
        }

        ~MarkupHandler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Embedded resource based Ribbon Dll
        /// </summary>
        private byte[] GetLocalizedRibbon(string markupResource, Assembly executingAssembly)
        {
            byte[]? data;
            bool found = false;

            // try to get from current current culture satellite assembly first
            var culture = Thread.CurrentThread.CurrentUICulture;
            Assembly satelliteAssembly;
            found = TryGetSatelliteAssembly(culture, executingAssembly, out satelliteAssembly);

            if (found)
            {
                found = TryGetRibbonMarkup(markupResource, satelliteAssembly, out data);
                if (found)
                    return data;
            }

            // try to get from current current culture fallback satellite assembly
            if (culture.Parent != null)
            {
                Assembly fallbackAssembly;
                found = TryGetSatelliteAssembly(culture.Parent, executingAssembly, out fallbackAssembly);

                if (found)
                {
                    found = TryGetRibbonMarkup(markupResource, fallbackAssembly, out data);
                    if (found)
                        return data;
                }
            }

            // try to get from current current culture fallback satellite assembly
            found = TryGetRibbonMarkup(markupResource, executingAssembly, out data);
            if (!found)
                throw new ArgumentException(string.Format(MarkupResourceIdent + " resource '{0}' not found in assembly '{1}'.", markupResource, executingAssembly.Location));

            return data;
        }

        private bool TryGetSatelliteAssembly(CultureInfo culture, Assembly executingAssembly, out Assembly satelliteAssembly)
        {
            try
            {
                satelliteAssembly = executingAssembly.GetSatelliteAssembly(culture);
                return true;
            }
            catch (Exception)
            {
                satelliteAssembly = executingAssembly;
                return false;
            }
        }

        private bool TryGetRibbonMarkup(string markupResource, Assembly assembly, out byte[] data)
        {
            try
            {
                byte[]? byteData = Util.GetEmbeddedResource(markupResource, assembly);
                if (byteData == null)
                {
                    data = new byte[0];
                    return false;
                }
                data = byteData;
                return true;
            }
            catch (Exception)
            {
                data = new byte[0];
                return false;
            }
        }

        /// <summary>
        /// File based Ribbon Dll method
        /// </summary>
        private byte[] GetLocalizedRibbonFileData(string markupResource)
        {
            string? path = null;
            string localizedPath = String.Empty;
            string fileName;
            string? directoryName;
            var culture = Thread.CurrentThread.CurrentUICulture;
            try
            {
                int start = markupResource.IndexOf('{'); //Mark for Special Folder like LocalApplicationData
                int last = -1;
                if (start > 0)
                {
                    last = markupResource.IndexOf('}');
                    if (last < start)
                    {
                        throw new ArgumentException("} not found in " + MarkupResourceIdent);
                    }
                    string specialFolder = markupResource.Substring(start + 1, last - start - 1);
                    Environment.SpecialFolder enumSpecial = (Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), specialFolder);
                    string specialFolderPath = Environment.GetFolderPath(enumSpecial);
                    path = markupResource.Substring(0, start) + specialFolderPath + markupResource.Substring(last + 1);
                }
                else
                {
                    path = MarkupAsFileMarker + Path.GetFullPath(markupResource.Substring(MarkupAsFileMarker.Length));
                }
                path = new Uri(path).LocalPath;
                if (File.Exists(path))
                {
                    localizedPath = path;
                    fileName = Path.GetFileName(path);
                    directoryName = Path.GetDirectoryName(path);
                    string cultureName = culture.Name;
                    string? localPath = null;
                    if (directoryName != null && (localPath = TryGetCultureFile(directoryName, fileName, cultureName, true)) == null)
                    {
                        if ((localPath = TryGetCultureFile(directoryName, fileName, cultureName, false)) == null)
                        {
                            if (culture.Parent != null)
                            {
                                cultureName = culture.Parent.Name;
                                if ((localPath = TryGetCultureFile(directoryName, fileName, cultureName, true)) == null)
                                {
                                    localPath = TryGetCultureFile(directoryName, fileName, cultureName, false);
                                }
                            }
                        }
                    }
                    if (localPath != null)
                    {
                        localizedPath = localPath;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{MarkupResourceIdent} is invalid", ex);
            }
            if (String.IsNullOrEmpty(localizedPath))
            {
                throw new ArgumentException($"{MarkupResourceIdent} is invalid");
            }
            else
                return File.ReadAllBytes(localizedPath);
        }

        private static string? TryGetCultureFile(string directoryName, string fileName, string cultureName, bool root)
        {
            string? localizedPath = null;
            string localizedDirectory;
            if (root)
                localizedDirectory = directoryName;
            else
                localizedDirectory = Path.Combine(directoryName, cultureName);

            if (Directory.Exists(localizedDirectory))
            {
                string tmpPath = Path.Combine(localizedDirectory, fileName);
                if (File.Exists(tmpPath) && !root)
                {
                    localizedPath = tmpPath;
                }
                else
                {
                    tmpPath = Path.Combine(localizedDirectory, Path.GetFileNameWithoutExtension(fileName) + "." + cultureName + Path.GetExtension(fileName));
                    if (File.Exists(tmpPath))
                    {
                        localizedPath = tmpPath;
                    }
                }
            }
            return localizedPath;
        }

        /// <summary>
        /// Initalize ribbon framework
        /// </summary>
        /// <param name="markupResource">Name of ribbon dll</param>
        /// <param name="executingAssembly">Assembly where ribbon should reside</param>
        private void InitFramework(string markupResource, Assembly executingAssembly)
        {
            string path = Path.GetTempPath();
            _tempDllFilename = Path.Combine(path, Path.GetTempFileName());
            byte[] buffer;
            if (markupResource.ToLowerInvariant().StartsWith(MarkupAsFileMarker))
            {
                buffer = GetLocalizedRibbonFileData(markupResource);
            }
            else
                buffer = GetLocalizedRibbon(markupResource, executingAssembly);

            File.WriteAllBytes(_tempDllFilename, buffer);

            // if ribbon dll exists, use it
            if (File.Exists(_tempDllFilename))
            {
                if (string.IsNullOrEmpty(ResourceIdentifier))
                    ResourceIdentifier = DefaultResourceIdent;
                else
                    ResourceIdentifier = ResourceIdentifier + ResourceIdentExtension;
                // load ribbon from ribbon dll resource
                InitFramework(_tempDllFilename);
            }
        }

        /// <summary>
        /// Initalize ribbon framework
        /// </summary>
        /// <param name="ribbonDllName">Dll name where to find ribbon resource</param>
        private unsafe void InitFramework(string ribbonDllName)
        {
            // dynamically load ribbon library
            fixed (char* ribbonDllNameLocal = ribbonDllName)
                MarkupDllHandle = PInvoke.LoadLibraryEx(ribbonDllNameLocal, HANDLE.Null,
                                                                LOAD_LIBRARY_FLAGS.DONT_RESOLVE_DLL_REFERENCES |
                                                                LOAD_LIBRARY_FLAGS.LOAD_IGNORE_CODE_AUTHZ_LEVEL |
                                                                LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_AS_DATAFILE |
                                                                LOAD_LIBRARY_FLAGS.LOAD_LIBRARY_AS_IMAGE_RESOURCE);

            if (MarkupDllHandle == HMODULE.Null)
            {
                Dispose();
                throw new ApplicationException(MarkupResourceIdent + " resource DLL exists but could not be loaded.");
            }

            //InitFramework(resourceName, _loadedDllHandle);
            HRSRC hrSRC;
            uint imageSize = 0;
            fixed (char* pName = ResourceIdentifier)
            fixed (char* pType = "UIFILE")
                hrSRC = PInvoke.FindResource(MarkupDllHandle, pName, pType);
            if (hrSRC != IntPtr.Zero)
            {
                imageSize = PInvoke.SizeofResource(MarkupDllHandle, hrSRC);
            }
            if (imageSize == 0)
            {
                Dispose();
                throw new ApplicationException(string.Format("Resource DLL not valid '{0}'?", nameof(RibbonStrip) + "." + nameof(_ribbonStrip.ResourceIdentifier)));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (MarkupDllHandle != HMODULE.Null)
            {
                // free dynamic library
                PInvoke.FreeLibrary(MarkupDllHandle);
                MarkupDllHandle = HMODULE.Null;
            }
            if (!string.IsNullOrEmpty(_tempDllFilename))
            {
                try
                {
                    if (File.Exists(_tempDllFilename))
                        File.Delete(_tempDllFilename);
                    _tempDllFilename = null;
                }
                catch { }
            }
        }
    }
}
