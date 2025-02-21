// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.SeasonWheel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class SeasonWheel : Widget
  {
    private Entity m_SelectedSeason;
    private List<SeasonWheel.Season> m_Seasons = new List<SeasonWheel.Season>();

    public SeasonWheel.IAdapter adapter { get; set; }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.adapter.selectedSeason != this.m_SelectedSeason)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_SelectedSeason = this.adapter.selectedSeason;
      }
      if (!this.m_Seasons.SequenceEqual<SeasonWheel.Season>(this.adapter.seasons))
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Seasons.Clear();
        this.m_Seasons.AddRange(this.adapter.seasons);
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("selectedSeason");
      writer.Write(this.m_SelectedSeason);
      writer.PropertyName("seasons");
      writer.Write<SeasonWheel.Season>((IList<SeasonWheel.Season>) this.m_Seasons);
    }

    public struct Season : IJsonWritable, IEquatable<SeasonWheel.Season>
    {
      public Entity entity;
      public Bounds1 startTimeOfYear;
      public float temperature;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("entity");
        writer.Write(this.entity);
        writer.PropertyName("startTimeOfYear");
        writer.Write(this.startTimeOfYear);
        writer.PropertyName("temperature");
        writer.Write(this.temperature);
        writer.TypeEnd();
      }

      public bool Equals(SeasonWheel.Season other)
      {
        return this.startTimeOfYear.Equals(other.startTimeOfYear) && this.temperature.Equals(other.temperature);
      }

      public override bool Equals(object obj)
      {
        return obj is SeasonWheel.Season other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return this.startTimeOfYear.GetHashCode() * 397 ^ this.temperature.GetHashCode();
      }

      public static bool operator ==(SeasonWheel.Season left, SeasonWheel.Season right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(SeasonWheel.Season left, SeasonWheel.Season right)
      {
        return !left.Equals(right);
      }
    }

    public interface IAdapter
    {
      Entity selectedSeason { get; set; }

      IEnumerable<SeasonWheel.Season> seasons { get; }

      void SetStartTimeOfYear(Entity season, Bounds1 startTimeOfYear);
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, Entity>(group, "setSelectedSeason", (Action<IWidget, Entity>) ((widget, season) =>
        {
          if (!(widget is SeasonWheel seasonWheel2))
            return;
          seasonWheel2.adapter.selectedSeason = season;
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, Entity, Bounds1>(group, "setSeasonStartTimeOfYear", (Action<IWidget, Entity, Bounds1>) ((widget, season, startTimeOfYear) =>
        {
          if (!(widget is SeasonWheel seasonWheel4))
            return;
          seasonWheel4.adapter.SetStartTimeOfYear(season, startTimeOfYear);
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
