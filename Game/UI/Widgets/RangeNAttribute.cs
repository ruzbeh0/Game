// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.RangeNAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class RangeNAttribute : PropertyAttribute
  {
    public float4 min { get; private set; }

    public float4 max { get; private set; }

    public RangeNAttribute(float min, float max, bool componentExpansion = true)
    {
      if (componentExpansion)
      {
        this.min = (float4) min;
        this.max = (float4) max;
      }
      else
      {
        this.min = new float4(min, float3.zero);
        this.max = new float4(max, float3.zero);
      }
    }

    public RangeNAttribute(float2 min, float2 max)
    {
      this.min = new float4(min, float2.zero);
      this.max = new float4(max, float2.zero);
    }

    public RangeNAttribute(float3 min, float3 max)
    {
      this.min = new float4(min, 0.0f);
      this.max = new float4(max, 0.0f);
    }

    public RangeNAttribute(float4 min, float4 max)
    {
      this.min = min;
      this.max = max;
    }
  }
}
