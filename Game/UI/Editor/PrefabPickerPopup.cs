// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.PrefabPickerPopup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Reflection;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class PrefabPickerPopup : IValueFieldPopup<PrefabBase>
  {
    private ITypedValueAccessor<PrefabBase> m_Accessor;
    private Type m_PrefabType;
    private Func<PrefabBase, bool> m_Filter;
    private PrefabPickerAdapter m_Adapter;

    public bool nullable { get; set; }

    public IList<IWidget> children { get; }

    public PrefabPickerPopup(Type prefabType, Func<PrefabBase, bool> filter = null)
    {
      this.m_PrefabType = prefabType;
      this.m_Filter = filter;
      this.m_Adapter = new PrefabPickerAdapter();
      this.m_Adapter.EventPrefabSelected += new Action<PrefabBase>(this.OnPrefabSelected);
      this.children = (IList<IWidget>) new IWidget[3]
      {
        (IWidget) new PopupSearchField()
        {
          adapter = (PopupSearchField.IAdapter) this.m_Adapter,
          hasFavorites = true
        },
        (IWidget) new ItemPicker<PrefabItem>()
        {
          adapter = (ItemPicker<PrefabItem>.IAdapter) this.m_Adapter,
          hasFavorites = true,
          hasImages = true
        },
        (IWidget) new ItemPickerFooter()
        {
          adapter = (ItemPickerFooter.IAdapter) this.m_Adapter
        }
      };
      ContainerExtensions.SetDefaults<IWidget>(this.children);
    }

    public bool Update()
    {
      this.m_Adapter.selectedPrefab = this.m_Accessor.GetTypedValue();
      this.m_Adapter.Update();
      return false;
    }

    public void Attach(ITypedValueAccessor<PrefabBase> accessor)
    {
      this.m_Accessor = accessor;
      List<PrefabBase> prefabs = new List<PrefabBase>();
      if (this.nullable)
        prefabs.Add((PrefabBase) null);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<PrefabSystem>();
      if (existingSystemManaged != null)
      {
        foreach (PrefabBase prefab in existingSystemManaged.prefabs)
        {
          if (this.m_PrefabType.IsInstanceOfType((object) prefab) && (this.m_Filter == null || this.m_Filter(prefab)))
            prefabs.Add(prefab);
        }
      }
      this.m_Adapter.SetPrefabs((ICollection<PrefabBase>) prefabs);
      this.m_Adapter.LoadSettings();
    }

    public void Detach()
    {
      this.m_Adapter.searchQuery = string.Empty;
      this.m_Adapter.SetPrefabs((ICollection<PrefabBase>) Array.Empty<PrefabBase>());
    }

    public LocalizedString GetDisplayValue(PrefabBase value)
    {
      return EditorPrefabUtils.GetPrefabLabel(value);
    }

    private void OnPrefabSelected(PrefabBase prefab) => this.m_Accessor.SetTypedValue(prefab);
  }
}
