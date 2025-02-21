// Decompiled with JetBrains decompiler
// Type: Game.Serialization.GarbageProducerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Notifications;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class GarbageProducerSystem : GameSystemBase, IPostDeserialize
  {
    private EntityQuery m_Query;
    private GarbageProducerSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_31347557_0;

    [Preserve]
    protected override void OnUpdate()
    {
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (!(context.version < Version.garbageProducerFlags))
        return;
      EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<GarbageProducer>());
      // ISSUE: reference to a compiler-generated field
      GarbageParameterData singleton = this.__query_31347557_0.GetSingleton<GarbageParameterData>();
      NativeArray<Entity> entityArray = entityQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < entityArray.Length; ++index1)
      {
        DynamicBuffer<IconElement> buffer;
        if (this.EntityManager.TryGetBuffer<IconElement>(entityArray[index1], true, out buffer))
        {
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            PrefabRef component1;
            if (this.EntityManager.TryGetComponent<PrefabRef>(buffer[index2].m_Icon, out component1) && component1.m_Prefab == singleton.m_GarbageNotificationPrefab)
            {
              GarbageProducer component2;
              if (this.EntityManager.TryGetComponent<GarbageProducer>(entityArray[index1], out component2) && (component2.m_Flags & GarbageProducerFlags.GarbagePilingUpWarning) == GarbageProducerFlags.None)
              {
                component2.m_Flags |= GarbageProducerFlags.GarbagePilingUpWarning;
                this.EntityManager.SetComponentData<GarbageProducer>(entityArray[index1], component2);
                break;
              }
              break;
            }
          }
        }
      }
      entityArray.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_31347557_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<GarbageParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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

    [Preserve]
    public GarbageProducerSystem()
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct TypeHandle
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
      }
    }
  }
}
