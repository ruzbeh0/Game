// Decompiled with JetBrains decompiler
// Type: Game.Input.BaseMaskProcessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public abstract class BaseMaskProcessor<TValue> : InputProcessor<TValue> where TValue : struct
  {
    public InputManager.DeviceType m_Mask;
    public int m_Index;
    private ProxyAction m_Action;

    public override TValue Process(TValue value, InputControl control)
    {
      if (this.m_Index == -1)
        return value;
      if ((this.m_Action == null || this.m_Index != this.m_Action.m_GlobalIndex) && !InputManager.instance.TryFindAction(this.m_Index, out this.m_Action))
      {
        this.m_Index = -1;
        return value;
      }
      return (this.m_Action.mask & this.m_Mask) != InputManager.DeviceType.None ? value : default (TValue);
    }
  }
}
