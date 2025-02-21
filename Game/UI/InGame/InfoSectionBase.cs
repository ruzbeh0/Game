// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.InfoSectionBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public abstract class InfoSectionBase : UISystemBase, ISectionSource, IJsonWritable
  {
    protected bool m_Dirty;
    protected NameSystem m_NameSystem;
    protected PrefabSystem m_PrefabSystem;
    protected EndFrameBarrier m_EndFrameBarrier;
    protected SelectedInfoUISystem m_InfoUISystem;

    public override GameMode gameMode => GameMode.Game;

    public bool visible { get; protected set; }

    protected virtual bool displayForDestroyedObjects => false;

    protected virtual bool displayForOutsideConnections => false;

    protected virtual bool displayForUnderConstruction => false;

    protected virtual bool displayForUpgrades => false;

    protected abstract string group { get; }

    protected List<string> tooltipKeys { get; set; }

    protected List<string> tooltipTags { get; set; }

    protected virtual Entity selectedEntity => this.m_InfoUISystem.selectedEntity;

    protected virtual Entity selectedPrefab => this.m_InfoUISystem.selectedPrefab;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.tooltipKeys = new List<string>();
      this.tooltipTags = new List<string>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
    }

    protected abstract void Reset();

    protected abstract void OnProcess();

    public abstract void OnWriteProperties(IJsonWriter writer);

    public void RequestUpdate() => this.m_Dirty = true;

    private bool Visible()
    {
      if (!this.visible || this.Destroyed && !this.displayForDestroyedObjects || this.OutsideConnection && !this.displayForOutsideConnections || this.UnderConstruction && !this.displayForUnderConstruction)
        return false;
      return !this.Upgrade || this.displayForUpgrades;
    }

    protected virtual void OnPreUpdate()
    {
    }

    public void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.OnPreUpdate();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Dirty)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = false;
      this.tooltipKeys.Clear();
      this.tooltipTags.Clear();
      // ISSUE: reference to a compiler-generated method
      this.Reset();
      this.Update();
      // ISSUE: reference to a compiler-generated method
      if (!this.Visible())
        return;
      // ISSUE: reference to a compiler-generated method
      this.OnProcess();
    }

    public void Write(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.Visible())
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("group");
        writer.Write(this.group);
        writer.PropertyName("tooltipKeys");
        writer.ArrayBegin(this.tooltipKeys.Count);
        for (int index = 0; index < this.tooltipKeys.Count; ++index)
          writer.Write(this.tooltipKeys[index]);
        writer.ArrayEnd();
        writer.PropertyName("tooltipTags");
        writer.ArrayBegin(this.tooltipTags.Count);
        for (int index = 0; index < this.tooltipTags.Count; ++index)
          writer.Write(this.tooltipTags[index]);
        writer.ArrayEnd();
        // ISSUE: reference to a compiler-generated method
        this.OnWriteProperties(writer);
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    protected bool Destroyed => this.EntityManager.HasComponent<Game.Common.Destroyed>(this.selectedEntity);

    protected bool OutsideConnection
    {
      get => this.EntityManager.HasComponent<Game.Objects.OutsideConnection>(this.selectedEntity);
    }

    protected bool UnderConstruction
    {
      get
      {
        Game.Objects.UnderConstruction component;
        return this.EntityManager.TryGetComponent<Game.Objects.UnderConstruction>(this.selectedEntity, out component) && component.m_NewPrefab == Entity.Null;
      }
    }

    protected bool Upgrade
    {
      get => this.EntityManager.HasComponent<ServiceUpgradeData>(this.selectedPrefab);
    }

    protected bool TryGetComponentWithUpgrades<T>(Entity entity, Entity prefab, out T data) where T : unmanaged, IComponentData, ICombineData<T>
    {
      return UpgradeUtils.TryGetCombinedComponent<T>(this.EntityManager, entity, prefab, out data);
    }

    [Preserve]
    protected InfoSectionBase()
    {
    }
  }
}
