// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DescriptionSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DescriptionSection : InfoSectionBase
  {
    private PrefabUISystem m_PrefabUISystem;
    private NativeList<LeisureProviderData> m_LeisureDatas;
    private NativeList<LocalModifierData> m_LocalModifierDatas;
    private NativeList<CityModifierData> m_CityModifierDatas;

    protected override string group => nameof (DescriptionSection);

    private string localeId { get; set; }

    protected override bool displayForOutsideConnections => true;

    protected override bool displayForUnderConstruction => true;

    protected override bool displayForUpgrades => true;

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureDatas = new NativeList<LeisureProviderData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LocalModifierDatas = new NativeList<LocalModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_CityModifierDatas = new NativeList<CityModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureDatas.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalModifierDatas.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_CityModifierDatas.Dispose();
      base.OnDestroy();
    }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LeisureDatas.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalModifierDatas.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_CityModifierDatas.Clear();
      this.localeId = (string) null;
    }

    private bool Visible()
    {
      return !this.EntityManager.HasComponent<Route>(this.selectedEntity) && (!this.EntityManager.HasComponent<District>(this.selectedEntity) || !this.EntityManager.HasComponent<Area>(this.selectedEntity)) && this.EntityManager.HasComponent<ServiceObjectData>(this.selectedPrefab) || this.EntityManager.HasComponent<SignatureBuildingData>(this.selectedPrefab) || this.EntityManager.HasComponent<Game.Objects.OutsideConnection>(this.selectedEntity) || this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      string descriptionId;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.GetTitleAndDescription(this.selectedPrefab, out string _, out descriptionId);
      this.localeId = descriptionId;
      DynamicBuffer<LocalModifierData> buffer1;
      if (this.EntityManager.TryGetBuffer<LocalModifierData>(this.selectedPrefab, true, out buffer1))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LocalModifierDatas.AddRange(buffer1.AsNativeArray());
      }
      DynamicBuffer<CityModifierData> buffer2;
      if (this.EntityManager.TryGetBuffer<CityModifierData>(this.selectedPrefab, true, out buffer2))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityModifierDatas.AddRange(buffer2.AsNativeArray());
      }
      LeisureProviderData component1;
      if (this.EntityManager.TryGetComponent<LeisureProviderData>(this.selectedPrefab, out component1) && component1.m_Efficiency > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LeisureDatas.Add(in component1);
      }
      DynamicBuffer<InstalledUpgrade> buffer3;
      if (!this.EntityManager.TryGetBuffer<InstalledUpgrade>(this.selectedEntity, true, out buffer3))
        return;
      for (int index = 0; index < buffer3.Length; ++index)
      {
        PrefabRef component2;
        if (this.EntityManager.TryGetComponent<PrefabRef>(buffer3[index].m_Upgrade, out component2))
        {
          DynamicBuffer<LocalModifierData> buffer4;
          if (this.EntityManager.TryGetBuffer<LocalModifierData>(component2.m_Prefab, true, out buffer4))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            LocalEffectSystem.AddToTempList(this.m_LocalModifierDatas, buffer4, false);
          }
          DynamicBuffer<CityModifierData> buffer5;
          if (this.EntityManager.TryGetBuffer<CityModifierData>(component2.m_Prefab, true, out buffer5))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            CityModifierUpdateSystem.AddToTempList(this.m_CityModifierDatas, buffer5);
          }
          LeisureProviderData component3;
          if (this.EntityManager.TryGetComponent<LeisureProviderData>(component2.m_Prefab, out component3) && component3.m_Efficiency > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            LeisureSystem.AddToTempList(this.m_LeisureDatas, component3);
          }
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("localeId");
      writer.Write(this.localeId);
      writer.PropertyName("effects");
      int size = 0;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CityModifierDatas.Length > 0)
        ++size;
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalModifierDatas.Length > 0)
        ++size;
      // ISSUE: reference to a compiler-generated field
      if (this.m_LeisureDatas.Length > 0)
        ++size;
      writer.ArrayBegin(size);
      // ISSUE: reference to a compiler-generated field
      if (this.m_CityModifierDatas.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.CityModifierBinder.Bind<NativeList<CityModifierData>>(writer, this.m_CityModifierDatas);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LocalModifierDatas.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.LocalModifierBinder.Bind<NativeList<LocalModifierData>>(writer, this.m_LocalModifierDatas);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_LeisureDatas.Length > 0)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabUISystem.LeisureProviderBinder.Bind<NativeList<LeisureProviderData>>(writer, this.m_LeisureDatas);
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public DescriptionSection()
    {
    }
  }
}
