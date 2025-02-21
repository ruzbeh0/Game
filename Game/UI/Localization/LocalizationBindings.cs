// Decompiled with JetBrains decompiler
// Type: Game.UI.Localization.LocalizationBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Localization;
using Colossal.UI.Binding;
using Game.Settings;
using System;

#nullable disable
namespace Game.UI.Localization
{
  public class LocalizationBindings : CompositeBinding, IDisposable
  {
    private const string kGroup = "l10n";
    private readonly LocalizationManager m_LocalizationManager;
    private readonly GetterValueBinding<string[]> m_LocalesBinding;
    private readonly ValueBinding<int> m_DebugModeBinding;
    private readonly EventBinding m_ActiveDictionaryChangedBinding;
    private readonly RawMapBinding<string> m_IndexCountsBinding;

    public LocalizationBindings.DebugMode debugMode
    {
      get => (LocalizationBindings.DebugMode) this.m_DebugModeBinding.value;
      set => this.m_DebugModeBinding.Update((int) value);
    }

    public LocalizationBindings(LocalizationManager localizationManager)
    {
      this.m_LocalizationManager = localizationManager;
      this.AddBinding((IBinding) (this.m_LocalesBinding = new GetterValueBinding<string[]>("l10n", "locales", (Func<string[]>) (() => this.m_LocalizationManager.GetSupportedLocales()), (IWriter<string[]>) new ArrayWriter<string>((IWriter<string>) new StringWriter()))));
      this.AddBinding((IBinding) (this.m_DebugModeBinding = new ValueBinding<int>("l10n", nameof (debugMode), 0)));
      this.AddBinding((IBinding) (this.m_ActiveDictionaryChangedBinding = new EventBinding("l10n", "activeDictionaryChanged")));
      this.AddBinding((IBinding) (this.m_IndexCountsBinding = new RawMapBinding<string>("l10n", "indexCounts", new Action<IJsonWriter, string>(this.BindIndexCounts))));
      this.AddBinding((IBinding) new TriggerBinding<string>("l10n", "selectLocale", new Action<string>(this.SelectLocale)));
      this.m_LocalizationManager.onSupportedLocalesChanged += new Action(this.OnSupportedLocalesChanged);
      this.m_LocalizationManager.onActiveDictionaryChanged += new Action(this.OnActiveDictionaryChanged);
    }

    public void Dispose()
    {
      this.m_LocalizationManager.onSupportedLocalesChanged -= new Action(this.OnSupportedLocalesChanged);
      this.m_LocalizationManager.onActiveDictionaryChanged -= new Action(this.OnActiveDictionaryChanged);
    }

    private void OnSupportedLocalesChanged() => this.m_LocalesBinding.Update();

    private void OnActiveDictionaryChanged()
    {
      this.m_ActiveDictionaryChangedBinding.Trigger();
      this.m_IndexCountsBinding.UpdateAll();
    }

    private void BindIndexCounts(IJsonWriter binder, string key)
    {
      int num;
      binder.Write(this.m_LocalizationManager.activeDictionary.indexCounts.TryGetValue(key, out num) ? num : 0);
    }

    private void SelectLocale(string localeID)
    {
      this.m_LocalizationManager?.SetActiveLocale(localeID);
      InterfaceSettings userInterface = SharedSettings.instance?.userInterface;
      if (userInterface == null)
        return;
      userInterface.locale = localeID;
    }

    public enum DebugMode
    {
      None,
      Id,
      Fallback,
    }
  }
}
