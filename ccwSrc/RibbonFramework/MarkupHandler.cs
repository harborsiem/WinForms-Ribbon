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
        private RibbonStrip _ribbon;
        private readonly string MarkupResourceIdent; //for Exceptions comment

        public string? ResourceIdentifier { get; private set; }

        public HMODULE MarkupDllHandle { get; private set; }

        public MarkupHandler(Assembly executingAssembly, RibbonStrip ribbon)
        {
            MarkupResourceIdent = $"{nameof(RibbonStrip)}.{nameof(ribbon.MarkupResource)}";
            _ribbon = ribbon;
            ResourceIdentifier = ribbon.ResourceIdentifier;
            MarkupDllHandle = HMODULE.Null;
            InitFramework(ribbon.MarkupResource, executingAssembly);
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
            return;

            //InitFramework(resourceName, _loadedDllHandle);
            //GCHandle namesHandle = GCHandle.Alloc(names);
            //try
            //{
            //    ENUMRESNAMEPROC callback = EnumResNameProc;
            //    IntPtr enumResNameProc = Marshal.GetFunctionPointerForDelegate(callback);
            //    fixed (char* pType = "UIFILE")
            //        PInvoke.EnumResourceNames(MarkupDllHandle, pType, (delegate* unmanaged[Stdcall]<HMODULE, PCWSTR, PWSTR, nint, BOOL>)enumResNameProc, (nint)namesHandle);
            //}
            //finally
            //{
            //    if (namesHandle.IsAllocated)
            //        namesHandle.Free();
            //}
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
                throw new ApplicationException(string.Format("Resource DLL not valid '{0}'?", nameof(RibbonStrip) + "." + nameof(_ribbon.ResourceIdentifier)));
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

        //delegate BOOL ENUMRESNAMEPROC(HMODULE hModule, PCWSTR lpszType, PWSTR lpszName, nint lParam);
        //List<string> names = new List<string>();

        //private static unsafe BOOL EnumResNameProc(HMODULE hModule, PCWSTR pType, PWSTR pName, nint param)
        //{
        //    if (pType.ToString() == "UIFILE")
        //    {
        //        GCHandle listHandle = GCHandle.FromIntPtr(param);
        //        List<string> names = (List<string>)listHandle.Target!;
        //        names.Add(pName.ToString());
        //    }
        //    return true;
        //}

        /// <summary>
        /// Parse the embedded resource RibbonMarkup.h file which is produced by UICC.exe during build process
        /// Don't use Symbols for resources because parsing failed
        /// </summary>
        /// <param name="markupHeader"></param>
        /// <param name="executingAssembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static Dictionary<ushort, MarkupResIds>? ParseHeader(string? markupHeader, Assembly executingAssembly)
        {
            if (markupHeader == null)
                return null;
            using Stream? stream = executingAssembly.GetManifestResourceStream(markupHeader);
            if (stream == null)
                return null;
            Dictionary<ushort, MarkupResIds> allMarkupResIds = new Dictionary<ushort, MarkupResIds>();
            Dictionary<string, ushort> commands = new Dictionary<string, ushort>();
            StreamReader sr = new StreamReader(stream);
            try
            {
                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (line!.StartsWith("#define"))
                    {
                        if (!line.Contains("_RESID "))
                        {
                            line = line.Remove(0, "#define ".Length).TrimEnd();
                            string[] lineSplit = line.Split(' ');
                            if (lineSplit.Length < 2)
                            {
                                throw new ArgumentException("Error in .h file");
                            }
                            if (lineSplit.Length > 2)
                            {
                                string comment = line.Substring(line.IndexOf("/*")).Remove(0, 3);
                                comment = comment.Substring(0, comment.LastIndexOf(" */"));
                            }
                            if (ushort.TryParse(lineSplit[1], out ushort value))
                            {
                                MarkupResIds resIds = new MarkupResIds() { CommandId = value, CommandName = lineSplit[0] };
                                commands[lineSplit[0]] = value;
                                allMarkupResIds[value] = resIds;
                            }
                        }
                        else
                        {
                            line = line.Remove(0, "#define ".Length).TrimEnd();
                            string[] lineSplit = line.Split(' ');
                            if (lineSplit.Length < 2)
                            {
                                throw new ArgumentException("Error in .h file");
                            }
                            if (ushort.TryParse(lineSplit[1], out ushort value))
                            {
                                string[] lineSplitComponents = lineSplit[0].Split('_');
                                string extraName = lineSplitComponents[lineSplitComponents.Length - 2];
                                int index;
                                string commandName;
                                MarkupResIds resIds;
                                if (string.IsNullOrEmpty(extraName)) //It is a Image with dpi value
                                {
                                    extraName = lineSplitComponents[1] + "_" + lineSplitComponents[2];
                                }
                                index = lineSplit[0].LastIndexOf(extraName, StringComparison.InvariantCulture);
                                commandName = lineSplit[0].Remove(index - 1);
                                if (commands.ContainsKey(commandName))
                                {
                                    ushort commandId = commands[commandName];
                                    resIds = allMarkupResIds[commandId];
                                    switch (extraName)
                                    {
                                        case "LabelTitle":
                                            resIds.LabelTitleId = value;
                                            break;
                                        case "LabelDescription":
                                            resIds.LabelDescriptionId = value;
                                            break;
                                        case "TooltipTitle":
                                            resIds.TooltipTitleId = value;
                                            break;
                                        case "TooltipDescription":
                                            resIds.TooltipDescriptionId = value;
                                            break;
                                        case "Keytip":
                                            resIds.KeytipId = value;
                                            break;
                                        case "SmallImages":
                                            resIds.SmallImages = value;
                                            break;
                                        case "SmallImages_96":
                                            resIds.SmallImages_96 = value;
                                            break;
                                        case "SmallImages_120":
                                            resIds.SmallImages_120 = value;
                                            break;
                                        case "SmallImages_144":
                                            resIds.SmallImages_144 = value;
                                            break;
                                        case "SmallImages_192":
                                            resIds.SmallImages_192 = value;
                                            break;
                                        case "LargeImages":
                                            resIds.LargeImages = value;
                                            break;
                                        case "LargeImages_96":
                                            resIds.LargeImages_96 = value;
                                            break;
                                        case "LargeImages_120":
                                            resIds.LargeImages_120 = value;
                                            break;
                                        case "LargeImages_144":
                                            resIds.LargeImages_144 = value;
                                            break;
                                        case "LargeImages_192":
                                            resIds.LargeImages_192 = value;
                                            break;
                                        case "SmallHighContrastImages":
                                            resIds.SmallHighContrastImages = value;
                                            break;
                                        case "SmallHighContrastImages_96":
                                            resIds.SmallHighContrastImages_96 = value;
                                            break;
                                        case "SmallHighContrastImages_120":
                                            resIds.SmallHighContrastImages_120 = value;
                                            break;
                                        case "SmallHighContrastImages_144":
                                            resIds.SmallHighContrastImages_144 = value;
                                            break;
                                        case "SmallHighContrastImages_192":
                                            resIds.SmallHighContrastImages_192 = value;
                                            break;
                                        case "LargeHighContrastImages":
                                            resIds.LargeHighContrastImages = value;
                                            break;
                                        case "LargeHighContrastImages_96":
                                            resIds.LargeHighContrastImages_96 = value;
                                            break;
                                        case "LargeHighContrastImages_120":
                                            resIds.LargeHighContrastImages_120 = value;
                                            break;
                                        case "LargeHighContrastImages_144":
                                            resIds.LargeHighContrastImages_144 = value;
                                            break;
                                        case "LargeHighContrastImages_192":
                                            resIds.LargeHighContrastImages_192 = value;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            return allMarkupResIds;
        }
    }
}
