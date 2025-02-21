// Decompiled with JetBrains decompiler
// Type: Game.Tools.SelectedUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Creatures;
using Game.Notifications;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class SelectedUpdateSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.selected == Entity.Null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (!this.EntityManager.Exists(this.m_ToolSystem.selected))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selected = Entity.Null;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.EntityManager.HasComponent<Deleted>(this.m_ToolSystem.selected))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_ToolSystem.selected;
        Owner component1;
        if (this.EntityManager.HasComponent<Icon>(entity) && this.EntityManager.TryGetComponent<Owner>(entity, out component1) && this.EntityManager.Exists(component1.m_Owner))
        {
          if (this.EntityManager.HasComponent<Deleted>(component1.m_Owner))
          {
            entity = component1.m_Owner;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = component1.m_Owner;
            return;
          }
        }
        Resident component2;
        if (this.EntityManager.TryGetComponent<Resident>(entity, out component2))
        {
          if (this.EntityManager.Exists(component2.m_Citizen))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = component2.m_Citizen;
          }
        }
        else
        {
          Pet component3;
          if (this.EntityManager.TryGetComponent<Pet>(entity, out component3) && this.EntityManager.Exists(component3.m_HouseholdPet))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.selected = component3.m_HouseholdPet;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.EntityManager.HasComponent<Deleted>(this.m_ToolSystem.selected))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.selected = Entity.Null;
      }
    }

    [Preserve]
    public SelectedUpdateSystem()
    {
    }
  }
}
