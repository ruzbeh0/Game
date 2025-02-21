// Decompiled with JetBrains decompiler
// Type: Game.Serialization.RenterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
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
  public class RenterSystem : GameSystemBase
  {
    private DeserializationBarrier m_DeserializationBarrier;
    private EntityQuery m_Query;
    private RenterSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier = this.World.GetOrCreateSystemManaged<DeserializationBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<PropertyRenter>(),
          ComponentType.ReadOnly<TouristHousehold>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RenterSystem.RenterJob jobData = new RenterSystem.RenterJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_TouristHouseholdType = this.__TypeHandle.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle,
        m_ServiceBuildings = this.__TypeHandle.__Game_City_CityServiceUpkeep_RO_ComponentLookup,
        m_CompanyDatas = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RW_BufferLookup,
        m_CommandBuffer = this.m_DeserializationBarrier.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<RenterSystem.RenterJob>(this.m_Query, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_DeserializationBarrier.AddJobHandleForProducer(this.Dependency);
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
    public RenterSystem()
    {
    }

    [BurstCompile]
    private struct RenterJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public ComponentTypeHandle<TouristHousehold> m_TouristHouseholdType;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> m_ServiceBuildings;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyDatas;
      public BufferLookup<Renter> m_Renters;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray2 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TouristHousehold> nativeArray3 = chunk.GetNativeArray<TouristHousehold>(ref this.m_TouristHouseholdType);
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          DynamicBuffer<Renter> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Renters.TryGetBuffer(nativeArray2[index1].m_Property, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_CompanyDatas.HasComponent(entity))
            {
              bool flag = false;
              for (int index2 = 0; index2 < bufferData.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_CompanyDatas.HasComponent(bufferData[index2].m_Renter))
                {
                  Debug.LogWarning((object) string.Format("delete duplicate company:{0}", (object) entity.Index));
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Deleted>(entity);
                  flag = true;
                }
              }
              if (!flag)
                bufferData.Add(new Renter()
                {
                  m_Renter = entity
                });
            }
            else
              bufferData.Add(new Renter()
              {
                m_Renter = entity
              });
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_ServiceBuildings.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<PropertyRenter>(entity);
            }
          }
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          DynamicBuffer<Renter> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Renters.TryGetBuffer(nativeArray3[index].m_Hotel, out bufferData))
            bufferData.Add(new Renter()
            {
              m_Renter = entity
            });
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
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TouristHousehold> __Game_Citizens_TouristHousehold_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CityServiceUpkeep> __Game_City_CityServiceUpkeep_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      public BufferLookup<Renter> __Game_Buildings_Renter_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TouristHousehold_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TouristHousehold>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityServiceUpkeep_RO_ComponentLookup = state.GetComponentLookup<CityServiceUpkeep>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RW_BufferLookup = state.GetBufferLookup<Renter>();
      }
    }
  }
}
