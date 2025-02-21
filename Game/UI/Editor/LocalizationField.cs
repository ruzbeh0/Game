// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.LocalizationField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.UI.Binding;
using Game.SceneFlow;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class LocalizationField : Widget
  {
    private List<LocalizationField.LocalizationFieldEntry> m_Localization = new List<LocalizationField.LocalizationFieldEntry>();

    public IReadOnlyList<LocalizationField.LocalizationFieldEntry> localization
    {
      get => (IReadOnlyList<LocalizationField.LocalizationFieldEntry>) this.m_Localization;
    }

    public LocalizedString placeholder { get; set; }

    public LocalizationField(LocalizedString placeholder)
    {
      this.placeholder = placeholder;
      this.Initialize();
    }

    public void Clear() => this.m_Localization.Clear();

    public void Initialize()
    {
      this.Clear();
      this.InitializeMandatory();
      if (this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (loc => loc.localeId == GameManager.instance.localizationManager.activeLocaleId)) >= 0)
        return;
      this.m_Localization.Add(new LocalizationField.LocalizationFieldEntry()
      {
        localeId = GameManager.instance.localizationManager.activeLocaleId,
        text = string.Empty
      });
    }

    public void Initialize(IEnumerable<LocaleAsset> assets, string localeFormat)
    {
      this.Clear();
      this.InitializeMandatory();
      if (assets == null)
        return;
      foreach (LocaleAsset asset in assets)
        this.Add(asset, localeFormat);
    }

    public void Initialize(
      IEnumerable<LocalizationField.LocalizationFieldEntry> entries)
    {
      this.Clear();
      this.InitializeMandatory();
      foreach (LocalizationField.LocalizationFieldEntry entry in entries)
        this.Add(entry);
    }

    public static bool IsMandatory(string localeId)
    {
      return localeId == GameManager.instance.localizationManager.fallbackLocaleId;
    }

    private void InitializeMandatory()
    {
      if (string.IsNullOrEmpty(GameManager.instance.localizationManager.fallbackLocaleId))
        return;
      int index1 = this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (l => LocalizationField.IsMandatory(l.localeId)));
      if (index1 < 0)
      {
        this.m_Localization.Add(new LocalizationField.LocalizationFieldEntry()
        {
          localeId = GameManager.instance.localizationManager.fallbackLocaleId,
          text = string.Empty
        });
      }
      else
      {
        if (index1 <= 0)
          return;
        List<LocalizationField.LocalizationFieldEntry> localization1 = this.m_Localization;
        List<LocalizationField.LocalizationFieldEntry> localization2 = this.m_Localization;
        int index2 = index1;
        LocalizationField.LocalizationFieldEntry localizationFieldEntry1 = this.m_Localization[index1];
        LocalizationField.LocalizationFieldEntry localizationFieldEntry2 = this.m_Localization[0];
        LocalizationField.LocalizationFieldEntry localizationFieldEntry3;
        LocalizationField.LocalizationFieldEntry localizationFieldEntry4 = localizationFieldEntry3 = localizationFieldEntry1;
        localization1[0] = localizationFieldEntry3;
        localization2[index2] = localizationFieldEntry4 = localizationFieldEntry2;
      }
    }

    public void Add(LocaleAsset asset, string localeFormat)
    {
      foreach (string key in asset.data.entries.Keys)
      {
        if (key == localeFormat && !string.IsNullOrEmpty(asset.data.entries[key]))
        {
          int index = this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (loc => loc.localeId == asset.localeId));
          if (index >= 0)
          {
            LocalizationField.LocalizationFieldEntry localizationFieldEntry = this.m_Localization[index] with
            {
              text = asset.data.entries[key]
            };
            this.m_Localization[index] = localizationFieldEntry;
          }
          else
            this.m_Localization.Add(new LocalizationField.LocalizationFieldEntry()
            {
              localeId = asset.localeId,
              text = asset.data.entries[key]
            });
        }
      }
    }

    public void Add(LocalizationField.LocalizationFieldEntry entry)
    {
      int index = this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (loc => loc.localeId == entry.localeId));
      if (index < 0)
        this.m_Localization.Add(entry);
      else
        this.m_Localization[index] = entry;
    }

    public void SetEntry(int index, string localeId, string text)
    {
      LocalizationField.LocalizationFieldEntry localizationFieldEntry = this.m_Localization[index] with
      {
        localeId = localeId,
        text = text
      };
      this.m_Localization[index] = localizationFieldEntry;
      this.SetPropertiesChanged();
    }

    public void AddLanguage()
    {
      string localeID;
      if (this.TryGetUnusedLanguage(out localeID))
        this.m_Localization.Add(new LocalizationField.LocalizationFieldEntry()
        {
          localeId = localeID,
          text = string.Empty
        });
      this.SetPropertiesChanged();
    }

    public void RemoveLanguage(int index)
    {
      if (LocalizationField.IsMandatory(this.m_Localization[index].localeId))
      {
        LocalizationField.LocalizationFieldEntry localizationFieldEntry = this.m_Localization[index] with
        {
          text = string.Empty
        };
        this.m_Localization[index] = localizationFieldEntry;
      }
      else
        this.m_Localization.RemoveAt(index);
      this.SetPropertiesChanged();
    }

    public bool IsValid()
    {
      if (!string.IsNullOrEmpty(GameManager.instance.localizationManager.fallbackLocaleId))
      {
        int index = this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (loc => loc.localeId == GameManager.instance.localizationManager.fallbackLocaleId));
        if (index >= 0)
          return this.IsValid(this.m_Localization[index]);
      }
      return this.ValidEntries().Any<LocalizationField.LocalizationFieldEntry>();
    }

    public IEnumerable<LocalizationField.LocalizationFieldEntry> ValidEntries()
    {
      foreach (LocalizationField.LocalizationFieldEntry entry in this.m_Localization)
      {
        if (this.IsValid(entry))
          yield return entry;
      }
    }

    private bool IsValid(LocalizationField.LocalizationFieldEntry entry)
    {
      return !string.IsNullOrWhiteSpace(entry.text);
    }

    private bool TryGetUnusedLanguage(out string localeID)
    {
      if (this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (entry => entry.localeId == GameManager.instance.localizationManager.activeLocaleId)) < 0)
      {
        localeID = GameManager.instance.localizationManager.activeLocaleId;
        return true;
      }
      foreach (string supportedLocale in GameManager.instance.localizationManager.GetSupportedLocales())
      {
        string locale = supportedLocale;
        if (this.m_Localization.FindIndex((Predicate<LocalizationField.LocalizationFieldEntry>) (entry => entry.localeId == locale)) < 0)
        {
          localeID = locale;
          return true;
        }
      }
      localeID = (string) null;
      return false;
    }

    public void BuildLocaleData(
      string format,
      System.Collections.Generic.Dictionary<string, LocaleData> localeDatas,
      string fallback = null)
    {
      foreach (LocalizationField.LocalizationFieldEntry entry in this.m_Localization)
      {
        if (this.IsValid(entry))
        {
          if (!localeDatas.ContainsKey(entry.localeId))
            localeDatas[entry.localeId] = new LocaleData(entry.localeId, new System.Collections.Generic.Dictionary<string, string>(), new System.Collections.Generic.Dictionary<string, int>());
          localeDatas[entry.localeId].entries[format] = entry.text;
        }
      }
      if (fallback == null)
        return;
      string fallbackLocaleId = GameManager.instance.localizationManager.fallbackLocaleId;
      if (!localeDatas.ContainsKey(fallbackLocaleId))
        localeDatas[fallbackLocaleId] = new LocaleData(fallbackLocaleId, new System.Collections.Generic.Dictionary<string, string>(), new System.Collections.Generic.Dictionary<string, int>());
      localeDatas[fallbackLocaleId].entries.TryAdd(format, fallback);
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("supportedLocales");
      writer.Write(GameManager.instance.localizationManager.GetSupportedLocales());
      writer.PropertyName("localization");
      writer.Write<LocalizationField.LocalizationFieldEntry>((IList<LocalizationField.LocalizationFieldEntry>) this.m_Localization);
      writer.PropertyName("placeholder");
      writer.Write<LocalizedString>(this.placeholder);
      writer.PropertyName("mandatoryId");
      writer.Write(GameManager.instance.localizationManager.fallbackLocaleId);
    }

    public struct LocalizationFieldEntry : IJsonWritable
    {
      public string localeId { get; set; }

      public string text { get; set; }

      public LocalizationFieldEntry(string localeId, string text)
      {
        this.localeId = localeId;
        this.text = text;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(nameof (LocalizationFieldEntry));
        writer.PropertyName("localeId");
        writer.Write(this.localeId);
        writer.PropertyName("text");
        writer.Write(this.text);
        writer.TypeEnd();
      }
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, int, string, string>(group, "setLocalizationEntry", (Action<IWidget, int, string, string>) ((widget, index, localeId, text) =>
        {
          if (!(widget is LocalizationField localizationField2))
            return;
          localizationField2.SetEntry(index, localeId, text);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget>(group, "addLocalizationEntry", (Action<IWidget>) (widget =>
        {
          if (!(widget is LocalizationField localizationField4))
            return;
          localizationField4.AddLanguage();
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int>(group, "removeLocalizationEntry", (Action<IWidget, int>) ((widget, index) =>
        {
          if (!(widget is LocalizationField localizationField6))
            return;
          localizationField6.RemoveLanguage(index);
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
