// Decompiled with JetBrains decompiler
// Type: Game.Serialization.PrimaryPrefabReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Effects;
using Game.Net;
using Game.Objects;
using Game.Policies;
using Game.Prefabs;
using Game.Rendering;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class PrimaryPrefabReferencesSystem : GameSystemBase
  {
    private CheckPrefabReferencesSystem m_CheckPrefabReferencesSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ClimateSystem m_ClimateSystem;
    private TerrainMaterialSystem m_TerrainMaterialSystem;
    private EntityQuery m_PrefabRefQuery;
    private EntityQuery m_SetLevelQuery;
    private EntityQuery m_CompanyDataQuery;
    private EntityQuery m_PolicyQuery;
    private EntityQuery m_ActualBudgetQuery;
    private EntityQuery m_ServiceBudgetQuery;
    private EntityQuery m_VehicleModelQuery;
    private EntityQuery m_EditorContainerQuery;
    private EntityQuery m_AtmosphereQuery;
    private EntityQuery m_BiomeQuery;
    private EntityQuery m_ChirpQuery;
    private EntityQuery m_SubReplacementQuery;
    private PrimaryPrefabReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CheckPrefabReferencesSystem = this.World.GetOrCreateSystemManaged<CheckPrefabReferencesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainMaterialSystem = this.World.GetOrCreateSystemManaged<TerrainMaterialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabRefQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<NetCompositionData>(), ComponentType.Exclude<EffectInstance>(), ComponentType.Exclude<LivePath>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SetLevelQuery = this.GetEntityQuery(ComponentType.ReadOnly<UnderConstruction>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<CompanyData>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Policy>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceBudgetQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceBudgetData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AtmosphereQuery = this.GetEntityQuery(ComponentType.ReadOnly<AtmosphereData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BiomeQuery = this.GetEntityQuery(ComponentType.ReadOnly<BiomeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleModelQuery = this.GetEntityQuery(ComponentType.ReadOnly<VehicleModel>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_EditorContainerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Tools.EditorContainer>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Triggers.Chirp>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubReplacementQuery = this.GetEntityQuery(ComponentType.ReadOnly<SubReplacement>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabReferences prefabReferences = this.m_CheckPrefabReferencesSystem.GetPrefabReferences((SystemBase) this, out dependencies1);
      JobHandle dependsOn1 = JobHandle.CombineDependencies(this.Dependency, dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixPrefabRefJob jobData1 = new PrimaryPrefabReferencesSystem.FixPrefabRefJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixUnderConstructionJob jobData2 = new PrimaryPrefabReferencesSystem.FixUnderConstructionJob()
      {
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixCompanyDataJob jobData3 = new PrimaryPrefabReferencesSystem.FixCompanyDataJob()
      {
        m_CompanyDataType = this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Policy_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixPolicyJob jobData4 = new PrimaryPrefabReferencesSystem.FixPolicyJob()
      {
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RW_BufferTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixServiceBudgetJob jobData5 = new PrimaryPrefabReferencesSystem.FixServiceBudgetJob()
      {
        m_BudgetType = this.__TypeHandle.__Game_Simulation_ServiceBudgetData_RW_BufferTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_AtmosphereData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixAtmosphereJob jobData6 = new PrimaryPrefabReferencesSystem.FixAtmosphereJob()
      {
        m_AtmosphereType = this.__TypeHandle.__Game_Simulation_AtmosphereData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_BiomeData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixBiomeJob jobData7 = new PrimaryPrefabReferencesSystem.FixBiomeJob()
      {
        m_BiomeType = this.__TypeHandle.__Game_Simulation_BiomeData_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleModel_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixVehicleModelJob jobData8 = new PrimaryPrefabReferencesSystem.FixVehicleModelJob()
      {
        m_VehicleModelType = this.__TypeHandle.__Game_Routes_VehicleModel_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixEditorContainerJob jobData9 = new PrimaryPrefabReferencesSystem.FixEditorContainerJob()
      {
        m_EditorContainerType = this.__TypeHandle.__Game_Tools_EditorContainer_RW_ComponentTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_ChirpEntity_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixChirpJob jobData10 = new PrimaryPrefabReferencesSystem.FixChirpJob()
      {
        m_ChirpType = this.__TypeHandle.__Game_Triggers_Chirp_RW_ComponentTypeHandle,
        m_ChirpEntityType = this.__TypeHandle.__Game_Triggers_ChirpEntity_RW_BufferTypeHandle,
        m_PrefabDatas = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubReplacement_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PrimaryPrefabReferencesSystem.FixSubReplacementJob jobData11 = new PrimaryPrefabReferencesSystem.FixSubReplacementJob()
      {
        m_SubReplacementType = this.__TypeHandle.__Game_Net_SubReplacement_RW_BufferTypeHandle,
        m_PrefabReferences = prefabReferences
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.ScheduleParallel<PrimaryPrefabReferencesSystem.FixPrefabRefJob>(this.m_PrefabRefQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = jobData2.ScheduleParallel<PrimaryPrefabReferencesSystem.FixUnderConstructionJob>(this.m_SetLevelQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle3 = jobData3.ScheduleParallel<PrimaryPrefabReferencesSystem.FixCompanyDataJob>(this.m_CompanyDataQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle4 = jobData4.ScheduleParallel<PrimaryPrefabReferencesSystem.FixPolicyJob>(this.m_PolicyQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle5 = jobData5.ScheduleParallel<PrimaryPrefabReferencesSystem.FixServiceBudgetJob>(this.m_ServiceBudgetQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle6 = jobData6.ScheduleParallel<PrimaryPrefabReferencesSystem.FixAtmosphereJob>(this.m_AtmosphereQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle7 = jobData7.ScheduleParallel<PrimaryPrefabReferencesSystem.FixBiomeJob>(this.m_BiomeQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle8 = jobData8.ScheduleParallel<PrimaryPrefabReferencesSystem.FixVehicleModelJob>(this.m_VehicleModelQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      EntityQuery editorContainerQuery = this.m_EditorContainerQuery;
      JobHandle dependsOn2 = dependsOn1;
      JobHandle job0 = jobData9.ScheduleParallel<PrimaryPrefabReferencesSystem.FixEditorContainerJob>(editorContainerQuery, dependsOn2);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle9 = jobData10.ScheduleParallel<PrimaryPrefabReferencesSystem.FixChirpJob>(this.m_ChirpQuery, dependsOn1);
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle10 = jobData11.ScheduleParallel<PrimaryPrefabReferencesSystem.FixSubReplacementJob>(this.m_SubReplacementQuery, dependsOn1);
      dependsOn1.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.defaultTheme = prefabReferences.Check(this.EntityManager, this.m_CityConfigurationSystem.defaultTheme);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem.loadedDefaultTheme = prefabReferences.Check(this.EntityManager, this.m_CityConfigurationSystem.loadedDefaultTheme);
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem.PatchReference(ref prefabReferences);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainMaterialSystem.PatchReferences(ref prefabReferences);
      JobHandle job1 = jobHandle8;
      JobHandle job2 = jobHandle1;
      JobHandle job3 = jobHandle3;
      JobHandle job4 = jobHandle4;
      JobHandle job5 = jobHandle5;
      JobHandle job6 = jobHandle2;
      JobHandle job7 = jobHandle6;
      JobHandle job8 = jobHandle7;
      JobHandle job9 = jobHandle9;
      JobHandle job10 = jobHandle10;
      JobHandle dependencies2 = JobUtils.CombineDependencies(job0, job1, job2, job3, job4, job5, job6, job7, job8, job9, job10);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CheckPrefabReferencesSystem.AddPrefabReferencesUser(dependencies2);
      this.Dependency = dependencies2;
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
    public PrimaryPrefabReferencesSystem()
    {
    }

    [BurstCompile]
    private struct FixPrefabRefJob : IJobChunk
    {
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray[index];
          if (prefabRef.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref prefabRef.m_Prefab);
            nativeArray[index] = prefabRef;
          }
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
    private struct FixUnderConstructionJob : IJobChunk
    {
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnderConstruction> nativeArray = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          UnderConstruction underConstruction = nativeArray[index];
          if (underConstruction.m_NewPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref underConstruction.m_NewPrefab);
            nativeArray[index] = underConstruction;
          }
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
    private struct FixCompanyDataJob : IJobChunk
    {
      public ComponentTypeHandle<CompanyData> m_CompanyDataType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<CompanyData> nativeArray = chunk.GetNativeArray<CompanyData>(ref this.m_CompanyDataType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          CompanyData companyData = nativeArray[index];
          if (companyData.m_Brand != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref companyData.m_Brand);
            nativeArray[index] = companyData;
          }
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
    private struct FixPolicyJob : IJobChunk
    {
      public BufferTypeHandle<Policy> m_PolicyType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<Policy> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            Policy policy = dynamicBuffer[index2];
            if (policy.m_Policy != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabReferences.Check(ref policy.m_Policy);
              dynamicBuffer[index2] = policy;
            }
          }
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
    private struct FixServiceBudgetJob : IJobChunk
    {
      public BufferTypeHandle<ServiceBudgetData> m_BudgetType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ServiceBudgetData> bufferAccessor = chunk.GetBufferAccessor<ServiceBudgetData>(ref this.m_BudgetType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<ServiceBudgetData> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            ServiceBudgetData serviceBudgetData = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref serviceBudgetData.m_Service);
            dynamicBuffer[index2] = serviceBudgetData;
          }
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
    private struct FixAtmosphereJob : IJobChunk
    {
      public ComponentTypeHandle<AtmosphereData> m_AtmosphereType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<AtmosphereData> nativeArray = chunk.GetNativeArray<AtmosphereData>(ref this.m_AtmosphereType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          AtmosphereData atmosphereData = nativeArray[index];
          if (atmosphereData.m_AtmospherePrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref atmosphereData.m_AtmospherePrefab);
          }
          nativeArray[index] = atmosphereData;
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
    private struct FixBiomeJob : IJobChunk
    {
      public ComponentTypeHandle<BiomeData> m_BiomeType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<BiomeData> nativeArray = chunk.GetNativeArray<BiomeData>(ref this.m_BiomeType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          BiomeData biomeData = nativeArray[index];
          if (biomeData.m_BiomePrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref biomeData.m_BiomePrefab);
          }
          nativeArray[index] = biomeData;
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
    private struct FixVehicleModelJob : IJobChunk
    {
      public ComponentTypeHandle<VehicleModel> m_VehicleModelType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<VehicleModel> nativeArray = chunk.GetNativeArray<VehicleModel>(ref this.m_VehicleModelType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          VehicleModel vehicleModel = nativeArray[index];
          if (vehicleModel.m_PrimaryPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref vehicleModel.m_PrimaryPrefab);
          }
          if (vehicleModel.m_SecondaryPrefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref vehicleModel.m_SecondaryPrefab);
          }
          nativeArray[index] = vehicleModel;
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
    private struct FixEditorContainerJob : IJobChunk
    {
      public ComponentTypeHandle<Game.Tools.EditorContainer> m_EditorContainerType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Tools.EditorContainer> nativeArray = chunk.GetNativeArray<Game.Tools.EditorContainer>(ref this.m_EditorContainerType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Game.Tools.EditorContainer editorContainer = nativeArray[index];
          if (editorContainer.m_Prefab != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref editorContainer.m_Prefab);
          }
          nativeArray[index] = editorContainer;
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
    private struct FixChirpJob : IJobChunk
    {
      public ComponentTypeHandle<Game.Triggers.Chirp> m_ChirpType;
      public BufferTypeHandle<ChirpEntity> m_ChirpEntityType;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabDatas;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Triggers.Chirp> nativeArray = chunk.GetNativeArray<Game.Triggers.Chirp>(ref this.m_ChirpType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ChirpEntity> bufferAccessor = chunk.GetBufferAccessor<ChirpEntity>(ref this.m_ChirpEntityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          ref Game.Triggers.Chirp local = ref nativeArray.ElementAt<Game.Triggers.Chirp>(index);
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabDatas.HasComponent(local.m_Sender))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_PrefabReferences.Check(ref local.m_Sender);
          }
        }
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<ChirpEntity> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            ref ChirpEntity local = ref dynamicBuffer.ElementAt(index2);
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabDatas.HasComponent(local.m_Entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabReferences.Check(ref local.m_Entity);
            }
          }
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
    private struct FixSubReplacementJob : IJobChunk
    {
      public BufferTypeHandle<SubReplacement> m_SubReplacementType;
      public PrefabReferences m_PrefabReferences;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubReplacement> bufferAccessor = chunk.GetBufferAccessor<SubReplacement>(ref this.m_SubReplacementType);
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          DynamicBuffer<SubReplacement> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            SubReplacement subReplacement = dynamicBuffer[index2];
            if (subReplacement.m_Prefab != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_PrefabReferences.Check(ref subReplacement.m_Prefab);
            }
            dynamicBuffer[index2] = subReplacement;
          }
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

    private struct TypeHandle
    {
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentTypeHandle;
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RW_ComponentTypeHandle;
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RW_ComponentTypeHandle;
      public BufferTypeHandle<Policy> __Game_Policies_Policy_RW_BufferTypeHandle;
      public BufferTypeHandle<ServiceBudgetData> __Game_Simulation_ServiceBudgetData_RW_BufferTypeHandle;
      public ComponentTypeHandle<AtmosphereData> __Game_Simulation_AtmosphereData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<BiomeData> __Game_Simulation_BiomeData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<VehicleModel> __Game_Routes_VehicleModel_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Triggers.Chirp> __Game_Triggers_Chirp_RW_ComponentTypeHandle;
      public BufferTypeHandle<ChirpEntity> __Game_Triggers_ChirpEntity_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      public BufferTypeHandle<SubReplacement> __Game_Net_SubReplacement_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RW_BufferTypeHandle = state.GetBufferTypeHandle<Policy>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ServiceBudgetData_RW_BufferTypeHandle = state.GetBufferTypeHandle<ServiceBudgetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_AtmosphereData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AtmosphereData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_BiomeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BiomeData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleModel_RW_ComponentTypeHandle = state.GetComponentTypeHandle<VehicleModel>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Tools.EditorContainer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_Chirp_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Triggers.Chirp>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Triggers_ChirpEntity_RW_BufferTypeHandle = state.GetBufferTypeHandle<ChirpEntity>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubReplacement_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubReplacement>();
      }
    }
  }
}
