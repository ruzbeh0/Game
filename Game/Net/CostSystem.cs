// Decompiled with JetBrains decompiler
// Type: Game.Net.CostSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class CostSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_UpdatedTempNetQuery;
    private EntityQuery m_EconomyParameterQuery;
    private CostSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedTempNetQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Temp>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Edge>(),
          ComponentType.ReadOnly<Node>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedTempNetQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      CostSystem.CalculateCostJob jobData = new CostSystem.CalculateCostJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RW_ComponentTypeHandle,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_TreeData = this.__TypeHandle.__Game_Objects_Tree_RO_ComponentLookup,
        m_RecentData = this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PlacableNetData = this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup,
        m_PlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CostSystem.CalculateCostJob>(this.m_UpdatedTempNetQuery, this.Dependency);
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
    public CostSystem()
    {
    }

    [BurstCompile]
    private struct CalculateCostJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Edge> m_EdgeType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public ComponentLookup<Recent> m_RecentData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> m_PlacableNetData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Edge> nativeArray3 = chunk.GetNativeArray<Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Composition> nativeArray4 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray5 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Objects.SubObject> bufferAccessor = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Temp temp = nativeArray5[index1] with
          {
            m_Cost = 0,
            m_Value = 0
          };
          if ((temp.m_Flags & TempFlags.Essential) != (TempFlags) 0)
          {
            int oldCost = 0;
            int newCost = 0;
            if (nativeArray4.Length != 0)
            {
              Curve curve1 = nativeArray2[index1];
              Edge edge1 = nativeArray3[index1];
              Composition composition = nativeArray4[index1];
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade | TempFlags.RemoveCost)) != (TempFlags) 0)
              {
                Composition componentData1;
                // ISSUE: reference to a compiler-generated field
                if (this.m_CompositionData.TryGetComponent(temp.m_Original, out componentData1))
                {
                  // ISSUE: reference to a compiler-generated field
                  Curve curve2 = this.m_CurveData[temp.m_Original];
                  // ISSUE: reference to a compiler-generated field
                  Edge edge2 = this.m_EdgeData[temp.m_Original];
                  PlaceableNetComposition componentData2;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PlacableNetData.TryGetComponent(composition.m_Edge, out componentData2))
                  {
                    Elevation componentData3;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge1.m_Start, out componentData3);
                    Elevation componentData4;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge1.m_End, out componentData4);
                    newCost += NetUtils.GetConstructionCost(curve1, componentData3, componentData4, componentData2);
                  }
                  PlaceableNetComposition componentData5;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PlacableNetData.TryGetComponent(componentData1.m_Edge, out componentData5))
                  {
                    Elevation componentData6;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge2.m_Start, out componentData6);
                    Elevation componentData7;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge2.m_End, out componentData7);
                    oldCost += NetUtils.GetConstructionCost(curve2, componentData6, componentData7, componentData5);
                  }
                }
                else
                {
                  PlaceableNetComposition componentData8;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PlacableNetData.TryGetComponent(composition.m_Edge, out componentData8))
                  {
                    Elevation componentData9;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge1.m_Start, out componentData9);
                    Elevation componentData10;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ElevationData.TryGetComponent(edge1.m_End, out componentData10);
                    newCost += NetUtils.GetConstructionCost(curve1, componentData9, componentData10, componentData8);
                  }
                }
              }
            }
            if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade | TempFlags.RemoveCost)) != (TempFlags) 0)
            {
              DynamicBuffer<Game.Objects.SubObject> dynamicBuffer;
              if (CollectionUtils.TryGet<Game.Objects.SubObject>(bufferAccessor, index1, out dynamicBuffer))
              {
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity subObject = dynamicBuffer[index2].m_SubObject;
                  Owner componentData11;
                  PlaceableObjectData componentData12;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.TryGetComponent(subObject, out componentData11) && componentData11.m_Owner == entity && this.m_PlaceableObjectData.TryGetComponent(this.m_PrefabRefData[subObject].m_Prefab, out componentData12))
                  {
                    int constructionCost = (int) componentData12.m_ConstructionCost;
                    Tree componentData13;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TreeData.TryGetComponent(subObject, out componentData13))
                    {
                      // ISSUE: reference to a compiler-generated field
                      constructionCost = ObjectUtils.GetContructionCost(constructionCost, componentData13, in this.m_EconomyParameterData);
                    }
                    newCost += constructionCost;
                  }
                }
              }
              DynamicBuffer<Game.Objects.SubObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubObjects.TryGetBuffer(temp.m_Original, out bufferData))
              {
                for (int index3 = 0; index3 < bufferData.Length; ++index3)
                {
                  Entity subObject = bufferData[index3].m_SubObject;
                  Owner componentData14;
                  PlaceableObjectData componentData15;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OwnerData.TryGetComponent(subObject, out componentData14) && componentData14.m_Owner == temp.m_Original && this.m_PlaceableObjectData.TryGetComponent(this.m_PrefabRefData[subObject].m_Prefab, out componentData15))
                  {
                    int constructionCost = (int) componentData15.m_ConstructionCost;
                    Tree componentData16;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_TreeData.TryGetComponent(subObject, out componentData16))
                    {
                      // ISSUE: reference to a compiler-generated field
                      constructionCost = ObjectUtils.GetContructionCost(constructionCost, componentData16, in this.m_EconomyParameterData);
                    }
                    oldCost += constructionCost;
                  }
                }
              }
            }
            temp.m_Value = newCost;
            if ((temp.m_Flags & TempFlags.RemoveCost) != (TempFlags) 0)
            {
              temp.m_Cost = -oldCost;
              if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
                temp.m_Cost += newCost;
            }
            else if ((temp.m_Flags & TempFlags.Delete) != (TempFlags) 0)
            {
              Recent componentData;
              // ISSUE: reference to a compiler-generated field
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0 && this.m_RecentData.TryGetComponent(temp.m_Original, out componentData))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                temp.m_Cost = -NetUtils.GetRefundAmount(componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
              }
            }
            else
            {
              Recent componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              temp.m_Cost = !this.m_RecentData.TryGetComponent(temp.m_Original, out componentData) ? NetUtils.GetUpgradeCost(newCost, oldCost) : NetUtils.GetUpgradeCost(newCost, oldCost, componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
            }
          }
          nativeArray5[index1] = temp;
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
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Tree> __Game_Objects_Tree_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Recent> __Game_Tools_Recent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> __Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Tree_RO_ComponentLookup = state.GetComponentLookup<Tree>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Recent_RO_ComponentLookup = state.GetComponentLookup<Recent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetComposition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
