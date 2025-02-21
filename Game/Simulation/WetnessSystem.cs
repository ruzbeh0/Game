// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WetnessSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
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
  public class WetnessSystem : GameSystemBase
  {
    public const int SNOW_REQUIREMENT_LIMIT = 15;
    private ClimateSystem m_ClimateSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_SurfaceQuery;
    private EntityArchetype m_SubObjectEventArchetype;
    private WetnessSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SurfaceQuery = this.GetEntityQuery(ComponentType.ReadWrite<Surface>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Overridden>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubObjectEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<SubObjectsUpdated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SurfaceQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      float4 x1 = new float4();
      float4 x2 = new float4();
      float4 x3 = new float4();
      // ISSUE: reference to a compiler-generated field
      float precipitation = (float) this.m_ClimateSystem.precipitation;
      // ISSUE: reference to a compiler-generated field
      float temperature = (float) this.m_ClimateSystem.temperature;
      if ((double) temperature > 0.0)
      {
        x1.x = math.sqrt(precipitation);
        x1.z = math.sqrt(x1.x);
        x2.x = precipitation * 0.1f;
        x2.z = precipitation * 0.01f;
        x3.x = (float) ((1.0 - (double) precipitation) * 0.05000000074505806);
        x3.y = temperature * 0.01f;
        x3.z = (float) ((1.0 - (double) precipitation) * 0.004999999888241291);
        x3.w = temperature * (1f / 1000f);
      }
      else
      {
        x1.yw = (float2) 1f;
        x2.y = precipitation * 0.05f;
        x2.w = precipitation * 0.005f;
        x3.x = temperature * -0.01f;
        x3.z = temperature * (-1f / 1000f);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Surface_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new WetnessSystem.WetnessJob()
      {
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_ObjectSurfaceType = this.__TypeHandle.__Game_Objects_Surface_RW_ComponentTypeHandle,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_SubObjectEventArchetype = this.m_SubObjectEventArchetype,
        m_RandomSeed = RandomSeed.Next(),
        m_TargetWetness = math.saturate(x1),
        m_WetSpeed = math.saturate(x2),
        m_DrySpeed = (-math.saturate(x3)),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<WetnessSystem.WetnessJob>(this.m_SurfaceQuery, this.Dependency);
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
    public WetnessSystem()
    {
    }

    [BurstCompile]
    private struct WetnessJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      public ComponentTypeHandle<Surface> m_ObjectSurfaceType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public EntityArchetype m_SubObjectEventArchetype;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public float4 m_TargetWetness;
      [ReadOnly]
      public float4 m_WetSpeed;
      [ReadOnly]
      public float4 m_DrySpeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Surface> nativeArray2 = chunk.GetNativeArray<Surface>(ref this.m_ObjectSurfaceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Game.Objects.SubObject>(ref this.m_SubObjectType) && !chunk.Has<Owner>(ref this.m_OwnerType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          ref Surface local = ref nativeArray2.ElementAt<Surface>(index);
          ObjectRequirementFlags requirementFlags1 = (ObjectRequirementFlags) 0;
          if (local.m_AccumulatedSnow >= (byte) 15)
            requirementFlags1 |= ObjectRequirementFlags.Snow;
          int4 int4 = new int4((int) local.m_Wetness, (int) local.m_SnowAmount, (int) local.m_AccumulatedWetness, (int) local.m_AccumulatedSnow);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float4 float4 = math.clamp(this.m_TargetWetness - (float4) int4 * 0.003921569f, this.m_DrySpeed, this.m_WetSpeed) * random.NextFloat4((float4) 0.8f, (float4) 1f);
          int4 = math.clamp(int4 + MathUtils.RoundToIntRandom(ref random, float4 * (float) byte.MaxValue), (int4) 0, (int4) (int) byte.MaxValue);
          local.m_Wetness = (byte) int4.x;
          local.m_SnowAmount = (byte) int4.y;
          local.m_AccumulatedWetness = (byte) int4.z;
          local.m_AccumulatedSnow = (byte) int4.w;
          ObjectRequirementFlags requirementFlags2 = (ObjectRequirementFlags) 0;
          if (local.m_AccumulatedSnow >= (byte) 15)
            requirementFlags2 |= ObjectRequirementFlags.Snow;
          ObjectRequirementFlags requirementFlags3 = requirementFlags2 ^ requirementFlags1;
          ObjectGeometryData componentData;
          // ISSUE: reference to a compiler-generated field
          if (flag && requirementFlags3 != (ObjectRequirementFlags) 0 && this.m_PrefabObjectGeometryData.TryGetComponent(nativeArray3[index].m_Prefab, out componentData) && (componentData.m_SubObjectMask & requirementFlags3) != (ObjectRequirementFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_SubObjectEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<SubObjectsUpdated>(unfilteredChunkIndex, entity, new SubObjectsUpdated(nativeArray1[index]));
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
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      public ComponentTypeHandle<Surface> __Game_Objects_Surface_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Surface_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Surface>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
      }
    }
  }
}
