// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.ModificationJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

#nullable disable
namespace Game.Pathfind
{
  public static class ModificationJobs
  {
    public interface IPathfindModificationJob
    {
      void SetPathfindData(NativePathfindData pathfindData);
    }

    [BurstCompile]
    public struct CreateEdgesJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public CreateAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        for (int index = 0; index < this.m_Action.m_CreateData.Length; ++index)
        {
          CreateActionData createActionData = this.m_Action.m_CreateData[index];
          if (createActionData.m_Specification.m_Methods != (PathMethod) 0)
          {
            EdgeID edge = this.m_PathfindData.CreateEdge(createActionData.m_StartNode, createActionData.m_MiddleNode, createActionData.m_EndNode, createActionData.m_Specification, createActionData.m_Location);
            this.m_PathfindData.AddEdge(createActionData.m_Owner, edge);
          }
          if (createActionData.m_SecondarySpecification.m_Methods != (PathMethod) 0)
          {
            PathNode startNode = new PathNode(createActionData.m_SecondaryStartNode, (createActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryStart) != 0);
            PathNode middleNode = new PathNode(createActionData.m_MiddleNode, true);
            PathNode endNode = new PathNode(createActionData.m_SecondaryEndNode, (createActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryEnd) != 0);
            PathSpecification secondarySpecification = createActionData.m_SecondarySpecification;
            secondarySpecification.m_Flags |= EdgeFlags.Secondary;
            LocationSpecification location = createActionData.m_Location;
            if ((createActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryStart) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              ++location.m_Line.a.y;
            if ((createActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryEnd) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              ++location.m_Line.b.y;
            EdgeID edge = this.m_PathfindData.CreateEdge(startNode, middleNode, endNode, secondarySpecification, location);
            this.m_PathfindData.AddSecondaryEdge(createActionData.m_Owner, edge);
          }
        }
      }
    }

    [BurstCompile]
    public struct UpdateEdgesJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public UpdateAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        for (int index = 0; index < this.m_Action.m_UpdateData.Length; ++index)
        {
          UpdateActionData updateActionData = this.m_Action.m_UpdateData[index];
          EdgeID edgeID;
          if (this.m_PathfindData.GetEdge(updateActionData.m_Owner, out edgeID))
            this.m_PathfindData.UpdateEdge(edgeID, updateActionData.m_StartNode, updateActionData.m_MiddleNode, updateActionData.m_EndNode, updateActionData.m_Specification, updateActionData.m_Location);
          if (this.m_PathfindData.GetSecondaryEdge(updateActionData.m_Owner, out edgeID))
          {
            PathNode startNode = new PathNode(updateActionData.m_SecondaryStartNode, (updateActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryStart) != 0);
            PathNode middleNode = new PathNode(updateActionData.m_MiddleNode, true);
            PathNode endNode = new PathNode(updateActionData.m_SecondaryEndNode, (updateActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryEnd) != 0);
            PathSpecification secondarySpecification = updateActionData.m_SecondarySpecification;
            secondarySpecification.m_Flags |= EdgeFlags.Secondary;
            LocationSpecification location = updateActionData.m_Location;
            if ((updateActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryStart) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              ++location.m_Line.a.y;
            if ((updateActionData.m_SecondarySpecification.m_Flags & EdgeFlags.SecondaryEnd) != ~(EdgeFlags.DefaultMask | EdgeFlags.Secondary))
              ++location.m_Line.b.y;
            this.m_PathfindData.UpdateEdge(edgeID, startNode, middleNode, endNode, secondarySpecification, location);
          }
        }
      }
    }

