// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.EditorTutorialSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.SceneFlow;
using Game.Settings;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class EditorTutorialSystem : TutorialSystem
  {
    protected override Dictionary<string, bool> ShownTutorials
    {
      get => SharedSettings.instance.editor.shownTutorials;
    }

    public override bool tutorialEnabled
    {
      get => SharedSettings.instance.editor.showTutorials;
      set
      {
        SharedSettings.instance.editor.showTutorials = value;
        if (value)
          return;
        this.mode = TutorialMode.Default;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Setting = (Setting) SharedSettings.instance.editor;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActive>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialListQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialListData>(), ComponentType.ReadOnly<TutorialRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialPhaseRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingPriorityTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialPhaseRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<ReplaceActiveData>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<Locked>(), ComponentType.ReadOnly<EditorTutorial>());
    }

    public override void OnResetTutorials()
    {
      this.ShownTutorials.Clear();
      // ISSUE: reference to a compiler-generated method
      base.OnResetTutorials();
      if (!GameManager.instance.gameMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Mode = TutorialMode.ListIntro;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode gameMode)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnGamePreload(purpose, gameMode);
      this.Enabled = gameMode.IsEditor();
    }

    protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode gameMode)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnGameLoadingComplete(purpose, gameMode);
      // ISSUE: reference to a compiler-generated field
      if (gameMode != GameMode.Editor || !this.tutorialEnabled || this.ShownTutorials.ContainsKey(TutorialSystem.kListIntroKey))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Mode = TutorialMode.ListIntro;
    }

    [Preserve]
    public EditorTutorialSystem()
    {
    }
  }
}
