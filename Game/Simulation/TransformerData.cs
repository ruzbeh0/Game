// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TransformerData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct TransformerData
  {
    [ReadOnly]
    public ComponentLookup<Deleted> m_Deleted;
    [ReadOnly]
    public ComponentLookup<PrefabRef> m_PrefabRefs;
    [ReadOnly]
    public ComponentLookup<ElectricityConnectionData> m_ElectricityConnectionDatas;
    [ReadOnly]
    public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
    [ReadOnly]
    public BufferLookup<Game.Net.SubNet> m_SubNets;
    [ReadOnly]
    public ComponentLookup<Node> m_NetNodes;
    [ReadOnly]
    public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
    [ReadOnly]
    public ComponentLookup<ElectricityValveConnection> m_ElectricityValveConnections;
    [ReadOnly]
    public BufferLookup<ConnectedFlowEdge> m_FlowConnections;
    [ReadOnly]
    public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;

    public void GetTransformerData(Entity entity, out int capacity, out int flow)
    {
      int lowVoltageCapacity = 0;
      int highVoltageCapacity = 0;
      flow = 0;
      DynamicBuffer<Game.Net.SubNet> bufferData1;
      if (this.m_SubNets.TryGetBuffer(entity, out bufferData1))
        this.ProcessMarkerNodes(bufferData1, ref lowVoltageCapacity, ref highVoltageCapacity, ref flow);
      DynamicBuffer<InstalledUpgrade> bufferData2;
      if (this.m_InstalledUpgrades.TryGetBuffer(entity, out bufferData2))
        this.ProcessMarkerNodes(bufferData2, ref lowVoltageCapacity, ref highVoltageCapacity, ref flow);
      capacity = math.min(lowVoltageCapacity, highVoltageCapacity);
    }

    private void ProcessMarkerNodes(
      DynamicBuffer<InstalledUpgrade> upgrades,
      ref int lowVoltageCapacity,
      ref int highVoltageCapacity,
      ref int flow)
    {
      for (int index = 0; index < upgrades.Length; ++index)
      {
        InstalledUpgrade upgrade = upgrades[index];
        DynamicBuffer<Game.Net.SubNet> bufferData;
        if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && this.m_SubNets.TryGetBuffer(upgrade.m_Upgrade, out bufferData))
          this.ProcessMarkerNodes(bufferData, ref lowVoltageCapacity, ref highVoltageCapacity, ref flow);
      }
    }

    private void ProcessMarkerNodes(
      DynamicBuffer<Game.Net.SubNet> subNets,
      ref int lowVoltageCapacity,
      ref int highVoltageCapacity,
      ref int flow)
    {
      for (int index = 0; index < subNets.Length; ++index)
      {
        Entity subNet = subNets[index].m_SubNet;
        ElectricityNodeConnection componentData1;
        ElectricityValveConnection componentData2;
        PrefabRef componentData3;
        ElectricityConnectionData componentData4;
        if (this.m_NetNodes.HasComponent(subNet) && !this.m_Deleted.HasComponent(subNet) && this.m_ElectricityNodeConnections.TryGetComponent(subNet, out componentData1) && this.m_ElectricityValveConnections.TryGetComponent(subNet, out componentData2) && this.m_PrefabRefs.TryGetComponent(subNet, out componentData3) && this.m_ElectricityConnectionDatas.TryGetComponent(componentData3.m_Prefab, out componentData4))
        {
          if (componentData4.m_Voltage == Game.Prefabs.ElectricityConnection.Voltage.Low)
          {
            lowVoltageCapacity += componentData4.m_Capacity;
            ElectricityFlowEdge edge;
            if (ElectricityGraphUtils.TryGetFlowEdge(componentData2.m_ValveNode, componentData1.m_ElectricityNode, ref this.m_FlowConnections, ref this.m_FlowEdges, out edge))
              flow += edge.m_Flow;
          }
          else
            highVoltageCapacity += componentData4.m_Capacity;
        }
      }
    }
  }
}
