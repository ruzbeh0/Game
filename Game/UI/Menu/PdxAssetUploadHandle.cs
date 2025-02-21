// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.PdxAssetUploadHandle
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Diagnostic;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Game.AssetPipeline;
using Game.PSI;
using Game.SceneFlow;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Entities;

#nullable disable
namespace Game.UI.Menu
{
  public class PdxAssetUploadHandle
  {
    private ILog log = LogManager.GetLogger("AssetUpload");
    private PdxSdkPlatform m_Manager = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
    private List<Colossal.IO.AssetDatabase.AssetData> m_Screenshots = new List<Colossal.IO.AssetDatabase.AssetData>();
    private List<Colossal.IO.AssetDatabase.AssetData> m_Assets = new List<Colossal.IO.AssetDatabase.AssetData>();
    private List<Colossal.IO.AssetDatabase.AssetData> m_OriginalPreviews = new List<Colossal.IO.AssetDatabase.AssetData>();
    private System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.AssetData, Colossal.IO.AssetDatabase.AssetData> m_WIPAssets = new System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.AssetData, Colossal.IO.AssetDatabase.AssetData>();
    private List<Colossal.IO.AssetDatabase.AssetData> m_AdditionalAssets = new List<Colossal.IO.AssetDatabase.AssetData>();
    private HashSet<Colossal.IO.AssetDatabase.AssetData> m_CachedAssetDependencies = new HashSet<Colossal.IO.AssetDatabase.AssetData>();
    public Action onSocialProfileSynced;

    public Colossal.IO.AssetDatabase.AssetData mainAsset { get; private set; }

    public IReadOnlyList<Colossal.IO.AssetDatabase.AssetData> assets
    {
      get => (IReadOnlyList<Colossal.IO.AssetDatabase.AssetData>) this.m_Assets;
    }

    public IReadOnlyList<Colossal.IO.AssetDatabase.AssetData> additionalAssets
    {
      get => (IReadOnlyList<Colossal.IO.AssetDatabase.AssetData>) this.m_AdditionalAssets;
    }

    public HashSet<Colossal.IO.AssetDatabase.AssetData> cachedDependencies
    {
      get => this.m_CachedAssetDependencies;
    }

    public bool hasPrefabAssets { get; private set; }

    public IEnumerable<Colossal.IO.AssetDatabase.AssetData> allAssets
    {
      get
      {
        foreach (Colossal.IO.AssetDatabase.AssetData asset in (IEnumerable<Colossal.IO.AssetDatabase.AssetData>) this.assets)
          yield return asset;
        foreach (Colossal.IO.AssetDatabase.AssetData additionalAsset in (IEnumerable<Colossal.IO.AssetDatabase.AssetData>) this.additionalAssets)
          yield return additionalAsset;
      }
    }

    public IReadOnlyList<Colossal.IO.AssetDatabase.AssetData> screenshots
    {
      get => (IReadOnlyList<Colossal.IO.AssetDatabase.AssetData>) this.m_Screenshots;
    }

    public Colossal.IO.AssetDatabase.AssetData preview { get; private set; }

    public IReadOnlyList<Colossal.IO.AssetDatabase.AssetData> originalPreviews
    {
      get => (IReadOnlyList<Colossal.IO.AssetDatabase.AssetData>) this.m_OriginalPreviews;
    }

    public IModsUploadSupport.ModInfo modInfo { get; set; }

    public bool updateExisting { get; set; }

    public int processVT { get; set; } = -1;

    public bool packThumbnailsAtlas { get; set; }

    public List<IModsUploadSupport.ModInfo> authorMods { get; private set; } = new List<IModsUploadSupport.ModInfo>();

    public IModsUploadSupport.ModTag[] availableTags { get; private set; } = Array.Empty<IModsUploadSupport.ModTag>();

    public HashSet<string> typeTags { get; private set; } = new HashSet<string>();

    public HashSet<string> tags { get; private set; } = new HashSet<string>();

    public List<string> additionalTags { get; private set; } = new List<string>();

    public int tagCount => this.tags.Count + this.additionalTags.Count;

    public bool binaryPackAssets { get; set; } = true;

    public IModsUploadSupport.SocialProfile socialProfile { get; private set; }

