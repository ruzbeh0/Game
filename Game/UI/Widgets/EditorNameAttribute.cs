// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.EditorNameAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public class EditorNameAttribute : PropertyAttribute
  {
    public string displayName { get; private set; }

    public EditorNameAttribute(string displayName) => this.displayName = displayName;
  }
}
