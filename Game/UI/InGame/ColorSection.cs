// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ColorSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Routes;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ColorSection : InfoSectionBase
  {
    private EntityArchetype m_ColorUpdateArchetype;

    protected override string group => nameof (ColorSection);

    private Color32 color { get; set; }

    protected override void Reset() => this.color = new Color32();

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.AddBinding((IBinding) new TriggerBinding<UnityEngine.Color>(this.group, "setColor", (Action<UnityEngine.Color>) (uiColor =>
      {
        if (!this.EntityManager.HasComponent<Route>(this.selectedEntity) || !this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) || !this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity) || !this.EntityManager.HasComponent<Game.Routes.Color>(this.selectedEntity))
          return;
        // ISSUE: reference to a compiler-generated field
        EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
        commandBuffer.SetComponent<Game.Routes.Color>(this.selectedEntity, new Game.Routes.Color((Color32) uiColor));
        DynamicBuffer<RouteVehicle> buffer;
        if (this.EntityManager.TryGetBuffer<RouteVehicle>(this.selectedEntity, true, out buffer))
        {
          for (int index = 0; index < buffer.Length; ++index)
            commandBuffer.AddComponent<Game.Routes.Color>(buffer[index].m_Vehicle, new Game.Routes.Color((Color32) uiColor));
        }
        // ISSUE: reference to a compiler-generated field
        Entity entity = commandBuffer.CreateEntity(this.m_ColorUpdateArchetype);
        commandBuffer.SetComponent<ColorUpdated>(entity, new ColorUpdated(this.selectedEntity));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.RequestUpdate();
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_ColorUpdateArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<ColorUpdated>());
    }

    private void OnSetColor(UnityEngine.Color uiColor)
    {
      if (!this.EntityManager.HasComponent<Route>(this.selectedEntity) || !this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) || !this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity) || !this.EntityManager.HasComponent<Game.Routes.Color>(this.selectedEntity))
        return;
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer();
      commandBuffer.SetComponent<Game.Routes.Color>(this.selectedEntity, new Game.Routes.Color((Color32) uiColor));
      DynamicBuffer<RouteVehicle> buffer;
      if (this.EntityManager.TryGetBuffer<RouteVehicle>(this.selectedEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
          commandBuffer.AddComponent<Game.Routes.Color>(buffer[index].m_Vehicle, new Game.Routes.Color((Color32) uiColor));
      }
      // ISSUE: reference to a compiler-generated field
      Entity entity = commandBuffer.CreateEntity(this.m_ColorUpdateArchetype);
      commandBuffer.SetComponent<ColorUpdated>(entity, new ColorUpdated(this.selectedEntity));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.RequestUpdate();
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Routes.Color>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      this.color = this.EntityManager.GetComponentData<Game.Routes.Color>(this.selectedEntity).m_Color;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("color");
      writer.Write(this.color);
    }

    [Preserve]
    public ColorSection()
    {
    }
  }
}
