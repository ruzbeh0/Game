// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TerrainInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [FormerlySerializedAs("Colossal.Terrain.TerrainInitializeSystem, Game")]
  [CompilerGenerated]
  public class TerrainInitializeSystem : GameSystemBase
  {
    private EntityQuery m_TerrainPropertiesQuery;
    private EntityQuery m_TerrainMaterialPropertiesQuery;
    private TerrainSystem m_TerrainSystem;
    private TerrainRenderSystem m_TerrainRenderSystem;
    private TerrainMaterialSystem m_TerrainMaterialSystem;
    private WaterSystem m_WaterSystem;
    private WaterRenderSystem m_WaterRenderSystem;
    private SnowSystem m_SnowSystem;
    private PrefabSystem m_PrefabSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainMaterialSystem = this.World.GetOrCreateSystemManaged<TerrainMaterialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainRenderSystem = this.World.GetOrCreateSystemManaged<TerrainRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterRenderSystem = this.World.GetOrCreateSystemManaged<WaterRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SnowSystem = this.World.GetOrCreateSystemManaged<SnowSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainPropertiesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<TerrainPropertiesData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainMaterialPropertiesQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<TerrainMaterialPropertiesData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TerrainPropertiesQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.MaxSpeed = this.m_PrefabSystem.GetPrefab<TerrainPropertiesPrefab>(this.m_TerrainPropertiesQuery.GetSingletonEntity()).m_WaterMaxSpeed;
    }

    [Preserve]
    public TerrainInitializeSystem()
    {
    }
  }
}
