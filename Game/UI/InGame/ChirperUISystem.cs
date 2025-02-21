// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ChirperUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using Game.Tools;
using Game.Triggers;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ChirperUISystem : UISystemBase
  {
    private const string kGroup = "chirper";
    private const int kBrandIconSize = 32;
    private PrefabSystem m_PrefabSystem;
    private SelectedInfoUISystem m_SelectedInfoUISystem;
    private InfoviewsUISystem m_InfoviewsUISystem;
    private ChirpLinkSystem m_ChirpLinkSystem;
    private NameSystem m_NameSystem;
    private EntityQuery m_ChirpQuery;
    private EntityQuery m_ModifiedChirpQuery;
    private EntityQuery m_CreatedChirpQuery;
    private EntityQuery m_TimeDataQuery;
    private EndFrameBarrier m_EndFrameBarrier;
    private RawValueBinding m_ChirpsBinding;
    private RawEventBinding m_ChirpAddedBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewsUISystem = this.World.GetOrCreateSystemManaged<InfoviewsUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpLinkSystem = this.World.GetOrCreateSystemManaged<ChirpLinkSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ChirpQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Triggers.Chirp>(),
          ComponentType.ReadOnly<RandomLocalizationIndex>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedChirpQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Triggers.Chirp>(),
          ComponentType.ReadOnly<PrefabRef>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedChirpQuery.AddOrderVersionFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedChirpQuery.AddChangedVersionFilter(ComponentType.ReadOnly<Game.Triggers.Chirp>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedChirpQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Triggers.Chirp>(),
          ComponentType.ReadOnly<RandomLocalizationIndex>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Created>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_TimeDataQuery = this.GetEntityQuery(ComponentType.ReadOnly<TimeData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ChirpsBinding = new RawValueBinding("chirper", "chirps", (Action<IJsonWriter>) (binder =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<Entity> sortedChirps = this.GetSortedChirps(this.m_ChirpQuery);
        binder.ArrayBegin(sortedChirps.Length);
        for (int index = 0; index < sortedChirps.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindChirp(binder, sortedChirps[index]);
        }
        binder.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ChirpAddedBinding = new RawEventBinding("chirper", "chirpAdded")));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("chirper", "addLike", (Action<Entity>) (entity =>
      {
        Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(entity);
        componentData.m_Flags |= ChirpFlags.Liked;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.SetComponent<Game.Triggers.Chirp>(entity, componentData);
        commandBuffer.AddComponent<Updated>(entity, new Updated());
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("chirper", "removeLike", (Action<Entity>) (entity =>
      {
        Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(entity);
        componentData.m_Flags &= ~ChirpFlags.Liked;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.SetComponent<Game.Triggers.Chirp>(entity, componentData);
        commandBuffer.AddComponent<Updated>(entity, new Updated());
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("chirper", "selectLink", (Action<string>) (target =>
      {
        Entity entity1;
        EntityManager entityManager;
        if (URI.TryParseEntity(target, out entity1))
        {
          entityManager = this.EntityManager;
          if (entityManager.Exists(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_SelectedInfoUISystem.SetSelection(entity1);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_SelectedInfoUISystem.Focus(entity1);
          }
        }
        Entity entity2;
        if (!URI.TryParseInfoview(target, out entity2))
          return;
        entityManager = this.EntityManager;
        if (!entityManager.Exists(entity2))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoviewsUISystem.SetActiveInfoview(entity2);
      })));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ModifiedChirpQuery.IsEmpty)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ChirpsBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ChirpAddedBinding.active || this.m_CreatedChirpQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.PublishAddedChirps();
    }

    private void AddLike(Entity entity)
    {
      Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(entity);
      componentData.m_Flags |= ChirpFlags.Liked;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      commandBuffer.SetComponent<Game.Triggers.Chirp>(entity, componentData);
      commandBuffer.AddComponent<Updated>(entity, new Updated());
    }

    private void RemoveLike(Entity entity)
    {
      Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(entity);
      componentData.m_Flags &= ~ChirpFlags.Liked;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      commandBuffer.SetComponent<Game.Triggers.Chirp>(entity, componentData);
      commandBuffer.AddComponent<Updated>(entity, new Updated());
    }

    private void SelectLink(string target)
    {
      Entity entity1;
      EntityManager entityManager;
      if (URI.TryParseEntity(target, out entity1))
      {
        entityManager = this.EntityManager;
        if (entityManager.Exists(entity1))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_SelectedInfoUISystem.SetSelection(entity1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_SelectedInfoUISystem.Focus(entity1);
        }
      }
      Entity entity2;
      if (!URI.TryParseInfoview(target, out entity2))
        return;
      entityManager = this.EntityManager;
      if (!entityManager.Exists(entity2))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoviewsUISystem.SetActiveInfoview(entity2);
    }

    private void UpdateChirps(IJsonWriter binder)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<Entity> sortedChirps = this.GetSortedChirps(this.m_ChirpQuery);
      binder.ArrayBegin(sortedChirps.Length);
      for (int index = 0; index < sortedChirps.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindChirp(binder, sortedChirps[index]);
      }
      binder.ArrayEnd();
    }

    private void PublishAddedChirps()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<Entity> sortedChirps = this.GetSortedChirps(this.m_CreatedChirpQuery);
      int length = sortedChirps.Length;
      for (int index = 0; index < length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.BindChirp(this.m_ChirpAddedBinding.EventBegin(), sortedChirps[index], true);
        // ISSUE: reference to a compiler-generated field
        this.m_ChirpAddedBinding.EventEnd();
      }
    }

    private NativeArray<Entity> GetSortedChirps(EntityQuery chirpQuery)
    {
      NativeArray<Entity> entityArray = chirpQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: object of a compiler-generated type is created
      entityArray.Sort<Entity, ChirperUISystem.ChirpComparer>(new ChirperUISystem.ChirpComparer(this.EntityManager));
      return entityArray;
    }

    public void BindChirp(IJsonWriter binder, Entity chirpEntity, bool newChirp = false)
    {
      // ISSUE: reference to a compiler-generated method
      string messageId = this.GetMessageID(chirpEntity);
      Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(chirpEntity);
      binder.TypeBegin("chirper.Chirp");
      binder.PropertyName("entity");
      binder.Write(chirpEntity);
      binder.PropertyName("sender");
      // ISSUE: reference to a compiler-generated method
      this.BindChirpSender(binder, chirpEntity);
      if (newChirp)
      {
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<ChirperAccountData>(componentData.m_Sender))
        {
          entityManager = this.EntityManager;
          Game.PSI.Telemetry.Chirp(entityManager.GetComponentData<PrefabRef>(chirpEntity).m_Prefab, componentData.m_Likes);
        }
      }
      binder.PropertyName("date");
      // ISSUE: reference to a compiler-generated method
      binder.Write(this.GetTicks(componentData.m_CreationFrame));
      binder.PropertyName("messageId");
      binder.Write(messageId);
      binder.PropertyName("links");
      DynamicBuffer<ChirpEntity> buffer;
      if (this.EntityManager.TryGetBuffer<ChirpEntity>(chirpEntity, true, out buffer))
      {
        int length = buffer.Length;
        binder.ArrayBegin(length);
        for (int linkIndex = 0; linkIndex < length; ++linkIndex)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindChirpLink(binder, chirpEntity, linkIndex);
        }
        binder.ArrayEnd();
      }
      else
        binder.WriteEmptyArray();
      binder.PropertyName("likes");
      binder.Write(componentData.m_Likes);
      binder.PropertyName("liked");
      binder.Write((componentData.m_Flags & ChirpFlags.Liked) != 0);
      binder.TypeEnd();
    }

    public string GetMessageID(Entity chirp)
    {
      PrefabRef component1;
      DynamicBuffer<RandomLocalizationIndex> buffer;
      PrefabBase prefab;
      RandomLocalization component2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.EntityManager.TryGetComponent<PrefabRef>(chirp, out component1) && this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(chirp, true, out buffer) && buffer.Length > 0 && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component1.m_Prefab, out prefab) && prefab.TryGet<RandomLocalization>(out component2) ? LocalizationUtils.AppendIndex(component2.m_LocalizationID, buffer[0]) : string.Empty;
    }

    private void BindChirpSender(IJsonWriter binder, Entity entity)
    {
      Game.Triggers.Chirp componentData = this.EntityManager.GetComponentData<Game.Triggers.Chirp>(entity);
      binder.TypeBegin("chirper.ChirpSender");
      if (this.EntityManager.Exists(componentData.m_Sender))
      {
        binder.PropertyName(nameof (entity));
        binder.Write(componentData.m_Sender);
        binder.PropertyName("link");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.BindChirpLink(binder, componentData.m_Sender, this.m_NameSystem.GetName(componentData.m_Sender));
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        ChirpLinkSystem.CachedChirpData data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_ChirpLinkSystem.TryGetData(entity, out data))
        {
          binder.PropertyName(nameof (entity));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          binder.Write(data.m_Sender.m_Entity);
          binder.PropertyName("link");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.BindChirpLink(binder, Entity.Null, data.m_Sender.m_Name);
        }
        else
        {
          binder.PropertyName(nameof (entity));
          binder.Write(Entity.Null);
          binder.PropertyName("link");
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.BindChirpLink(binder, Entity.Null, NameSystem.Name.CustomName(string.Empty));
        }
      }
      binder.TypeEnd();
    }

    private void BindChirpLink(IJsonWriter binder, Entity entity, int linkIndex)
    {
      DynamicBuffer<ChirpEntity> buffer = this.EntityManager.GetBuffer<ChirpEntity>(entity, true);
      if (this.EntityManager.Exists(buffer[linkIndex].m_Entity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.BindChirpLink(binder, buffer[linkIndex].m_Entity, this.m_NameSystem.GetName(buffer[linkIndex].m_Entity, true));
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        ChirpLinkSystem.CachedChirpData data;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        if (this.m_ChirpLinkSystem.TryGetData(entity, out data) && data.m_Links.Length > linkIndex)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.BindChirpLink(binder, Entity.Null, data.m_Links[linkIndex].m_Name);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.BindChirpLink(binder, Entity.Null, NameSystem.Name.CustomName(string.Empty));
        }
      }
    }

    public void BindChirpLink(IJsonWriter binder, Entity entity, NameSystem.Name name)
    {
      binder.TypeBegin("chirper.ChirpLink");
      binder.PropertyName(nameof (name));
      binder.Write<NameSystem.Name>(name);
      binder.PropertyName("target");
      PropertyRenter component;
      if (this.EntityManager.HasComponent<CompanyData>(entity) && this.EntityManager.TryGetComponent<PropertyRenter>(entity, out component))
        entity = component.m_Property;
      string str = entity != Entity.Null ? URI.FromEntity(entity) : string.Empty;
      if (this.EntityManager.HasComponent<ChirperAccountData>(entity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        str = URI.FromInfoView(this.m_PrefabSystem.GetEntity((PrefabBase) this.m_PrefabSystem.GetPrefab<ChirperAccount>(entity).m_InfoView));
      }
      binder.Write(str);
      binder.TypeEnd();
    }

    [CanBeNull]
    private string GetAvatar(Entity chirpEntity)
    {
      Game.Triggers.Chirp component1;
      if (this.EntityManager.TryGetComponent<Game.Triggers.Chirp>(chirpEntity, out component1))
      {
        Entity entity = component1.m_Sender;
        CompanyData component2;
        if (this.EntityManager.TryGetComponent<CompanyData>(entity, out component2))
          entity = component2.m_Brand;
        PrefabData component3;
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.EntityManager.TryGetComponent<PrefabData>(entity, out component3) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component3, out prefab))
        {
          // ISSUE: reference to a compiler-generated method
          string icon = ImageSystem.GetIcon(prefab);
          if (icon != null)
            return icon;
          switch (prefab)
          {
            case ChirperAccount chirperAccount when (UnityEngine.Object) chirperAccount.m_InfoView != (UnityEngine.Object) null:
              return chirperAccount.m_InfoView.m_IconPath;
            case BrandPrefab brandPrefab:
              return string.Format("{0}?width={1}&height={2}", (object) brandPrefab.thumbnailUrl, (object) 32, (object) 32);
          }
        }
      }
      return (string) null;
    }

    private int GetRandomIndex(Entity chirpEntity)
    {
      Game.Triggers.Chirp component;
      DynamicBuffer<RandomLocalizationIndex> buffer;
      return this.EntityManager.TryGetComponent<Game.Triggers.Chirp>(chirpEntity, out component) && this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(component.m_Sender, true, out buffer) && buffer.Length > 0 ? buffer[0].m_Index : 0;
    }

    private uint GetTicks(uint frameIndex)
    {
      // ISSUE: reference to a compiler-generated field
      return frameIndex - TimeData.GetSingleton(this.m_TimeDataQuery).m_FirstFrame;
    }

    [Preserve]
    public ChirperUISystem()
    {
    }

    private struct ChirpComparer : IComparer<Entity>
    {
      private EntityManager m_EntityManager;

      public ChirpComparer(EntityManager entityManager) => this.m_EntityManager = entityManager;

      public int Compare(Entity a, Entity b)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = -this.m_EntityManager.GetComponentData<Game.Triggers.Chirp>(a).m_CreationFrame.CompareTo(this.m_EntityManager.GetComponentData<Game.Triggers.Chirp>(b).m_CreationFrame);
        return num == 0 ? a.CompareTo(b) : num;
      }
    }
  }
}
