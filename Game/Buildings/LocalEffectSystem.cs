// Decompiled with JetBrains decompiler
// Type: Game.Buildings.LocalEffectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class LocalEffectSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedProvidersQuery;
    private EntityQuery m_AllProvidersQuery;
    private NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> m_SearchTree;
    private JobHandle m_ReadDependencies;
    private JobHandle m_WriteDependencies;
    private bool m_Loaded;
    private LocalEffectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedProvidersQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<LocalEffectProvider>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_AllProvidersQuery = this.GetEntityQuery(ComponentType.ReadOnly<LocalEffectProvider>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree = new NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds>(1f, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SearchTree.Dispose();
      base.OnDestroy();
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
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      EntityQuery entityQuery = loaded ? this.m_AllProvidersQuery : this.m_UpdatedProvidersQuery;
      if (entityQuery.IsEmptyIgnoreFilter)
        return;
      JobHandle outJobHandle;
      NativeList<ArchetypeChunk> archetypeChunkListAsync = entityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Signature_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated method
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
      JobHandle jobHandle = new LocalEffectSystem.UpdateLocalEffectsJob()
      {
        m_Loaded = loaded,
        m_Chunks = archetypeChunkListAsync,
        m_SearchTree = this.GetSearchTree(false, out dependencies),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_DestroyedType = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_SignatureType = this.__TypeHandle.__Game_Buildings_Signature_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_LocalModifierData = this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup
      }.Schedule<LocalEffectSystem.UpdateLocalEffectsJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated method
      this.AddLocalEffectWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    public LocalEffectSystem.ReadData GetReadData(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new LocalEffectSystem.ReadData(this.m_SearchTree);
    }

    public NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> GetSearchTree(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_WriteDependencies : JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies);
      // ISSUE: reference to a compiler-generated field
      return this.m_SearchTree;
    }

    public void AddLocalEffectReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, jobHandle);
    }

    public void AddLocalEffectWriter(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = JobHandle.CombineDependencies(this.m_WriteDependencies, jobHandle);
    }

    public static void InitializeTempList(
      NativeList<LocalModifierData> tempModifierList,
      DynamicBuffer<LocalModifierData> localModifiers)
    {
      tempModifierList.Clear();
      tempModifierList.AddRange(localModifiers.AsNativeArray());
    }

    public static void AddToTempList(
      NativeList<LocalModifierData> tempModifierList,
      DynamicBuffer<LocalModifierData> localModifiers,
      bool disabled)
    {
label_15:
      for (int index1 = 0; index1 < localModifiers.Length; ++index1)
      {
        LocalModifierData localModifier = localModifiers[index1];
        if (disabled)
        {
          localModifier.m_Delta = new Bounds1();
          localModifier.m_Radius = new Bounds1();
        }
        for (int index2 = 0; index2 < tempModifierList.Length; ++index2)
        {
          LocalModifierData tempModifier = tempModifierList[index2];
          if (tempModifier.m_Type == localModifier.m_Type)
          {
            if (tempModifier.m_Mode != localModifier.m_Mode)
              throw new Exception(string.Format("Modifier mode mismatch (type: {0})", (object) localModifier.m_Type));
            tempModifier.m_Delta.min += localModifier.m_Delta.min;
            tempModifier.m_Delta.max += localModifier.m_Delta.max;
            switch (tempModifier.m_RadiusCombineMode)
            {
              case ModifierRadiusCombineMode.Maximal:
                tempModifier.m_Radius.min = math.max(tempModifier.m_Radius.min, localModifier.m_Radius.min);
                tempModifier.m_Radius.max = math.max(tempModifier.m_Radius.max, localModifier.m_Radius.max);
                break;
              case ModifierRadiusCombineMode.Additive:
                tempModifier.m_Radius.min += localModifier.m_Radius.min;
                tempModifier.m_Radius.max += localModifier.m_Radius.max;
                break;
            }
            tempModifierList[index2] = tempModifier;
            goto label_15;
          }
        }
        tempModifierList.Add(in localModifier);
      }
    }

    public static bool GetEffectBounds(
      Transform transform,
      LocalModifierData localModifier,
      out LocalEffectSystem.EffectBounds effectBounds)
    {
      float max1 = localModifier.m_Radius.max;
      float max2 = localModifier.m_Delta.max;
      Bounds2 bounds = new Bounds2(transform.m_Position.xz - max1, transform.m_Position.xz + max1);
      uint typeMask = 1U << (int) (localModifier.m_Type & (LocalModifierType) 31);
      float num = math.select(max2, (float) (1.0 / (double) math.max(1f / 1000f, 1f + max2) - 1.0), localModifier.m_Mode == ModifierValueMode.InverseRelative);
      float2 delta = math.select(new float2(0.0f, num), new float2(num, 0.0f), localModifier.m_Mode == ModifierValueMode.Absolute);
      // ISSUE: object of a compiler-generated type is created
      effectBounds = new LocalEffectSystem.EffectBounds(bounds, typeMask, delta);
      return (double) max1 >= 1.0 && (double) num != 0.0;
    }

    public static bool GetEffectBounds(
      Transform transform,
      float efficiency,
      LocalModifierData localModifier,
      out LocalEffectSystem.EffectBounds effectBounds)
    {
      efficiency = math.sqrt(efficiency);
      float num1 = math.lerp(localModifier.m_Radius.min, localModifier.m_Radius.max, math.sqrt(efficiency));
      float a = math.lerp(localModifier.m_Delta.min, localModifier.m_Delta.max, efficiency);
      Bounds2 bounds = new Bounds2(transform.m_Position.xz - num1, transform.m_Position.xz + num1);
      uint typeMask = 1U << (int) (localModifier.m_Type & (LocalModifierType) 31);
      float num2 = math.select(a, (float) (1.0 / (double) math.max(1f / 1000f, 1f + a) - 1.0), localModifier.m_Mode == ModifierValueMode.InverseRelative);
      float2 delta = math.select(new float2(0.0f, num2), new float2(num2, 0.0f), localModifier.m_Mode == ModifierValueMode.Absolute);
      // ISSUE: object of a compiler-generated type is created
      effectBounds = new LocalEffectSystem.EffectBounds(bounds, typeMask, delta);
      return (double) num1 >= 1.0 && (double) num2 != 0.0;
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
    public LocalEffectSystem()
    {
    }

    public struct EffectItem : IEquatable<LocalEffectSystem.EffectItem>
    {
      public Entity m_Provider;
      public LocalModifierType m_Type;

      public EffectItem(Entity provider, LocalModifierType type)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Provider = provider;
        // ISSUE: reference to a compiler-generated field
        this.m_Type = type;
      }

      public bool Equals(LocalEffectSystem.EffectItem other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Provider.Equals(other.m_Provider) && this.m_Type == other.m_Type;
      }

      public override int GetHashCode()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) ((LocalModifierType) this.m_Provider.GetHashCode() ^ this.m_Type);
      }
    }

    public struct EffectBounds : 
      IEquatable<LocalEffectSystem.EffectBounds>,
      IBounds2<LocalEffectSystem.EffectBounds>
    {
      public Bounds2 m_Bounds;
      public uint m_TypeMask;
      public float2 m_Delta;

      public EffectBounds(Bounds2 bounds, uint typeMask, float2 delta)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds = bounds;
        // ISSUE: reference to a compiler-generated field
        this.m_TypeMask = typeMask;
        // ISSUE: reference to a compiler-generated field
        this.m_Delta = delta;
      }

      public bool Equals(LocalEffectSystem.EffectBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Bounds.Equals(other.m_Bounds) && (int) this.m_TypeMask == (int) other.m_TypeMask && this.m_Delta.Equals(other.m_Delta);
      }

      public void Reset()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds.Reset();
        // ISSUE: reference to a compiler-generated field
        this.m_TypeMask = 0U;
        // ISSUE: reference to a compiler-generated field
        this.m_Delta = new float2();
      }

      public float2 Center() => this.m_Bounds.Center();

      public float2 Size() => this.m_Bounds.Size();

      public LocalEffectSystem.EffectBounds Merge(LocalEffectSystem.EffectBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new LocalEffectSystem.EffectBounds()
        {
          m_Bounds = this.m_Bounds.Merge(other.m_Bounds),
          m_TypeMask = this.m_TypeMask | other.m_TypeMask
        };
      }

      public bool Intersect(LocalEffectSystem.EffectBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Bounds.Intersect(other.m_Bounds) && (this.m_TypeMask & other.m_TypeMask) > 0U;
      }
    }

    public struct ReadData
    {
      private NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> m_SearchTree;

      public ReadData(
        NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> searchTree)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree = searchTree;
      }

      public void ApplyModifier(ref float value, float3 position, LocalModifierType type)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LocalEffectSystem.ReadData.Iterator iterator = new LocalEffectSystem.ReadData.Iterator()
        {
          m_Position = position.xz,
          m_TypeMask = 1U << (int) (type & (LocalModifierType) 31)
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<LocalEffectSystem.ReadData.Iterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        value += iterator.m_Delta.x;
        // ISSUE: reference to a compiler-generated field
        value += value * iterator.m_Delta.y;
      }

      private struct Iterator : 
        INativeQuadTreeIterator<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds>,
        IUnsafeQuadTreeIterator<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds>
      {
        public float2 m_Position;
        public float2 m_Delta;
        public uint m_TypeMask;

        public bool Intersect(LocalEffectSystem.EffectBounds bounds)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Position) && (bounds.m_TypeMask & this.m_TypeMask) > 0U;
        }

        public void Iterate(
          LocalEffectSystem.EffectBounds bounds,
          LocalEffectSystem.EffectItem entity2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Position) || ((int) bounds.m_TypeMask & (int) this.m_TypeMask) == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          float2 x = MathUtils.Center(bounds.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num1 = (float) (((double) bounds.m_Bounds.max.x - (double) bounds.m_Bounds.min.x) * 0.5);
          // ISSUE: reference to a compiler-generated field
          float num2 = (float) (1.0 - (double) math.distancesq(x, this.m_Position) / ((double) num1 * (double) num1));
          if ((double) num2 <= 0.0)
            return;
          // ISSUE: reference to a compiler-generated field
          float2 float2 = bounds.m_Delta * num2;
          // ISSUE: reference to a compiler-generated field
          this.m_Delta.y *= 1f + float2.y;
          // ISSUE: reference to a compiler-generated field
          this.m_Delta += float2;
        }
      }
    }

    [BurstCompile]
    private struct UpdateLocalEffectsJob : IJob
    {
      [ReadOnly]
      public bool m_Loaded;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> m_SearchTree;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> m_DestroyedType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Signature> m_SignatureType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<LocalModifierData> m_LocalModifierData;

      public void Execute()
      {
        NativeList<LocalModifierData> tempModifierList = new NativeList<LocalModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType) || chunk.Has<Destroyed>(ref this.m_DestroyedType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            BufferAccessor<InstalledUpgrade> bufferAccessor = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Entity provider = nativeArray1[index2];
              PrefabRef prefabRef = nativeArray2[index2];
              // ISSUE: reference to a compiler-generated method
              this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
              if (bufferAccessor.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddToTempList(tempModifierList, bufferAccessor[index2]);
              }
              for (int index3 = 0; index3 < tempModifierList.Length; ++index3)
              {
                LocalModifierData localModifierData = tempModifierList[index3];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                this.m_SearchTree.TryRemove(new LocalEffectSystem.EffectItem(provider, localModifierData.m_Type));
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Loaded || chunk.Has<Created>(ref this.m_CreatedType))
            {
              // ISSUE: reference to a compiler-generated field
              int num = chunk.Has<Signature>(ref this.m_SignatureType) ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
              if (num == 0 && bufferAccessor1.Length != 0)
              {
                for (int index4 = 0; index4 < nativeArray3.Length; ++index4)
                {
                  Entity provider = nativeArray3[index4];
                  Transform transform = nativeArray4[index4];
                  PrefabRef prefabRef = nativeArray5[index4];
                  float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1[index4]);
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
                  if (bufferAccessor2.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToTempList(tempModifierList, bufferAccessor2[index4]);
                  }
                  for (int index5 = 0; index5 < tempModifierList.Length; ++index5)
                  {
                    LocalModifierData localModifier = tempModifierList[index5];
                    // ISSUE: variable of a compiler-generated type
                    LocalEffectSystem.EffectBounds effectBounds;
                    // ISSUE: reference to a compiler-generated method
                    if (LocalEffectSystem.GetEffectBounds(transform, efficiency, localModifier, out effectBounds))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.Add(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type), effectBounds);
                    }
                  }
                }
              }
              else
              {
                for (int index6 = 0; index6 < nativeArray3.Length; ++index6)
                {
                  Entity provider = nativeArray3[index6];
                  Transform transform = nativeArray4[index6];
                  PrefabRef prefabRef = nativeArray5[index6];
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
                  if (bufferAccessor2.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToTempList(tempModifierList, bufferAccessor2[index6]);
                  }
                  for (int index7 = 0; index7 < tempModifierList.Length; ++index7)
                  {
                    LocalModifierData localModifier = tempModifierList[index7];
                    // ISSUE: variable of a compiler-generated type
                    LocalEffectSystem.EffectBounds effectBounds;
                    // ISSUE: reference to a compiler-generated method
                    if (LocalEffectSystem.GetEffectBounds(transform, localModifier, out effectBounds))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.Add(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type), effectBounds);
                    }
                  }
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              int num = chunk.Has<Signature>(ref this.m_SignatureType) ? 1 : 0;
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray6 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<Efficiency> bufferAccessor3 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Transform> nativeArray7 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray8 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              // ISSUE: reference to a compiler-generated field
              BufferAccessor<InstalledUpgrade> bufferAccessor4 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
              if (num == 0 && bufferAccessor3.Length != 0)
              {
                for (int index8 = 0; index8 < nativeArray6.Length; ++index8)
                {
                  Entity provider = nativeArray6[index8];
                  Transform transform = nativeArray7[index8];
                  PrefabRef prefabRef = nativeArray8[index8];
                  float efficiency = BuildingUtils.GetEfficiency(bufferAccessor3[index8]);
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
                  if (bufferAccessor4.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToTempList(tempModifierList, bufferAccessor4[index8]);
                  }
                  for (int index9 = 0; index9 < tempModifierList.Length; ++index9)
                  {
                    LocalModifierData localModifier = tempModifierList[index9];
                    // ISSUE: variable of a compiler-generated type
                    LocalEffectSystem.EffectBounds effectBounds;
                    // ISSUE: reference to a compiler-generated method
                    if (LocalEffectSystem.GetEffectBounds(transform, efficiency, localModifier, out effectBounds))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.AddOrUpdate(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type), effectBounds);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.TryRemove(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type));
                    }
                  }
                }
              }
              else
              {
                for (int index10 = 0; index10 < nativeArray6.Length; ++index10)
                {
                  Entity provider = nativeArray6[index10];
                  Transform transform = nativeArray7[index10];
                  PrefabRef prefabRef = nativeArray8[index10];
                  // ISSUE: reference to a compiler-generated method
                  this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
                  if (bufferAccessor4.Length != 0)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.AddToTempList(tempModifierList, bufferAccessor4[index10]);
                  }
                  for (int index11 = 0; index11 < tempModifierList.Length; ++index11)
                  {
                    LocalModifierData localModifier = tempModifierList[index11];
                    // ISSUE: variable of a compiler-generated type
                    LocalEffectSystem.EffectBounds effectBounds;
                    // ISSUE: reference to a compiler-generated method
                    if (LocalEffectSystem.GetEffectBounds(transform, localModifier, out effectBounds))
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.AddOrUpdate(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type), effectBounds);
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: object of a compiler-generated type is created
                      this.m_SearchTree.TryRemove(new LocalEffectSystem.EffectItem(provider, localModifier.m_Type));
                    }
                  }
                }
              }
            }
          }
        }
        tempModifierList.Dispose();
      }

      private void InitializeTempList(NativeList<LocalModifierData> tempModifierList, Entity prefab)
      {
        DynamicBuffer<LocalModifierData> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LocalModifierData.TryGetBuffer(prefab, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          LocalEffectSystem.InitializeTempList(tempModifierList, bufferData);
        }
        else
          tempModifierList.Clear();
      }

      private void AddToTempList(
        NativeList<LocalModifierData> tempModifierList,
        DynamicBuffer<InstalledUpgrade> upgrades)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<LocalModifierData> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalModifierData.TryGetBuffer(this.m_PrefabRefData[upgrade.m_Upgrade].m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            LocalEffectSystem.AddToTempList(tempModifierList, bufferData, BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive));
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Destroyed> __Game_Common_Destroyed_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Signature> __Game_Buildings_Signature_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LocalModifierData> __Game_Prefabs_LocalModifierData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Signature_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Signature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalModifierData_RO_BufferLookup = state.GetBufferLookup<LocalModifierData>(true);
      }
    }
  }
}
