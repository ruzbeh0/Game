// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.RaycastWaterTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class RaycastWaterTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private WaterPipeFlowSystem m_WaterPipeFlowSystem;
    private EntityQuery m_InfomodeQuery;
    private IntTooltip m_WaterCapacity;
    private IntTooltip m_WaterUsage;
    private IntTooltip m_SewageCapacity;
    private IntTooltip m_SewageUsage;
    private ProgressTooltip m_WaterConsumption;
    private ProgressTooltip m_SewageConsumption;
    private IntTooltip m_WaterFlow;
    private IntTooltip m_SewageFlow;
    private RaycastWaterTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeFlowSystem = this.World.GetOrCreateSystemManaged<WaterPipeFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewNetStatusData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InfomodeQuery);
      IntTooltip intTooltip1 = new IntTooltip();
      intTooltip1.path = (PathSegment) "waterCapacity";
      intTooltip1.label = LocalizedString.Id("SelectedInfoPanel.WATER_OUTPUT");
      intTooltip1.unit = "volume";
      // ISSUE: reference to a compiler-generated field
      this.m_WaterCapacity = intTooltip1;
      IntTooltip intTooltip2 = new IntTooltip();
      intTooltip2.path = (PathSegment) "waterUsage";
      intTooltip2.label = LocalizedString.Id("SelectedInfoPanel.WATER_PUMP_USAGE");
      intTooltip2.unit = "percentage";
      // ISSUE: reference to a compiler-generated field
      this.m_WaterUsage = intTooltip2;
      IntTooltip intTooltip3 = new IntTooltip();
      intTooltip3.path = (PathSegment) "sewageCapacity";
      intTooltip3.label = LocalizedString.Id("SelectedInfoPanel.SEWAGE_PROCESSING_CAPACITY");
      intTooltip3.unit = "volume";
      // ISSUE: reference to a compiler-generated field
      this.m_SewageCapacity = intTooltip3;
      IntTooltip intTooltip4 = new IntTooltip();
      intTooltip4.path = (PathSegment) "sewageUsage";
      intTooltip4.label = LocalizedString.Id("SelectedInfoPanel.SEWAGE_OUTLET_USAGE");
      intTooltip4.unit = "percentage";
      // ISSUE: reference to a compiler-generated field
      this.m_SewageUsage = intTooltip4;
      ProgressTooltip progressTooltip1 = new ProgressTooltip();
      progressTooltip1.path = (PathSegment) "waterConsumption";
      progressTooltip1.label = LocalizedString.Id("Tools.WATER_CONSUMPTION_LABEL");
      progressTooltip1.unit = "volume";
      progressTooltip1.color = TooltipColor.Warning;
      progressTooltip1.omitMax = true;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConsumption = progressTooltip1;
      ProgressTooltip progressTooltip2 = new ProgressTooltip();
      progressTooltip2.path = (PathSegment) "sewageConsumption";
      progressTooltip2.label = LocalizedString.Id("Tools.SEWAGE_CONSUMPTION_LABEL");
      progressTooltip2.unit = "volume";
      progressTooltip2.color = TooltipColor.Warning;
      progressTooltip2.omitMax = true;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageConsumption = progressTooltip2;
      IntTooltip intTooltip5 = new IntTooltip();
      intTooltip5.path = (PathSegment) "waterFlow";
      intTooltip5.label = LocalizedString.Id("Tools.WATER_FLOW_LABEL");
      intTooltip5.unit = "volume";
      // ISSUE: reference to a compiler-generated field
      this.m_WaterFlow = intTooltip5;
      IntTooltip intTooltip6 = new IntTooltip();
      intTooltip6.path = (PathSegment) "sewageFlow";
      intTooltip6.label = LocalizedString.Id("Tools.SEWAGE_FLOW_LABEL");
      intTooltip6.unit = "volume";
      // ISSUE: reference to a compiler-generated field
      this.m_SewageFlow = intTooltip6;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.CompleteDependency();
      RaycastResult result;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.IsInfomodeActivated() || this.m_ToolSystem.activeTool != this.m_DefaultTool || !this.m_ToolRaycastSystem.GetRaycastResult(out result) || this.EntityManager.HasComponent<Destroyed>(result.m_Owner))
        return;
      UtilityLaneData component1;
      EdgeMapping component2;
      if (this.EntityManager.HasComponent<Game.Net.UtilityLane>(result.m_Owner) && this.EntityManager.TryGetComponent<UtilityLaneData>(this.EntityManager.GetComponentData<PrefabRef>(result.m_Owner).m_Prefab, out component1) && this.EntityManager.TryGetComponent<EdgeMapping>(result.m_Owner, out component2) && (component1.m_UtilityTypes & (UtilityTypes.WaterPipe | UtilityTypes.SewagePipe)) != UtilityTypes.None)
      {
        if (component2.m_Parent1 != Entity.Null)
        {
          if (this.EntityManager.HasComponent<Edge>(component2.m_Parent1))
          {
            if (component2.m_Parent2 != Entity.Null)
            {
              if ((double) result.m_Hit.m_CurvePosition < 0.5)
              {
                float curvePosition = math.lerp(component2.m_CurveDelta1.x, component2.m_CurveDelta1.y, result.m_Hit.m_CurvePosition * 2f);
                // ISSUE: reference to a compiler-generated method
                this.AddEdgeFlow(component2.m_Parent1, curvePosition);
              }
              else
              {
                float curvePosition = math.lerp(component2.m_CurveDelta2.x, component2.m_CurveDelta2.y, (float) ((double) result.m_Hit.m_CurvePosition * 2.0 - 1.0));
                // ISSUE: reference to a compiler-generated method
                this.AddEdgeFlow(component2.m_Parent2, curvePosition);
              }
            }
            else
            {
              float curvePosition = math.lerp(component2.m_CurveDelta1.x, component2.m_CurveDelta1.y, result.m_Hit.m_CurvePosition);
              // ISSUE: reference to a compiler-generated method
              this.AddEdgeFlow(component2.m_Parent1, curvePosition);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddNodeFlow(component2.m_Parent1, component2.m_Parent2);
          }
        }
        else
        {
          Owner component3;
          if (this.EntityManager.HasComponent<Game.Net.SecondaryLane>(result.m_Owner) && this.EntityManager.TryGetComponent<Owner>(result.m_Owner, out component3))
            result.m_Owner = component3.m_Owner;
        }
      }
      Game.Buildings.SewageOutlet component4;
      if (this.EntityManager.TryGetComponent<Game.Buildings.SewageOutlet>(result.m_Owner, out component4))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SewageCapacity.value = component4.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_SewageCapacity);
        if (component4.m_Capacity > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SewageUsage.value = component4.m_LastProcessed > 0 ? math.clamp(100 * component4.m_LastProcessed / component4.m_Capacity, 1, 100) : 0;
          // ISSUE: reference to a compiler-generated field
          this.m_SewageUsage.color = TooltipColor.Info;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_SewageUsage);
        }
      }
      Game.Buildings.WaterPumpingStation component5;
      if (this.EntityManager.TryGetComponent<Game.Buildings.WaterPumpingStation>(result.m_Owner, out component5))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WaterCapacity.value = component5.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_WaterCapacity);
        if (component5.m_Capacity > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_WaterUsage.value = component5.m_LastProduction > 0 ? math.clamp(100 * component5.m_LastProduction / component5.m_Capacity, 1, 100) : 0;
          // ISSUE: reference to a compiler-generated field
          this.m_WaterUsage.color = TooltipColor.Info;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_WaterUsage);
        }
      }
      WaterConsumer component6;
      if (!this.EntityManager.TryGetComponent<WaterConsumer>(result.m_Owner, out component6))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConsumption.value = (float) component6.m_FulfilledFresh;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConsumption.max = (float) component6.m_WantedConsumption;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterConsumption.color = component6.m_FulfilledFresh < component6.m_WantedConsumption ? TooltipColor.Warning : TooltipColor.Info;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_WaterConsumption);
      if (component6.m_FulfilledFresh >= component6.m_WantedConsumption && component6.m_FulfilledSewage >= component6.m_WantedConsumption)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageConsumption.value = (float) component6.m_FulfilledSewage;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageConsumption.max = (float) component6.m_WantedConsumption;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageConsumption.color = component6.m_FulfilledSewage < component6.m_WantedConsumption ? TooltipColor.Warning : TooltipColor.Info;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_SewageConsumption);
    }

    private void AddEdgeFlow(Entity edge, float curvePosition)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WaterPipeNodeConnection> roComponentLookup1 = this.__TypeHandle.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<ConnectedFlowEdge> edgeRoBufferLookup = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WaterPipeEdge> roComponentLookup2 = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WaterConsumer> roComponentLookup3 = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Building> roComponentLookup4 = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WaterPipeBuildingConnection> roComponentLookup5 = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup;
      Edge component;
      WaterPipeNodeConnection componentData1;
      WaterPipeNodeConnection componentData2;
      WaterPipeEdge edge1;
      if (!this.EntityManager.TryGetComponent<Edge>(edge, out component) || !roComponentLookup1.TryGetComponent(edge, out componentData1) || !roComponentLookup1.TryGetComponent(component.m_Start, out componentData2) || !roComponentLookup1.TryGetComponent(component.m_End, out WaterPipeNodeConnection _) || !WaterPipeGraphUtils.TryGetFlowEdge(componentData2.m_WaterPipeNode, componentData1.m_WaterPipeNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge1))
        return;
      int2 int2_1 = math.max((int2) 1, edge1.capacity);
      int2 flow1 = edge1.flow;
      DynamicBuffer<ConnectedNode> buffer1;
      if (this.EntityManager.TryGetBuffer<ConnectedNode>(edge, true, out buffer1))
      {
        for (int index = 0; index < buffer1.Length; ++index)
        {
          ConnectedNode connectedNode = buffer1[index];
          WaterPipeNodeConnection componentData3;
          WaterPipeEdge edge2;
          if ((double) connectedNode.m_CurvePosition < (double) curvePosition && roComponentLookup1.TryGetComponent(connectedNode.m_Node, out componentData3) && WaterPipeGraphUtils.TryGetFlowEdge(componentData3.m_WaterPipeNode, componentData1.m_WaterPipeNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge2))
            flow1 += edge2.flow;
        }
      }
      WaterPipeEdge edge3;
      DynamicBuffer<ConnectedBuilding> buffer2;
      // ISSUE: reference to a compiler-generated field
      if (WaterPipeGraphUtils.TryGetFlowEdge(componentData1.m_WaterPipeNode, this.m_WaterPipeFlowSystem.sinkNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge3) && this.EntityManager.TryGetBuffer<ConnectedBuilding>(edge, true, out buffer2))
      {
        int2 flow2 = edge3.flow;
        int2 int2_2 = (int2) 0;
        for (int index = 0; index < buffer2.Length; ++index)
        {
          ConnectedBuilding connectedBuilding = buffer2[index];
          WaterConsumer componentData4;
          if (!roComponentLookup5.HasComponent(connectedBuilding.m_Building) && roComponentLookup3.TryGetComponent(connectedBuilding.m_Building, out componentData4))
            int2_2 += componentData4.m_WantedConsumption;
        }
        for (int index = 0; index < buffer2.Length; ++index)
        {
          ConnectedBuilding connectedBuilding = buffer2[index];
          WaterConsumer componentData5;
          if (!roComponentLookup5.HasComponent(connectedBuilding.m_Building) && roComponentLookup3.TryGetComponent(connectedBuilding.m_Building, out componentData5))
          {
            int2 int2_3 = new int2(FlowUtils.ConsumeFromTotal(componentData5.m_WantedConsumption, ref flow2.x, ref int2_2.x), FlowUtils.ConsumeFromTotal(componentData5.m_WantedConsumption, ref flow2.y, ref int2_2.y));
            if ((double) roComponentLookup4[connectedBuilding.m_Building].m_CurvePosition < (double) curvePosition)
              flow1 -= int2_3;
          }
        }
      }
      if (int2_1.x > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WaterFlow.value = math.abs(flow1.x);
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_WaterFlow);
      }
      if (int2_1.y <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageFlow.value = math.abs(flow1.y);
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_SewageFlow);
    }

    private void AddNodeFlow(Entity node, Entity edge)
    {
      WaterPipeNodeConnection component1;
      WaterPipeNodeConnection component2;
      DynamicBuffer<ConnectedFlowEdge> buffer;
      if (!this.EntityManager.TryGetComponent<WaterPipeNodeConnection>(node, out component1) || !this.EntityManager.TryGetComponent<WaterPipeNodeConnection>(edge, out component2) || !this.EntityManager.TryGetBuffer<ConnectedFlowEdge>(component1.m_WaterPipeNode, true, out buffer))
        return;
      int2 x1 = (int2) 0;
      int2 x2 = (int2) 0;
      for (int index = 0; index < buffer.Length; ++index)
      {
        WaterPipeEdge componentData = this.EntityManager.GetComponentData<WaterPipeEdge>(buffer[index].m_Edge);
        if (componentData.m_Start == component2.m_WaterPipeNode || componentData.m_End == component2.m_WaterPipeNode)
        {
          x1 = math.max(x1, math.abs(componentData.flow));
          x2 = math.max(x2, componentData.capacity);
        }
      }
      if (x2.x > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_WaterFlow.value = x1.x;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_WaterFlow);
      }
      if (x2.y <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SewageFlow.value = x1.y;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_SewageFlow);
    }

    private bool IsInfomodeActivated()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<InfoviewNetStatusData> componentDataArray = this.m_InfomodeQuery.ToComponentDataArray<InfoviewNetStatusData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        foreach (InfoviewNetStatusData infoviewNetStatusData in componentDataArray)
        {
          if (infoviewNetStatusData.m_Type == NetStatusType.PipeWaterFlow || infoviewNetStatusData.m_Type == NetStatusType.PipeSewageFlow)
            return true;
        }
      }
      return false;
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
    public RaycastWaterTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<WaterPipeNodeConnection> __Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeNodeConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RO_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<WaterPipeBuildingConnection>(true);
      }
    }
  }
}
