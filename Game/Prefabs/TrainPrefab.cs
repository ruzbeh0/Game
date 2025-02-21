// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrainPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Pathfind;
using Game.PSI;
using Game.Vehicles;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ExcludeGeneratedModTag]
  public abstract class TrainPrefab : VehiclePrefab
  {
    public TrackTypes m_TrackType = TrackTypes.Train;
    public EnergyTypes m_EnergyType = EnergyTypes.Electricity;
    public float m_MaxSpeed = 200f;
    public float m_Acceleration = 5f;
    public float m_Braking = 10f;
    public float2 m_Turning = new float2(90f, 15f);
    public float2 m_BogieOffset = new float2(4f, 4f);
    public float2 m_AttachOffset = new float2(0.0f, 0.0f);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TrainData>());
      components.Add(ComponentType.ReadWrite<TrainObjectData>());
      components.Add(ComponentType.ReadWrite<UpdateFrameData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<Train>());
      if (components.Contains(ComponentType.ReadWrite<Stopped>()))
        components.Add(ComponentType.ReadWrite<ParkedTrain>());
      if (!components.Contains(ComponentType.ReadWrite<Moving>()))
        return;
      components.Add(ComponentType.ReadWrite<TrainNavigation>());
      components.Add(ComponentType.ReadWrite<TrainCurrentLane>());
      components.Add(ComponentType.ReadWrite<TrainBogieFrame>());
      if (!components.Contains(ComponentType.ReadWrite<LayoutElement>()))
        return;
      components.Add(ComponentType.ReadWrite<PathOwner>());
      components.Add(ComponentType.ReadWrite<PathElement>());
      components.Add(ComponentType.ReadWrite<Target>());
      components.Add(ComponentType.ReadWrite<Blocker>());
      components.Add(ComponentType.ReadWrite<TrainNavigationLane>());
    }

    protected override void RefreshArchetype(EntityManager entityManager, Entity entity)
    {
      List<ComponentBase> list = new List<ComponentBase>();
      this.GetComponents<ComponentBase>(list);
      ObjectData componentData1 = new ObjectData();
      MovingObjectData componentData2 = new MovingObjectData();
      TrainObjectData componentData3 = new TrainObjectData();
      HashSet<ComponentType> componentTypeSet = new HashSet<ComponentType>();
      componentTypeSet.Add(ComponentType.ReadWrite<Controller>());
      componentTypeSet.Add(ComponentType.ReadWrite<Moving>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentData1.m_Archetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentTypeSet.Clear();
      componentTypeSet.Add(ComponentType.ReadWrite<Controller>());
      componentTypeSet.Add(ComponentType.ReadWrite<Stopped>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentData2.m_StoppedArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentTypeSet.Clear();
      componentTypeSet.Add(ComponentType.ReadWrite<Controller>());
      componentTypeSet.Add(ComponentType.ReadWrite<Moving>());
      componentTypeSet.Add(ComponentType.ReadWrite<LayoutElement>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentData3.m_ControllerArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      componentTypeSet.Clear();
      componentTypeSet.Add(ComponentType.ReadWrite<Controller>());
      componentTypeSet.Add(ComponentType.ReadWrite<Stopped>());
      componentTypeSet.Add(ComponentType.ReadWrite<LayoutElement>());
      for (int index = 0; index < list.Count; ++index)
        list[index].GetArchetypeComponents(componentTypeSet);
      componentTypeSet.Add(ComponentType.ReadWrite<Created>());
      componentTypeSet.Add(ComponentType.ReadWrite<Updated>());
      componentData3.m_StoppedControllerArchetype = entityManager.CreateArchetype(PrefabUtils.ToArray<ComponentType>(componentTypeSet));
      entityManager.SetComponentData<ObjectData>(entity, componentData1);
      entityManager.SetComponentData<MovingObjectData>(entity, componentData2);
      entityManager.SetComponentData<TrainObjectData>(entity, componentData3);
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      entityManager.SetComponentData<UpdateFrameData>(entity, new UpdateFrameData(3));
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        foreach (string enumFlagTag in ModTags.GetEnumFlagTags<TrackTypes>(this.m_TrackType, TrackTypes.Train))
          yield return enumFlagTag;
      }
    }
  }
}
