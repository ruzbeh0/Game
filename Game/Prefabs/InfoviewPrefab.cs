// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfoviewPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tools/", new System.Type[] {})]
  public class InfoviewPrefab : PrefabBase
  {
    public InfomodeInfo[] m_Infomodes;
    public Color m_DefaultColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public Color m_SecondaryColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    [FormerlySerializedAs("m_IconName")]
    public string m_IconPath;
    public int m_Priority;
    public int m_Group;
    public IconCategory[] m_WarningCategories;
    public bool m_Editor;

    public bool isValid { get; private set; }

    public override bool ignoreUnlockDependencies => true;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Infomodes == null)
        return;
      for (int index = 0; index < this.m_Infomodes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Infomodes[index].m_Mode);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewData>());
      components.Add(ComponentType.ReadWrite<InfoviewMode>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      InfoviewData componentData;
      componentData.m_NotificationMask = 0U;
      if (this.m_WarningCategories != null)
      {
        for (int index = 0; index < this.m_WarningCategories.Length; ++index)
          componentData.m_NotificationMask |= 1U << (int) (this.m_WarningCategories[index] & (IconCategory) 31);
      }
      entityManager.SetComponentData<InfoviewData>(entity, componentData);
      this.isValid = this.m_Infomodes != null && this.m_Infomodes.Length != 0;
    }
  }
}
