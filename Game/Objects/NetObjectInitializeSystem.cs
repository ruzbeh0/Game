// Decompiled with JetBrains decompiler
// Type: Game.Objects.NetObjectInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class NetObjectInitializeSystem : GameSystemBase
  {
    private EntityQuery m_UpdateQuery;
    private NetObjectInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<NetObject>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_NetObject_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new NetObjectInitializeSystem.InitializeJob()
      {
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_SecondaryType = this.__TypeHandle.__Game_Objects_Secondary_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NetObjectType = this.__TypeHandle.__Game_Objects_NetObject_RW_ComponentTypeHandle,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_TrackLaneData = this.__TypeHandle.__Game_Net_TrackLane_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabNetObjectData = this.__TypeHandle.__Game_Prefabs_NetObjectData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabLaneDirectionData = this.__TypeHandle.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup
      }.ScheduleParallel<NetObjectInitializeSystem.InitializeJob>(this.m_UpdateQuery, this.Dependency);
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
    public NetObjectInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Secondary> m_SecondaryType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<NetObject> m_NetObjectType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> m_TrackLaneData;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> m_CarLaneData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetObjectData> m_PrefabNetObjectData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> m_PrefabLaneDirectionData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray1 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<NetObject> nativeArray6 = chunk.GetNativeArray<NetObject>(ref this.m_NetObjectType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Secondary>(ref this.m_SecondaryType);
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          PrefabRef prefabRef = nativeArray5[index];
          NetObject netObject = nativeArray6[index];
          netObject.m_Flags &= ~(NetObjectFlags.TrackPassThrough | NetObjectFlags.Backward);
          netObject.m_Flags |= NetObjectFlags.IsClear;
          NetObjectData componentData1;
          Attached attached;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData1) && CollectionUtils.TryGet<Attached>(nativeArray1, index, out attached) && this.m_NodeData.HasComponent(attached.m_Parent))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckNodeParent(ref netObject, componentData1, attached.m_Parent);
          }
          // ISSUE: reference to a compiler-generated field
          if (!flag && this.m_PrefabLaneDirectionData.HasComponent(prefabRef.m_Prefab))
          {
            Temp temp;
            if (CollectionUtils.TryGet<Temp>(nativeArray4, index, out temp) && (temp.m_Flags & (TempFlags.Delete | TempFlags.Select | TempFlags.Duplicate)) != (TempFlags) 0 && temp.m_Original != Entity.Null)
            {
              Owner componentData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_OwnerData.TryGetComponent(temp.m_Original, out componentData2))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckOwnerLanes(ref netObject, nativeArray3[index], componentData2.m_Owner);
              }
            }
            else
            {
              Owner owner;
              if (CollectionUtils.TryGet<Owner>(nativeArray2, index, out owner))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckOwnerLanes(ref netObject, nativeArray3[index], owner.m_Owner);
              }
            }
          }
          nativeArray6[index] = netObject;
        }
      }

      private void CheckNodeParent(
        ref NetObject netObject,
        NetObjectData netObjectData,
        Entity parent)
      {
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubLanes.TryGetBuffer(parent, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subLane = bufferData[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TrackLaneData.HasComponent(subLane))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((this.m_PrefabTrackLaneData[this.m_PrefabRefData[subLane].m_Prefab].m_TrackTypes & netObjectData.m_TrackPassThrough) != TrackTypes.None)
            {
              netObject.m_Flags &= ~NetObjectFlags.IsClear;
              netObject.m_Flags |= NetObjectFlags.TrackPassThrough;
            }
            else
            {
              netObject.m_Flags &= ~(NetObjectFlags.IsClear | NetObjectFlags.TrackPassThrough);
              break;
            }
          }
        }
      }

      private void CheckOwnerLanes(ref NetObject netObject, Transform transform, Entity owner)
      {
        DynamicBuffer<Game.Net.SubNet> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubNets.TryGetBuffer(owner, out bufferData1))
          return;
        float3 x = new float3();
        float num1 = 100f;
        for (int index1 = 0; index1 < bufferData1.Length; ++index1)
        {
          Entity subNet = bufferData1[index1].m_SubNet;
          float num2 = float.MaxValue;
          EdgeGeometry componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EdgeGeometryData.TryGetComponent(subNet, out componentData1))
          {
            num2 = MathUtils.DistanceSquared(componentData1.m_Bounds, transform.m_Position);
          }
          else
          {
            NodeGeometry componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeGeometryData.TryGetComponent(subNet, out componentData2))
              num2 = MathUtils.DistanceSquared(componentData2.m_Bounds, transform.m_Position);
          }
          DynamicBuffer<Game.Net.SubLane> bufferData2;
          // ISSUE: reference to a compiler-generated field
          if ((double) num2 < (double) num1 && this.m_SubLanes.TryGetBuffer(subNet, out bufferData2))
          {
            for (int index2 = 0; index2 < bufferData2.Length; ++index2)
            {
              Game.Net.SubLane subLane = bufferData2[index2];
              // ISSUE: reference to a compiler-generated field
              if ((subLane.m_PathMethods & PathMethod.Road) != (PathMethod) 0 && this.m_CarLaneData.HasComponent(subLane.m_SubLane))
              {
                // ISSUE: reference to a compiler-generated field
                Curve curve = this.m_CurveData[subLane.m_SubLane];
                if ((double) MathUtils.DistanceSquared(MathUtils.Bounds(curve.m_Bezier), transform.m_Position) < (double) num1)
                {
                  float t;
                  float num3 = MathUtils.DistanceSquared(curve.m_Bezier, transform.m_Position, out t);
                  if ((double) num3 < (double) num1)
                  {
                    x = MathUtils.Tangent(curve.m_Bezier, t);
                    num1 = num3;
                  }
                }
              }
            }
          }
        }
        if ((double) math.dot(x, math.forward(transform.m_Rotation)) >= 0.0)
          return;
        netObject.m_Flags |= NetObjectFlags.Backward;
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
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Secondary> __Game_Objects_Secondary_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NetObject> __Game_Objects_NetObject_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.TrackLane> __Game_Net_TrackLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetObjectData> __Game_Prefabs_NetObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LaneDirectionData> __Game_Prefabs_LaneDirectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Secondary_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Secondary>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_NetObject_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NetObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_TrackLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.TrackLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<Game.Net.CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetObjectData_RO_ComponentLookup = state.GetComponentLookup<NetObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LaneDirectionData_RO_ComponentLookup = state.GetComponentLookup<LaneDirectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
      }
    }
  }
}
