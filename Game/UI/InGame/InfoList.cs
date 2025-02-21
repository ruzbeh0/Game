// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.InfoList
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public class InfoList : ISubsectionSource, IJsonWritable
  {
    private readonly Func<Entity, Entity, bool> m_ShouldDisplay;
    private readonly Action<Entity, Entity, InfoList> m_OnUpdate;

    public string label { get; set; }

    private List<InfoList.Item> list { get; set; }

    private bool expanded { get; set; }

    public InfoList(
      Func<Entity, Entity, bool> shouldDisplay,
      Action<Entity, Entity, InfoList> onUpdate)
    {
      this.list = new List<InfoList.Item>();
      this.m_ShouldDisplay = shouldDisplay;
      this.m_OnUpdate = onUpdate;
    }

    public bool DisplayFor(Entity entity, Entity prefab) => this.m_ShouldDisplay(entity, prefab);

    public void OnRequestUpdate(Entity entity, Entity prefab)
    {
      this.list.Clear();
      this.m_OnUpdate(entity, prefab, this);
    }

    public void Add(InfoList.Item item) => this.list.Add(item);

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("expanded");
      writer.Write(this.expanded);
      writer.PropertyName("label");
      writer.Write(this.label);
      writer.PropertyName("list");
      writer.ArrayBegin(this.list.Count);
      for (int index = 0; index < this.list.Count; ++index)
        writer.Write<InfoList.Item>(this.list[index]);
      writer.ArrayEnd();
      writer.TypeEnd();
    }

    public readonly struct Item : IJsonWritable
    {
      public static readonly Entity kNullEntity = Entity.Null;

      public string text { get; }

      public Entity entity { get; }

      public Item(string text, Entity entity)
      {
        this.text = text;
        this.entity = entity;
      }

      public Item(string text)
        : this(text, InfoList.Item.kNullEntity)
      {
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("text");
        writer.Write(this.text);
        writer.PropertyName("entity");
        if (this.entity == Entity.Null)
          writer.WriteNull();
        else
          writer.Write(this.entity);
        writer.TypeEnd();
      }
    }
  }
}
