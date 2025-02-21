// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ResourcePanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Serialization.Entities;
using Colossal.UI;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class ResourcePanelSystem : EditorPanelSystemBase
  {
    private static readonly string kTextureImportFolder = "Heightmaps";
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private NaturalResourceSystem m_NaturalResourceSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private EntityQuery m_PrefabQuery;
    private IconButtonGroup m_ToolButtonGroup;
    private EditorSection m_TextureImportButtons;
    private List<PrefabBase> m_ToolPrefabs = new List<PrefabBase>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_NaturalResourceSystem = this.World.GetOrCreateSystemManaged<NaturalResourceSystem>();
      this.m_GroundWaterSystem = this.World.GetExistingSystemManaged<GroundWaterSystem>();
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<TerraformingData>(),
          ComponentType.ReadOnly<UIObjectData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      this.title = (LocalizedString) "Editor.RESOURCES";
      IWidget[] widgetArray = new IWidget[1];
      IWidget[] children = new IWidget[2];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Editor.RESOURCE_TOOLS";
      editorSection1.tooltip = new LocalizedString?((LocalizedString) "Editor.RESOURCE_TOOLS_TOOLTIP");
      editorSection1.expanded = true;
      editorSection1.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_ToolButtonGroup = new IconButtonGroup())
      };
      children[0] = (IWidget) editorSection1;
      EditorSection editorSection2 = new EditorSection();
      editorSection2.displayName = (LocalizedString) "Editor.RESOURCE_TEXTURE_LABEL";
      editorSection2.tooltip = new LocalizedString?((LocalizedString) "Editor.RESOURCE_TEXTURE_LABEL_TOOLTIP");
      editorSection2.expanded = true;
      EditorSection editorSection3 = editorSection2;
      this.m_TextureImportButtons = editorSection2;
      children[1] = (IWidget) editorSection3;
      widgetArray[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children);
      this.children = (IList<IWidget>) widgetArray;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.m_ToolPrefabs.Clear();
      List<IconButton> iconButtonList = new List<IconButton>();
      List<IWidget> widgetList1 = new List<IWidget>();
      using (NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.m_PrefabQuery, Allocator.Temp))
      {
        foreach (UIObjectInfo uiObjectInfo in sortedObjects)
        {
          // ISSUE: reference to a compiler-generated method
          TerraformingPrefab prefab = this.m_PrefabSystem.GetPrefab<TerraformingPrefab>(uiObjectInfo.prefabData);
          if (ResourcePanelSystem.IsResourceTerraformingPrefab(prefab))
          {
            this.m_ToolPrefabs.Add((PrefabBase) prefab);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            iconButtonList.Add(new IconButton()
            {
              icon = ImageSystem.GetIcon((PrefabBase) prefab) ?? "Media/Editor/Terrain.svg",
              tooltip = new LocalizedString?(LocalizedString.Id("Assets.NAME[" + prefab.name + "]")),
              action = (System.Action) (() => this.m_ToolSystem.ActivatePrefabTool((UnityEngine.Object) this.m_ToolSystem.activePrefab != (UnityEngine.Object) prefab ? (PrefabBase) prefab : (PrefabBase) null)),
              selected = (Func<bool>) (() => (UnityEngine.Object) this.m_ToolSystem.activePrefab == (UnityEngine.Object) prefab)
            });
            List<IWidget> widgetList2 = widgetList1;
            ButtonRow buttonRow1 = new ButtonRow();
            ButtonRow buttonRow2 = buttonRow1;
            Button[] buttonArray = new Button[2];
            Button button1 = new Button();
            button1.displayName = (LocalizedString) string.Format("Editor.IMPORT_RESOURCE[{0}]", (object) prefab.m_Target);
            button1.tooltip = new LocalizedString?((LocalizedString) string.Format("Editor.IMPORT_RESOURCE_TOOLTIP[{0}]", (object) prefab.m_Target));
            button1.action = (System.Action) (() => this.ImportTexture(prefab.m_Target));
            buttonArray[0] = button1;
            Button button2 = new Button();
            button2.displayName = (LocalizedString) string.Format("Editor.CLEAR_RESOURCE[{0}]", (object) prefab.m_Target);
            button2.tooltip = new LocalizedString?((LocalizedString) string.Format("Editor.CLEAR_RESOURCE_TOOLTIP[{0}]", (object) prefab.m_Target));
            button2.action = (System.Action) (() => this.Clear(prefab.m_Target));
            buttonArray[1] = button2;
            buttonRow2.children = buttonArray;
            ButtonRow buttonRow3 = buttonRow1;
            widgetList2.Add((IWidget) buttonRow3);
          }
        }
        this.m_ToolButtonGroup.children = iconButtonList.ToArray();
        this.m_TextureImportButtons.children = (IList<IWidget>) widgetList1;
      }
    }

    private static bool IsResourceTerraformingPrefab(TerraformingPrefab prefab)
    {
      return prefab.m_Target != TerraformingTarget.Height && prefab.m_Target != TerraformingTarget.Material && prefab.m_Target != TerraformingTarget.None;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      if (!this.m_ToolPrefabs.Contains(this.m_ToolSystem.activePrefab))
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
    }

    protected override bool OnCancel()
    {
      if (!this.m_ToolPrefabs.Contains(this.m_ToolSystem.activePrefab))
        return base.OnCancel();
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool((PrefabBase) null);
      return false;
    }

    private void ImportTexture(TerraformingTarget target)
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) ("Import " + target.ToString()), ResourcePanelSystem.GetTextures(), (LoadAssetPanel.LoadCallback) (guid => this.OnLoadTexture(guid, target)), new System.Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private static IEnumerable<AssetItem> GetTextures()
    {
      foreach (ImageAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (a =>
      {
        string subPath = a.GetMeta().subPath;
        return subPath != null && subPath.StartsWith(ResourcePanelSystem.kTextureImportFolder);
      }))))
      {
        AssetItem texture = new AssetItem();
        texture.guid = asset.guid;
        texture.fileName = asset.name;
        texture.displayName = (LocalizedString) asset.name;
        texture.image = asset.ToUri();
        yield return texture;
      }
    }

    private void OnLoadTexture(Colossal.Hash128 guid, TerraformingTarget target)
    {
      this.CloseSubPanel();
      ImageAsset assetData;
      if (!Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<ImageAsset>(guid, out assetData))
        return;
      using (assetData)
        this.ApplyTexture(assetData.Load(true), target);
    }

    private void ApplyTexture(Texture2D texture, TerraformingTarget target)
    {
      switch (target)
      {
        case TerraformingTarget.Ore:
          this.ApplyTexture<NaturalResourceCell>(texture, (CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Action<Texture2D, CellMapData<NaturalResourceCell>, int, int, Func<NaturalResourceCell, ushort, NaturalResourceCell>>(this.ApplyResource<NaturalResourceCell>), new Func<NaturalResourceCell, ushort, NaturalResourceCell>(this.ApplyOre));
          break;
        case TerraformingTarget.Oil:
          this.ApplyTexture<NaturalResourceCell>(texture, (CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Action<Texture2D, CellMapData<NaturalResourceCell>, int, int, Func<NaturalResourceCell, ushort, NaturalResourceCell>>(this.ApplyResource<NaturalResourceCell>), new Func<NaturalResourceCell, ushort, NaturalResourceCell>(this.ApplyOil));
          break;
        case TerraformingTarget.FertileLand:
          this.ApplyTexture<NaturalResourceCell>(texture, (CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Action<Texture2D, CellMapData<NaturalResourceCell>, int, int, Func<NaturalResourceCell, ushort, NaturalResourceCell>>(this.ApplyResource<NaturalResourceCell>), new Func<NaturalResourceCell, ushort, NaturalResourceCell>(this.ApplyFertile));
          break;
        case TerraformingTarget.GroundWater:
          this.ApplyTexture<GroundWater>(texture, (CellMapSystem<GroundWater>) this.m_GroundWaterSystem, new Action<Texture2D, CellMapData<GroundWater>, int, int, Func<GroundWater, ushort, GroundWater>>(this.ApplyGroundWater), (Func<GroundWater, ushort, GroundWater>) null);
          break;
      }
    }

    private void ApplyTexture<TCell>(
      Texture2D texture,
      CellMapSystem<TCell> cellMapSystem,
      Action<Texture2D, CellMapData<TCell>, int, int, Func<TCell, ushort, TCell>> applyCallback,
      Func<TCell, ushort, TCell> resourceCallback)
      where TCell : struct, ISerializable
    {
      JobHandle dependencies;
      CellMapData<TCell> data = cellMapSystem.GetData(false, out dependencies);
      dependencies.Complete();
      for (int index1 = 0; index1 < data.m_TextureSize.y; ++index1)
      {
        for (int index2 = 0; index2 < data.m_TextureSize.x; ++index2)
          applyCallback(texture, data, index2, index1, resourceCallback);
      }
    }

    private void ApplyResource<TCell>(
      Texture2D texture,
      CellMapData<TCell> data,
      int x,
      int y,
      Func<TCell, ushort, TCell> applyCallback)
      where TCell : struct, ISerializable
    {
      int index = y * data.m_TextureSize.x + x;
      TCell cell = data.m_Buffer[index];
      ushort num = (ushort) this.Sample<TCell>(texture, data, x, y, 10000);
      data.m_Buffer[index] = applyCallback(cell, num);
    }

    private void ApplyGroundWater(
      Texture2D texture,
      CellMapData<GroundWater> data,
      int x,
      int y,
      Func<GroundWater, ushort, GroundWater> _)
    {
      int index = y * data.m_TextureSize.x + x;
      short num = (short) this.Sample<GroundWater>(texture, data, x, y, 10000);
      data.m_Buffer[index] = new GroundWater()
      {
        m_Amount = num,
        m_Max = num
      };
    }

    private NaturalResourceCell ApplyOre(NaturalResourceCell cell, ushort amount)
    {
      cell.m_Ore = new NaturalResourceAmount()
      {
        m_Base = amount
      };
      return cell;
    }

    private NaturalResourceCell ApplyOil(NaturalResourceCell cell, ushort amount)
    {
      cell.m_Oil = new NaturalResourceAmount()
      {
        m_Base = amount
      };
      return cell;
    }

    private NaturalResourceCell ApplyFertile(NaturalResourceCell cell, ushort amount)
    {
      cell.m_Fertility = new NaturalResourceAmount()
      {
        m_Base = amount
      };
      return cell;
    }

    private int Sample<TCell>(Texture2D texture, CellMapData<TCell> data, int x, int y, int max) where TCell : struct, ISerializable
    {
      return Mathf.RoundToInt(math.saturate(texture.GetPixelBilinear((float) x / (float) (data.m_TextureSize.x - 1), (float) y / (float) (data.m_TextureSize.y - 1)).r) * (float) max);
    }

    private void Clear(TerraformingTarget target)
    {
      switch (target)
      {
        case TerraformingTarget.Ore:
          this.ClearMap<NaturalResourceCell>((CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Func<NaturalResourceCell, NaturalResourceCell>(this.ClearOre));
          break;
        case TerraformingTarget.Oil:
          this.ClearMap<NaturalResourceCell>((CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Func<NaturalResourceCell, NaturalResourceCell>(this.ClearOil));
          break;
        case TerraformingTarget.FertileLand:
          this.ClearMap<NaturalResourceCell>((CellMapSystem<NaturalResourceCell>) this.m_NaturalResourceSystem, new Func<NaturalResourceCell, NaturalResourceCell>(this.ClearFertile));
          break;
        case TerraformingTarget.GroundWater:
          this.ClearMap<GroundWater>((CellMapSystem<GroundWater>) this.m_GroundWaterSystem, new Func<GroundWater, GroundWater>(this.ClearGroundWater));
          break;
      }
    }

    private void ClearMap<TCell>(
      CellMapSystem<TCell> cellMapSystem,
      Func<TCell, TCell> clearCallback)
      where TCell : struct, ISerializable
    {
      JobHandle dependencies;
      CellMapData<TCell> data = cellMapSystem.GetData(false, out dependencies);
      dependencies.Complete();
      for (int index = 0; index < data.m_Buffer.Length; ++index)
        data.m_Buffer[index] = clearCallback(data.m_Buffer[index]);
    }

    private NaturalResourceCell ClearOre(NaturalResourceCell cell)
    {
      cell.m_Ore = new NaturalResourceAmount();
      return cell;
    }

    private NaturalResourceCell ClearOil(NaturalResourceCell cell)
    {
      cell.m_Oil = new NaturalResourceAmount();
      return cell;
    }

    private NaturalResourceCell ClearFertile(NaturalResourceCell cell)
    {
      cell.m_Fertility = new NaturalResourceAmount();
      return cell;
    }

    private GroundWater ClearGroundWater(GroundWater _) => new GroundWater();

    [Preserve]
    public ResourcePanelSystem()
    {
    }
  }
}
