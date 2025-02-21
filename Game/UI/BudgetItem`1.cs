// Decompiled with JetBrains decompiler
// Type: Game.UI.BudgetItem`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine;

#nullable disable
namespace Game.UI
{
  [Serializable]
  public class BudgetItem<T> where T : struct, Enum
  {
    public string m_ID;
    public Color m_Color = Color.gray;
    public string m_Icon;
    public T[] m_Sources;
  }
}
