// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ManagedBatchSystemDebugger
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class ManagedBatchSystemDebugger : MonoBehaviour
  {
    public const string kSectionName = "======System======";

    public ManagedBatchSystem managedBatchSystem { get; set; }

    public IReadOnlyDictionary<ManagedBatchSystem.MaterialKey, Material> materials
    {
      get => this.managedBatchSystem?.materials;
    }
  }
}
