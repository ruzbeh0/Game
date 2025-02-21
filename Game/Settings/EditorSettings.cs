// Decompiled with JetBrains decompiler
// Type: Game.Settings.EditorSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Tutorials;
using Unity.Entities;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Editor")]
  public class EditorSettings : Setting
  {
    public int prefabPickerColumnCount { get; set; }

    public string[] prefabPickerFavorites { get; set; }

    public string[] prefabPickerSearchHistory { get; set; }

    public string[] prefabPickerSearchFavorites { get; set; }

    public int assetPickerColumnCount { get; set; }

    public string[] assetPickerFavorites { get; set; }

    public string[] directoryPickerFavorites { get; set; }

    public int inspectorWidth { get; set; }

    public int hierarchyWidth { get; set; }

    public bool useParallelImport { get; set; }

    public bool lowQualityTextureCompression { get; set; }

    public string lastSelectedProjectRootDirectory { get; set; }

    public string lastSelectedImportDirectory { get; set; }

    public bool showTutorials { get; set; }

    public System.Collections.Generic.Dictionary<string, bool> shownTutorials { get; set; }

    [SettingsUIButton]
    [SettingsUIConfirmation(null, null)]
    public bool resetTutorials
    {
      set => this.ResetTutorials();
    }

    public void ResetTutorials()
    {
      this.shownTutorials.Clear();
      this.ApplyAndSave();
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EditorTutorialSystem>().OnResetTutorials();
    }

    public EditorSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.prefabPickerColumnCount = 1;
      this.prefabPickerFavorites = new string[0];
      this.prefabPickerSearchHistory = new string[0];
      this.prefabPickerSearchFavorites = new string[0];
      this.assetPickerColumnCount = 4;
      this.assetPickerFavorites = new string[0];
      this.directoryPickerFavorites = new string[0];
      this.inspectorWidth = 450;
      this.hierarchyWidth = 350;
      this.lastSelectedProjectRootDirectory = (string) null;
      this.lastSelectedImportDirectory = (string) null;
      this.useParallelImport = true;
      this.lowQualityTextureCompression = false;
      this.showTutorials = false;
      this.shownTutorials = new System.Collections.Generic.Dictionary<string, bool>();
    }
  }
}
