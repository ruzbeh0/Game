// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PdxLoginRequirement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Content/", new Type[] {typeof (ContentPrefab)})]
  public class PdxLoginRequirement : ContentRequirementBase
  {
    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override bool CheckRequirement()
    {
      PdxSdkPlatform psi = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      return psi != null && psi.hasEverLoggedIn;
    }
  }
}
