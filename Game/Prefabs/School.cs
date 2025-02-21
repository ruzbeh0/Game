// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.School
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Buildings/CityServices/", new System.Type[] {typeof (BuildingPrefab), typeof (BuildingExtensionPrefab), typeof (MarkerObjectPrefab)})]
  public class School : ComponentBase, IServiceUpgrade
  {
    public int m_StudentCapacity = 80;
    public SchoolLevel m_Level;
    public float m_GraduationModifier;
    public sbyte m_StudentWellbeing;
    public sbyte m_StudentHealth;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SchoolData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.School>());
      if (!((UnityEngine.Object) this.GetComponent<ServiceUpgrade>() == (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.GetComponent<CityServiceBuilding>() != (UnityEngine.Object) null)
        components.Add(ComponentType.ReadWrite<Efficiency>());
      components.Add(ComponentType.ReadWrite<Student>());
      if (!((UnityEngine.Object) this.GetComponent<UniqueObject>() == (UnityEngine.Object) null))
        return;
      components.Add(ComponentType.ReadWrite<ServiceDistrict>());
    }

    public void GetUpgradeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Buildings.School>());
      components.Add(ComponentType.ReadWrite<Student>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      SchoolData componentData;
      componentData.m_EducationLevel = (byte) this.m_Level;
      componentData.m_StudentCapacity = this.m_StudentCapacity;
      componentData.m_GraduationModifier = this.m_GraduationModifier;
      componentData.m_StudentWellbeing = this.m_StudentWellbeing;
      componentData.m_StudentHealth = this.m_StudentHealth;
      entityManager.SetComponentData<SchoolData>(entity, componentData);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(6));
    }
  }
}
