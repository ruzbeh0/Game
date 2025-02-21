// Decompiled with JetBrains decompiler
// Type: Game.Tools.AnimationUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class AnimationUpdateSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_AnimatedQuery;
    private AnimationUpdateSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(ComponentType.ReadWrite<Animation>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AnimatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Animation>(), ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Game.Objects.Transform>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeParallelMultiHashMap<Entity, Animation> parallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Animation>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Animation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimationUpdateSystem.AnimationMapJob jobData1 = new AnimationUpdateSystem.AnimationMapJob()
      {
        m_AnimationType = this.__TypeHandle.__Game_Tools_Animation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AnimationMap = parallelMultiHashMap
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Animation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      AnimationUpdateSystem.AnimationUpdateJob jobData2 = new AnimationUpdateSystem.AnimationUpdateJob()
      {
        m_DeltaTime = UnityEngine.Time.deltaTime,
        m_AnimationMap = parallelMultiHashMap,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Tools_Animation_RW_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.Schedule<AnimationUpdateSystem.AnimationMapJob>(this.m_AnimatedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery updatedQuery = this.m_UpdatedQuery;
      JobHandle dependsOn = jobHandle;
      JobHandle inputDeps = jobData2.ScheduleParallel<AnimationUpdateSystem.AnimationUpdateJob>(updatedQuery, dependsOn);
      parallelMultiHashMap.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public AnimationUpdateSystem()
    {
    }

    [BurstCompile]
    private struct AnimationMapJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Animation> m_AnimationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public NativeParallelMultiHashMap<Entity, Animation> m_AnimationMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Animation> nativeArray1 = chunk.GetNativeArray<Animation>(ref this.m_AnimationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Animation animation = nativeArray1[index];
          PrefabRef prefabRef = nativeArray2[index];
          if (!animation.m_Rotation.Equals(new quaternion()))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_AnimationMap.Add(prefabRef.m_Prefab, animation);
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
    private struct AnimationUpdateJob : IJobChunk
    {
      [ReadOnly]
      public float m_DeltaTime;
      [ReadOnly]
      public NativeParallelMultiHashMap<Entity, Animation> m_AnimationMap;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Animation> m_AnimationType;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray1 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Animation> nativeArray5 = chunk.GetNativeArray<Animation>(ref this.m_AnimationType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Game.Objects.Transform transform = nativeArray1[index];
          Temp temp = nativeArray2[index];
          PrefabRef prefabRef = nativeArray4[index];
          Animation animationData = nativeArray5[index];
          Owner owner = new Owner();
          if (nativeArray3.Length != 0)
            owner = nativeArray3[index];
          ObjectGeometryData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
          {
            float3 pivot = componentData1.m_Pivot;
            NetGeometryData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabNetGeometryData.TryGetComponent(owner.m_Owner, out componentData2))
              pivot = new float3(0.0f, MathUtils.Center(componentData2.m_DefaultHeightRange), 0.0f);
            // ISSUE: reference to a compiler-generated method
            animationData = this.CalculateAnimationData(transform, prefabRef, temp, componentData1.m_Size, pivot);
          }
          nativeArray5[index] = animationData;
        }
      }

      private Animation CalculateAnimationData(
        Game.Objects.Transform transform,
        PrefabRef prefabRef,
        Temp temp,
        float3 size,
        float3 pivot)
      {
        Animation animationData = new Animation();
        float num1 = float.MaxValue;
        bool flag = false;
        InterpolatedTransform componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_InterpolatedTransformData.TryGetComponent(temp.m_Original, out componentData1))
        {
          animationData.m_TargetPosition = componentData1.m_Position;
          animationData.m_Position = componentData1.m_Position;
          animationData.m_Rotation = componentData1.m_Rotation;
          num1 = math.distance(componentData1.m_Position, transform.m_Position);
          flag = true;
        }
        else
        {
          Game.Objects.Transform componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformData.TryGetComponent(temp.m_Original, out componentData2))
          {
            animationData.m_TargetPosition = componentData2.m_Position;
            animationData.m_Position = componentData2.m_Position;
            animationData.m_Rotation = componentData2.m_Rotation;
            num1 = math.distance(componentData2.m_Position, transform.m_Position);
            flag = true;
          }
          else
          {
            animationData.m_Position = transform.m_Position;
            animationData.m_Rotation = transform.m_Rotation;
          }
        }
        Animation animation;
        NativeParallelMultiHashMapIterator<Entity> it;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AnimationMap.TryGetFirstValue(prefabRef.m_Prefab, out animation, out it))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            float num2 = math.distance(animation.m_TargetPosition, transform.m_Position);
            if ((double) num2 <= (double) num1)
            {
              num1 = num2;
              animationData = animation;
              flag = true;
            }
          }
          while (this.m_AnimationMap.TryGetNextValue(out animation, ref it));
        }
        if (flag)
        {
          size.y *= 0.5f;
          // ISSUE: reference to a compiler-generated field
          animationData.m_PushFactor = (size.y - pivot.y) / math.max(1f / 1000f, this.m_DeltaTime * size.y * size.y);
        }
        animationData.m_SwayPivot = pivot;
        return animationData;
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
      public ComponentTypeHandle<Animation> __Game_Tools_Animation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Animation> __Game_Tools_Animation_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Animation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Animation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
      }
    }
  }
}
