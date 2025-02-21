// Decompiled with JetBrains decompiler
// Type: Game.SafeCommandBufferSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game
{
  public class SafeCommandBufferSystem : EntityCommandBufferSystem
  {
    private bool m_IsAllowed = true;

    public void AllowUsage() => this.m_IsAllowed = true;

    public new EntityCommandBuffer CreateCommandBuffer()
    {
      if (this.m_IsAllowed)
        return base.CreateCommandBuffer();
      throw new Exception("Trying to create EntityCommandBuffer when it's not allowed!");
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_IsAllowed = false;
      base.OnUpdate();
    }

    [Preserve]
    public SafeCommandBufferSystem()
    {
    }
  }
}
