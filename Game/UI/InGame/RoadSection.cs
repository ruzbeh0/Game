// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.RoadSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Net;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class RoadSection : InfoSectionBase
  {
    private float[] m_Volume;
    private float[] m_Flow;

    protected override string group => nameof (RoadSection);

    private float length { get; set; }

    private float bestCondition { get; set; }

    private float worstCondition { get; set; }

    private float condition { get; set; }

    private float upkeep { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Volume = new float[5];
      // ISSUE: reference to a compiler-generated field
      this.m_Flow = new float[5];
    }

    protected override void Reset()
    {
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Volume[index] = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_Flow[index] = 0.0f;
      }
      this.length = 0.0f;
      this.bestCondition = 100f;
      this.worstCondition = 0.0f;
      this.condition = 0.0f;
      this.upkeep = 0.0f;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.visible = this.EntityManager.HasComponent<Aggregate>(this.selectedEntity) && this.EntityManager.HasComponent<AggregateElement>(this.selectedEntity);
    }

    protected override void OnProcess()
    {
      DynamicBuffer<AggregateElement> buffer = this.EntityManager.GetBuffer<AggregateElement>(this.selectedEntity, true);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity edge = buffer[index].m_Edge;
        Road component1;
        Curve component2;
        if (this.EntityManager.TryGetComponent<Road>(edge, out component1) && this.EntityManager.TryGetComponent<Curve>(edge, out component2))
        {
          this.length += component2.m_Length;
          float4 float4_1 = (component1.m_TrafficFlowDistance0 + component1.m_TrafficFlowDistance1) * 16f;
          float4 float4_2 = NetUtils.GetTrafficFlowSpeed(component1) * 100f;
          // ISSUE: reference to a compiler-generated field
          this.m_Volume[0] += (float) ((double) float4_1.x * 4.0 / 24.0);
          // ISSUE: reference to a compiler-generated field
          this.m_Volume[1] += (float) ((double) float4_1.y * 4.0 / 24.0);
          // ISSUE: reference to a compiler-generated field
          this.m_Volume[2] += (float) ((double) float4_1.z * 4.0 / 24.0);
          // ISSUE: reference to a compiler-generated field
          this.m_Volume[3] += (float) ((double) float4_1.w * 4.0 / 24.0);
          // ISSUE: reference to a compiler-generated field
          this.m_Flow[0] += float4_2.x;
          // ISSUE: reference to a compiler-generated field
          this.m_Flow[1] += float4_2.y;
          // ISSUE: reference to a compiler-generated field
          this.m_Flow[2] += float4_2.z;
          // ISSUE: reference to a compiler-generated field
          this.m_Flow[3] += float4_2.w;
        }
        NetCondition component3;
        if (this.EntityManager.TryGetComponent<NetCondition>(edge, out component3))
        {
          float2 wear = component3.m_Wear;
          if ((double) wear.x > (double) this.worstCondition)
            this.worstCondition = wear.x;
          if ((double) wear.y > (double) this.worstCondition)
            this.worstCondition = wear.y;
          if ((double) wear.x < (double) this.bestCondition)
            this.bestCondition = wear.x;
          if ((double) wear.y < (double) this.bestCondition)
            this.bestCondition = wear.y;
          this.condition += math.csum(wear) * 0.5f;
        }
        PrefabRef component4;
        PlaceableNetData component5;
        if (this.EntityManager.TryGetComponent<PrefabRef>(edge, out component4) && this.EntityManager.TryGetComponent<PlaceableNetData>(component4.m_Prefab, out component5))
          this.upkeep += component5.m_DefaultUpkeepCost;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Volume[0] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Volume[1] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Volume[2] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Volume[3] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Volume[4] = this.m_Volume[0];
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[0] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[1] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[2] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[3] /= (float) buffer.Length;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Flow[4] = this.m_Flow[0];
      this.bestCondition = (float) (100.0 - (double) this.bestCondition / 10.0 * 100.0);
      this.worstCondition = (float) (100.0 - (double) this.worstCondition / 10.0 * 100.0);
      this.condition = (float) ((double) this.condition / 10.0 * 100.0);
      this.condition = (float) (100.0 - (double) this.condition / (double) buffer.Length);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("volumeData");
      // ISSUE: reference to a compiler-generated field
      writer.ArrayBegin(this.m_Volume.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Volume.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Volume[index]);
      }
      writer.ArrayEnd();
      writer.PropertyName("flowData");
      // ISSUE: reference to a compiler-generated field
      writer.ArrayBegin(this.m_Flow.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_Flow.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Flow[index]);
      }
      writer.ArrayEnd();
      writer.PropertyName("length");
      writer.Write(this.length);
      writer.PropertyName("bestCondition");
      writer.Write(this.bestCondition);
      writer.PropertyName("worstCondition");
      writer.Write(this.worstCondition);
      writer.PropertyName("condition");
      writer.Write(this.condition);
      writer.PropertyName("upkeep");
      writer.Write(this.upkeep);
    }

    [Preserve]
    public RoadSection()
    {
    }
  }
}
