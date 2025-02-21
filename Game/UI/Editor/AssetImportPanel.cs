// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.AssetImportPanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline;
using Colossal.AssetPipeline.Collectors;
using Colossal.AssetPipeline.Diagnostic;
using Colossal.AssetPipeline.Importers;
using Colossal.AssetPipeline.PostProcessors;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game.AssetPipeline;
using Game.Prefabs;
using Game.Reflection;
using Game.Settings;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class AssetImportPanel : EditorPanelSystemBase
  {
    private string m_SelectedProjectRoot;
    private string m_SelectedDirectory;
    private bool m_ProjectRootSelected;
    private DirectoryPickerButton m_OpenProjectRootButton;
    private DirectoryPickerButton m_OpenSelectedAssetPathButton;
    private List<FileItem> m_Assets;
    private List<FileItem> m_CachedAssets;
    private FilePickerAdapter m_Adapter;
    private ItemPicker<FileItem> m_AssetList;
    private ItemPickerFooter m_ItemPickerFooter;
    private PrefabBase m_SelectedTemplate;
    private Button m_ImportButton;
    private bool m_Importing;

    private bool importing
    {
      get => this.m_Importing;
      set
      {
        this.m_Importing = value;
        this.m_ImportButton.displayName = (LocalizedString) (value ? "Editor.IMPORTING" : "Editor.IMPORT");
      }
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      this.activeSubPanel = (IEditorPanel) null;
      this.m_Assets = this.GetAssets().ToList<FileItem>();
      this.m_Adapter = new FilePickerAdapter((IEnumerable<FileItem>) this.m_Assets);
      this.m_Adapter.EventItemSelected += new Action<FileItem>(this.OnAssetSelected);
      DirectoryPickerButton directoryPickerButton1 = new DirectoryPickerButton();
      directoryPickerButton1.displayName = (LocalizedString) "Editor.PROJECT_ROOT";
      directoryPickerButton1.action = new Action(this.OpenDirectory);
      directoryPickerButton1.tooltip = new LocalizedString?((LocalizedString) "Editor.PROJECT_ROOT_TOOLTIP");
      directoryPickerButton1.uiTag = "UITagPrefab:SelectProjectRoot";
      this.m_OpenProjectRootButton = directoryPickerButton1;
      DirectoryPickerButton directoryPickerButton2 = new DirectoryPickerButton();
      directoryPickerButton2.displayName = (LocalizedString) "Editor.SELECTED_ASSETS";
      directoryPickerButton2.action = new Action(this.OpenAssetSubDirectory);
      directoryPickerButton2.disabled = new Func<bool>(this.IsSelectedAssetFolderDisabled);
      directoryPickerButton2.tooltip = new LocalizedString?((LocalizedString) "Editor.SELECTED_ASSETS_TOOLTIP");
      directoryPickerButton2.uiTag = "UITagPrefab:SelectAssets";
      this.m_OpenSelectedAssetPathButton = directoryPickerButton2;
      this.m_AssetList = new ItemPicker<FileItem>()
      {
        adapter = (ItemPicker<FileItem>.IAdapter) this.m_Adapter
      };
      this.m_ItemPickerFooter = new ItemPickerFooter()
      {
        adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
      };
      this.title = (LocalizedString) "Editor.TOOL[AssetImportTool]";
      IWidget[] widgetArray = new IWidget[7]
      {
        (IWidget) this.m_OpenProjectRootButton,
        (IWidget) this.m_OpenSelectedAssetPathButton,
        (IWidget) this.m_AssetList,
        (IWidget) this.m_ItemPickerFooter,
        null,
        null,
        null
      };
      PopupValueField<PrefabBase> popupValueField = new PopupValueField<PrefabBase>();
      popupValueField.displayName = (LocalizedString) "Editor.SELECT_TEMPLATE";
      popupValueField.uiTag = "UITagPrefab:SelectTemplate";
      popupValueField.accessor = (ITypedValueAccessor<PrefabBase>) new DelegateAccessor<PrefabBase>((Func<PrefabBase>) (() => this.m_SelectedTemplate), (Action<PrefabBase>) (prefab => this.m_SelectedTemplate = prefab));
      popupValueField.disabled = new Func<bool>(this.IsImportDisabled);
      popupValueField.popup = (IValueFieldPopup<PrefabBase>) new PrefabPickerPopup(typeof (ObjectGeometryPrefab))
      {
        nullable = true
      };
      widgetArray[4] = (IWidget) popupValueField;
      Button button1 = new Button();
      button1.displayName = (LocalizedString) "Editor.IMPORT";
      button1.action = new Action(this.ImportAssets);
      button1.disabled = new Func<bool>(this.IsImportDisabled);
      button1.tooltip = new LocalizedString?((LocalizedString) "Import selected assets");
      button1.uiTag = "UITagPrefab:ImportButton";
      Button button2 = button1;
      this.m_ImportButton = button1;
      widgetArray[5] = (IWidget) button2;
      widgetArray[6] = (IWidget) new ImageField()
      {
        m_URI = "Media/Menu/InstaLOD-Logo-BW-WhiteOnBlack.svg",
        m_Label = (LocalizedString) "Editor.INSTALOD_LABEL"
      };
      this.children = (IList<IWidget>) widgetArray;
      EditorSettings editor = SharedSettings.instance?.editor;
      this.m_SelectedProjectRoot = editor?.lastSelectedProjectRootDirectory;
      this.m_SelectedDirectory = editor?.lastSelectedImportDirectory;
      try
      {
        if (string.IsNullOrEmpty(this.m_SelectedProjectRoot))
          return;
        this.OnSelectProjectRoot(this.m_SelectedProjectRoot + "/");
        if (string.IsNullOrEmpty(this.m_SelectedDirectory))
          return;
        this.OnSelectDirectory(this.m_SelectedDirectory + "/");
      }
      catch (Exception ex)
      {
        this.log.Error(ex, (object) ("Exception occured while trying to select project root or import directory " + this.m_SelectedProjectRoot + ", " + this.m_SelectedDirectory));
        this.m_SelectedProjectRoot = string.Empty;
        this.m_SelectedDirectory = string.Empty;
        this.OnSelectProjectRoot(this.m_SelectedProjectRoot + "/");
        this.OnSelectDirectory(this.m_SelectedDirectory + "/");
      }
    }

    private void OnAssetSelected(FileItem item)
    {
      UnityEngine.Debug.Log((object) ("Asset selected " + item.displayName.ToString()));
    }

    private void OpenDirectory()
    {
      this.activeSubPanel = (IEditorPanel) new DirectoryBrowserPanel(this.m_SelectedProjectRoot, (string) null, new DirectoryBrowserPanel.SelectCallback(this.OnSelectProjectRoot), new Action(this.CloseDirectoryBrowser));
    }

    private void OpenAssetSubDirectory()
    {
      if (string.IsNullOrEmpty(this.m_SelectedDirectory) || !this.m_SelectedDirectory.StartsWith(this.m_SelectedProjectRoot))
        this.m_SelectedDirectory = this.m_SelectedProjectRoot;
      this.activeSubPanel = (IEditorPanel) new DirectoryBrowserPanel(this.m_SelectedDirectory, this.m_SelectedProjectRoot, new DirectoryBrowserPanel.SelectCallback(this.OnSelectDirectory), new Action(this.CloseDirectoryBrowser));
    }

    private void CloseDirectoryBrowser() => this.CloseSubPanel();

    private void OnLoadAsset(Guid guid) => this.CloseSubPanel();

    private void OnSelectProjectRoot(string directory)
    {
      this.CloseSubPanel();
      string str = directory.Remove(directory.Length - 1);
      if (str != this.m_SelectedProjectRoot)
      {
        this.m_SelectedDirectory = (string) null;
        this.m_OpenSelectedAssetPathButton.displayValue = "";
        this.m_OpenSelectedAssetPathButton.tooltip = new LocalizedString?((LocalizedString) "Select asset folder");
      }
      this.m_OpenProjectRootButton.displayValue = str.LastIndexOf('/') == -1 ? str : ".." + str.Substring(str.LastIndexOf('/')) + "/";
      this.m_OpenProjectRootButton.tooltip = new LocalizedString?((LocalizedString) str);
      this.m_SelectedProjectRoot = str;
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
      {
        editor.lastSelectedProjectRootDirectory = this.m_SelectedProjectRoot;
        editor.ApplyAndSave();
      }
      this.m_Assets.Clear();
      this.m_Adapter = new FilePickerAdapter((IEnumerable<FileItem>) this.m_Assets);
      this.m_Adapter.EventItemSelected += new Action<FileItem>(this.OnAssetSelected);
      this.m_AssetList.adapter = (ItemPicker<FileItem>.IAdapter) this.m_Adapter;
      this.m_ItemPickerFooter.adapter = (ItemPickerFooter.IAdapter) this.m_Adapter;
      this.m_ProjectRootSelected = true;
    }

    private void OnSelectDirectory(string directory)
    {
      this.CloseSubPanel();
      string str = directory.Remove(directory.Length - 1);
      this.m_OpenSelectedAssetPathButton.displayValue = str.LastIndexOf('/') == -1 ? str : ".." + str.Substring(str.LastIndexOf('/')) + "/";
      this.m_OpenSelectedAssetPathButton.tooltip = new LocalizedString?((LocalizedString) str);
      this.m_SelectedDirectory = str;
      EditorSettings editor = SharedSettings.instance?.editor;
      if (editor != null)
      {
        editor.lastSelectedImportDirectory = this.m_SelectedDirectory;
        editor.ApplyAndSave();
      }
      this.m_Assets = this.GetAssets().ToList<FileItem>();
      this.m_Adapter = new FilePickerAdapter((IEnumerable<FileItem>) this.m_Assets);
      this.m_Adapter.EventItemSelected += new Action<FileItem>(this.OnAssetSelected);
      this.m_AssetList.adapter = (ItemPicker<FileItem>.IAdapter) this.m_Adapter;
      this.m_ItemPickerFooter.adapter = (ItemPickerFooter.IAdapter) this.m_Adapter;
    }

    private IEnumerable<FileItem> GetAssets()
    {
      if (this.m_SelectedDirectory != null)
      {
        PostProcessorCache.CachePostProcessors();
        ImporterCache.CacheSupportedExtensions();
        Report report1 = new Report();
        string selectedProjectRoot = this.m_SelectedProjectRoot;
        string[] assetPaths = new string[1]
        {
          this.m_SelectedDirectory
        };
        Report report2 = report1;
        foreach (KeyValuePair<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings> keyValuePair in (IEnumerable<KeyValuePair<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings>>) AssetImportPipeline.CollectDataToImport(selectedProjectRoot, assetPaths, report2))
        {
          foreach (SourceAssetCollector.Asset asset1 in keyValuePair.Key)
          {
            FileItem asset2 = new FileItem();
            asset2.path = asset1.path;
            asset2.displayName = (LocalizedString) asset1.name;
            asset2.tooltip = new LocalizedString?((LocalizedString) asset1.path);
            yield return asset2;
          }
        }
      }
    }

    private bool IsImportDisabled() => this.importing || !this.m_Assets.Any<FileItem>();

    private bool IsSelectedAssetFolderDisabled()
    {
      return string.IsNullOrEmpty(this.m_SelectedProjectRoot) || !this.m_ProjectRootSelected;
    }

    private static bool ReportProgress(string title, string info, float progress)
    {
      if ((double) progress == 1.0)
        UnityEngine.Debug.Log((object) "Import completed");
      return false;
    }

    private async void ImportAssets()
    {
      AssetImportPanel assetImportPanel = this;
      try
      {
        assetImportPanel.importing = true;
        AssetImportPanel.PrefabFactory prefabFactory = await AssetImportPanel.ImportAssets(assetImportPanel.m_SelectedProjectRoot, assetImportPanel.m_SelectedDirectory, assetImportPanel.m_SelectedTemplate, assetImportPanel.World, assetImportPanel.log);
      }
      finally
      {
        assetImportPanel.importing = false;
      }
    }

    public static async System.Threading.Tasks.Task<AssetImportPanel.PrefabFactory> ImportAssets(
      string selectedProjectRoot,
      string selectedDirectory,
      PrefabBase selectedTemplate,
      World world,
      ILog log)
    {
      AssetImportPanel.PrefabFactory prefabFactory = (AssetImportPanel.PrefabFactory) null;
      try
      {
        EditorSettings editor = SharedSettings.instance?.editor;
        if (selectedProjectRoot == null)
          throw new Exception("The path must contains ProjectFiles to act as the root folder of the art assets");
        string artProjectPath;
        List<string> artProjectRelativePaths;
        if (AssetImportPipeline.IsArtRootPath(selectedProjectRoot, new string[1]
        {
          selectedDirectory
        }, out artProjectPath, out artProjectRelativePaths))
        {
          prefabFactory = new AssetImportPanel.PrefabFactory();
          AssetImportPipeline.useParallelImport = editor == null || editor.useParallelImport;
          AssetImportPipeline.targetDatabase = Colossal.IO.AssetDatabase.AssetDatabase.user;
          TextureImporter.overrideCompressionEffort = (editor != null ? (editor.lowQualityTextureCompression ? 1 : 0) : 1) != 0 ? 0 : -1;
          await AssetImportPipeline.ImportPath(artProjectPath, (IEnumerable<string>) artProjectRelativePaths, ImportMode.All, false, new Func<string, string, float, bool>(AssetImportPanel.ReportProgress), (IPrefabFactory) prefabFactory);
          // ISSUE: variable of a compiler-generated type
          PrefabSystem systemManaged1 = world.GetOrCreateSystemManaged<PrefabSystem>();
          // ISSUE: variable of a compiler-generated type
          ToolSystem systemManaged2 = world.GetOrCreateSystemManaged<ToolSystem>();
          foreach ((PrefabBase, string) rootPrefab in (IEnumerable<(PrefabBase, string)>) prefabFactory.rootPrefabs)
          {
            log.InfoFormat("Root prefab: {0} ({1})", (object) rootPrefab.Item1.name, (object) rootPrefab.Item2);
            string fileName = Path.GetFileName(rootPrefab.Item2);
            string subPath = "StreamingData~/" + fileName;
            if ((UnityEngine.Object) selectedTemplate != (UnityEngine.Object) null)
            {
              PrefabBase prefab = selectedTemplate.Clone(fileName);
              if (prefab is ObjectGeometryPrefab objectGeometryPrefab)
              {
                ObjectMeshInfo[] objectMeshInfoArray = new ObjectMeshInfo[1]
                {
                  new ObjectMeshInfo()
                  {
                    m_Mesh = rootPrefab.Item1 as RenderPrefabBase
                  }
                };
                objectGeometryPrefab.m_Meshes = objectMeshInfoArray;
              }
              prefab.Remove<ObjectSubObjects>();
              prefab.Remove<ObjectSubAreas>();
              prefab.Remove<ObjectSubLanes>();
              prefab.Remove<ObjectSubNets>();
              prefab.Remove<NetSubObjects>();
              prefab.Remove<AreaSubObjects>();
              prefab.Remove<EffectSource>();
              prefab.Remove<ObsoleteIdentifiers>();
              AssetImportPipeline.targetDatabase.AddAsset(AssetDataPath.Create(subPath, prefab.name ?? ""), (ScriptableObject) prefab).Save(false);
              // ISSUE: reference to a compiler-generated method
              systemManaged1.AddPrefab(prefab);
              // ISSUE: reference to a compiler-generated method
              systemManaged2.ActivatePrefabTool(prefab);
            }
            else
            {
              if ((Colossal.IO.AssetDatabase.AssetData) rootPrefab.Item1.asset == (IAssetData) null)
                AssetImportPipeline.targetDatabase.AddAsset(AssetDataPath.Create(subPath, rootPrefab.Item1.name ?? ""), (ScriptableObject) rootPrefab.Item1);
              rootPrefab.Item1.asset.Save(true);
              // ISSUE: reference to a compiler-generated method
              systemManaged1.AddPrefab(rootPrefab.Item1);
            }
          }
        }
        else
          log.Error((object) "The path must contains ProjectFiles to act as the root folder of the art assets");
      }
      catch (Exception ex)
      {
        log.Error(ex);
      }
      AssetImportPanel.PrefabFactory prefabFactory1 = prefabFactory;
      prefabFactory = (AssetImportPanel.PrefabFactory) null;
      return prefabFactory1;
    }

    [Preserve]
    public AssetImportPanel()
    {
    }

    public class PrefabFactory : IPrefabFactory
    {
      private readonly List<(PrefabBase prefab, string source)> m_RootPrefabs = new List<(PrefabBase, string)>();
      private readonly List<PrefabBase> m_CreatedPrefabs = new List<PrefabBase>();

      public IReadOnlyList<(PrefabBase prefab, string source)> rootPrefabs
      {
        get => (IReadOnlyList<(PrefabBase, string)>) this.m_RootPrefabs;
      }

      public IReadOnlyList<PrefabBase> prefabs => (IReadOnlyList<PrefabBase>) this.m_CreatedPrefabs;

      public T CreatePrefab<T>(string sourcePath, string rootMeshName, int lodLevel) where T : PrefabBase
      {
        T asset = this.LoadOrCreateAsset<T>(sourcePath, rootMeshName);
        asset.name = rootMeshName;
        if (lodLevel == 0)
          this.m_RootPrefabs.Add(((PrefabBase) asset, sourcePath));
        this.m_CreatedPrefabs.Add((PrefabBase) asset);
        return asset;
      }

      private T LoadOrCreateAsset<T>(string sourcePath, string rootMeshName) where T : PrefabBase
      {
        string subPath = "StreamingData~/" + sourcePath + "/" + Path.GetFileName(rootMeshName);
        PrefabAsset asset1;
        return Colossal.IO.AssetDatabase.AssetDatabase.user.TryGetAsset<PrefabAsset>(SearchFilter<PrefabAsset>.ByCondition((Func<PrefabAsset, bool>) (asset => AssetImportPanel.PrefabFactory.PathCompare(subPath, asset.subPath + "/" + asset.name))), out asset1) && asset1.Load() is T obj ? obj : ScriptableObject.CreateInstance<T>();
      }

      private static bool PathCompare(string subPath1, string subPath2)
      {
        return string.Compare(Path.GetFullPath(subPath1).TrimEnd('\\'), Path.GetFullPath(subPath2).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) == 0;
      }
    }
  }
}
