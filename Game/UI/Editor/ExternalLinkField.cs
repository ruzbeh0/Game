// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ExternalLinkField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.UI.Editor
{
  public class ExternalLinkField : Widget
  {
    private static readonly IModsUploadSupport.ExternalLinkData kDefaultLink = new IModsUploadSupport.ExternalLinkData()
    {
      m_Type = IModsUploadSupport.ExternalLinkInfo.kAcceptedTypes[0].m_Type,
      m_URL = string.Empty
    };
    private static readonly string[] kAcceptedTypes = ((IEnumerable<IModsUploadSupport.ExternalLinkInfo>) IModsUploadSupport.ExternalLinkInfo.kAcceptedTypes).Select<IModsUploadSupport.ExternalLinkInfo, string>((Func<IModsUploadSupport.ExternalLinkInfo, string>) (info => info.m_Type)).ToArray<string>();

    public List<IModsUploadSupport.ExternalLinkData> links { get; set; } = new List<IModsUploadSupport.ExternalLinkData>();

    public int maxLinks { get; set; } = 5;

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("links");
      writer.ArrayBegin(this.links.Count);
      foreach (IModsUploadSupport.ExternalLinkData link in this.links)
        this.WriteExternalLink(writer, link);
      writer.ArrayEnd();
      writer.PropertyName("acceptedTypes");
      writer.Write(ExternalLinkField.kAcceptedTypes);
      writer.PropertyName("maxLinks");
      writer.Write(this.maxLinks);
    }

    private void WriteExternalLink(IJsonWriter writer, IModsUploadSupport.ExternalLinkData link)
    {
      writer.TypeBegin("ExternalLinkData");
      writer.PropertyName("type");
      writer.Write(link.m_Type);
      writer.PropertyName("url");
      writer.Write(link.m_URL);
      writer.PropertyName("error");
      writer.Write(!AssetUploadUtils.ValidateExternalLink(link));
      writer.PropertyName("lockType");
      writer.Write(AssetUploadUtils.LockLinkType(link.m_URL, out string _));
      writer.TypeEnd();
    }

    private void SetValue(int index, string type, string url)
    {
      IModsUploadSupport.ExternalLinkData externalLinkData = new IModsUploadSupport.ExternalLinkData()
      {
        m_Type = type,
        m_URL = url
      };
      string type1;
      if (AssetUploadUtils.LockLinkType(externalLinkData.m_URL, out type1))
        externalLinkData.m_Type = type1;
      this.links[index] = externalLinkData;
      this.SetPropertiesChanged();
    }

    private void Remove(int index)
    {
      this.links.RemoveAt(index);
      this.SetPropertiesChanged();
    }

    private void Add()
    {
      this.links.Add(ExternalLinkField.kDefaultLink);
      this.SetPropertiesChanged();
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget, int, string, string>(group, "setExternalLink", (Action<IWidget, int, string, string>) ((widget, index, type, url) =>
        {
          if (!(widget is ExternalLinkField externalLinkField2))
            return;
          externalLinkField2.SetValue(index, type, url);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget, int>(group, "removeExternalLink", (Action<IWidget, int>) ((widget, index) =>
        {
          if (!(widget is ExternalLinkField externalLinkField4))
            return;
          externalLinkField4.Remove(index);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget>(group, "addExternalLink", (Action<IWidget>) (widget =>
        {
          if (!(widget is ExternalLinkField externalLinkField6))
            return;
          externalLinkField6.Add();
          onValueChanged(widget);
        }), pathResolver);
      }
    }
  }
}
