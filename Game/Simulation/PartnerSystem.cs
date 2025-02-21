// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PartnerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Economy;
using Game.Serialization;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PartnerSystem : GameSystemBase, IPostDeserialize
  {
    public static readonly int kUpdatesPerDay = 4;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_PartnerQuery;
    private TriggerSystem m_TriggerSystem;
    private PartnerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (PartnerSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_PartnerQuery = this.GetEntityQuery(ComponentType.ReadWrite<LookingForPartner>());
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PartnerQuery.IsEmptyIgnoreFilter)
        return;
      this.World.EntityManager.CreateEntity(ComponentType.ReadWrite<LookingForPartner>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdPet_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_LookingForPartner_RW_BufferLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PartnerSystem.MatchJob jobData = new PartnerSystem.MatchJob()
      {
        m_PartnerEntity = this.m_PartnerQuery.GetSingletonEntity(),
        m_Partners = this.__TypeHandle.__Game_Citizens_LookingForPartner_RW_BufferLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RW_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RW_BufferLookup,
        m_HouseholdAnimals = this.__TypeHandle.__Game_Citizens_HouseholdAnimal_RW_BufferLookup,
        m_Households = this.__TypeHandle.__Game_Citizens_Household_RW_ComponentLookup,
        m_HouseholdPets = this.__TypeHandle.__Game_Citizens_HouseholdPet_RW_ComponentLookup,
        m_HouseholdMembers = this.__TypeHandle.__Game_Citizens_HouseholdMember_RW_ComponentLookup,
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_TriggerBuffer = this.m_TriggerSystem.CreateActionBuffer()
      };
      this.Dependency = jobData.Schedule<PartnerSystem.MatchJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TriggerSystem.AddActionBufferWriter(this.Dependency);
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
    public PartnerSystem()
    {
    }

    [BurstCompile]
    private struct MatchJob : IJob
    {
      public Entity m_PartnerEntity;
      public BufferLookup<LookingForPartner> m_Partners;
      public ComponentLookup<Citizen> m_Citizens;
      public ComponentLookup<Household> m_Households;
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      public BufferLookup<HouseholdAnimal> m_HouseholdAnimals;
      public ComponentLookup<HouseholdMember> m_HouseholdMembers;
      public ComponentLookup<HouseholdPet> m_HouseholdPets;
      public BufferLookup<Resources> m_Resources;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<TriggerAction> m_TriggerBuffer;

      private void MoveTogether(Entity citizen1, Entity citizen2)
      {
        // ISSUE: reference to a compiler-generated field
        Entity household1 = this.m_HouseholdMembers[citizen1].m_Household;
        // ISSUE: reference to a compiler-generated field
        Entity household2 = this.m_HouseholdMembers[citizen2].m_Household;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Households.HasComponent(household1) || !this.m_Households.HasComponent(household2))
          return;
        // ISSUE: reference to a compiler-generated field
        Household household3 = this.m_Households[household1];
        // ISSUE: reference to a compiler-generated field
        Household household4 = this.m_Households[household2];
        household3.m_Resources = (int) math.clamp((long) household3.m_Resources + (long) household4.m_Resources, (long) int.MinValue, (long) int.MaxValue);
        // ISSUE: reference to a compiler-generated field
        this.m_Households[household1] = household3;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Resources> resource1 = this.m_Resources[household1];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Resources> resource2 = this.m_Resources[household2];
        for (int index = 0; index < resource2.Length; ++index)
          EconomyUtils.AddResources(resource2[index].m_Resource, resource2[index].m_Amount, resource1);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdCitizen> householdCitizen1 = this.m_HouseholdCitizens[household1];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdCitizen> householdCitizen2 = this.m_HouseholdCitizens[household2];
        for (int index = 0; index < householdCitizen2.Length; ++index)
        {
          householdCitizen1.Add(householdCitizen2[index]);
          // ISSUE: reference to a compiler-generated field
          this.m_HouseholdMembers[householdCitizen2[index].m_Citizen] = new HouseholdMember()
          {
            m_Household = household1
          };
        }
        householdCitizen2.Clear();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_HouseholdAnimals.HasBuffer(household2))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<HouseholdAnimal> householdAnimal = this.m_HouseholdAnimals[household2];
        for (int index = 0; index < householdAnimal.Length; ++index)
        {
          DynamicBuffer<HouseholdAnimal> dynamicBuffer = new DynamicBuffer<HouseholdAnimal>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          (this.m_HouseholdAnimals.HasBuffer(household1) ? this.m_HouseholdAnimals[household1] : this.m_CommandBuffer.AddBuffer<HouseholdAnimal>(household1)).Add(householdAnimal[index]);
          // ISSUE: reference to a compiler-generated field
          this.m_HouseholdPets[householdAnimal[index].m_HouseholdPet] = new HouseholdPet()
          {
            m_Household = household1
          };
        }
        householdAnimal.Clear();
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LookingForPartner> partner = this.m_Partners[this.m_PartnerEntity];
        for (int index1 = partner.Length - 1; index1 >= 0; --index1)
        {
          LookingForPartner lookingForPartner1 = partner[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_Citizens.HasComponent(lookingForPartner1.m_Citizen))
          {
            // ISSUE: reference to a compiler-generated field
            bool flag1 = (this.m_Citizens[lookingForPartner1.m_Citizen].m_State & CitizenFlags.Male) != 0;
            for (int index2 = index1 - 1; index2 >= 0; --index2)
            {
              LookingForPartner lookingForPartner2 = partner[index2];
              // ISSUE: reference to a compiler-generated field
              if (this.m_Citizens.HasComponent(lookingForPartner2.m_Citizen))
              {
                // ISSUE: reference to a compiler-generated field
                bool flag2 = (this.m_Citizens[lookingForPartner2.m_Citizen].m_State & CitizenFlags.Male) != 0;
                bool flag3 = flag1 == flag2;
                if ((lookingForPartner2.m_PartnerType == PartnerType.Any || flag3 && lookingForPartner2.m_PartnerType == PartnerType.Same || !flag3 && lookingForPartner2.m_PartnerType == PartnerType.Other) && (lookingForPartner1.m_PartnerType == PartnerType.Any || flag3 && lookingForPartner1.m_PartnerType == PartnerType.Same || !flag3 && lookingForPartner1.m_PartnerType == PartnerType.Other))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.MoveTogether(partner[index1].m_Citizen, partner[index2].m_Citizen);
                  lookingForPartner1.m_PartnerType = PartnerType.None;
                  partner[index1] = lookingForPartner1;
                  lookingForPartner2.m_PartnerType = PartnerType.None;
                  partner[index2] = lookingForPartner2;
                  // ISSUE: reference to a compiler-generated field
                  Citizen citizen1 = this.m_Citizens[lookingForPartner1.m_Citizen];
                  citizen1.m_State &= ~CitizenFlags.LookingForPartner;
                  // ISSUE: reference to a compiler-generated field
                  this.m_Citizens[lookingForPartner1.m_Citizen] = citizen1;
                  // ISSUE: reference to a compiler-generated field
                  Citizen citizen2 = this.m_Citizens[lookingForPartner2.m_Citizen];
                  citizen2.m_State &= ~CitizenFlags.LookingForPartner;
                  // ISSUE: reference to a compiler-generated field
                  this.m_Citizens[lookingForPartner2.m_Citizen] = citizen2;
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenPartneredUp, Entity.Null, lookingForPartner1.m_Citizen, lookingForPartner2.m_Citizen));
                  // ISSUE: reference to a compiler-generated field
                  this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.CitizenPartneredUp, Entity.Null, lookingForPartner2.m_Citizen, lookingForPartner1.m_Citizen));
                }
              }
            }
          }
        }
        for (int index = partner.Length - 1; index >= 0; --index)
        {
          // ISSUE: reference to a compiler-generated field
          if (partner[index].m_PartnerType == PartnerType.None || !this.m_Citizens.HasComponent(partner[index].m_Citizen))
          {
            partner[index] = partner[partner.Length - 1];
            partner.RemoveAt(partner.Length - 1);
          }
        }
      }
    }

    private struct TypeHandle
    {
      public BufferLookup<LookingForPartner> __Game_Citizens_LookingForPartner_RW_BufferLookup;
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RW_ComponentLookup;
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RW_BufferLookup;
      public BufferLookup<HouseholdAnimal> __Game_Citizens_HouseholdAnimal_RW_BufferLookup;
      public ComponentLookup<Household> __Game_Citizens_Household_RW_ComponentLookup;
      public ComponentLookup<HouseholdPet> __Game_Citizens_HouseholdPet_RW_ComponentLookup;
      public ComponentLookup<HouseholdMember> __Game_Citizens_HouseholdMember_RW_ComponentLookup;
      public BufferLookup<Resources> __Game_Economy_Resources_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_LookingForPartner_RW_BufferLookup = state.GetBufferLookup<LookingForPartner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RW_ComponentLookup = state.GetComponentLookup<Citizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RW_BufferLookup = state.GetBufferLookup<HouseholdCitizen>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdAnimal_RW_BufferLookup = state.GetBufferLookup<HouseholdAnimal>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Household_RW_ComponentLookup = state.GetComponentLookup<Household>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdPet_RW_ComponentLookup = state.GetComponentLookup<HouseholdPet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdMember_RW_ComponentLookup = state.GetComponentLookup<HouseholdMember>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Resources>();
      }
    }
  }
}
