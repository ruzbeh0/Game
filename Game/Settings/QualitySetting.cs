// Decompiled with JetBrains decompiler
// Type: Game.Settings.QualitySetting
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.Settings
{
  public abstract class QualitySetting : Setting
  {
    [Exclude]
    [IgnoreEquals]
    [SettingsUIHidden]
    public bool disableSetting { get; set; }

    public abstract QualitySetting.Level GetLevel();

    public abstract void SetLevel(QualitySetting.Level quality, bool apply = true);

    public abstract void TransferValuesFrom(QualitySetting setting);

    public abstract IEnumerable<QualitySetting.Level> EnumerateAvailableLevels();

    public abstract string GetMockName(QualitySetting.Level level);

    public override void SetDefaults()
    {
    }

    public DropdownItem<int>[] GetQualityValues()
    {
      QualitySetting.Level[] array = this.EnumerateAvailableLevels().ToArray<QualitySetting.Level>();
      List<DropdownItem<int>> dropdownItemList = new List<DropdownItem<int>>(array.Length);
      foreach (QualitySetting.Level level in array)
      {
        DropdownItem<int> dropdownItem = new DropdownItem<int>()
        {
          displayName = (LocalizedString) ("Options." + level.GetType().Name.ToUpperInvariant() + "[" + this.GetMockName(level) + "]"),
          value = (int) level,
          disabled = level == QualitySetting.Level.Custom
        };
        dropdownItemList.Add(dropdownItem);
      }
      return dropdownItemList.ToArray();
    }

    internal virtual void AddToPageData(AutomaticSettings.SettingPageData pageData)
    {
      AutomaticSettings.FillSettingsPage(pageData, (Setting) this);
    }

    public virtual bool IsOptionsDisabled()
    {
      return this.IsOptionFullyDisabled() || this.GetLevel() == QualitySetting.Level.Disabled;
    }

    public virtual bool IsOptionFullyDisabled() => this.disableSetting;

    public enum Level
    {
      Disabled,
      VeryLow,
      Low,
      Medium,
      High,
      Colossal,
      Custom,
    }
  }
}
