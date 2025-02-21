// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UpgradePropertiesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class UpgradePropertiesSection : InfoSectionBase
  {
    private static readonly string kMainBuildingName = "mainBuildingName";

    protected override string group => nameof (UpgradePropertiesSection);

    protected override bool displayForUpgrades => true;

    private Entity mainBuilding { get; set; }

    private Entity upgrade { get; set; }

    private UpgradePropertiesSection.UpgradeType type { get; set; }

    protected override void Reset()
    {
      this.mainBuilding = Entity.Null;
      this.upgrade = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<ServiceUpgradeData>(this.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      this.upgrade = this.selectedPrefab;
      Owner component;
      if (this.EntityManager.TryGetComponent<Owner>(this.selectedEntity, out component))
        this.mainBuilding = component.m_Owner;
      this.type = this.EntityManager.HasComponent<BuildingExtensionData>(this.selectedPrefab) ? UpgradePropertiesSection.UpgradeType.Extension : UpgradePropertiesSection.UpgradeType.SubBuilding;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("mainBuilding");
      writer.Write(this.mainBuilding);
      // ISSUE: reference to a compiler-generated field
      writer.PropertyName(UpgradePropertiesSection.kMainBuildingName);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindName(writer, this.mainBuilding);
      writer.PropertyName("upgrade");
      writer.Write(this.upgrade);
      writer.PropertyName("type");
      IJsonWriter jsonWriter = writer;
      // ISSUE: variable of a compiler-generated type
      UpgradePropertiesSection.UpgradeType type = this.type;
      string str = type.ToString();
      jsonWriter.Write(str);
    }

    [Preserve]
    public UpgradePropertiesSection()
    {
    }

    private enum UpgradeType
    {
      SubBuilding,
      Extension,
    }
  }
}
