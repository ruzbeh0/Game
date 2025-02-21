// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UITransportType
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct UITransportType
  {
    private readonly Entity m_Prefab;

    public string id { get; }

    public string icon { get; }

    public bool locked { get; }

    public UITransportType(Entity prefab, string id, string icon, bool locked)
    {
      this.m_Prefab = prefab;
      this.id = id;
      this.icon = icon;
      this.locked = locked;
    }

    public void Write(PrefabUISystem prefabUISystem, IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("id");
      writer.Write(this.id);
      writer.PropertyName("icon");
      writer.Write(this.icon);
      writer.PropertyName("locked");
      writer.Write(this.locked);
      writer.PropertyName("requirements");
      // ISSUE: reference to a compiler-generated method
      prefabUISystem.BindPrefabRequirements(writer, this.m_Prefab);
      writer.TypeEnd();
    }
  }
}
