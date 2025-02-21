// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialInputTriggerPrefab
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
  [ComponentMenu("Tutorials/Triggers/", new Type[] {})]
  public class TutorialInputTriggerPrefab : TutorialTriggerPrefabBase
  {
    public TutorialInputTriggerPrefab.InputAction[] m_Actions;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InputTriggerData>());
    }

    [Serializable]
    public struct InputAction
    {
      public string m_Map;
      public string m_Action;
    }
  }
}
