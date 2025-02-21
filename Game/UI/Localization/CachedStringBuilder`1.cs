// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.CachedStringBuilder`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.Localization
{
  public class CachedStringBuilder<T>
  {
    private readonly Func<T, string> m_Builder;
    private readonly Dictionary<T, string> m_Cache;

    public CachedStringBuilder(Func<T, string> builder)
    {
      this.m_Builder = builder;
      this.m_Cache = new Dictionary<T, string>();
    }

    public string this[T key]
    {
      get
      {
        string str;
        return this.m_Cache.TryGetValue(key, out str) ? str : (this.m_Cache[key] = this.m_Builder(key));
      }
    }
  }
}
