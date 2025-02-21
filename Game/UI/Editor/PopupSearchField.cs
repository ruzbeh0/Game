// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.PopupSearchField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class PopupSearchField : Widget, ISettable, IWidget, IJsonWritable
  {
    private string m_Value;
    private bool m_ValueIsFavorite;
    private List<PopupSearchField.Suggestion> m_Suggestions = new List<PopupSearchField.Suggestion>();

    public PopupSearchField.IAdapter adapter { get; set; }

    public bool hasFavorites { get; set; }

    public bool shouldTriggerValueChangedEvent => true;

    public void SetValue(IJsonReader reader)
    {
      string str;
      reader.Read(out str);
      this.SetValue(str);
    }

    public void SetValue(string value)
    {
      if (!(value != this.m_Value))
        return;
      this.adapter.searchQuery = value;
    }

    protected override WidgetChanges Update()
    {
      WidgetChanges widgetChanges = base.Update();
      if (this.adapter.searchQuery != this.m_Value)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Value = this.adapter.searchQuery;
      }
      if (this.adapter.searchQueryIsFavorite != this.m_ValueIsFavorite)
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_ValueIsFavorite = this.adapter.searchQueryIsFavorite;
      }
      if (!this.adapter.searchSuggestions.SequenceEqual<PopupSearchField.Suggestion>((IEnumerable<PopupSearchField.Suggestion>) this.m_Suggestions))
      {
        widgetChanges |= WidgetChanges.Properties;
        this.m_Suggestions.Clear();
        this.m_Suggestions.AddRange(this.adapter.searchSuggestions);
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("hasFavorites");
      writer.Write(this.hasFavorites);
      writer.PropertyName("value");
      writer.Write(this.m_Value ?? string.Empty);
      writer.PropertyName("valueIsFavorite");
      writer.Write(this.m_ValueIsFavorite);
      writer.PropertyName("suggestions");
      writer.Write<PopupSearchField.Suggestion>((IList<PopupSearchField.Suggestion>) this.m_Suggestions);
    }

    public interface IAdapter : SearchField.IAdapter
    {
      bool searchQueryIsFavorite { get; }

      IEnumerable<PopupSearchField.Suggestion> searchSuggestions { get; }

      void SetFavorite(string query, bool favorite);
    }

    public struct Suggestion : 
      IComparable<PopupSearchField.Suggestion>,
      IEquatable<PopupSearchField.Suggestion>,
      IJsonWritable
    {
      public string value { get; set; }

      public bool favorite { get; set; }

      public Suggestion(string value, bool favorite)
      {
        this.value = value;
        this.favorite = favorite;
      }

      public static PopupSearchField.Suggestion NonFavorite(string value)
      {
        return new PopupSearchField.Suggestion(value, false);
      }

      public static PopupSearchField.Suggestion Favorite(string value)
      {
        return new PopupSearchField.Suggestion(value, true);
      }

      public int CompareTo(PopupSearchField.Suggestion other)
      {
        return this.favorite == other.favorite ? string.CompareOrdinal(this.value, other.value) : this.favorite.CompareTo(other.favorite);
      }

      public bool Equals(PopupSearchField.Suggestion other)
      {
        return this.value == other.value && this.favorite == other.favorite;
      }

      public override bool Equals(object obj)
      {
        return obj is PopupSearchField.Suggestion other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return this.value.GetHashCode() * 397 ^ this.favorite.GetHashCode();
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("value");
        writer.Write(this.value);
        writer.PropertyName("favorite");
        writer.Write(this.favorite);
        writer.TypeEnd();
      }
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, string, bool>(group, "setSearchFavorite", (Action<IWidget, string, bool>) ((widget, query, favorite) =>
        {
          if (!(widget is PopupSearchField popupSearchField2))
            return;
          popupSearchField2.adapter.SetFavorite(query, favorite);
        }), pathResolver);
      }
    }
  }
}
