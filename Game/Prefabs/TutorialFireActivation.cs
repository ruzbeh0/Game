// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialFireActivation
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tutorials;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tutorials/Activation/", new Type[] {typeof (TutorialPrefab)})]
  public class TutorialFireActivation : TutorialActivation
  {
    [EnumFlag]
    public TutorialFireActivation.FireActivationTarget m_Target = TutorialFireActivation.FireActivationTarget.Building | TutorialFireActivation.FireActivationTarget.Forest;

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      if ((this.m_Target & TutorialFireActivation.FireActivationTarget.Building) != (TutorialFireActivation.FireActivationTarget) 0)
        components.Add(ComponentType.ReadWrite<BuildingFireActivationData>());
      if ((this.m_Target & TutorialFireActivation.FireActivationTarget.Forest) == (TutorialFireActivation.FireActivationTarget) 0)
        return;
      components.Add(ComponentType.ReadWrite<ForestFireActivationData>());
    }

    public enum FireActivationTarget
    {
      Building = 1,
      Forest = 2,
    }
  }
}
