// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.RiderPathLocator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using JetBrains.Annotations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  internal static class RiderPathLocator
  {
    public static RiderPathLocator.RiderInfo[] GetAllRiderPaths()
    {
      try
      {
        switch (PlatformInfo.operatingSystemFamily)
        {
          case OperatingSystemFamily.MacOSX:
            return RiderPathLocator.CollectRiderInfosMac();
          case OperatingSystemFamily.Windows:
            return RiderPathLocator.CollectRiderInfosWindows();
          case OperatingSystemFamily.Linux:
            return RiderPathLocator.CollectAllRiderPathsLinux();
        }
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      return Array.Empty<RiderPathLocator.RiderInfo>();
    }

    private static RiderPathLocator.RiderInfo[] CollectAllRiderPathsLinux()
    {
      List<RiderPathLocator.RiderInfo> source = new List<RiderPathLocator.RiderInfo>();
      string environmentVariable = Environment.GetEnvironmentVariable("HOME");
      if (!string.IsNullOrEmpty(environmentVariable))
      {
        string toolboxBaseDir = RiderPathLocator.GetToolboxBaseDir();
        source.AddRange((IEnumerable<RiderPathLocator.RiderInfo>) ((IEnumerable<string>) RiderPathLocator.CollectPathsFromToolbox(toolboxBaseDir, "bin", "rider.sh", false)).Select<string, RiderPathLocator.RiderInfo>((Func<string, RiderPathLocator.RiderInfo>) (a => new RiderPathLocator.RiderInfo(a, true))).ToList<RiderPathLocator.RiderInfo>());
        FileInfo fileInfo = new FileInfo(Path.Combine(environmentVariable, ".local/share/applications/jetbrains-rider.desktop"));
        if (fileInfo.Exists)
        {
          foreach (string readAllLine in File.ReadAllLines(fileInfo.FullName))
          {
            if (readAllLine.StartsWith("Exec=\""))
            {
              string path = ((IEnumerable<string>) readAllLine.Split('"', StringSplitOptions.None)).Where<string>((Func<string, int, bool>) ((item, index) => index == 1)).SingleOrDefault<string>();
              if (!string.IsNullOrEmpty(path) && !source.Any<RiderPathLocator.RiderInfo>((Func<RiderPathLocator.RiderInfo, bool>) (a => a.Path == path)))
                source.Add(new RiderPathLocator.RiderInfo(path, false));
            }
          }
        }
      }
      string str = "/snap/rider/current/bin/rider.sh";
      if (new FileInfo(str).Exists)
        source.Add(new RiderPathLocator.RiderInfo(str, false));
      return source.ToArray();
    }

    private static RiderPathLocator.RiderInfo[] CollectRiderInfosMac()
    {
      List<RiderPathLocator.RiderInfo> riderInfoList = new List<RiderPathLocator.RiderInfo>();
      DirectoryInfo directoryInfo = new DirectoryInfo("/Applications");
      if (directoryInfo.Exists)
        riderInfoList.AddRange((IEnumerable<RiderPathLocator.RiderInfo>) ((IEnumerable<DirectoryInfo>) directoryInfo.GetDirectories("*Rider*.app")).Select<DirectoryInfo, RiderPathLocator.RiderInfo>((Func<DirectoryInfo, RiderPathLocator.RiderInfo>) (a => new RiderPathLocator.RiderInfo(a.FullName, false))).ToList<RiderPathLocator.RiderInfo>());
      IEnumerable<RiderPathLocator.RiderInfo> collection = ((IEnumerable<string>) RiderPathLocator.CollectPathsFromToolbox(RiderPathLocator.GetToolboxBaseDir(), "", "Rider*.app", true)).Select<string, RiderPathLocator.RiderInfo>((Func<string, RiderPathLocator.RiderInfo>) (a => new RiderPathLocator.RiderInfo(a, true)));
      riderInfoList.AddRange(collection);
      return riderInfoList.ToArray();
    }

    private static RiderPathLocator.RiderInfo[] CollectRiderInfosWindows()
    {
      List<RiderPathLocator.RiderInfo> riderInfoList = new List<RiderPathLocator.RiderInfo>();
      riderInfoList.AddRange((IEnumerable<RiderPathLocator.RiderInfo>) ((IEnumerable<string>) RiderPathLocator.CollectPathsFromToolbox(RiderPathLocator.GetToolboxBaseDir(), "bin", "rider64.exe", false)).ToList<string>().Select<string, RiderPathLocator.RiderInfo>((Func<string, RiderPathLocator.RiderInfo>) (a => new RiderPathLocator.RiderInfo(a, true))).ToList<RiderPathLocator.RiderInfo>());
      List<string> stringList = new List<string>();
      RiderPathLocator.CollectPathsFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", stringList);
      RiderPathLocator.CollectPathsFromRegistry("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", stringList);
      riderInfoList.AddRange((IEnumerable<RiderPathLocator.RiderInfo>) stringList.Select<string, RiderPathLocator.RiderInfo>((Func<string, RiderPathLocator.RiderInfo>) (a => new RiderPathLocator.RiderInfo(a, false))).ToList<RiderPathLocator.RiderInfo>());
      return riderInfoList.ToArray();
    }

    private static string GetToolboxBaseDir()
    {
      switch (PlatformInfo.operatingSystemFamily)
      {
        case OperatingSystemFamily.MacOSX:
          string environmentVariable1 = Environment.GetEnvironmentVariable("HOME");
          if (!string.IsNullOrEmpty(environmentVariable1))
            return RiderPathLocator.GetToolboxRiderRootPath(Path.Combine(environmentVariable1, "Library/Application Support"));
          break;
        case OperatingSystemFamily.Windows:
          return RiderPathLocator.GetToolboxRiderRootPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        case OperatingSystemFamily.Linux:
          string environmentVariable2 = Environment.GetEnvironmentVariable("HOME");
          if (!string.IsNullOrEmpty(environmentVariable2))
            return RiderPathLocator.GetToolboxRiderRootPath(Path.Combine(environmentVariable2, ".local/share"));
          break;
      }
      return string.Empty;
    }

    private static string GetToolboxRiderRootPath(string localAppData)
    {
      string path1 = Path.Combine(localAppData, "JetBrains/Toolbox");
      string path = Path.Combine(path1, ".settings.json");
      if (File.Exists(path))
      {
        string locationFromJson = RiderPathLocator.SettingsJson.GetInstallLocationFromJson(File.ReadAllText(path));
        if (!string.IsNullOrEmpty(locationFromJson))
          path1 = locationFromJson;
      }
      return Path.Combine(path1, "apps/Rider");
    }

    internal static RiderPathLocator.ProductInfo GetBuildVersion(string path)
    {
      string directoryName = new FileInfo(Path.Combine(path, RiderPathLocator.GetRelativePathToBuildTxt())).DirectoryName;
      if (!Directory.Exists(directoryName))
        return (RiderPathLocator.ProductInfo) null;
      FileInfo fileInfo = new FileInfo(Path.Combine(directoryName, "product-info.json"));
      return !fileInfo.Exists ? (RiderPathLocator.ProductInfo) null : RiderPathLocator.ProductInfo.GetProductInfo(File.ReadAllText(fileInfo.FullName));
    }

    internal static System.Version GetBuildNumber(string path)
    {
      FileInfo fileInfo = new FileInfo(Path.Combine(path, RiderPathLocator.GetRelativePathToBuildTxt()));
      if (!fileInfo.Exists)
        return (System.Version) null;
      string str = File.ReadAllText(fileInfo.FullName);
      int startIndex = str.IndexOf("-", StringComparison.Ordinal) + 1;
      if (startIndex <= 0)
        return (System.Version) null;
      System.Version result;
      return !System.Version.TryParse(str.Substring(startIndex), out result) ? (System.Version) null : result;
    }

    internal static bool GetIsToolbox(string path)
    {
      return Path.GetFullPath(path).StartsWith(Path.GetFullPath(RiderPathLocator.GetToolboxBaseDir()));
    }

    private static string GetRelativePathToBuildTxt()
    {
      switch (PlatformInfo.operatingSystemFamily)
      {
        case OperatingSystemFamily.MacOSX:
          return "Contents/Resources/build.txt";
        case OperatingSystemFamily.Windows:
        case OperatingSystemFamily.Linux:
          return "../../build.txt";
        default:
          throw new Exception("Unknown OS");
      }
    }

    private static void CollectPathsFromRegistry(string registryKey, List<string> installPaths)
    {
      using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey))
        RiderPathLocator.CollectPathsFromRegistry(installPaths, key);
      using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
        RiderPathLocator.CollectPathsFromRegistry(installPaths, key);
    }

    private static void CollectPathsFromRegistry(List<string> installPaths, RegistryKey key)
    {
      if (key == null)
        return;
      foreach (string subKeyName in key.GetSubKeyNames())
      {
        using (RegistryKey registryKey = key.OpenSubKey(subKeyName))
        {
          object obj1 = registryKey?.GetValue("InstallLocation");
          if (obj1 != null)
          {
            string path1 = obj1.ToString();
            if (path1.Length != 0)
            {
              object obj2 = registryKey.GetValue("DisplayName");
              if (obj2 != null)
              {
                if (obj2.ToString().Contains("Rider"))
                {
                  try
                  {
                    string path = Path.Combine(path1, "bin\\rider64.exe");
                    if (File.Exists(path))
                      installPaths.Add(path);
                  }
                  catch (ArgumentException ex)
                  {
                  }
                }
              }
            }
          }
        }
      }
    }

    private static string[] CollectPathsFromToolbox(
      string toolboxRiderRootPath,
      string dirName,
      string searchPattern,
      bool isMac)
    {
      return !Directory.Exists(toolboxRiderRootPath) ? new string[0] : ((IEnumerable<string>) Directory.GetDirectories(toolboxRiderRootPath)).SelectMany<string, string>((Func<string, IEnumerable<string>>) (channelDir =>
      {
        try
        {
          string path1 = Path.Combine(channelDir, ".history.json");
          if (File.Exists(path1))
          {
            string latestBuildFromJson = RiderPathLocator.ToolboxHistory.GetLatestBuildFromJson(File.ReadAllText(path1));
            if (latestBuildFromJson != null)
            {
              string[] executablePaths = RiderPathLocator.GetExecutablePaths(dirName, searchPattern, isMac, Path.Combine(channelDir, latestBuildFromJson));
              if (((IEnumerable<string>) executablePaths).Any<string>())
                return (IEnumerable<string>) executablePaths;
            }
          }
          string path2 = Path.Combine(channelDir, ".channel.settings.json");
          if (File.Exists(path2))
          {
            string latestBuildFromJson = RiderPathLocator.ToolboxInstallData.GetLatestBuildFromJson(File.ReadAllText(path2).Replace("active-application", "active_application"));
            if (latestBuildFromJson != null)
            {
              string[] executablePaths = RiderPathLocator.GetExecutablePaths(dirName, searchPattern, isMac, Path.Combine(channelDir, latestBuildFromJson));
              if (((IEnumerable<string>) executablePaths).Any<string>())
                return (IEnumerable<string>) executablePaths;
            }
          }
          return ((IEnumerable<string>) Directory.GetDirectories(channelDir)).SelectMany<string, string>((Func<string, IEnumerable<string>>) (buildDir => (IEnumerable<string>) RiderPathLocator.GetExecutablePaths(dirName, searchPattern, isMac, buildDir)));
        }
        catch (Exception ex)
        {
          RiderPathLocator.Logger.Warn("Failed to get RiderPath from " + channelDir, ex);
        }
        return (IEnumerable<string>) new string[0];
      })).Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c))).ToArray<string>();
    }

    private static string[] GetExecutablePaths(
      string dirName,
      string searchPattern,
      bool isMac,
      string buildDir)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(buildDir, dirName));
      if (!directoryInfo.Exists)
        return new string[0];
      if (isMac)
        return ((IEnumerable<DirectoryInfo>) directoryInfo.GetDirectories(searchPattern)).Select<DirectoryInfo, string>((Func<DirectoryInfo, string>) (f => f.FullName)).Where<string>(new Func<string, bool>(Directory.Exists)).ToArray<string>();
      return ((IEnumerable<string>) new string[1]
      {
        Path.Combine(directoryInfo.FullName, searchPattern)
      }).Where<string>(new Func<string, bool>(File.Exists)).ToArray<string>();
    }

    [Serializable]
    private class SettingsJson
    {
      public string install_location;

      [CanBeNull]
      public static string GetInstallLocationFromJson(string json)
      {
        try
        {
          return JsonUtility.FromJson<RiderPathLocator.SettingsJson>(json).install_location;
        }
        catch (Exception ex)
        {
          RiderPathLocator.Logger.Warn("Failed to get install_location from json " + json);
        }
        return (string) null;
      }
    }

    [Serializable]
    private class ToolboxHistory
    {
      public List<RiderPathLocator.ItemNode> history;

      [CanBeNull]
      public static string GetLatestBuildFromJson(string json)
      {
        try
        {
          return JsonUtility.FromJson<RiderPathLocator.ToolboxHistory>(json).history.LastOrDefault<RiderPathLocator.ItemNode>()?.item.build;
        }
        catch (Exception ex)
        {
          RiderPathLocator.Logger.Warn("Failed to get latest build from json " + json);
        }
        return (string) null;
      }
    }

    [Serializable]
    private class ItemNode
    {
      public RiderPathLocator.BuildNode item;
    }

    [Serializable]
    private class BuildNode
    {
      public string build;
    }

    [Serializable]
    internal class ProductInfo
    {
      public string version;
      public string versionSuffix;

      [CanBeNull]
      internal static RiderPathLocator.ProductInfo GetProductInfo(string json)
      {
        try
        {
          return JsonUtility.FromJson<RiderPathLocator.ProductInfo>(json);
        }
        catch (Exception ex)
        {
          RiderPathLocator.Logger.Warn("Failed to get version from json " + json);
        }
        return (RiderPathLocator.ProductInfo) null;
      }
    }

    [Serializable]
    private class ToolboxInstallData
    {
      public RiderPathLocator.ActiveApplication active_application;

      [CanBeNull]
      public static string GetLatestBuildFromJson(string json)
      {
        try
        {
          List<string> builds = JsonUtility.FromJson<RiderPathLocator.ToolboxInstallData>(json).active_application.builds;
          if (builds != null)
          {
            if (builds.Any<string>())
              return builds.First<string>();
          }
        }
        catch (Exception ex)
        {
          RiderPathLocator.Logger.Warn("Failed to get latest build from json " + json);
        }
        return (string) null;
      }
    }

    [Serializable]
    private class ActiveApplication
    {
      public List<string> builds;
    }

    internal struct RiderInfo
    {
      public bool IsToolbox;
      public string Presentation;
      public System.Version BuildNumber;
      public RiderPathLocator.ProductInfo ProductInfo;
      public string Path;

      public RiderInfo(string path, bool isToolbox)
      {
        this.BuildNumber = RiderPathLocator.GetBuildNumber(path);
        this.ProductInfo = RiderPathLocator.GetBuildVersion(path);
        this.Path = new FileInfo(path).FullName;
        string str = string.Format("Rider {0}", (object) this.BuildNumber);
        if (this.ProductInfo != null && !string.IsNullOrEmpty(this.ProductInfo.version))
          str = "Rider " + this.ProductInfo.version + (string.IsNullOrEmpty(this.ProductInfo.versionSuffix) ? "" : " " + this.ProductInfo.versionSuffix);
        if (isToolbox)
          str += " (JetBrains Toolbox)";
        this.Presentation = str;
        this.IsToolbox = isToolbox;
      }
    }

    private static class Logger
    {
      internal static void Warn(string message, Exception e = null)
      {
        Debug.LogError((object) message);
        if (e == null)
          return;
        Debug.LogException(e);
      }
    }
  }
}
