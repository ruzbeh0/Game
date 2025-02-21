// Decompiled with JetBrains decompiler
// Type: Game.Simulation.RoadSafetySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class RoadSafetySystem : GameSystemBase
  {
    private const int UPDATES_PER_DAY = 64;
    private TimeSystem m_TimeSystem;
    private CitySystem m_CitySystem;
    private LightingSystem m_LightingSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_RoadQuery;
    private EntityQuery m_AccidentPrefabQuery;
    private RoadSafetySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TimeSystem = this.World.GetOrCreateSystemManaged<TimeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_RoadQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Composition>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Road>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AccidentPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<EventData>(), ComponentType.ReadOnly<TrafficAccidentData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RoadQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AccidentPrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      float num = this.m_TimeSystem.normalizedTime * 4f;
      float4 float4 = math.saturate(new float4(math.max(num - 3f, 1f - num), 1f - math.abs(num - new float3(1f, 2f, 3f))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      JobHandle producerJob = new RoadSafetySystem.RoadSafetyJob()
      {
        m_FirePrefabChunks = this.m_AccidentPrefabQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NetConditionType = this.__TypeHandle.__Game_Net_NetCondition_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_BorderDistrictType = this.__TypeHandle.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle,
        m_PrefabEventType = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentTypeHandle,
        m_PrefabTrafficAccidentType = this.__TypeHandle.__Game_Prefabs_TrafficAccidentData_RO_ComponentTypeHandle,
        m_LockedType = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentTypeHandle,
        m_RoadCompositionData = this.__TypeHandle.__Game_Prefabs_RoadComposition_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RO_BufferLookup,
        m_City = this.m_CitySystem.City,
        m_RandomSeed = RandomSeed.Next(),
        m_TimeFactors = float4,
        m_Brightness = this.m_LightingSystem.dayLightBrightness,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<RoadSafetySystem.RoadSafetyJob>(this.m_RoadQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public RoadSafetySystem()
    {
    }

    [BurstCompile]
    private struct RoadSafetyJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_FirePrefabChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<NetCondition> m_NetConditionType;
      [ReadOnly]
      public ComponentTypeHandle<Road> m_RoadType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<BorderDistrict> m_BorderDistrictType;
      [ReadOnly]
      public ComponentTypeHandle<EventData> m_PrefabEventType;
      [ReadOnly]
      public ComponentTypeHandle<TrafficAccidentData> m_PrefabTrafficAccidentType;
      [ReadOnly]
      public ComponentTypeHandle<Locked> m_LockedType;
      [ReadOnly]
      public ComponentLookup<RoadComposition> m_RoadCompositionData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public float4 m_TimeFactors;
      [ReadOnly]
      public float m_Brightness;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        if (random.NextInt(64) != 0)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetCondition> nativeArray2 = chunk.GetNativeArray<NetCondition>(ref this.m_NetConditionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Road> nativeArray3 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray4 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BorderDistrict> nativeArray5 = chunk.GetNativeArray<BorderDistrict>(ref this.m_BorderDistrictType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          NetCondition netCondition = nativeArray2[index];
          Road road = nativeArray3[index];
          Composition composition = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          float duration = math.dot(road.m_TrafficFlowDuration0 + road.m_TrafficFlowDuration1, this.m_TimeFactors) * 2.66666675f;
          // ISSUE: reference to a compiler-generated field
          float num1 = math.dot(road.m_TrafficFlowDistance0 + road.m_TrafficFlowDistance1, this.m_TimeFactors) * 2.66666675f;
          RoadComposition componentData;
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 >= 0.0099999997764825821 && this.m_RoadCompositionData.TryGetComponent(composition.m_Edge, out componentData))
          {
            float trafficFlowSpeed = NetUtils.GetTrafficFlowSpeed(duration, num1);
            float x1 = math.select(0.7f, 0.9f, (componentData.m_Flags & Game.Prefabs.RoadFlags.HasStreetLights) != (Game.Prefabs.RoadFlags) 0 && (road.m_Flags & Game.Net.RoadFlags.LightsOff) == (Game.Net.RoadFlags) 0);
            float num2 = 500f / math.sqrt(num1);
            num2 *= math.lerp(0.5f, 1f, trafficFlowSpeed);
            num2 *= math.lerp(1f, 0.75f, math.csum(netCondition.m_Wear) * 0.05f);
            // ISSUE: reference to a compiler-generated field
            num2 *= math.lerp(x1, 1f, math.min(1f, this.m_Brightness * 2f));
            if ((componentData.m_Flags & Game.Prefabs.RoadFlags.SeparatedCarriageways) != (Game.Prefabs.RoadFlags) 0)
              num2 *= 1.1f;
            if ((componentData.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) == (Game.Prefabs.RoadFlags) 0 && nativeArray5.Length != 0)
            {
              float2 x2 = (float2) num2;
              BorderDistrict borderDistrict = nativeArray5[index];
              DynamicBuffer<DistrictModifier> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_DistrictModifiers.TryGetBuffer(borderDistrict.m_Left, out bufferData))
                AreaUtils.ApplyModifier(ref x2.x, bufferData, DistrictModifierType.StreetTrafficSafety);
              // ISSUE: reference to a compiler-generated field
              if (this.m_DistrictModifiers.TryGetBuffer(borderDistrict.m_Right, out bufferData))
                AreaUtils.ApplyModifier(ref x2.y, bufferData, DistrictModifierType.StreetTrafficSafety);
              num2 = (double) math.cmax(x2) < (double) num2 ? math.min(num2, math.cmax(x2)) : math.max(num2, math.cmin(x2));
            }
            if ((componentData.m_Flags & Game.Prefabs.RoadFlags.UseHighwayRules) != (Game.Prefabs.RoadFlags) 0)
              CityUtils.ApplyModifier(ref num2, cityModifier, CityModifierType.HighwayTrafficSafety);
            // ISSUE: reference to a compiler-generated method
            this.TryStartAccident(unfilteredChunkIndex, ref random, entity, num2, EventTargetType.Road);
          }
        }
      }

      private void TryStartAccident(
        int jobIndex,
        ref Random random,
        Entity entity,
        float roadSafety,
        EventTargetType targetType)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_FirePrefabChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk firePrefabChunk = this.m_FirePrefabChunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = firePrefabChunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<EventData> nativeArray2 = firePrefabChunk.GetNativeArray<EventData>(ref this.m_PrefabEventType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<TrafficAccidentData> nativeArray3 = firePrefabChunk.GetNativeArray<TrafficAccidentData>(ref this.m_PrefabTrafficAccidentType);
          // ISSUE: reference to a compiler-generated field
          EnabledMask enabledMask = firePrefabChunk.GetEnabledMask<Locked>(ref this.m_LockedType);
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            TrafficAccidentData trafficAccidentData = nativeArray3[index2];
            if (trafficAccidentData.m_RandomSiteType == targetType && (!enabledMask.EnableBit.IsValid || !enabledMask[index2]))
            {
              float num = trafficAccidentData.m_OccurenceProbability / math.max(1f, roadSafety);
              if ((double) random.NextFloat(1f) < (double) num)
              {
                // ISSUE: reference to a compiler-generated method
                this.CreateAccidentEvent(jobIndex, entity, nativeArray1[index2], nativeArray2[index2], trafficAccidentData);
                return;
              }
            }
          }
        }
      }

      private void CreateAccidentEvent(
        int jobIndex,
        Entity targetEntity,
        Entity eventPrefab,
        EventData eventData,
        TrafficAccidentData trafficAccidentData)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex, eventData.m_Archetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(eventPrefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetBuffer<TargetElement>(jobIndex, entity).Add(new TargetElement(targetEntity));
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
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCondition> __Game_Net_NetCondition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Road> __Game_Net_Road_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<BorderDistrict> __Game_Areas_BorderDistrict_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<EventData> __Game_Prefabs_EventData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrafficAccidentData> __Game_Prefabs_TrafficAccidentData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Locked> __Game_Prefabs_Locked_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<RoadComposition> __Game_Prefabs_RoadComposition_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NetCondition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCondition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Road>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_BorderDistrict_RO_ComponentTypeHandle = state.GetComponentTypeHandle<BorderDistrict>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrafficAccidentData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrafficAccidentData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Locked>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RoadComposition_RO_ComponentLookup = state.GetComponentLookup<RoadComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RO_BufferLookup = state.GetBufferLookup<DistrictModifier>(true);
      }
    }
  }
}
