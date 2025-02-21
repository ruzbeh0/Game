// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PoliciesUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Policies;
using Game.Prefabs;
using Game.Routes;
using Game.SceneFlow;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PoliciesUISystem : UISystemBase
  {
    public const string kGroup = "policies";
    public System.Action EventPolicyUnlocked;
    private CitySystem m_CitySystem;
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private SelectedInfoUISystem m_InfoSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ImageSystem m_ImageSystem;
    private EntityQuery m_CityPoliciesQuery;
    private EntityQuery m_CityPoliciesUpdatedQuery;
    private EntityQuery m_DistrictPoliciesQuery;
    private EntityQuery m_BuildingPoliciesQuery;
    private EntityQuery m_RoutePoliciesQuery;
    private EntityQuery m_PolicyUnlockedQuery;
    private EntityArchetype m_PolicyEventArchetype;
    private List<UIPolicy> m_CityPolicies;
    private List<UIPolicy> m_SelectedInfoPolicies;
    private GetterValueBinding<List<UIPolicy>> m_CityPoliciesBinding;
    private PoliciesUISystem.TypeHandle __TypeHandle;

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      this.m_CityPolicies.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoPolicies.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_CityPoliciesBinding.Update();
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoSystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityPoliciesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PolicyData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<CityOptionData>(),
          ComponentType.ReadOnly<CityModifierData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_CityPoliciesUpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.City.City>(),
          ComponentType.ReadOnly<Policy>(),
          ComponentType.ReadOnly<Updated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictPoliciesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PolicyData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<DistrictOptionData>(),
          ComponentType.ReadOnly<DistrictModifierData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingPoliciesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PolicyData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<BuildingOptionData>(),
          ComponentType.ReadOnly<BuildingModifierData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RoutePoliciesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PolicyData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<RouteOptionData>(),
          ComponentType.ReadOnly<RouteModifierData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyUnlockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Modify>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityPolicies = new List<UIPolicy>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoPolicies = new List<UIPolicy>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CityPoliciesBinding = new GetterValueBinding<List<UIPolicy>>("policies", "cityPolicies", (Func<List<UIPolicy>>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CityPolicies.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindAndSortPolicies(this.m_CitySystem.City, this.m_CityPoliciesQuery, this.m_CityPolicies);
        // ISSUE: reference to a compiler-generated field
        return this.m_CityPolicies;
      }), (IWriter<List<UIPolicy>>) new DelegateWriter<List<UIPolicy>>((WriterDelegate<List<UIPolicy>>) ((writer, policies) =>
      {
        writer.ArrayBegin(policies.Count);
        foreach (UIPolicy policy in policies)
        {
          // ISSUE: reference to a compiler-generated field
          policy.Write(this.m_PrefabUISystem, writer);
        }
        writer.ArrayEnd();
      })))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool, float>("policies", "setPolicy", (Action<Entity, bool, float>) ((policy, active, adjustment) => this.ModifyPolicy(this.m_InfoSystem.selectedEntity, policy, active, adjustment))));
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool, float>("policies", "setCityPolicy", (Action<Entity, bool, float>) ((policy, active, adjustment) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ModifyPolicy(this.m_CitySystem.City, policy, active, adjustment);
        // ISSUE: reference to a compiler-generated method
        this.RefreshCityPolicyAchievement(policy, active);
      })));
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.EventPolicyUnlocked = (System.Action) null;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (PrefabUtils.HasUnlockedPrefab<PolicyData>(this.EntityManager, this.m_PolicyUnlockedQuery))
      {
        // ISSUE: reference to a compiler-generated field
        System.Action eventPolicyUnlocked = this.EventPolicyUnlocked;
        if (eventPolicyUnlocked != null)
          eventPolicyUnlocked();
        // ISSUE: reference to a compiler-generated field
        this.m_CityPoliciesBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_CityPoliciesUpdatedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_CityPoliciesBinding.Update();
    }

    public void SetPolicy(Entity target, Entity policy, bool active, float adjustment = 0.0f)
    {
      // ISSUE: reference to a compiler-generated method
      this.ModifyPolicy(target, policy, active, adjustment);
    }

    public void SetCityPolicy(Entity policy, bool active, float adjustment)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ModifyPolicy(this.m_CitySystem.City, policy, active, adjustment);
      // ISSUE: reference to a compiler-generated method
      this.RefreshCityPolicyAchievement(policy, active);
    }

    private void RefreshCityPolicyAchievement(Entity policy, bool active)
    {
      DynamicBuffer<Policy> buffer;
      // ISSUE: reference to a compiler-generated field
      if (!World.DefaultGameObjectInjectionWorld.EntityManager.TryGetBuffer<Policy>(this.m_CitySystem.City, true, out buffer))
        return;
      bool flag = false;
      int num = 0;
      for (int index = 0; index < buffer.Length; ++index)
      {
        if ((buffer[index].m_Flags & PolicyFlags.Active) != (PolicyFlags) 0)
        {
          ++num;
          if (buffer[index].m_Policy == policy)
            flag = true;
        }
      }
      if (active && !flag)
        ++num;
      if (!active & flag)
        --num;
      PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.CallingtheShots, num, IndicateType.Absolute);
    }

    public void SetSelectedInfoPolicy(Entity policy, bool active, float adjustment = 0.0f)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ModifyPolicy(this.m_InfoSystem.selectedEntity, policy, active, adjustment);
    }

    public void SetSelectedInfoPolicy(Entity target, Entity policy, bool active, float adjustment = 0.0f)
    {
      // ISSUE: reference to a compiler-generated method
      this.ModifyPolicy(target, policy, active, adjustment);
    }

    private void ModifyPolicy(Entity target, Entity policy, bool active, float adjustment)
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      Entity entity = commandBuffer.CreateEntity(this.m_PolicyEventArchetype);
      commandBuffer.SetComponent<Modify>(entity, new Modify(target, policy, active, adjustment));
    }

    private void FindAndSortPolicies(Entity entity, EntityQuery policyQuery, List<UIPolicy> list)
    {
      DynamicBuffer<Policy> buffer = this.EntityManager.GetBuffer<Policy>(entity, true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PolicySliderData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentTypeHandle;
      NativeArray<ArchetypeChunk> archetypeChunkArray = policyQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        NativeArray<Entity> nativeArray1 = archetypeChunkArray[index1].GetNativeArray(entityTypeHandle);
        bool slider = archetypeChunkArray[index1].Has<PolicySliderData>(ref componentTypeHandle);
        NativeArray<PolicySliderData> nativeArray2 = archetypeChunkArray[index1].GetNativeArray<PolicySliderData>(ref componentTypeHandle);
        for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PolicyPrefab prefab = this.m_PrefabSystem.GetPrefab<PolicyPrefab>(nativeArray1[index2]);
          int priority = 0;
          UIObjectData component;
          if (this.EntityManager.TryGetComponent<UIObjectData>(nativeArray1[index2], out component))
            priority = component.m_Priority;
          // ISSUE: reference to a compiler-generated method
          if (this.FilterPolicy(nativeArray1[index2], prefab, entity))
          {
            // ISSUE: reference to a compiler-generated method
            UIPolicy info = this.ExtractInfo(nativeArray1[index2], prefab, buffer, slider, slider ? nativeArray2[index2] : new PolicySliderData(), priority);
            list.Add(info);
          }
        }
      }
      list.Sort();
      archetypeChunkArray.Dispose();
    }

    private bool FilterPolicy(Entity policy, PolicyPrefab prefab, Entity target)
    {
      if (prefab.m_Visibility == PolicyVisibility.HideFromPolicyList)
        return false;
      EntityManager entityManager = this.EntityManager;
      if (!entityManager.HasComponent<District>(target))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Game.City.City>(target))
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Route>(target))
          {
            entityManager = this.EntityManager;
            BuildingOptionData component1;
            if (!entityManager.HasComponent<Building>(target) || !this.EntityManager.TryGetComponent<BuildingOptionData>(policy, out component1))
              return false;
            PrefabRef component2;
            Game.Prefabs.BuildingData component3;
            PrefabRef component4;
            GarbageFacilityData component5;
            // ISSUE: reference to a compiler-generated method
            if (BuildingUtils.HasOption(component1, BuildingOption.PaidParking) && this.EntityManager.TryGetComponent<PrefabRef>(target, out component2) && this.EntityManager.TryGetComponent<Game.Prefabs.BuildingData>(component2.m_Prefab, out component3) && (component3.m_Flags & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) == (Game.Prefabs.BuildingFlags) 0 && this.HasParkingLanes(target) || BuildingUtils.HasOption(component1, BuildingOption.Empty) && this.EntityManager.TryGetComponent<PrefabRef>(target, out component4) && this.EntityManager.TryGetComponent<GarbageFacilityData>(component4.m_Prefab, out component5) && component5.m_LongTermStorage)
              return true;
            if (!BuildingUtils.HasOption(component1, BuildingOption.Inactive))
              return false;
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<CityServiceUpkeep>(target))
              return false;
            entityManager = this.EntityManager;
            return entityManager.HasComponent<Efficiency>(target);
          }
        }
      }
      return true;
    }

    private UIPolicy ExtractInfo(
      Entity entity,
      PolicyPrefab prefab,
      DynamicBuffer<Policy> activePolicies,
      bool slider,
      PolicySliderData sliderData,
      int priority)
    {
      string name = prefab.name;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      string icon = ImageSystem.GetIcon((PrefabBase) prefab) ?? this.m_ImageSystem.placeholderIcon;
      bool active = false;
      bool locked = this.EntityManager.HasEnabledComponent<Locked>(entity);
      float adjustment = sliderData.m_Default;
      for (int index = 0; index < activePolicies.Length; ++index)
      {
        if (activePolicies[index].m_Policy == entity)
        {
          active = (activePolicies[index].m_Flags & PolicyFlags.Active) != 0;
          adjustment = activePolicies[index].m_Adjustment;
          break;
        }
      }
      string empty;
      if (!GameManager.instance.localizationManager.activeDictionary.TryGetValue(string.Format("Policy.TITLE[{0}]", (object) name), out empty))
        empty = string.Empty;
      int requiredMilestone = locked ? ProgressionUtils.GetRequiredMilestone(this.EntityManager, entity) : 0;
      UIPolicySlider data = new UIPolicySlider(adjustment, sliderData);
      return new UIPolicy(name, empty, priority, icon, entity, active, locked, prefab.uiTag, requiredMilestone, slider, data);
    }

    private bool HasParkingLanes(Entity building)
    {
      DynamicBuffer<Game.Net.SubLane> buffer1;
      DynamicBuffer<Game.Net.SubNet> buffer2;
      DynamicBuffer<Game.Objects.SubObject> buffer3;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.EntityManager.TryGetBuffer<Game.Net.SubLane>(building, true, out buffer1) && this.HasParkingLanes(buffer1) || this.EntityManager.TryGetBuffer<Game.Net.SubNet>(building, true, out buffer2) && this.HasParkingLanes(buffer2) || this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(building, true, out buffer3) && this.HasParkingLanes(buffer3);
    }

    private bool HasParkingLanes(DynamicBuffer<Game.Objects.SubObject> subObjects)
    {
      for (int index = 0; index < subObjects.Length; ++index)
      {
        Entity subObject = subObjects[index].m_SubObject;
        DynamicBuffer<Game.Net.SubLane> buffer1;
        DynamicBuffer<Game.Objects.SubObject> buffer2;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subObject, true, out buffer1) && this.HasParkingLanes(buffer1) || this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(subObject, true, out buffer2) && this.HasParkingLanes(buffer2))
          return true;
      }
      return false;
    }

    private bool HasParkingLanes(DynamicBuffer<Game.Net.SubNet> subNets)
    {
      for (int index = 0; index < subNets.Length; ++index)
      {
        DynamicBuffer<Game.Net.SubLane> buffer;
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.TryGetBuffer<Game.Net.SubLane>(subNets[index].m_SubNet, true, out buffer) && this.HasParkingLanes(buffer))
          return true;
      }
      return false;
    }

    private bool HasParkingLanes(DynamicBuffer<Game.Net.SubLane> subLanes)
    {
      for (int index = 0; index < subLanes.Length; ++index)
      {
        Entity subLane = subLanes[index].m_SubLane;
        Game.Net.ParkingLane component1;
        if (this.EntityManager.TryGetComponent<Game.Net.ParkingLane>(subLane, out component1))
        {
          if ((component1.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
            return true;
        }
        else
        {
          Game.Net.ConnectionLane component2;
          if (this.EntityManager.TryGetComponent<Game.Net.ConnectionLane>(subLane, out component2) && (component2.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
            return true;
        }
      }
      return false;
    }

    private List<UIPolicy> BindCityPolicies()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CityPolicies.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.FindAndSortPolicies(this.m_CitySystem.City, this.m_CityPoliciesQuery, this.m_CityPolicies);
      // ISSUE: reference to a compiler-generated field
      return this.m_CityPolicies;
    }

    private void WriteCityPolicies(IJsonWriter writer, List<UIPolicy> policies)
    {
      writer.ArrayBegin(policies.Count);
      foreach (UIPolicy policy in policies)
      {
        // ISSUE: reference to a compiler-generated field
        policy.Write(this.m_PrefabUISystem, writer);
      }
      writer.ArrayEnd();
    }

    public bool GatherSelectedInfoPolicies(Entity target)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoPolicies.Clear();
      if (this.EntityManager.HasComponent<Building>(target))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindAndSortPolicies(target, this.m_BuildingPoliciesQuery, this.m_SelectedInfoPolicies);
      }
      else if (this.EntityManager.HasComponent<District>(target))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindAndSortPolicies(target, this.m_DistrictPoliciesQuery, this.m_SelectedInfoPolicies);
      }
      else if (this.EntityManager.HasComponent<Route>(target))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FindAndSortPolicies(target, this.m_RoutePoliciesQuery, this.m_SelectedInfoPolicies);
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_SelectedInfoPolicies.Count > 0;
    }

    public void BindDistrictPolicies(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BindPolicies(binder, this.m_SelectedInfoPolicies);
    }

    public void BindBuildingPolicies(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BindPolicies(binder, this.m_SelectedInfoPolicies);
    }

    public void BindRoutePolicies(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.BindPolicies(binder, this.m_SelectedInfoPolicies);
    }

    private void BindPolicies(IJsonWriter binder, List<UIPolicy> list)
    {
      binder.ArrayBegin(list.Count);
      for (int index = 0; index < list.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        list[index].Write(this.m_PrefabUISystem, binder);
      }
      binder.ArrayEnd();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public PoliciesUISystem()
    {
    }

    internal static class BindingNames
    {
      internal const string kCityPolicies = "cityPolicies";
      internal const string kSetPolicy = "setPolicy";
      internal const string kSetCityPolicy = "setCityPolicy";
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PolicySliderData> __Game_Prefabs_PolicySliderData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PolicySliderData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PolicySliderData>(true);
      }
    }
  }
}
