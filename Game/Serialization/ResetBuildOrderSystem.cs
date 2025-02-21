// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ResetBuildOrderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ResetBuildOrderSystem : GameSystemBase
  {
    private GenerateEdgesSystem m_GenerateEdgesSystem;
    private EntityQuery m_BuildOrderQuery;
    private ResetBuildOrderSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GenerateEdgesSystem = this.World.GetOrCreateSystemManaged<GenerateEdgesSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildOrderQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Net.BuildOrder>(),
          ComponentType.ReadOnly<Game.Zones.BuildOrder>()
        }
      });
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_BuildOrderQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_BuildOrder_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new ResetBuildOrderSystem.ResetBuildOrderJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_NetBuildOrderType = this.__TypeHandle.__Game_Net_BuildOrder_RW_ComponentTypeHandle,
        m_ZoneBuildOrderType = this.__TypeHandle.__Game_Zones_BuildOrder_RW_ComponentTypeHandle,
        m_BuildOrder = this.m_GenerateEdgesSystem.GetBuildOrder()
      }.Schedule<ResetBuildOrderSystem.ResetBuildOrderJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps);
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
    public ResetBuildOrderSystem()
    {
    }

    [BurstCompile]
    private struct ResetBuildOrderJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public ComponentTypeHandle<Game.Net.BuildOrder> m_NetBuildOrderType;
      public ComponentTypeHandle<Game.Zones.BuildOrder> m_ZoneBuildOrderType;
      public NativeValue<uint> m_BuildOrder;

      public void Execute()
      {
        int num1 = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index];
          num1 += chunk.Count;
        }
        if (num1 != 0)
        {
          NativeArray<ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem> array = new NativeArray<ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem>(num1, Allocator.Temp);
          NativeParallelHashMap<uint, uint> nativeParallelHashMap = new NativeParallelHashMap<uint, uint>(num1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          int num2 = 0;
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk chunk = this.m_Chunks[index1];
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.BuildOrder> nativeArray1 = chunk.GetNativeArray<Game.Net.BuildOrder>(ref this.m_NetBuildOrderType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Zones.BuildOrder> nativeArray2 = chunk.GetNativeArray<Game.Zones.BuildOrder>(ref this.m_ZoneBuildOrderType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              // ISSUE: object of a compiler-generated type is created
              array[num2++] = new ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem(nativeArray1[index2]);
            }
            for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
            {
              // ISSUE: object of a compiler-generated type is created
              array[num2++] = new ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem(nativeArray2[index3]);
            }
          }
          array.Sort<ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem>();
          // ISSUE: variable of a compiler-generated type
          ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem orderItem1 = array[0];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem orderItem2 = new ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem(0U, orderItem1.m_Max - orderItem1.m_Min);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          nativeParallelHashMap.TryAdd(orderItem1.m_Min, orderItem2.m_Min);
          for (int index = 1; index < num2; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem orderItem3 = array[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (orderItem3.m_Min <= orderItem1.m_Max)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (orderItem3.m_Max > orderItem1.m_Max)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                orderItem1.m_Max = orderItem3.m_Max;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                orderItem2.m_Max = orderItem2.m_Min + (orderItem3.m_Max - orderItem1.m_Min);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (orderItem3.m_Min > orderItem1.m_Min)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                nativeParallelHashMap.TryAdd(orderItem3.m_Min, orderItem2.m_Min + (orderItem3.m_Min - orderItem1.m_Min));
              }
            }
            else
            {
              orderItem1 = orderItem3;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              orderItem2.m_Min = orderItem2.m_Max + 1U;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              orderItem2.m_Max = orderItem2.m_Min + (orderItem3.m_Max - orderItem3.m_Min);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              nativeParallelHashMap.TryAdd(orderItem3.m_Min, orderItem2.m_Min);
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index4 = 0; index4 < this.m_Chunks.Length; ++index4)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk chunk = this.m_Chunks[index4];
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Net.BuildOrder> nativeArray3 = chunk.GetNativeArray<Game.Net.BuildOrder>(ref this.m_NetBuildOrderType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Zones.BuildOrder> nativeArray4 = chunk.GetNativeArray<Game.Zones.BuildOrder>(ref this.m_ZoneBuildOrderType);
            for (int index5 = 0; index5 < nativeArray3.Length; ++index5)
            {
              Game.Net.BuildOrder buildOrder1 = nativeArray3[index5];
              Game.Net.BuildOrder buildOrder2;
              if (buildOrder1.m_End >= buildOrder1.m_Start)
              {
                buildOrder2.m_Start = nativeParallelHashMap[buildOrder1.m_Start];
                buildOrder2.m_End = buildOrder2.m_Start + (buildOrder1.m_End - buildOrder1.m_Start);
              }
              else
              {
                buildOrder2.m_End = nativeParallelHashMap[buildOrder1.m_End];
                buildOrder2.m_Start = buildOrder2.m_End + (buildOrder1.m_Start - buildOrder1.m_End);
              }
              nativeArray3[index5] = buildOrder2;
            }
            for (int index6 = 0; index6 < nativeArray4.Length; ++index6)
            {
              Game.Zones.BuildOrder buildOrder = nativeArray4[index6];
              buildOrder.m_Order = nativeParallelHashMap[buildOrder.m_Order];
              nativeArray4[index6] = buildOrder;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_BuildOrder.value = orderItem2.m_Max + 1U;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BuildOrder.value = 0U;
        }
      }

      private struct OrderItem : IComparable<ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem>
      {
        public uint m_Min;
        public uint m_Max;

        public OrderItem(uint min, uint max)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Min = min;
          // ISSUE: reference to a compiler-generated field
          this.m_Max = max;
        }

        public OrderItem(Game.Net.BuildOrder buildOrder)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Min = math.min(buildOrder.m_Start, buildOrder.m_End);
          // ISSUE: reference to a compiler-generated field
          this.m_Max = math.max(buildOrder.m_Start, buildOrder.m_End);
        }

        public OrderItem(Game.Zones.BuildOrder buildOrder)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Min = buildOrder.m_Order;
          // ISSUE: reference to a compiler-generated field
          this.m_Max = buildOrder.m_Order;
        }

        public int CompareTo(
          ResetBuildOrderSystem.ResetBuildOrderJob.OrderItem other)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return math.select(0, math.select(-1, 1, this.m_Min > other.m_Min), (int) this.m_Min != (int) other.m_Min);
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentTypeHandle<Game.Net.BuildOrder> __Game_Net_BuildOrder_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Zones.BuildOrder> __Game_Zones_BuildOrder_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_BuildOrder_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.BuildOrder>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_BuildOrder_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Zones.BuildOrder>();
      }
    }
  }
}
