// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ChirpLinkSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Triggers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ChirpLinkSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private NameSystem m_NameSystem;
    private EntityQuery m_CreatedChirpQuery;
    private EntityQuery m_AllChirpsQuery;
    private EntityQuery m_DeletedChirpQuery;
    private EntityQuery m_UpdatedLinkEntityQuery;
    private EntityQuery m_DeletedLinkEntityQuery;
    private Dictionary<Entity, ChirpLinkSystem.CachedChirpData> m_CachedChirpData;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Chirp>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AllChirpsQuery = this.GetEntityQuery(ComponentType.ReadOnly<Chirp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedChirpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Chirp>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedLinkEntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<ChirpLink>(), ComponentType.ReadOnly<Updated>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedLinkEntityQuery = this.GetEntityQuery(ComponentType.ReadOnly<ChirpLink>(), ComponentType.ReadOnly<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CachedChirpData = new Dictionary<Entity, ChirpLinkSystem.CachedChirpData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CreatedChirpQuery, this.m_UpdatedLinkEntityQuery, this.m_DeletedLinkEntityQuery, this.m_DeletedChirpQuery);
    }

    public bool TryGetData(Entity chirp, out ChirpLinkSystem.CachedChirpData data)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_CachedChirpData.TryGetValue(chirp, out data);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedChirpQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CreatedChirpQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Chirp> componentDataArray = this.m_CreatedChirpQuery.ToComponentDataArray<Chirp>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index1 = 0; index1 < entityArray.Length; ++index1)
        {
          DynamicBuffer<ChirpEntity> buffer;
          if (this.EntityManager.TryGetBuffer<ChirpEntity>(entityArray[index1], true, out buffer))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_CachedChirpData[entityArray[index1]] = new ChirpLinkSystem.CachedChirpData(this.m_NameSystem, componentDataArray[index1], buffer);
            NativeArray<ChirpEntity> nativeArray = new NativeArray<ChirpEntity>(buffer.AsNativeArray(), Allocator.Temp);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              entityManager = this.EntityManager;
              if (entityManager.Exists(nativeArray[index2].m_Entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.RegisterLink(nativeArray[index2].m_Entity, entityArray[index1]);
              }
            }
            nativeArray.Dispose();
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_CachedChirpData[entityArray[index1]] = new ChirpLinkSystem.CachedChirpData(this.m_NameSystem, componentDataArray[index1]);
          }
          entityManager = this.EntityManager;
          if (entityManager.Exists(componentDataArray[index1].m_Sender))
          {
            // ISSUE: reference to a compiler-generated method
            this.RegisterLink(componentDataArray[index1].m_Sender, entityArray[index1]);
          }
        }
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_UpdatedLinkEntityQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_UpdatedLinkEntityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index3 = 0; index3 < entityArray.Length; ++index3)
        {
          entityManager = this.EntityManager;
          DynamicBuffer<ChirpLink> buffer = entityManager.GetBuffer<ChirpLink>(entityArray[index3], true);
          for (int index4 = 0; index4 < buffer.Length; ++index4)
          {
            // ISSUE: variable of a compiler-generated type
            ChirpLinkSystem.CachedChirpData cachedChirpData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CachedChirpData.TryGetValue(buffer[index4].m_Chirp, out cachedChirpData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_CachedChirpData[buffer[index4].m_Chirp] = cachedChirpData.Update(this.m_NameSystem, entityArray[index3]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_DeletedLinkEntityQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_DeletedLinkEntityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index5 = 0; index5 < entityArray.Length; ++index5)
        {
          entityManager = this.EntityManager;
          DynamicBuffer<ChirpLink> buffer = entityManager.GetBuffer<ChirpLink>(entityArray[index5], true);
          for (int index6 = 0; index6 < buffer.Length; ++index6)
          {
            // ISSUE: variable of a compiler-generated type
            ChirpLinkSystem.CachedChirpData cachedChirpData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CachedChirpData.TryGetValue(buffer[index6].m_Chirp, out cachedChirpData))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_CachedChirpData[buffer[index6].m_Chirp] = cachedChirpData.Remove(entityArray[index5]);
            }
          }
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_DeletedChirpQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_DeletedChirpQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Chirp> componentDataArray1 = this.m_DeletedChirpQuery.ToComponentDataArray<Chirp>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index7 = 0; index7 < entityArray1.Length; ++index7)
      {
        DynamicBuffer<ChirpEntity> buffer;
        if (this.EntityManager.TryGetBuffer<ChirpEntity>(entityArray1[index7], true, out buffer))
        {
          NativeArray<ChirpEntity> nativeArray = new NativeArray<ChirpEntity>(buffer.AsNativeArray(), Allocator.Temp);
          for (int index8 = 0; index8 < nativeArray.Length; ++index8)
          {
            // ISSUE: reference to a compiler-generated method
            this.UnregisterLink(nativeArray[index8].m_Entity, entityArray1[index7]);
          }
          nativeArray.Dispose();
        }
        // ISSUE: reference to a compiler-generated method
        this.UnregisterLink(componentDataArray1[index7].m_Sender, entityArray1[index7]);
        // ISSUE: reference to a compiler-generated field
        this.m_CachedChirpData.Remove(entityArray1[index7]);
      }
      componentDataArray1.Dispose();
      entityArray1.Dispose();
    }

    private void RegisterLink(Entity linkEntity, Entity chirpEntity)
    {
      DynamicBuffer<ChirpLink> buffer;
      if (!this.EntityManager.TryGetBuffer<ChirpLink>(linkEntity, false, out buffer))
        buffer = this.EntityManager.AddBuffer<ChirpLink>(linkEntity);
      // ISSUE: reference to a compiler-generated method
      if (this.LinkExists(buffer, chirpEntity))
        return;
      buffer.Add(new ChirpLink() { m_Chirp = chirpEntity });
    }

    private void UnregisterLink(Entity linkEntity, Entity chirpEntity)
    {
      DynamicBuffer<ChirpLink> buffer;
      if (!this.EntityManager.TryGetBuffer<ChirpLink>(linkEntity, false, out buffer))
        return;
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index].m_Chirp == chirpEntity)
        {
          buffer.RemoveAt(index);
          break;
        }
      }
      if (buffer.Length != 0)
        return;
      this.EntityManager.RemoveComponent<ChirpLink>(linkEntity);
    }

    private bool LinkExists(DynamicBuffer<ChirpLink> links, Entity link)
    {
      for (int index = 0; index < links.Length; ++index)
      {
        if (links[index].m_Chirp == link)
          return true;
      }
      return false;
    }

    private void Initialize()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CachedChirpData.Clear();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Chirp> componentDataArray = this.m_AllChirpsQuery.ToComponentDataArray<Chirp>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_AllChirpsQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        DynamicBuffer<ChirpEntity> buffer;
        if (this.EntityManager.TryGetBuffer<ChirpEntity>(entityArray[index1], true, out buffer))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_CachedChirpData[entityArray[index1]] = new ChirpLinkSystem.CachedChirpData(this.m_NameSystem, componentDataArray[index1], buffer);
          NativeArray<ChirpEntity> nativeArray = new NativeArray<ChirpEntity>(buffer.AsNativeArray(), Allocator.Temp);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            if (this.EntityManager.Exists(nativeArray[index2].m_Entity))
            {
              // ISSUE: reference to a compiler-generated method
              this.RegisterLink(nativeArray[index2].m_Entity, entityArray[index1]);
            }
          }
          nativeArray.Dispose();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_CachedChirpData[entityArray[index1]] = new ChirpLinkSystem.CachedChirpData(this.m_NameSystem, componentDataArray[index1]);
        }
        if (this.EntityManager.Exists(componentDataArray[index1].m_Sender))
        {
          // ISSUE: reference to a compiler-generated method
          this.RegisterLink(componentDataArray[index1].m_Sender, entityArray[index1]);
        }
      }
      entityArray.Dispose();
      componentDataArray.Dispose();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CachedChirpData.Count);
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<Entity, ChirpLinkSystem.CachedChirpData> keyValuePair in this.m_CachedChirpData)
      {
        writer.Write(keyValuePair.Key);
        writer.Write<ChirpLinkSystem.CachedChirpData>(keyValuePair.Value);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CachedChirpData.Clear();
      int num;
      reader.Read(out num);
      for (int index = 0; index < num; ++index)
      {
        Entity key;
        reader.Read(out key);
        // ISSUE: variable of a compiler-generated type
        ChirpLinkSystem.CachedChirpData cachedChirpData;
        reader.Read<ChirpLinkSystem.CachedChirpData>(out cachedChirpData);
        // ISSUE: reference to a compiler-generated field
        this.m_CachedChirpData[key] = cachedChirpData;
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CachedChirpData.Clear();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated field
      if (this.m_CachedChirpData.Count != 0)
        return;
      // ISSUE: reference to a compiler-generated method
      this.Initialize();
    }

    [Preserve]
    public ChirpLinkSystem()
    {
    }

    public struct CachedChirpData : ISerializable
    {
      public ChirpLinkSystem.CachedEntityName m_Sender;
      public ChirpLinkSystem.CachedEntityName[] m_Links;

      public CachedChirpData(NameSystem nameSystem, Chirp chirpData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Sender = new ChirpLinkSystem.CachedEntityName(nameSystem, chirpData.m_Sender);
        // ISSUE: reference to a compiler-generated field
        this.m_Links = (ChirpLinkSystem.CachedEntityName[]) null;
      }

      public CachedChirpData(
        NameSystem nameSystem,
        Chirp chirpData,
        DynamicBuffer<ChirpEntity> chirpEntities)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_Sender = new ChirpLinkSystem.CachedEntityName(nameSystem, chirpData.m_Sender);
        // ISSUE: reference to a compiler-generated field
        this.m_Links = new ChirpLinkSystem.CachedEntityName[chirpEntities.Length];
        for (int index = 0; index < chirpEntities.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_Links[index] = new ChirpLinkSystem.CachedEntityName(nameSystem, chirpEntities[index].m_Entity);
        }
      }

      public ChirpLinkSystem.CachedChirpData Update(NameSystem nameSystem, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Sender.m_Entity == entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_Sender = new ChirpLinkSystem.CachedEntityName(nameSystem, entity);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Links != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_Links.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Links[index].m_Entity == entity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_Links[index] = new ChirpLinkSystem.CachedEntityName(nameSystem, entity);
            }
          }
        }
        return this;
      }

      public ChirpLinkSystem.CachedChirpData Remove(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Sender.m_Entity == entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Sender.m_Entity = Entity.Null;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_Links != null)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_Links.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Links[index].m_Entity == entity)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: object of a compiler-generated type is created
              this.m_Links[index] = new ChirpLinkSystem.CachedEntityName()
              {
                m_Entity = Entity.Null,
                m_Name = this.m_Links[index].m_Name
              };
            }
          }
        }
        return this;
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write<ChirpLinkSystem.CachedEntityName>(this.m_Sender);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int length = this.m_Links != null ? this.m_Links.Length : 0;
        writer.Write(length);
        for (int index = 0; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write<ChirpLinkSystem.CachedEntityName>(this.m_Links[index]);
        }
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read<ChirpLinkSystem.CachedEntityName>(out this.m_Sender);
        int length;
        reader.Read(out length);
        if (length > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Links = new ChirpLinkSystem.CachedEntityName[length];
          for (int index = 0; index < length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            reader.Read<ChirpLinkSystem.CachedEntityName>(out this.m_Links[index]);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Links = (ChirpLinkSystem.CachedEntityName[]) null;
        }
      }
    }

    public struct CachedEntityName : ISerializable
    {
      public Entity m_Entity;
      public NameSystem.Name m_Name;

      public CachedEntityName(NameSystem nameSystem, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Entity = entity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_Name = nameSystem.GetName(entity);
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_Entity);
        // ISSUE: reference to a compiler-generated field
        writer.Write<NameSystem.Name>(this.m_Name);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_Entity);
        // ISSUE: reference to a compiler-generated field
        reader.Read<NameSystem.Name>(out this.m_Name);
      }
    }
  }
}
