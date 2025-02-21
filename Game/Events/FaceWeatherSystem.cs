// Decompiled with JetBrains decompiler
// Type: Game.Events.FaceWeatherSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Events
{
  [CompilerGenerated]
  public class FaceWeatherSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_FaceWeatherQuery;
    private EntityArchetype m_JournalDataArchetype;
    private FaceWeatherSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_FaceWeatherQuery = this.GetEntityQuery(ComponentType.ReadOnly<FaceWeather>(), ComponentType.ReadOnly<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.m_JournalDataArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<AddEventJournalData>(), ComponentType.ReadWrite<Game.Common.Event>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_FaceWeatherQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_FaceWeatherQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_FacingWeather_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_FaceWeather_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new FaceWeatherSystem.FaceWeatherJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_FaceWeatherType = this.__TypeHandle.__Game_Events_FaceWeather_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_FacingWeatherData = this.__TypeHandle.__Game_Events_FacingWeather_RW_ComponentLookup,
        m_TargetElements = this.__TypeHandle.__Game_Events_TargetElement_RW_BufferLookup,
        m_JournalDataArchetype = this.m_JournalDataArchetype,
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<FaceWeatherSystem.FaceWeatherJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      this.Dependency = jobHandle;
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
    public FaceWeatherSystem()
    {
    }

    [BurstCompile]
    private struct FaceWeatherJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeHandle<FaceWeather> m_FaceWeatherType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      public ComponentLookup<FacingWeather> m_FacingWeatherData;
      public BufferLookup<TargetElement> m_TargetElements;
      public EntityArchetype m_JournalDataArchetype;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          capacity += this.m_Chunks[index].Count;
        }
        NativeParallelHashMap<Entity, FacingWeather> nativeParallelHashMap = new NativeParallelHashMap<Entity, FacingWeather>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<FaceWeather> nativeArray = this.m_Chunks[index1].GetNativeArray<FaceWeather>(ref this.m_FaceWeatherType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            FaceWeather faceWeather = nativeArray[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(faceWeather.m_Target))
            {
              FacingWeather facingWeather1 = new FacingWeather(faceWeather.m_Event, faceWeather.m_Severity);
              FacingWeather facingWeather2;
              if (nativeParallelHashMap.TryGetValue(faceWeather.m_Target, out facingWeather2))
              {
                if ((double) facingWeather1.m_Severity > (double) facingWeather2.m_Severity)
                  nativeParallelHashMap[faceWeather.m_Target] = facingWeather1;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_FacingWeatherData.HasComponent(faceWeather.m_Target))
                {
                  // ISSUE: reference to a compiler-generated field
                  facingWeather2 = this.m_FacingWeatherData[faceWeather.m_Target];
                  if ((double) facingWeather1.m_Severity > (double) facingWeather2.m_Severity)
                    nativeParallelHashMap.TryAdd(faceWeather.m_Target, facingWeather1);
                }
                else
                  nativeParallelHashMap.TryAdd(faceWeather.m_Target, facingWeather1);
              }
            }
          }
        }
        if (nativeParallelHashMap.Count() == 0)
          return;
        NativeArray<Entity> keyArray = nativeParallelHashMap.GetKeyArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < keyArray.Length; ++index)
        {
          Entity entity = keyArray[index];
          FacingWeather facingWeather = nativeParallelHashMap[entity];
          // ISSUE: reference to a compiler-generated field
          if (this.m_FacingWeatherData.HasComponent(entity))
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_FacingWeatherData[entity].m_Event != facingWeather.m_Event)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_TargetElements.HasBuffer(facingWeather.m_Event))
              {
                // ISSUE: reference to a compiler-generated field
                CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[facingWeather.m_Event], new TargetElement(entity));
              }
              // ISSUE: reference to a compiler-generated method
              this.AddJournalData(facingWeather, entity);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_FacingWeatherData[entity] = facingWeather;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TargetElements.HasBuffer(facingWeather.m_Event))
            {
              // ISSUE: reference to a compiler-generated field
              CollectionUtils.TryAddUniqueValue<TargetElement>(this.m_TargetElements[facingWeather.m_Event], new TargetElement(entity));
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<FacingWeather>(entity, facingWeather);
            // ISSUE: reference to a compiler-generated method
            this.AddJournalData(facingWeather, entity);
          }
        }
      }

      private void AddJournalData(FacingWeather facingWeather, Entity target)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingData.HasComponent(target))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<AddEventJournalData>(this.m_CommandBuffer.CreateEntity(this.m_JournalDataArchetype), new AddEventJournalData(facingWeather.m_Event, EventDataTrackingType.Damages));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<FaceWeather> __Game_Events_FaceWeather_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      public ComponentLookup<FacingWeather> __Game_Events_FacingWeather_RW_ComponentLookup;
      public BufferLookup<TargetElement> __Game_Events_TargetElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_FaceWeather_RO_ComponentTypeHandle = state.GetComponentTypeHandle<FaceWeather>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_FacingWeather_RW_ComponentLookup = state.GetComponentLookup<FacingWeather>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_TargetElement_RW_BufferLookup = state.GetBufferLookup<TargetElement>();
      }
    }
  }
}
