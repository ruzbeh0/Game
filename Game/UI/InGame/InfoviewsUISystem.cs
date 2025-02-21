// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.InfoviewsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class InfoviewsUISystem : UISystemBase
  {
    private const string kGroup = "infoviews";
    private ToolSystem m_ToolSystem;
    private PrefabSystem m_PrefabSystem;
    private UnlockSystem m_UnlockSystem;
    private PrefabUISystem m_PrefabUISystem;
    private InfoviewInitializeSystem m_InfoviewInitializeSystem;
    private RawValueBinding m_ActiveView;
    private List<InfoviewsUISystem.Infoview> m_InfoviewsCache;
    private RawValueBinding m_Infoviews;
    private EntityQuery m_UnlockedInfoviewQuery;
    private bool m_InfoviewChanged;

    public override GameMode gameMode => GameMode.GameOrEditor;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedInfoviewQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockSystem = this.World.GetOrCreateSystemManaged<UnlockSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewInitializeSystem = this.World.GetOrCreateSystemManaged<InfoviewInitializeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewsCache = new List<InfoviewsUISystem.Infoview>();
      this.AddBinding((IBinding) new TriggerBinding<Entity>("infoviews", "setActiveInfoview", (Action<Entity>) (entity =>
      {
        InfoviewPrefab infoviewPrefab = (InfoviewPrefab) null;
        if (this.EntityManager.Exists(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          infoviewPrefab = this.m_PrefabSystem.GetPrefab<InfoviewPrefab>(entity);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.infoview = infoviewPrefab;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool, int>("infoviews", "setInfomodeActive", (Action<Entity, bool, int>) ((entity, active, priority) => this.m_ToolSystem.SetInfomodeActive(entity, active, priority))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Infoviews = new RawValueBinding("infoviews", "infoviews", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InfoviewsCache.Clear();
        // ISSUE: reference to a compiler-generated field
        foreach (InfoviewPrefab infoview in this.m_InfoviewInitializeSystem.infoviews)
        {
          if (infoview.isValid)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            bool locked = this.m_UnlockSystem.IsLocked((PrefabBase) infoview);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            // ISSUE: object of a compiler-generated type is created
            this.m_InfoviewsCache.Add(new InfoviewsUISystem.Infoview(this.m_PrefabSystem.GetEntity((PrefabBase) infoview), infoview, locked));
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_InfoviewsCache.Sort();
        // ISSUE: reference to a compiler-generated field
        writer.ArrayBegin(this.m_InfoviewsCache.Count);
        // ISSUE: reference to a compiler-generated field
        foreach (InfoviewsUISystem.Infoview infoview in this.m_InfoviewsCache)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          infoview.Write(this.m_PrefabUISystem, writer);
        }
        writer.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveView = new RawValueBinding("infoviews", "activeInfoview", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        InfoviewPrefab activeInfoview = this.m_ToolSystem.activeInfoview;
        if ((UnityEngine.Object) activeInfoview != (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) activeInfoview);
          writer.TypeBegin("infoviews.ActiveInfoview");
          writer.PropertyName("entity");
          writer.Write(entity);
          writer.PropertyName("id");
          writer.Write(activeInfoview.name);
          writer.PropertyName("icon");
          writer.Write(activeInfoview.m_IconPath);
          writer.PropertyName("uiTag");
          writer.Write(activeInfoview.uiTag);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          List<InfomodeInfo> infoviewInfomodes = this.m_ToolSystem.GetInfoviewInfomodes();
          writer.PropertyName("infomodes");
          writer.ArrayBegin(infoviewInfomodes.Count);
          for (int index = 0; index < infoviewInfomodes.Count; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.BindInfomode(writer, infoviewInfomodes[index]);
          }
          writer.ArrayEnd();
          writer.PropertyName("editor");
          writer.Write(activeInfoview.m_Editor);
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventInfoviewChanged += (Action<InfoviewPrefab>) (prefab => this.m_InfoviewChanged = true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventInfomodesChanged += (Action) (() => this.m_InfoviewChanged = true);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventInfoviewChanged -= (Action<InfoviewPrefab>) (prefab => this.m_InfoviewChanged = true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventInfomodesChanged -= (Action) (() => this.m_InfoviewChanged = true);
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (PrefabUtils.HasUnlockedPrefab<InfoviewData>(this.EntityManager, this.m_UnlockedInfoviewQuery))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Infoviews.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InfoviewChanged)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveView.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewChanged = false;
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Infoviews.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveView.Update();
    }

    private void OnInfoviewChanged(InfoviewPrefab prefab) => this.m_InfoviewChanged = true;

    private void OnChanged() => this.m_InfoviewChanged = true;

    private void BindInfoviews(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewsCache.Clear();
      // ISSUE: reference to a compiler-generated field
      foreach (InfoviewPrefab infoview in this.m_InfoviewInitializeSystem.infoviews)
      {
        if (infoview.isValid)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          bool locked = this.m_UnlockSystem.IsLocked((PrefabBase) infoview);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          this.m_InfoviewsCache.Add(new InfoviewsUISystem.Infoview(this.m_PrefabSystem.GetEntity((PrefabBase) infoview), infoview, locked));
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewsCache.Sort();
      // ISSUE: reference to a compiler-generated field
      writer.ArrayBegin(this.m_InfoviewsCache.Count);
      // ISSUE: reference to a compiler-generated field
      foreach (InfoviewsUISystem.Infoview infoview in this.m_InfoviewsCache)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        infoview.Write(this.m_PrefabUISystem, writer);
      }
      writer.ArrayEnd();
    }

    private void BindActiveInfoview(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      InfoviewPrefab activeInfoview = this.m_ToolSystem.activeInfoview;
      if ((UnityEngine.Object) activeInfoview != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) activeInfoview);
        writer.TypeBegin("infoviews.ActiveInfoview");
        writer.PropertyName("entity");
        writer.Write(entity);
        writer.PropertyName("id");
        writer.Write(activeInfoview.name);
        writer.PropertyName("icon");
        writer.Write(activeInfoview.m_IconPath);
        writer.PropertyName("uiTag");
        writer.Write(activeInfoview.uiTag);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        List<InfomodeInfo> infoviewInfomodes = this.m_ToolSystem.GetInfoviewInfomodes();
        writer.PropertyName("infomodes");
        writer.ArrayBegin(infoviewInfomodes.Count);
        for (int index = 0; index < infoviewInfomodes.Count; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.BindInfomode(writer, infoviewInfomodes[index]);
        }
        writer.ArrayEnd();
        writer.PropertyName("editor");
        writer.Write(activeInfoview.m_Editor);
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    private void BindInfomode(IJsonWriter writer, InfomodeInfo info)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) info.m_Mode);
      IColorInfomode mode1 = info.m_Mode as IColorInfomode;
      IGradientInfomode mode2 = info.m_Mode as IGradientInfomode;
      writer.TypeBegin("infoviews.Infomode");
      writer.PropertyName("entity");
      writer.Write(entity);
      writer.PropertyName("id");
      writer.Write(info.m_Mode.name);
      writer.PropertyName("uiTag");
      writer.Write(info.m_Mode.uiTag);
      writer.PropertyName("active");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      writer.Write(this.m_ToolSystem.IsInfomodeActive(info.m_Mode));
      writer.PropertyName("priority");
      writer.Write(info.m_Priority);
      writer.PropertyName("color");
      if (mode1 != null)
        writer.Write(mode1.color);
      else if (mode2 != null && mode2.legendType == GradientLegendType.Fields && !mode2.lowLabel.HasValue)
        writer.Write(mode2.lowColor);
      else
        writer.WriteNull();
      writer.PropertyName("gradientLegend");
      if (mode2 != null && mode2.legendType == GradientLegendType.Gradient)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindInfomodeGradientLegend(writer, mode2);
      }
      else
        writer.WriteNull();
      writer.PropertyName("colorLegends");
      if (mode2 != null && mode2.legendType == GradientLegendType.Fields)
      {
        // ISSUE: reference to a compiler-generated method
        this.BindColorLegends(writer, mode2);
      }
      else
        writer.WriteEmptyArray();
      writer.PropertyName("type");
      writer.Write(info.m_Mode.infomodeTypeLocaleKey);
      writer.TypeEnd();
    }

    private void BindInfomodeGradientLegend(IJsonWriter writer, IGradientInfomode gradientInfomode)
    {
      writer.TypeBegin("infoviews.InfomodeGradientLegend");
      writer.PropertyName("lowLabel");
      writer.Write<LocalizedString>(gradientInfomode.lowLabel);
      writer.PropertyName("highLabel");
      writer.Write<LocalizedString>(gradientInfomode.highLabel);
      writer.PropertyName("gradient");
      writer.TypeBegin("infoviews.Gradient");
      writer.PropertyName("stops");
      writer.ArrayBegin(3U);
      // ISSUE: reference to a compiler-generated method
      this.BindGradientStop(writer, 0.0f, gradientInfomode.lowColor);
      // ISSUE: reference to a compiler-generated method
      this.BindGradientStop(writer, 0.5f, gradientInfomode.mediumColor);
      // ISSUE: reference to a compiler-generated method
      this.BindGradientStop(writer, 1f, gradientInfomode.highColor);
      writer.ArrayEnd();
      writer.TypeEnd();
      writer.TypeEnd();
    }

    private void BindGradientStop(IJsonWriter writer, float offset, Color color)
    {
      writer.TypeBegin("infoviews.GradientStop");
      writer.PropertyName(nameof (offset));
      writer.Write(offset);
      writer.PropertyName(nameof (color));
      writer.Write(color);
      writer.TypeEnd();
    }

    private void BindColorLegends(IJsonWriter writer, IGradientInfomode gradientInfomode)
    {
      uint size = 0;
      if (gradientInfomode.lowLabel.HasValue)
        ++size;
      if (gradientInfomode.mediumLabel.HasValue)
        ++size;
      if (gradientInfomode.highLabel.HasValue)
        ++size;
      writer.ArrayBegin(size);
      LocalizedString? nullable;
      if (gradientInfomode.lowLabel.HasValue)
      {
        IJsonWriter writer1 = writer;
        Color lowColor = gradientInfomode.lowColor;
        nullable = gradientInfomode.lowLabel;
        LocalizedString label = nullable.Value;
        // ISSUE: reference to a compiler-generated method
        this.BindColorLegend(writer1, lowColor, label);
      }
      nullable = gradientInfomode.mediumLabel;
      if (nullable.HasValue)
      {
        IJsonWriter writer2 = writer;
        Color mediumColor = gradientInfomode.mediumColor;
        nullable = gradientInfomode.mediumLabel;
        LocalizedString label = nullable.Value;
        // ISSUE: reference to a compiler-generated method
        this.BindColorLegend(writer2, mediumColor, label);
      }
      nullable = gradientInfomode.highLabel;
      if (nullable.HasValue)
      {
        IJsonWriter writer3 = writer;
        Color highColor = gradientInfomode.highColor;
        nullable = gradientInfomode.highLabel;
        LocalizedString label = nullable.Value;
        // ISSUE: reference to a compiler-generated method
        this.BindColorLegend(writer3, highColor, label);
      }
      writer.ArrayEnd();
    }

    private void BindColorLegend(IJsonWriter writer, Color color, LocalizedString label)
    {
      writer.TypeBegin("infoviews.ColorLegend");
      writer.PropertyName(nameof (color));
      writer.Write(color);
      writer.PropertyName(nameof (label));
      writer.Write<LocalizedString>(label);
      writer.TypeEnd();
    }

    public void SetActiveInfoview(Entity entity)
    {
      InfoviewPrefab infoviewPrefab = (InfoviewPrefab) null;
      if (this.EntityManager.Exists(entity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        infoviewPrefab = this.m_PrefabSystem.GetPrefab<InfoviewPrefab>(entity);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.infoview = infoviewPrefab;
    }

    private void SetInfomodeActive(Entity entity, bool active, int priority)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.SetInfomodeActive(entity, active, priority);
    }

    [Preserve]
    public InfoviewsUISystem()
    {
    }

    public readonly struct Infoview : IComparable<InfoviewsUISystem.Infoview>
    {
      public Entity entity { get; }

      [NotNull]
      public string id { get; }

      [NotNull]
      public string icon { get; }

      public bool locked { get; }

      [NotNull]
      public string uiTag { get; }

      public int group { get; }

      private int priority { get; }

      private bool editor { get; }

      public Infoview(Entity entity, InfoviewPrefab prefab, bool locked)
      {
        this.entity = entity;
        this.id = prefab.name;
        this.icon = prefab.m_IconPath;
        this.locked = locked;
        this.uiTag = prefab.uiTag;
        this.priority = prefab.m_Priority;
        this.group = prefab.m_Group;
        this.editor = prefab.m_Editor;
      }

      public int CompareTo(InfoviewsUISystem.Infoview other)
      {
        int num1 = this.group - other.group;
        int num2 = num1 != 0 ? num1 : this.priority - other.priority;
        return num2 == 0 ? string.Compare(this.id, other.id, StringComparison.Ordinal) : num2;
      }

      public void Write(PrefabUISystem prefabUISystem, IJsonWriter writer)
      {
        writer.TypeBegin("infoviews.Infoview");
        writer.PropertyName("entity");
        writer.Write(this.entity);
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("locked");
        writer.Write(this.locked);
        writer.PropertyName("uiTag");
        writer.Write(this.uiTag);
        writer.PropertyName("group");
        writer.Write(this.group);
        writer.PropertyName("editor");
        writer.Write(this.editor);
        writer.PropertyName("requirements");
        // ISSUE: reference to a compiler-generated method
        prefabUISystem.BindPrefabRequirements(writer, this.entity);
        writer.TypeEnd();
      }
    }
  }
}
