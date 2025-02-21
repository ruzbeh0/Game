// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ResourceSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Objects;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ResourceSection : InfoSectionBase
  {
    protected override string group => nameof (ResourceSection);

    private float resourceAmount { get; set; }

    private ResourceSection.ResourceKey resourceKey { get; set; }

    protected override bool displayForDestroyedObjects => true;

    protected override void Reset() => this.resourceAmount = 0.0f;

    [Preserve]
    protected override void OnUpdate()
    {
      TreeData component;
      this.visible = this.EntityManager.HasComponent<Tree>(this.selectedEntity) && this.EntityManager.TryGetComponent<TreeData>(this.selectedPrefab, out component) && (double) component.m_WoodAmount > 0.0;
    }

    protected override void OnProcess()
    {
      Tree componentData1 = this.EntityManager.GetComponentData<Tree>(this.selectedEntity);
      Plant componentData2 = this.EntityManager.GetComponentData<Plant>(this.selectedEntity);
      TreeData componentData3 = this.EntityManager.GetComponentData<TreeData>(this.selectedPrefab);
      Damaged component;
      this.EntityManager.TryGetComponent<Damaged>(this.selectedEntity, out component);
      this.resourceAmount = math.round(ObjectUtils.CalculateWoodAmount(componentData1, componentData2, component, componentData3));
      this.resourceKey = ResourceSection.ResourceKey.Wood;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("resourceAmount");
      writer.Write(this.resourceAmount);
      writer.PropertyName("resourceKey");
      writer.Write(Enum.GetName(typeof (ResourceSection.ResourceKey), (object) this.resourceKey));
    }

    [Preserve]
    public ResourceSection()
    {
    }

    private enum ResourceKey
    {
      Wood,
    }
  }
}
