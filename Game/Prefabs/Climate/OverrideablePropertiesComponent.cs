// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Climate.OverrideablePropertiesComponent
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Prefabs.Climate
{
  public abstract class OverrideablePropertiesComponent : ComponentBase
  {
    public OverrideablePropertiesComponent.InterpolationMode m_InterpolationMode;
    public float m_InterpolationTime = 5f;

    public bool hasTimeBasedInterpolation
    {
      get
      {
        return this.m_InterpolationMode == OverrideablePropertiesComponent.InterpolationMode.RealTime || this.m_InterpolationMode == OverrideablePropertiesComponent.InterpolationMode.RenderingTime;
      }
    }

    public ReadOnlyCollection<VolumeParameter> parameters { get; private set; }

    protected abstract void OnBindVolumeProperties(Volume volume);

    protected override void OnEnable()
    {
      this.CollectVolumeParameters();
      foreach (VolumeParameter parameter in this.parameters)
      {
        if (parameter != null)
          parameter.OnEnable();
        else
          Debug.LogWarning((object) ("OverrideablePropertiesComponent " + this.GetType().Name + " contains a null parameter"));
      }
    }

    public void Bind(Volume volume)
    {
      this.OnBindVolumeProperties(volume);
      this.CollectVolumeParameters();
    }

    public void CollectVolumeParameters()
    {
      List<VolumeParameter> parameters = new List<VolumeParameter>();
      OverrideablePropertiesComponent.FindParameters((object) this, parameters);
      this.parameters = parameters.AsReadOnly();
    }

    public FieldInfo[] GetFieldsInfo()
    {
      return ((IEnumerable<FieldInfo>) this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).OrderBy<FieldInfo, int>((Func<FieldInfo, int>) (t => t.MetadataToken)).Where<FieldInfo>((Func<FieldInfo, bool>) (x => x.FieldType.IsSubclassOf(typeof (VolumeParameter)))).ToArray<FieldInfo>();
    }

    private static void FindParameters(
      object o,
      List<VolumeParameter> parameters,
      Func<FieldInfo, bool> filter = null)
    {
      if (o == null)
        return;
      foreach (FieldInfo fieldInfo in (IEnumerable<FieldInfo>) ((IEnumerable<FieldInfo>) o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).OrderBy<FieldInfo, int>((Func<FieldInfo, int>) (t => t.MetadataToken)))
      {
        if (fieldInfo.FieldType.IsSubclassOf(typeof (VolumeParameter)))
        {
          if ((filter != null ? (filter(fieldInfo) ? 1 : 0) : 1) != 0)
          {
            VolumeParameter volumeParameter = (VolumeParameter) fieldInfo.GetValue(o);
            parameters.Add(volumeParameter);
          }
        }
        else if (!fieldInfo.FieldType.IsArray && fieldInfo.FieldType.IsClass)
          OverrideablePropertiesComponent.FindParameters(fieldInfo.GetValue(o), parameters, filter);
      }
    }

    private void SetOverridesTo(IEnumerable<VolumeParameter> enumerable, bool state)
    {
      foreach (VolumeParameter volumeParameter in enumerable)
      {
        volumeParameter.overrideState = state;
        System.Type type = volumeParameter.GetType();
        if (VolumeParameter.IsObjectParameter(type))
        {
          ReadOnlyCollection<VolumeParameter> enumerable1 = (ReadOnlyCollection<VolumeParameter>) type.GetProperty("parameters", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) volumeParameter, (object[]) null);
          if (enumerable1 != null)
            this.SetOverridesTo((IEnumerable<VolumeParameter>) enumerable1, state);
        }
      }
    }

    public void SetAllOverridesTo(bool state)
    {
      this.SetOverridesTo((IEnumerable<VolumeParameter>) this.parameters, state);
    }

    public virtual void Override(OverrideablePropertiesComponent state, float interpFactor = 1f)
    {
      int count = this.parameters.Count;
      for (int index = 0; index < count; ++index)
      {
        VolumeParameter parameter1 = state.parameters[index];
        VolumeParameter parameter2 = this.parameters[index];
        if (parameter2.overrideState)
        {
          parameter1.overrideState = parameter2.overrideState;
          parameter1.Interp(parameter1, parameter2, interpFactor);
        }
      }
    }

    public virtual void Override(
      OverrideablePropertiesComponent previous,
      OverrideablePropertiesComponent to,
      float interpFactor = 1f)
    {
      int count = this.parameters.Count;
      this.m_InterpolationMode = to.m_InterpolationMode;
      this.m_InterpolationTime = to.m_InterpolationTime;
      for (int index = 0; index < count; ++index)
      {
        VolumeParameter parameter1 = previous.parameters[index];
        VolumeParameter parameter2 = to.parameters[index];
        if (parameter2.overrideState)
        {
          this.parameters[index].overrideState = parameter2.overrideState;
          this.parameters[index].Interp(parameter1, parameter2, interpFactor);
        }
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public enum InterpolationMode
    {
      RealTime,
      Cloudiness,
      Precipitation,
      RenderingTime,
      Aurora,
    }
  }
}
