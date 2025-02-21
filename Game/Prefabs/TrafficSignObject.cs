// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrafficSignObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Objects/", new Type[] {typeof (StaticObjectPrefab), typeof (MarkerObjectPrefab)})]
  public class TrafficSignObject : ComponentBase
  {
    public TrafficSignType[] m_SignTypes;
    public int m_SpeedLimit;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrafficSignData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      TrafficSignData componentData;
      componentData.m_TypeMask = 0U;
      componentData.m_SpeedLimit = this.m_SpeedLimit;
      if (this.m_SignTypes != null)
      {
        for (int index = 0; index < this.m_SignTypes.Length; ++index)
          componentData.m_TypeMask |= TrafficSignData.GetTypeMask(this.m_SignTypes[index]);
      }
      entityManager.SetComponentData<TrafficSignData>(entity, componentData);
    }
  }
}
