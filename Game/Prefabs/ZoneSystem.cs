// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class ZoneSystem : GameSystemBase
  {
    private EntityQuery m_CreatedQuery;
    private EntityQuery m_PrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private NativeList<Entity> m_ZonePrefabs;
    private int m_ZoneFillColors;
    private int m_ZoneEdgeColors;
    private bool m_IsEditorMode;
    private bool m_UpdateColors;
    private bool m_RemovedZones;
    private Vector4[] m_FillColorArray;
    private Vector4[] m_EdgeColorArray;
    private JobHandle m_PrefabsReaders;
    private ZoneSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<ZoneData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ZoneData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZonePrefabs = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FillColorArray = new Vector4[1023];
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeColorArray = new Vector4[1023];
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneFillColors = Shader.PropertyToID("colossal_ZoneFillColors");
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneEdgeColors = Shader.PropertyToID("colossal_ZoneEdgeColors");
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ZonePrefabs.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated method
        this.InitializeZonePrefabs();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_UpdateColors)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateZoneColors();
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      // ISSUE: reference to a compiler-generated field
      if (mode.IsEditor() == this.m_IsEditorMode)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_IsEditorMode = !this.m_IsEditorMode;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateColors = this.m_ZonePrefabs.Length != 0;
    }

    private void InitializeZonePrefabs()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_CreatedQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ZoneData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
        NativeArray<PrefabData> nativeArray2 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle2);
        NativeArray<ZoneData> nativeArray3 = archetypeChunk.GetNativeArray<ZoneData>(ref componentTypeHandle3);
        if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
        {
          for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            ZoneData zoneData = nativeArray3[index2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((int) zoneData.m_ZoneType.m_Index < this.m_ZonePrefabs.Length && this.m_ZonePrefabs[(int) zoneData.m_ZoneType.m_Index] == entity)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ZonePrefabs[(int) zoneData.m_ZoneType.m_Index] = Entity.Null;
              // ISSUE: reference to a compiler-generated field
              this.m_RemovedZones = true;
            }
          }
        }
        else
        {
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            ZonePrefab prefab = this.m_PrefabSystem.GetPrefab<ZonePrefab>(nativeArray2[index3]);
            ZoneData zoneData = nativeArray3[index3] with
            {
              m_AreaType = prefab.m_AreaType
            };
            if (prefab.m_AreaType != AreaType.None)
            {
              // ISSUE: reference to a compiler-generated method
              zoneData.m_ZoneType = new ZoneType()
              {
                m_Index = (ushort) this.GetNextIndex()
              };
              zoneData.m_MinOddHeight = ushort.MaxValue;
              zoneData.m_MinEvenHeight = ushort.MaxValue;
              zoneData.m_MaxHeight = (ushort) 0;
            }
            else
            {
              zoneData.m_ZoneFlags |= ZoneFlags.SupportNarrow;
              zoneData.m_MinOddHeight = (ushort) 1;
              zoneData.m_MinEvenHeight = (ushort) 1;
              zoneData.m_MaxHeight = (ushort) 1;
            }
            // ISSUE: reference to a compiler-generated field
            if ((int) zoneData.m_ZoneType.m_Index < this.m_ZonePrefabs.Length)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ZonePrefabs[(int) zoneData.m_ZoneType.m_Index] = entity;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              while ((int) zoneData.m_ZoneType.m_Index > this.m_ZonePrefabs.Length)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_ZonePrefabs.Add(Entity.Null);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_ZonePrefabs.Add(in entity);
            }
            nativeArray3[index3] = zoneData;
            // ISSUE: reference to a compiler-generated method
            this.UpdateZoneColors(prefab, zoneData);
          }
        }
      }
      archetypeChunkArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVectorArray(this.m_ZoneFillColors, this.m_FillColorArray);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVectorArray(this.m_ZoneEdgeColors, this.m_EdgeColorArray);
    }

    private int GetNextIndex()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_RemovedZones)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 1; index < this.m_ZonePrefabs.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ZonePrefabs[index] == Entity.Null)
            return index;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_RemovedZones = false;
      }
      // ISSUE: reference to a compiler-generated field
      return math.max(1, this.m_ZonePrefabs.Length);
    }

    private void UpdateZoneColors()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateColors = false;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ZoneData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle;
      this.CompleteDependency();
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
        NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
        NativeArray<ZoneData> nativeArray2 = archetypeChunk.GetNativeArray<ZoneData>(ref componentTypeHandle2);
        for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.UpdateZoneColors(this.m_PrefabSystem.GetPrefab<ZonePrefab>(nativeArray1[index2]), nativeArray2[index2]);
        }
      }
      archetypeChunkArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVectorArray(this.m_ZoneFillColors, this.m_FillColorArray);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalVectorArray(this.m_ZoneEdgeColors, this.m_EdgeColorArray);
    }

    private void UpdateZoneColors(ZonePrefab zonePrefab, ZoneData zoneData)
    {
      Color color = zonePrefab.m_Color;
      Color edge = zonePrefab.m_Edge;
      Color occupied1;
      Color selected1;
      // ISSUE: reference to a compiler-generated method
      this.GetZoneColors(color, out occupied1, out selected1);
      Color occupied2;
      Color selected2;
      // ISSUE: reference to a compiler-generated method
      this.GetZoneColors(edge, out occupied2, out selected2);
      int colorIndex1 = ZoneUtils.GetColorIndex(CellFlags.Visible, zoneData.m_ZoneType);
      int colorIndex2 = ZoneUtils.GetColorIndex(CellFlags.Visible | CellFlags.Occupied, zoneData.m_ZoneType);
      int colorIndex3 = ZoneUtils.GetColorIndex(CellFlags.Visible | CellFlags.Selected, zoneData.m_ZoneType);
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsEditorMode)
      {
        color.a = 0.0f;
        edge.a *= 0.5f;
        occupied1.a = 0.0f;
        occupied2.a = 0.0f;
        selected1.a = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_FillColorArray[colorIndex1] = (Vector4) color;
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeColorArray[colorIndex1] = (Vector4) edge;
      // ISSUE: reference to a compiler-generated field
      this.m_FillColorArray[colorIndex2] = (Vector4) occupied1;
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeColorArray[colorIndex2] = (Vector4) occupied2;
      // ISSUE: reference to a compiler-generated field
      this.m_FillColorArray[colorIndex3] = (Vector4) selected1;
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeColorArray[colorIndex3] = (Vector4) selected2;
    }

    private void GetZoneColors(Color color, out Color occupied, out Color selected)
    {
      float H;
      float S;
      float V;
      Color.RGBToHSV(color, out H, out S, out V);
      occupied = Color.HSVToRGB(H, S * 0.75f, V);
      occupied.a = color.a * 0.5f;
      selected = Color.HSVToRGB(H, math.min(1f, S * 1.25f), V);
      selected.a = math.min(color.a * 1.5f, math.lerp(color.a, 1f, 0.5f));
    }

    public ZonePrefabs GetPrefabs() => new ZonePrefabs(this.m_ZonePrefabs.AsArray());

    public void AddPrefabsReader(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabsReaders = JobHandle.CombineDependencies(this.m_PrefabsReaders, handle);
    }

    public Entity GetPrefab(ZoneType zoneType)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return (int) zoneType.m_Index >= this.m_ZonePrefabs.Length ? Entity.Null : this.m_ZonePrefabs[(int) zoneType.m_Index];
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

    [Preserve]
    public ZoneSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<ZoneData> __Game_Prefabs_ZoneData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ZoneData>(true);
      }
    }
  }
}
