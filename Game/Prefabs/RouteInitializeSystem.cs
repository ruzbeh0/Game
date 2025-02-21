// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RouteInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Routes;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class RouteInitializeSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PrefabQuery;
    private RouteInitializeSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<RouteData>(),
          ComponentType.ReadOnly<TransportStopData>(),
          ComponentType.ReadOnly<MailBoxData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<RouteData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_RouteData_RW_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<TransportLineData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<TransportStopData> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MailBoxData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<MailBoxData> componentTypeHandle5 = this.__TypeHandle.__Game_Prefabs_MailBoxData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ObjectGeometryData> componentTypeHandle6 = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
        NativeArray<RouteData> nativeArray2 = archetypeChunk.GetNativeArray<RouteData>(ref componentTypeHandle2);
        if (nativeArray2.Length != 0)
        {
          float x = 0.0f;
          RouteType routeType = RouteType.None;
          if (archetypeChunk.Has<TransportLineData>(ref componentTypeHandle3))
          {
            x = 8f;
            routeType = RouteType.TransportLine;
          }
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            RoutePrefab prefab = this.m_PrefabSystem.GetPrefab<RoutePrefab>(nativeArray1[index2]);
            RouteData routeData = nativeArray2[index2] with
            {
              m_SnapDistance = math.max(x, prefab.m_Width),
              m_Type = routeType,
              m_Color = (Color32) prefab.m_Color,
              m_Width = prefab.m_Width,
              m_SegmentLength = prefab.m_SegmentLength
            };
            nativeArray2[index2] = routeData;
          }
        }
        if (archetypeChunk.Has<TransportStopData>(ref componentTypeHandle4) || archetypeChunk.Has<MailBoxData>(ref componentTypeHandle5))
        {
          NativeArray<ObjectGeometryData> nativeArray3 = archetypeChunk.GetNativeArray<ObjectGeometryData>(ref componentTypeHandle6);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            ObjectGeometryData objectGeometryData = nativeArray3[index3];
            objectGeometryData.m_Flags &= ~(GeometryFlags.Overridable | GeometryFlags.Brushable);
            nativeArray3[index3] = objectGeometryData;
          }
        }
      }
      archetypeChunkArray.Dispose();
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

    [Preserve]
    public RouteInitializeSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<RouteData> __Game_Prefabs_RouteData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TransportStopData> __Game_Prefabs_TransportStopData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MailBoxData> __Game_Prefabs_MailBoxData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<RouteData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportStopData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TransportStopData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MailBoxData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MailBoxData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ObjectGeometryData>();
      }
    }
  }
}
