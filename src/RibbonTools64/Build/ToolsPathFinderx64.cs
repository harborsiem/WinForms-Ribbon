using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Diagnostics.Eventing.Reader;

namespace UIRibbonTools
{
    public class ToolsPathFinder
    {
        private DirectoryInfo _latestSDKToolsPath;
        private DirectoryInfo _latestMSVCToolsPath;
        private LinkPaths _linkPaths;

        public ToolsPathFinder(bool getSDK, bool getMSVCTools)
        {
            if (getSDK)
                _latestSDKToolsPath = GetLatestSDKToolsPath();
            if (getMSVCTools)
            {
                _linkPaths = new LinkPaths();
                if (DetectLatestVSVersion(_linkPaths))
                    _latestMSVCToolsPath = new DirectoryInfo(Path.GetDirectoryName(_linkPaths.VcLinkExe));
            }
        }

        //SDK detection functions
        /// <summary>
        /// return array is sorted with most recent sdk version at index 0.
        /// </summary>
        private static SDKVersion[] GetSortedSDKVersions(string[] versions)
        {
            SDKVersion[] sdkVersions = new SDKVersion[versions.Length];
            for (int i = 0; i < versions.Length; i++)
            {
                SDKVersion v = new SDKVersion();
                v.sdkVersion = versions[i];
                string sdk = versions[i];

                if (sdk.StartsWith("v"))
                {
                    sdk = sdk.Substring(1);
                }
                string[] sdkSplit = sdk.Split('.');
                if (sdkSplit.Length >= 2)
                {
                    Int32.TryParse(sdkSplit[0], out v.major);
                    v.minor = sdkSplit[1];
                }
                sdkVersions[i] = v;
            }
            Array.Sort<SDKVersion>(sdkVersions);
            return sdkVersions;
        }

        /// <summary>
        /// return folder that contains uicc.exe and rc.exe.
        /// </summary>
        private static string FindUiccFolder(string installFolder, string productVersion)
        {
            const string uicc = "uicc.exe";
            string binFolder = Path.Combine(installFolder, "bin");
            if (File.Exists(Path.Combine(binFolder, uicc)))
            {
                return binFolder;
            }
            string toolsFolder = Path.Combine(binFolder, Environment.Is64BitProcess ? "x64" : "x86");
            if (File.Exists(Path.Combine(toolsFolder, uicc)))
            {
                return toolsFolder;
            }
            if (!string.IsNullOrEmpty(productVersion))
            {
                string[] versionComponents = productVersion.Split('.');
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    if (i < versionComponents.Length)
                    {
                        builder.Append(versionComponents[i] + ".");
                    }
                    else
                    {
                        builder.Append("0.");
                    }
                }
                builder.Length = builder.Length - 1;
                string productVersionFull = builder.ToString();

                toolsFolder = Path.Combine(binFolder, productVersionFull, Environment.Is64BitProcess ? "x64" : "x86");
                if (File.Exists(Path.Combine(toolsFolder, uicc)))
                {
                    return toolsFolder;
                }
            }
            return string.Empty;
        }

