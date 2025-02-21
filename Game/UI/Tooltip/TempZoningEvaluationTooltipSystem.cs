// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempZoningEvaluationTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using Game.Tools;
using Game.UI.Widgets;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  public class TempZoningEvaluationTooltipSystem : TooltipSystemBase
  {
    private IZoningInfoSystem m_ZoningInfoSystem;
    private List<ZoningEvaluationTooltip> m_Tooltips;

    public int maxCount { get; set; } = 5;

    public float scoreThreshold { get; set; } = 10f;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ZoningInfoSystem = (IZoningInfoSystem) this.World.GetOrCreateSystemManaged<ZoningInfoSystem>();
      this.m_Tooltips = new List<ZoningEvaluationTooltip>(this.maxCount);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> evaluationResults = this.m_ZoningInfoSystem.evaluationResults;
      for (int index = 0; index < math.min(evaluationResults.Length, this.maxCount); ++index)
      {
        if (this.m_Tooltips.Count <= index)
        {
          List<ZoningEvaluationTooltip> tooltips = this.m_Tooltips;
          ZoningEvaluationTooltip evaluationTooltip = new ZoningEvaluationTooltip();
          evaluationTooltip.path = (PathSegment) string.Format("zoningEvaluation{0}", (object) index);
          tooltips.Add(evaluationTooltip);
        }
        ZoneEvaluationUtils.ZoningEvaluationResult evaluationResult = evaluationResults[index];
        if ((double) Mathf.Abs(evaluationResult.m_Score) > (double) this.scoreThreshold)
        {
          ZoningEvaluationTooltip tooltip = this.m_Tooltips[index];
          tooltip.factor = evaluationResult.m_Factor;
          tooltip.score = evaluationResult.m_Score;
          this.AddMouseTooltip((IWidget) tooltip);
        }
      }
    }

    [Preserve]
    public TempZoningEvaluationTooltipSystem()
    {
    }
  }
}
