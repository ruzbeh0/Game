// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.SelectVehiclesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class SelectVehiclesSection : InfoSectionBase
  {
    private PrefabUISystem m_PrefabUISystem;
    private ImageSystem m_ImageSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TransportVehicleSelectData m_TransportVehicleSelectData;
    private EntityQuery m_VehiclePrefabQuery;
    private EntityQuery m_DepotQuery;
    private NativeArray<int> m_Results;
    private SelectVehiclesSection.TypeHandle __TypeHandle;

    protected override string group => nameof (SelectVehiclesSection);

    private Entity primaryVehicle { get; set; }

    private Entity secondaryVehicle { get; set; }

    private NativeList<Entity> primaryVehicles { get; set; }

    private NativeList<Entity> secondaryVehicles { get; set; }

    protected override void Reset()
    {
      this.primaryVehicles.Clear();
      this.secondaryVehicles.Clear();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Results.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Results[index] = 0;
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData = new TransportVehicleSelectData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_DepotQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.TransportDepot>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<InstalledUpgrade>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehiclePrefabQuery = this.GetEntityQuery(TransportVehicleSelectData.GetEntityQueryDesc());
      this.primaryVehicles = new NativeList<Entity>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.secondaryVehicles = new NativeList<Entity>(20, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<int>(2, Allocator.Persistent);
      this.AddBinding((IBinding) new TriggerBinding<Entity, Entity>(this.group, "selectVehicles", (Action<Entity, Entity>) ((primary, secondary) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.CreateCommandBuffer().SetComponent<VehicleModel>(this.selectedEntity, this.EntityManager.GetComponentData<VehicleModel>(this.selectedEntity) with
        {
          m_PrimaryPrefab = primary,
          m_SecondaryPrefab = secondary
        });
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.RequestUpdate();
      })));
    }

    private void SetVehicleModel(Entity primary, Entity secondary)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().SetComponent<VehicleModel>(this.selectedEntity, this.EntityManager.GetComponentData<VehicleModel>(this.selectedEntity) with
      {
        m_PrimaryPrefab = primary,
        m_SecondaryPrefab = secondary
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.RequestUpdate();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.primaryVehicles.Dispose();
      this.secondaryVehicles.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<VehicleModel>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLineData>(this.selectedPrefab);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      if (!(this.visible = this.Visible()))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new SelectVehiclesSection.DepotsJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_InstalledUpgradesType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_TransportDepotDataFromEntity = this.__TypeHandle.__Game_Prefabs_TransportDepotData_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<SelectVehiclesSection.DepotsJob>(this.m_DepotQuery, this.Dependency).Complete();
      TransportLineData componentData = this.EntityManager.GetComponentData<TransportLineData>(this.selectedPrefab);
      // ISSUE: reference to a compiler-generated field
      bool flag = this.m_InfoUISystem.tooltipTags.Contains(TooltipTags.CargoRoute);
      JobHandle jobHandle1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PreUpdate((SystemBase) this, this.m_CityConfigurationSystem, this.m_VehiclePrefabQuery, Allocator.TempJob, out jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle2 = new SelectVehiclesSection.VehiclesListJob()
      {
        m_Resources = (flag ? Resource.Food : Resource.NoResource),
        m_EnergyTypes = ((EnergyTypes) this.m_Results[1]),
        m_TransportType = componentData.m_TransportType,
        m_PrimaryList = this.primaryVehicles,
        m_SecondaryList = this.secondaryVehicles,
        m_VehicleSelectData = this.m_TransportVehicleSelectData
      }.Schedule<SelectVehiclesSection.VehiclesListJob>(JobHandle.CombineDependencies(this.Dependency, jobHandle1));
      // ISSUE: reference to a compiler-generated field
      this.m_TransportVehicleSelectData.PostUpdate(jobHandle2);
      jobHandle2.Complete();
      this.visible = this.primaryVehicles.Length > 1 || this.secondaryVehicles.Length > 1;
    }

    protected override void OnProcess()
    {
      VehicleModel componentData = this.EntityManager.GetComponentData<VehicleModel>(this.selectedEntity);
      this.primaryVehicle = componentData.m_PrimaryPrefab;
      this.secondaryVehicle = componentData.m_SecondaryPrefab;
      this.tooltipTags.Add("TransportLine");
      this.tooltipTags.Add("CargoRoute");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("primaryVehicle");
      if (this.primaryVehicle == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.WriteVehicle(writer, this.primaryVehicle);
      }
      writer.PropertyName("secondaryVehicle");
      if (this.secondaryVehicle == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.WriteVehicle(writer, this.secondaryVehicle);
      }
      writer.PropertyName("primaryVehicles");
      IJsonWriter writer1 = writer;
      NativeList<Entity> nativeList = this.primaryVehicles;
      int length1 = nativeList.Length;
      writer1.ArrayBegin(length1);
      int index1 = 0;
      while (true)
      {
        int num = index1;
        nativeList = this.primaryVehicles;
        int length2 = nativeList.Length;
        if (num < length2)
        {
          IJsonWriter writer2 = writer;
          nativeList = this.primaryVehicles;
          Entity entity = nativeList[index1];
          // ISSUE: reference to a compiler-generated method
          this.WriteVehicle(writer2, entity);
          ++index1;
        }
        else
          break;
      }
      writer.ArrayEnd();
      writer.PropertyName("secondaryVehicles");
      if (this.EntityManager.HasComponent<TrainCarriageData>(this.primaryVehicle) && !this.EntityManager.HasComponent<MultipleUnitTrainData>(this.primaryVehicle))
      {
        IJsonWriter writer3 = writer;
        nativeList = this.secondaryVehicles;
        int length3 = nativeList.Length;
        writer3.ArrayBegin(length3);
        int index2 = 0;
        while (true)
        {
          int num = index2;
          nativeList = this.secondaryVehicles;
          int length4 = nativeList.Length;
          if (num < length4)
          {
            IJsonWriter writer4 = writer;
            nativeList = this.secondaryVehicles;
            Entity entity = nativeList[index2];
            // ISSUE: reference to a compiler-generated method
            this.WriteVehicle(writer4, entity);
            ++index2;
          }
          else
            break;
        }
        writer.ArrayEnd();
      }
      else
        writer.WriteNull();
    }

    private void WriteVehicle(IJsonWriter writer, Entity entity)
    {
      writer.TypeBegin(this.GetType().FullName + "+VehiclePrefab");
      writer.PropertyName(nameof (entity));
      writer.Write(entity);
      writer.PropertyName("id");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      writer.Write(this.m_PrefabSystem.GetPrefabName(entity));
      writer.PropertyName("locked");
      writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
      writer.PropertyName("requirements");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.BindPrefabRequirements(writer, entity);
      writer.PropertyName("thumbnail");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ImageSystem.GetThumbnail(entity) ?? this.m_ImageSystem.placeholderIcon);
      writer.TypeEnd();
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

    [UnityEngine.Scripting.Preserve]
    public SelectVehiclesSection()
    {
    }

    private enum Result
    {
      HasDepots,
      EnergyTypes,
      Count,
    }

    [BurstCompile]
    private struct DepotsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradesType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> m_TransportDepotDataFromEntity;
      public NativeArray<int> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradesType);
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] = 1;
        for (int index = 0; index < chunk.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity prefab = this.m_PrefabRefFromEntity[nativeArray[index]].m_Prefab;
          DynamicBuffer<InstalledUpgrade> upgrades = bufferAccessor[index];
          TransportDepotData componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_TransportDepotDataFromEntity.TryGetComponent(prefab, out componentData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          UpgradeUtils.CombineStats<TransportDepotData>(ref componentData, upgrades, ref this.m_PrefabRefFromEntity, ref this.m_TransportDepotDataFromEntity);
          // ISSUE: reference to a compiler-generated field
          ref NativeArray<int> local = ref this.m_Results;
          local[1] = (int) ((EnergyTypes) local[1] | componentData.m_EnergyTypes);
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct VehiclesListJob : IJob
    {
      public Resource m_Resources;
      public EnergyTypes m_EnergyTypes;
      public TransportType m_TransportType;
      public NativeList<Entity> m_PrimaryList;
      public NativeList<Entity> m_SecondaryList;
      [ReadOnly]
      public TransportVehicleSelectData m_VehicleSelectData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_VehicleSelectData.ListVehicles(this.m_TransportType, this.m_EnergyTypes, PublicTransportPurpose.TransportLine, this.m_Resources, this.m_PrimaryList, this.m_SecondaryList);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TransportDepotData> __Game_Prefabs_TransportDepotData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportDepotData_RO_ComponentLookup = state.GetComponentLookup<TransportDepotData>(true);
      }
    }
  }
}
