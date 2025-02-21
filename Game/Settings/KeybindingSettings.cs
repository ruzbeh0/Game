// Decompiled with JetBrains decompiler
// Type: Game.Settings.KeybindingSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Input;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  public class KeybindingSettings : Setting
  {
    private bool m_IsDefault;

    [SettingsUIHidden]
    public List<ProxyBinding> bindings
    {
      get
      {
        return InputManager.instance.GetBindings(this.m_IsDefault ? InputManager.PathType.Original : InputManager.PathType.Effective, InputManager.BindingOptions.OnlyRebound | InputManager.BindingOptions.OnlyBuiltIn).ToList<ProxyBinding>();
      }
      set
      {
        if (this.m_IsDefault)
          return;
        InputManager.instance.SetBindings((IEnumerable<ProxyBinding>) value, out List<ProxyBinding> _);
      }
    }

    public KeybindingSettings(bool isDefault = false) => this.m_IsDefault = isDefault;

    public override void SetDefaults()
    {
    }
  }
}
