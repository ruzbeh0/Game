// Decompiled with JetBrains decompiler
// Type: Game.UI.TypeNames
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;

#nullable disable
namespace Game.UI
{
  public static class TypeNames
  {
    public const string kTutorialsGroup = "tutorials";
    public const string kEditorTutorialsGroup = "editorTutorials";
    public static readonly string kAdvisorCategory = "tutorials.AdvisorCategory";
    public static readonly string kAdvisorItem = "tutorials.AdvisorItem";
    public static readonly string kTutorial = "tutorials.Tutorial";
    public static readonly string kTutorialTrigger = "tutorials.Trigger";
    public static readonly string kTutorialPhase = "tutorials.Phase";
    public static readonly string kTutorialList = "tutorials.List";
    public static readonly string kTutorialBalloonUITarget = typeof (TutorialBalloonPrefab.BalloonUITarget).FullName ?? "";
    public const string kPoliciesGroup = "policies";
    public static readonly string kPolicy = "policies.Policy";
    public static readonly string kPolicySlider = "policies.Slider";
  }
}
