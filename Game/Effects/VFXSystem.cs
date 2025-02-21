// Decompiled with JetBrains decompiler
// Type: Game.Effects.VFXSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Entities;
using Game.Prefabs;
using Game.Rendering;
using Game.Rendering.Utilities;
using Game.Serialization;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.VFX;

#nullable disable
namespace Game.Effects
{
  [CompilerGenerated]
  public class VFXSystem : GameSystemBase, IPreDeserialize
  {
    private Queue<NativeQueue<VFXUpdateInfo>> m_SourceUpdateQueue;
    private JobHandle m_SourceUpdateWriter;
    private EntityQuery m_VFXPrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private bool m_Initialized;
    private VFXSystem.EffectInfo[] m_Effects;
    private JobHandle m_TextureUpdate;
    private WindTextureSystem m_WindTextureSystem;
    private TerrainSystem m_TerrainSystem;
    private EffectControlSystem m_EffectControlSystem;
    private RenderingSystem m_RenderingSystem;

    public NativeQueue<VFXUpdateInfo> GetSourceUpdateData()
    {
      NativeQueue<VFXUpdateInfo> sourceUpdateData = new NativeQueue<VFXUpdateInfo>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateQueue.Enqueue(sourceUpdateData);
      return sourceUpdateData;
    }

    public void AddSourceUpdateWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateWriter = JobHandle.CombineDependencies(this.m_SourceUpdateWriter, jobHandle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateQueue = new Queue<NativeQueue<VFXUpdateInfo>>();
      // ISSUE: reference to a compiler-generated field
      this.m_VFXPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<VFXData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindTextureSystem = this.World.GetOrCreateSystemManaged<WindTextureSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
    }

