// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TriggerPrefabData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Triggers;
using System;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct TriggerPrefabData : IDisposable
  {
    private NativeParallelMultiHashMap<TriggerPrefabData.PrefabKey, TriggerPrefabData.PrefabValue> m_PrefabMap;

    public TriggerPrefabData(Allocator allocator)
    {
      this.m_PrefabMap = new NativeParallelMultiHashMap<TriggerPrefabData.PrefabKey, TriggerPrefabData.PrefabValue>(100, (AllocatorManager.AllocatorHandle) allocator);
    }

    public void Dispose() => this.m_PrefabMap.Dispose();

    public void AddPrefab(Entity prefab, TriggerData triggerData)
    {
      this.m_PrefabMap.Add(new TriggerPrefabData.PrefabKey()
      {
        m_TriggerType = triggerData.m_TriggerType,
        m_TriggerEntity = triggerData.m_TriggerPrefab
      }, new TriggerPrefabData.PrefabValue()
      {
        m_TargetTypes = triggerData.m_TargetTypes,
        m_Prefab = prefab
      });
    }

    public void RemovePrefab(Entity prefab, TriggerData triggerData)
    {
      TriggerPrefabData.PrefabValue prefabValue;
      NativeParallelMultiHashMapIterator<TriggerPrefabData.PrefabKey> it;
      if (!this.m_PrefabMap.TryGetFirstValue(new TriggerPrefabData.PrefabKey()
      {
        m_TriggerType = triggerData.m_TriggerType,
        m_TriggerEntity = triggerData.m_TriggerPrefab
      }, out prefabValue, out it))
        return;
      while (prefabValue.m_TargetTypes != triggerData.m_TargetTypes || !(prefabValue.m_Prefab == prefab))
      {
        if (!this.m_PrefabMap.TryGetNextValue(out prefabValue, ref it))
          return;
      }
      this.m_PrefabMap.Remove(it);
    }

    public bool HasAnyPrefabs(TriggerType triggerType, Entity triggerPrefab)
    {
      return this.m_PrefabMap.TryGetFirstValue(new TriggerPrefabData.PrefabKey()
      {
        m_TriggerType = triggerType,
        m_TriggerEntity = triggerPrefab
      }, out TriggerPrefabData.PrefabValue _, out NativeParallelMultiHashMapIterator<TriggerPrefabData.PrefabKey> _);
    }

    public bool TryGetFirstPrefab(
      TriggerType triggerType,
      TargetType targetType,
      Entity triggerPrefab,
      out Entity prefab,
      out TriggerPrefabData.Iterator iterator)
    {
      TriggerPrefabData.PrefabValue prefabValue;
      if (this.m_PrefabMap.TryGetFirstValue(new TriggerPrefabData.PrefabKey()
      {
        m_TriggerType = triggerType,
        m_TriggerEntity = triggerPrefab
      }, out prefabValue, out iterator.m_Iterator))
      {
        while (targetType != TargetType.Nothing && (prefabValue.m_TargetTypes & targetType) == TargetType.Nothing)
        {
          if (!this.m_PrefabMap.TryGetNextValue(out prefabValue, ref iterator.m_Iterator))
            goto label_4;
        }
        prefab = prefabValue.m_Prefab;
        return true;
      }
label_4:
      prefab = Entity.Null;
      return false;
    }

    public bool TryGetNextPrefab(
      TriggerType triggerType,
      TargetType targetType,
      Entity triggerPrefab,
      out Entity prefab,
      ref TriggerPrefabData.Iterator iterator)
    {
      TriggerPrefabData.PrefabKey prefabKey = new TriggerPrefabData.PrefabKey()
      {
        m_TriggerType = triggerType,
        m_TriggerEntity = triggerPrefab
      };
      TriggerPrefabData.PrefabValue prefabValue;
      while (this.m_PrefabMap.TryGetNextValue(out prefabValue, ref iterator.m_Iterator))
      {
        if (targetType == TargetType.Nothing || (prefabValue.m_TargetTypes & targetType) != TargetType.Nothing)
        {
          prefab = prefabValue.m_Prefab;
          return true;
        }
      }
      prefab = Entity.Null;
      return false;
    }

    public struct PrefabKey : IEquatable<TriggerPrefabData.PrefabKey>
    {
      public TriggerType m_TriggerType;
      public Entity m_TriggerEntity;

      public bool Equals(TriggerPrefabData.PrefabKey other)
      {
        return this.m_TriggerType == other.m_TriggerType && this.m_TriggerEntity == other.m_TriggerEntity;
      }

      public override int GetHashCode()
      {
        return (int) this.m_TriggerType * 31 + this.m_TriggerEntity.GetHashCode();
      }
    }

    public struct PrefabValue
    {
      public TargetType m_TargetTypes;
      public Entity m_Prefab;
    }

    public struct Iterator
    {
      public NativeParallelMultiHashMapIterator<TriggerPrefabData.PrefabKey> m_Iterator;
    }
  }
}
