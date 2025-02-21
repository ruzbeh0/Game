// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DummyHumanSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Creatures;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class DummyHumanSection : InfoSectionBase
  {
    protected override string group => nameof (DummyHumanSection);

    private Entity originEntity { get; set; }

    private Entity destinationEntity { get; set; }

    protected override void Reset()
    {
      this.originEntity = Entity.Null;
      this.destinationEntity = Entity.Null;
    }

    private bool Visible()
    {
      Resident component;
      return this.EntityManager.TryGetComponent<Resident>(this.selectedEntity, out component) && component.m_Citizen == Entity.Null;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      CurrentVehicle component;
      if (!this.EntityManager.TryGetComponent<CurrentVehicle>(this.selectedEntity, out component))
        return;
      this.originEntity = this.EntityManager.GetComponentData<Owner>(component.m_Vehicle).m_Owner;
      this.destinationEntity = VehicleUIUtils.GetDestination(this.EntityManager, component.m_Vehicle);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("origin");
      if (this.originEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.originEntity);
      }
      writer.PropertyName("originEntity");
      if (this.originEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.originEntity);
      writer.PropertyName("destination");
      if (this.destinationEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.destinationEntity);
      }
      writer.PropertyName("destinationEntity");
      if (this.destinationEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.destinationEntity);
    }

    [Preserve]
    public DummyHumanSection()
    {
    }
  }
}
