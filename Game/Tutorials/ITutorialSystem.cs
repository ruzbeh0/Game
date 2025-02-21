// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.ITutorialSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Tutorials
{
  public interface ITutorialSystem
  {
    Entity activeTutorial { get; }

    Entity activeTutorialPhase { get; }

    Entity activeTutorialList { get; }

    void CompleteCurrentTutorialPhase();

    void SetTutorial(Entity tutorial, Entity phase);

    void ForceTutorial(Entity tutorial, Entity phase, bool advisorActivation);

    bool tutorialEnabled { get; set; }

    TutorialMode mode { get; set; }

    Entity tutorialPending { get; }

    Entity nextListTutorial { get; }

    bool showListReminder { get; }

    void CompleteTutorial(Entity tutorial);
  }
}
