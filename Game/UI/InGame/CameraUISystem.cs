// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CameraUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Rendering;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class CameraUISystem : UISystemBase
  {
    private const string kGroup = "camera";
    private CameraUpdateSystem m_CameraUpdateSystem;
    private GetterValueBinding<Entity> m_FocusedEntityBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.AddBinding((IBinding) (this.m_FocusedEntityBinding = new GetterValueBinding<Entity>("camera", "focusedEntity", new Func<Entity>(this.GetFocusedEntity))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("camera", "focusEntity", new Action<Entity>(this.FocusEntity)));
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
    }

    [Preserve]
    protected override void OnUpdate() => this.m_FocusedEntityBinding.Update();

    private Entity GetFocusedEntity()
    {
      return !((UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null) ? Entity.Null : this.m_CameraUpdateSystem.orbitCameraController.followedEntity;
    }

    private void FocusEntity(Entity entity)
    {
      if (entity != Entity.Null && (UnityEngine.Object) this.m_CameraUpdateSystem.orbitCameraController != (UnityEngine.Object) null && entity != this.m_CameraUpdateSystem.orbitCameraController.followedEntity)
      {
        this.m_CameraUpdateSystem.orbitCameraController.followedEntity = entity;
        this.m_CameraUpdateSystem.orbitCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController;
      }
      if (!(entity == Entity.Null) || this.m_CameraUpdateSystem.activeCameraController != this.m_CameraUpdateSystem.orbitCameraController)
        return;
      this.m_CameraUpdateSystem.gamePlayController.TryMatchPosition((IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController);
      this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.gamePlayController;
    }

    [Preserve]
    public CameraUISystem()
    {
    }
  }
}
