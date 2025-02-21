// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ProgressionUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Prefabs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public static class ProgressionUtils
  {
    public static bool CollectSubRequirements(
      EntityManager entityManager,
      Entity prefab,
      NativeParallelHashMap<Entity, UnlockFlags> requiredPrefabs,
      UnlockFlags flags = UnlockFlags.RequireAll | UnlockFlags.RequireAny)
    {
      DynamicBuffer<UnlockRequirement> buffer;
      if (prefab == Entity.Null || requiredPrefabs.ContainsKey(prefab) && (requiredPrefabs[prefab] & flags) != (UnlockFlags) 0 || !entityManager.TryGetBuffer<UnlockRequirement>(prefab, true, out buffer))
        return false;
      for (int index = 0; index < buffer.Length; ++index)
      {
        UnlockRequirement unlockRequirement = buffer[index];
        if (unlockRequirement.m_Prefab == prefab && (unlockRequirement.m_Flags & UnlockFlags.RequireAll) != (UnlockFlags) 0)
          return true;
      }
      for (int index = 0; index < buffer.Length; ++index)
      {
        UnlockRequirement unlockRequirement = buffer[index];
        if (!(unlockRequirement.m_Prefab == prefab))
        {
          requiredPrefabs.Add(prefab, UnlockFlags.RequireAll | UnlockFlags.RequireAny);
          if (ProgressionUtils.CollectSubRequirements(entityManager, unlockRequirement.m_Prefab, requiredPrefabs, unlockRequirement.m_Flags))
          {
            if (requiredPrefabs.ContainsKey(unlockRequirement.m_Prefab))
              requiredPrefabs[unlockRequirement.m_Prefab] |= unlockRequirement.m_Flags;
            else
              requiredPrefabs.Add(unlockRequirement.m_Prefab, unlockRequirement.m_Flags);
          }
          requiredPrefabs.Remove(prefab);
        }
      }
      return false;
    }

    public static int GetRequiredMilestone(EntityManager entityManager, Entity entity)
    {
      int requiredMilestone = 0;
      if (entityManager.HasComponent<UnlockRequirement>(entity))
      {
        NativeParallelHashMap<Entity, UnlockFlags> requiredPrefabs = new NativeParallelHashMap<Entity, UnlockFlags>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        ProgressionUtils.CollectSubRequirements(entityManager, entity, requiredPrefabs);
        foreach (KeyValue<Entity, UnlockFlags> keyValue in requiredPrefabs)
        {
          MilestoneData component;
          if ((keyValue.Value & UnlockFlags.RequireAll) != (UnlockFlags) 0 && entityManager.TryGetComponent<MilestoneData>(keyValue.Key, out component) && component.m_Index > requiredMilestone)
            requiredMilestone = component.m_Index;
        }
        requiredPrefabs.Dispose();
      }
      return requiredMilestone;
    }
  }
}
