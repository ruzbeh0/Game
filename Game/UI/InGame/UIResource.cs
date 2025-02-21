// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UIResource
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Economy;
using System;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct UIResource : IJsonWritable, IComparable<UIResource>
  {
    public Resource key { get; }

    public int amount { get; }

    public UIResource(Game.Economy.Resources resource)
    {
      this.key = resource.m_Resource;
      this.amount = resource.m_Amount;
    }

    public UIResource(Resource resource, int amount)
    {
      this.key = resource;
      this.amount = amount;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("key");
      writer.Write(Enum.GetName(typeof (Resource), (object) this.key));
      writer.PropertyName("amount");
      writer.Write(this.amount);
      writer.TypeEnd();
    }

    public int CompareTo(UIResource other) => other.amount.CompareTo(this.amount);
  }
}
