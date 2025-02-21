// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ContentPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Content/", new Type[] {})]
  public class ContentPrefab : PrefabBase
  {
    public bool IsAvailable()
    {
      foreach (ComponentBase component in this.components)
      {
        if (component is ContentRequirementBase contentRequirementBase && !contentRequirementBase.CheckRequirement())
          return false;
      }
      return true;
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ContentData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      ContentData componentData = entityManager.GetComponentData<ContentData>(entity);
      DlcRequirement component;
      if (this.TryGet<DlcRequirement>(out component))
      {
        componentData.m_Flags |= ContentFlags.RequireDlc;
        componentData.m_DlcID = component.m_Dlc.id;
      }
      if (this.Has<PdxLoginRequirement>())
        componentData.m_Flags |= ContentFlags.RequirePdxLogin;
      entityManager.SetComponentData<ContentData>(entity, componentData);
    }
  }
}
