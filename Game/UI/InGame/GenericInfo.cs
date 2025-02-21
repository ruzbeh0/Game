// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GenericInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public class GenericInfo : ISubsectionSource, IJsonWritable
  {
    private readonly Func<Entity, Entity, bool> m_ShouldDisplay;
    private readonly Action<Entity, Entity, GenericInfo> m_OnUpdate;

    public string label { get; set; }

    public string value { get; set; }

    public Entity target { get; set; }

    public GenericInfo(
      Func<Entity, Entity, bool> shouldDisplay,
      Action<Entity, Entity, GenericInfo> onUpdate)
    {
      this.m_ShouldDisplay = shouldDisplay;
      this.m_OnUpdate = onUpdate;
    }

    public bool DisplayFor(Entity entity, Entity prefab) => this.m_ShouldDisplay(entity, prefab);

    public void OnRequestUpdate(Entity entity, Entity prefab)
    {
      this.m_OnUpdate(entity, prefab, this);
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("label");
      writer.Write(this.label ?? string.Empty);
      writer.PropertyName("value");
      writer.Write(this.value ?? string.Empty);
      writer.PropertyName("target");
      if (this.target == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.target);
      writer.TypeEnd();
    }
  }
}
