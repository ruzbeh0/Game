// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.Dependencies.VsWhereResult
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;

#nullable disable
namespace Game.Modding.Toolchain.Dependencies
{
  internal class VsWhereResult
  {
    public VsWhereEntry[] entries;

    public static VsWhereResult FromJson(string json)
    {
      return JSON.MakeInto<VsWhereResult>(JSON.Load(json));
    }
  }
}
