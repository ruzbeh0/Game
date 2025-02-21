// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AreaPathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Objects;
using Game.Pathfind;
using Game.Vehicles;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct AreaPathfindSetup
  {
    private ComponentLookup<Tree> m_TreeData;
    private BufferLookup<SubObject> m_SubObjects;
    private BufferLookup<WoodResource> m_WoodResources;
    private BufferLookup<SubArea> m_SubAreas;

    public AreaPathfindSetup(PathfindSetupSystem system)
    {
      this.m_TreeData = system.GetComponentLookup<Tree>(true);
      this.m_SubObjects = system.GetBufferLookup<SubObject>(true);
      this.m_WoodResources = system.GetBufferLookup<WoodResource>(true);
      this.m_SubAreas = system.GetBufferLookup<SubArea>(true);
    }

    public JobHandle SetupAreaLocation(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_SubObjects.Update((SystemBase) system);
      this.m_SubAreas.Update((SystemBase) system);
      return new AreaPathfindSetup.SetupAreaLocationJob()
      {
        m_SubObjects = this.m_SubObjects,
        m_SubAreas = this.m_SubAreas,
        m_SetupData = setupData
      }.Schedule<AreaPathfindSetup.SetupAreaLocationJob>(setupData.Length, 1, inputDeps);
    }

    public JobHandle SetupWoodResource(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_TreeData.Update((SystemBase) system);
      this.m_WoodResources.Update((SystemBase) system);
      this.m_SubAreas.Update((SystemBase) system);
      return new AreaPathfindSetup.SetupWoodResourceJob()
      {
        m_TreeData = this.m_TreeData,
        m_WoodResources = this.m_WoodResources,
        m_SubAreas = this.m_SubAreas,
        m_SetupData = setupData
      }.Schedule<AreaPathfindSetup.SetupWoodResourceJob>(setupData.Length, 1, inputDeps);
    }

    [BurstCompile]
    private struct SetupAreaLocationJob : IJobParallelFor
    {
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<SubArea> m_SubAreas;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity, out targetSeeker);
        if (!this.m_SubObjects.HasBuffer(entity))
          return;
        DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[entity];
        Random random = targetSeeker.m_RandomSeed.GetRandom(0);
        DynamicBuffer<SubArea> bufferData;
        this.m_SubAreas.TryGetBuffer(entity, out bufferData);
        for (int index1 = 0; index1 < subObject1.Length; ++index1)
        {
          Entity subObject2 = subObject1[index1].m_SubObject;
          float cost = random.NextFloat(600f);
          targetSeeker.AddAreaTargets(ref random, subObject2, entity, subObject2, bufferData, cost, false, EdgeFlags.DefaultMask);
        }
      }
    }

    [BurstCompile]
    private struct SetupWoodResourceJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<Tree> m_TreeData;
      [ReadOnly]
      public BufferLookup<WoodResource> m_WoodResources;
      [ReadOnly]
      public BufferLookup<SubArea> m_SubAreas;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(int index)
      {
        Entity entity;
        PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
        // ISSUE: reference to a compiler-generated method
        this.m_SetupData.GetItem(index, out entity, out targetSeeker);
        VehicleWorkType vehicleWorkType = (VehicleWorkType) targetSeeker.m_SetupQueueTarget.m_Value;
        if (!this.m_WoodResources.HasBuffer(entity))
          return;
        DynamicBuffer<WoodResource> woodResource = this.m_WoodResources[entity];
        Random random = targetSeeker.m_RandomSeed.GetRandom(0);
        DynamicBuffer<SubArea> bufferData;
        this.m_SubAreas.TryGetBuffer(entity, out bufferData);
        for (int index1 = 0; index1 < woodResource.Length; ++index1)
        {
          Entity tree1 = woodResource[index1].m_Tree;
          Tree tree2 = this.m_TreeData[tree1];
          float cost = random.NextFloat(15f);
          switch (vehicleWorkType)
          {
            case VehicleWorkType.Harvest:
              if ((tree2.m_State & TreeState.Adult) != (TreeState) 0)
              {
                cost += (float) (511 - (int) tree2.m_Growth) * (15f / 128f);
                goto default;
              }
              else if ((tree2.m_State & TreeState.Elderly) != (TreeState) 0)
              {
                cost += (float) ((int) byte.MaxValue - (int) tree2.m_Growth) * (15f / 128f);
                goto default;
              }
              else
                break;
            case VehicleWorkType.Collect:
              if ((tree2.m_State & (TreeState.Stump | TreeState.Collected)) == TreeState.Stump)
              {
                cost += (float) tree2.m_Growth * (15f / 128f);
                goto default;
              }
              else if ((tree2.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Collected)) == (TreeState) 0)
              {
                cost += (float) (256 + (int) tree2.m_Growth) * (15f / 128f);
                goto default;
              }
              else
                break;
            default:
              targetSeeker.AddAreaTargets(ref random, tree1, entity, tree1, bufferData, cost, false, EdgeFlags.DefaultMask);
              break;
          }
        }
      }
    }
  }
}