        private static DirectoryInfo GetLatestSDKToolsPath()
        {
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey winSdkKey = hklm.OpenSubKey("SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows");
            if (winSdkKey != null)
            {
                string[] versions = winSdkKey.GetSubKeyNames();

                SDKVersion[] sdkVersions = GetSortedSDKVersions(versions);

                for (int i = 0; i < sdkVersions.Length; i++)
                {
                    RegistryKey sdkToolsKey = winSdkKey.OpenSubKey(sdkVersions[i].sdkVersion);
                    if (sdkToolsKey == null)
                        continue;

                    string installFolder = sdkToolsKey.GetValue("InstallationFolder") as string;
                    string productVersion = sdkToolsKey.GetValue("ProductVersion") as string;
                    if (!string.IsNullOrEmpty(installFolder))
                    {
                        string sdkToolsPath = FindUiccFolder(installFolder, productVersion);

                        if (!string.IsNullOrEmpty(sdkToolsPath))
                        {
                            if (!sdkToolsPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                            {
                                sdkToolsPath += Path.DirectorySeparatorChar;
                            }
                            return new DirectoryInfo(sdkToolsPath);
                        }
                    }
                }
            }
            return null;
        }

        //MSVC detection functions
        private static string[] GetVsWhereInfo(string option)
        {
            string[] lines = null;
            string programFiles = Environment.ExpandEnvironmentVariables(string.Format(@"%PROGRAMFILES{0}%", Environment.Is64BitOperatingSystem ? "(x86)" : ""));
            string vswhereExe = Path.Combine(programFiles, "Microsoft Visual Studio", "Installer", "vswhere.exe");
            if (File.Exists(vswhereExe))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(vswhereExe);
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.Arguments = option;

                //if (RequiredWorkloads != null && RequiredWorkloads.Length > 0)
                //{
                //    startInfo.Arguments += " -requires " + string.Join(" ", RequiredWorkloads);
                //}

                Process proc = Process.Start(startInfo);
                proc.WaitForExit();
                if (proc.ExitCode != 0) throw new InvalidOperationException($"vswhere.exe exited with code {proc.ExitCode}");

                string allOutput = proc.StandardOutput.ReadToEnd();
                lines = allOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return lines;
        }

        private static string GetMSVCToolsVersion(string visualStudioInstallRoot)
        {
            string filePath = Path.Combine(visualStudioInstallRoot, "VC", "Auxiliary", "Build", "Microsoft.VCToolsVersion.default.txt");
            if (File.Exists(filePath))
                return File.ReadAllText(filePath).Trim();
            else
                return string.Empty;
        }

        private static string[] DetectMSVCVersion()
        {
            string[] lines = GetVsWhereInfo("-latest");
            if (lines != null)
            {
                string installRoot = null;
                foreach (string line in lines)
                {
                    if (line.StartsWith("installationPath:"))
                    {
                        installRoot = line.Substring("installationPath:".Length).TrimStart();
                        string msvcVersion = GetMSVCToolsVersion(installRoot);
                        if (!string.IsNullOrEmpty(msvcVersion))
                            return new string[] { msvcVersion, installRoot };
                    }
                }
            }
            return new string[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkPaths"></param>
        /// <returns>true if detected</returns>
        private static bool DetectLatestVSVersion(LinkPaths linkPaths)
        {
            bool result = false;
            string vcvarsBat = string.Empty;
            string vcLinkExe = string.Empty;

            string[] lines = GetVsWhereInfo("-latest");
            if (lines != null)
            {
                string installRoot = null;
                foreach (string line in lines)
                {
                    if (line.StartsWith("installationPath:"))
                    {
                        installRoot = line.Substring("installationPath:".Length).TrimStart();
                        if (Environment.Is64BitProcess)
                            vcvarsBat = Path.Combine(installRoot, "VC", "Auxiliary", "Build", "vcvars64.bat");
                        else
                            vcvarsBat = Path.Combine(installRoot, "VC", "Auxiliary", "Build", "vcvars32.bat");
                        string msvcVersion = GetMSVCToolsVersion(installRoot);
                        if (!string.IsNullOrEmpty(msvcVersion))
                        {
                            if (Environment.Is64BitProcess)
                                vcLinkExe = Path.Combine(installRoot, "VC", "Tools", "MSVC", msvcVersion, "bin", "Hostx64", "x64", "link.exe");
                            else
                                vcLinkExe = Path.Combine(installRoot, "VC", "Tools", "MSVC", msvcVersion, "bin", "Hostx86", "x86", "link.exe");
                            linkPaths.VcVarsBat = vcvarsBat;
                            linkPaths.VcLinkExe = vcLinkExe;
                            linkPaths.EnvironmentVcVarsBat = string.Empty;
                            linkPaths.EnvironmentVcLinkExe = string.Empty;
                            result = true;
                        }
                        break;
                    }
                }
            }
            else
                result = GetLatestOlderVSVersion(linkPaths);

            return result;
        }

        //Detection for VS2010 to VS2015
        private static bool GetLatestOlderVSVersion(LinkPaths linkPaths)
        {
            if (GetPaths("%VS140COMNTOOLS%", linkPaths))
            {
                return true;
            }
            if (GetPaths("%VS120COMNTOOLS%", linkPaths))
            {
                return true;
            }
            if (GetPaths("%VS110COMNTOOLS%", linkPaths))
            {
                return true;
            }
            if (GetPaths("%VS100COMNTOOLS%", linkPaths))
            {
                return true;
            }
            return false;
        }

        private static bool GetPaths(string envVsVers, LinkPaths linkPaths)
        {
            bool result = false;
            string vsVers = Environment.ExpandEnvironmentVariables(envVsVers);
            string vcvarsBat = string.Empty;
            string vcLinkExe = string.Empty;
            if (!vsVers.StartsWith("%VS"))
            {
                if (Environment.Is64BitProcess)
                    vcvarsBat = Path.GetFullPath(Path.Combine(vsVers, @"..\..\VC\bin\vcvars64.bat"));
                else
                    vcvarsBat = Path.GetFullPath(Path.Combine(vsVers, @"..\..\VC\bin\vcvars32.bat"));
                vcLinkExe = Path.GetFullPath(Path.Combine(vsVers, @"..\..\VC\bin\link.exe"));
                result = true;
            }
            linkPaths.VcVarsBat = vcvarsBat;
            linkPaths.VcLinkExe = vcLinkExe;
            if (Environment.Is64BitProcess)
                linkPaths.EnvironmentVcVarsBat = envVsVers + @"..\..\VC\bin\vcvars64.bat";
            else
                linkPaths.EnvironmentVcVarsBat = envVsVers + @"..\..\VC\bin\vcvars32.bat";
            linkPaths.EnvironmentVcLinkExe = envVsVers + @"..\..\VC\bin\link.exe";
            return result;
        }

        public string GetUiccExe()
        {
            if (_latestSDKToolsPath != null)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_latestSDKToolsPath.FullName, "uicc.exe"));
                if (fileInfo.Exists)
                    return fileInfo.FullName;
            }
            return string.Empty;
        }

