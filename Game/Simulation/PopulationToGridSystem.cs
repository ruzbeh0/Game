// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PopulationToGridSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Objects;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class PopulationToGridSystem : CellMapSystem<PopulationCell>, IJobSerializable
  {
    public static readonly int kTextureSize = 64;
    public static readonly int kUpdatesPerDay = 32;
    private EntityQuery m_ResidentialPropertyQuery;
    private PopulationToGridSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / PopulationToGridSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get => new int2(PopulationToGridSystem.kTextureSize, PopulationToGridSystem.kTextureSize);
    }

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<PopulationCell>.GetCellCenter(index, PopulationToGridSystem.kTextureSize);
    }

    public static PopulationCell GetPopulation(
      float3 position,
      NativeArray<PopulationCell> populationMap)
    {
      PopulationCell population1 = new PopulationCell();
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<PopulationCell>.GetCell(position, CellMapSystem<PopulationCell>.kMapSize, PopulationToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 cellCoords = CellMapSystem<PopulationCell>.GetCellCoords(position, CellMapSystem<PopulationCell>.kMapSize, PopulationToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cell.x < 0 || cell.x >= PopulationToGridSystem.kTextureSize || cell.y < 0 || cell.y >= PopulationToGridSystem.kTextureSize)
        return population1;
      // ISSUE: reference to a compiler-generated field
      float population2 = populationMap[cell.x + PopulationToGridSystem.kTextureSize * cell.y].m_Population;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float y1 = cell.x < PopulationToGridSystem.kTextureSize - 1 ? populationMap[cell.x + 1 + PopulationToGridSystem.kTextureSize * cell.y].m_Population : 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float x = cell.y < PopulationToGridSystem.kTextureSize - 1 ? populationMap[cell.x + PopulationToGridSystem.kTextureSize * (cell.y + 1)].m_Population : 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float y2 = cell.x >= PopulationToGridSystem.kTextureSize - 1 || cell.y >= PopulationToGridSystem.kTextureSize - 1 ? 0.0f : populationMap[cell.x + 1 + PopulationToGridSystem.kTextureSize * (cell.y + 1)].m_Population;
      population1.m_Population = math.lerp(math.lerp(population2, y1, cellCoords.x - (float) cell.x), math.lerp(x, y2, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y);
      return population1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(PopulationToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialPropertyQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResidentialProperty>(), ComponentType.ReadOnly<Renter>(), ComponentType.ReadOnly<Transform>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PopulationToGridSystem.PopulationToGridJob jobData = new PopulationToGridSystem.PopulationToGridJob()
      {
        m_Entities = this.m_ResidentialPropertyQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_PopulationMap = this.m_Map,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup
      };
      this.Dependency = jobData.Schedule<PopulationToGridSystem.PopulationToGridJob>(JobUtils.CombineDependencies(outJobHandle, this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency));
      this.AddWriter(this.Dependency);
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
    public PopulationToGridSystem()
    {
    }

    [BurstCompile]
    private struct PopulationToGridJob : IJob
    {
      [ReadOnly]
      public NativeList<Entity> m_Entities;
      public NativeArray<PopulationCell> m_PopulationMap;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public ComponentLookup<Transform> m_Transforms;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < PopulationToGridSystem.kTextureSize * PopulationToGridSystem.kTextureSize; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PopulationMap[index] = new PopulationCell();
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Entities.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_Entities[index1];
          int num = 0;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Renter> renter1 = this.m_Renters[entity];
          for (int index2 = 0; index2 < renter1.Length; ++index2)
          {
            Entity renter2 = renter1[index2].m_Renter;
            // ISSUE: reference to a compiler-generated field
            if (this.m_HouseholdCitizens.HasBuffer(renter2))
            {
              // ISSUE: reference to a compiler-generated field
              num += this.m_HouseholdCitizens[renter2].Length;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int2 cell = CellMapSystem<PopulationCell>.GetCell(this.m_Transforms[entity].m_Position, CellMapSystem<PopulationCell>.kMapSize, PopulationToGridSystem.kTextureSize);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (cell.x >= 0 && cell.y >= 0 && cell.x < PopulationToGridSystem.kTextureSize && cell.y < PopulationToGridSystem.kTextureSize)
          {
            // ISSUE: reference to a compiler-generated field
            int index3 = cell.x + cell.y * PopulationToGridSystem.kTextureSize;
            // ISSUE: reference to a compiler-generated field
            PopulationCell population = this.m_PopulationMap[index3];
            population.m_Population += (float) num;
            // ISSUE: reference to a compiler-generated field
            this.m_PopulationMap[index3] = population;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
      }
    }
  }
}
