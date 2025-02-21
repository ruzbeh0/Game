// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DeathcareVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DeathcareVehicleSection : VehicleSection
  {
    protected override string group => nameof (DeathcareVehicleSection);

    private Entity deadEntity { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.deadEntity = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Hearse>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Hearse componentData = this.EntityManager.GetComponentData<Hearse>(this.selectedEntity);
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData, this.EntityManager);
      this.deadEntity = this.stateKey == VehicleStateLocaleKey.Conveying ? componentData.m_TargetCorpse : Entity.Null;
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("dead");
      if (this.deadEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.deadEntity);
      }
      writer.PropertyName("deadEntity");
      if (this.deadEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.deadEntity);
    }

    [Preserve]
    public DeathcareVehicleSection()
    {
    }
  }
}
