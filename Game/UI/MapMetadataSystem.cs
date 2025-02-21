// Decompiled with JetBrains decompiler
// Type: Game.UI.MapMetadataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Colossal.Json;
using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Areas;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Simulation;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI
{
  [CompilerGenerated]
  public class MapMetadataSystem : GameSystemBase
  {
    private PlanetarySystem m_PlanetarySystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private ClimateSystem m_ClimateSystem;
    private PrefabSystem m_PrefabSystem;
    private float m_Area;
    private float m_BuildableLand;
    private float m_WaterAvailability;
    private MapMetadataSystem.Resources m_Resources;
    private MapMetadataSystem.Connections m_Connections;
    private EntityQuery m_MapTileQuery;
    private EntityQuery m_OutsideConnectionQuery;
    private MapMetadataSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.ElectricityOutsideConnection>(),
          ComponentType.ReadOnly<Game.Objects.WaterPipeOutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.CompleteDependency();
      // ISSUE: reference to a compiler-generated method
      this.UpdateResources();
      // ISSUE: reference to a compiler-generated method
      this.UpdateConnections();
    }

    private void UpdateResources()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Area = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_BuildableLand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_WaterAvailability = 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Resources = new MapMetadataSystem.Resources();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<MapFeatureElement> bufferTypeHandle = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_MapTileQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          BufferAccessor<MapFeatureElement> bufferAccessor = archetypeChunkArray[index1].GetBufferAccessor<MapFeatureElement>(ref bufferTypeHandle);
          for (int index2 = 0; index2 < bufferAccessor.Length; ++index2)
          {
            DynamicBuffer<MapFeatureElement> dynamicBuffer = bufferAccessor[index2];
            // ISSUE: reference to a compiler-generated field
            this.m_Area += dynamicBuffer[0].m_Amount;
            // ISSUE: reference to a compiler-generated field
            this.m_BuildableLand += dynamicBuffer[1].m_Amount;
            // ISSUE: reference to a compiler-generated field
            this.m_WaterAvailability += dynamicBuffer[6].m_Amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Resources.fertile += dynamicBuffer[2].m_Amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Resources.forest += dynamicBuffer[3].m_Amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Resources.oil += dynamicBuffer[4].m_Amount;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Resources.ore += dynamicBuffer[5].m_Amount;
          }
        }
      }
    }

    private void UpdateConnections()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_Connections = new MapMetadataSystem.Connections();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Game.Objects.ElectricityOutsideConnection> componentTypeHandle1 = this.__TypeHandle.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection> componentTypeHandle2 = this.__TypeHandle.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabRef> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_OutsideConnectionQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<PrefabRef> nativeArray = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Connections.electricity |= archetypeChunk.Has<Game.Objects.ElectricityOutsideConnection>(ref componentTypeHandle1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Connections.water |= archetypeChunk.Has<Game.Objects.WaterPipeOutsideConnection>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            OutsideConnectionData component;
            if (this.EntityManager.TryGetComponent<OutsideConnectionData>(nativeArray[index2].m_Prefab, out component))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Connections.road |= (component.m_Type & OutsideConnectionTransferType.Road) != 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Connections.train |= (component.m_Type & OutsideConnectionTransferType.Train) != 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Connections.air |= (component.m_Type & OutsideConnectionTransferType.Air) != 0;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Connections.ship |= (component.m_Type & OutsideConnectionTransferType.Ship) != 0;
            }
          }
        }
      }
    }

    [CanBeNull]
    public string mapName { get; set; }

    [CanBeNull]
    public string theme
    {
      get
      {
        return this.m_CityConfigurationSystem.defaultTheme != Entity.Null ? this.m_PrefabSystem.GetPrefab<ThemePrefab>(this.m_CityConfigurationSystem.defaultTheme).name : (string) null;
      }
    }

    public Bounds1 temperatureRange
    {
      get
      {
        return this.m_ClimateSystem.currentClimate != Entity.Null ? this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_ClimateSystem.currentClimate).temperatureRange : new Bounds1();
      }
    }

    public float cloudiness
    {
      get
      {
        return this.m_ClimateSystem.currentClimate != Entity.Null ? this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_ClimateSystem.currentClimate).averageCloudiness : 0.0f;
      }
    }

    public float precipitation
    {
      get
      {
        return this.m_ClimateSystem.currentClimate != Entity.Null ? this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_ClimateSystem.currentClimate).averagePrecipitation : 0.0f;
      }
    }

    public float latitude => this.m_PlanetarySystem.latitude;

    public float longitude => this.m_PlanetarySystem.longitude;

    public float area => this.m_Area;

    public float buildableLand => this.m_BuildableLand;

    public float waterAvailability => this.m_WaterAvailability;

    public MapMetadataSystem.Resources resources => this.m_Resources;

    public MapMetadataSystem.Connections connections => this.m_Connections;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public MapMetadataSystem()
    {
    }

    public struct Resources : IJsonWritable
    {
      public float fertile;
      public float forest;
      public float oil;
      public float ore;

      public ProxyObject ToVariant()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return new ProxyObject()
        {
          {
            "fertile",
            (Colossal.Json.Variant) new ProxyNumber((IConvertible) this.fertile)
          },
          {
            "forest",
            (Colossal.Json.Variant) new ProxyNumber((IConvertible) this.forest)
          },
          {
            "oil",
            (Colossal.Json.Variant) new ProxyNumber((IConvertible) this.oil)
          },
          {
            "ore",
            (Colossal.Json.Variant) new ProxyNumber((IConvertible) this.ore)
          }
        };
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("fertile");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.fertile);
        writer.PropertyName("forest");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.forest);
        writer.PropertyName("oil");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.oil);
        writer.PropertyName("ore");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.ore);
        writer.TypeEnd();
      }
    }

    public struct Connections : IJsonWritable
    {
      public bool road;
      public bool train;
      public bool air;
      public bool ship;
      public bool electricity;
      public bool water;

      public ProxyObject ToVariant()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return new ProxyObject()
        {
          {
            "road",
            (Colossal.Json.Variant) new ProxyBoolean(this.road)
          },
          {
            "train",
            (Colossal.Json.Variant) new ProxyBoolean(this.train)
          },
          {
            "air",
            (Colossal.Json.Variant) new ProxyBoolean(this.air)
          },
          {
            "ship",
            (Colossal.Json.Variant) new ProxyBoolean(this.ship)
          },
          {
            "electricity",
            (Colossal.Json.Variant) new ProxyBoolean(this.electricity)
          },
          {
            "water",
            (Colossal.Json.Variant) new ProxyBoolean(this.water)
          }
        };
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("road");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.road);
        writer.PropertyName("train");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.train);
        writer.PropertyName("air");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.air);
        writer.PropertyName("ship");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.ship);
        writer.PropertyName("electricity");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.electricity);
        writer.PropertyName("water");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.water);
        writer.TypeEnd();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.ElectricityOutsideConnection> __Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection> __Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<MapFeatureElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_ElectricityOutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.ElectricityOutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_WaterPipeOutsideConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.WaterPipeOutsideConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
