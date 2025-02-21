// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.ControlSchemeActivationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Input;
using Unity.Entities;

#nullable disable
namespace Game.Tutorials
{
  public struct ControlSchemeActivationData : IComponentData, IQueryTypeParameter
  {
    public InputManager.ControlScheme m_ControlScheme;

    public ControlSchemeActivationData(InputManager.ControlScheme controlScheme)
    {
      this.m_ControlScheme = controlScheme;
    }
  }
}
