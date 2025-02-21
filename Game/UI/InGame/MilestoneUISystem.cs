// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.MilestoneUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Settings;
using Game.Simulation;
using Game.Tools;
using Game.Tutorials;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class MilestoneUISystem : UISystemBase, IXPMessageHandler
  {
    private const string kGroup = "milestone";
    private PrefabSystem m_PrefabSystem;
    private IXPSystem m_XPSystem;
    private CitySystem m_CitySystem;
    private IMilestoneSystem m_XpMilestoneSystem;
    private ImageSystem m_ImageSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private TutorialSystem m_TutorialSystem;
    private EntityQuery m_MilestoneLevelQuery;
    private EntityQuery m_MilestoneQuery;
    private EntityQuery m_LockedMilestoneQuery;
    private EntityQuery m_ModifiedMilestoneQuery;
    private EntityQuery m_MilestoneReachedEventQuery;
    private EntityQuery m_UnlockableAssetQuery;
    private EntityQuery m_UnlockableZoneQuery;
    private EntityQuery m_DevTreeNodeQuery;
    private EntityQuery m_UnlockableFeatureQuery;
    private EntityQuery m_UnlockablePolicyQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private GetterValueBinding<int> m_AchievedMilestoneBinding;
    private GetterValueBinding<bool> m_MaxMilestoneReachedBinding;
    private GetterValueBinding<int> m_AchievedMilestoneXPBinding;
    private GetterValueBinding<int> m_NextMilestoneXPBinding;
    private GetterValueBinding<int> m_TotalXPBinding;
    private RawEventBinding m_XpMessageAddedBinding;
    private RawValueBinding m_MilestonesBinding;
    private ValueBinding<Entity> m_UnlockedMilestoneBinding;
    private RawMapBinding<Entity> m_MilestoneDetailsBinding;
    private RawMapBinding<Entity> m_MilestoneUnlocksBinding;
    private RawMapBinding<Entity> m_UnlockDetailsBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_XPSystem = (IXPSystem) this.World.GetOrCreateSystemManaged<XPSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_XpMilestoneSystem = (IMilestoneSystem) this.World.GetOrCreateSystemManaged<MilestoneSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem = this.World.GetOrCreateSystemManaged<TutorialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneLevelQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneLevel>());
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<MilestoneData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_LockedMilestoneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<MilestoneData>(),
          ComponentType.ReadOnly<Locked>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedMilestoneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<MilestoneData>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_MilestoneReachedEventQuery = this.GetEntityQuery(ComponentType.ReadOnly<MilestoneReachedEvent>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockableAssetQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<UIObjectData>(),
          ComponentType.ReadOnly<ServiceObjectData>(),
          ComponentType.ReadOnly<UnlockRequirement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockableZoneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<UIObjectData>(),
          ComponentType.ReadOnly<ZoneData>(),
          ComponentType.ReadOnly<UnlockRequirement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[5]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<UIObjectData>(),
          ComponentType.ReadOnly<PlaceholderBuildingData>(),
          ComponentType.ReadOnly<PlaceableObjectData>(),
          ComponentType.ReadOnly<UnlockRequirement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DevTreeNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<DevTreeNodeData>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockableFeatureQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<FeatureData>(),
          ComponentType.ReadOnly<UIObjectData>(),
          ComponentType.ReadOnly<UnlockRequirement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockablePolicyQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<PolicyData>(), ComponentType.ReadOnly<UIObjectData>(), ComponentType.ReadOnly<UnlockRequirement>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AchievedMilestoneBinding = new GetterValueBinding<int>("milestone", "achievedMilestone", (Func<int>) (() => this.m_MilestoneLevelQuery.IsEmptyIgnoreFilter ? 0 : this.m_MilestoneLevelQuery.GetSingleton<MilestoneLevel>().m_AchievedMilestone))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MaxMilestoneReachedBinding = new GetterValueBinding<bool>("milestone", "maxMilestoneReached", (Func<bool>) (() => this.m_LockedMilestoneQuery.IsEmpty))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AchievedMilestoneXPBinding = new GetterValueBinding<int>("milestone", "achievedMilestoneXP", (Func<int>) (() => this.m_XpMilestoneSystem.lastRequiredXP))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_NextMilestoneXPBinding = new GetterValueBinding<int>("milestone", "nextMilestoneXP", (Func<int>) (() => this.m_XpMilestoneSystem.nextRequiredXP))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TotalXPBinding = new GetterValueBinding<int>("milestone", "totalXP", (Func<int>) (() => this.m_CitySystem.XP))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_XpMessageAddedBinding = new RawEventBinding("milestone", "xpMessageAdded")));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MilestonesBinding = new RawValueBinding("milestone", "milestones", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeArray<MilestoneUISystem.ComparableMilestone> sortedMilestones = this.GetSortedMilestones(Allocator.TempJob);
        writer.ArrayBegin(sortedMilestones.Length);
        for (int index = 0; index < sortedMilestones.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = sortedMilestones[index].m_Entity;
          // ISSUE: reference to a compiler-generated field
          MilestoneData data = sortedMilestones[index].m_Data;
          writer.TypeBegin("milestone.Milestone");
          writer.PropertyName("entity");
          writer.Write(entity);
          writer.PropertyName("index");
          writer.Write(data.m_Index);
          writer.PropertyName("major");
          writer.Write(data.m_Major);
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedMilestones.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UnlockedMilestoneBinding = new ValueBinding<Entity>("milestone", "unlockedMilestone", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding("milestone", "clearUnlockedMilestone", (System.Action) (() => this.m_UnlockedMilestoneBinding.Update(Entity.Null))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MilestoneDetailsBinding = new RawMapBinding<Entity>("milestone", "milestoneDetails", (Action<IJsonWriter, Entity>) ((writer, milestone) =>
      {
        Entity entity = milestone;
        MilestoneData component;
        if (entity != Entity.Null && this.EntityManager.TryGetComponent<MilestoneData>(entity, out component))
        {
          bool flag = this.EntityManager.HasEnabledComponent<Locked>(entity);
          writer.TypeBegin("milestone.MilestoneDetails");
          writer.PropertyName("entity");
          writer.Write(milestone);
          writer.PropertyName("index");
          writer.Write(component.m_Index);
          writer.PropertyName("xpRequirement");
          writer.Write(component.m_XpRequried);
          writer.PropertyName("reward");
          writer.Write(component.m_Reward);
          writer.PropertyName("devTreePoints");
          writer.Write(component.m_DevTreePoints);
          writer.PropertyName("mapTiles");
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_CityConfigurationSystem.unlockMapTiles ? 0 : component.m_MapTiles);
          writer.PropertyName("loanLimit");
          writer.Write(component.m_LoanLimit);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          MilestonePrefab prefab = this.m_PrefabSystem.GetPrefab<MilestonePrefab>(entity);
          writer.PropertyName("image");
          writer.Write(prefab.m_Image);
          writer.PropertyName("backgroundColor");
          writer.Write(prefab.m_BackgroundColor);
          writer.PropertyName("accentColor");
          writer.Write(prefab.m_AccentColor);
          writer.PropertyName("textColor");
          writer.Write(prefab.m_TextColor);
          writer.PropertyName("locked");
          writer.Write(flag);
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MilestoneUnlocksBinding = new RawMapBinding<Entity>("milestone", "milestoneUnlocks", (Action<IJsonWriter, Entity>) ((writer, milestoneEntity) =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<Entity> unlockedDevTreeServices = this.GetUnlockedDevTreeServices(milestoneEntity, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<Entity> unlockedZones = this.GetUnlockedZones(milestoneEntity, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<Entity> unlockedAssets = this.GetUnlockedAssets(milestoneEntity, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<UIObjectInfo> unlockedFeatures = this.GetSortedUnlockedFeatures(milestoneEntity, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<MilestoneUISystem.ServiceInfo> sortedServices = this.GetSortedServices(unlockedDevTreeServices, unlockedAssets, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<MilestoneUISystem.AssetInfo> sortedZones = this.GetSortedZones(unlockedZones, Allocator.TempJob);
        // ISSUE: reference to a compiler-generated method
        NativeList<UIObjectInfo> sortedPolicies = this.GetSortedPolicies(milestoneEntity, Allocator.TempJob);
        NativeList<MilestoneUISystem.AssetInfo> result = new NativeList<MilestoneUISystem.AssetInfo>(20, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<Entity> assetThemes = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        writer.ArrayBegin(unlockedFeatures.Length + sortedZones.Length + sortedServices.Length + sortedPolicies.Length);
        for (int index = 0; index < unlockedFeatures.Length; ++index)
        {
          UIObjectInfo uiObjectInfo = unlockedFeatures[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          FeaturePrefab prefab = this.m_PrefabSystem.GetPrefab<FeaturePrefab>(uiObjectInfo.prefabData);
          UIObject component = prefab.GetComponent<UIObject>();
          writer.TypeBegin("milestone.Feature");
          writer.PropertyName("entity");
          writer.Write(uiObjectInfo.entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          writer.Write(component.m_Icon);
          writer.TypeEnd();
        }
        for (int index = 0; index < sortedZones.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          MilestoneUISystem.AssetInfo asset = sortedZones[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(asset.m_PrefabData);
          // ISSUE: reference to a compiler-generated method
          this.BindAsset(writer, asset, prefab, assetThemes);
        }
        for (int index1 = 0; index1 < sortedServices.Length; ++index1)
        {
          // ISSUE: variable of a compiler-generated type
          MilestoneUISystem.ServiceInfo serviceInfo = sortedServices[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          ServicePrefab prefab1 = this.m_PrefabSystem.GetPrefab<ServicePrefab>(serviceInfo.m_PrefabData);
          UIObject component = prefab1.GetComponent<UIObject>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.FilterAndSortAssets(result, serviceInfo.m_Entity, unlockedAssets);
          writer.TypeBegin("milestone.Service");
          writer.PropertyName("entity");
          // ISSUE: reference to a compiler-generated field
          writer.Write(serviceInfo.m_Entity);
          writer.PropertyName("name");
          writer.Write(prefab1.name);
          writer.PropertyName("icon");
          writer.Write(component.m_Icon);
          writer.PropertyName("devTreeUnlocked");
          // ISSUE: reference to a compiler-generated field
          writer.Write(serviceInfo.m_DevTreeUnlocked);
          writer.PropertyName("assets");
          writer.ArrayBegin(result.Length);
          for (int index2 = 0; index2 < result.Length; ++index2)
          {
            // ISSUE: variable of a compiler-generated type
            MilestoneUISystem.AssetInfo asset = result[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(asset.m_PrefabData);
            // ISSUE: reference to a compiler-generated method
            this.BindAsset(writer, asset, prefab2, assetThemes);
          }
          writer.ArrayEnd();
          writer.TypeEnd();
        }
        for (int index = 0; index < sortedPolicies.Length; ++index)
        {
          UIObjectInfo uiObjectInfo = sortedPolicies[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PolicyPrefab prefab = this.m_PrefabSystem.GetPrefab<PolicyPrefab>(uiObjectInfo.prefabData);
          UIObject component = prefab.GetComponent<UIObject>();
          writer.TypeBegin("milestone.Policy");
          writer.PropertyName("entity");
          writer.Write(uiObjectInfo.entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          writer.Write(component.m_Icon);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        unlockedDevTreeServices.Dispose();
        unlockedAssets.Dispose();
        unlockedZones.Dispose();
        unlockedFeatures.Dispose();
        sortedServices.Dispose();
        result.Dispose();
        sortedZones.Dispose();
        sortedPolicies.Dispose();
        assetThemes.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UnlockDetailsBinding = new RawMapBinding<Entity>("milestone", "unlockDetails", (Action<IJsonWriter, Entity>) ((writer, unlockEntity) =>
      {
        if (unlockEntity != Entity.Null)
        {
          EntityManager entityManager = this.EntityManager;
          if (entityManager.HasComponent<PrefabData>(unlockEntity))
          {
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<UIObjectData>(unlockEntity))
            {
              bool locked = this.EntityManager.HasEnabledComponent<Locked>(unlockEntity);
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<FeatureData>(unlockEntity))
              {
                // ISSUE: reference to a compiler-generated method
                this.BindFeatureUnlock(writer, unlockEntity, locked);
                return;
              }
              entityManager = this.EntityManager;
              if (entityManager.HasComponent<ServiceData>(unlockEntity))
              {
                // ISSUE: reference to a compiler-generated method
                this.BindServiceUnlock(writer, unlockEntity, locked);
                return;
              }
              entityManager = this.EntityManager;
              if (!entityManager.HasComponent<ServiceObjectData>(unlockEntity))
              {
                entityManager = this.EntityManager;
                if (!entityManager.HasComponent<PlaceableObjectData>(unlockEntity))
                {
                  entityManager = this.EntityManager;
                  if (!entityManager.HasComponent<ZoneData>(unlockEntity))
                  {
                    entityManager = this.EntityManager;
                    if (entityManager.HasComponent<PolicyData>(unlockEntity))
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.BindPolicyUnlock(writer, unlockEntity, locked);
                      return;
                    }
                    writer.WriteNull();
                    return;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.BindAssetUnlock(writer, unlockEntity, locked);
              return;
            }
          }
        }
        writer.WriteNull();
      }))));
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedMilestoneBinding.Update(Entity.Null);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AchievedMilestoneBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MaxMilestoneReachedBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_TotalXPBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_AchievedMilestoneXPBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_NextMilestoneXPBinding.Update();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MilestoneReachedEventQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.PublishReachedMilestones();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MilestoneReachedEventQuery.IsEmptyIgnoreFilter || !this.m_ModifiedMilestoneQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MilestonesBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_MilestoneDetailsBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_XPSystem.TransferMessages((IXPMessageHandler) this);
    }

    public void AddMessage(XPMessage message)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_XpMessageAddedBinding.active)
        return;
      // ISSUE: reference to a compiler-generated field
      IJsonWriter jsonWriter = this.m_XpMessageAddedBinding.EventBegin();
      jsonWriter.TypeBegin("milestone.XPMessage");
      jsonWriter.PropertyName("amount");
      jsonWriter.Write(message.amount);
      jsonWriter.PropertyName("reason");
      jsonWriter.Write(Enum.GetName(typeof (XPReason), (object) message.reason));
      jsonWriter.TypeEnd();
      // ISSUE: reference to a compiler-generated field
      this.m_XpMessageAddedBinding.EventEnd();
    }

    private int GetAchievedMilestone()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_MilestoneLevelQuery.IsEmptyIgnoreFilter ? 0 : this.m_MilestoneLevelQuery.GetSingleton<MilestoneLevel>().m_AchievedMilestone;
    }

    private bool IsMaxMilestoneReached() => this.m_LockedMilestoneQuery.IsEmpty;

    private int GetAchievedMilestoneXP() => this.m_XpMilestoneSystem.lastRequiredXP;

    private int GetNextMilestoneXP() => this.m_XpMilestoneSystem.nextRequiredXP;

    private int GetTotalXP() => this.m_CitySystem.XP;

    private void PublishReachedMilestones()
    {
      // ISSUE: reference to a compiler-generated field
      Entity milestone = this.m_UnlockedMilestoneBinding.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int milestoneIndex = this.GetMilestoneIndex(this.m_UnlockedMilestoneBinding.value);
      // ISSUE: reference to a compiler-generated field
      NativeArray<MilestoneReachedEvent> componentDataArray = this.m_MilestoneReachedEventQuery.ToComponentDataArray<MilestoneReachedEvent>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        if (componentDataArray[index].m_Index > milestoneIndex)
        {
          milestone = componentDataArray[index].m_Milestone;
          milestoneIndex = componentDataArray[index].m_Index;
        }
      }
      componentDataArray.Dispose();
      Game.PSI.Telemetry.MilestoneUnlocked(milestoneIndex);
      PlatformManager.instance.IndicateAchievementProgress(Game.Achievements.Achievements.TheLastMileMarker, milestoneIndex, IndicateType.Absolute);
      // ISSUE: reference to a compiler-generated field
      if (!SharedSettings.instance.userInterface.blockingPopupsEnabled || this.m_CityConfigurationSystem.unlockAll)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedMilestoneBinding.Update(milestone);
    }

    private int GetMilestoneIndex(Entity milestoneEntity)
    {
      MilestoneData component;
      return !(milestoneEntity != Entity.Null) || !this.EntityManager.TryGetComponent<MilestoneData>(milestoneEntity, out component) ? -1 : component.m_Index;
    }

    private void BindMilestones(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      NativeArray<MilestoneUISystem.ComparableMilestone> sortedMilestones = this.GetSortedMilestones(Allocator.TempJob);
      writer.ArrayBegin(sortedMilestones.Length);
      for (int index = 0; index < sortedMilestones.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = sortedMilestones[index].m_Entity;
        // ISSUE: reference to a compiler-generated field
        MilestoneData data = sortedMilestones[index].m_Data;
        writer.TypeBegin("milestone.Milestone");
        writer.PropertyName("entity");
        writer.Write(entity);
        writer.PropertyName("index");
        writer.Write(data.m_Index);
        writer.PropertyName("major");
        writer.Write(data.m_Major);
        writer.PropertyName("locked");
        writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity));
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      sortedMilestones.Dispose();
    }

    private NativeArray<MilestoneUISystem.ComparableMilestone> GetSortedMilestones(
      Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_MilestoneQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<MilestoneData> componentDataArray = this.m_MilestoneQuery.ToComponentDataArray<MilestoneData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<MilestoneUISystem.ComparableMilestone> array = new NativeArray<MilestoneUISystem.ComparableMilestone>(componentDataArray.Length, allocator);
      for (int index = 0; index < componentDataArray.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        array[index] = new MilestoneUISystem.ComparableMilestone()
        {
          m_Entity = entityArray[index],
          m_Data = componentDataArray[index]
        };
      }
      array.Sort<MilestoneUISystem.ComparableMilestone>();
      entityArray.Dispose();
      componentDataArray.Dispose();
      return array;
    }

    private void BindMilestoneDetails(IJsonWriter writer, Entity milestone)
    {
      Entity entity = milestone;
      MilestoneData component;
      if (entity != Entity.Null && this.EntityManager.TryGetComponent<MilestoneData>(entity, out component))
      {
        bool flag = this.EntityManager.HasEnabledComponent<Locked>(entity);
        writer.TypeBegin("milestone.MilestoneDetails");
        writer.PropertyName("entity");
        writer.Write(milestone);
        writer.PropertyName("index");
        writer.Write(component.m_Index);
        writer.PropertyName("xpRequirement");
        writer.Write(component.m_XpRequried);
        writer.PropertyName("reward");
        writer.Write(component.m_Reward);
        writer.PropertyName("devTreePoints");
        writer.Write(component.m_DevTreePoints);
        writer.PropertyName("mapTiles");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CityConfigurationSystem.unlockMapTiles ? 0 : component.m_MapTiles);
        writer.PropertyName("loanLimit");
        writer.Write(component.m_LoanLimit);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        MilestonePrefab prefab = this.m_PrefabSystem.GetPrefab<MilestonePrefab>(entity);
        writer.PropertyName("image");
        writer.Write(prefab.m_Image);
        writer.PropertyName("backgroundColor");
        writer.Write(prefab.m_BackgroundColor);
        writer.PropertyName("accentColor");
        writer.Write(prefab.m_AccentColor);
        writer.PropertyName("textColor");
        writer.Write(prefab.m_TextColor);
        writer.PropertyName("locked");
        writer.Write(flag);
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    private void BindMilestoneUnlocks(IJsonWriter writer, Entity milestoneEntity)
    {
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedDevTreeServices = this.GetUnlockedDevTreeServices(milestoneEntity, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedZones = this.GetUnlockedZones(milestoneEntity, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedAssets = this.GetUnlockedAssets(milestoneEntity, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<UIObjectInfo> unlockedFeatures = this.GetSortedUnlockedFeatures(milestoneEntity, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<MilestoneUISystem.ServiceInfo> sortedServices = this.GetSortedServices(unlockedDevTreeServices, unlockedAssets, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<MilestoneUISystem.AssetInfo> sortedZones = this.GetSortedZones(unlockedZones, Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<UIObjectInfo> sortedPolicies = this.GetSortedPolicies(milestoneEntity, Allocator.TempJob);
      NativeList<MilestoneUISystem.AssetInfo> result = new NativeList<MilestoneUISystem.AssetInfo>(20, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> assetThemes = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      writer.ArrayBegin(unlockedFeatures.Length + sortedZones.Length + sortedServices.Length + sortedPolicies.Length);
      for (int index = 0; index < unlockedFeatures.Length; ++index)
      {
        UIObjectInfo uiObjectInfo = unlockedFeatures[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        FeaturePrefab prefab = this.m_PrefabSystem.GetPrefab<FeaturePrefab>(uiObjectInfo.prefabData);
        UIObject component = prefab.GetComponent<UIObject>();
        writer.TypeBegin("milestone.Feature");
        writer.PropertyName("entity");
        writer.Write(uiObjectInfo.entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        writer.Write(component.m_Icon);
        writer.TypeEnd();
      }
      for (int index = 0; index < sortedZones.Length; ++index)
      {
        // ISSUE: variable of a compiler-generated type
        MilestoneUISystem.AssetInfo asset = sortedZones[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(asset.m_PrefabData);
        // ISSUE: reference to a compiler-generated method
        this.BindAsset(writer, asset, prefab, assetThemes);
      }
      for (int index1 = 0; index1 < sortedServices.Length; ++index1)
      {
        // ISSUE: variable of a compiler-generated type
        MilestoneUISystem.ServiceInfo serviceInfo = sortedServices[index1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ServicePrefab prefab1 = this.m_PrefabSystem.GetPrefab<ServicePrefab>(serviceInfo.m_PrefabData);
        UIObject component = prefab1.GetComponent<UIObject>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.FilterAndSortAssets(result, serviceInfo.m_Entity, unlockedAssets);
        writer.TypeBegin("milestone.Service");
        writer.PropertyName("entity");
        // ISSUE: reference to a compiler-generated field
        writer.Write(serviceInfo.m_Entity);
        writer.PropertyName("name");
        writer.Write(prefab1.name);
        writer.PropertyName("icon");
        writer.Write(component.m_Icon);
        writer.PropertyName("devTreeUnlocked");
        // ISSUE: reference to a compiler-generated field
        writer.Write(serviceInfo.m_DevTreeUnlocked);
        writer.PropertyName("assets");
        writer.ArrayBegin(result.Length);
        for (int index2 = 0; index2 < result.Length; ++index2)
        {
          // ISSUE: variable of a compiler-generated type
          MilestoneUISystem.AssetInfo asset = result[index2];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(asset.m_PrefabData);
          // ISSUE: reference to a compiler-generated method
          this.BindAsset(writer, asset, prefab2, assetThemes);
        }
        writer.ArrayEnd();
        writer.TypeEnd();
      }
      for (int index = 0; index < sortedPolicies.Length; ++index)
      {
        UIObjectInfo uiObjectInfo = sortedPolicies[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PolicyPrefab prefab = this.m_PrefabSystem.GetPrefab<PolicyPrefab>(uiObjectInfo.prefabData);
        UIObject component = prefab.GetComponent<UIObject>();
        writer.TypeBegin("milestone.Policy");
        writer.PropertyName("entity");
        writer.Write(uiObjectInfo.entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        writer.Write(component.m_Icon);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      unlockedDevTreeServices.Dispose();
      unlockedAssets.Dispose();
      unlockedZones.Dispose();
      unlockedFeatures.Dispose();
      sortedServices.Dispose();
      result.Dispose();
      sortedZones.Dispose();
      sortedPolicies.Dispose();
      assetThemes.Dispose();
    }

    private NativeList<Entity> GetUnlockedZones(Entity milestoneEntity, Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_UnlockableZoneQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedZones = this.FilterUnlockedPrefabs(entityArray, milestoneEntity, allocator);
      entityArray.Dispose();
      return unlockedZones;
    }

    private NativeList<MilestoneUISystem.AssetInfo> GetSortedZones(
      NativeList<Entity> unlockedZones,
      Allocator allocator)
    {
      NativeList<MilestoneUISystem.AssetInfo> list = new NativeList<MilestoneUISystem.AssetInfo>(10, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < unlockedZones.Length; ++index)
      {
        Entity unlockedZone = unlockedZones[index];
        EntityManager entityManager = this.EntityManager;
        PrefabData componentData1 = entityManager.GetComponentData<PrefabData>(unlockedZone);
        entityManager = this.EntityManager;
        UIObjectData componentData2 = entityManager.GetComponentData<UIObjectData>(unlockedZone);
        int num = int.MinValue;
        UIObjectData component;
        if (componentData2.m_Group != Entity.Null && this.EntityManager.TryGetComponent<UIObjectData>(componentData2.m_Group, out component))
          num = component.m_Priority;
        // ISSUE: object of a compiler-generated type is created
        list.Add(new MilestoneUISystem.AssetInfo()
        {
          m_Entity = unlockedZone,
          m_PrefabData = componentData1,
          m_UIPriority1 = num,
          m_UIPriority2 = componentData2.m_Priority
        });
      }
      list.Sort<MilestoneUISystem.AssetInfo>();
      return list;
    }

    private void BindAsset(
      IJsonWriter writer,
      MilestoneUISystem.AssetInfo asset,
      PrefabBase assetPrefab,
      NativeList<Entity> assetThemes)
    {
      writer.TypeBegin("milestone.Asset");
      writer.PropertyName("entity");
      // ISSUE: reference to a compiler-generated field
      writer.Write(asset.m_Entity);
      writer.PropertyName("name");
      writer.Write(assetPrefab.name);
      writer.PropertyName("icon");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      writer.Write(ImageSystem.GetThumbnail(assetPrefab) ?? this.m_ImageSystem.placeholderIcon);
      writer.PropertyName("themes");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.GetThemes(assetThemes, asset.m_Entity);
      writer.ArrayBegin(assetThemes.Length);
      for (int index = 0; index < assetThemes.Length; ++index)
        writer.Write(assetThemes[index]);
      writer.ArrayEnd();
      writer.TypeEnd();
    }

    private NativeList<Entity> GetUnlockedDevTreeServices(
      Entity milestoneEntity,
      Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<DevTreeNodeData> componentDataArray = this.m_DevTreeNodeQuery.ToComponentDataArray<DevTreeNodeData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray.Length; ++index)
        nativeParallelHashSet.Add(componentDataArray[index].m_Service);
      NativeArray<Entity> nativeArray = nativeParallelHashSet.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedDevTreeServices = this.FilterUnlockedPrefabs(nativeArray, milestoneEntity, allocator);
      componentDataArray.Dispose();
      nativeParallelHashSet.Dispose();
      nativeArray.Dispose();
      return unlockedDevTreeServices;
    }

    private NativeList<Entity> GetUnlockedAssets(Entity milestoneEntity, Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_UnlockableAssetQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> unlockedAssets = this.FilterUnlockedPrefabs(entityArray, milestoneEntity, allocator);
      entityArray.Dispose();
      return unlockedAssets;
    }

    private NativeList<UIObjectInfo> GetSortedPolicies(Entity milestoneEntity, Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_UnlockablePolicyQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> entities = this.FilterUnlockedPrefabs(entityArray, milestoneEntity, Allocator.TempJob);
      NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, entities, allocator);
      entityArray.Dispose();
      entities.Dispose();
      return sortedObjects;
    }

    private NativeList<Entity> FilterUnlockedPrefabs(
      NativeArray<Entity> prefabs,
      Entity milestoneEntity,
      Allocator allocator)
    {
      NativeParallelHashMap<Entity, UnlockFlags> requiredPrefabs = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>(20, (AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < prefabs.Length; ++index)
      {
        Entity prefab = prefabs[index];
        requiredPrefabs.Clear();
        ProgressionUtils.CollectSubRequirements(this.EntityManager, prefab, requiredPrefabs);
        Entity key = Entity.Null;
        int num = -1;
        foreach (KeyValue<Entity, UnlockFlags> keyValue in requiredPrefabs)
        {
          if ((keyValue.Value & UnlockFlags.RequireAll) != (UnlockFlags) 0)
          {
            if (this.EntityManager.HasComponent<DevTreeNodeData>(keyValue.Key) || this.EntityManager.HasComponent<UnlockRequirementData>(keyValue.Key))
            {
              key = Entity.Null;
              break;
            }
            MilestoneData component;
            if (this.EntityManager.TryGetComponent<MilestoneData>(keyValue.Key, out component) && component.m_Index > num)
            {
              key = keyValue.Key;
              num = component.m_Index;
            }
          }
        }
        if (key == milestoneEntity && key != Entity.Null)
          nativeList.Add(in prefab);
      }
      requiredPrefabs.Dispose();
      return nativeList;
    }

    private NativeList<UIObjectInfo> GetSortedUnlockedFeatures(
      Entity milestoneEntity,
      Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_UnlockableFeatureQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated method
      NativeList<Entity> entities = this.FilterUnlockedPrefabs(entityArray, milestoneEntity, Allocator.TempJob);
      NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, entities, allocator);
      entityArray.Dispose();
      entities.Dispose();
      // ISSUE: reference to a compiler-generated field
      if (this.m_CityConfigurationSystem.unlockMapTiles)
      {
        int index1 = -1;
        for (int index2 = 0; index2 < sortedObjects.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.m_PrefabSystem.GetPrefab<PrefabBase>(sortedObjects[index2].prefabData).name == "Map Tiles")
          {
            index1 = index2;
            break;
          }
        }
        if (index1 != -1)
          sortedObjects.RemoveAt(index1);
      }
      return sortedObjects;
    }

    private NativeList<MilestoneUISystem.ServiceInfo> GetSortedServices(
      NativeList<Entity> unlockedDevTreeServices,
      NativeList<Entity> unlockedAssets,
      Allocator allocator)
    {
      NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<MilestoneUISystem.ServiceInfo> list = new NativeList<MilestoneUISystem.ServiceInfo>(10, (AllocatorManager.AllocatorHandle) allocator);
      // ISSUE: variable of a compiler-generated type
      MilestoneUISystem.ServiceInfo serviceInfo;
      for (int index = 0; index < unlockedDevTreeServices.Length; ++index)
      {
        Entity unlockedDevTreeService = unlockedDevTreeServices[index];
        UIObjectData component;
        if (nativeParallelHashSet.Add(unlockedDevTreeService) && this.EntityManager.TryGetComponent<UIObjectData>(unlockedDevTreeService, out component))
        {
          PrefabData componentData = this.EntityManager.GetComponentData<PrefabData>(unlockedDevTreeService);
          ref NativeList<MilestoneUISystem.ServiceInfo> local1 = ref list;
          // ISSUE: object of a compiler-generated type is created
          serviceInfo = new MilestoneUISystem.ServiceInfo();
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_Entity = unlockedDevTreeService;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_PrefabData = componentData;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_UIPriority = component.m_Priority;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_DevTreeUnlocked = true;
          ref MilestoneUISystem.ServiceInfo local2 = ref serviceInfo;
          local1.Add(in local2);
        }
      }
      for (int index = 0; index < unlockedAssets.Length; ++index)
      {
        Entity service = this.EntityManager.GetComponentData<ServiceObjectData>(unlockedAssets[index]).m_Service;
        UIObjectData component;
        if (nativeParallelHashSet.Add(service) && this.EntityManager.TryGetComponent<UIObjectData>(service, out component))
        {
          PrefabData componentData = this.EntityManager.GetComponentData<PrefabData>(service);
          ref NativeList<MilestoneUISystem.ServiceInfo> local3 = ref list;
          // ISSUE: object of a compiler-generated type is created
          serviceInfo = new MilestoneUISystem.ServiceInfo();
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_Entity = service;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_PrefabData = componentData;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_UIPriority = component.m_Priority;
          // ISSUE: reference to a compiler-generated field
          serviceInfo.m_DevTreeUnlocked = false;
          ref MilestoneUISystem.ServiceInfo local4 = ref serviceInfo;
          local3.Add(in local4);
        }
      }
      list.Sort<MilestoneUISystem.ServiceInfo>();
      nativeParallelHashSet.Dispose();
      return list;
    }

    private void FilterAndSortAssets(
      NativeList<MilestoneUISystem.AssetInfo> result,
      Entity serviceEntity,
      NativeList<Entity> unlockedAssets)
    {
      result.Clear();
      for (int index = 0; index < unlockedAssets.Length; ++index)
      {
        Entity unlockedAsset = unlockedAssets[index];
        EntityManager entityManager = this.EntityManager;
        if (entityManager.GetComponentData<ServiceObjectData>(unlockedAsset).m_Service == serviceEntity)
        {
          entityManager = this.EntityManager;
          PrefabData componentData1 = entityManager.GetComponentData<PrefabData>(unlockedAsset);
          entityManager = this.EntityManager;
          UIObjectData componentData2 = entityManager.GetComponentData<UIObjectData>(unlockedAsset);
          int num = int.MinValue;
          UIObjectData component;
          if (componentData2.m_Group != Entity.Null && this.EntityManager.TryGetComponent<UIObjectData>(componentData2.m_Group, out component))
            num = component.m_Priority;
          // ISSUE: object of a compiler-generated type is created
          result.Add(new MilestoneUISystem.AssetInfo()
          {
            m_Entity = unlockedAsset,
            m_PrefabData = componentData1,
            m_UIPriority1 = num,
            m_UIPriority2 = componentData2.m_Priority
          });
        }
      }
      result.Sort<MilestoneUISystem.AssetInfo>();
    }

    private void GetThemes(NativeList<Entity> result, Entity assetEntity)
    {
      result.Clear();
      DynamicBuffer<ObjectRequirementElement> buffer;
      if (!this.EntityManager.TryGetBuffer<ObjectRequirementElement>(assetEntity, true, out buffer))
        return;
      foreach (ObjectRequirementElement requirementElement in buffer)
      {
        if (this.EntityManager.HasComponent<ThemeData>(requirementElement.m_Requirement))
          result.Add(in requirementElement.m_Requirement);
      }
    }

    private void BindUnlockDetails(IJsonWriter writer, Entity unlockEntity)
    {
      if (unlockEntity != Entity.Null)
      {
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<PrefabData>(unlockEntity))
        {
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<UIObjectData>(unlockEntity))
          {
            bool locked = this.EntityManager.HasEnabledComponent<Locked>(unlockEntity);
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<FeatureData>(unlockEntity))
            {
              // ISSUE: reference to a compiler-generated method
              this.BindFeatureUnlock(writer, unlockEntity, locked);
              return;
            }
            entityManager = this.EntityManager;
            if (entityManager.HasComponent<ServiceData>(unlockEntity))
            {
              // ISSUE: reference to a compiler-generated method
              this.BindServiceUnlock(writer, unlockEntity, locked);
              return;
            }
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<ServiceObjectData>(unlockEntity))
            {
              entityManager = this.EntityManager;
              if (!entityManager.HasComponent<PlaceableObjectData>(unlockEntity))
              {
                entityManager = this.EntityManager;
                if (!entityManager.HasComponent<ZoneData>(unlockEntity))
                {
                  entityManager = this.EntityManager;
                  if (entityManager.HasComponent<PolicyData>(unlockEntity))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.BindPolicyUnlock(writer, unlockEntity, locked);
                    return;
                  }
                  writer.WriteNull();
                  return;
                }
              }
            }
            // ISSUE: reference to a compiler-generated method
            this.BindAssetUnlock(writer, unlockEntity, locked);
            return;
          }
        }
      }
      writer.WriteNull();
    }

    private void BindFeatureUnlock(IJsonWriter writer, Entity featureEntity, bool locked)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      FeaturePrefab prefab = this.m_PrefabSystem.GetPrefab<FeaturePrefab>(featureEntity);
      UIObject component = prefab.GetComponent<UIObject>();
      writer.TypeBegin("milestone.UnlockDetails");
      writer.PropertyName("entity");
      writer.Write(featureEntity);
      writer.PropertyName("icon");
      writer.Write(component.m_Icon);
      writer.PropertyName("titleId");
      writer.Write("Assets.NAME[" + prefab.name + "]");
      writer.PropertyName("descriptionId");
      writer.Write("Assets.DESCRIPTION[" + prefab.name + "]");
      writer.PropertyName(nameof (locked));
      writer.Write(locked);
      writer.PropertyName("hasDevTree");
      writer.Write(false);
      writer.TypeEnd();
    }

    private void BindServiceUnlock(IJsonWriter writer, Entity serviceEntity, bool locked)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ServicePrefab prefab = this.m_PrefabSystem.GetPrefab<ServicePrefab>(serviceEntity);
      UIObject component = prefab.GetComponent<UIObject>();
      writer.TypeBegin("milestone.UnlockDetails");
      writer.PropertyName("entity");
      writer.Write(serviceEntity);
      writer.PropertyName("icon");
      writer.Write(component.m_Icon);
      writer.PropertyName("titleId");
      writer.Write("Services.NAME[" + prefab.name + "]");
      writer.PropertyName("descriptionId");
      writer.Write("Services.DESCRIPTION[" + prefab.name + "]");
      writer.PropertyName(nameof (locked));
      writer.Write(locked);
      writer.PropertyName("hasDevTree");
      // ISSUE: reference to a compiler-generated method
      writer.Write(this.HasDevTree(serviceEntity));
      writer.TypeEnd();
    }

    private bool HasDevTree(Entity serviceEntity)
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<DevTreeNodeData> componentDataArray = this.m_DevTreeNodeQuery.ToComponentDataArray<DevTreeNodeData>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          if (componentDataArray[index].m_Service == serviceEntity)
            return true;
        }
        return false;
      }
    }

    private void BindAssetUnlock(IJsonWriter writer, Entity assetEntity, bool locked)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(assetEntity);
      writer.TypeBegin("milestone.UnlockDetails");
      writer.PropertyName("entity");
      writer.Write(assetEntity);
      writer.PropertyName("icon");
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      writer.Write(ImageSystem.GetThumbnail(prefab) ?? this.m_ImageSystem.placeholderIcon);
      writer.PropertyName("titleId");
      writer.Write("Assets.NAME[" + prefab.name + "]");
      writer.PropertyName("descriptionId");
      writer.Write("Assets.DESCRIPTION[" + prefab.name + "]");
      writer.PropertyName(nameof (locked));
      writer.Write(locked);
      writer.PropertyName("hasDevTree");
      writer.Write(false);
      writer.TypeEnd();
    }

    private void BindPolicyUnlock(IJsonWriter writer, Entity policyEntity, bool locked)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(policyEntity);
      UIObject component = prefab.GetComponent<UIObject>();
      writer.TypeBegin("milestone.UnlockDetails");
      writer.PropertyName("entity");
      writer.Write(policyEntity);
      writer.PropertyName("icon");
      writer.Write(component.m_Icon);
      writer.PropertyName("titleId");
      writer.Write("Policy.TITLE[" + prefab.name + "]");
      writer.PropertyName("descriptionId");
      writer.Write("Policy.DESCRIPTION[" + prefab.name + "]");
      writer.PropertyName(nameof (locked));
      writer.Write(locked);
      writer.PropertyName("hasDevTree");
      writer.Write(false);
      writer.TypeEnd();
    }

    [Preserve]
    public MilestoneUISystem()
    {
    }

    private struct ComparableMilestone : IComparable<MilestoneUISystem.ComparableMilestone>
    {
      public Entity m_Entity;
      public MilestoneData m_Data;

      public int CompareTo(MilestoneUISystem.ComparableMilestone other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Data.m_Index.CompareTo(other.m_Data.m_Index);
      }
    }

    private struct ServiceInfo : IComparable<MilestoneUISystem.ServiceInfo>
    {
      public Entity m_Entity;
      public PrefabData m_PrefabData;
      public int m_UIPriority;
      public bool m_DevTreeUnlocked;

      public int CompareTo(MilestoneUISystem.ServiceInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_UIPriority.CompareTo(other.m_UIPriority);
      }
    }

    private struct AssetInfo : IComparable<MilestoneUISystem.AssetInfo>
    {
      public Entity m_Entity;
      public PrefabData m_PrefabData;
      public int m_UIPriority1;
      public int m_UIPriority2;

      public int CompareTo(MilestoneUISystem.AssetInfo other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = this.m_UIPriority1.CompareTo(other.m_UIPriority1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return num != 0 ? num : this.m_UIPriority2.CompareTo(other.m_UIPriority2);
      }
    }
  }
}
