// Decompiled with JetBrains decompiler
// Type: Game.Policies.DefaultPoliciesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Policies
{
  [CompilerGenerated]
  public class DefaultPoliciesSystem : GameSystemBase, IPostDeserialize
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_CreatedQuery;
    private EntityQuery m_CityConfigurationQuery;
    private DefaultPoliciesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadWrite<Policy>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceFeeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DefaultPolicyData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Policy_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      DefaultPoliciesSystem.AddDefaultPoliciesJob jobData = new DefaultPoliciesSystem.AddDefaultPoliciesJob()
      {
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RW_BufferTypeHandle,
        m_PolicySliderData = this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentLookup,
        m_DefaultPolicyData = this.__TypeHandle.__Game_Prefabs_DefaultPolicyData_RO_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<DefaultPoliciesSystem.AddDefaultPoliciesJob>(this.m_CreatedQuery, this.Dependency);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose != Colossal.Serialization.Entities.Purpose.NewGame && (context.purpose != Colossal.Serialization.Entities.Purpose.LoadGame || !(context.version < Version.taxiFee)))
        return;
      // ISSUE: reference to a compiler-generated field
      Entity singletonEntity = this.m_CityConfigurationQuery.GetSingletonEntity();
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<Policy> buffer1 = entityManager.GetBuffer<Policy>(this.m_CitySystem.City);
      DynamicBuffer<DefaultPolicyData> buffer2;
      if (!this.EntityManager.TryGetBuffer<DefaultPolicyData>(singletonEntity, true, out buffer2))
        return;
      for (int index = 0; index < buffer2.Length; ++index)
      {
        DefaultPolicyData defaultPolicyData = buffer2[index];
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<PolicySliderData>(defaultPolicyData.m_Policy))
        {
          entityManager = this.EntityManager;
          PolicySliderData componentData = entityManager.GetComponentData<PolicySliderData>(defaultPolicyData.m_Policy);
          buffer1.Add(new Policy(defaultPolicyData.m_Policy, PolicyFlags.Active, componentData.m_Default));
        }
        else
          buffer1.Add(new Policy(defaultPolicyData.m_Policy, PolicyFlags.Active, 0.0f));
      }
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
    public DefaultPoliciesSystem()
    {
    }

    [BurstCompile]
    private struct AddDefaultPoliciesJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<Policy> m_PolicyType;
      [ReadOnly]
      public ComponentLookup<PolicySliderData> m_PolicySliderData;
      [ReadOnly]
      public BufferLookup<DefaultPolicyData> m_DefaultPolicyData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_DefaultPolicyData.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<DefaultPolicyData> dynamicBuffer1 = this.m_DefaultPolicyData[prefabRef.m_Prefab];
            DynamicBuffer<Policy> dynamicBuffer2 = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
            {
              DefaultPolicyData defaultPolicyData = dynamicBuffer1[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_PolicySliderData.HasComponent(defaultPolicyData.m_Policy))
              {
                // ISSUE: reference to a compiler-generated field
                PolicySliderData policySliderData = this.m_PolicySliderData[defaultPolicyData.m_Policy];
                dynamicBuffer2.Add(new Policy(defaultPolicyData.m_Policy, PolicyFlags.Active, policySliderData.m_Default));
              }
              else
                dynamicBuffer2.Add(new Policy(defaultPolicyData.m_Policy, PolicyFlags.Active, 0.0f));
            }
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public BufferTypeHandle<Policy> __Game_Policies_Policy_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PolicySliderData> __Game_Prefabs_PolicySliderData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<DefaultPolicyData> __Game_Prefabs_DefaultPolicyData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RW_BufferTypeHandle = state.GetBufferTypeHandle<Policy>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PolicySliderData_RO_ComponentLookup = state.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DefaultPolicyData_RO_BufferLookup = state.GetBufferLookup<DefaultPolicyData>(true);
      }
    }
  }
}
