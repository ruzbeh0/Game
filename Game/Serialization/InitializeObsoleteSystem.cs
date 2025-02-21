// Decompiled with JetBrains decompiler
// Type: Game.Serialization.InitializeObsoleteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class InitializeObsoleteSystem : GameSystemBase
  {
    private EntityQuery m_ObsoleteQuery;
    private EntityQuery m_MeshSettingsQuery;
    private HashSet<ComponentType> m_ArchetypeComponents;
    private Dictionary<System.Type, PrefabBase> m_PrefabInstances;
    private InitializeObsoleteSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObsoleteQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[4]
        {
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<NetData>(),
          ComponentType.ReadOnly<AggregateNetData>(),
          ComponentType.ReadOnly<NetLaneArchetypeData>()
        },
        Disabled = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MeshSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObsoleteQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents = new HashSet<ComponentType>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabInstances = new Dictionary<System.Type, PrefabBase>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype1 = this.GetArchetype<ObjectPrefab>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype2 = this.GetArchetype<StaticObjectPrefab>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype3 = this.GetArchetype<NetGeometryPrefab, Game.Net.Node>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype4 = this.GetArchetype<NetGeometryPrefab, Game.Net.Edge>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype5 = this.GetArchetype<NetGeometryPrefab, NetCompositionData, NetCompositionCrosswalk>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype6 = this.GetArchetype<NetGeometryPrefab, NetCompositionData, NetCompositionLane>();
      // ISSUE: reference to a compiler-generated method
      EntityArchetype archetype7 = this.GetArchetype<AggregateNetPrefab>();
      NetLaneArchetypeData laneArchetypeData;
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_LaneArchetype = this.GetArchetype<NetLanePrefab, Lane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_AreaLaneArchetype = this.GetArchetype<NetLanePrefab, Lane, AreaLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_EdgeLaneArchetype = this.GetArchetype<NetLanePrefab, Lane, EdgeLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_NodeLaneArchetype = this.GetArchetype<NetLanePrefab, Lane, NodeLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_EdgeSlaveArchetype = this.GetArchetype<NetLanePrefab, Lane, SlaveLane, EdgeLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_NodeSlaveArchetype = this.GetArchetype<NetLanePrefab, Lane, SlaveLane, NodeLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_EdgeMasterArchetype = this.GetArchetype<NetLanePrefab, Lane, MasterLane, EdgeLane>();
      // ISSUE: reference to a compiler-generated method
      laneArchetypeData.m_NodeMasterArchetype = this.GetArchetype<NetLanePrefab, Lane, MasterLane, NodeLane>();
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<System.Type, PrefabBase> prefabInstance in this.m_PrefabInstances)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) prefabInstance.Value);
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents = (HashSet<ComponentType>) null;
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabInstances = (Dictionary<System.Type, PrefabBase>) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AggregateNetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MovingObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new InitializeObsoleteSystem.InitializeObsoleteJob()
      {
        m_StackType = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentTypeHandle,
        m_ObjectType = this.__TypeHandle.__Game_Prefabs_ObjectData_RW_ComponentTypeHandle,
        m_MovingObjectType = this.__TypeHandle.__Game_Prefabs_MovingObjectData_RW_ComponentTypeHandle,
        m_ObjectGeometryType = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle,
        m_NetType = this.__TypeHandle.__Game_Prefabs_NetData_RW_ComponentTypeHandle,
        m_NetGeometryType = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle,
        m_AggregateNetType = this.__TypeHandle.__Game_Prefabs_AggregateNetData_RW_ComponentTypeHandle,
        m_NetNameType = this.__TypeHandle.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle,
        m_NetArrowType = this.__TypeHandle.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle,
        m_NetLaneType = this.__TypeHandle.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle,
        m_NetLaneArchetypeType = this.__TypeHandle.__Game_Prefabs_NetLaneArchetypeData_RW_ComponentTypeHandle,
        m_SubMeshType = this.__TypeHandle.__Game_Prefabs_SubMesh_RW_BufferTypeHandle,
        m_NetGeometrySectionType = this.__TypeHandle.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle,
        m_MeshSettingsData = this.m_MeshSettingsQuery.GetSingleton<MeshSettingsData>(),
        m_ObjectArchetype = archetype1,
        m_ObjectGeometryArchetype = archetype2,
        m_NetGeometryNodeArchetype = archetype3,
        m_NetGeometryEdgeArchetype = archetype4,
        m_NetNodeCompositionArchetype = archetype5,
        m_NetEdgeCompositionArchetype = archetype6,
        m_NetAggregateArchetype = archetype7,
        m_NetLaneArchetypeData = laneArchetypeData
      }.ScheduleParallel<InitializeObsoleteSystem.InitializeObsoleteJob>(this.m_ObsoleteQuery, this.Dependency);
    }

    private T GetPrefabInstance<T>() where T : PrefabBase
    {
      PrefabBase prefabInstance;
      // ISSUE: reference to a compiler-generated field
      if (this.m_PrefabInstances.TryGetValue(typeof (T), out prefabInstance))
        return (T) prefabInstance;
      T instance = ScriptableObject.CreateInstance<T>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabInstances.Add(typeof (T), (PrefabBase) instance);
      return instance;
    }

    private EntityArchetype GetArchetype<T>() where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      T prefabInstance = this.GetPrefabInstance<T>();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Clear();
      // ISSUE: reference to a compiler-generated field
      prefabInstance.GetArchetypeComponents(this.m_ArchetypeComponents);
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(this.m_ArchetypeComponents));
    }

    private EntityArchetype GetArchetype<T, TComponentType>() where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      T prefabInstance = this.GetPrefabInstance<T>();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType>());
      // ISSUE: reference to a compiler-generated field
      prefabInstance.GetArchetypeComponents(this.m_ArchetypeComponents);
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(this.m_ArchetypeComponents));
    }

    private EntityArchetype GetArchetype<T, TComponentType1, TComponentType2>() where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      T prefabInstance = this.GetPrefabInstance<T>();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType1>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType2>());
      // ISSUE: reference to a compiler-generated field
      prefabInstance.GetArchetypeComponents(this.m_ArchetypeComponents);
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(this.m_ArchetypeComponents));
    }

    private EntityArchetype GetArchetype<T, TComponentType1, TComponentType2, TComponentType3>() where T : PrefabBase
    {
      // ISSUE: reference to a compiler-generated method
      T prefabInstance = this.GetPrefabInstance<T>();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType1>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType2>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<TComponentType3>());
      // ISSUE: reference to a compiler-generated field
      prefabInstance.GetArchetypeComponents(this.m_ArchetypeComponents);
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_ArchetypeComponents.Add(ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(this.m_ArchetypeComponents));
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
    public InitializeObsoleteSystem()
    {
    }

    [BurstCompile]
    private struct InitializeObsoleteJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<StackData> m_StackType;
      public ComponentTypeHandle<ObjectData> m_ObjectType;
      public ComponentTypeHandle<MovingObjectData> m_MovingObjectType;
      public ComponentTypeHandle<ObjectGeometryData> m_ObjectGeometryType;
      public ComponentTypeHandle<NetData> m_NetType;
      public ComponentTypeHandle<NetGeometryData> m_NetGeometryType;
      public ComponentTypeHandle<AggregateNetData> m_AggregateNetType;
      public ComponentTypeHandle<NetNameData> m_NetNameType;
      public ComponentTypeHandle<NetArrowData> m_NetArrowType;
      public ComponentTypeHandle<NetLaneData> m_NetLaneType;
      public ComponentTypeHandle<NetLaneArchetypeData> m_NetLaneArchetypeType;
      public BufferTypeHandle<SubMesh> m_SubMeshType;
      public BufferTypeHandle<NetGeometrySection> m_NetGeometrySectionType;
      [ReadOnly]
      public MeshSettingsData m_MeshSettingsData;
      [ReadOnly]
      public EntityArchetype m_ObjectArchetype;
      [ReadOnly]
      public EntityArchetype m_ObjectGeometryArchetype;
      [ReadOnly]
      public EntityArchetype m_NetGeometryNodeArchetype;
      [ReadOnly]
      public EntityArchetype m_NetGeometryEdgeArchetype;
      [ReadOnly]
      public EntityArchetype m_NetNodeCompositionArchetype;
      [ReadOnly]
      public EntityArchetype m_NetEdgeCompositionArchetype;
      [ReadOnly]
      public EntityArchetype m_NetAggregateArchetype;
      [ReadOnly]
      public NetLaneArchetypeData m_NetLaneArchetypeData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectData> nativeArray1 = chunk.GetNativeArray<ObjectData>(ref this.m_ObjectType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<MovingObjectData> nativeArray2 = chunk.GetNativeArray<MovingObjectData>(ref this.m_MovingObjectType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ObjectGeometryData> nativeArray3 = chunk.GetNativeArray<ObjectGeometryData>(ref this.m_ObjectGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetData> nativeArray4 = chunk.GetNativeArray<NetData>(ref this.m_NetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetGeometryData> nativeArray5 = chunk.GetNativeArray<NetGeometryData>(ref this.m_NetGeometryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AggregateNetData> nativeArray6 = chunk.GetNativeArray<AggregateNetData>(ref this.m_AggregateNetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetNameData> nativeArray7 = chunk.GetNativeArray<NetNameData>(ref this.m_NetNameType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetArrowData> nativeArray8 = chunk.GetNativeArray<NetArrowData>(ref this.m_NetArrowType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<StackData> nativeArray9 = chunk.GetNativeArray<StackData>(ref this.m_StackType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetLaneData> nativeArray10 = chunk.GetNativeArray<NetLaneData>(ref this.m_NetLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetLaneArchetypeData> nativeArray11 = chunk.GetNativeArray<NetLaneArchetypeData>(ref this.m_NetLaneArchetypeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubMesh> bufferAccessor1 = chunk.GetBufferAccessor<SubMesh>(ref this.m_SubMeshType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<NetGeometrySection> bufferAccessor2 = chunk.GetBufferAccessor<NetGeometrySection>(ref this.m_NetGeometrySectionType);
        ChunkEntityEnumerator entityEnumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        int nextIndex;
        while (entityEnumerator.NextEntityIndex(out nextIndex))
        {
          if (nativeArray1.Length != 0)
          {
            ref ObjectData local1 = ref nativeArray1.ElementAt<ObjectData>(nextIndex);
            if (nativeArray3.Length != 0)
            {
              ref ObjectGeometryData local2 = ref nativeArray3.ElementAt<ObjectGeometryData>(nextIndex);
              // ISSUE: reference to a compiler-generated field
              local1.m_Archetype = this.m_ObjectGeometryArchetype;
              local2.m_MinLod = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(MathUtils.Size(local2.m_Bounds)));
              local2.m_Layers = MeshLayer.Default;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              local1.m_Archetype = this.m_ObjectArchetype;
            }
            if (nativeArray2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              nativeArray2.ElementAt<MovingObjectData>(nextIndex).m_StoppedArchetype = this.m_ObjectGeometryArchetype;
            }
          }
          if (bufferAccessor1.Length != 0)
          {
            DynamicBuffer<SubMesh> dynamicBuffer = bufferAccessor1[nextIndex];
            if (dynamicBuffer.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              SubMesh elem = new SubMesh(this.m_MeshSettingsData.m_MissingObjectMesh, SubMeshFlags.DefaultMissingMesh, (ushort) 0);
              if (nativeArray9.Length != 0)
                elem.m_Flags |= SubMeshFlags.IsStackMiddle;
              dynamicBuffer.Add(elem);
            }
          }
          if (nativeArray4.Length != 0)
          {
            ref NetData local3 = ref nativeArray4.ElementAt<NetData>(nextIndex);
            if (nativeArray5.Length != 0)
            {
              ref NetGeometryData local4 = ref nativeArray5.ElementAt<NetGeometryData>(nextIndex);
              // ISSUE: reference to a compiler-generated field
              local3.m_NodeArchetype = this.m_NetGeometryNodeArchetype;
              // ISSUE: reference to a compiler-generated field
              local3.m_EdgeArchetype = this.m_NetGeometryEdgeArchetype;
              // ISSUE: reference to a compiler-generated field
              local4.m_NodeCompositionArchetype = this.m_NetNodeCompositionArchetype;
              // ISSUE: reference to a compiler-generated field
              local4.m_EdgeCompositionArchetype = this.m_NetEdgeCompositionArchetype;
            }
          }
          if (bufferAccessor2.Length != 0)
          {
            DynamicBuffer<NetGeometrySection> dynamicBuffer = bufferAccessor2[nextIndex];
            if (dynamicBuffer.Length == 0)
            {
              // ISSUE: reference to a compiler-generated field
              NetGeometrySection elem = new NetGeometrySection()
              {
                m_Section = this.m_MeshSettingsData.m_MissingNetSection
              };
              dynamicBuffer.Add(elem);
            }
          }
          if (nativeArray6.Length != 0)
          {
            ref AggregateNetData local5 = ref nativeArray6.ElementAt<AggregateNetData>(nextIndex);
            if (nativeArray7.Length != 0)
            {
              ref NetNameData local6 = ref nativeArray7.ElementAt<NetNameData>(nextIndex);
              local6.m_Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 192);
              local6.m_SelectedColor = new Color32((byte) 192, (byte) 192, byte.MaxValue, (byte) 192);
            }
            if (nativeArray8.Length != 0)
            {
              ref NetArrowData local7 = ref nativeArray8.ElementAt<NetArrowData>(nextIndex);
              local7.m_RoadColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 192);
              local7.m_TrackColor = new Color32(byte.MaxValue, byte.MaxValue, (byte) 192, (byte) 192);
            }
            // ISSUE: reference to a compiler-generated field
            EntityArchetype aggregateArchetype = this.m_NetAggregateArchetype;
            local5.m_Archetype = aggregateArchetype;
          }
          if (nativeArray10.Length != 0)
          {
            nativeArray10.ElementAt<NetLaneData>(nextIndex).m_Flags &= ~LaneFlags.PseudoRandom;
            // ISSUE: reference to a compiler-generated field
            nativeArray11[nextIndex] = this.m_NetLaneArchetypeData;
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
      public ComponentTypeHandle<StackData> __Game_Prefabs_StackData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ObjectData> __Game_Prefabs_ObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<MovingObjectData> __Game_Prefabs_MovingObjectData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetData> __Game_Prefabs_NetData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetGeometryData> __Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AggregateNetData> __Game_Prefabs_AggregateNetData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetNameData> __Game_Prefabs_NetNameData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetArrowData> __Game_Prefabs_NetArrowData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetLaneData> __Game_Prefabs_NetLaneData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NetLaneArchetypeData> __Game_Prefabs_NetLaneArchetypeData_RW_ComponentTypeHandle;
      public BufferTypeHandle<SubMesh> __Game_Prefabs_SubMesh_RW_BufferTypeHandle;
      public BufferTypeHandle<NetGeometrySection> __Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MovingObjectData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovingObjectData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AggregateNetData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AggregateNetData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetNameData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetNameData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetArrowData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetArrowData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneArchetypeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetLaneArchetypeData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RW_BufferTypeHandle = state.GetBufferTypeHandle<SubMesh>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometrySection_RW_BufferTypeHandle = state.GetBufferTypeHandle<NetGeometrySection>();
      }
    }
  }
}
