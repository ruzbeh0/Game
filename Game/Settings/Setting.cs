// Decompiled with JetBrains decompiler
// Type: Game.Settings.Setting
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.Logging;
using Colossal.Reflection;
using Game.SceneFlow;
using Game.UI.Menu;
using System;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#nullable disable
namespace Game.Settings
{
  public abstract class Setting : IEquatable<Setting>
  {
    protected static ILog log = LogManager.GetLogger("SceneFlow");

    protected static SharedSettings settings => GameManager.instance?.settings;

    [Exclude]
    protected internal virtual bool builtIn => true;

    public event OnSettingsAppliedHandler onSettingsApplied;

    public bool Equals(Setting obj) => this.Equals((object) obj);

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      System.Type type = obj.GetType();
      if (!type.IsAssignableFrom(this.GetType()))
        return false;
      PropertyInfo property1 = type.GetProperty("enabled", BindingFlags.Instance | BindingFlags.Public);
      if (property1 != (PropertyInfo) null && !(bool) property1.GetValue((object) this) && object.Equals(property1.GetValue((object) this), property1.GetValue(obj)))
        return true;
      foreach (PropertyInfo property2 in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (ReflectionUtils.GetAttribute<IgnoreEqualsAttribute>(property2.GetCustomAttributes(false)) == null && property2.CanRead && !object.Equals(property2.GetValue((object) this), property2.GetValue(obj)))
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        hashCode = hashCode * 937 ^ property.GetValue((object) this).GetHashCode();
      return hashCode;
    }

    protected bool TryGetGameplayCameraController(ref CameraController controller)
    {
      if ((UnityEngine.Object) controller != (UnityEngine.Object) null)
        return true;
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("GameplayCamera");
      if ((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null)
      {
        controller = gameObjectWithTag.GetComponent<CameraController>();
        return true;
      }
      controller = (CameraController) null;
      return false;
    }

    protected bool TryGetGameplayCamera(ref HDAdditionalCameraData cameraData)
    {
      if ((UnityEngine.Object) cameraData != (UnityEngine.Object) null)
        return true;
      Camera main = Camera.main;
      if ((UnityEngine.Object) main != (UnityEngine.Object) null)
      {
        cameraData = main.GetComponent<HDAdditionalCameraData>();
        return true;
      }
      cameraData = (HDAdditionalCameraData) null;
      return false;
    }

    protected bool TryGetGameplayCamera(ref Camera camera)
    {
      if ((UnityEngine.Object) camera != (UnityEngine.Object) null)
        return true;
      camera = Camera.main;
      return (UnityEngine.Object) camera != (UnityEngine.Object) null;
    }

    protected bool TryGetSunLight(ref Light sunLight)
    {
      if ((UnityEngine.Object) sunLight != (UnityEngine.Object) null)
        return true;
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("SunLight");
      if ((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null)
      {
        sunLight = gameObjectWithTag.GetComponent<Light>();
        return true;
      }
      sunLight = (Light) null;
      return false;
    }

    protected bool TryGetSunLightData(ref HDAdditionalLightData sunLightData)
    {
      if ((UnityEngine.Object) sunLightData != (UnityEngine.Object) null)
        return true;
      GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("SunLight");
      if ((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null)
      {
        sunLightData = gameObjectWithTag.GetComponent<HDAdditionalLightData>();
        return true;
      }
      sunLightData = (HDAdditionalLightData) null;
      return false;
    }

    public async void ApplyAndSave()
    {
      this.Apply();
      await Colossal.IO.AssetDatabase.AssetDatabase.global.SaveSettings();
    }

    public virtual void Apply()
    {
      Setting.log.VerboseFormat("Applying settings for {0}", (object) this.GetType());
      OnSettingsAppliedHandler onSettingsApplied = this.onSettingsApplied;
      if (onSettingsApplied == null)
        return;
      onSettingsApplied(this);
    }

    public abstract void SetDefaults();

    public virtual AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix)
    {
      return AutomaticSettings.FillSettingsPage(this, id, addPrefix);
    }

    internal void RegisterInOptionsUI(string name, bool addPrefix = false)
    {
      Setting.RegisterInOptionsUI(this, name, addPrefix);
    }

    internal static bool RegisterInOptionsUI(Setting instance, string name, bool addPrefix)
    {
      // ISSUE: variable of a compiler-generated type
      OptionsUISystem systemManaged = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OptionsUISystem>();
      if (systemManaged == null)
        return false;
      // ISSUE: reference to a compiler-generated method
      systemManaged.RegisterSetting(instance, name, addPrefix);
      return true;
    }

    internal static bool UnregisterInOptionsUI(string name)
    {
      // ISSUE: variable of a compiler-generated type
      OptionsUISystem systemManaged = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OptionsUISystem>();
      if (systemManaged == null)
        return false;
      // ISSUE: reference to a compiler-generated method
      systemManaged.UnregisterSettings(name);
      return true;
    }
  }
}
