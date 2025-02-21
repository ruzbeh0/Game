// Decompiled with JetBrains decompiler
// Type: Game.Notifications.IconCommandBuffer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Notifications
{
  public struct IconCommandBuffer
  {
    private NativeQueue<IconCommandBuffer.Command>.ParallelWriter m_Commands;
    private int m_BufferIndex;

    public IconCommandBuffer(
      NativeQueue<IconCommandBuffer.Command>.ParallelWriter commands,
      int bufferIndex)
    {
      this.m_Commands = commands;
      this.m_BufferIndex = bufferIndex;
    }

    public void Add(
      Entity owner,
      Entity prefab,
      IconPriority priority = IconPriority.Info,
      IconClusterLayer clusterLayer = IconClusterLayer.Default,
      IconFlags flags = (IconFlags) 0,
      Entity target = default (Entity),
      bool isTemp = false,
      bool isHidden = false,
      bool disallowCluster = false,
      float delay = 0.0f)
    {
      this.m_Commands.Enqueue(new IconCommandBuffer.Command()
      {
        m_Owner = owner,
        m_Prefab = prefab,
        m_Target = target,
        m_CommandFlags = (IconCommandBuffer.CommandFlags) (1 | (isTemp ? 8 : 0) | (isHidden ? 16 : 0) | (disallowCluster ? 32 : 0)),
        m_Priority = priority,
        m_ClusterLayer = clusterLayer,
        m_Flags = flags & ~IconFlags.CustomLocation,
        m_BufferIndex = this.m_BufferIndex,
        m_Delay = delay
      });
    }

    public void Add(
      Entity owner,
      Entity prefab,
      float3 location,
      IconPriority priority = IconPriority.Info,
      IconClusterLayer clusterLayer = IconClusterLayer.Default,
      IconFlags flags = IconFlags.IgnoreTarget,
      Entity target = default (Entity),
      bool isTemp = false,
      bool isHidden = false,
      bool disallowCluster = false,
      float delay = 0.0f)
    {
      this.m_Commands.Enqueue(new IconCommandBuffer.Command()
      {
        m_Owner = owner,
        m_Prefab = prefab,
        m_Target = target,
        m_Location = location,
        m_CommandFlags = (IconCommandBuffer.CommandFlags) (1 | (isTemp ? 8 : 0) | (isHidden ? 16 : 0) | (disallowCluster ? 32 : 0)),
        m_Priority = priority,
        m_ClusterLayer = clusterLayer,
        m_Flags = flags | IconFlags.CustomLocation,
        m_BufferIndex = this.m_BufferIndex,
        m_Delay = delay
      });
    }

    public void Remove(Entity owner, Entity prefab, Entity target = default (Entity), IconFlags flags = (IconFlags) 0)
    {
      this.m_Commands.Enqueue(new IconCommandBuffer.Command()
      {
        m_Owner = owner,
        m_Prefab = prefab,
        m_Target = target,
        m_CommandFlags = IconCommandBuffer.CommandFlags.Remove,
        m_Flags = flags,
        m_BufferIndex = this.m_BufferIndex
      });
    }

    public void Remove(Entity owner, IconPriority priority)
    {
      this.m_Commands.Enqueue(new IconCommandBuffer.Command()
      {
        m_Owner = owner,
        m_CommandFlags = IconCommandBuffer.CommandFlags.Remove | IconCommandBuffer.CommandFlags.All,
        m_Priority = priority,
        m_BufferIndex = this.m_BufferIndex
      });
    }

    public void Update(Entity owner)
    {
      this.m_Commands.Enqueue(new IconCommandBuffer.Command()
      {
        m_Owner = owner,
        m_CommandFlags = IconCommandBuffer.CommandFlags.Update,
        m_BufferIndex = this.m_BufferIndex
      });
    }

    public enum CommandFlags : byte
    {
      Add = 1,
      Remove = 2,
      Update = 4,
      Temp = 8,
      Hidden = 16, // 0x10
      DisallowCluster = 32, // 0x20
      All = 64, // 0x40
    }

    public struct Command : IComparable<IconCommandBuffer.Command>
    {
      public Entity m_Owner;
      public Entity m_Prefab;
      public Entity m_Target;
      public float3 m_Location;
      public IconCommandBuffer.CommandFlags m_CommandFlags;
      public IconPriority m_Priority;
      public IconClusterLayer m_ClusterLayer;
      public IconFlags m_Flags;
      public int m_BufferIndex;
      public float m_Delay;

      public int CompareTo(IconCommandBuffer.Command other)
      {
        int2 int2 = math.select((int2) 0, (int2) 1, new bool2(this.m_Prefab != Entity.Null, other.m_Prefab != Entity.Null));
        return math.select(math.select(this.m_BufferIndex - other.m_BufferIndex, int2.x - int2.y, int2.x != int2.y), this.m_Owner.Index - other.m_Owner.Index, this.m_Owner.Index != other.m_Owner.Index);
      }
    }
  }
}