    public bool LoggedIn()
    {
      PdxSdkPlatform manager = this.m_Manager;
      return manager != null && manager.cachedLoggedIn;
    }

    public PdxAssetUploadHandle() => this.Initialize();

    public PdxAssetUploadHandle(Colossal.IO.AssetDatabase.AssetData mainAsset, params Colossal.IO.AssetDatabase.AssetData[] assets)
    {
      this.mainAsset = mainAsset;
      if (mainAsset != (IAssetData) null)
        this.m_Assets.Add(mainAsset);
      this.m_Assets.AddRange((IEnumerable<Colossal.IO.AssetDatabase.AssetData>) assets);
      this.Initialize();
    }

    private void Initialize()
    {
      this.m_Manager = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      PlatformManager.instance.onPlatformRegistered += (PlatformRegisteredHandler) (psi =>
      {
        if (!(psi is PdxSdkPlatform pdxSdkPlatform2))
          return;
        this.m_Manager = pdxSdkPlatform2;
      });
      this.InitializePreviews();
      IModsUploadSupport.ModInfo modInfo = this.modInfo;
      modInfo.Clear();
      ref IModsUploadSupport.ModInfo local = ref modInfo;
      Colossal.Version current = Game.Version.current;
      // ISSUE: variable of a boxed type
      __Boxed<byte> majorVersion = (ValueType) current.majorVersion;
      current = Game.Version.current;
      // ISSUE: variable of a boxed type
      __Boxed<byte> minorVersion = (ValueType) current.minorVersion;
      string str = string.Format("{0}.{1}.*", (object) majorVersion, (object) minorVersion);
      local.m_RecommendedGameVersion = str;
      modInfo.m_DisplayName = this.mainAsset?.name;
      modInfo.m_ExternalLinks.Add(new IModsUploadSupport.ExternalLinkData()
      {
        m_Type = IModsUploadSupport.ExternalLinkInfo.kAcceptedTypes[0].m_Type,
        m_URL = string.Empty
      });
      this.modInfo = modInfo;
      this.RebuildDependencyCache();
    }

    public async Task<IModsUploadSupport.ModOperationResult> BeginSubmit()
    {
      IModsUploadSupport.ModOperationResult modOperationResult1;
      if (this.updateExisting)
        modOperationResult1 = await this.m_Manager.RegisterExistingWIP(this.modInfo);
      else
        modOperationResult1 = await this.m_Manager.RegisterWIP(this.modInfo);
      IModsUploadSupport.ModOperationResult modOperationResult2 = modOperationResult1;
      this.modInfo = modOperationResult2.m_ModInfo;
      if (!modOperationResult2.m_Success)
        return modOperationResult2;
      HashSet<IModsUploadSupport.ModInfo.ModDependency> modDependencySet = new HashSet<IModsUploadSupport.ModInfo.ModDependency>();
      (bool, string) tuple = this.CopyFiles(modDependencySet);
      int num = tuple.Item1 ? 1 : 0;
      string error = tuple.Item2;
      if (num == 0)
      {
        this.log.Error((object) error);
        await this.Cleanup();
        return new IModsUploadSupport.ModOperationResult()
        {
          m_ModInfo = this.modInfo,
          m_Success = false,
          m_Error = new IModsUploadSupport.ModError()
          {
            m_Details = error
          }
        };
      }
      this.modInfo = this.modInfo with
      {
        m_ModDependencies = modDependencySet.ToArray<IModsUploadSupport.ModInfo.ModDependency>(),
        m_Tags = this.CollectTags()
      };
      IModsUploadSupport.ModOperationResult updateResult = await this.m_Manager.UpdateWIP(this.modInfo);
      this.modInfo = updateResult.m_ModInfo;
      if (!updateResult.m_Success)
        await this.Cleanup();
      return updateResult;
    }

    public async Task<IModsUploadSupport.ModOperationResult> FinalizeSubmit()
    {
      IModsUploadSupport.ModOperationResult modOperationResult1;
      if (this.updateExisting)
        modOperationResult1 = await this.m_Manager.UpdateExisting(this.modInfo);
      else
        modOperationResult1 = await this.m_Manager.PublishWIP(this.modInfo);
      IModsUploadSupport.ModOperationResult publishResult = modOperationResult1;
      this.modInfo = publishResult.m_ModInfo;
      await this.Cleanup();
      IModsUploadSupport.ModOperationResult modOperationResult2 = publishResult;
      publishResult = new IModsUploadSupport.ModOperationResult();
      return modOperationResult2;
    }

