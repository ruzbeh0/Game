// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MeshGroupSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Creatures;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class MeshGroupSystem : GameSystemBase
  {
    private EntityQuery m_UpdateQuery;
    private EntityQuery m_AllQuery;
    private bool m_Loaded;
    private MeshGroupSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<MeshGroup>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<BatchesUpdated>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllQuery = this.GetEntityQuery(ComponentType.ReadOnly<MeshGroup>());
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery query = this.GetLoaded() ? this.m_AllQuery : this.m_UpdateQuery;
      if (query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new MeshGroupSystem.SetMeshGroupsJob()
      {
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_HumanType = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RW_BufferTypeHandle,
        m_MeshBatchType = this.__TypeHandle.__Game_Rendering_MeshBatch_RW_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_HumanData = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_OverlayElements = this.__TypeHandle.__Game_Prefabs_OverlayElement_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_RandomSeed = RandomSeed.Next()
      }.ScheduleParallel<MeshGroupSystem.SetMeshGroupsJob>(query, this.Dependency);
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
    public MeshGroupSystem()
    {
    }

    [BurstCompile]
    private struct SetMeshGroupsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Human> m_HumanType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      public BufferTypeHandle<MeshBatch> m_MeshBatchType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Human> m_HumanData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<OverlayElement> m_OverlayElements;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [ReadOnly]
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray1 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Human> nativeArray3 = chunk.GetNativeArray<Human>(ref this.m_HumanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray4 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor1 = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshBatch> bufferAccessor2 = chunk.GetBufferAccessor<MeshBatch>(ref this.m_MeshBatchType);
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = nativeArray1.Length != 0 ? new Unity.Mathematics.Random() : this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        NativeList<MeshGroup> nativeList = new NativeList<MeshGroup>();
        for (int index1 = 0; index1 < bufferAccessor1.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray5[index1];
          DynamicBuffer<MeshGroup> newGroups = bufferAccessor1[index1];
          DynamicBuffer<MeshBatch> batches = bufferAccessor2[index1];
          MeshGroup oldGroup = new MeshGroup();
          int length = newGroups.Length;
          if (length > 1)
          {
            if (!nativeList.IsCreated)
              nativeList = new NativeList<MeshGroup>(length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            nativeList.AddRange(newGroups.AsNativeArray());
          }
          else if (length == 1)
            oldGroup = newGroups[0];
          newGroups.Clear();
          DynamicBuffer<SubMeshGroup> bufferData;
          MeshGroup meshGroup;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubMeshGroups.TryGetBuffer(prefabRef.m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<SubMesh> subMesh = this.m_SubMeshes[prefabRef.m_Prefab];
            int index2 = 0;
            int num1 = 0;
            int num2 = 0;
            PseudoRandomSeed pseudoRandomSeed;
            if (CollectionUtils.TryGet<PseudoRandomSeed>(nativeArray1, index1, out pseudoRandomSeed))
              random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kMeshGroup);
            MeshGroupFlags meshGroupFlags1 = (MeshGroupFlags) 0;
            Temp temp;
            MeshGroupFlags meshGroupFlags2;
            if (CollectionUtils.TryGet<Temp>(nativeArray2, index1, out temp) && temp.m_Original != Entity.Null)
            {
              Human componentData1;
              // ISSUE: reference to a compiler-generated field
              if (this.m_HumanData.TryGetComponent(temp.m_Original, out componentData1))
              {
                // ISSUE: reference to a compiler-generated method
                meshGroupFlags1 |= MeshGroupSystem.SetMeshGroupsJob.GetHumanFlags(componentData1);
              }
              CurrentVehicle componentData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              meshGroupFlags2 = !this.m_CurrentVehicleData.TryGetComponent(temp.m_Original, out componentData2) ? meshGroupFlags1 | MeshGroupFlags.ForbidMotorcycle : meshGroupFlags1 | this.GetCurrentVehicleFlags(componentData2);
            }
            else
            {
              Human human;
              if (CollectionUtils.TryGet<Human>(nativeArray3, index1, out human))
              {
                // ISSUE: reference to a compiler-generated method
                meshGroupFlags1 |= MeshGroupSystem.SetMeshGroupsJob.GetHumanFlags(human);
              }
              CurrentVehicle currentVehicle;
              // ISSUE: reference to a compiler-generated method
              meshGroupFlags2 = !CollectionUtils.TryGet<CurrentVehicle>(nativeArray4, index1, out currentVehicle) ? meshGroupFlags1 | MeshGroupFlags.ForbidMotorcycle : meshGroupFlags1 | this.GetCurrentVehicleFlags(currentVehicle);
            }
            while (index2 < bufferData.Length)
            {
              SubMeshGroup subMeshGroup1 = bufferData[index2];
              int num3 = index2;
              index2 += subMeshGroup1.m_SubGroupCount;
              if (subMeshGroup1.m_SubGroupCount <= 0 || index2 + subMeshGroup1.m_SubGroupCount >= 65536)
                throw new Exception("Invalid m_SubGroupCount!");
              if ((subMeshGroup1.m_Flags & meshGroupFlags2) == subMeshGroup1.m_Flags)
              {
                int index3 = num3 + random.NextInt(subMeshGroup1.m_SubGroupCount);
                SubMeshGroup subMeshGroup2 = bufferData[index3];
                ref DynamicBuffer<MeshGroup> local = ref newGroups;
                meshGroup = new MeshGroup();
                meshGroup.m_SubMeshGroup = (ushort) index3;
                meshGroup.m_MeshOffset = (byte) num1;
                meshGroup.m_ColorOffset = (byte) num2;
                MeshGroup elem = meshGroup;
                local.Add(elem);
                num1 += subMeshGroup2.m_SubMeshRange.y - subMeshGroup2.m_SubMeshRange.x;
                num2 += subMeshGroup2.m_SubMeshRange.y - subMeshGroup2.m_SubMeshRange.x;
                for (int x = subMeshGroup2.m_SubMeshRange.x; x < subMeshGroup2.m_SubMeshRange.y; ++x)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_OverlayElements.HasBuffer(subMesh[x].m_SubMesh))
                  {
                    num2 += 8;
                    break;
                  }
                }
              }
            }
          }
          else
          {
            ref DynamicBuffer<MeshGroup> local = ref newGroups;
            meshGroup = new MeshGroup();
            meshGroup.m_SubMeshGroup = ushort.MaxValue;
            meshGroup.m_MeshOffset = (byte) 0;
            MeshGroup elem = meshGroup;
            local.Add(elem);
          }
          if (length > 1)
          {
            for (int index4 = 0; index4 < nativeList.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              this.TryRemoveBatches(nativeList[index4], index4, newGroups, batches);
            }
            nativeList.Clear();
          }
          else if (length == 1)
          {
            // ISSUE: reference to a compiler-generated method
            this.TryRemoveBatches(oldGroup, 0, newGroups, batches);
          }
        }
        if (!nativeList.IsCreated)
          return;
        nativeList.Dispose();
      }

      private void TryRemoveBatches(
        MeshGroup oldGroup,
        int groupIndex,
        DynamicBuffer<MeshGroup> newGroups,
        DynamicBuffer<MeshBatch> batches)
      {
        for (int index = 0; index < newGroups.Length; ++index)
        {
          if ((int) newGroups[index].m_SubMeshGroup == (int) oldGroup.m_SubMeshGroup)
            return;
        }
        for (int index = 0; index < batches.Length; ++index)
        {
          MeshBatch batch = batches[index];
          if ((int) batch.m_MeshGroup == groupIndex)
          {
            batch.m_MeshGroup = byte.MaxValue;
            batch.m_MeshIndex = byte.MaxValue;
            batch.m_TileIndex = byte.MaxValue;
            batches[index] = batch;
          }
        }
      }

      public static MeshGroupFlags GetHumanFlags(Human human)
      {
        MeshGroupFlags meshGroupFlags1 = (MeshGroupFlags) 0;
        MeshGroupFlags meshGroupFlags2 = (human.m_Flags & HumanFlags.Cold) == (HumanFlags) 0 ? meshGroupFlags1 | MeshGroupFlags.RequireWarm : meshGroupFlags1 | MeshGroupFlags.RequireCold;
        return (human.m_Flags & HumanFlags.Homeless) == (HumanFlags) 0 ? meshGroupFlags2 | MeshGroupFlags.RequireHome : meshGroupFlags2 | MeshGroupFlags.RequireHomeless;
      }

      private MeshGroupFlags GetCurrentVehicleFlags(CurrentVehicle currentVehicle)
      {
        MeshGroupFlags currentVehicleFlags = MeshGroupFlags.ForbidMotorcycle;
        PrefabRef componentData;
        DynamicBuffer<ActivityLocationElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.TryGetComponent(currentVehicle.m_Vehicle, out componentData) && this.m_ActivityLocations.TryGetBuffer(componentData.m_Prefab, out bufferData))
        {
          ActivityMask activityMask = new ActivityMask(ActivityType.Driving);
          for (int index = 0; index < bufferData.Length; ++index)
          {
            if (((int) bufferData[index].m_ActivityMask.m_Mask & (int) activityMask.m_Mask) != 0)
              currentVehicleFlags = currentVehicleFlags & ~MeshGroupFlags.ForbidMotorcycle | MeshGroupFlags.RequireMotorcycle;
          }
        }
        return currentVehicleFlags;
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

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Human> __Game_Creatures_Human_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RW_BufferTypeHandle;
      public BufferTypeHandle<MeshBatch> __Game_Rendering_MeshBatch_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Human> __Game_Creatures_Human_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<OverlayElement> __Game_Prefabs_OverlayElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RW_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RW_BufferTypeHandle = state.GetBufferTypeHandle<MeshBatch>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentLookup = state.GetComponentLookup<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OverlayElement_RO_BufferLookup = state.GetBufferLookup<OverlayElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
      }
    }
  }
}
