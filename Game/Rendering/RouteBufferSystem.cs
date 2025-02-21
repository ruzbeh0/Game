// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RouteBufferSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Serialization;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class RouteBufferSystem : GameSystemBase, IPreDeserialize
  {
    private RenderingSystem m_RenderingSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_UpdatedRoutesQuery;
    private EntityQuery m_AllRoutesQuery;
    private List<RouteBufferSystem.ManagedData> m_ManagedData;
    private NativeList<RouteBufferSystem.NativeData> m_NativeData;
    private Stack<int> m_FreeBufferIndices;
    private JobHandle m_BufferDependencies;
    private bool m_Loaded;
    private RouteBufferSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedRoutesQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Route>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Deleted>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Common.Event>(),
          ComponentType.ReadOnly<PathUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllRoutesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated method
      this.Clear();
      // ISSUE: reference to a compiler-generated field
      if (this.m_NativeData.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NativeData.Dispose();
      }
      base.OnDestroy();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private void Clear()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ManagedData != null)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ManagedData.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ManagedData[index].Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ManagedData.Clear();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_FreeBufferIndices != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FreeBufferIndices.Clear();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NativeData.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_BufferDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_NativeData.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NativeData.ElementAt(index).Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_NativeData.Clear();
    }

    public unsafe void GetBuffer(
      int index,
      out Material material,
      out ComputeBuffer segmentBuffer,
      out int originalRenderQueue,
      out Bounds bounds,
      out Vector4 size)
    {
      material = (Material) null;
      segmentBuffer = (ComputeBuffer) null;
      originalRenderQueue = 0;
      bounds = new Bounds();
      size = new Vector4();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ManagedData == null || index < 0 || index >= this.m_ManagedData.Count)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_BufferDependencies.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      RouteBufferSystem.ManagedData managedData = this.m_ManagedData[index];
      // ISSUE: reference to a compiler-generated field
      ref RouteBufferSystem.NativeData local = ref this.m_NativeData.ElementAt(index);
      // ISSUE: reference to a compiler-generated field
      if (managedData.m_Updated)
      {
        // ISSUE: reference to a compiler-generated field
        managedData.m_Updated = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (managedData.m_SegmentBuffer != null && managedData.m_SegmentBuffer.count != local.m_SegmentData.Length)
        {
          // ISSUE: reference to a compiler-generated field
          managedData.m_SegmentBuffer.Release();
          // ISSUE: reference to a compiler-generated field
          managedData.m_SegmentBuffer = (ComputeBuffer) null;
        }
        // ISSUE: reference to a compiler-generated field
        if (local.m_SegmentData.Length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (managedData.m_SegmentBuffer == null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            managedData.m_SegmentBuffer = new ComputeBuffer(local.m_SegmentData.Length, sizeof (RouteBufferSystem.SegmentData));
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            managedData.m_SegmentBuffer.name = "Route segment buffer (" + managedData.m_Material.name + ")";
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<RouteBufferSystem.SegmentData> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<RouteBufferSystem.SegmentData>((void*) local.m_SegmentData.Ptr, local.m_SegmentData.Length, Allocator.None);
          // ISSUE: reference to a compiler-generated field
          managedData.m_SegmentBuffer.SetData<RouteBufferSystem.SegmentData>(nativeArray);
        }
        // ISSUE: reference to a compiler-generated field
        local.m_SegmentData.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      material = managedData.m_Material;
      // ISSUE: reference to a compiler-generated field
      segmentBuffer = managedData.m_SegmentBuffer;
      // ISSUE: reference to a compiler-generated field
      originalRenderQueue = managedData.m_OriginalRenderQueue;
      // ISSUE: reference to a compiler-generated field
      bounds = RenderingUtils.ToBounds(local.m_Bounds);
      // ISSUE: reference to a compiler-generated field
      size = managedData.m_Size;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = loaded ? this.m_AllRoutesQuery : this.m_UpdatedRoutesQuery;
      if (entityQuery.IsEmptyIgnoreFilter)
        return;
      HashSet<Entity> entitySet = (HashSet<Entity>) null;
      // ISSUE: reference to a compiler-generated field
      this.m_BufferDependencies.Complete();
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = entityQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Created> componentTypeHandle2 = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Applied> componentTypeHandle3 = this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PathUpdated> componentTypeHandle4 = this.__TypeHandle.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<RouteBufferIndex> componentTypeHandle6 = this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Game.Routes.Segment> roComponentLookup1 = this.__TypeHandle.__Game_Routes_Segment_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Owner> roComponentLookup2 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Deleted> roComponentLookup3 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<PathUpdated> nativeArray1 = archetypeChunk.GetNativeArray<PathUpdated>(ref componentTypeHandle4);
          if (nativeArray1.Length != 0)
          {
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              PathUpdated pathUpdated = nativeArray1[index2];
              if (roComponentLookup1.HasComponent(pathUpdated.m_Owner) && roComponentLookup2.HasComponent(pathUpdated.m_Owner) && !roComponentLookup3.HasComponent(pathUpdated.m_Owner))
              {
                if (entitySet == null)
                  entitySet = new HashSet<Entity>();
                entitySet.Add(roComponentLookup2[pathUpdated.m_Owner].m_Owner);
              }
            }
          }
          else if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
          {
            NativeArray<RouteBufferIndex> nativeArray2 = archetypeChunk.GetNativeArray<RouteBufferIndex>(ref componentTypeHandle6);
            // ISSUE: reference to a compiler-generated field
            if (this.m_FreeBufferIndices == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_FreeBufferIndices = new Stack<int>(nativeArray2.Length);
            }
            for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
            {
              RouteBufferIndex routeBufferIndex = nativeArray2[index3];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: variable of a compiler-generated type
              RouteBufferSystem.ManagedData managedData = this.m_ManagedData[routeBufferIndex.m_Index];
              // ISSUE: reference to a compiler-generated field
              ref RouteBufferSystem.NativeData local = ref this.m_NativeData.ElementAt(routeBufferIndex.m_Index);
              // ISSUE: reference to a compiler-generated field
              managedData.m_Updated = false;
              // ISSUE: reference to a compiler-generated field
              local.m_Updated = false;
              // ISSUE: reference to a compiler-generated field
              this.m_FreeBufferIndices.Push(routeBufferIndex.m_Index);
              routeBufferIndex.m_Index = -1;
              nativeArray2[index3] = routeBufferIndex;
            }
          }
          else if (loaded || archetypeChunk.Has<Created>(ref componentTypeHandle2) && !archetypeChunk.Has<Applied>(ref componentTypeHandle3))
          {
            NativeArray<Entity> nativeArray3 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<RouteBufferIndex> nativeArray4 = archetypeChunk.GetNativeArray<RouteBufferIndex>(ref componentTypeHandle6);
            NativeArray<PrefabRef> nativeArray5 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle5);
            // ISSUE: reference to a compiler-generated field
            if (this.m_ManagedData == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ManagedData = new List<RouteBufferSystem.ManagedData>(nativeArray4.Length);
            }
            // ISSUE: reference to a compiler-generated field
            if (!this.m_NativeData.IsCreated)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_NativeData = new NativeList<RouteBufferSystem.NativeData>(nativeArray4.Length, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
            }
            if (entitySet == null)
              entitySet = new HashSet<Entity>();
            for (int index4 = 0; index4 < nativeArray4.Length; ++index4)
            {
              Entity entity1 = nativeArray3[index4];
              RouteBufferIndex routeBufferIndex = nativeArray4[index4];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              RoutePrefab prefab = this.m_PrefabSystem.GetPrefab<RoutePrefab>(nativeArray5[index4]);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_FreeBufferIndices != null && this.m_FreeBufferIndices.Count > 0)
              {
                // ISSUE: reference to a compiler-generated field
                routeBufferIndex.m_Index = this.m_FreeBufferIndices.Pop();
                // ISSUE: reference to a compiler-generated field
                // ISSUE: variable of a compiler-generated type
                RouteBufferSystem.ManagedData managedData = this.m_ManagedData[routeBufferIndex.m_Index];
                // ISSUE: reference to a compiler-generated field
                ref RouteBufferSystem.NativeData local = ref this.m_NativeData.ElementAt(routeBufferIndex.m_Index);
                // ISSUE: reference to a compiler-generated method
                managedData.Initialize(prefab);
                Entity entity2 = entity1;
                // ISSUE: reference to a compiler-generated method
                local.Initialize(entity2);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                routeBufferIndex.m_Index = this.m_ManagedData.Count;
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                RouteBufferSystem.ManagedData managedData = new RouteBufferSystem.ManagedData();
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                RouteBufferSystem.NativeData nativeData = new RouteBufferSystem.NativeData();
                // ISSUE: reference to a compiler-generated method
                managedData.Initialize(prefab);
                // ISSUE: reference to a compiler-generated method
                nativeData.Initialize(entity1);
                // ISSUE: reference to a compiler-generated field
                this.m_ManagedData.Add(managedData);
                // ISSUE: reference to a compiler-generated field
                this.m_NativeData.Add(in nativeData);
              }
              nativeArray4[index4] = routeBufferIndex;
              entitySet.Add(entity1);
            }
          }
          else
          {
            NativeArray<Entity> nativeArray6 = archetypeChunk.GetNativeArray(entityTypeHandle);
            if (entitySet == null)
              entitySet = new HashSet<Entity>();
            for (int index5 = 0; index5 < nativeArray6.Length; ++index5)
              entitySet.Add(nativeArray6[index5]);
          }
        }
      }
      if (entitySet == null)
        return;
      foreach (Entity entity in entitySet)
      {
        RouteBufferIndex componentData = this.EntityManager.GetComponentData<RouteBufferIndex>(entity);
        if (componentData.m_Index >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          RouteBufferSystem.ManagedData managedData = this.m_ManagedData[componentData.m_Index];
          // ISSUE: reference to a compiler-generated field
          ref RouteBufferSystem.NativeData local = ref this.m_NativeData.ElementAt(componentData.m_Index);
          // ISSUE: reference to a compiler-generated field
          managedData.m_Updated = true;
          // ISSUE: reference to a compiler-generated field
          local.m_Updated = true;
          // ISSUE: reference to a compiler-generated field
          if (!local.m_SegmentData.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            local.m_SegmentData = new UnsafeList<RouteBufferSystem.SegmentData>(0, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveSource_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_LivePath_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RouteBufferSystem.UpdateBufferJob jobData = new RouteBufferSystem.UpdateBufferJob()
      {
        m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PositionData = this.__TypeHandle.__Game_Routes_Position_RO_ComponentLookup,
        m_PathSourceData = this.__TypeHandle.__Game_Routes_PathSource_RO_ComponentLookup,
        m_LivePathData = this.__TypeHandle.__Game_Routes_LivePath_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_UnspawnedData = this.__TypeHandle.__Game_Objects_Unspawned_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_CurveElements = this.__TypeHandle.__Game_Routes_CurveElement_RO_BufferLookup,
        m_CurveSources = this.__TypeHandle.__Game_Routes_CurveSource_RO_BufferLookup,
        m_TransformFrames = this.__TypeHandle.__Game_Objects_TransformFrame_RO_BufferLookup,
        m_PathElements = this.__TypeHandle.__Game_Pathfind_PathElement_RO_BufferLookup,
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime,
        m_NativeData = this.m_NativeData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_BufferDependencies = jobData.Schedule<RouteBufferSystem.UpdateBufferJob>(this.m_NativeData.Length, 1, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.Dependency = this.m_BufferDependencies;
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
    public RouteBufferSystem()
    {
    }

    private class ManagedData : IDisposable
    {
      public Material m_Material;
      public ComputeBuffer m_SegmentBuffer;
      public Vector4 m_Size;
      public int m_OriginalRenderQueue;
      public bool m_Updated;

      public void Initialize(RoutePrefab routePrefab)
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_Material != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Material);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Material = new Material(routePrefab.m_Material);
        // ISSUE: reference to a compiler-generated field
        this.m_Material.name = "Routes (" + routePrefab.name + ")";
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OriginalRenderQueue = this.m_Material.renderQueue;
        // ISSUE: reference to a compiler-generated field
        this.m_Size = new Vector4(routePrefab.m_Width, routePrefab.m_Width * 0.25f, routePrefab.m_SegmentLength, 0.0f);
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        if ((UnityEngine.Object) this.m_Material != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Material);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SegmentBuffer == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SegmentBuffer.Release();
      }
    }

    private struct NativeData : IDisposable
    {
      public UnsafeList<RouteBufferSystem.SegmentData> m_SegmentData;
      public Bounds3 m_Bounds;
      public Entity m_Entity;
      public float m_Length;
      public bool m_Updated;

      public void Initialize(Entity entity) => this.m_Entity = entity;

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SegmentData.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_SegmentData.Dispose();
      }
    }

    private struct SegmentData
    {
      public float4x4 m_Curve;
      public float3 m_Position;
      public float3 m_SizeFactor;
      public float2 m_Opacity;
      public float2 m_DividedOpacity;
      public float m_Broken;
    }

    private struct CurveKey : IEquatable<RouteBufferSystem.CurveKey>
    {
      public Line3.Segment m_Line;
      public Entity m_Entity;
      public float2 m_Range;

      public bool Equals(RouteBufferSystem.CurveKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity != Entity.Null ? this.m_Entity.Equals(other.m_Entity) && this.m_Range.Equals(other.m_Range) : this.m_Line.a.Equals(other.m_Line.a) && this.m_Line.b.Equals(other.m_Line.b);
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity != Entity.Null ? this.m_Entity.GetHashCode() ^ this.m_Range.GetHashCode() : this.m_Line.a.GetHashCode() ^ this.m_Line.b.GetHashCode();
      }
    }

    private struct CurveValue
    {
      public int m_SegmentDataIndex;
      public int m_SharedCount;
    }

    private struct SourceKey : IEquatable<RouteBufferSystem.SourceKey>
    {
      public Entity m_Entity;
      public bool m_Forward;

      public bool Equals(RouteBufferSystem.SourceKey other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity.Equals(other.m_Entity) && this.m_Forward == other.m_Forward;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Entity.GetHashCode() ^ this.m_Forward.GetHashCode();
      }
    }

    [BurstCompile]
    private struct UpdateBufferJob : IJobParallelFor
    {
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<PathSource> m_PathSourceData;
      [ReadOnly]
      public ComponentLookup<LivePath> m_LivePathData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Unspawned> m_UnspawnedData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegments;
      [ReadOnly]
      public BufferLookup<CurveElement> m_CurveElements;
      [ReadOnly]
      public BufferLookup<CurveSource> m_CurveSources;
      [ReadOnly]
      public BufferLookup<TransformFrame> m_TransformFrames;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;
      [NativeDisableParallelForRestriction]
      public NativeList<RouteBufferSystem.NativeData> m_NativeData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref RouteBufferSystem.NativeData local1 = ref this.m_NativeData.ElementAt(index);
        // ISSUE: reference to a compiler-generated field
        if (!local1.m_Updated)
          return;
        // ISSUE: reference to a compiler-generated field
        local1.m_Updated = false;
        // ISSUE: reference to a compiler-generated field
        local1.m_SegmentData.Clear();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[local1.m_Entity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> routeSegment = this.m_RouteSegments[local1.m_Entity];
        NativeHashMap<RouteBufferSystem.CurveKey, RouteBufferSystem.CurveValue> curveMap = new NativeHashMap<RouteBufferSystem.CurveKey, RouteBufferSystem.CurveValue>();
        NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float> parallelMultiHashMap = new NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float>();
        NativeList<int> nativeList = new NativeList<int>();
        NativeList<float> list = new NativeList<float>();
        // ISSUE: variable of a compiler-generated type
        RouteBufferSystem.SourceKey sourceKey;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_LivePathData.HasComponent(local1.m_Entity))
        {
          curveMap = new NativeHashMap<RouteBufferSystem.CurveKey, RouteBufferSystem.CurveValue>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          parallelMultiHashMap = new NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          nativeList = new NativeList<int>(1000, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          list = new NativeList<float>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index1 = 0; index1 < routeSegment.Length; ++index1)
          {
            Entity segment = routeSegment[index1].m_Segment;
            if (!(segment == Entity.Null))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<CurveSource> curveSource1 = this.m_CurveSources[segment];
              for (int index2 = 0; index2 < curveSource1.Length; ++index2)
              {
                CurveSource curveSource2 = curveSource1[index2];
                if ((double) curveSource2.m_Range.x != (double) curveSource2.m_Range.y)
                {
                  if ((double) curveSource2.m_Range.x != 0.0 && (double) curveSource2.m_Range.x != 1.0)
                  {
                    ref NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float> local2 = ref parallelMultiHashMap;
                    // ISSUE: object of a compiler-generated type is created
                    sourceKey = new RouteBufferSystem.SourceKey();
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Entity = curveSource2.m_Entity;
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Forward = (double) curveSource2.m_Range.y > (double) curveSource2.m_Range.x;
                    // ISSUE: variable of a compiler-generated type
                    RouteBufferSystem.SourceKey key = sourceKey;
                    double x = (double) curveSource2.m_Range.x;
                    local2.Add(key, (float) x);
                  }
                  if ((double) curveSource2.m_Range.y != 0.0 && (double) curveSource2.m_Range.y != 1.0)
                  {
                    ref NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float> local3 = ref parallelMultiHashMap;
                    // ISSUE: object of a compiler-generated type is created
                    sourceKey = new RouteBufferSystem.SourceKey();
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Entity = curveSource2.m_Entity;
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Forward = (double) curveSource2.m_Range.y > (double) curveSource2.m_Range.x;
                    // ISSUE: variable of a compiler-generated type
                    RouteBufferSystem.SourceKey key = sourceKey;
                    double y = (double) curveSource2.m_Range.y;
                    local3.Add(key, (float) y);
                  }
                }
              }
            }
          }
        }
        float x1 = 0.0f;
        Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        for (int index3 = 0; index3 < routeSegment.Length; ++index3)
        {
          Entity segment = routeSegment[index3].m_Segment;
          if (!(segment == Entity.Null))
          {
            float broken = 0.0f;
            DynamicBuffer<PathElement> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathElements.TryGetBuffer(segment, out bufferData1))
              broken = math.select(0.0f, 1f, bufferData1.Length == 0);
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CurveElement> curveElement1 = this.m_CurveElements[segment];
            DynamicBuffer<CurveSource> dynamicBuffer = new DynamicBuffer<CurveSource>();
            float3 y1 = new float3();
            float3 y2 = new float3();
            float3 y3 = new float3();
            float3 y4 = new float3();
            if (curveElement1.Length > 0)
            {
              y3 = math.normalizesafe(MathUtils.StartTangent(curveElement1[0].m_Curve));
              y4 = curveElement1[0].m_Curve.a;
              PathSource componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PathSourceData.TryGetComponent(segment, out componentData1))
              {
                // ISSUE: reference to a compiler-generated field
                dynamicBuffer = this.m_CurveSources[segment];
                x1 = 0.0f;
                Game.Objects.Transform componentData2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_TransformData.TryGetComponent(componentData1.m_Entity, out componentData2) && !this.m_UnspawnedData.HasComponent(componentData1.m_Entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  EntityStorageInfo entityStorageInfo = this.m_EntityLookup[componentData1.m_Entity];
                  bool flag = false;
                  DynamicBuffer<TransformFrame> bufferData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (entityStorageInfo.Chunk.Has<UpdateFrame>(this.m_UpdateFrameType) && this.m_TransformFrames.TryGetBuffer(componentData1.m_Entity, out bufferData2))
                  {
                    uint updateFrame1;
                    uint updateFrame2;
                    float framePosition;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    ObjectInterpolateSystem.CalculateUpdateFrames(this.m_FrameIndex, this.m_FrameTime, entityStorageInfo.Chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index, out updateFrame1, out updateFrame2, out framePosition);
                    // ISSUE: reference to a compiler-generated method
                    componentData2 = ObjectInterpolateSystem.CalculateTransform(bufferData2[(int) updateFrame1], bufferData2[(int) updateFrame2], framePosition).ToTransform();
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_CurrentVehicleData.HasComponent(componentData1.m_Entity))
                      flag = true;
                  }
                  if (!flag)
                  {
                    float3 a = curveElement1[0].m_Curve.a;
                    float3 float3_1 = a - componentData2.m_Position;
                    float3 y5 = MathUtils.Normalize(float3_1, float3_1.xz);
                    Bezier4x3 curve;
                    if (y3.Equals(new float3()))
                    {
                      curve = NetUtils.StraightCurve(componentData2.m_Position, a);
                      float num = (curve.a.y - curve.b.y) / math.max(1f, math.abs(y5.y));
                      curve.b.y += num;
                      curve.c.y += num;
                      y3 = math.normalizesafe(MathUtils.EndTangent(curve));
                    }
                    else
                    {
                      float3 float3_2 = MathUtils.Normalize(y3, y3.xz);
                      float3 startTangent = y5 * (math.dot(float3_2, y5) * 2f) - float3_2;
                      curve = NetUtils.FitCurve(componentData2.m_Position, startTangent, float3_2, a);
                    }
                    float num1 = MathUtils.Length(curve);
                    float2 offset = new float2(x1, x1 + num1);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    int num2 = this.AddSegment(ref local1.m_SegmentData, curveMap, curve, Entity.Null, new float2(), offset, new float2(0.0f, 1f), broken);
                    x1 = offset.y;
                    bounds3 |= MathUtils.Bounds(curve);
                    y1 = y3;
                    y2 = y4;
                    if (nativeList.IsCreated)
                      nativeList.Add(in num2);
                  }
                }
              }
            }
            if (!y3.Equals(new float3()))
            {
              for (int index4 = 0; index4 < curveElement1.Length; ++index4)
              {
                CurveElement curveElement2 = curveElement1[index4];
                float3 x2 = y3;
                float3 x3 = y4;
                float3 x4 = math.normalizesafe(MathUtils.EndTangent(curveElement2.m_Curve));
                float3 d = curveElement2.m_Curve.d;
                if (index4 + 1 < curveElement1.Length)
                {
                  y3 = math.normalizesafe(MathUtils.StartTangent(curveElement1[index4 + 1].m_Curve));
                  y4 = curveElement1[index4 + 1].m_Curve.a;
                }
                else
                {
                  y3 = new float3();
                  y4 = new float3();
                }
                float2 float2 = math.select((float2) 1f, (float2) 0.0f, new float2(math.dot(x2, y1), math.dot(x4, y3)) < 0.99f | new float2(math.distancesq(x3, y2), math.distancesq(d, y4)) > 0.01f);
                bounds3 |= MathUtils.Bounds(curveElement2.m_Curve);
                y1 = x4;
                y2 = d;
                CurveSource curveSource = new CurveSource();
                if (dynamicBuffer.IsCreated && parallelMultiHashMap.IsCreated)
                {
                  curveSource = dynamicBuffer[index4];
                  if ((double) curveSource.m_Range.x != (double) curveSource.m_Range.y)
                  {
                    bool flag = (double) curveSource.m_Range.y > (double) curveSource.m_Range.x;
                    ref NativeParallelMultiHashMap<RouteBufferSystem.SourceKey, float> local4 = ref parallelMultiHashMap;
                    // ISSUE: object of a compiler-generated type is created
                    sourceKey = new RouteBufferSystem.SourceKey();
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Entity = curveSource.m_Entity;
                    // ISSUE: reference to a compiler-generated field
                    sourceKey.m_Forward = flag;
                    // ISSUE: variable of a compiler-generated type
                    RouteBufferSystem.SourceKey key = sourceKey;
                    float num3;
                    ref float local5 = ref num3;
                    NativeParallelMultiHashMapIterator<RouteBufferSystem.SourceKey> it;
                    ref NativeParallelMultiHashMapIterator<RouteBufferSystem.SourceKey> local6 = ref it;
                    if (local4.TryGetFirstValue(key, out local5, out local6))
                    {
                      do
                      {
                        list.Add(in num3);
                      }
                      while (parallelMultiHashMap.TryGetNextValue(out num3, ref it));
                      if (list.Length >= 2)
                        list.Sort<float>();
                      if (!flag)
                        curveElement2.m_Curve = MathUtils.Invert(curveElement2.m_Curve);
                      Curve componentData;
                      // ISSUE: reference to a compiler-generated field
                      if (((double) curveSource.m_Range.x != 0.0 && (double) curveSource.m_Range.x != 1.0 || (double) curveSource.m_Range.y != 0.0 && (double) curveSource.m_Range.y != 1.0) && this.m_CurveData.TryGetComponent(curveSource.m_Entity, out componentData))
                        curveElement2.m_Curve = componentData.m_Bezier;
                      float2 range = curveSource.m_Range;
                      for (int index5 = 0; index5 <= list.Length; ++index5)
                      {
                        if (flag)
                        {
                          if (index5 < list.Length)
                          {
                            range.y = list[index5];
                            if ((double) range.y <= (double) range.x || (double) range.y >= (double) curveSource.m_Range.y)
                              continue;
                          }
                          else
                            range.y = curveSource.m_Range.y;
                        }
                        else if (index5 < list.Length)
                        {
                          range.y = list[list.Length - index5 - 1];
                          if ((double) range.y >= (double) range.x || (double) range.y <= (double) curveSource.m_Range.y)
                            continue;
                        }
                        else
                          range.y = curveSource.m_Range.y;
                        Bezier4x3 curve = MathUtils.Cut(curveElement2.m_Curve, range);
                        float num4 = MathUtils.Length(curve);
                        float2 offset = new float2(x1, x1 + num4);
                        float2 opacity = math.select(float2, (float2) 1f, range != curveSource.m_Range);
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated method
                        int num5 = this.AddSegment(ref local1.m_SegmentData, curveMap, curve, curveSource.m_Entity, range, offset, opacity, broken);
                        x1 = offset.y;
                        range.x = range.y;
                        nativeList.Add(in num5);
                      }
                      list.Clear();
                      continue;
                    }
                  }
                }
                float num6 = MathUtils.Length(curveElement2.m_Curve);
                float2 offset1 = new float2(x1, x1 + num6);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                int num7 = this.AddSegment(ref local1.m_SegmentData, curveMap, curveElement2.m_Curve, curveSource.m_Entity, curveSource.m_Range, offset1, float2, broken);
                x1 = offset1.y;
                if (nativeList.IsCreated)
                  nativeList.Add(in num7);
              }
            }
            if (nativeList.IsCreated)
              nativeList.Add(-1);
          }
        }
        if (nativeList.IsCreated && nativeList.Length > 0)
        {
          bool flag = true;
          for (int index6 = 0; flag && index6 < 10; ++index6)
          {
            int index7 = nativeList[0];
            flag = false;
            if (index6 == 0)
            {
              for (int index8 = 1; index8 < nativeList.Length; ++index8)
              {
                int index9 = nativeList[index8];
                if (index7 != -1 && index9 != -1)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref RouteBufferSystem.SegmentData local7 = ref local1.m_SegmentData.ElementAt(index7);
                  // ISSUE: reference to a compiler-generated field
                  ref RouteBufferSystem.SegmentData local8 = ref local1.m_SegmentData.ElementAt(index9);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float num = math.max(local7.m_SizeFactor.z, local8.m_SizeFactor.x);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  flag = ((flag ? 1 : 0) | ((double) num != (double) local7.m_SizeFactor.z ? 1 : ((double) num != (double) local8.m_SizeFactor.x ? 1 : 0))) != 0;
                  // ISSUE: reference to a compiler-generated field
                  local7.m_SizeFactor.z = num;
                  // ISSUE: reference to a compiler-generated field
                  local8.m_SizeFactor.x = num;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float y6 = math.select(local7.m_Opacity.y / local8.m_Opacity.x, 1f, (double) local8.m_Opacity.x < 0.5 || (double) local7.m_Opacity.y > (double) local8.m_Opacity.x);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float y7 = math.select(local8.m_Opacity.x / local7.m_Opacity.y, 1f, (double) local7.m_Opacity.y < 0.5 || (double) local8.m_Opacity.x > (double) local7.m_Opacity.y);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  local7.m_DividedOpacity.y = math.min(local7.m_DividedOpacity.y, y6);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  local8.m_DividedOpacity.x = math.min(local8.m_DividedOpacity.x, y7);
                }
                index7 = index9;
              }
              // ISSUE: reference to a compiler-generated field
              for (int index10 = 0; index10 < local1.m_SegmentData.Length; ++index10)
              {
                // ISSUE: reference to a compiler-generated field
                ref RouteBufferSystem.SegmentData local9 = ref local1.m_SegmentData.ElementAt(index10);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                local9.m_Opacity = math.saturate(local9.m_Opacity);
              }
            }
            else
            {
              for (int index11 = 1; index11 < nativeList.Length; ++index11)
              {
                int index12 = nativeList[index11];
                if (index7 != -1 && index12 != -1)
                {
                  // ISSUE: reference to a compiler-generated field
                  ref RouteBufferSystem.SegmentData local10 = ref local1.m_SegmentData.ElementAt(index7);
                  // ISSUE: reference to a compiler-generated field
                  ref RouteBufferSystem.SegmentData local11 = ref local1.m_SegmentData.ElementAt(index12);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  float num = math.max(local10.m_SizeFactor.z, local11.m_SizeFactor.x);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  flag = ((flag ? 1 : 0) | ((double) num != (double) local10.m_SizeFactor.z ? 1 : ((double) num != (double) local11.m_SizeFactor.x ? 1 : 0))) != 0;
                  // ISSUE: reference to a compiler-generated field
                  local10.m_SizeFactor.z = num;
                  // ISSUE: reference to a compiler-generated field
                  local11.m_SizeFactor.x = num;
                }
                index7 = index12;
              }
            }
          }
          int num8 = 0;
          for (int index13 = 0; index13 < nativeList.Length; ++index13)
          {
            if (nativeList[index13] == -1)
            {
              float num9 = 0.0f;
              for (int index14 = index13 - 1; index14 >= num8; --index14)
              {
                int index15 = nativeList[index14];
                // ISSUE: reference to a compiler-generated field
                ref RouteBufferSystem.SegmentData local12 = ref local1.m_SegmentData.ElementAt(index15);
                // ISSUE: reference to a compiler-generated field
                float num10 = math.min(0.0f, num9 - local12.m_Curve.c3.w);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                num9 -= local12.m_Curve.c3.w - local12.m_Curve.c0.w;
                // ISSUE: reference to a compiler-generated field
                local12.m_Curve.c0.w += num10;
                // ISSUE: reference to a compiler-generated field
                local12.m_Curve.c1.w += num10;
                // ISSUE: reference to a compiler-generated field
                local12.m_Curve.c2.w += num10;
                // ISSUE: reference to a compiler-generated field
                local12.m_Curve.c3.w += num10;
              }
              num8 = index13 + 1;
            }
          }
        }
        for (int index16 = 0; index16 < routeWaypoint.Length; ++index16)
        {
          // ISSUE: reference to a compiler-generated field
          float3 position = this.m_PositionData[routeWaypoint[index16].m_Waypoint].m_Position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddNode(ref local1.m_SegmentData, position, 1f);
          bounds3 |= position;
        }
        if (curveMap.IsCreated)
          curveMap.Dispose();
        if (parallelMultiHashMap.IsCreated)
          parallelMultiHashMap.Dispose();
        if (nativeList.IsCreated)
          nativeList.Dispose();
        if (list.IsCreated)
          list.Dispose();
        // ISSUE: reference to a compiler-generated field
        local1.m_Bounds = bounds3;
        // ISSUE: reference to a compiler-generated field
        local1.m_Length = x1;
      }

      private int AddSegment(
        ref UnsafeList<RouteBufferSystem.SegmentData> segments,
        NativeHashMap<RouteBufferSystem.CurveKey, RouteBufferSystem.CurveValue> curveMap,
        Bezier4x3 curve,
        Entity sourceEntity,
        float2 sourceRange,
        float2 offset,
        float2 opacity,
        float broken)
      {
        float3 float3 = math.lerp(curve.a, curve.d, 0.5f);
        // ISSUE: variable of a compiler-generated type
        RouteBufferSystem.SegmentData segmentData;
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Curve = new float4x4()
        {
          c0 = new float4(curve.a - float3, offset.x),
          c1 = new float4(curve.b - float3, offset.x + math.distance(curve.a, curve.b)),
          c2 = new float4(curve.c - float3, offset.y - math.distance(curve.c, curve.d)),
          c3 = new float4(curve.d - float3, offset.y)
        };
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Position = float3;
        // ISSUE: reference to a compiler-generated field
        segmentData.m_SizeFactor = new float3(1f, 1f, 1f);
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Opacity = opacity;
        // ISSUE: reference to a compiler-generated field
        segmentData.m_DividedOpacity = new float2(1f, 1f);
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Broken = broken;
        int num = -1;
        if (curveMap.IsCreated)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          RouteBufferSystem.CurveKey key = new RouteBufferSystem.CurveKey()
          {
            m_Line = new Line3.Segment(curve.a, curve.d),
            m_Entity = sourceEntity,
            m_Range = sourceRange
          };
          // ISSUE: variable of a compiler-generated type
          RouteBufferSystem.CurveValue curveValue;
          if (curveMap.TryGetValue(key, out curveValue))
          {
            // ISSUE: reference to a compiler-generated field
            ref RouteBufferSystem.SegmentData local = ref segments.ElementAt(curveValue.m_SegmentDataIndex);
            // ISSUE: reference to a compiler-generated field
            ++curveValue.m_SharedCount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            local.m_SizeFactor = (float3) this.GetSizeFactor(curveValue.m_SharedCount);
            // ISSUE: reference to a compiler-generated field
            local.m_Opacity += opacity;
            curveMap[key] = curveValue;
            // ISSUE: reference to a compiler-generated field
            return curveValue.m_SegmentDataIndex;
          }
          // ISSUE: reference to a compiler-generated field
          curveValue.m_SharedCount = 1;
          // ISSUE: reference to a compiler-generated field
          curveValue.m_SegmentDataIndex = segments.Length;
          curveMap.Add(key, curveValue);
          // ISSUE: reference to a compiler-generated field
          num = curveValue.m_SegmentDataIndex;
        }
        segments.Add(in segmentData);
        return num;
      }

      private float GetSizeFactor(int sharedCount)
      {
        return (float) (4.0 - 21.0 / (6.0 + (double) sharedCount));
      }

      private void AddNode(
        ref UnsafeList<RouteBufferSystem.SegmentData> segments,
        float3 position,
        float opacity)
      {
        // ISSUE: variable of a compiler-generated type
        RouteBufferSystem.SegmentData segmentData;
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Curve = new float4x4()
        {
          c0 = new float4(0.0f, 0.0f, -1f, -1000000f),
          c1 = new float4(0.0f, 0.0f, -0.333333343f, -1000000f),
          c2 = new float4(0.0f, 0.0f, 0.333333343f, -1000000f),
          c3 = new float4(0.0f, 0.0f, 1f, -1000000f)
        };
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Position = position;
        // ISSUE: reference to a compiler-generated field
        segmentData.m_SizeFactor = new float3(1f, 1f, 1f);
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Opacity = new float2(opacity, opacity);
        // ISSUE: reference to a compiler-generated field
        segmentData.m_DividedOpacity = new float2(1f, 1f);
        // ISSUE: reference to a compiler-generated field
        segmentData.m_Broken = 0.0f;
        segments.Add(in segmentData);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Applied> __Game_Common_Applied_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PathUpdated> __Game_Pathfind_PathUpdated_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<RouteBufferIndex> __Game_Rendering_RouteBufferIndex_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Routes.Segment> __Game_Routes_Segment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Position> __Game_Routes_Position_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathSource> __Game_Routes_PathSource_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LivePath> __Game_Routes_LivePath_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Unspawned> __Game_Objects_Unspawned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CurveElement> __Game_Routes_CurveElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CurveSource> __Game_Routes_CurveSource_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PathElement> __Game_Pathfind_PathElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Applied_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Applied>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathUpdated_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PathUpdated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_RouteBufferIndex_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RouteBufferIndex>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Segment_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.Segment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Position_RO_ComponentLookup = state.GetComponentLookup<Position>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_PathSource_RO_ComponentLookup = state.GetComponentLookup<PathSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_LivePath_RO_ComponentLookup = state.GetComponentLookup<LivePath>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Unspawned_RO_ComponentLookup = state.GetComponentLookup<Unspawned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveElement_RO_BufferLookup = state.GetBufferLookup<CurveElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurveSource_RO_BufferLookup = state.GetBufferLookup<CurveSource>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RO_BufferLookup = state.GetBufferLookup<TransformFrame>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathElement_RO_BufferLookup = state.GetBufferLookup<PathElement>(true);
      }
    }
  }
}
