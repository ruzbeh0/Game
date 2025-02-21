// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.DiversityPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Reflection;
using Game.Simulation;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class DiversityPanelSystem : EditorPanelSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private DiversitySystem m_DiversitySystem;
    private EntityQuery m_AtmosphereQuery;
    private EntityQuery m_BiomeQuery;
    private AtmospherePrefab m_Atmosphere;
    private BiomePrefab m_Biome;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DiversitySystem = this.World.GetOrCreateSystemManaged<DiversitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AtmosphereQuery = this.GetEntityQuery(ComponentType.ReadOnly<AtmosphereData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BiomeQuery = this.GetEntityQuery(ComponentType.ReadOnly<BiomeData>());
      this.title = LocalizedString.Value("Diversity");
      IWidget[] widgetArray1 = new IWidget[1];
      IWidget[] children = new IWidget[1];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Diversity Settings";
      editorSection1.expanded = true;
      EditorSection editorSection2 = editorSection1;
      IWidget[] widgetArray2 = new IWidget[2];
      PopupValueField<PrefabBase> popupValueField1 = new PopupValueField<PrefabBase>();
      popupValueField1.displayName = (LocalizedString) "Atmosphere";
      // ISSUE: reference to a compiler-generated field
      popupValueField1.accessor = (ITypedValueAccessor<PrefabBase>) new DelegateAccessor<PrefabBase>((Func<PrefabBase>) (() => (PrefabBase) this.m_Atmosphere), (Action<PrefabBase>) (prefab =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Atmosphere = (AtmospherePrefab) prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_DiversitySystem.ApplyAtmospherePreset(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Atmosphere));
      }));
      popupValueField1.popup = (IValueFieldPopup<PrefabBase>) new PrefabPickerPopup(typeof (AtmospherePrefab));
      widgetArray2[0] = (IWidget) popupValueField1;
      PopupValueField<PrefabBase> popupValueField2 = new PopupValueField<PrefabBase>();
      popupValueField2.displayName = (LocalizedString) "Biome";
      // ISSUE: reference to a compiler-generated field
      popupValueField2.accessor = (ITypedValueAccessor<PrefabBase>) new DelegateAccessor<PrefabBase>((Func<PrefabBase>) (() => (PrefabBase) this.m_Biome), (Action<PrefabBase>) (prefab =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Biome = (BiomePrefab) prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_DiversitySystem.ApplyBiomePreset(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Biome));
      }));
      popupValueField2.popup = (IValueFieldPopup<PrefabBase>) new PrefabPickerPopup(typeof (BiomePrefab));
      widgetArray2[1] = (IWidget) popupValueField2;
      editorSection2.children = (IList<IWidget>) widgetArray2;
      children[0] = (IWidget) editorSection1;
      widgetArray1[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children);
      this.children = (IList<IWidget>) widgetArray1;
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.TryGetPrefab<AtmospherePrefab>(this.m_AtmosphereQuery.GetSingleton<AtmosphereData>().m_AtmospherePrefab, out this.m_Atmosphere);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.TryGetPrefab<BiomePrefab>(this.m_BiomeQuery.GetSingleton<BiomeData>().m_BiomePrefab, out this.m_Biome);
    }

    private void SetAtmosphere(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Atmosphere = (AtmospherePrefab) prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_DiversitySystem.ApplyAtmospherePreset(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Atmosphere));
    }

    private void SetBiome(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Biome = (BiomePrefab) prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_DiversitySystem.ApplyBiomePreset(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_Biome));
    }

    [Preserve]
    public DiversityPanelSystem()
    {
    }
  }
}
