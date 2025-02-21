// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PresetDescriptor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering.CinematicCamera;
using System.Collections.Generic;

#nullable disable
namespace Game.UI.InGame
{
  public class PresetDescriptor
  {
    private List<string> m_OptionsId = new List<string>();
    private Dictionary<PhotoModeProperty, float[]> m_Values = new Dictionary<PhotoModeProperty, float[]>();

    public IReadOnlyCollection<string> optionsId => (IReadOnlyCollection<string>) this.m_OptionsId;

    public IReadOnlyDictionary<PhotoModeProperty, float[]> values
    {
      get => (IReadOnlyDictionary<PhotoModeProperty, float[]>) this.m_Values;
    }

    public bool Validate()
    {
      int count = this.m_OptionsId.Count;
      foreach (KeyValuePair<PhotoModeProperty, float[]> keyValuePair in this.m_Values)
      {
        if (keyValuePair.Value.Length != count)
          return false;
      }
      return true;
    }

    public void AddOptions(IEnumerable<string> optionIds)
    {
      foreach (string optionId in optionIds)
        this.AddOption(optionId);
    }

    public void AddOption(string optionId) => this.m_OptionsId.Add(optionId);

    public void AddValues(PhotoModeProperty targetProperty, float[] values)
    {
      this.m_Values.Add(targetProperty, values);
    }
  }
}
