// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.UILocalizationManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using Colossal.Localization;

#nullable disable
namespace Game.UI.Localization
{
  public class UILocalizationManager : ILocalizationManager
  {
    private readonly LocalizationManager m_LocalizationManager;

    public UILocalizationManager(LocalizationManager localizationManager)
    {
      this.m_LocalizationManager = localizationManager;
    }

    private UILocalizationManager()
    {
    }

    public override void Translate(string key, ILocalizationManager.TranslationData data)
    {
      string text;
      if (this.m_LocalizationManager != null && this.m_LocalizationManager.activeDictionary.TryGetValue(key, out text))
        data.Set(text);
      else
        data.Set(key);
    }
  }
}
