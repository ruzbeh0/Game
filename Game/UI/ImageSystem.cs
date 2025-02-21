// Decompiled with JetBrains decompiler
// Type: Game.UI.ImageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Prefabs;
using Game.SceneFlow;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI
{
  [CompilerGenerated]
  public class ImageSystem : GameSystemBase
  {
    private const string kPlaceholderIcon = "Media/Placeholder.svg";
    private const string kCitizenIcon = "Media/Game/Icons/Citizen.svg";
    private const string kTouristIcon = "Media/Game/Icons/Tourist.svg";
    private const string kCommuterIcon = "Media/Game/Icons/Commuter.svg";
    private const string kAnimalIcon = "Media/Game/Icons/Animal.svg";
    private const string kPetIcon = "Media/Game/Icons/Pet.svg";
    private const string kHealthcareIcon = "Media/Game/Icons/Healthcare.svg";
    private const string kDeathcareIcon = "Media/Game/Icons/Deathcare.svg";
    private const string kPoliceIcon = "Media/Game/Icons/Police.svg";
    private const string kGarbageIcon = "Media/Game/Icons/Garbage.svg";
    private const string kFireIcon = "Media/Game/Icons/FireSafety.svg";
    private const string kPostIcon = "Media/Game/Icons/PostService.svg";
    private const string kDeliveryIcon = "Media/Game/Icons/DeliveryVan.svg";
    private PrefabSystem m_PrefabSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    public string placeholderIcon => "Media/Placeholder.svg";

    [CanBeNull]
    public static string GetIcon(PrefabBase prefab)
    {
      UIObject component;
      return prefab.TryGet<UIObject>(out component) && !string.IsNullOrEmpty(component.m_Icon) ? component.m_Icon : (string) null;
    }

    [CanBeNull]
    public string GetGroupIcon(Entity prefabEntity)
    {
      UIObjectData component;
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.EntityManager.TryGetComponent<UIObjectData>(prefabEntity, out component) && component.m_Group != Entity.Null && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component.m_Group, out prefab) ? ImageSystem.GetIcon(prefab) : (string) null;
    }

    [CanBeNull]
    public string GetIconOrGroupIcon(Entity prefabEntity)
    {
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.TryGetPrefab<PrefabBase>(prefabEntity, out prefab) ? ImageSystem.GetIcon(prefab) ?? this.GetGroupIcon(prefabEntity) : (string) null;
    }

    [CanBeNull]
    public string GetThumbnail(Entity prefabEntity)
    {
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.TryGetPrefab<PrefabBase>(prefabEntity, out prefab) ? ImageSystem.GetThumbnail(prefab) : (string) null;
    }

    [CanBeNull]
    public static string GetThumbnail(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated method
      string icon = ImageSystem.GetIcon(prefab);
      if (icon != null)
        return icon;
      if (GameManager.instance.configuration.noThumbnails)
        return "Media/Placeholder.svg";
      string p1 = string.Format("{0}?width={1}&height={2}", (object) prefab.thumbnailUrl, (object) 128, (object) 128);
      COSystemBase.baseLog.VerboseFormat("GetThumbnail - {0}", (object) p1);
      return p1;
    }

    [CanBeNull]
    public string GetInstanceIcon(Entity instanceEntity)
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetInstanceIcon(instanceEntity, this.EntityManager.GetComponentData<PrefabRef>(instanceEntity).m_Prefab);
    }

    [CanBeNull]
    public string GetInstanceIcon(Entity instanceEntity, Entity prefabEntity)
    {
      SpawnableBuildingData component1;
      if (this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefabEntity, out component1))
      {
        // ISSUE: reference to a compiler-generated method
        string iconOrGroupIcon = this.GetIconOrGroupIcon(component1.m_ZonePrefab);
        if (iconOrGroupIcon != null)
          return iconOrGroupIcon;
      }
      // ISSUE: reference to a compiler-generated method
      string iconOrGroupIcon1 = this.GetIconOrGroupIcon(prefabEntity);
      if (iconOrGroupIcon1 != null)
        return iconOrGroupIcon1;
      HouseholdMember component2;
      if (this.EntityManager.HasComponent<Citizen>(instanceEntity) && this.EntityManager.TryGetComponent<HouseholdMember>(instanceEntity, out component2))
      {
        if (this.EntityManager.HasChunkComponent<CommuterHousehold>(component2.m_Household))
          return "Media/Game/Icons/Commuter.svg";
        return this.EntityManager.HasComponent<TouristHousehold>(component2.m_Household) ? "Media/Game/Icons/Tourist.svg" : "Media/Game/Icons/Citizen.svg";
      }
      if (this.EntityManager.HasComponent<Animal>(instanceEntity))
        return "Media/Game/Icons/Animal.svg";
      if (this.EntityManager.HasComponent<HouseholdPet>(instanceEntity))
        return "Media/Game/Icons/Pet.svg";
      if (this.EntityManager.HasComponent<AmbulanceData>(prefabEntity))
        return "Media/Game/Icons/Healthcare.svg";
      if (this.EntityManager.HasComponent<PoliceCarData>(prefabEntity))
        return "Media/Game/Icons/Police.svg";
      if (this.EntityManager.HasComponent<FireEngineData>(prefabEntity))
        return "Media/Game/Icons/FireSafety.svg";
      if (this.EntityManager.HasComponent<DeliveryTruckData>(prefabEntity))
        return "Media/Game/Icons/DeliveryVan.svg";
      if (this.EntityManager.HasComponent<PostVanData>(prefabEntity))
        return "Media/Game/Icons/PostService.svg";
      if (this.EntityManager.HasComponent<HearseData>(prefabEntity))
        return "Media/Game/Icons/Deathcare.svg";
      if (this.EntityManager.HasComponent<GarbageTruckData>(prefabEntity))
        return "Media/Game/Icons/Garbage.svg";
      Owner component3;
      PrefabRef component4;
      if (this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(instanceEntity) && this.EntityManager.TryGetComponent<Owner>(instanceEntity, out component3) && this.EntityManager.TryGetComponent<PrefabRef>(component3.m_Owner, out component4))
      {
        instanceEntity = component3.m_Owner;
        prefabEntity = component4.m_Prefab;
      }
      ServiceObjectData component5;
      if (this.EntityManager.TryGetComponent<ServiceObjectData>(prefabEntity, out component5))
      {
        // ISSUE: reference to a compiler-generated method
        string iconOrGroupIcon2 = this.GetIconOrGroupIcon(component5.m_Service);
        if (iconOrGroupIcon2 != null)
          return iconOrGroupIcon2;
      }
      DynamicBuffer<AggregateElement> buffer;
      PrefabRef component6;
      if (this.EntityManager.TryGetBuffer<AggregateElement>(instanceEntity, true, out buffer) && buffer.Length != 0 && this.EntityManager.TryGetComponent<PrefabRef>(buffer[0].m_Edge, out component6))
      {
        // ISSUE: reference to a compiler-generated method
        string iconOrGroupIcon3 = this.GetIconOrGroupIcon(component6.m_Prefab);
        if (iconOrGroupIcon3 != null)
          return iconOrGroupIcon3;
      }
      Owner component7;
      PropertyRenter component8;
      PrefabRef component9;
      if (this.EntityManager.TryGetComponent<Owner>(instanceEntity, out component7) && this.EntityManager.TryGetComponent<PropertyRenter>(component7.m_Owner, out component8) && this.EntityManager.TryGetComponent<PrefabRef>(component8.m_Property, out component9))
      {
        SpawnableBuildingData component10;
        // ISSUE: reference to a compiler-generated method
        string iconOrGroupIcon4 = this.GetIconOrGroupIcon(this.EntityManager.TryGetComponent<SpawnableBuildingData>(component9.m_Prefab, out component10) ? component10.m_ZonePrefab : component9.m_Prefab);
        if (iconOrGroupIcon4 != null)
          return iconOrGroupIcon4;
      }
      return (string) null;
    }

    [Preserve]
    public ImageSystem()
    {
    }
  }
}
