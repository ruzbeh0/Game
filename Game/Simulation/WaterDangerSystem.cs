// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterDangerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Events;
using Game.Objects;
using Game.Prefabs;
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
  public class WaterDangerSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private CitySystem m_CitySystem;
    private SearchSystem m_ObjectSearchSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_WaterLevelChangeQuery;
    private EntityArchetype m_EndangerArchetype;
    private WaterDangerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterLevelChangeQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterLevelChange>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EndangerArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Endanger>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_WaterLevelChangeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_DangerLevel_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new WaterDangerSystem.WaterDangerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_WaterLevelChangeType = this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle,
        m_DurationType = this.__TypeHandle.__Game_Events_Duration_RO_ComponentTypeHandle,
        m_DangerLevelType = this.__TypeHandle.__Game_Events_DangerLevel_RW_ComponentTypeHandle,
        m_InDangerData = this.__TypeHandle.__Game_Events_InDanger_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_EmergencyShelterData = this.__TypeHandle.__Game_Buildings_EmergencyShelter_RO_ComponentLookup,
        m_PrefabWaterLevelChangeData = this.__TypeHandle.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_City = this.m_CitySystem.City,
        m_EndangerArchetype = this.m_EndangerArchetype,
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<WaterDangerSystem.WaterDangerJob>(this.m_WaterLevelChangeQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public WaterDangerSystem()
    {
    }

    [BurstCompile]
    private struct WaterDangerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<WaterLevelChange> m_WaterLevelChangeType;
      [ReadOnly]
      public ComponentTypeHandle<Duration> m_DurationType;
      public ComponentTypeHandle<Game.Events.DangerLevel> m_DangerLevelType;
      [ReadOnly]
      public ComponentLookup<InDanger> m_InDangerData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> m_PrefabWaterLevelChangeData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public EntityArchetype m_EndangerArchetype;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterLevelChange> nativeArray3 = chunk.GetNativeArray<WaterLevelChange>(ref this.m_WaterLevelChangeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Duration> nativeArray4 = chunk.GetNativeArray<Duration>(ref this.m_DurationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Events.DangerLevel> nativeArray5 = chunk.GetNativeArray<Game.Events.DangerLevel>(ref this.m_DangerLevelType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity eventEntity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          WaterLevelChange waterLevelChange = nativeArray3[index];
          Duration duration = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          WaterLevelChangeData waterLevelChangeData = this.m_PrefabWaterLevelChangeData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          if (this.m_SimulationFrame < duration.m_EndFrame && waterLevelChangeData.m_DangerFlags != (DangerFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.FindEndangeredObjects(unfilteredChunkIndex, eventEntity, duration, waterLevelChange, waterLevelChangeData);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_SimulationFrame > duration.m_StartFrame && this.m_SimulationFrame < duration.m_EndFrame;
          nativeArray5[index] = new Game.Events.DangerLevel(flag ? waterLevelChangeData.m_DangerLevel : 0.0f);
        }
      }

      private void FindEndangeredObjects(
        int jobIndex,
        Entity eventEntity,
        Duration duration,
        WaterLevelChange waterLevelChange,
        WaterLevelChangeData waterLevelChangeData)
      {
        float y = 10f;
        float num1 = 0.0f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        CityUtils.ApplyModifier(ref y, cityModifier, CityModifierType.DisasterWarningTime);
        // ISSUE: reference to a compiler-generated field
        if (duration.m_StartFrame > this.m_SimulationFrame)
        {
          // ISSUE: reference to a compiler-generated field
          y -= (float) (duration.m_StartFrame - this.m_SimulationFrame) / 60f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          num1 = (float) (this.m_SimulationFrame - duration.m_StartFrame) / 60f;
        }
        float num2 = math.max(0.0f, y);
        float num3 = (float) ((long) duration.m_EndFrame - (long) WaterLevelChangeSystem.TsunamiEndDelay - (long) duration.m_StartFrame) / 60f;
        float num4 = WaterSystem.WaveSpeed * 60f;
        float _max = num1 * num4;
        float _min = _max - num3 * num4;
        // ISSUE: reference to a compiler-generated field
        float2 _a = (float) (WaterSystem.kMapSize / 2) * -waterLevelChange.m_Direction;
        Line2 line2 = new Line2(_a, _a + MathUtils.Right(waterLevelChange.m_Direction));
        Bounds1 bounds1 = new Bounds1(_min, _max);
        bounds1.max += num2 * num4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        WaterDangerSystem.WaterDangerJob.EndangeredStaticObjectIterator iterator = new WaterDangerSystem.WaterDangerJob.EndangeredStaticObjectIterator()
        {
          m_JobIndex = jobIndex,
          m_SimulationFrame = this.m_SimulationFrame,
          m_DangerSpeed = num4,
          m_DangerHeight = waterLevelChange.m_DangerHeight,
          m_PredictionDistance = bounds1,
          m_Event = eventEntity,
          m_StartLine = line2,
          m_WaterLevelChangeData = waterLevelChangeData,
          m_BuildingData = this.m_BuildingData,
          m_EmergencyShelterData = this.m_EmergencyShelterData,
          m_InDangerData = this.m_InDangerData,
          m_EndangerArchetype = this.m_EndangerArchetype,
          m_CommandBuffer = this.m_CommandBuffer
        };
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<WaterDangerSystem.WaterDangerJob.EndangeredStaticObjectIterator>(ref iterator);
      }

      private static Bounds1 GetDistanceBounds(Bounds2 bounds, Line2 line)
      {
        float t;
        float4 x = new float4(MathUtils.Distance(line, bounds.min, out t), MathUtils.Distance(line, new float2(bounds.min.x, bounds.max.y), out t), MathUtils.Distance(line, bounds.max, out t), MathUtils.Distance(line, new float2(bounds.max.x, bounds.min.y), out t));
        Bounds1 distanceBounds = new Bounds1(math.cmin(x), math.cmax(x));
        if (MathUtils.Intersect(bounds, line, out float2 _))
          distanceBounds |= 0.0f;
        return distanceBounds;
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

      private struct EndangeredStaticObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public int m_JobIndex;
        public uint m_SimulationFrame;
        public float m_DangerSpeed;
        public float m_DangerHeight;
        public Bounds1 m_PredictionDistance;
        public Entity m_Event;
        public Line2 m_StartLine;
        public WaterLevelChangeData m_WaterLevelChangeData;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<Game.Buildings.EmergencyShelter> m_EmergencyShelterData;
        public ComponentLookup<InDanger> m_InDangerData;
        public EntityArchetype m_EndangerArchetype;
        public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return (double) bounds.m_Bounds.min.y < (double) this.m_DangerHeight && MathUtils.Intersect(this.m_PredictionDistance, WaterDangerSystem.WaterDangerJob.GetDistanceBounds(bounds.m_Bounds.xz, this.m_StartLine));
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) bounds.m_Bounds.min.y >= (double) this.m_DangerHeight)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Bounds1 distanceBounds = WaterDangerSystem.WaterDangerJob.GetDistanceBounds(bounds.m_Bounds.xz, this.m_StartLine);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_PredictionDistance, distanceBounds) || !this.m_BuildingData.HasComponent(item))
            return;
          // ISSUE: reference to a compiler-generated field
          DangerFlags flags = this.m_WaterLevelChangeData.m_DangerFlags;
          // ISSUE: reference to a compiler-generated field
          if ((flags & DangerFlags.Evacuate) != (DangerFlags) 0 && this.m_EmergencyShelterData.HasComponent(item))
            flags = flags & ~DangerFlags.Evacuate | DangerFlags.StayIndoors;
          // ISSUE: reference to a compiler-generated field
          if (this.m_InDangerData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated field
            InDanger inDanger = this.m_InDangerData[item];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (inDanger.m_EndFrame >= this.m_SimulationFrame + 64U && (inDanger.m_Event == this.m_Event || !EventUtils.IsWorse(flags, inDanger.m_Flags)))
              return;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num = (float) (30.0 + ((double) distanceBounds.max - (double) this.m_PredictionDistance.min) / (double) this.m_DangerSpeed);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Endanger>(this.m_JobIndex, this.m_CommandBuffer.CreateEntity(this.m_JobIndex, this.m_EndangerArchetype), new Endanger()
          {
            m_Event = this.m_Event,
            m_Target = item,
            m_Flags = flags,
            m_EndFrame = this.m_SimulationFrame + 64U + (uint) ((double) num * 60.0)
          });
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<WaterLevelChange> __Game_Events_WaterLevelChange_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Duration> __Game_Events_Duration_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Events.DangerLevel> __Game_Events_DangerLevel_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<InDanger> __Game_Events_InDanger_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.EmergencyShelter> __Game_Buildings_EmergencyShelter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterLevelChangeData> __Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RW_ComponentTypeHandle = state.GetComponentTypeHandle<WaterLevelChange>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Duration_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Duration>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_DangerLevel_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Events.DangerLevel>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_InDanger_RO_ComponentLookup = state.GetComponentLookup<InDanger>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyShelter_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.EmergencyShelter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterLevelChangeData_RO_ComponentLookup = state.GetComponentLookup<WaterLevelChangeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
