// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.TerrainPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.AssetPipeline.Native;
using Colossal.IO.AssetDatabase;
using Colossal.UI;
using Game.Common;
using Game.Prefabs;
using Game.Reflection;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class TerrainPanelSystem : EditorPanelSystemBase
  {
    private static readonly string kHeightmapFolder = "Heightmaps";
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_PrefabQuery;
    private IconButtonGroup m_ToolButtonGroup;
    private IconButtonGroup m_MaterialButtonGroup = new IconButtonGroup();
    private List<PrefabBase> m_ToolPrefabs = new List<PrefabBase>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<TerraformingData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.title = (LocalizedString) "Editor.TOOL[TerrainTool]";
      IWidget[] widgetArray1 = new IWidget[1];
      IWidget[] children1 = new IWidget[2];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Editor.TERRAIN_TOOLS";
      editorSection1.uiTag = "UITagPrefab:TerrainTools";
      editorSection1.tooltip = new LocalizedString?((LocalizedString) "Editor.TERRAIN_TOOLS_TOOLTIP");
      editorSection1.expanded = true;
      editorSection1.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_ToolButtonGroup = new IconButtonGroup())
      };
      children1[0] = (IWidget) editorSection1;
      EditorSection editorSection2 = new EditorSection();
      editorSection2.displayName = (LocalizedString) "Editor.HEIGHTMAPS";
      editorSection2.tooltip = new LocalizedString?((LocalizedString) "Editor.HEIGHTMAPS_TOOLTIP");
      editorSection2.uiTag = "UITagPrefab:Heightmaps";
      editorSection2.expanded = true;
      EditorSection editorSection3 = editorSection2;
      IWidget[] widgetArray2 = new IWidget[3];
      FloatInputField floatInputField = new FloatInputField();
      floatInputField.displayName = (LocalizedString) "Editor.HEIGHT_SCALE";
      floatInputField.tooltip = new LocalizedString?((LocalizedString) "Editor.HEIGHT_SCALE_TOOLTIP");
      floatInputField.uiTag = "UITagPrefab:HeightScale";
      floatInputField.accessor = (ITypedValueAccessor<double>) new DelegateAccessor<double>((Func<double>) (() => (double) this.m_TerrainSystem.heightScaleOffset.x), (Action<double>) (val => this.RefreshTerrainProperties(this.m_TerrainSystem.heightScaleOffset with
      {
        x = (float) val
      })));
      floatInputField.min = 200.0;
      floatInputField.max = 10000.0;
      widgetArray2[0] = (IWidget) floatInputField;
      Game.UI.Widgets.Button[] children2 = new Game.UI.Widgets.Button[2];
      Game.UI.Widgets.Button button1 = new Game.UI.Widgets.Button();
      button1.displayName = (LocalizedString) "Editor.IMPORT_HEIGHTMAP";
      button1.tooltip = new LocalizedString?((LocalizedString) "Editor.IMPORT_HEIGHTMAP_TOOLTIP");
      button1.uiTag = "UITagPrefab:ImportHeightmap";
      button1.action = new System.Action(this.ShowImportHeightmapPanel);
      children2[0] = button1;
      Game.UI.Widgets.Button button2 = new Game.UI.Widgets.Button();
      button2.displayName = (LocalizedString) "Editor.EXPORT_HEIGHTMAP";
      button2.uiTag = "UITagPrefab:ExportHeightmap";
      button2.tooltip = new LocalizedString?((LocalizedString) "Editor.EXPORT_HEIGHTMAP_TOOLTIP");
      button2.action = new System.Action(this.ShowExportHeightmapPanel);
      children2[1] = button2;
      widgetArray2[1] = (IWidget) ButtonRow.WithChildren(children2);
      Game.UI.Widgets.Button[] children3 = new Game.UI.Widgets.Button[3];
      Game.UI.Widgets.Button button3 = new Game.UI.Widgets.Button();
      button3.displayName = (LocalizedString) "Editor.IMPORT_WORLDMAP";
      button3.tooltip = new LocalizedString?((LocalizedString) "Editor.IMPORT_WORLDMAP_TOOLTIP");
      button3.uiTag = "UITagPrefab:ImportWorldmap";
      button3.action = new System.Action(this.ShowImportWorldmapPanel);
      children3[0] = button3;
      Game.UI.Widgets.Button button4 = new Game.UI.Widgets.Button();
      button4.displayName = (LocalizedString) "Editor.EXPORT_WORLDMAP";
      button4.tooltip = new LocalizedString?((LocalizedString) "Editor.EXPORT_WORLDMAP_TOOLTIP");
      button4.action = new System.Action(this.ShowExportWorldmapPanel);
      button4.disabled = (Func<bool>) (() => (UnityEngine.Object) this.m_TerrainSystem.worldHeightmap == (UnityEngine.Object) null);
      children3[1] = button4;
      Game.UI.Widgets.Button button5 = new Game.UI.Widgets.Button();
      button5.displayName = (LocalizedString) "Editor.REMOVE_WORLDMAP";
      button5.tooltip = new LocalizedString?((LocalizedString) "Editor.REMOVE_WORLDMAP_TOOLTIP");
      button5.action = new System.Action(this.RemoveWorldmap);
      button5.disabled = (Func<bool>) (() => (UnityEngine.Object) this.m_TerrainSystem.worldHeightmap == (UnityEngine.Object) null);
      children3[2] = button5;
      widgetArray2[2] = (IWidget) ButtonRow.WithChildren(children3);
      editorSection3.children = (IList<IWidget>) widgetArray2;
      children1[1] = (IWidget) editorSection2;
      widgetArray1[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children1);
      this.children = (IList<IWidget>) widgetArray1;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      List<IconButton> iconButtonList1 = new List<IconButton>();
      List<IconButton> iconButtonList2 = new List<IconButton>();
      NativeArray<PrefabData> componentDataArray = this.m_PrefabQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      componentDataArray.Sort<PrefabData, TerrainPanelSystem.UIPriorityComparer>(new TerrainPanelSystem.UIPriorityComparer(this.m_PrefabSystem));
      this.m_ToolPrefabs.Clear();
      foreach (PrefabData prefabData in componentDataArray)
      {
        // ISSUE: reference to a compiler-generated method
        TerraformingPrefab prefab = this.m_PrefabSystem.GetPrefab<TerraformingPrefab>(prefabData);
        if (prefab.m_Target == TerraformingTarget.Height)
        {
          this.m_ToolPrefabs.Add((PrefabBase) prefab);
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          iconButtonList1.Add(new IconButton()
          {
            icon = ImageSystem.GetIcon((PrefabBase) prefab) ?? "Media/Editor/Terrain.svg",
            tooltip = new LocalizedString?(LocalizedString.Id("Assets.NAME[" + prefab.name + "]")),
            action = (System.Action) (() => this.m_ToolSystem.ActivatePrefabTool((UnityEngine.Object) this.m_ToolSystem.activePrefab != (UnityEngine.Object) prefab ? (PrefabBase) prefab : (PrefabBase) null)),
            selected = (Func<bool>) (() => (UnityEngine.Object) this.m_ToolSystem.activePrefab == (UnityEngine.Object) prefab)
          });
        }
        if (prefab.m_Target == TerraformingTarget.Material)
        {
          this.m_ToolPrefabs.Add((PrefabBase) prefab);
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          iconButtonList2.Add(new IconButton()
          {
            icon = ImageSystem.GetIcon((PrefabBase) prefab) ?? "Media/Editor/Material.svg",
            tooltip = new LocalizedString?(LocalizedString.Id("Assets.NAME[" + prefab.name + "]")),
            action = (System.Action) (() => this.m_ToolSystem.ActivatePrefabTool((UnityEngine.Object) this.m_ToolSystem.activePrefab != (UnityEngine.Object) prefab ? (PrefabBase) prefab : (PrefabBase) null)),
            selected = (Func<bool>) (() => (UnityEngine.Object) this.m_ToolSystem.activePrefab == (UnityEngine.Object) prefab)
          });
        }
      }
      componentDataArray.Dispose();
      this.m_ToolButtonGroup.children = iconButtonList1.ToArray();
      this.m_MaterialButtonGroup.children = iconButtonList2.ToArray();
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      this.activeSubPanel = (IEditorPanel) null;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      if (this.m_ToolPrefabs.Contains(this.m_ToolSystem.activePrefab))
      {
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      }
      base.OnStopRunning();
    }

    protected override bool OnCancel()
    {
      if (!this.m_ToolPrefabs.Contains(this.m_ToolSystem.activePrefab))
        return base.OnCancel();
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      return false;
    }

    private void ShowImportHeightmapPanel()
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.IMPORT_HEIGHTMAP", TerrainPanelSystem.GetHeightmaps(), new LoadAssetPanel.LoadCallback(this.OnLoadHeightmap), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowExportHeightmapPanel()
    {
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.EXPORT_HEIGHTMAP", TerrainPanelSystem.GetHeightmaps(), new Colossal.Hash128?(), new SaveAssetPanel.SaveCallback(this.OnSaveHeightmap), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowImportWorldmapPanel()
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.IMPORT_WORLDMAP", TerrainPanelSystem.GetHeightmaps(), new LoadAssetPanel.LoadCallback(this.OnLoadWorldHeightmap), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void ShowExportWorldmapPanel()
    {
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.EXPORT_WORLDMAP", TerrainPanelSystem.GetHeightmaps(), new Colossal.Hash128?(), new SaveAssetPanel.SaveCallback(this.OnSaveWorldHeightmap), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void RemoveWorldmap() => this.m_TerrainSystem.ReplaceWorldHeightmap((Texture2D) null);

    private void RefreshTerrainProperties(float2 heightScaleOffset)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.SetTerrainProperties(heightScaleOffset);
    }

    private static IEnumerable<AssetItem> GetHeightmaps()
    {
      foreach (ImageAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (a =>
      {
        string subPath = a.GetMeta().subPath;
        return subPath != null && subPath.StartsWith(TerrainPanelSystem.kHeightmapFolder);
      }))))
      {
        AssetItem heightmap = new AssetItem();
        heightmap.guid = asset.guid;
        heightmap.fileName = asset.name;
        heightmap.displayName = (LocalizedString) asset.name;
        heightmap.image = asset.ToUri();
        heightmap.tooltip = new LocalizedString?((LocalizedString) asset.name);
        yield return heightmap;
      }
    }

    private void OnLoadHeightmap(Colossal.Hash128 guid)
    {
      this.CloseSubPanel();
      ImageAsset assetData;
      if (!Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<ImageAsset>(guid, out assetData))
        return;
      using (assetData)
      {
        Texture2D texture2D = assetData.Load(true);
        // ISSUE: reference to a compiler-generated method
        if (!TerrainSystem.IsValidHeightmapFormat(texture2D))
        {
          this.DisplayHeightmapError();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.m_TerrainSystem.ReplaceHeightmap(texture2D);
        }
      }
    }

    private void OnLoadWorldHeightmap(Colossal.Hash128 guid)
    {
      this.CloseSubPanel();
      ImageAsset assetData;
      if (!Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<ImageAsset>(guid, out assetData))
        return;
      using (assetData)
      {
        Texture2D texture2D = assetData.Load(true);
        // ISSUE: reference to a compiler-generated method
        if (!TerrainSystem.IsValidHeightmapFormat(texture2D))
        {
          this.DisplayHeightmapError();
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.m_TerrainSystem.ReplaceWorldHeightmap(texture2D);
        }
      }
    }

    private void DisplayHeightmapError()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.userInterface.appBindings.ShowMessageDialog(new MessageDialog(new LocalizedString?(LocalizedString.Id("Editor.INCORRECT_HEIGHTMAP_TITLE")), new LocalizedString("Editor.INCORRECT_HEIGHTMAP_MESSAGE", (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
      {
        {
          "WIDTH",
          (ILocElement) LocalizedString.Value(TerrainSystem.kDefaultHeightmapWidth.ToString())
        },
        {
          "HEIGHT",
          (ILocElement) LocalizedString.Value(TerrainSystem.kDefaultHeightmapHeight.ToString())
        }
      }), LocalizedString.Id("Common.ERROR_DIALOG_CONTINUE"), System.Array.Empty<LocalizedString>()), (Action<int>) null);
    }

    private void OnSaveHeightmap(string fileName, Colossal.Hash128? overwriteGuid)
    {
      this.OnSaveHeightmap(fileName, overwriteGuid, false);
    }

    private void OnSaveWorldHeightmap(string fileName, Colossal.Hash128? overwriteGuid)
    {
      this.OnSaveHeightmap(fileName, overwriteGuid, true);
    }

    private unsafe void OnSaveHeightmap(string fileName, Colossal.Hash128? overwriteGuid, bool worldMap)
    {
      this.CloseSubPanel();
      bool flag = false;
      ImageAsset assetData;
      if (overwriteGuid.HasValue && Colossal.IO.AssetDatabase.AssetDatabase.user.TryGetAsset<ImageAsset>(overwriteGuid.Value, out assetData))
      {
        Colossal.IO.AssetDatabase.AssetDatabase.user.DeleteAsset<ImageAsset>(assetData);
        flag = true;
      }
      Texture src = worldMap ? this.m_TerrainSystem.worldHeightmap : this.m_TerrainSystem.heightmap;
      NativeArray<ushort> output = new NativeArray<ushort>(src.width * src.height, Allocator.Persistent);
      AsyncGPUReadback.RequestIntoNativeArray<ushort>(ref output, src).WaitForCompletion();
      try
      {
        byte[] buffer = TextureUtilities.SaveImage((IntPtr) output.GetUnsafeReadOnlyPtr<ushort>(), (long) (output.Length * 2), src.width, src.height, 1, 16, NativeTextures.ImageFileFormat.PNG);
        using (Stream writeStream = Colossal.IO.AssetDatabase.AssetDatabase.user.AddAsset<ImageAsset>(AssetDataPath.Create(TerrainPanelSystem.kHeightmapFolder, fileName), flag ? overwriteGuid.Value : new Colossal.Hash128()).GetWriteStream())
          writeStream.Write(buffer, 0, buffer.Length);
      }
      finally
      {
        output.Dispose();
      }
    }

    [Preserve]
    public TerrainPanelSystem()
    {
    }

    private struct UIPriorityComparer : IComparer<PrefabData>
    {
      private PrefabSystem m_PrefabSystem;

      public UIPriorityComparer(PrefabSystem prefabSystem) => this.m_PrefabSystem = prefabSystem;

      public int Compare(PrefabData a, PrefabData b)
      {
        // ISSUE: reference to a compiler-generated method
        TerraformingPrefab prefab1 = this.m_PrefabSystem.GetPrefab<TerraformingPrefab>(a);
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab2 = (PrefabBase) this.m_PrefabSystem.GetPrefab<TerraformingPrefab>(b);
        UIObject uiObject;
        ref UIObject local = ref uiObject;
        UIObject component;
        return prefab1.TryGet<UIObject>(out local) && prefab2.TryGet<UIObject>(out component) ? uiObject.m_Priority - component.m_Priority : -1;
      }
    }
  }
}
