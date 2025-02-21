// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AchievementsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.UI.Binding;
using System;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class AchievementsUISystem : UISystemBase
  {
    private const string kGroup = "achievements";
    private RawValueBinding m_AchievementsBinding;
    private GetterValueBinding<bool> m_ShowTabBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      PlatformManager.instance.onAchievementUpdated += new AchievementUpdatedEventHandler(this.UpdateAchievements);
      this.AddBinding((IBinding) (this.m_AchievementsBinding = new RawValueBinding("achievements", "achievements", new Action<IJsonWriter>(this.BindAchievements))));
      this.AddBinding((IBinding) (this.m_ShowTabBinding = new GetterValueBinding<bool>("achievements", "showAchievementsTab", (Func<bool>) (() => PlatformManager.instance.CountAchievements(false) > 0))));
    }

    [Preserve]
    protected override void OnDestroy() => base.OnDestroy();

    [Preserve]
    protected override void OnUpdate()
    {
    }

    private void UpdateAchievements(IAchievementsSupport backend, AchievementId id)
    {
      this.m_AchievementsBinding.Update();
    }

    private void BindAchievements(IJsonWriter binder)
    {
      int size = PlatformManager.instance.CountAchievements(false);
      this.m_ShowTabBinding.Update();
      if (size > 0)
      {
        binder.ArrayBegin(size);
        foreach (IAchievement enumerateAchievement in PlatformManager.instance.EnumerateAchievements())
          this.BindAchievement(binder, enumerateAchievement);
        binder.ArrayEnd();
      }
      else
      {
        binder.ArrayBegin(0U);
        binder.ArrayEnd();
      }
    }

    private void BindAchievement(IJsonWriter binder, IAchievement achievement)
    {
      binder.TypeBegin("achievements.Achievement");
      binder.PropertyName("localeKey");
      binder.Write(achievement.internalName);
      bool locked = !achievement.achieved;
      binder.PropertyName("imagePath");
      binder.Write(this.GetImagePath(achievement, locked));
      binder.PropertyName("locked");
      binder.Write(locked);
      binder.PropertyName("isIncremental");
      binder.Write(achievement.isIncremental);
      binder.PropertyName("progress");
      binder.Write(achievement.progress);
      binder.PropertyName("maxProgress");
      binder.Write(achievement.maxProgress);
      binder.TypeEnd();
    }

    private string GetImagePath(IAchievement achievement, bool locked)
    {
      return "Media/Game/Achievements/" + achievement.internalName + (locked ? "_locked" : "") + ".png";
    }

    [Preserve]
    public AchievementsUISystem()
    {
    }
  }
}
