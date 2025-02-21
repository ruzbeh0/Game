// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UpgradeUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public static class UpgradeUtils
  {
    public static void CombineStats<T>(
      ref T result,
      BufferAccessor<InstalledUpgrade> accessor,
      int i,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<T> combineDatas)
      where T : unmanaged, IComponentData, ICombineData<T>
    {
      if (accessor.Length == 0)
        return;
      UpgradeUtils.CombineStats<T>(ref result, accessor[i], ref prefabs, ref combineDatas);
    }

    public static bool CombineStats<T>(
      ref T data,
      DynamicBuffer<InstalledUpgrade> upgrades,
      ref ComponentLookup<PrefabRef> prefabs,
      ref ComponentLookup<T> combineDatas)
      where T : unmanaged, IComponentData, ICombineData<T>
    {
      bool flag = false;
      for (int index = 0; index < upgrades.Length; ++index)
      {
        InstalledUpgrade upgrade = upgrades[index];
        if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive))
        {
          PrefabRef prefabRef = prefabs[upgrade.m_Upgrade];
          T componentData;
          if (combineDatas.TryGetComponent(prefabRef.m_Prefab, out componentData))
          {
            data.Combine(componentData);
            flag = true;
          }
        }
      }
      return flag;
    }

    public static bool CombineStats<T>(
      EntityManager entityManager,
      ref T data,
      DynamicBuffer<InstalledUpgrade> upgrades)
      where T : unmanaged, IComponentData, ICombineData<T>
    {
      bool flag = false;
      for (int index = 0; index < upgrades.Length; ++index)
      {
        InstalledUpgrade upgrade = upgrades[index];
        PrefabRef component1;
        T component2;
        if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && entityManager.TryGetComponent<PrefabRef>(upgrade.m_Upgrade, out component1) && entityManager.TryGetComponent<T>(component1.m_Prefab, out component2))
        {
          data.Combine(component2);
          flag = true;
        }
      }
      return flag;
    }

    public static void CombineStats<T>(
      NativeList<T> result,
      DynamicBuffer<InstalledUpgrade> upgrades,
      ref ComponentLookup<PrefabRef> prefabs,
      ref BufferLookup<T> combineDatas)
      where T : unmanaged, IBufferElementData, ICombineBuffer<T>
    {
      for (int index = 0; index < upgrades.Length; ++index)
      {
        InstalledUpgrade upgrade = upgrades[index];
        if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive))
        {
          PrefabRef prefabRef = prefabs[upgrade.m_Upgrade];
          UpgradeUtils.CombineStats<T>(result, prefabRef.m_Prefab, ref combineDatas);
        }
      }
    }

    public static void CombineStats<T>(
      NativeList<T> result,
      Entity prefab,
      ref BufferLookup<T> combineDatas)
      where T : unmanaged, IBufferElementData, ICombineBuffer<T>
    {
      DynamicBuffer<T> bufferData;
      if (!combineDatas.TryGetBuffer(prefab, out bufferData))
        return;
      UpgradeUtils.CombineStats<T>(result, bufferData);
    }

    public static void CombineStats<T>(NativeList<T> result, DynamicBuffer<T> combineData) where T : unmanaged, IBufferElementData, ICombineBuffer<T>
    {
      for (int index = 0; index < combineData.Length; ++index)
        combineData[index].Combine(result);
    }

    public static void CombineStats<T>(NativeList<T> result, T combineData) where T : unmanaged, IBufferElementData, ICombineBuffer<T>
    {
      combineData.Combine(result);
    }

    public static bool TryGetCombinedComponent<T>(
      EntityManager entityManager,
      Entity entity,
      Entity prefab,
      out T data)
      where T : unmanaged, IComponentData, ICombineData<T>
    {
      bool component = entityManager.TryGetComponent<T>(prefab, out data);
      return UpgradeUtils.TryCombineData<T>(entityManager, entity, ref data) | component;
    }

    public static bool TryCombineData<T>(EntityManager entityManager, Entity entity, ref T data) where T : unmanaged, IComponentData, ICombineData<T>
    {
      DynamicBuffer<InstalledUpgrade> buffer;
      return entityManager.TryGetBuffer<InstalledUpgrade>(entity, true, out buffer) && UpgradeUtils.CombineStats<T>(entityManager, ref data, buffer);
    }

    public static bool TryGetCombinedComponent<T>(
      Entity entity,
      out T data,
      ref ComponentLookup<PrefabRef> prefabRefLookup,
      ref ComponentLookup<T> combineDataLookup,
      ref BufferLookup<InstalledUpgrade> installedUpgradeLookup)
      where T : unmanaged, IComponentData, ICombineData<T>
    {
      data = default (T);
      PrefabRef componentData;
      bool combinedComponent = prefabRefLookup.TryGetComponent(entity, out componentData) && combineDataLookup.TryGetComponent(componentData.m_Prefab, out data);
      DynamicBuffer<InstalledUpgrade> bufferData;
      if (installedUpgradeLookup.TryGetBuffer(entity, out bufferData))
        combinedComponent |= UpgradeUtils.CombineStats<T>(ref data, bufferData, ref prefabRefLookup, ref combineDataLookup);
      return combinedComponent;
    }
  }
}
