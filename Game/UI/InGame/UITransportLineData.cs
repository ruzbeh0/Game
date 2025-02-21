// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UITransportLineData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using System;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct UITransportLineData : IJsonWritable, IComparable<UITransportLineData>
  {
    public Entity entity { get; }

    public bool active { get; }

    public bool visible { get; }

    public bool isCargo { get; }

    public Color32 color { get; }

    public int schedule { get; }

    public TransportType type { get; }

    public float length { get; }

    public int stops { get; }

    public int vehicles { get; }

    public int cargo { get; }

    public float usage { get; }

    public UITransportLineData(
      Entity entity,
      bool active,
      bool visible,
      bool isCargo,
      Game.Routes.Color color,
      RouteSchedule schedule,
      TransportType type,
      float length,
      int stops,
      int vehicles,
      int cargo,
      float usage)
    {
      this.entity = entity;
      this.active = active;
      this.visible = visible;
      this.isCargo = isCargo;
      this.color = color.m_Color;
      this.schedule = (int) schedule;
      this.type = type;
      this.length = length;
      this.stops = stops;
      this.vehicles = vehicles;
      this.cargo = cargo;
      this.usage = usage;
    }

    public int CompareTo(UITransportLineData other)
    {
      int num = this.type.CompareTo((object) other.type);
      return num == 0 ? this.entity.Index.CompareTo(other.entity.Index) : num;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("entity");
      writer.Write(this.entity);
      writer.PropertyName("active");
      writer.Write(this.active);
      writer.PropertyName("visible");
      writer.Write(this.visible);
      writer.PropertyName("isCargo");
      writer.Write(this.isCargo);
      writer.PropertyName("color");
      writer.Write(this.color);
      writer.PropertyName("schedule");
      writer.Write(this.schedule);
      writer.PropertyName("type");
      writer.Write(Enum.GetName(typeof (TransportType), (object) this.type));
      writer.PropertyName("length");
      writer.Write(this.length);
      writer.PropertyName("stops");
      writer.Write(this.stops);
      writer.PropertyName("vehicles");
      writer.Write(this.vehicles);
      writer.PropertyName("cargo");
      writer.Write(this.cargo);
      writer.PropertyName("usage");
      writer.Write(this.usage);
      writer.TypeEnd();
    }
  }
}
