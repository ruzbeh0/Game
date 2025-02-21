// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.RaycastElectricityTooltipSystem
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
  public class RaycastElectricityTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private ElectricityFlowSystem m_ElectricityFlowSystem;
    private EntityQuery m_InfomodeQuery;
    private IntTooltip m_Production;
    private IntTooltip m_TransformerCapacity;
    private IntTooltip m_Usage;
    private IntTooltip m_BatteryFlow;
    private IntTooltip m_BatteryCharge;
    private ProgressTooltip m_Consumption;
    private ProgressTooltip m_Flow;
    private RaycastElectricityTooltipSystem.TypeHandle __TypeHandle;

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
      this.m_ElectricityFlowSystem = this.World.GetOrCreateSystemManaged<ElectricityFlowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewNetStatusData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InfomodeQuery);
      IntTooltip intTooltip1 = new IntTooltip();
      intTooltip1.path = (PathSegment) "electricityProduction";
      intTooltip1.label = LocalizedString.Id("Tools.ELECTRICITY_PRODUCTION_LABEL");
      intTooltip1.unit = "power";
      // ISSUE: reference to a compiler-generated field
      this.m_Production = intTooltip1;
      IntTooltip intTooltip2 = new IntTooltip();
      intTooltip2.path = (PathSegment) "transformerCapacity";
      intTooltip2.label = LocalizedString.Id("SelectedInfoPanel.ELECTRICITY_TRANSFORMER_CAPACITY");
      intTooltip2.unit = "power";
      // ISSUE: reference to a compiler-generated field
      this.m_TransformerCapacity = intTooltip2;
      IntTooltip intTooltip3 = new IntTooltip();
      intTooltip3.path = (PathSegment) "electricityUsage";
      intTooltip3.label = LocalizedString.Id("SelectedInfoPanel.ELECTRICITY_POWER_USAGE");
      intTooltip3.unit = "percentage";
      // ISSUE: reference to a compiler-generated field
      this.m_Usage = intTooltip3;
      IntTooltip intTooltip4 = new IntTooltip();
      intTooltip4.path = (PathSegment) "batteryFlow";
      intTooltip4.label = LocalizedString.Id("Tools.BATTERY_FLOW");
      intTooltip4.unit = "power";
      intTooltip4.signed = true;
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryFlow = intTooltip4;
      IntTooltip intTooltip5 = new IntTooltip();
      intTooltip5.path = (PathSegment) "batteryCharge";
      intTooltip5.label = LocalizedString.Id("Tools.BATTERY_CHARGE");
      intTooltip5.unit = "percentage";
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryCharge = intTooltip5;
      ProgressTooltip progressTooltip1 = new ProgressTooltip();
      progressTooltip1.path = (PathSegment) "cElectricityConsumption";
      progressTooltip1.label = LocalizedString.Id("Tools.ELECTRICITY_CONSUMPTION_LABEL");
      progressTooltip1.unit = "power";
      progressTooltip1.color = TooltipColor.Warning;
      progressTooltip1.omitMax = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption = progressTooltip1;
      ProgressTooltip progressTooltip2 = new ProgressTooltip();
      progressTooltip2.path = (PathSegment) "electricityFlow";
      progressTooltip2.label = LocalizedString.Id("Tools.ELECTRICITY_FLOW_LABEL");
      progressTooltip2.unit = "power";
      // ISSUE: reference to a compiler-generated field
      this.m_Flow = progressTooltip2;
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
      if (this.EntityManager.HasComponent<Game.Net.UtilityLane>(result.m_Owner) && this.EntityManager.TryGetComponent<UtilityLaneData>(this.EntityManager.GetComponentData<PrefabRef>(result.m_Owner).m_Prefab, out component1) && this.EntityManager.TryGetComponent<EdgeMapping>(result.m_Owner, out component2) && (component1.m_UtilityTypes & (UtilityTypes.LowVoltageLine | UtilityTypes.HighVoltageLine)) != UtilityTypes.None)
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
      ElectricityProducer component4;
      if (this.EntityManager.TryGetComponent<ElectricityProducer>(result.m_Owner, out component4))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Production.value = component4.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_Production);
        if (component4.m_Capacity > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Usage.value = component4.m_LastProduction > 0 ? math.clamp(100 * component4.m_LastProduction / component4.m_Capacity, 1, 100) : 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Usage.color = this.HasBottleneck(result.m_Owner) ? TooltipColor.Warning : TooltipColor.Info;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Usage);
        }
      }
      else if (this.EntityManager.HasComponent<Game.Buildings.Transformer>(result.m_Owner))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        int capacity;
        int flow;
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
        new Game.Simulation.TransformerData()
        {
          m_Deleted = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
          m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_ElectricityConnectionDatas = this.__TypeHandle.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup,
          m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
          m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
          m_NetNodes = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
          m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
          m_ElectricityValveConnections = this.__TypeHandle.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup,
          m_FlowConnections = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
          m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup
        }.GetTransformerData(result.m_Owner, out capacity, out flow);
        // ISSUE: reference to a compiler-generated field
        this.m_TransformerCapacity.value = capacity;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_TransformerCapacity);
        if (capacity > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Usage.value = flow != 0 ? math.clamp(100 * math.abs(flow) / capacity, 1, 100) : 0;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_Usage.color = this.HasBottleneck(result.m_Owner) ? TooltipColor.Warning : TooltipColor.Info;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Usage);
        }
      }
      Game.Buildings.Battery component5;
      if (this.EntityManager.TryGetComponent<Game.Buildings.Battery>(result.m_Owner, out component5) && component5.m_Capacity > 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BatteryFlow.value = component5.m_LastFlow;
        // ISSUE: reference to a compiler-generated field
        this.m_BatteryCharge.value = 100 * component5.storedEnergyHours / component5.m_Capacity;
        // ISSUE: reference to a compiler-generated field
        this.m_BatteryCharge.color = component5.m_StoredEnergy > 0L ? TooltipColor.Info : TooltipColor.Warning;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_BatteryFlow);
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_BatteryCharge);
      }
      ElectricityConsumer component6;
      if (!this.EntityManager.TryGetComponent<ElectricityConsumer>(result.m_Owner, out component6))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.value = (float) component6.m_FulfilledConsumption;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.max = (float) component6.m_WantedConsumption;
      // ISSUE: reference to a compiler-generated field
      this.m_Consumption.color = component6.m_FulfilledConsumption < component6.m_WantedConsumption ? TooltipColor.Warning : TooltipColor.Info;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Consumption);
    }

    private void AddEdgeFlow(Entity edge, float curvePosition)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ElectricityNodeConnection> roComponentLookup1 = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<ConnectedFlowEdge> edgeRoBufferLookup = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ElectricityFlowEdge> roComponentLookup2 = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ElectricityConsumer> roComponentLookup3 = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Building> roComponentLookup4 = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ElectricityBuildingConnection> roComponentLookup5 = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup;
      Edge component;
      ElectricityNodeConnection componentData1;
      ElectricityNodeConnection componentData2;
      ElectricityFlowEdge edge1;
      if (!this.EntityManager.TryGetComponent<Edge>(edge, out component) || !roComponentLookup1.TryGetComponent(edge, out componentData1) || !roComponentLookup1.TryGetComponent(component.m_Start, out componentData2) || !roComponentLookup1.TryGetComponent(component.m_End, out ElectricityNodeConnection _) || !ElectricityGraphUtils.TryGetFlowEdge(componentData2.m_ElectricityNode, componentData1.m_ElectricityNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge1))
        return;
      int num1 = math.max(1, edge1.m_Capacity);
      int flow1 = edge1.m_Flow;
      DynamicBuffer<ConnectedNode> buffer1;
      if (this.EntityManager.TryGetBuffer<ConnectedNode>(edge, true, out buffer1))
      {
        for (int index = 0; index < buffer1.Length; ++index)
        {
          ConnectedNode connectedNode = buffer1[index];
          ElectricityNodeConnection componentData3;
          ElectricityFlowEdge edge2;
          if ((double) connectedNode.m_CurvePosition < (double) curvePosition && roComponentLookup1.TryGetComponent(connectedNode.m_Node, out componentData3) && ElectricityGraphUtils.TryGetFlowEdge(componentData3.m_ElectricityNode, componentData1.m_ElectricityNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge2))
            flow1 += edge2.m_Flow;
        }
      }
      ElectricityFlowEdge edge3;
      DynamicBuffer<ConnectedBuilding> buffer2;
      // ISSUE: reference to a compiler-generated field
      if (ElectricityGraphUtils.TryGetFlowEdge(componentData1.m_ElectricityNode, this.m_ElectricityFlowSystem.sinkNode, ref edgeRoBufferLookup, ref roComponentLookup2, out edge3) && this.EntityManager.TryGetBuffer<ConnectedBuilding>(edge, true, out buffer2))
      {
        int flow2 = edge3.m_Flow;
        int totalDemand = 0;
        for (int index = 0; index < buffer2.Length; ++index)
        {
          ConnectedBuilding connectedBuilding = buffer2[index];
          ElectricityConsumer componentData4;
          if (!roComponentLookup5.HasComponent(connectedBuilding.m_Building) && roComponentLookup3.TryGetComponent(connectedBuilding.m_Building, out componentData4))
            totalDemand += componentData4.m_WantedConsumption;
        }
        for (int index = 0; index < buffer2.Length; ++index)
        {
          ConnectedBuilding connectedBuilding = buffer2[index];
          ElectricityConsumer componentData5;
          if (!roComponentLookup5.HasComponent(connectedBuilding.m_Building) && roComponentLookup3.TryGetComponent(connectedBuilding.m_Building, out componentData5))
          {
            int num2 = FlowUtils.ConsumeFromTotal(componentData5.m_WantedConsumption, ref flow2, ref totalDemand);
            if ((double) roComponentLookup4[connectedBuilding.m_Building].m_CurvePosition < (double) curvePosition)
              flow1 -= num2;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Flow.value = (float) math.abs(flow1);
      // ISSUE: reference to a compiler-generated field
      this.m_Flow.max = (float) num1;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Flow);
    }

    private void AddNodeFlow(Entity node, Entity edge)
    {
      ElectricityNodeConnection component1;
      ElectricityNodeConnection component2;
      DynamicBuffer<ConnectedFlowEdge> buffer;
      if (!this.EntityManager.TryGetComponent<ElectricityNodeConnection>(node, out component1) || !this.EntityManager.TryGetComponent<ElectricityNodeConnection>(edge, out component2) || !this.EntityManager.TryGetBuffer<ConnectedFlowEdge>(component1.m_ElectricityNode, true, out buffer))
        return;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < buffer.Length; ++index)
      {
        ElectricityFlowEdge componentData = this.EntityManager.GetComponentData<ElectricityFlowEdge>(buffer[index].m_Edge);
        if (componentData.m_Start == component2.m_ElectricityNode || componentData.m_End == component2.m_ElectricityNode)
        {
          int num3 = math.abs(componentData.m_Flow);
          if (num3 > num1 || num3 == num1 && componentData.m_Capacity > num2)
          {
            num1 = num3;
            num2 = componentData.m_Capacity;
          }
        }
      }
      if (num2 <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Flow.value = (float) num1;
      // ISSUE: reference to a compiler-generated field
      this.m_Flow.max = (float) num2;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Flow);
    }

    private bool IsInfomodeActivated()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<InfoviewNetStatusData> componentDataArray = this.m_InfomodeQuery.ToComponentDataArray<InfoviewNetStatusData>((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        foreach (InfoviewNetStatusData infoviewNetStatusData in componentDataArray)
        {
          if (infoviewNetStatusData.m_Type == NetStatusType.LowVoltageFlow || infoviewNetStatusData.m_Type == NetStatusType.HighVoltageFlow)
            return true;
        }
      }
      return false;
    }

    private bool HasBottleneck(Entity building)
    {
      ElectricityBuildingConnection component;
      if (this.EntityManager.TryGetComponent<ElectricityBuildingConnection>(building, out component))
      {
        if (component.m_ProducerEdge != Entity.Null && this.EntityManager.GetComponentData<ElectricityFlowEdge>(component.m_ProducerEdge).isBottleneck)
          return true;
        if (component.m_TransformerNode != Entity.Null)
        {
          foreach (ConnectedFlowEdge connectedFlowEdge in this.EntityManager.GetBuffer<ConnectedFlowEdge>(component.m_TransformerNode, true))
          {
            if (this.EntityManager.GetComponentData<ElectricityFlowEdge>(connectedFlowEdge.m_Edge).isBottleneck)
              return true;
          }
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
    public RaycastElectricityTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConnectionData> __Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityValveConnection> __Game_Simulation_ElectricityValveConnection_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ElectricityConnectionData_RO_ComponentLookup = state.GetComponentLookup<ElectricityConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityValveConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityValveConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityBuildingConnection>(true);
      }
    }
  }
}
