// Decompiled with JetBrains decompiler
// Type: Game.Settings.InputSettings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Input;
using Game.UI.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Settings
{
  [FileLocation("Settings")]
  [SettingsUITabOrder(typeof (InputSettings), "GetTabOrder")]
  [SettingsUIGroupOrder(new string[] {"General", "Navigation", "Camera", "Tool", "Menu", "Simulation", "Toolbar", "SIP", "Tutorial", "Photo mode", "Editor", "Shortcuts", "Debug"})]
  [SettingsUITabWarning("Keyboard", typeof (InputSettings), "isKeyboardConflict")]
  [SettingsUITabWarning("Mouse", typeof (InputSettings), "isMouseConflict")]
  [SettingsUITabWarning("Gamepad", typeof (InputSettings), "isGamepadConflict")]
  public class InputSettings : Setting
  {
    public const string kName = "Input";
    public const string kMiscTab = "Misc";

    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadActive")]
    [SettingsUISection("Misc", "General")]
    public bool elevationDraggingEnabled { get; set; }

    [SettingsUISection("Mouse", "Navigation")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public float mouseScrollSensitivity { get; set; }

    public float finalScrollSensitivity => this.mouseScrollSensitivity;

    [SettingsUISection("Keyboard", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsKeyboardConnected", true)]
    public float keyboardMoveSensitivity { get; set; }

    [SettingsUISection("Keyboard", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsKeyboardConnected", true)]
    public float keyboardRotateSensitivity { get; set; }

    [SettingsUISection("Keyboard", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsKeyboardConnected", true)]
    public float keyboardZoomSensitivity { get; set; }

    [SettingsUISection("Mouse", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public float mouseMoveSensitivity { get; set; }

    [SettingsUISection("Mouse", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public float mouseRotateSensitivity { get; set; }

    [SettingsUISection("Mouse", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public float mouseZoomSensitivity { get; set; }

    [SettingsUISection("Mouse", "Camera")]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public bool mouseInvertX { get; set; }

    [SettingsUISection("Mouse", "Camera")]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsMouseConnected", true)]
    public bool mouseInvertY { get; set; }

    [SettingsUISection("Gamepad", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadConnected", true)]
    public float gamepadMoveSensitivity { get; set; }

    [SettingsUISection("Gamepad", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadConnected", true)]
    public float gamepadRotateSensitivity { get; set; }

    [SettingsUISection("Gamepad", "Camera")]
    [SettingsUISlider(min = 0.1f, max = 5f, step = 0.1f, unit = "custom")]
    [SettingsUICustomFormat(fractionDigits = 1)]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadConnected", true)]
    public float gamepadZoomSensitivity { get; set; }

    [SettingsUISection("Gamepad", "Camera")]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadConnected", true)]
    public bool gamepadInvertX { get; set; }

    [SettingsUISection("Gamepad", "Camera")]
    [SettingsUIHideByCondition(typeof (Game.Input.InputManager), "IsGamepadConnected", true)]
    public bool gamepadInvertY { get; set; }

    private string[] GetTabOrder()
    {
      return Game.Input.InputManager.instance.activeControlScheme == Game.Input.InputManager.ControlScheme.Gamepad ? new string[4]
      {
        "Gamepad",
        "Keyboard",
        "Mouse",
        "Misc"
      } : new string[4]
      {
        "Keyboard",
        "Mouse",
        "Gamepad",
        "Misc"
      };
    }

    public InputSettings() => this.SetDefaults();

    public override void SetDefaults()
    {
      this.elevationDraggingEnabled = false;
      this.SetDefaultsForDevice(Game.Input.InputManager.DeviceType.Keyboard);
      this.SetDefaultsForDevice(Game.Input.InputManager.DeviceType.Mouse);
      this.SetDefaultsForDevice(Game.Input.InputManager.DeviceType.Gamepad);
    }

    private void SetDefaultsForDevice(Game.Input.InputManager.DeviceType device)
    {
      switch (device)
      {
        case Game.Input.InputManager.DeviceType.Keyboard:
          this.keyboardMoveSensitivity = 1f;
          this.keyboardZoomSensitivity = 1f;
          this.keyboardRotateSensitivity = 1f;
          break;
        case Game.Input.InputManager.DeviceType.Mouse:
          this.mouseMoveSensitivity = 1f;
          this.mouseRotateSensitivity = 1f;
          this.mouseZoomSensitivity = 1f;
          this.mouseInvertX = false;
          this.mouseInvertY = false;
          this.mouseScrollSensitivity = 1f;
          break;
        case Game.Input.InputManager.DeviceType.Gamepad:
          this.gamepadMoveSensitivity = 1f;
          this.gamepadZoomSensitivity = 1f;
          this.gamepadRotateSensitivity = 1f;
          this.gamepadInvertX = false;
          this.gamepadInvertY = false;
          break;
      }
    }

    private bool isKeyboardConflict
    {
      get => (Game.Input.InputManager.instance.bindingConflicts & Game.Input.InputManager.DeviceType.Keyboard) != 0;
    }

    private bool isMouseConflict
    {
      get => (Game.Input.InputManager.instance.bindingConflicts & Game.Input.InputManager.DeviceType.Mouse) != 0;
    }

    private bool isGamepadConflict
    {
      get => (Game.Input.InputManager.instance.bindingConflicts & Game.Input.InputManager.DeviceType.Gamepad) != 0;
    }

    public override AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix)
    {
      AutomaticSettings.SettingPageData pageData = base.GetPageData(id, addPrefix);
      if (UnityEngine.InputSystem.InputSystem.devices.Count != 0)
      {
        if (UnityEngine.InputSystem.InputSystem.devices.Any<InputDevice>((Func<InputDevice, bool>) (d => d.added && d is Keyboard)))
          this.GetPageSection(pageData, Game.Input.InputManager.DeviceType.Keyboard);
        if (UnityEngine.InputSystem.InputSystem.devices.Any<InputDevice>((Func<InputDevice, bool>) (d => d.added && d is Mouse)))
          this.GetPageSection(pageData, Game.Input.InputManager.DeviceType.Mouse);
        if (UnityEngine.InputSystem.InputSystem.devices.Any<InputDevice>((Func<InputDevice, bool>) (d => d.added && d is Gamepad)))
          this.GetPageSection(pageData, Game.Input.InputManager.DeviceType.Gamepad);
      }
      return pageData;
    }

    private void GetPageSection(
      AutomaticSettings.SettingPageData pageData,
      Game.Input.InputManager.DeviceType device)
    {
      AutomaticSettings.SettingItemData settingItemData1 = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.BoolButtonWithConfirmation, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (InputSettings), typeof (bool), "resetButton")
      {
        canRead = false,
        canWrite = true,
        attributes = {
          (System.Attribute) new SettingsUIButtonAttribute(),
          (System.Attribute) new SettingsUIPathAttribute(string.Format("{0}.{1}.resetbutton", (object) nameof (InputSettings), (object) device)),
          (System.Attribute) new SettingsUIButtonGroupAttribute(string.Format("{0}.{1}.resetbutton_Group", (object) nameof (InputSettings), (object) device)),
          (System.Attribute) new SettingsUIConfirmationAttribute(string.Format("{0}.{1}.resetbutton", (object) nameof (InputSettings), (object) device)),
          (System.Attribute) new SettingsUIDisplayNameAttribute(string.Format("{0}.{1}.resetbutton", (object) nameof (InputSettings), (object) device))
        },
        setter = (Action<object, object>) ((obj, value) =>
        {
          Game.Input.InputManager.instance.ResetGroupBindings(device);
          this.SetDefaultsForDevice(device);
          this.ApplyAndSave();
        })
      }, pageData.prefix)
      {
        simpleGroup = "General"
      };
      pageData[device.ToString()].AddItem(settingItemData1);
      pageData.AddGroup("General");
      foreach (ProxyAction action in Game.Input.InputManager.instance.actions)
      {
        foreach (KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite> composite in (IEnumerable<KeyValuePair<Game.Input.InputManager.DeviceType, ProxyComposite>>) action.composites)
        {
          Game.Input.InputManager.DeviceType deviceType;
          ProxyComposite proxyComposite1;
          composite.Deconstruct(ref deviceType, ref proxyComposite1);
          ProxyComposite proxyComposite2 = proxyComposite1;
          if (proxyComposite2.m_Device == device && action.isBuiltIn && !proxyComposite2.isDummy)
          {
            ActionComponent actionComponent;
            ProxyBinding proxyBinding1;
            if (!proxyComposite2.isHidden)
            {
              foreach (KeyValuePair<ActionComponent, ProxyBinding> binding1 in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
              {
                binding1.Deconstruct(ref actionComponent, ref proxyBinding1);
                ProxyBinding binding = proxyBinding1;
                AutomaticSettings.SettingItemData settingItemData2 = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.KeyBinding, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (InputSettings), binding.GetType(), binding.name)
                {
                  attributes = {
                    (System.Attribute) new SettingsUIPathAttribute(string.Format("{0}.{1}.{2}", (object) nameof (InputSettings), (object) device, (object) binding.title))
                  },
                  getter = (Func<object, object>) (_ => (object) binding)
                }, pageData.prefix)
                {
                  simpleGroup = binding.GetOptionsGroup()
                };
                pageData[device.ToString()].AddItem(settingItemData2);
                pageData.AddGroup(settingItemData2.simpleGroup);
                pageData.AddGroupToShowName(settingItemData2.simpleGroup);
              }
            }
            foreach (UIBaseInputAction uiAlias in action.m_UIAliases)
            {
              if (uiAlias.showInOptions)
              {
                foreach (UIInputActionPart actionPart in (IEnumerable<UIInputActionPart>) uiAlias.actionParts)
                {
                  if ((actionPart.m_Mask & device) != Game.Input.InputManager.DeviceType.None)
                  {
                    foreach (KeyValuePair<ActionComponent, ProxyBinding> binding in (IEnumerable<KeyValuePair<ActionComponent, ProxyBinding>>) proxyComposite2.bindings)
                    {
                      binding.Deconstruct(ref actionComponent, ref proxyBinding1);
                      ProxyBinding proxyBinding2 = proxyBinding1;
                      if (actionPart.m_Transform == UIBaseInputAction.Transform.None || (proxyBinding2.component.ToTransform() & actionPart.m_Transform) != UIBaseInputAction.Transform.None)
                      {
                        ProxyBinding aliasBinding = proxyBinding2.Copy() with
                        {
                          alies = uiAlias
                        };
                        AutomaticSettings.SettingItemData settingItemData3 = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.KeyBinding, (Setting) this, (AutomaticSettings.IProxyProperty) new AutomaticSettings.ManualProperty(typeof (InputSettings), aliasBinding.GetType(), aliasBinding.name)
                        {
                          attributes = {
                            (System.Attribute) new SettingsUIPathAttribute(string.Format("{0}.{1}.{2}", (object) nameof (InputSettings), (object) device, (object) aliasBinding.title))
                          },
                          getter = (Func<object, object>) (_ => (object) aliasBinding)
                        }, pageData.prefix)
                        {
                          simpleGroup = aliasBinding.GetOptionsGroup()
                        };
                        pageData[device.ToString()].AddItem(settingItemData3);
                        pageData.AddGroup(settingItemData3.simpleGroup);
                        pageData.AddGroupToShowName(settingItemData3.simpleGroup);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
