// Decompiled with JetBrains decompiler
// Type: Game.Serialization.SerializerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Agents;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Effects;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  public class SerializerSystem : GameSystemBase
  {
    private SaveGameSystem m_SaveGameSystem;
    private LoadGameSystem m_LoadGameSystem;
    private WriteSystem m_WriteSystem;
    private ReadSystem m_ReadSystem;
    private UpdateSystem m_UpdateSystem;
    private ComponentSerializerLibrary m_ComponentSerializerLibrary;
    private SystemSerializerLibrary m_SystemSerializerLibrary;
    private EntityQuery m_Query;

    public ComponentSerializerLibrary componentLibrary => this.m_ComponentSerializerLibrary;

    public SystemSerializerLibrary systemLibrary => this.m_SystemSerializerLibrary;

    public int totalSize { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_SaveGameSystem = this.World.GetOrCreateSystemManaged<SaveGameSystem>();
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      this.m_WriteSystem = this.World.GetOrCreateSystemManaged<WriteSystem>();
      this.m_ReadSystem = this.World.GetOrCreateSystemManaged<ReadSystem>();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      this.CreateQuery((IEnumerable<ComponentType>) Array.Empty<ComponentType>());
    }

    [Preserve]
    protected override void OnDestroy()
    {
      if (this.m_ComponentSerializerLibrary != null)
        this.m_ComponentSerializerLibrary.Dispose();
      if (this.m_SystemSerializerLibrary != null)
        this.m_SystemSerializerLibrary.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_ComponentSerializerLibrary == null)
        this.m_ComponentSerializerLibrary = new ComponentSerializerLibrary();
      if (this.m_ComponentSerializerLibrary.isDirty)
      {
        List<ComponentType> serializableComponents;
        this.m_ComponentSerializerLibrary.Initialize((SystemBase) this, out serializableComponents);
        this.CreateQuery((IEnumerable<ComponentType>) serializableComponents);
      }
      if (this.m_SystemSerializerLibrary == null)
        this.m_SystemSerializerLibrary = new SystemSerializerLibrary();
      if (this.m_SystemSerializerLibrary.isDirty)
        this.m_SystemSerializerLibrary.Initialize(this.World);
      switch (this.m_UpdateSystem.currentPhase)
      {
        case SystemUpdatePhase.Serialize:
          EntitySerializer<WriteBuffer> entitySerializer = new EntitySerializer<WriteBuffer>(this.EntityManager, this.m_ComponentSerializerLibrary, this.m_SystemSerializerLibrary, (IWriteBufferProvider<WriteBuffer>) this.m_WriteSystem);
          try
          {
            this.totalSize = 0;
            Context context = this.m_SaveGameSystem.context;
            entitySerializer.Serialize<BinaryWriter>(context, this.m_Query, BufferFormat.CompressedZStd, ComponentType.ReadWrite<PrefabData>());
            break;
          }
          finally
          {
            entitySerializer.Dispose();
          }
        case SystemUpdatePhase.Deserialize:
          EntityDeserializer<ReadBuffer> entityDeserializer = new EntityDeserializer<ReadBuffer>(this.EntityManager, this.m_ComponentSerializerLibrary, this.m_SystemSerializerLibrary, (IReadBufferProvider<ReadBuffer>) this.m_ReadSystem);
          try
          {
            this.totalSize = 0;
            Context context = this.m_LoadGameSystem.context;
            entityDeserializer.Deserialize<BinaryReader>(ref context);
            COSystemBase.baseLog.InfoFormat("Serialized version: {0}", (object) context.version);
            this.m_LoadGameSystem.context = context;
            break;
          }
          finally
          {
            entityDeserializer.Dispose();
          }
      }
    }

    public void SetDirty()
    {
      if (this.m_ComponentSerializerLibrary != null)
        this.m_ComponentSerializerLibrary.SetDirty();
      if (this.m_SystemSerializerLibrary == null)
        return;
      this.m_SystemSerializerLibrary.SetDirty();
    }

    private void CreateQuery(IEnumerable<ComponentType> serializableComponents)
    {
      HashSet<ComponentType> source = new HashSet<ComponentType>()
      {
        ComponentType.ReadOnly<LoadedIndex>(),
        ComponentType.ReadOnly<PrefabRef>(),
        ComponentType.ReadOnly<ElectricityFlowNode>(),
        ComponentType.ReadOnly<ElectricityFlowEdge>(),
        ComponentType.ReadOnly<WaterPipeNode>(),
        ComponentType.ReadOnly<WaterPipeEdge>(),
        ComponentType.ReadOnly<ServiceRequest>(),
        ComponentType.ReadOnly<Game.Simulation.WaterSourceData>(),
        ComponentType.ReadOnly<Game.City.City>(),
        ComponentType.ReadOnly<SchoolSeeker>(),
        ComponentType.ReadOnly<JobSeeker>(),
        ComponentType.ReadOnly<CityStatistic>(),
        ComponentType.ReadOnly<ServiceBudgetData>(),
        ComponentType.ReadOnly<FloodCounterData>(),
        ComponentType.ReadOnly<CoordinatedMeeting>(),
        ComponentType.ReadOnly<LookingForPartner>(),
        ComponentType.ReadOnly<AtmosphereData>(),
        ComponentType.ReadOnly<BiomeData>(),
        ComponentType.ReadOnly<TimeData>()
      };
      foreach (ComponentType serializableComponent in serializableComponents)
        source.Add(ComponentType.ReadOnly(serializableComponent.TypeIndex));
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = source.ToArray<ComponentType>(),
        None = new ComponentType[5]
        {
          ComponentType.ReadOnly<NetCompositionData>(),
          ComponentType.ReadOnly<EffectInstance>(),
          ComponentType.ReadOnly<LivePath>(),
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [Preserve]
    public SerializerSystem()
    {
    }
  }
}
