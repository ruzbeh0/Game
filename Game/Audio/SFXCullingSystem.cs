// Decompiled with JetBrains decompiler
// Type: Game.Audio.SFXCullingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Effects;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Audio
{
  [CompilerGenerated]
  public class SFXCullingSystem : GameSystemBase
  {
    private AudioManager m_AudioManager;
    private EffectControlSystem m_EffectControlSystem;
    private EntityQuery m_CullingAudioSettingsQuery;
    private SFXCullingSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CullingAudioSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<CullingAudioSettingsData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      Camera main = Camera.main;
      if ((UnityEngine.Object) main == (UnityEngine.Object) null)
        return;
      int num = 4;
      NativeParallelQueue<SFXCullingSystem.CullingGroupItem> nativeParallelQueue = new NativeParallelQueue<SFXCullingSystem.CullingGroupItem>(num, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<EnabledEffectData> enabledData = this.m_EffectControlSystem.GetEnabledData(false, out dependencies);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      SourceUpdateData sourceUpdateData = this.m_AudioManager.GetSourceUpdateData(out deps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AudioSpotData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CullingGroupData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: variable of a compiler-generated type
      SFXCullingSystem.SFXCullingJob jobData = new SFXCullingSystem.SFXCullingJob()
      {
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_CullingGroupData = this.__TypeHandle.__Game_Prefabs_CullingGroupData_RO_ComponentLookup,
        m_AudioSpotData = this.__TypeHandle.__Game_Prefabs_AudioSpotData_RO_ComponentLookup,
        m_AudioEffectDatas = this.__TypeHandle.__Game_Prefabs_AudioEffectData_RO_ComponentLookup,
        m_AudioSourceDatas = this.__TypeHandle.__Game_Prefabs_AudioSourceData_RO_BufferLookup,
        m_PrefabEffects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_CameraPosition = (float3) main.transform.position,
        m_RandomSeed = RandomSeed.Next(),
        m_DeltaTime = UnityEngine.Time.deltaTime,
        m_EnabledData = enabledData,
        m_CullingGroupItems = nativeParallelQueue.AsWriter(),
        m_SourceUpdateData = sourceUpdateData
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<SFXCullingSystem.SFXCullingJob, EnabledEffectData>(jobData.m_EnabledData, 16, JobHandle.CombineDependencies(dependencies, deps, this.Dependency));
      JobHandle jobHandle = this.Dependency;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CullingAudioSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        CullingAudioSettingsData singleton = this.m_CullingAudioSettingsQuery.GetSingleton<CullingAudioSettingsData>();
        // ISSUE: object of a compiler-generated type is created
        jobHandle = new SFXCullingSystem.SFXGroupCullingJob()
        {
          m_MaxAllowedAmount = singleton.m_PublicTransCullMaxAmount,
          m_MaxDistance = singleton.m_PublicTransCullMaxDistance,
          m_CullingGroupItems = nativeParallelQueue.AsReader(),
          m_EnabledData = enabledData,
          m_SourceUpdateData = sourceUpdateData
        }.Schedule<SFXCullingSystem.SFXGroupCullingJob>(num, 1, jobHandle);
      }
      nativeParallelQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.AddEnabledDataWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.AddSourceUpdateWriter(jobHandle);
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
    public SFXCullingSystem()
    {
    }

    [BurstCompile]
    private struct SFXCullingJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<CullingGroupData> m_CullingGroupData;
      [ReadOnly]
      public ComponentLookup<AudioSpotData> m_AudioSpotData;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> m_AudioEffectDatas;
      [ReadOnly]
      public BufferLookup<AudioSourceData> m_AudioSourceDatas;
      [ReadOnly]
      public BufferLookup<Effect> m_PrefabEffects;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public float m_DeltaTime;
      [NativeDisableParallelForRestriction]
      public NativeList<EnabledEffectData> m_EnabledData;
      public NativeParallelQueue<SFXCullingSystem.CullingGroupItem>.Writer m_CullingGroupItems;
      public SourceUpdateData m_SourceUpdateData;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref EnabledEffectData local = ref this.m_EnabledData.ElementAt(index);
        if ((local.m_Flags & EnabledEffectFlags.IsAudio) == (EnabledEffectFlags) 0)
          return;
        if ((local.m_Flags & EnabledEffectFlags.IsEnabled) == (EnabledEffectFlags) 0)
        {
          if ((local.m_Flags & EnabledEffectFlags.WrongPrefab) != (EnabledEffectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SourceUpdateData.WrongPrefab(new SourceInfo(local.m_Owner, local.m_EffectIndex));
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_SourceUpdateData.Remove(new SourceInfo(local.m_Owner, local.m_EffectIndex));
          }
          local.m_NextTime = -1f;
        }
        else
        {
          CullingGroupData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CullingGroupData.TryGetComponent(local.m_Prefab, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: object of a compiler-generated type is created
            this.m_CullingGroupItems.Enqueue(componentData1.m_GroupIndex, new SFXCullingSystem.CullingGroupItem()
            {
              m_EnabledIndex = index,
              m_DistanceSq = math.distancesq(local.m_Position, this.m_CameraPosition)
            });
          }
          else
          {
            float3 x = local.m_Position;
            float num = float.MaxValue;
            DynamicBuffer<AudioSourceData> bufferData;
            AudioEffectData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_AudioSourceDatas.TryGetBuffer(local.m_Prefab, out bufferData) && bufferData.Length > 0 && this.m_AudioEffectDatas.TryGetComponent(bufferData[0].m_SFXEntity, out componentData2))
            {
              num = componentData2.m_MaxDistance;
              if ((double) componentData2.m_SourceSize.x > 0.0 || (double) componentData2.m_SourceSize.y > 0.0 || (double) componentData2.m_SourceSize.z > 0.0)
              {
                float3 sourceOffset = new float3();
                if ((local.m_Flags & EnabledEffectFlags.EditorContainer) == (EnabledEffectFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  sourceOffset = this.m_PrefabEffects[this.m_Prefabs[local.m_Owner].m_Prefab][local.m_EffectIndex].m_Position;
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                x = AudioManager.GetClosestSourcePosition(this.m_CameraPosition, new Game.Objects.Transform(local.m_Position, local.m_Rotation), sourceOffset, componentData2.m_SourceSize);
              }
            }
            // ISSUE: reference to a compiler-generated field
            if ((double) math.distancesq(x, this.m_CameraPosition) >= (double) num * (double) num)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Remove(new SourceInfo(local.m_Owner, local.m_EffectIndex));
              local.m_NextTime = -1f;
            }
            else
            {
              AudioSpotData componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_AudioSpotData.TryGetComponent(local.m_Prefab, out componentData3))
              {
                if ((double) local.m_NextTime <= 0.0)
                {
                  // ISSUE: reference to a compiler-generated field
                  Unity.Mathematics.Random random = this.m_RandomSeed.GetRandom(index);
                  local.m_NextTime = random.NextFloat(componentData3.m_Interval.y);
                  return;
                }
                // ISSUE: reference to a compiler-generated field
                local.m_NextTime -= this.m_DeltaTime;
                if ((double) local.m_NextTime >= 0.0)
                  return;
                // ISSUE: reference to a compiler-generated field
                Unity.Mathematics.Random random1 = this.m_RandomSeed.GetRandom(index);
                local.m_NextTime = random1.NextFloat(componentData3.m_Interval.x, componentData3.m_Interval.y);
              }
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Add(new SourceInfo(local.m_Owner, local.m_EffectIndex));
            }
          }
        }
      }
    }

    private struct CullingGroupItem : IComparable<SFXCullingSystem.CullingGroupItem>
    {
      public int m_EnabledIndex;
      public float m_DistanceSq;

      public int CompareTo(SFXCullingSystem.CullingGroupItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_DistanceSq.CompareTo(other.m_DistanceSq);
      }
    }

    [BurstCompile]
    private struct SFXGroupCullingJob : IJobParallelFor
    {
      [ReadOnly]
      public int m_MaxAllowedAmount;
      [ReadOnly]
      public float m_MaxDistance;
      [ReadOnly]
      public NativeParallelQueue<SFXCullingSystem.CullingGroupItem>.Reader m_CullingGroupItems;
      [NativeDisableParallelForRestriction]
      public NativeList<EnabledEffectData> m_EnabledData;
      public SourceUpdateData m_SourceUpdateData;

      public void Execute(int groupIndex)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<SFXCullingSystem.CullingGroupItem> array = this.m_CullingGroupItems.ToArray(groupIndex, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        array.Sort<SFXCullingSystem.CullingGroupItem>();
        int index = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num = this.m_MaxDistance * this.m_MaxDistance;
        // ISSUE: reference to a compiler-generated field
        for (; index < array.Length && index < this.m_MaxAllowedAmount; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          SFXCullingSystem.CullingGroupItem cullingGroupItem = array[index];
          // ISSUE: reference to a compiler-generated field
          if ((double) cullingGroupItem.m_DistanceSq <= (double) num)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ref EnabledEffectData local = ref this.m_EnabledData.ElementAt(cullingGroupItem.m_EnabledIndex);
            if ((local.m_Flags & (EnabledEffectFlags.EnabledUpdated | EnabledEffectFlags.AudioDisabled)) != (EnabledEffectFlags) 0)
            {
              local.m_Flags &= ~EnabledEffectFlags.AudioDisabled;
              // ISSUE: reference to a compiler-generated field
              this.m_SourceUpdateData.Add(new SourceInfo(local.m_Owner, local.m_EffectIndex));
            }
          }
          else
            break;
        }
        for (; index < array.Length; ++index)
        {
          // ISSUE: variable of a compiler-generated type
          SFXCullingSystem.CullingGroupItem cullingGroupItem = array[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_EnabledData.ElementAt(cullingGroupItem.m_EnabledIndex).m_Flags |= EnabledEffectFlags.AudioDisabled;
        }
        array.Dispose();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingGroupData> __Game_Prefabs_CullingGroupData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AudioSpotData> __Game_Prefabs_AudioSpotData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AudioEffectData> __Game_Prefabs_AudioEffectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<AudioSourceData> __Game_Prefabs_AudioSourceData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CullingGroupData_RO_ComponentLookup = state.GetComponentLookup<CullingGroupData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSpotData_RO_ComponentLookup = state.GetComponentLookup<AudioSpotData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioEffectData_RO_ComponentLookup = state.GetComponentLookup<AudioEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AudioSourceData_RO_BufferLookup = state.GetBufferLookup<AudioSourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
      }
    }
  }
}
