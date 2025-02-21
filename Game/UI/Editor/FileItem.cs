// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.FileItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.UI.Editor
{
  public class FileItem : IItemPicker.Item, IComparable<FileItem>
  {
    public string path { get; set; }

    public int CompareTo(FileItem other) => string.CompareOrdinal(this.path, other.path);
  }
}
