// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.LotInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class LotInitializeSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PrefabQuery;
    private LotInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<LotData>(), ComponentType.ReadWrite<AreaGeometryData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_PrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new LotInitializeSystem.InitializeLotPrefabsJob()
      {
        m_SubObjectType = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferTypeHandle,
        m_AreaGeometryType = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PlaceholderObjectElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup
      }.ScheduleParallel<LotInitializeSystem.InitializeLotPrefabsJob>(this.m_PrefabQuery, this.Dependency);
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
    public LotInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeLotPrefabsJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      public ComponentTypeHandle<AreaGeometryData> m_AreaGeometryType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PlaceholderObjectElements;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<AreaGeometryData> nativeArray = chunk.GetNativeArray<AreaGeometryData>(ref this.m_AreaGeometryType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          AreaGeometryData areaGeometryData = nativeArray[index1] with
          {
            m_MaxHeight = 0.0f
          };
          DynamicBuffer<SubObject> dynamicBuffer;
          if (CollectionUtils.TryGet<SubObject>(bufferAccessor, index1, out dynamicBuffer))
          {
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              SubObject subObject = dynamicBuffer[index2];
              DynamicBuffer<PlaceholderObjectElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PlaceholderObjectElements.TryGetBuffer(subObject.m_Prefab, out bufferData))
              {
                for (int index3 = 0; index3 < bufferData.Length; ++index3)
                {
                  ObjectGeometryData componentData;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_ObjectGeometryData.TryGetComponent(bufferData[index3].m_Object, out componentData))
                    areaGeometryData.m_MaxHeight = math.max(areaGeometryData.m_MaxHeight, componentData.m_Bounds.max.y);
                }
              }
              else
              {
                ObjectGeometryData componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ObjectGeometryData.TryGetComponent(subObject.m_Prefab, out componentData))
                  areaGeometryData.m_MaxHeight = math.max(areaGeometryData.m_MaxHeight, componentData.m_Bounds.max.y);
              }
            }
          }
          nativeArray[index1] = areaGeometryData;
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

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Prefabs_SubObject_RO_BufferTypeHandle;
      public ComponentTypeHandle<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AreaGeometryData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
      }
    }
  }
}
