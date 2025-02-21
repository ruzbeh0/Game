// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.SeasonsField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Simulation;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.UI.Editor
{
  public class SeasonsField : Widget
  {
    private Entity m_SelectedSeason;
    private List<ClimateSystem.SeasonInfo> m_Seasons = new List<ClimateSystem.SeasonInfo>();
    private SeasonsField.SeasonCurves m_SeasonCurves;

    public SeasonsField.IAdapter adapter { get; set; }

    protected override WidgetChanges Update()
    {
      int num = (int) base.Update();
      this.m_Seasons = this.adapter.seasons.ToList<ClimateSystem.SeasonInfo>();
      this.m_SeasonCurves = this.adapter.curves;
      return (WidgetChanges) (num | 2);
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("seasons");
      writer.Write<ClimateSystem.SeasonInfo>((IList<ClimateSystem.SeasonInfo>) this.m_Seasons);
      writer.PropertyName("curves");
      writer.Write<SeasonsField.SeasonCurves>(this.m_SeasonCurves);
    }

    public struct SeasonCurves : IJsonWritable, IJsonReadable
    {
      public AnimationCurve m_Temperature;
      public AnimationCurve m_Precipitation;
      public AnimationCurve m_Cloudiness;
      public AnimationCurve m_Aurora;
      public AnimationCurve m_Fog;

      public void Write(IJsonWriter writer)
      {
        if (this.m_Temperature == null)
          return;
        writer.TypeBegin("SeasonsCurves");
        writer.PropertyName("temperature");
        writer.Write(this.m_Temperature);
        writer.PropertyName("precipitation");
        writer.Write(this.m_Precipitation);
        writer.PropertyName("cloudiness");
        writer.Write(this.m_Cloudiness);
        writer.PropertyName("aurora");
        writer.Write(this.m_Aurora);
        writer.PropertyName("fog");
        writer.Write(this.m_Fog);
        writer.TypeEnd();
      }

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("temperature");
        reader.Read(out this.m_Temperature);
        reader.ReadProperty("precipitation");
        reader.Read(out this.m_Precipitation);
        reader.ReadProperty("cloudiness");
        reader.Read(out this.m_Cloudiness);
        reader.ReadProperty("aurora");
        reader.Read(out this.m_Aurora);
        reader.ReadProperty("fog");
        reader.Read(out this.m_Fog);
        reader.ReadMapEnd();
      }
    }

    public struct Season : IJsonWritable, IEquatable<SeasonsField.Season>
    {
      public Entity entity;
      public string m_Name;
      public float m_StartTime;
      public float m_TempNightDay;
      public float m_TempDeviationNightDay;
      public float m_CloudChance;
      public float m_CloudAmount;
      public float m_CloudAmountDeviation;
      public float m_PrecipitationChance;
      public float m_PrecipitationAmount;
      public float m_PrecipitationAmountDeviation;
      public float m_Turbulence;
      public float m_AuroraAmount;
      public float m_AuroraChance;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("name");
        writer.Write(this.m_Name);
        writer.PropertyName("startTime");
        writer.Write(this.m_StartTime);
        writer.PropertyName("tempNightDay");
        writer.Write(this.m_TempNightDay);
        writer.PropertyName("tempDeviationNightDay");
        writer.Write(this.m_TempDeviationNightDay);
        writer.PropertyName("cloudChance");
        writer.Write(this.m_CloudChance);
        writer.PropertyName("cloudAmount");
        writer.Write(this.m_CloudAmount);
        writer.PropertyName("cloudAmountDeviation");
        writer.Write(this.m_CloudAmountDeviation);
        writer.PropertyName("cloudAmountDeviation");
        writer.Write(this.m_PrecipitationChance);
        writer.PropertyName("precipitationAmount");
        writer.Write(this.m_PrecipitationAmount);
        writer.PropertyName("precipitationAmountDeviation");
        writer.Write(this.m_PrecipitationAmountDeviation);
        writer.PropertyName("turbulence");
        writer.Write(this.m_Turbulence);
        writer.PropertyName("auroraAmount");
        writer.Write(this.m_AuroraAmount);
        writer.PropertyName("auroraChance");
        writer.Write(this.m_AuroraChance);
        writer.TypeEnd();
      }

      public bool Equals(SeasonsField.Season other) => this.m_Name.Equals(other.m_Name);

      public override bool Equals(object obj)
      {
        return obj is SeasonsField.Season other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return this.m_Name.GetHashCode() * 397 ^ this.m_Name.GetHashCode();
      }

      public static bool operator ==(SeasonsField.Season left, SeasonsField.Season right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(SeasonsField.Season left, SeasonsField.Season right)
      {
        return !left.Equals(right);
      }
    }

    public interface IAdapter
    {
      Entity selectedSeason { get; set; }

      IEnumerable<ClimateSystem.SeasonInfo> seasons { get; set; }

      SeasonsField.SeasonCurves curves { get; set; }

      void RebuildCurves();
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new CallBinding<IWidget, ClimateSystem.SeasonInfo, int>(group, "onUpdateSeason", (Func<IWidget, ClimateSystem.SeasonInfo, int>) ((widget, season) =>
        {
          int bindings = -1;
          if (widget is SeasonsField seasonsField2)
          {
            for (int index = 0; index < seasonsField2.m_Seasons.Count; ++index)
            {
              if (seasonsField2.m_Seasons[index].m_NameID == season.m_NameID)
              {
                seasonsField2.m_Seasons[index] = season;
                bindings = index;
              }
            }
            seasonsField2.adapter.seasons = (IEnumerable<ClimateSystem.SeasonInfo>) seasonsField2.m_Seasons;
            seasonsField2.adapter.RebuildCurves();
            int num = (int) widget.Update();
          }
          return bindings;
        }), pathResolver, (IReader<ClimateSystem.SeasonInfo>) new ValueReader<ClimateSystem.SeasonInfo>());
      }
    }
  }
}
