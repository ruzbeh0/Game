// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LevelInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LevelInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "levelInfo";
    private RawValueBinding m_ResidentialLevels;
    private RawValueBinding m_CommercialLevels;
    private RawValueBinding m_IndustrialLevels;
    private RawValueBinding m_OfficeLevels;
    private EntityQuery m_SpawnableQuery;
    private NativeArray<LevelInfoviewUISystem.Levels> m_Results;
    private LevelInfoviewUISystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnableQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<ResidentialProperty>(),
          ComponentType.ReadOnly<CommercialProperty>(),
          ComponentType.ReadOnly<IndustrialProperty>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResidentialLevels = new RawValueBinding("levelInfo", "residential", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels result = this.m_Results[0];
        // ISSUE: reference to a compiler-generated method
        this.WriteLevels(writer, result);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CommercialLevels = new RawValueBinding("levelInfo", "commercial", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels result = this.m_Results[1];
        // ISSUE: reference to a compiler-generated method
        this.WriteLevels(writer, result);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_IndustrialLevels = new RawValueBinding("levelInfo", "industrial", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels result = this.m_Results[2];
        // ISSUE: reference to a compiler-generated method
        this.WriteLevels(writer, result);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_OfficeLevels = new RawValueBinding("levelInfo", "office", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels result = this.m_Results[3];
        // ISSUE: reference to a compiler-generated method
        this.WriteLevels(writer, result);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<LevelInfoviewUISystem.Levels>(4, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_ResidentialLevels.active || this.m_CommercialLevels.active || this.m_IndustrialLevels.active || this.m_OfficeLevels.active;
      }
    }

    protected override void PerformUpdate() => this.UpdateBindings();

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated method
      this.UpdateBindings();
    }

    private void UpdateBindings()
    {
      // ISSUE: reference to a compiler-generated field
      this.ResetResults<LevelInfoviewUISystem.Levels>(this.m_Results);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new LevelInfoviewUISystem.UpdateLevelsJob()
      {
        m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ResidentialPropertyHandle = this.__TypeHandle.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle,
        m_CommercialPropertyHandle = this.__TypeHandle.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle,
        m_IndustrialPropertyHandle = this.__TypeHandle.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle,
        m_OfficeBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_OfficeBuilding_RW_ComponentLookup,
        m_SpawnableBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_SignatureBuildingFromEntity = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup,
        m_Results = this.m_Results
      }.Schedule<LevelInfoviewUISystem.UpdateLevelsJob>(this.m_SpawnableQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialLevels.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialLevels.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialLevels.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeLevels.Update();
    }

    private void UpdateResidentialLevels(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      LevelInfoviewUISystem.Levels result = this.m_Results[0];
      // ISSUE: reference to a compiler-generated method
      this.WriteLevels(writer, result);
    }

    private void UpdateCommercialLevels(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      LevelInfoviewUISystem.Levels result = this.m_Results[1];
      // ISSUE: reference to a compiler-generated method
      this.WriteLevels(writer, result);
    }

    private void UpdateIndustrialLevels(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      LevelInfoviewUISystem.Levels result = this.m_Results[2];
      // ISSUE: reference to a compiler-generated method
      this.WriteLevels(writer, result);
    }

    private void UpdateOfficeLevels(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      LevelInfoviewUISystem.Levels result = this.m_Results[3];
      // ISSUE: reference to a compiler-generated method
      this.WriteLevels(writer, result);
    }

    private void WriteLevels(IJsonWriter writer, LevelInfoviewUISystem.Levels levels)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      InfoviewsUIUtils.UpdateFiveSlicePieChartData(writer, levels.Level1, levels.Level2, levels.Level3, levels.Level4, levels.Level5);
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
    public LevelInfoviewUISystem()
    {
    }

    private enum Result
    {
      Residential,
      Commercial,
      Industrial,
      Office,
      ResultCount,
    }

    [BurstCompile]
    private struct UpdateLevelsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResidentialProperty> m_ResidentialPropertyHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> m_CommercialPropertyHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> m_IndustrialPropertyHandle;
      [ReadOnly]
      public ComponentLookup<OfficeBuilding> m_OfficeBuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingFromEntity;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> m_SignatureBuildingFromEntity;
      public NativeArray<LevelInfoviewUISystem.Levels> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels levels1 = new LevelInfoviewUISystem.Levels();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels levels2 = new LevelInfoviewUISystem.Levels();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels levels3 = new LevelInfoviewUISystem.Levels();
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LevelInfoviewUISystem.Levels levels4 = new LevelInfoviewUISystem.Levels();
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefabRef = nativeArray[index];
          SpawnableBuildingData componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_SpawnableBuildingFromEntity.TryGetComponent(prefabRef.m_Prefab, out componentData) && !this.m_SignatureBuildingFromEntity.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<ResidentialProperty>(ref this.m_ResidentialPropertyHandle))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddLevel(componentData, ref levels1);
            }
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<CommercialProperty>(ref this.m_CommercialPropertyHandle))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddLevel(componentData, ref levels2);
            }
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<IndustrialProperty>(ref this.m_IndustrialPropertyHandle))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_OfficeBuildingFromEntity.HasComponent(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLevel(componentData, ref levels4);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.AddLevel(componentData, ref levels3);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += levels1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += levels2;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += levels3;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[3] += levels4;
      }

      private void AddLevel(
        SpawnableBuildingData spawnableBuildingData,
        ref LevelInfoviewUISystem.Levels levels)
      {
        switch (spawnableBuildingData.m_Level)
        {
          case 1:
            // ISSUE: reference to a compiler-generated field
            ++levels.Level1;
            break;
          case 2:
            // ISSUE: reference to a compiler-generated field
            ++levels.Level2;
            break;
          case 3:
            // ISSUE: reference to a compiler-generated field
            ++levels.Level3;
            break;
          case 4:
            // ISSUE: reference to a compiler-generated field
            ++levels.Level4;
            break;
          case 5:
            // ISSUE: reference to a compiler-generated field
            ++levels.Level5;
            break;
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct Levels
    {
      public int Level1;
      public int Level2;
      public int Level3;
      public int Level4;
      public int Level5;

      public Levels(int level1, int level2, int level3, int level4, int level5)
      {
        // ISSUE: reference to a compiler-generated field
        this.Level1 = level1;
        // ISSUE: reference to a compiler-generated field
        this.Level2 = level2;
        // ISSUE: reference to a compiler-generated field
        this.Level3 = level3;
        // ISSUE: reference to a compiler-generated field
        this.Level4 = level4;
        // ISSUE: reference to a compiler-generated field
        this.Level5 = level5;
      }

      public static LevelInfoviewUISystem.Levels operator +(
        LevelInfoviewUISystem.Levels left,
        LevelInfoviewUISystem.Levels right)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new LevelInfoviewUISystem.Levels(left.Level1 + right.Level1, left.Level2 + right.Level2, left.Level3 + right.Level3, left.Level4 + right.Level4, left.Level5 + right.Level5);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResidentialProperty> __Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CommercialProperty> __Game_Buildings_CommercialProperty_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<IndustrialProperty> __Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle;
      public ComponentLookup<OfficeBuilding> __Game_Prefabs_OfficeBuilding_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResidentialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResidentialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_CommercialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CommercialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_IndustrialProperty_RO_ComponentTypeHandle = state.GetComponentTypeHandle<IndustrialProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OfficeBuilding_RW_ComponentLookup = state.GetComponentLookup<OfficeBuilding>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
      }
    }
  }
}
