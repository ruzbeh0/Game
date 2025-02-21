// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.DlcRequirement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Prefabs/Content/", new Type[] {typeof (ContentPrefab), typeof (UIWhatsNewPanelPrefab)})]
  public class DlcRequirement : ContentRequirementBase
  {
    public DlcId m_Dlc;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override bool CheckRequirement() => PlatformManager.instance.IsDlcOwned(this.m_Dlc);
  }
}
