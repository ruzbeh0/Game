// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorBottomBarUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Simulation;
using System;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorBottomBarUISystem : UISystemBase
  {
    private static readonly string kGroup = "editorBottomBar";
    private ClimateSystem m_ClimateSystem;
    private PlanetarySystem m_PlanetarySystem;

    public override GameMode gameMode => GameMode.Editor;

    private float m_NormalizedTimeBindingValue
    {
      get => MathUtils.Snap(this.m_PlanetarySystem.normalizedTime, 0.01f);
    }

    private float m_NormalizedDateBindingValue
    {
      get => MathUtils.Snap((float) this.m_ClimateSystem.currentDate, 0.01f);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(EditorBottomBarUISystem.kGroup, "timeOfDay", (Func<float>) (() => this.m_NormalizedTimeBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(EditorBottomBarUISystem.kGroup, "date", (Func<float>) (() => this.m_NormalizedDateBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>(EditorBottomBarUISystem.kGroup, "cloudiness", (Func<float>) (() => (float) this.m_ClimateSystem.cloudiness)));
      this.AddBinding((IBinding) new TriggerBinding<float>(EditorBottomBarUISystem.kGroup, "setTimeOfDay", new Action<float>(this.SetTimeOfDay)));
      this.AddBinding((IBinding) new TriggerBinding(EditorBottomBarUISystem.kGroup, "resetTimeOfDay", new Action(this.ResetTimeOfDay)));
      this.AddBinding((IBinding) new TriggerBinding<float>(EditorBottomBarUISystem.kGroup, "setDate", new Action<float>(this.SetDate)));
      this.AddBinding((IBinding) new TriggerBinding(EditorBottomBarUISystem.kGroup, "resetDate", new Action(this.ResetDate)));
      this.AddBinding((IBinding) new TriggerBinding<float>(EditorBottomBarUISystem.kGroup, "setCloudiness", new Action<float>(this.SetCloudiness)));
      this.AddBinding((IBinding) new TriggerBinding(EditorBottomBarUISystem.kGroup, "resetCloudiness", new Action(this.ResetCloudiness)));
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      this.m_PlanetarySystem.overrideTime = false;
      this.m_ClimateSystem.currentDate.overrideState = false;
      this.m_ClimateSystem.cloudiness.overrideState = false;
    }

    private void SetTimeOfDay(float time)
    {
      this.m_PlanetarySystem.overrideTime = true;
      this.m_PlanetarySystem.normalizedTime = time;
    }

    private void ResetTimeOfDay() => this.m_PlanetarySystem.overrideTime = false;

    private void SetDate(float date) => this.m_ClimateSystem.currentDate.overrideValue = date;

    private void ResetDate() => this.m_ClimateSystem.currentDate.overrideState = false;

    private void SetCloudiness(float cloudiness)
    {
      this.m_ClimateSystem.cloudiness.overrideValue = cloudiness;
    }

    private void ResetCloudiness() => this.m_ClimateSystem.cloudiness.overrideState = false;

    [Preserve]
    public EditorBottomBarUISystem()
    {
    }
  }
}