        public string GetRcExe()
        {
            if (_latestSDKToolsPath != null)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_latestSDKToolsPath.FullName, "rc.exe"));
                if (fileInfo.Exists)
                    return fileInfo.FullName;
            }
            return string.Empty;
        }

        public string GetLinkExe()
        {
            if (_latestMSVCToolsPath != null)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(_latestMSVCToolsPath.FullName, "link.exe"));
                if (fileInfo.Exists)
                    return fileInfo.FullName;
            }
            return string.Empty;
        }

        public string GetVcvarsBat()
        {
            if (_linkPaths != null)
            {
                return _linkPaths.VcVarsBat;
            }
            return string.Empty;
        }

        public string TryUpdateMSVCVersion(string linkPath)
        {
            const string Msvc = "MSVC";
            string newLinkPath = null;
            int startIndex = linkPath.LastIndexOf(Msvc, StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                startIndex += Msvc.Length + 1;
                int endIndex = linkPath.IndexOf(Path.DirectorySeparatorChar, startIndex);
                string msvcVersion = linkPath.Substring(startIndex, endIndex - startIndex);
                string actualVersion = null;
                string installRoot = null;
                string[] result = DetectMSVCVersion();
                if (result.Length == 2)
                {
                    actualVersion = result[0];
                    installRoot = result[1];
                }
                if (!string.IsNullOrEmpty(installRoot) && linkPath.IndexOf(installRoot, StringComparison.OrdinalIgnoreCase) >= 0 && (actualVersion != msvcVersion))
                {
                    newLinkPath = linkPath.Substring(0, startIndex) + actualVersion + linkPath.Substring(endIndex);

                }
            }
            return newLinkPath;
        }

        class LinkPaths
        {
            public string VcLinkExe { get; set; }
            public string VcVarsBat { get; set; }
            public string EnvironmentVcLinkExe { get; set; }
            public string EnvironmentVcVarsBat { get; set; }
        }

        class SDKVersion : IComparable<SDKVersion>
        {
            public string sdkVersion;
            public int major;
            public string minor;
            //public string build;

            public int CompareTo(SDKVersion other)
            {
                if (major > other.major)
                {
                    return -1;
                }
                if (major < other.major)
                {
                    return 1;
                }
                if (major == other.major)
                {
                    return -minor.CompareTo(other.minor);
                }
                return 0;
            }
        }
    }
}