    [BurstCompile]
    public struct DeleteEdgesJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public DeleteAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        for (int index = 0; index < this.m_Action.m_DeleteData.Length; ++index)
        {
          DeleteActionData deleteActionData = this.m_Action.m_DeleteData[index];
          EdgeID edgeID;
          if (this.m_PathfindData.RemoveEdge(deleteActionData.m_Owner, out edgeID))
            this.m_PathfindData.DestroyEdge(edgeID);
          if (this.m_PathfindData.RemoveSecondaryEdge(deleteActionData.m_Owner, out edgeID))
            this.m_PathfindData.DestroyEdge(edgeID);
        }
      }
    }

    [BurstCompile]
    public struct SetDensityJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public DensityAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        NativeQueue<DensityActionData>.Enumerator enumerator = this.m_Action.m_DensityData.AsReadOnly().GetEnumerator();
        while (enumerator.MoveNext())
        {
          DensityActionData current = enumerator.Current;
          EdgeID edgeID;
          if (this.m_PathfindData.GetEdge(current.m_Owner, out edgeID))
            this.m_PathfindData.SetDensity(edgeID) = current.m_Density;
          if (this.m_PathfindData.GetSecondaryEdge(current.m_Owner, out edgeID))
            this.m_PathfindData.SetDensity(edgeID) = current.m_Density;
        }
        enumerator.Dispose();
      }
    }

    [BurstCompile]
    public struct SetTimeJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public TimeAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        NativeQueue<TimeActionData>.Enumerator enumerator = this.m_Action.m_TimeData.AsReadOnly().GetEnumerator();
        while (enumerator.MoveNext())
        {
          TimeActionData current = enumerator.Current;
          EdgeID edgeID1;
          if ((current.m_Flags & TimeActionFlags.SetPrimary) != (TimeActionFlags) 0 && this.m_PathfindData.GetEdge(current.m_Owner, out edgeID1))
          {
            this.m_PathfindData.SetCosts(edgeID1).m_Value.x = current.m_Time;
            this.m_PathfindData.SetEdgeDirections(edgeID1, current.m_StartNode, current.m_EndNode, (current.m_Flags & TimeActionFlags.EnableForward) != 0, (current.m_Flags & TimeActionFlags.EnableBackward) != 0);
          }
          EdgeID edgeID2;
          if ((current.m_Flags & TimeActionFlags.SetSecondary) != (TimeActionFlags) 0 && this.m_PathfindData.GetSecondaryEdge(current.m_Owner, out edgeID2))
          {
            this.m_PathfindData.SetCosts(edgeID2).m_Value.x = current.m_Time;
            EdgeFlags flags = this.m_PathfindData.GetFlags(edgeID2);
            PathNode startNode = new PathNode(current.m_SecondaryStartNode, (flags & EdgeFlags.SecondaryStart) != 0);
            PathNode endNode = new PathNode(current.m_SecondaryEndNode, (flags & EdgeFlags.SecondaryEnd) != 0);
            this.m_PathfindData.SetEdgeDirections(edgeID2, startNode, endNode, (current.m_Flags & TimeActionFlags.EnableForward) != 0, (current.m_Flags & TimeActionFlags.EnableBackward) != 0);
          }
        }
        enumerator.Dispose();
      }
    }

    [BurstCompile]
    public struct SetFlowJob : IJob, ModificationJobs.IPathfindModificationJob
    {
      [ReadOnly]
      public FlowAction m_Action;
      public NativePathfindData m_PathfindData;

      public void SetPathfindData(NativePathfindData pathfindData)
      {
        this.m_PathfindData = pathfindData;
      }

      public void Execute()
      {
        NativeQueue<FlowActionData>.Enumerator enumerator = this.m_Action.m_FlowData.AsReadOnly().GetEnumerator();
        while (enumerator.MoveNext())
        {
          FlowActionData current = enumerator.Current;
          EdgeID edgeID;
          if (this.m_PathfindData.GetEdge(current.m_Owner, out edgeID))
            this.m_PathfindData.SetFlowOffset(edgeID) = current.m_FlowOffset;
          if (this.m_PathfindData.GetSecondaryEdge(current.m_Owner, out edgeID))
            this.m_PathfindData.SetFlowOffset(edgeID) = current.m_FlowOffset;
        }
        enumerator.Dispose();
      }
    }
  }
}
