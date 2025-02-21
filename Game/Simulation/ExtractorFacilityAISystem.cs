// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ExtractorFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
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
  public class ExtractorFacilityAISystem : GameSystemBase
  {
    private EntityQuery m_BuildingQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private ExtractorFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 224;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.ExtractorFacility>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ExtractorFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle producerJob = new ExtractorFacilityAISystem.ExtractorFacilityTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ExtractorFacilityType = this.__TypeHandle.__Game_Buildings_ExtractorFacility_RW_ComponentTypeHandle,
        m_PointOfInterestType = this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_BuildingEfficiencyData = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_Building = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabExtractorFacilityData = this.__TypeHandle.__Game_Prefabs_ExtractorFacilityData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next()
      }.Schedule<ExtractorFacilityAISystem.ExtractorFacilityTickJob>(this.m_BuildingQuery, this.Dependency);
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
    public ExtractorFacilityAISystem()
    {
    }

    [BurstCompile]
    private struct ExtractorFacilityTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Game.Buildings.ExtractorFacility> m_ExtractorFacilityType;
      public ComponentTypeHandle<PointOfInterest> m_PointOfInterestType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Building> m_Building;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyData;
      [ReadOnly]
      public ComponentLookup<ExtractorFacilityData> m_PrefabExtractorFacilityData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
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
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.ExtractorFacility> nativeArray4 = chunk.GetNativeArray<Game.Buildings.ExtractorFacility>(ref this.m_ExtractorFacilityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PointOfInterest> nativeArray5 = chunk.GetNativeArray<PointOfInterest>(ref this.m_PointOfInterestType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          PrefabRef prefabRef = nativeArray3[index];
          ref Game.Buildings.ExtractorFacility local1 = ref nativeArray4.ElementAt<Game.Buildings.ExtractorFacility>(index);
          ref PointOfInterest local2 = ref nativeArray5.ElementAt<PointOfInterest>(index);
          Owner owner = new Owner();
          if (nativeArray2.Length != 0)
            owner = nativeArray2[index];
          // ISSUE: reference to a compiler-generated method
          this.Tick(unfilteredChunkIndex, entity, ref random, ref local1, ref local2, owner, prefabRef);
        }
      }

      private void Tick(
        int jobIndex,
        Entity entity,
        ref Random random,
        ref Game.Buildings.ExtractorFacility extractorFacility,
        ref PointOfInterest pointOfInterest,
        Owner owner,
        PrefabRef prefabRef)
      {
        ExtractorFacilityData prefabExtractorFacilityData = new ExtractorFacilityData();
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabExtractorFacilityData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          prefabExtractorFacilityData = this.m_PrefabExtractorFacilityData[prefabRef.m_Prefab];
        }
        Entity entity1 = owner.m_Owner;
        Owner componentData1;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity1, out componentData1))
          entity1 = componentData1.m_Owner;
        Attachment componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachmentData.TryGetComponent(entity1, out componentData2))
          entity1 = componentData2.m_Attached;
        // ISSUE: reference to a compiler-generated field
        float efficiency = BuildingUtils.GetEfficiency(entity1, ref this.m_BuildingEfficiencyData);
        Building componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Building.TryGetComponent(entity1, out componentData3) && ((componentData3.m_Flags ^ extractorFacility.m_MainBuildingFlags) & Game.Buildings.BuildingFlags.LowEfficiency) != Game.Buildings.BuildingFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, entity);
          extractorFacility.m_MainBuildingFlags = componentData3.m_Flags;
        }
        if ((double) random.NextFloat(1f) >= (double) efficiency)
          return;
        if ((extractorFacility.m_Flags & ExtractorFlags.Working) != (ExtractorFlags) 0)
        {
          if (--extractorFacility.m_Timer != (byte) 0)
            return;
          extractorFacility.m_Flags &= ~ExtractorFlags.Working;
          if ((double) prefabExtractorFacilityData.m_RotationRange.max != (double) prefabExtractorFacilityData.m_RotationRange.min)
          {
            // ISSUE: reference to a compiler-generated method
            this.StartRotating(entity, ref random, ref extractorFacility, ref pointOfInterest, prefabRef, prefabExtractorFacilityData);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.StartWorking(entity, ref random, ref extractorFacility, ref pointOfInterest, prefabExtractorFacilityData);
          }
        }
        else if ((extractorFacility.m_Flags & ExtractorFlags.Rotating) != (ExtractorFlags) 0)
        {
          if (--extractorFacility.m_Timer != (byte) 0)
            return;
          extractorFacility.m_Flags &= ~ExtractorFlags.Rotating;
          // ISSUE: reference to a compiler-generated method
          this.StartWorking(entity, ref random, ref extractorFacility, ref pointOfInterest, prefabExtractorFacilityData);
        }
        else if ((double) prefabExtractorFacilityData.m_RotationRange.max != (double) prefabExtractorFacilityData.m_RotationRange.min)
        {
          // ISSUE: reference to a compiler-generated method
          this.StartRotating(entity, ref random, ref extractorFacility, ref pointOfInterest, prefabRef, prefabExtractorFacilityData);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.StartWorking(entity, ref random, ref extractorFacility, ref pointOfInterest, prefabExtractorFacilityData);
        }
      }

      private void StartRotating(
        Entity entity,
        ref Random random,
        ref Game.Buildings.ExtractorFacility extractorFacility,
        ref PointOfInterest pointOfInterest,
        PrefabRef prefabRef,
        ExtractorFacilityData prefabExtractorFacilityData)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
        float x1 = random.NextFloat(prefabExtractorFacilityData.m_RotationRange.min, prefabExtractorFacilityData.m_RotationRange.max);
        float z = objectGeometryData.m_Bounds.max.z;
        float3 v = new float3();
        v.x = math.sin(x1);
        v.y = prefabExtractorFacilityData.m_HeightOffset.max;
        v.z = math.cos(x1);
        float2 t;
        float2 float2_1;
        if (pointOfInterest.m_IsValid)
        {
          float2 xz = math.rotate(math.inverse(transform.m_Rotation), pointOfInterest.m_Position - transform.m_Position).xz;
          t = new float2();
          float2 defaultvalue = t;
          float2_1 = math.normalizesafe(xz, defaultvalue);
        }
        else
          float2_1 = math.forward().xz;
        if ((double) prefabExtractorFacilityData.m_RotationRange.max - (double) prefabExtractorFacilityData.m_RotationRange.min < 6.2821855545043945)
        {
          float x2 = MathUtils.Center(prefabExtractorFacilityData.m_RotationRange) + 3.14159274f;
          float2 float2_2 = new float2(math.sin(x2), math.cos(x2));
          if ((double) MathUtils.Distance(new Line2.Segment(float2_1, v.xz), new Line2.Segment(float2.zero, float2_2), out t) < 1.0 / 1000.0)
            v.xz = float2_2 * (math.dot(v.xz, float2_2) * 2f) - v.xz;
        }
        float num = MathUtils.RotationAngle(v.xz, float2_1);
        v.xz *= z;
        pointOfInterest.m_Position = transform.m_Position + math.rotate(transform.m_Rotation, v);
        pointOfInterest.m_IsValid = true;
        extractorFacility.m_Flags |= ExtractorFlags.Rotating;
        extractorFacility.m_Timer = (byte) MathUtils.RoundToIntRandom(ref random, (float) (2.0 + (double) num * 2.0));
      }

      private void StartWorking(
        Entity entity,
        ref Random random,
        ref Game.Buildings.ExtractorFacility extractorFacility,
        ref PointOfInterest pointOfInterest,
        ExtractorFacilityData prefabExtractorFacilityData)
      {
        extractorFacility.m_Flags |= ExtractorFlags.Working;
        extractorFacility.m_Timer = (byte) random.NextInt(10, 31);
        if (!pointOfInterest.m_IsValid)
          return;
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[entity];
        float y = math.rotate(math.inverse(transform.m_Rotation), pointOfInterest.m_Position - transform.m_Position).y;
        pointOfInterest.m_Position += math.rotate(transform.m_Rotation, new float3(0.0f, prefabExtractorFacilityData.m_HeightOffset.min - y, 0.0f));
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
      public ComponentTypeHandle<Game.Buildings.ExtractorFacility> __Game_Buildings_ExtractorFacility_RW_ComponentTypeHandle;
      public ComponentTypeHandle<PointOfInterest> __Game_Common_PointOfInterest_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorFacilityData> __Game_Prefabs_ExtractorFacilityData_RO_ComponentLookup;
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
        this.__Game_Buildings_ExtractorFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.ExtractorFacility>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PointOfInterest_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PointOfInterest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorFacilityData_RO_ComponentLookup = state.GetComponentLookup<ExtractorFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
      }
    }
  }
}
