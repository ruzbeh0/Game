// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIProductionLinks
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("UI/", new System.Type[] {typeof (ResourcePrefab)})]
  public class UIProductionLinks : ComponentBase
  {
    public UIProductionLinkPrefab m_Producer;
    [CanBeNull]
    public UIProductionLinkPrefab[] m_FinalConsumers;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((UnityEngine.Object) this.m_Producer != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_Producer);
      if (this.m_FinalConsumers == null)
        return;
      for (int index = 0; index < this.m_FinalConsumers.Length; ++index)
      {
        UIProductionLinkPrefab finalConsumer = this.m_FinalConsumers[index];
        if ((UnityEngine.Object) finalConsumer != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) finalConsumer);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UIProductionLinksData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }
  }
}