    private bool Initialize()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_Initialized || this.m_VFXPrefabQuery.IsEmptyIgnoreFilter)
        return false;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_VFXPrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<VFXData> componentDataArray = this.m_VFXPrefabQuery.ToComponentDataArray<VFXData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_Effects = new VFXSystem.EffectInfo[entityArray.Length];
      for (int index = 0; index < entityArray.Length; ++index)
      {
        EntityManager entityManager = this.World.EntityManager;
        entityManager.GetComponentData<VFXData>(entityArray[index]);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        EffectPrefab prefab = this.m_PrefabSystem.GetPrefab<EffectPrefab>(entityArray[index]);
        Game.Prefabs.VFX component = prefab.GetComponent<Game.Prefabs.VFX>();
        VisualEffect effect = new GameObject("VFX " + prefab.name).AddComponent<VisualEffect>();
        effect.visualEffectAsset = component.m_Effect;
        // ISSUE: reference to a compiler-generated field
        effect.SetCheckedInt(VFXSystem.VFXIDs.Count, 0);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Effects[index].m_VisualEffect = effect;
        VFXData componentData = componentDataArray[index] with
        {
          m_MaxCount = component.m_MaxCount,
          m_Index = index
        };
        entityManager = this.World.EntityManager;
        entityManager.SetComponentData<VFXData>(entityArray[index], componentData);
        Texture2D texture2D = new Texture2D(component.m_MaxCount, 3, GraphicsFormat.R32G32B32A32_SFloat, 1, TextureCreationFlags.None);
        texture2D.name = "VFXTexture " + prefab.name;
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        Texture2D v = texture2D;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Effects[index].m_Texture = v;
        // ISSUE: reference to a compiler-generated field
        effect.SetCheckedTexture(VFXSystem.VFXIDs.InstanceData, (Texture) v);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Effects[index].m_Instances = new NativeArray<int>(componentData.m_MaxCount, Allocator.Persistent);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Effects[index].m_Indices = new NativeParallelHashMap<int, int>(componentData.m_MaxCount, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      }
      componentDataArray.Dispose();
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_Initialized = true;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_Initialized && this.m_Effects != null)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Effects.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Effects[index].m_Texture);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Effects[index].m_Instances.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_Instances.Dispose();
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Effects[index].m_Indices.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_Indices.Dispose();
          }
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.ClearQueue();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_Initialized && !this.Initialize())
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearQueue();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TextureUpdate.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_SourceUpdateWriter.Complete();
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeList<EnabledEffectData> enabledData = this.m_EffectControlSystem.GetEnabledData(true, out dependencies);
        NativeQueue<VFXUpdateInfo> nativeQueue;
        // ISSUE: reference to a compiler-generated field
        while (this.m_SourceUpdateQueue.TryDequeue(ref nativeQueue))
        {
          if (!nativeQueue.IsEmpty())
          {
            dependencies.Complete();
            VFXUpdateInfo vfxUpdateInfo;
            while (nativeQueue.TryDequeue(out vfxUpdateInfo))
            {
              VFXData component;
              if (this.EntityManager.TryGetComponent<VFXData>(enabledData[vfxUpdateInfo.m_EnabledIndex.x].m_Prefab, out component))
              {
                int index1 = component.m_Index;
                switch (vfxUpdateInfo.m_Type)
                {
                  case VFXUpdateType.Add:
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (!this.m_Effects[index1].m_Indices.ContainsKey(vfxUpdateInfo.m_EnabledIndex.x) && index1 >= 0 && this.m_Effects[index1].m_Indices.Count() < this.m_Effects[index1].m_Instances.Length)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      int index2 = this.m_Effects[index1].m_Indices.Count();
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Instances[index2] = vfxUpdateInfo.m_EnabledIndex.x;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Indices[vfxUpdateInfo.m_EnabledIndex.x] = index2;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((UnityEngine.Object) this.m_Effects[index1].m_VisualEffect != (UnityEngine.Object) null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_Effects[index1].m_VisualEffect.SetCheckedInt(VFXSystem.VFXIDs.Count, this.m_Effects[index1].m_Indices.Count());
                        continue;
                      }
                      continue;
                    }
                    continue;
                  case VFXUpdateType.Remove:
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Effects[index1].m_Indices.ContainsKey(vfxUpdateInfo.m_EnabledIndex.x))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      int instance = this.m_Effects[index1].m_Instances[this.m_Effects[index1].m_Indices.Count() - 1];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      int index3 = this.m_Effects[index1].m_Indices[vfxUpdateInfo.m_EnabledIndex.x];
                      if (vfxUpdateInfo.m_EnabledIndex.x != instance)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_Effects[index1].m_Instances[index3] = instance;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_Effects[index1].m_Indices[instance] = index3;
                      }
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Instances[this.m_Effects[index1].m_Indices.Count() - 1] = -1;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Indices.Remove(vfxUpdateInfo.m_EnabledIndex.x);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if ((UnityEngine.Object) this.m_Effects[index1].m_VisualEffect != (UnityEngine.Object) null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.m_Effects[index1].m_VisualEffect.SetCheckedInt(VFXSystem.VFXIDs.Count, this.m_Effects[index1].m_Indices.Count());
                        continue;
                      }
                      continue;
                    }
                    continue;
                  case VFXUpdateType.MoveIndex:
                    int index4;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_Effects[index1].m_Indices.TryGetValue(vfxUpdateInfo.m_EnabledIndex.y, out index4))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Indices.Remove(vfxUpdateInfo.m_EnabledIndex.y);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Indices[vfxUpdateInfo.m_EnabledIndex.x] = index4;
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      this.m_Effects[index1].m_Instances[index4] = vfxUpdateInfo.m_EnabledIndex.x;
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
          nativeQueue.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        float num1 = this.m_RenderingSystem.frameDelta / math.max(1E-06f, this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime * 60f);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Effects.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_Effects[index].m_VisualEffect != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_VisualEffect.playRate = num1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_VisualEffect.SetCheckedTexture(VFXSystem.VFXIDs.WindTexture, (Texture) this.m_WindTextureSystem.WindTexture);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_VisualEffect.SetCheckedVector4(VFXSystem.VFXIDs.MapOffsetScale, this.m_TerrainSystem.mapOffsetScale);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num2 = math.max(this.m_Effects[index].m_Indices.Count(), this.m_Effects[index].m_LastCount);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Effects[index].m_NeedApply)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_Texture.Apply();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_NeedApply = false;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Effects[index].m_VisualEffect.SetCheckedInt(VFXSystem.VFXIDs.Count, this.m_Effects[index].m_LastCount);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Effects[index].m_LastCount = this.m_Effects[index].m_Indices.Count();
          if (num2 != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            VFXSystem.VFXTextureUpdateJob jobData = new VFXSystem.VFXTextureUpdateJob()
            {
              m_TextureData = this.m_Effects[index].m_Texture.GetRawTextureData<float4>(),
              m_Instances = this.m_Effects[index].m_Instances,
              m_EnabledData = enabledData,
              m_Count = num2,
              m_TextureWidth = this.m_Effects[index].m_Texture.width
            };
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_NeedApply = true;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureUpdate = JobHandle.CombineDependencies(this.m_TextureUpdate, jobData.Schedule<VFXSystem.VFXTextureUpdateJob>(dependencies));
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_EffectControlSystem.AddEnabledDataReader(this.m_TextureUpdate);
      }
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_Initialized && this.m_Effects != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TextureUpdate.Complete();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Effects.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((UnityEngine.Object) this.m_Effects[index].m_VisualEffect != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_VisualEffect.SetCheckedInt(VFXSystem.VFXIDs.Count, 0);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Effects[index].m_VisualEffect.gameObject);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Effects[index].m_Instances.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_Instances.Dispose();
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_Effects[index].m_Indices.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_Effects[index].m_Indices.Dispose();
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Initialized = false;
      }
      // ISSUE: reference to a compiler-generated method
      this.ClearQueue();
    }

    private void ClearQueue()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SourceUpdateWriter.Complete();
      NativeQueue<VFXUpdateInfo> nativeQueue;
      // ISSUE: reference to a compiler-generated field
      while (this.m_SourceUpdateQueue.TryDequeue(ref nativeQueue))
        nativeQueue.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    public VFXSystem()
    {
    }

    private static class VFXIDs
    {
      public static readonly int WindTexture = Shader.PropertyToID(nameof (WindTexture));
      public static readonly int MapOffsetScale = Shader.PropertyToID(nameof (MapOffsetScale));
      public static readonly int Count = Shader.PropertyToID(nameof (Count));
      public static readonly int InstanceData = Shader.PropertyToID(nameof (InstanceData));
    }

    private struct EffectInfo
    {
      public VisualEffect m_VisualEffect;
      public Texture2D m_Texture;
      public NativeArray<int> m_Instances;
      public NativeParallelHashMap<int, int> m_Indices;
      public int m_LastCount;
      public bool m_NeedApply;
    }

    [BurstCompile]
    private struct VFXTextureUpdateJob : IJob
    {
      public NativeArray<float4> m_TextureData;
      [ReadOnly]
      public NativeArray<int> m_Instances;
      [ReadOnly]
      public NativeList<EnabledEffectData> m_EnabledData;
      public int m_Count;
      public int m_TextureWidth;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int instance = this.m_Instances[index];
          if (instance == -1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData.ElementAt<float4>(index).w = 0.0f;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            EnabledEffectData enabledEffectData = this.m_EnabledData[instance];
            Quaternion rotation = (Quaternion) enabledEffectData.m_Rotation;
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index] = new float4(enabledEffectData.m_Position, enabledEffectData.m_Intensity);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index + this.m_TextureWidth] = new float4((float3) ((float) Math.PI / 180f * rotation.eulerAngles), 0.0f);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TextureData[index + 2 * this.m_TextureWidth] = new float4(enabledEffectData.m_Scale, 0.0f);
          }
        }
      }
    }
  }
}