    public void ShowModsUIProfilePage()
    {
      this.m_Manager.onModsUIClosed += new Action(this.OnModsUIClosed);
      this.m_Manager.ShowModsUIProfilePage();
    }

    private void OnModsUIClosed()
    {
      this.m_Manager.onModsUIClosed -= new Action(this.OnModsUIClosed);
      this.RefreshSocialProfile();
    }

    private async void RefreshSocialProfile()
    {
      IModsUploadSupport.SocialProfile socialProfileResult = await this.m_Manager.GetSocialProfile();
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        this.socialProfile = socialProfileResult;
        Action socialProfileSynced = this.onSocialProfileSynced;
        if (socialProfileSynced == null)
          return;
        socialProfileSynced();
      }));
    }

    public void ExcludeSourceTextures(
      IEnumerable<SurfaceAsset> surfaces,
      ILocalAssetDatabase database)
    {
      System.Collections.Generic.Dictionary<TextureAsset, List<SurfaceAsset>> references1 = new System.Collections.Generic.Dictionary<TextureAsset, List<SurfaceAsset>>();
      System.Collections.Generic.Dictionary<TextureAsset, List<SurfaceAsset>> references2 = new System.Collections.Generic.Dictionary<TextureAsset, List<SurfaceAsset>>();
      foreach (SurfaceAsset surface in surfaces)
      {
        surface.LoadProperties(true);
        if (surface.isVTMaterial)
        {
          foreach (KeyValuePair<string, TextureAsset> texture in (IEnumerable<KeyValuePair<string, TextureAsset>>) surface.textures)
          {
            if (surface.IsHandledByVirtualTexturing(texture))
              AddReferenceTo(references1, texture.Value, surface);
            else
              AddReferenceTo(references2, texture.Value, surface);
          }
        }
        else
        {
          foreach (KeyValuePair<string, TextureAsset> texture in (IEnumerable<KeyValuePair<string, TextureAsset>>) surface.textures)
            AddReferenceTo(references2, texture.Value, surface);
        }
        surface.Unload(false);
      }
      List<TextureAsset> list = database.GetAssets<TextureAsset>().ToList<TextureAsset>();
      for (int index = 0; index < list.Count; ++index)
      {
        TextureAsset textureAsset = list[index];
        if (references1.ContainsKey(textureAsset))
        {
          if (references2.ContainsKey(textureAsset))
          {
            this.log.WarnFormat("Texture {0} is referenced {1} times by VT materials and {2} times by non VT materials. It will be duplicated on disk.", (object) textureAsset, (object) references1[textureAsset].Count, (object) references2[textureAsset].Count);
            this.log.InfoFormat("Detail for {0}:\nvt: {1}\nnon vt: {2}", (object) textureAsset, (object) string.Join<SurfaceAsset>(", ", (IEnumerable<SurfaceAsset>) references1[textureAsset]), (object) string.Join<SurfaceAsset>(", ", (IEnumerable<SurfaceAsset>) references2[textureAsset]));
          }
          else
          {
            this.log.InfoFormat(string.Format("Deleting {0}", (object) textureAsset));
            textureAsset.Delete();
          }
        }
      }

      static void AddReferenceTo(
        System.Collections.Generic.Dictionary<TextureAsset, List<SurfaceAsset>> references,
        TextureAsset texture,
        SurfaceAsset surface)
      {
        List<SurfaceAsset> surfaceAssetList;
        if (!references.TryGetValue(texture, out surfaceAssetList))
        {
          surfaceAssetList = new List<SurfaceAsset>();
          references.Add(texture, surfaceAssetList);
        }
        surfaceAssetList.Add(surface);
      }
    }

    private (bool, string) CopyFiles(
      HashSet<IModsUploadSupport.ModInfo.ModDependency> externalReferences)
    {
      Directory.CreateDirectory(this.GetAbsoluteContentPath());
      if (this.allAssets.Any<Colossal.IO.AssetDatabase.AssetData>())
      {
        using (ILocalAssetDatabase transient = Colossal.IO.AssetDatabase.AssetDatabase.GetTransient())
        {
          System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.AssetData, Colossal.IO.AssetDatabase.AssetData> processed = new System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.AssetData, Colossal.IO.AssetDatabase.AssetData>();
          foreach (Colossal.IO.AssetDatabase.AssetData allAsset in this.allAssets)
          {
            this.log.VerboseFormat("Copying {0} to {1}. Processed {2} references.", (object) allAsset, (object) transient, (object) processed.Count);
            AssetUploadUtils.CopyAsset(allAsset, transient, processed, externalReferences, allAsset == (IAssetData) this.mainAsset, this.binaryPackAssets, this.modInfo.m_PublishedID);
          }
          if (this.processVT > -1)
          {
            // ISSUE: variable of a compiler-generated type
            SimulationSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystem>();
            float selectedSpeed = existingSystemManaged.selectedSpeed;
            int processVt = this.processVT;
            Report report1 = new Report();
            Report.ImportStep report2 = report1.AddImportStep("Convert Selected VT");
            List<SurfaceAsset> list = transient.GetAssets<SurfaceAsset>().ToList<SurfaceAsset>();
            AssetImportPipeline.ConvertSurfacesToVT((IEnumerable<SurfaceAsset>) list, (IEnumerable<SurfaceAsset>) list, false, 512, 3, processVt, false, report2);
            AssetImportPipeline.BuildMidMipsCache((IEnumerable<SurfaceAsset>) list, 512, 3, transient);
            this.ExcludeSourceTextures((IEnumerable<SurfaceAsset>) list, transient);
            report1.Log(this.log, Colossal.AssetPipeline.Diagnostic.Severity.Verbose);
            existingSystemManaged.selectedSpeed = selectedSpeed;
          }
          if (this.packThumbnailsAtlas)
            AssetUploadUtils.CreateThumbnailAtlas(processed, transient);
          PackageAsset key = Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.AddAsset(AssetDataPath.Create(this.GetContentPath(), this.modInfo.m_DisplayName), transient);
          key.Save(false);
          this.m_WIPAssets.Add((Colossal.IO.AssetDatabase.AssetData) key, (Colossal.IO.AssetDatabase.AssetData) key);
        }
      }
      this.CopyPreview();
      for (int index = 0; index < this.screenshots.Count; ++index)
        this.CopyScreenshot(this.screenshots[index], index);
      return (true, (string) null);
    }

    public async Task Cleanup()
    {
      foreach (KeyValuePair<Colossal.IO.AssetDatabase.AssetData, Colossal.IO.AssetDatabase.AssetData> wipAsset in this.m_WIPAssets)
        Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.DeleteAsset<Colossal.IO.AssetDatabase.AssetData>(wipAsset.Value);
      this.m_WIPAssets.Clear();
      this.modInfo = (await this.m_Manager.UnregisterWIP(this.modInfo)).m_ModInfo;
    }

    public async Task SyncPlatformData()
    {
      Task<List<IModsUploadSupport.ModInfo>> modsTask = this.m_Manager.ListAllModsByMe(this.typeTags.ToArray<string>());
      Task<IModsUploadSupport.ModTag[]> tagsTask = this.m_Manager.GetTags();
      Task<IModsUploadSupport.SocialProfile> socialProfileTask = this.m_Manager.GetSocialProfile();
      await Task.WhenAll((Task) modsTask, (Task) tagsTask, (Task) socialProfileTask);
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        this.authorMods = modsTask.Result;
        this.authorMods?.Sort((Comparison<IModsUploadSupport.ModInfo>) ((a, b) => string.Compare(a.m_DisplayName, b.m_DisplayName, StringComparison.OrdinalIgnoreCase)));
        this.availableTags = tagsTask.Result;
        this.socialProfile = socialProfileTask.Result;
        (HashSet<string>, HashSet<string>) tags = PdxAssetUploadHandle.GetTags(this.mainAsset, new HashSet<string>(((IEnumerable<IModsUploadSupport.ModTag>) this.availableTags).Select<IModsUploadSupport.ModTag, string>((Func<IModsUploadSupport.ModTag, string>) (tag => tag.m_Id))));
        this.typeTags = (this.tags = tags.Item1) = tags.Item2;
      }));
    }

    public async Task<IModsUploadSupport.ModInfo> GetExistingInfo()
    {
      return await this.m_Manager.GetDetails(this.modInfo);
    }

    public async Task<(bool, IModsUploadSupport.ModLocalData)> GetLocalData(int id)
    {
      (bool flag, IModsUploadSupport.ModLocalData modLocalData) = await this.m_Manager.GetLocalData(id);
      if (flag && Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.dataSource is ParadoxModsDataSource dataSource)
        await dataSource.PopulateMetadata(modLocalData.m_AbsolutePath);
      (bool, IModsUploadSupport.ModLocalData) localData = (flag, modLocalData);
      modLocalData = new IModsUploadSupport.ModLocalData();
      return localData;
    }

    private void CopyPreview()
    {
      if (this.preview == (IAssetData) null)
        return;
      Colossal.IO.AssetDatabase.AssetData asset;
      if (!this.m_WIPAssets.TryGetValue(this.preview, out asset))
        asset = AssetUploadUtils.CopyPreviewImage(this.preview, (ILocalAssetDatabase) Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance, this.GetMetadataPath("preview"));
      if (asset == (IAssetData) null)
        return;
      this.m_WIPAssets[this.preview] = asset;
      string filename = this.GetFilename(asset);
      this.modInfo = this.modInfo with
      {
        m_ThumbnailFilename = filename
      };
    }

    public void AddScreenshot(Colossal.IO.AssetDatabase.AssetData asset)
    {
      this.m_Screenshots.Add(asset);
    }

    public void RemoveScreenshot(Colossal.IO.AssetDatabase.AssetData asset)
    {
      Colossal.IO.AssetDatabase.AssetData asset1;
      if (this.m_WIPAssets.TryGetValue(asset, out asset1))
      {
        this.modInfo.m_ScreenshotFileNames.Remove(this.GetFilename(asset1));
        Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.DeleteAsset<Colossal.IO.AssetDatabase.AssetData>(asset1);
        this.m_WIPAssets.Remove(asset);
      }
      this.m_Screenshots.Remove(asset);
    }

    public void ClearScreenshots()
    {
      while (this.m_Screenshots.Count > 0)
        this.RemoveScreenshot(this.m_Screenshots[this.m_Screenshots.Count - 1]);
    }

    public void SetPreview(Colossal.IO.AssetDatabase.AssetData asset)
    {
      Colossal.IO.AssetDatabase.AssetData asset1;
      if (this.preview != (IAssetData) null && this.preview != (IAssetData) asset && this.m_WIPAssets.TryGetValue(this.preview, out asset1))
      {
        Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.DeleteAsset<Colossal.IO.AssetDatabase.AssetData>(asset1);
        this.m_WIPAssets.Remove(this.preview);
      }
      this.preview = asset;
    }

    private void CopyScreenshot(Colossal.IO.AssetDatabase.AssetData asset, int index)
    {
      Colossal.IO.AssetDatabase.AssetData asset1;
      if (!this.m_WIPAssets.TryGetValue(asset, out asset1))
        asset1 = AssetUploadUtils.CopyPreviewImage(asset, (ILocalAssetDatabase) Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance, this.GetMetadataPath(string.Format("screenshot{0}", (object) index)));
      if (asset1 == (IAssetData) null)
        return;
      this.m_WIPAssets[asset] = asset1;
      string filename = this.GetFilename(asset1);
      IModsUploadSupport.ModInfo modInfo = this.modInfo;
      if (!modInfo.m_ScreenshotFileNames.Contains(filename))
        modInfo.m_ScreenshotFileNames.Add(filename);
      this.modInfo = modInfo;
    }

    public void SetPreviewsFromExisting(IModsUploadSupport.ModLocalData localData)
    {
      if (localData.m_ThumbnailFilename != null)
      {
        string thumbnailPath = Path.GetFullPath(Path.Combine(localData.m_AbsolutePath, localData.m_ThumbnailFilename));
        ImageAsset asset;
        if (Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.TryGetAsset<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (candidate => this.FindByPath(candidate, thumbnailPath))), out asset))
          this.SetPreview((Colossal.IO.AssetDatabase.AssetData) asset);
      }
      if (localData.m_ScreenshotFilenames == null)
        return;
      this.ClearScreenshots();
      foreach (string screenshotFilename in localData.m_ScreenshotFilenames)
      {
        string screenshotPath = Path.GetFullPath(Path.Combine(localData.m_AbsolutePath, screenshotFilename));
        ImageAsset asset;
        if (Colossal.IO.AssetDatabase.AssetDatabase<ParadoxMods>.instance.TryGetAsset<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (candidate => this.FindByPath(candidate, screenshotPath))), out asset))
          this.AddScreenshot((Colossal.IO.AssetDatabase.AssetData) asset);
      }
    }

    public void AddAdditionalAsset(Colossal.IO.AssetDatabase.AssetData asset)
    {
      this.m_AdditionalAssets.Add(asset);
      this.RebuildDependencyCache();
    }

    public void RemoveAdditionalAsset(Colossal.IO.AssetDatabase.AssetData asset)
    {
      this.m_AdditionalAssets.Remove(asset);
      this.RebuildDependencyCache();
    }

    private bool FindByPath(ImageAsset candidate, string imagePath)
    {
      string fullPath = Path.GetFullPath(candidate.GetMeta().path);
      return imagePath.Equals(fullPath, StringComparison.OrdinalIgnoreCase);
    }

    private static (HashSet<string>, HashSet<string>) GetTags(
      Colossal.IO.AssetDatabase.AssetData asset,
      HashSet<string> validTags)
    {
      HashSet<string> typeTags = new HashSet<string>();
      HashSet<string> tags = new HashSet<string>();
      if (asset != (IAssetData) null)
        ModTags.GetTags(asset, tags, typeTags, validTags);
      return (tags, typeTags);
    }

    private void InitializePreviews()
    {
      Colossal.IO.AssetDatabase.AssetData result1;
      this.preview = !AssetUploadUtils.TryGetPreview(this.mainAsset, out result1) ? (Colossal.IO.AssetDatabase.AssetData) MenuHelpers.defaultPreview : result1;
      HashSet<Colossal.IO.AssetDatabase.AssetData> collection = new HashSet<Colossal.IO.AssetDatabase.AssetData>();
      foreach (Colossal.IO.AssetDatabase.AssetData asset in (IEnumerable<Colossal.IO.AssetDatabase.AssetData>) this.assets)
      {
        Colossal.IO.AssetDatabase.AssetData result2;
        if (AssetUploadUtils.TryGetPreview(asset, out result2))
          collection.Add(result2);
      }
      this.m_OriginalPreviews.AddRange((IEnumerable<Colossal.IO.AssetDatabase.AssetData>) collection);
      this.m_Screenshots.AddRange((IEnumerable<Colossal.IO.AssetDatabase.AssetData>) collection);
    }

    private string GetFilename(Colossal.IO.AssetDatabase.AssetData asset)
    {
      SourceMeta meta = asset.GetMeta();
      return meta.fileName + meta.mimeType;
    }

    public void AddAdditionalTag(string tag) => this.additionalTags.Add(tag);

    public void RemoveAdditionalTag(string tag) => this.additionalTags.Remove(tag);

    private string[] CollectTags()
    {
      HashSet<string> source = new HashSet<string>((IEnumerable<string>) this.tags);
      foreach (string additionalTag in this.additionalTags)
        source.Add(additionalTag);
      return source.ToArray<string>();
    }

    private AssetDataPath GetMetadataPath(string name)
    {
      return AssetDataPath.Create(this.modInfo.m_RootPath + "/" + IModsUploadSupport.ModInfo.kMetadataDirectory, name);
    }

    private string GetContentPath()
    {
      return this.modInfo.m_RootPath + "/" + IModsUploadSupport.ModInfo.kContentDirectory;
    }

    public string GetAbsoluteContentPath()
    {
      return Path.Combine(this.m_Manager.modsRootPath, this.GetContentPath());
    }

    private void RebuildDependencyCache()
    {
      this.m_CachedAssetDependencies.Clear();
      this.hasPrefabAssets = false;
      foreach (Colossal.IO.AssetDatabase.AssetData allAsset in this.allAssets)
      {
        if (allAsset is PrefabAsset prefabAsset)
        {
          AssetUploadUtils.CollectPrefabAssetDependencies(prefabAsset, this.m_CachedAssetDependencies, allAsset == (IAssetData) this.mainAsset);
          this.hasPrefabAssets = true;
        }
        this.m_CachedAssetDependencies.Add(allAsset);
      }
    }
  }
}
