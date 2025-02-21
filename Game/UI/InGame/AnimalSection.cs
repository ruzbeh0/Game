// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AnimalSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class AnimalSection : InfoSectionBase
  {
    protected override string group => nameof (AnimalSection);

    private AnimalSection.TypeKey typeKey { get; set; }

    private Entity ownerEntity { get; set; }

    private Entity destinationEntity { get; set; }

    private string GetTypeKeyString(AnimalSection.TypeKey typeKey)
    {
      if (typeKey == AnimalSection.TypeKey.Pet)
        return "Pet";
      return typeKey == AnimalSection.TypeKey.Livestock ? "Livestock" : "Wildlife";
    }

    protected override void Reset()
    {
      this.ownerEntity = Entity.Null;
      this.destinationEntity = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<HouseholdPet>(this.selectedEntity) || this.EntityManager.HasComponent<Wildlife>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated method
      this.typeKey = this.GetTypeKey();
      HouseholdPet component1;
      this.ownerEntity = this.EntityManager.TryGetComponent<HouseholdPet>(this.selectedEntity, out component1) ? component1.m_Household : Entity.Null;
      CurrentTransport component2;
      Target component3;
      this.destinationEntity = !this.EntityManager.TryGetComponent<CurrentTransport>(this.selectedEntity, out component2) || !this.EntityManager.TryGetComponent<Target>(component2.m_CurrentTransport, out component3) ? Entity.Null : component3.m_Target;
      // ISSUE: reference to a compiler-generated method
      this.tooltipKeys.Add(this.GetTypeKeyString(this.typeKey));
    }

    private AnimalSection.TypeKey GetTypeKey()
    {
      if (this.EntityManager.HasComponent<HouseholdPet>(this.selectedEntity))
        return AnimalSection.TypeKey.Pet;
      return this.EntityManager.HasComponent<Wildlife>(this.selectedEntity) ? AnimalSection.TypeKey.Wildlife : AnimalSection.TypeKey.Livestock;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("typeKey");
      writer.Write(Enum.GetName(typeof (AnimalSection.TypeKey), (object) this.typeKey));
      writer.PropertyName("owner");
      if (this.ownerEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.ownerEntity);
      }
      writer.PropertyName("ownerEntity");
      if (this.ownerEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.ownerEntity);
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
    public AnimalSection()
    {
    }

    private enum TypeKey
    {
      Pet,
      Livestock,
      Wildlife,
    }
  }
}
