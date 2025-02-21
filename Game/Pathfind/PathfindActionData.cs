// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathfindActionData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

#nullable disable
namespace Game.Pathfind
{
  public struct PathfindActionData : IDisposable
  {
    public UnsafeList<PathTarget> m_StartTargets;
    public UnsafeList<PathTarget> m_EndTargets;
    public UnsafeList<PathfindResult> m_Result;
    public UnsafeList<PathfindPath> m_Path;
    public PathfindParameters m_Parameters;
    public SetupTargetType m_OriginType;
    public SetupTargetType m_DestinationType;
    public PathfindActionState m_State;

    public PathfindActionData(
      int startCount,
      int endCount,
      Allocator allocator,
      PathfindParameters parameters,
      SetupTargetType originType,
      SetupTargetType destinationType)
    {
      if (startCount != 0)
      {
        this.m_StartTargets = new UnsafeList<PathTarget>(startCount, (AllocatorManager.AllocatorHandle) allocator);
        this.m_StartTargets.Length = startCount;
      }
      else
        this.m_StartTargets = new UnsafeList<PathTarget>();
      if (endCount != 0)
      {
        this.m_EndTargets = new UnsafeList<PathTarget>(endCount, (AllocatorManager.AllocatorHandle) allocator);
        this.m_EndTargets.Length = endCount;
      }
      else
        this.m_EndTargets = new UnsafeList<PathTarget>();
      this.m_Result = new UnsafeList<PathfindResult>(1, (AllocatorManager.AllocatorHandle) allocator);
      this.m_Path = (parameters.m_PathfindFlags & PathfindFlags.IgnorePath) != (PathfindFlags) 0 ? new UnsafeList<PathfindPath>() : new UnsafeList<PathfindPath>(100, (AllocatorManager.AllocatorHandle) allocator);
      this.m_Parameters = parameters;
      this.m_OriginType = originType;
      this.m_DestinationType = destinationType;
      this.m_State = PathfindActionState.Pending;
    }

    public void Dispose()
    {
      if (this.m_StartTargets.IsCreated)
        this.m_StartTargets.Dispose();
      if (this.m_EndTargets.IsCreated)
        this.m_EndTargets.Dispose();
      this.m_Result.Dispose();
      if (!this.m_Path.IsCreated)
        return;
      this.m_Path.Dispose();
    }
  }
}
