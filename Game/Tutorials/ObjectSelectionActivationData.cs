// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.ObjectSelectionActivationData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Tutorials
{
  public struct ObjectSelectionActivationData : IBufferElementData
  {
    public Entity m_Prefab;
    public bool m_AllowTool;

    public ObjectSelectionActivationData(Entity prefab, bool allowTool)
    {
      this.m_Prefab = prefab;
      this.m_AllowTool = allowTool;
    }
  }
}
