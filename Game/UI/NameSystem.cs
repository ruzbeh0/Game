// Decompiled with JetBrains decompiler
// Type: Game.UI.NameSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.SceneFlow;
using Game.UI.InGame;
using Game.UI.Localization;
using Game.Vehicles;
using Game.Zones;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI
{
  [CompilerGenerated]
  public class NameSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private EntityQuery m_DeletedQuery;
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private Dictionary<Entity, string> m_Names;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<CustomName>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_Names = new Dictionary<Entity, string>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_DeletedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_DeletedQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_Names.ContainsKey(entityArray[index]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Names.Remove(entityArray[index]);
          }
        }
      }
    }

    public string GetDebugName(Entity entity)
    {
      string str = (string) null;
      PrefabRef component;
      if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component))
      {
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        str = !this.EntityManager.HasComponent<PrefabData>(component.m_Prefab) ? "(invalid prefab)" : (!this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component.m_Prefab, out prefab) ? this.m_PrefabSystem.GetObsoleteID(component.m_Prefab).GetName() : prefab.name);
      }
      return string.Format("{0} {1}", (object) str, (object) entity.Index);
    }

    public bool TryGetCustomName(Entity entity, out string customName)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_Names.TryGetValue(entity, out customName);
    }

    public void SetCustomName(Entity entity, string name)
    {
      if (entity == Entity.Null)
        return;
      Controller component;
      if (this.EntityManager.TryGetComponent<Controller>(entity, out component))
        entity = component.m_Controller;
      if (name == string.Empty || string.IsNullOrWhiteSpace(name))
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Names.ContainsKey(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.RemoveComponent<CustomName>(entity);
        commandBuffer.AddComponent<BatchesUpdated>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_Names.Remove(entity);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Names[entity] = name;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.AddComponent<CustomName>(entity);
        commandBuffer.AddComponent<BatchesUpdated>(entity);
      }
    }

    public string GetRenderedLabelName(Entity entity)
    {
      string customName;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetCustomName(entity, out customName))
        return customName;
      // ISSUE: reference to a compiler-generated method
      string id = this.GetId(entity);
      string str;
      return !GameManager.instance.localizationManager.activeDictionary.TryGetValue(id, out str) ? id : str;
    }

    public void BindNameForVirtualKeyboard(IJsonWriter writer, Entity entity)
    {
      // ISSUE: reference to a compiler-generated method
      writer.Write<NameSystem.Name>(this.GetNameForVirtualKeyboard(entity));
    }

    public NameSystem.Name GetNameForVirtualKeyboard(Entity entity)
    {
      Entity prefab1 = Entity.Null;
      PrefabRef component1;
      if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component1))
        prefab1 = component1.m_Prefab;
      if (prefab1 != Entity.Null && this.EntityManager.HasComponent<TransportLine>(entity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        RoutePrefab prefab2 = this.m_PrefabSystem.GetPrefab<RoutePrefab>(prefab1);
        // ISSUE: reference to a compiler-generated method
        return NameSystem.Name.LocalizedName(prefab2.m_LocaleID + "[" + prefab2.name + "]");
      }
      Controller component2;
      if (this.EntityManager.TryGetComponent<Controller>(entity, out component2))
        entity = component2.m_Controller;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.LocalizedName(this.GetId(entity, false));
    }

    public void BindName(IJsonWriter writer, Entity entity)
    {
      // ISSUE: reference to a compiler-generated method
      writer.Write<NameSystem.Name>(this.GetName(entity));
    }

    public NameSystem.Name GetName(Entity entity, bool omitBrand = false)
    {
      Entity prefab = Entity.Null;
      PrefabRef component1;
      if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component1))
        prefab = component1.m_Prefab;
      Controller component2;
      if (this.EntityManager.TryGetComponent<Controller>(entity, out component2))
        entity = component2.m_Controller;
      string customName;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetCustomName(entity, out customName))
      {
        // ISSUE: reference to a compiler-generated method
        return NameSystem.Name.CustomName(customName);
      }
      CompanyData component3;
      if (this.EntityManager.TryGetComponent<CompanyData>(entity, out component3))
      {
        // ISSUE: reference to a compiler-generated method
        string id = this.GetId(component3.m_Brand);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return id == null ? NameSystem.Name.CustomName(this.m_PrefabSystem.GetPrefab<PrefabBase>(prefab).name + " brand is null!") : NameSystem.Name.LocalizedName(id);
      }
      EntityManager entityManager;
      if (prefab != Entity.Null)
      {
        entityManager = this.EntityManager;
        SpawnableBuildingData component4;
        if (!entityManager.HasComponent<SignatureBuildingData>(prefab) && this.EntityManager.TryGetComponent<SpawnableBuildingData>(prefab, out component4))
        {
          // ISSUE: reference to a compiler-generated method
          return this.GetSpawnableBuildingName(entity, component4.m_ZonePrefab, omitBrand);
        }
      }
      entityManager = this.EntityManager;
      if (entityManager.HasComponent<Game.Routes.TransportStop>(entity))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Game.Objects.OutsideConnection>(entity))
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          return entityManager.HasComponent<Marker>(entity) ? this.GetMarkerTransportStopName(entity) : this.GetStaticTransportStopName(entity);
        }
      }
      if (prefab != Entity.Null)
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<TransportLine>(entity))
        {
          // ISSUE: reference to a compiler-generated method
          return this.GetRouteName(entity, prefab);
        }
      }
      if (prefab != Entity.Null)
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Citizen>(entity))
        {
          // ISSUE: reference to a compiler-generated method
          return this.GetCitizenName(entity, prefab);
        }
      }
      if (prefab != Entity.Null)
      {
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<Game.Creatures.Resident>(entity))
        {
          // ISSUE: reference to a compiler-generated method
          return this.GetResidentName(entity, prefab);
        }
      }
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return entityManager.HasComponent<Game.Events.TrafficAccident>(entity) ? NameSystem.Name.LocalizedName("SelectedInfoPanel.TRAFFIC_ACCIDENT") : NameSystem.Name.LocalizedName(this.GetId(entity));
    }

    private string GetGenderedLastNameId(Entity household, bool male)
    {
      if (household == Entity.Null)
        return (string) null;
      PrefabRef component1;
      RandomGenderedLocalization component2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetComponent<PrefabRef>(household, out component1) || !this.m_PrefabSystem.GetPrefab<PrefabBase>(component1).TryGet<RandomGenderedLocalization>(out component2))
      {
        // ISSUE: reference to a compiler-generated method
        return this.GetId(household);
      }
      string localeId = male ? component2.m_MaleID : component2.m_FemaleID;
      DynamicBuffer<RandomLocalizationIndex> buffer;
      return this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(household, true, out buffer) && buffer.Length > 0 ? LocalizationUtils.AppendIndex(localeId, buffer[0]) : localeId;
    }

    public void BindFamilyName(IJsonWriter writer, Entity household)
    {
      // ISSUE: reference to a compiler-generated method
      writer.Write<NameSystem.Name>(this.GetFamilyName(household));
    }

    private NameSystem.Name GetFamilyName(Entity household)
    {
      string customName;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetCustomName(household, out customName))
      {
        // ISSUE: reference to a compiler-generated method
        return NameSystem.Name.CustomName(customName);
      }
      int num = 0;
      DynamicBuffer<HouseholdCitizen> buffer = this.EntityManager.GetBuffer<HouseholdCitizen>(household);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Citizen component;
        if (this.EntityManager.TryGetComponent<Citizen>((Entity) buffer[index], out component) && (component.m_State & CitizenFlags.Male) != CitizenFlags.None)
          ++num;
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.LocalizedName(this.GetGenderedLastNameId(household, num > 1));
    }

    private string GetId(Entity entity, bool useRandomLocalization = true)
    {
      if (entity == Entity.Null)
        return (string) null;
      Entity entity1 = Entity.Null;
      PrefabRef component1;
      if (this.EntityManager.TryGetComponent<PrefabRef>(entity, out component1))
        entity1 = component1.m_Prefab;
      EntityManager entityManager = this.EntityManager;
      SpawnableBuildingData component2;
      if (!entityManager.HasComponent<SignatureBuildingData>(entity1) && this.EntityManager.TryGetComponent<SpawnableBuildingData>(entity1, out component2))
        entity1 = component2.m_ZonePrefab;
      entityManager = this.EntityManager;
      if (!entityManager.HasComponent<ChirperAccountData>(entity))
      {
        entityManager = this.EntityManager;
        if (!entityManager.HasComponent<BrandData>(entity))
          goto label_9;
      }
      entity1 = entity;
label_9:
      if (!(entity1 != Entity.Null))
        return string.Empty;
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_PrefabSystem.TryGetPrefab<PrefabBase>(entity1, out prefab))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabSystem.GetObsoleteID(entity1).GetName();
      }
      Game.Prefabs.Localization component3;
      if (prefab.TryGet<Game.Prefabs.Localization>(out component3))
      {
        DynamicBuffer<RandomLocalizationIndex> buffer;
        return useRandomLocalization && component3 is RandomLocalization && this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(entity, true, out buffer) && buffer.Length > 0 ? LocalizationUtils.AppendIndex(component3.m_LocalizationID, buffer[0]) : component3.m_LocalizationID;
      }
      string titleId;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.GetTitleAndDescription(entity1, out titleId, out string _);
      return titleId;
    }

    private NameSystem.Name GetCitizenName(Entity entity, Entity prefab)
    {
      Citizen component;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.FormattedName("Assets.CITIZEN_NAME_FORMAT", "FIRST_NAME", this.GetId(entity), "LAST_NAME", this.GetGenderedLastNameId(this.EntityManager.GetComponentData<HouseholdMember>(entity).m_Household, this.EntityManager.TryGetComponent<Citizen>(entity, out component) && (component.m_State & CitizenFlags.Male) != 0));
    }

    private NameSystem.Name GetResidentName(Entity entity, Entity prefab)
    {
      PseudoRandomSeed component1;
      this.EntityManager.TryGetComponent<PseudoRandomSeed>(entity, out component1);
      CreatureData component2;
      this.EntityManager.TryGetComponent<CreatureData>(prefab, out component2);
      Random random = component1.GetRandom((uint) PseudoRandomSeed.kDummyName);
      bool flag = false;
      if (component2.m_Gender == GenderMask.Male)
        flag = true;
      else if (component2.m_Gender != GenderMask.Female)
        flag = random.NextBool();
      string str1 = flag ? "Assets.CITIZEN_NAME_MALE" : "Assets.CITIZEN_NAME_FEMALE";
      string str2 = flag ? "Assets.CITIZEN_SURNAME_MALE" : "Assets.CITIZEN_SURNAME_FEMALE";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(prefab);
      int localizationIndexCount1 = RandomLocalization.GetLocalizationIndexCount(prefab1, str1);
      int localizationIndexCount2 = RandomLocalization.GetLocalizationIndexCount(prefab1, str2);
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.FormattedName("Assets.CITIZEN_NAME_FORMAT", "FIRST_NAME", LocalizationUtils.AppendIndex(str1, new RandomLocalizationIndex(random.NextInt(localizationIndexCount1))), "LAST_NAME", LocalizationUtils.AppendIndex(str2, new RandomLocalizationIndex(random.NextInt(localizationIndexCount2))));
    }

    private NameSystem.Name GetSpawnableBuildingName(Entity building, Entity zone, bool omitBrand = false)
    {
      Entity road;
      int number;
      BuildingUtils.GetAddress(this.EntityManager, building, out road, out number);
      string customName;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetCustomName(road, out customName))
      {
        // ISSUE: reference to a compiler-generated method
        customName = this.GetId(road);
      }
      if (customName == null)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return NameSystem.Name.LocalizedName(this.GetId(building));
      }
      ZoneData component;
      if (!omitBrand && this.EntityManager.TryGetComponent<ZoneData>(zone, out component) && component.m_AreaType != AreaType.Residential)
      {
        // ISSUE: reference to a compiler-generated method
        string brandId = this.GetBrandId(building);
        if (brandId != null)
        {
          // ISSUE: reference to a compiler-generated method
          return NameSystem.Name.FormattedName("Assets.NAMED_ADDRESS_NAME_FORMAT", "NAME", brandId, "ROAD", customName, "NUMBER", number.ToString());
        }
      }
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.FormattedName("Assets.ADDRESS_NAME_FORMAT", "ROAD", customName, "NUMBER", number.ToString());
    }

    private string GetBrandId(Entity building)
    {
      DynamicBuffer<Renter> buffer = this.EntityManager.GetBuffer<Renter>(building, true);
      for (int index = 0; index < buffer.Length; ++index)
      {
        CompanyData component;
        if (this.EntityManager.TryGetComponent<CompanyData>(buffer[index].m_Renter, out component))
        {
          // ISSUE: reference to a compiler-generated method
          return this.GetId(component.m_Brand);
        }
      }
      return (string) null;
    }

    private NameSystem.Name GetStaticTransportStopName(Entity stop)
    {
      Entity road;
      int number;
      BuildingUtils.GetAddress(this.EntityManager, stop, out road, out number);
      string customName;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetCustomName(road, out customName))
      {
        // ISSUE: reference to a compiler-generated method
        customName = this.GetId(road);
      }
      if (customName == null)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        return NameSystem.Name.LocalizedName(this.GetId(stop));
      }
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.FormattedName("Assets.ADDRESS_NAME_FORMAT", "ROAD", customName, "NUMBER", number.ToString());
    }

    private NameSystem.Name GetMarkerTransportStopName(Entity stop)
    {
      Entity entity = stop;
      Owner component;
      for (int index = 0; index < 8 && this.EntityManager.TryGetComponent<Owner>(entity, out component); ++index)
        entity = component.m_Owner;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.LocalizedName(this.GetId(entity));
    }

    private NameSystem.Name GetRouteName(Entity route, Entity prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      RoutePrefab prefab1 = this.m_PrefabSystem.GetPrefab<RoutePrefab>(prefab);
      RouteNumber componentData = this.EntityManager.GetComponentData<RouteNumber>(route);
      // ISSUE: reference to a compiler-generated method
      return NameSystem.Name.FormattedName(prefab1.m_LocaleID + "[" + prefab1.name + "]", "NUMBER", componentData.m_Number.ToString());
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_Names.Count);
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<Entity, string> name in this.m_Names)
      {
        writer.Write(name.Key);
        writer.Write(name.Value);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int num;
      reader.Read(out num);
      // ISSUE: reference to a compiler-generated field
      this.m_Names.Clear();
      for (int index = 0; index < num; ++index)
      {
        Entity key;
        reader.Read(out key);
        string str;
        reader.Read(out str);
        if (key != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Names.Add(key, str);
        }
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Names.Clear();
    }

    [Preserve]
    public NameSystem()
    {
    }

    public enum NameType
    {
      Custom,
      Localized,
      Formatted,
    }

    public struct Name : IJsonWritable, ISerializable
    {
      private NameSystem.NameType m_NameType;
      private string m_NameID;
      private string[] m_NameArgs;

      public static NameSystem.Name CustomName(string name)
      {
        // ISSUE: object of a compiler-generated type is created
        return new NameSystem.Name()
        {
          m_NameType = NameSystem.NameType.Custom,
          m_NameID = name
        };
      }

      public static NameSystem.Name LocalizedName(string nameID)
      {
        // ISSUE: object of a compiler-generated type is created
        return new NameSystem.Name()
        {
          m_NameType = NameSystem.NameType.Localized,
          m_NameID = nameID
        };
      }

      public static NameSystem.Name FormattedName(string nameID, params string[] args)
      {
        // ISSUE: object of a compiler-generated type is created
        return new NameSystem.Name()
        {
          m_NameType = NameSystem.NameType.Formatted,
          m_NameID = nameID,
          m_NameArgs = args
        };
      }

      public void Write(IJsonWriter writer)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NameType == NameSystem.NameType.Custom)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindCustomName(writer);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NameType == NameSystem.NameType.Formatted)
          {
            // ISSUE: reference to a compiler-generated method
            this.BindFormattedName(writer);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_NameType != NameSystem.NameType.Localized)
              return;
            // ISSUE: reference to a compiler-generated method
            this.BindLocalizedName(writer);
          }
        }
      }

      private void BindCustomName(IJsonWriter writer)
      {
        writer.TypeBegin("names.CustomName");
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_NameID);
        writer.TypeEnd();
      }

      private void BindFormattedName(IJsonWriter writer)
      {
        writer.TypeBegin("names.FormattedName");
        writer.PropertyName("nameId");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_NameID);
        writer.PropertyName("nameArgs");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int size = this.m_NameArgs != null ? this.m_NameArgs.Length / 2 : 0;
        writer.MapBegin(size);
        for (int index = 0; index < size; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_NameArgs[index * 2] ?? string.Empty);
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_NameArgs[index * 2 + 1] ?? string.Empty);
        }
        writer.MapEnd();
        writer.TypeEnd();
      }

      private void BindLocalizedName(IJsonWriter writer)
      {
        writer.TypeBegin("names.LocalizedName");
        writer.PropertyName("nameId");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_NameID ?? string.Empty);
        writer.TypeEnd();
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write((int) this.m_NameType);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_NameID ?? string.Empty);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int length = this.m_NameArgs != null ? this.m_NameArgs.Length : 0;
        writer.Write(length);
        for (int index = 0; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_NameArgs[index] ?? string.Empty);
        }
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        int num;
        reader.Read(out num);
        // ISSUE: reference to a compiler-generated field
        this.m_NameType = (NameSystem.NameType) num;
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_NameID);
        int length;
        reader.Read(out length);
        // ISSUE: reference to a compiler-generated field
        this.m_NameArgs = new string[length];
        for (int index = 0; index < length; ++index)
        {
          string str;
          reader.Read(out str);
          // ISSUE: reference to a compiler-generated field
          this.m_NameArgs[index] = str;
        }
      }
    }
  }
}
