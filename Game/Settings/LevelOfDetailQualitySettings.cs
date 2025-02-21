// Decompiled with JetBrains decompiler
// Type: Game.Settings.LevelOfDetailQualitySettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Unity.Entities;

#nullable disable
namespace Game.Settings
{
  [SettingsUIAdvanced]
  [SettingsUISection("LevelOfDetailQualitySettings")]
  [SettingsUIDisableByCondition(typeof (LevelOfDetailQualitySettings), "IsOptionsDisabled")]
  public class LevelOfDetailQualitySettings : QualitySetting<LevelOfDetailQualitySettings>
  {
    [SettingsUISlider(min = 10f, max = 100f, step = 1f, unit = "percentage", scalarMultiplier = 100f)]
    public float levelOfDetail { get; set; }

    public bool lodCrossFade { get; set; }

    [SettingsUISlider(min = 512f, max = 16384f, step = 256f, unit = "integer")]
    public int maxLightCount { get; set; }

    [SettingsUISlider(min = 128f, max = 4096f, step = 64f, unit = "dataMegabytes")]
    public int meshMemoryBudget { get; set; }

    public bool strictMeshMemory { get; set; }

    static LevelOfDetailQualitySettings()
    {
      QualitySetting<LevelOfDetailQualitySettings>.RegisterSetting(QualitySetting.Level.VeryLow, LevelOfDetailQualitySettings.veryLowQuality);
      QualitySetting<LevelOfDetailQualitySettings>.RegisterSetting(QualitySetting.Level.Low, LevelOfDetailQualitySettings.lowQuality);
      QualitySetting<LevelOfDetailQualitySettings>.RegisterSetting(QualitySetting.Level.Medium, LevelOfDetailQualitySettings.mediumQuality);
      QualitySetting<LevelOfDetailQualitySettings>.RegisterSetting(QualitySetting.Level.High, LevelOfDetailQualitySettings.highQuality);
    }

    public LevelOfDetailQualitySettings()
    {
    }

    public LevelOfDetailQualitySettings(QualitySetting.Level quality)
    {
      this.SetLevel(quality, false);
    }

    public override void Apply()
    {
      base.Apply();
      // ISSUE: variable of a compiler-generated type
      RenderingSystem existingSystemManaged1 = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<RenderingSystem>();
      if (existingSystemManaged1 != null)
      {
        existingSystemManaged1.levelOfDetail = this.levelOfDetail;
        existingSystemManaged1.lodCrossFade = this.lodCrossFade;
        existingSystemManaged1.maxLightCount = this.maxLightCount;
      }
      // ISSUE: variable of a compiler-generated type
      BatchMeshSystem existingSystemManaged2 = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<BatchMeshSystem>();
      if (existingSystemManaged2 == null)
        return;
      existingSystemManaged2.memoryBudget = (ulong) this.meshMemoryBudget * 1048576UL;
      existingSystemManaged2.strictMemoryBudget = this.strictMeshMemory;
    }

    private static LevelOfDetailQualitySettings highQuality
    {
      get
      {
        return new LevelOfDetailQualitySettings()
        {
          levelOfDetail = 0.7f,
          lodCrossFade = true,
          maxLightCount = 8192,
          meshMemoryBudget = 2048,
          strictMeshMemory = false
        };
      }
    }

    private static LevelOfDetailQualitySettings mediumQuality
    {
      get
      {
        return new LevelOfDetailQualitySettings()
        {
          levelOfDetail = 0.5f,
          lodCrossFade = true,
          maxLightCount = 4096,
          meshMemoryBudget = 1024,
          strictMeshMemory = false
        };
      }
    }

    private static LevelOfDetailQualitySettings lowQuality
    {
      get
      {
        return new LevelOfDetailQualitySettings()
        {
          levelOfDetail = 0.35f,
          lodCrossFade = true,
          maxLightCount = 2048,
          meshMemoryBudget = 512,
          strictMeshMemory = false
        };
      }
    }

    private static LevelOfDetailQualitySettings veryLowQuality
    {
      get
      {
        return new LevelOfDetailQualitySettings()
        {
          levelOfDetail = 0.25f,
          lodCrossFade = false,
          maxLightCount = 1024,
          meshMemoryBudget = 256,
          strictMeshMemory = true
        };
      }
    }
  }
}
