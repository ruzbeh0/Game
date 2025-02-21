// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.InputBindingField
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Input;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.UI.Menu
{
  public class InputBindingField : Field<ProxyBinding>, IWarning
  {
    public bool warning
    {
      get
      {
        return (this.m_Value.hasConflicts & (this.m_Value.isBuiltIn ? ProxyBinding.ConflictType.WithBuiltIn : ProxyBinding.ConflictType.All)) != 0;
      }
      set => throw new NotSupportedException("warning cannot be set to InputBindingField");
    }

    public InputBindingField()
    {
      this.valueWriter = (IWriter<ProxyBinding>) new ValueWriter<ProxyBinding>();
    }

    protected override bool ValueEquals(ProxyBinding newValue, ProxyBinding oldValue)
    {
      return ProxyBinding.pathAndModifiersComparer.Equals(newValue, oldValue);
    }

    protected override void WriteProperties(IJsonWriter writer)
    {
      base.WriteProperties(writer);
      writer.PropertyName("conflicts");
      writer.Write<ProxyBinding>(this.m_Value.conflicts);
    }

    public class Bindings : IWidgetBindingFactory
    {
      public IEnumerable<IBinding> CreateBindings(
        string group,
        IReader<IWidget> pathResolver,
        ValueChangedCallback onValueChanged)
      {
        yield return (IBinding) new TriggerBinding<IWidget>(group, "rebindInput", (Action<IWidget>) (widget =>
        {
          InputBindingField bindingField = widget as InputBindingField;
          if (bindingField == null)
            return;
          World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<InputRebindingUISystem>().Start(bindingField.m_Value, (Action<ProxyBinding>) (value => bindingField.SetValue(value)));
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget>(group, "unsetInputBinding", (Action<IWidget>) (widget =>
        {
          if (!(widget is InputBindingField inputBindingField2))
            return;
          ProxyBinding proxyBinding = inputBindingField2.m_Value.Copy() with
          {
            path = string.Empty,
            modifiers = (IReadOnlyList<ProxyModifier>) Array.Empty<ProxyModifier>()
          };
          inputBindingField2.SetValue(proxyBinding);
          onValueChanged(widget);
        }), pathResolver);
        yield return (IBinding) new TriggerBinding<IWidget>(group, "resetInputBinding", (Action<IWidget>) (widget =>
        {
          InputBindingField bindingField = widget as InputBindingField;
          if (bindingField == null)
            return;
          ProxyBinding newBinding = bindingField.m_Value.original.Copy();
          newBinding.ResetConflictCache();
          World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<InputRebindingUISystem>().Start(bindingField.m_Value, newBinding, (Action<ProxyBinding>) (value => bindingField.SetValue(value)));
        }), pathResolver);
      }
    }
  }
}
