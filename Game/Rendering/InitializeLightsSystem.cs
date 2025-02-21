// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InitializeLightsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class InitializeLightsSystem : GameSystemBase
  {
    private ProceduralEmissiveSystem m_ProceduralEmissiveSystem;
    private PreCullingSystem m_PreCullingSystem;
    private InitializeLightsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralEmissiveSystem = this.World.GetOrCreateSystemManaged<ProceduralEmissiveSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      NativeReference<ProceduralEmissiveSystem.AllocationInfo> allocationInfo;
      NativeQueue<ProceduralEmissiveSystem.AllocationRemove> allocationRemoves;
      int currentTime;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeHeapAllocator heapAllocator = this.m_ProceduralEmissiveSystem.GetHeapAllocator(out allocationInfo, out allocationRemoves, out currentTime, out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new InitializeLightsSystem.ProceduralInitializeJob()
      {
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ProceduralLights = this.__TypeHandle.__Game_Prefabs_ProceduralLight_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_Emissives = this.__TypeHandle.__Game_Rendering_Emissive_RW_BufferLookup,
        m_Lights = this.__TypeHandle.__Game_Rendering_LightState_RW_BufferLookup,
        m_CurrentTime = currentTime,
        m_CullingData = this.m_PreCullingSystem.GetUpdatedData(true, out dependencies2),
        m_HeapAllocator = heapAllocator,
        m_AllocationInfo = allocationInfo,
        m_AllocationRemoves = allocationRemoves
      }.Schedule<InitializeLightsSystem.ProceduralInitializeJob>(JobHandle.CombineDependencies(this.Dependency, dependencies2, dependencies1));
      // ISSUE: reference to a compiler-generated field
      this.m_ProceduralEmissiveSystem.AddHeapWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle);
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
    public InitializeLightsSystem()
    {
    }

    [BurstCompile]
    private struct ProceduralInitializeJob : IJob
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<ProceduralLight> m_ProceduralLights;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      public BufferLookup<Emissive> m_Emissives;
      public BufferLookup<LightState> m_Lights;
      [ReadOnly]
      public int m_CurrentTime;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public NativeHeapAllocator m_HeapAllocator;
      public NativeReference<ProceduralEmissiveSystem.AllocationInfo> m_AllocationInfo;
      public NativeQueue<ProceduralEmissiveSystem.AllocationRemove> m_AllocationRemoves;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        ref ProceduralEmissiveSystem.AllocationInfo local = ref this.m_AllocationInfo.ValueAsRef<ProceduralEmissiveSystem.AllocationInfo>();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CullingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          PreCullingData cullingData = this.m_CullingData[index];
          if ((cullingData.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated)) != (PreCullingFlags) 0 && (cullingData.m_Flags & PreCullingFlags.Emissive) != (PreCullingFlags) 0)
          {
            if ((cullingData.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.Remove(cullingData);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Update(cullingData, ref local);
            }
          }
        }
      }

      private void Remove(PreCullingData cullingData)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Emissive> emissive = this.m_Emissives[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LightState> light = this.m_Lights[cullingData.m_Entity];
        // ISSUE: reference to a compiler-generated method
        this.Deallocate(emissive);
        emissive.Clear();
        light.Clear();
      }

      private void Update(
        PreCullingData cullingData,
        ref ProceduralEmissiveSystem.AllocationInfo allocationInfo)
      {
        DynamicBuffer<SubMesh> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubMeshes.TryGetBuffer(this.m_PrefabRefData[cullingData.m_Entity].m_Prefab, out bufferData))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Emissive> emissive = this.m_Emissives[cullingData.m_Entity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<LightState> light = this.m_Lights[cullingData.m_Entity];
          int length = 0;
          for (int index = 0; index < bufferData.Length; ++index)
          {
            SubMesh subMesh = bufferData[index];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProceduralLights.HasBuffer(subMesh.m_SubMesh))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralLight> proceduralLight = this.m_ProceduralLights[subMesh.m_SubMesh];
              length += proceduralLight.Length;
            }
          }
          if (emissive.Length == bufferData.Length && light.Length == length)
            return;
          // ISSUE: reference to a compiler-generated method
          this.Deallocate(emissive);
          emissive.ResizeUninitialized(bufferData.Length);
          light.ResizeUninitialized(length);
          int num = 0;
          for (int index1 = 0; index1 < bufferData.Length; ++index1)
          {
            SubMesh subMesh = bufferData[index1];
            // ISSUE: reference to a compiler-generated field
            if (this.m_ProceduralLights.HasBuffer(subMesh.m_SubMesh))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ProceduralLight> proceduralLight = this.m_ProceduralLights[subMesh.m_SubMesh];
              // ISSUE: reference to a compiler-generated field
              NativeHeapBlock nativeHeapBlock = this.m_HeapAllocator.Allocate((uint) (proceduralLight.Length + 1));
              if (nativeHeapBlock.Empty)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_HeapAllocator.Resize(this.m_HeapAllocator.Size + 1048576U / (uint) sizeof (float4));
                // ISSUE: reference to a compiler-generated field
                nativeHeapBlock = this.m_HeapAllocator.Allocate((uint) (proceduralLight.Length + 1));
              }
              ++allocationInfo.m_AllocationCount;
              emissive[index1] = new Emissive()
              {
                m_BufferAllocation = nativeHeapBlock,
                m_LightOffset = num,
                m_Updated = true
              };
              for (int index2 = 0; index2 < proceduralLight.Length; ++index2)
                light[num++] = new LightState()
                {
                  m_Intensity = 0.0f,
                  m_Color = 0.0f
                };
            }
            else
              emissive[index1] = new Emissive()
              {
                m_LightOffset = -1
              };
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.Remove(cullingData);
        }
      }

      private void Deallocate(DynamicBuffer<Emissive> emissives)
      {
        for (int index = 0; index < emissives.Length; ++index)
        {
          Emissive emissive = emissives[index];
          if (!emissive.m_BufferAllocation.Empty)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AllocationRemoves.Enqueue(new ProceduralEmissiveSystem.AllocationRemove()
            {
              m_Allocation = emissive.m_BufferAllocation,
              m_RemoveTime = this.m_CurrentTime
            });
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ProceduralLight> __Game_Prefabs_ProceduralLight_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      public BufferLookup<Emissive> __Game_Rendering_Emissive_RW_BufferLookup;
      public BufferLookup<LightState> __Game_Rendering_LightState_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ProceduralLight_RO_BufferLookup = state.GetBufferLookup<ProceduralLight>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_Emissive_RW_BufferLookup = state.GetBufferLookup<Emissive>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_LightState_RW_BufferLookup = state.GetBufferLookup<LightState>();
      }
    }
  }
}
