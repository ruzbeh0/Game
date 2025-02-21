// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.IUniqueAssetTrackingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public interface IUniqueAssetTrackingSystem
  {
    NativeParallelHashSet<Entity> placedUniqueAssets { get; }

    Action<Entity, bool> EventUniqueAssetStatusChanged { get; set; }
  }
}
