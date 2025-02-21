// Decompiled with JetBrains decompiler
// Type: Game.Simulation.MilestoneSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class MilestoneSystem : GameSystemBase, IMilestoneSystem
  {
    private int m_LastRequired;
    private int m_NextRequired;
    private int m_Progress;
    private int m_NextMilestone;
    private CitySystem m_CitySystem;
    private ModificationEndBarrier m_ModificationEndBarrier;
    private EntityArchetype m_UnlockEventArchetype;
    private EntityArchetype m_MilestoneReachedEventArchetype;
    private EntityQuery m_MilestoneLevelGroup;
    private EntityQuery m_XPGroup;
    private EntityQuery m_MilestoneGroup;

    public int currentXP => this.m_Progress;

    public int requiredXP => this.nextRequiredXP - math.max(0, this.lastRequiredXP);

    public int lastRequiredXP => this.m_LastRequired;

    public int nextRequiredXP => this.m_NextRequired;

    public float progress => (float) this.m_Progress / (float) this.requiredXP;

    public int nextMilestone => this.m_NextMilestone;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneReachedEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<MilestoneReachedEvent>());
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneLevelGroup = this.GetEntityQuery(ComponentType.ReadWrite<MilestoneLevel>());
      // ISSUE: reference to a compiler-generated field
      this.m_XPGroup = this.GetEntityQuery(ComponentType.ReadOnly<XP>());
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneGroup = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MilestoneLevelGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_XPGroup);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_MilestoneGroup);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      MilestoneLevel singleton = this.m_MilestoneLevelGroup.GetSingleton<MilestoneLevel>();
      int achievedMilestone = singleton.m_AchievedMilestone;
      Entity entity;
      MilestoneData milestone1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LastRequired = this.TryGetMilestone(achievedMilestone, out entity, out milestone1) ? milestone1.m_XpRequried : 0;
      MilestoneData milestone2;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetMilestone(achievedMilestone + 1, out entity, out milestone2))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NextRequired = milestone2.m_XpRequried;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CitySystem.XP >= this.m_NextRequired)
        {
          ++singleton.m_AchievedMilestone;
          // ISSUE: reference to a compiler-generated field
          this.m_MilestoneLevelGroup.SetSingleton<MilestoneLevel>(singleton);
          // ISSUE: reference to a compiler-generated method
          this.NextMilestone(singleton.m_AchievedMilestone);
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Progress = this.m_CitySystem.XP - math.max(0, this.m_LastRequired);
      // ISSUE: reference to a compiler-generated field
      this.m_NextMilestone = singleton.m_AchievedMilestone + 1;
    }

    private void NextMilestone(int index)
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_ModificationEndBarrier.CreateCommandBuffer();
      Entity entity1;
      MilestoneData milestone;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetMilestone(index, out entity1, out milestone))
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity2 = commandBuffer.CreateEntity(this.m_MilestoneReachedEventArchetype);
        commandBuffer.SetComponent<MilestoneReachedEvent>(entity2, new MilestoneReachedEvent(entity1, index));
        // ISSUE: reference to a compiler-generated field
        Entity entity3 = commandBuffer.CreateEntity(this.m_UnlockEventArchetype);
        commandBuffer.SetComponent<Unlock>(entity3, new Unlock(entity1));
        // ISSUE: reference to a compiler-generated field
        PlayerMoney componentData1 = this.EntityManager.GetComponentData<PlayerMoney>(this.m_CitySystem.City);
        componentData1.Add(milestone.m_Reward);
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetComponentData<PlayerMoney>(this.m_CitySystem.City, componentData1);
        EntityManager entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        Creditworthiness componentData2 = entityManager.GetComponentData<Creditworthiness>(this.m_CitySystem.City);
        componentData2.m_Amount += milestone.m_LoanLimit;
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.SetComponentData<Creditworthiness>(this.m_CitySystem.City, componentData2);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity4 = commandBuffer.CreateEntity(this.m_MilestoneReachedEventArchetype);
        commandBuffer.SetComponent<MilestoneReachedEvent>(entity4, new MilestoneReachedEvent(Entity.Null, index));
        Debug.LogWarning((object) ("Warning: did not find data for milestone " + index.ToString()));
      }
    }

    private bool TryGetMilestone(int index, out Entity entity, out MilestoneData milestone)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_MilestoneGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<MilestoneData> componentDataArray = this.m_MilestoneGroup.ToComponentDataArray<MilestoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        for (int index1 = 0; index1 < componentDataArray.Length; ++index1)
        {
          if (componentDataArray[index1].m_Index == index)
          {
            entity = entityArray[index1];
            milestone = componentDataArray[index1];
            return true;
          }
        }
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
      entity = new Entity();
      milestone = new MilestoneData();
      return false;
    }

    public void UnlockAllMilestones()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_MilestoneGroup.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<MilestoneData> componentDataArray = this.m_MilestoneGroup.ToComponentDataArray<MilestoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      MilestoneLevel singleton = this.m_MilestoneLevelGroup.GetSingleton<MilestoneLevel>();
      // ISSUE: reference to a compiler-generated field
      PlayerMoney componentData1 = this.EntityManager.GetComponentData<PlayerMoney>(this.m_CitySystem.City);
      // ISSUE: reference to a compiler-generated field
      Creditworthiness componentData2 = this.EntityManager.GetComponentData<Creditworthiness>(this.m_CitySystem.City);
      try
      {
        for (int achievedMilestone = singleton.m_AchievedMilestone; achievedMilestone < componentDataArray.Length; ++achievedMilestone)
        {
          EntityManager entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          Entity entity = entityManager.CreateEntity(this.m_UnlockEventArchetype);
          entityManager = this.EntityManager;
          entityManager.SetComponentData<Unlock>(entity, new Unlock(entityArray[achievedMilestone]));
          singleton.m_AchievedMilestone = math.max(singleton.m_AchievedMilestone, componentDataArray[achievedMilestone].m_Index);
          componentData1.Add(componentDataArray[achievedMilestone].m_Reward);
          componentData2.m_Amount += componentDataArray[achievedMilestone].m_LoanLimit;
        }
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneLevelGroup.SetSingleton<MilestoneLevel>(singleton);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentData<PlayerMoney>(this.m_CitySystem.City, componentData1);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.SetComponentData<Creditworthiness>(this.m_CitySystem.City, componentData2);
    }

    [Preserve]
    public MilestoneSystem()
    {
    }
  }
}
