// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.NamedWidget
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.UI.Localization;
using System;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class NamedWidget : Widget, INamed
  {
    private LocalizedString m_displayName;
    private LocalizedString m_description;

    [CanBeNull]
    public Func<LocalizedString> displayNameAction { get; set; }

    [CanBeNull]
    public Func<LocalizedString> descriptionAction { get; set; }

    public LocalizedString displayName
    {
      get => this.m_displayName;
      set
      {
        this.displayNameAction = (Func<LocalizedString>) null;
        this.m_displayName = value;
      }
    }

    public LocalizedString description
    {
      get => this.m_description;
      set
      {
        this.descriptionAction = (Func<LocalizedString>) null;
        this.m_description = value;
      }
    }

    protected override WidgetChanges Update()
    {
      return base.Update() | this.UpdateNameAndDescription(false);
    }

    public WidgetChanges UpdateNameAndDescription(bool setChanged = true)
    {
      WidgetChanges widgetChanges = WidgetChanges.None;
      if (this.displayNameAction != null)
      {
        LocalizedString localizedString = this.displayNameAction();
        if (!localizedString.Equals(this.m_displayName))
        {
          this.m_displayName = localizedString;
          widgetChanges |= WidgetChanges.Properties;
          if (setChanged)
            this.SetPropertiesChanged();
        }
      }
      if (this.descriptionAction != null)
      {
        LocalizedString localizedString = this.descriptionAction();
        if (!localizedString.Equals(this.m_description))
        {
          this.m_description = localizedString;
          widgetChanges |= WidgetChanges.Properties;
          if (setChanged)
            this.SetPropertiesChanged();
        }
      }
      return widgetChanges;
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("displayName");
      writer.Write<LocalizedString>(this.displayName);
      writer.PropertyName("description");
      writer.Write<LocalizedString>(this.description);
    }
  }
}
