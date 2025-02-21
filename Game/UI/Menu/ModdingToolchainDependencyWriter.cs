// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.ModdingToolchainDependencyWriter
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Modding.Toolchain;
using Game.UI.Localization;
using System;

#nullable disable
namespace Game.UI.Menu
{
  public class ModdingToolchainDependencyWriter : IWriter<IToolchainDependency>
  {
    public void Write(IJsonWriter writer, IToolchainDependency value)
    {
      if (value != null)
      {
        writer.TypeBegin(value.GetType().FullName);
        writer.PropertyName("name");
        writer.Write<LocalizedString>(value.localizedName);
        writer.PropertyName("state");
        writer.Write((int) value.state.m_State);
        writer.PropertyName("progress");
        writer.Write(value.state.m_Progress ?? -1);
        writer.PropertyName("details");
        writer.Write<LocalizedString>(value.GetLocalizedState(false));
        writer.PropertyName("version");
        writer.Write<LocalizedString>(value.GetLocalizedVersion());
        writer.PropertyName("icon");
        writer.Write(value.icon);
        writer.TypeEnd();
      }
      else
      {
        writer.WriteNull();
        throw new ArgumentNullException(nameof (value), "Null passed to non-nullable value writer");
      }
    }
  }
}
