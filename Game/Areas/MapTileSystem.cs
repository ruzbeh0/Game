// Decompiled with JetBrains decompiler
// Type: Game.Areas.MapTileSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Areas
{
  [CompilerGenerated]
  public class MapTileSystem : GameSystemBase, IDefaultSerializable, ISerializable, IPostDeserialize
  {
    private const int LEGACY_GRID_WIDTH = 23;
    private const int LEGACY_GRID_LENGTH = 23;
    private const float LEGACY_CELL_SIZE = 623.3043f;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_MapTileQuery;
    private NativeList<Entity> m_StartTiles;
    private MapTileSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTileData>(), ComponentType.ReadOnly<AreaData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>());
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame)
      {
        if (context.version >= Version.editorMapTiles)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_StartTiles.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_StartTiles[index] == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_StartTiles.RemoveAtSwapBack(index);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_StartTiles.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.RemoveComponent<Native>(this.m_StartTiles.AsArray());
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.LegacyGenerateMapTiles(false);
        }
      }
      else
      {
        if (context.purpose != Colossal.Serialization.Entities.Purpose.NewMap)
          return;
        // ISSUE: reference to a compiler-generated method
        this.LegacyGenerateMapTiles(true);
      }
    }

    public NativeList<Entity> GetStartTiles() => this.m_StartTiles;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StartTiles.Length);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_StartTiles.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_StartTiles[index]);
      }
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      int length;
      reader.Read(out length);
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles.ResizeUninitialized(length);
      for (int index = 0; index < length; ++index)
      {
        Entity entity;
        reader.Read(out entity);
        // ISSUE: reference to a compiler-generated field
        this.m_StartTiles[index] = entity;
      }
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles.Clear();
    }

    private void LegacyGenerateMapTiles(bool editorMode)
    {
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MapTileQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.DestroyEntity(this.m_MapTileQuery);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles.Clear();
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_PrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        Entity entity1 = entityArray[0];
        entityManager = this.EntityManager;
        AreaData componentData = entityManager.GetComponentData<AreaData>(entity1);
        int entityCount = 529;
        entityManager = this.EntityManager;
        NativeArray<Entity> entity2 = entityManager.CreateEntity(componentData.m_Archetype, entityCount, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        if (!editorMode)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponent<Native>(entity2);
        }
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(10, 10), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(11, 10), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(12, 10), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(10, 11), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(11, 11), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(12, 11), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(10, 12), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(11, 12), entity2);
        // ISSUE: reference to a compiler-generated method
        this.AddOwner(new int2(12, 12), entity2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Areas_Area_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        new MapTileSystem.GenerateMapTilesJob()
        {
          m_Entities = entity2,
          m_Prefab = entity1,
          m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup,
          m_AreaData = this.__TypeHandle.__Game_Areas_Area_RW_ComponentLookup,
          m_NodeData = this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup
        }.Schedule<MapTileSystem.GenerateMapTilesJob>(entity2.Length, 4).Complete();
      }
    }

    private void AddOwner(int2 tile, NativeArray<Entity> entities)
    {
      int index = tile.y * 23 + tile.x;
      this.EntityManager.RemoveComponent<Native>(entities[index]);
      // ISSUE: reference to a compiler-generated field
      this.m_StartTiles.Add(entities[index]);
    }

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

    [UnityEngine.Scripting.Preserve]
    public MapTileSystem()
    {
    }

    [BurstCompile]
    private struct GenerateMapTilesJob : IJobParallelFor
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<Entity> m_Entities;
      [ReadOnly]
      public Entity m_Prefab;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Area> m_AreaData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Node> m_NodeData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_Entities[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData[entity] = new PrefabRef(this.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        this.m_AreaData[entity] = new Area(AreaFlags.Complete);
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Node> dynamicBuffer = this.m_NodeData[entity];
        int2 int2 = new int2(index % 23, index / 23);
        float2 float2 = new float2(23f, 23f) * 311.652161f;
        Bounds2 bounds2;
        bounds2.min = (float2) int2 * 623.3043f - float2;
        bounds2.max = (float2) (int2 + 1) * 623.3043f - float2;
        dynamicBuffer.ResizeUninitialized(4);
        dynamicBuffer[0] = new Node(new float3(bounds2.min.x, 0.0f, bounds2.min.y), float.MinValue);
        dynamicBuffer[1] = new Node(new float3(bounds2.min.x, 0.0f, bounds2.max.y), float.MinValue);
        dynamicBuffer[2] = new Node(new float3(bounds2.max.x, 0.0f, bounds2.max.y), float.MinValue);
        dynamicBuffer[3] = new Node(new float3(bounds2.max.x, 0.0f, bounds2.min.y), float.MinValue);
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentLookup;
      public ComponentLookup<Area> __Game_Areas_Area_RW_ComponentLookup;
      public BufferLookup<Node> __Game_Areas_Node_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentLookup = state.GetComponentLookup<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RW_ComponentLookup = state.GetComponentLookup<Area>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RW_BufferLookup = state.GetBufferLookup<Node>();
      }
    }
  }
}
