// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.IEditorTool
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Editor
{
  public interface IEditorTool : IJsonWritable
  {
    string id { get; }

    string icon { get; }

    string uiTag { get; }

    bool disabled { get; }

    string shortcut { get; }

    bool active { get; set; }

    void IJsonWritable.Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (IEditorTool).FullName);
      writer.PropertyName("id");
      writer.Write(this.id);
      writer.PropertyName("icon");
      writer.Write(this.icon);
      writer.PropertyName("uiTag");
      writer.Write(this.uiTag);
      writer.PropertyName("shortcut");
      writer.Write(this.shortcut);
      writer.PropertyName("disabled");
      writer.Write(this.disabled);
      writer.TypeEnd();
    }
  }
}
