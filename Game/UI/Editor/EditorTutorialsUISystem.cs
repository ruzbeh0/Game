// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorTutorialsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Tutorials;
using Game.UI.InGame;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class EditorTutorialsUISystem : TutorialsUISystem
  {
    private const string kEditorGroup = "editorTutorials";
    private EntityQuery m_TutorialCategoryQuery;
    private bool m_EditorTutorialsDisabled;

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem = (ITutorialSystem) this.World.GetOrCreateSystemManaged<EditorTutorialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialCategoryQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIEditorTutorialGroupData>(), ComponentType.ReadOnly<UIObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>(), ComponentType.ReadOnly<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_EditorTutorialsDisabled = true;
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("editorTutorials", "tutorialsDisabled", (Func<bool>) (() => this.m_EditorTutorialsDisabled)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("editorTutorials", "tutorialsEnabled", (Func<bool>) (() => this.m_TutorialSystem.tutorialEnabled)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("editorTutorials", "introActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.Intro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("editorTutorials", "listIntroActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.ListIntro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("editorTutorials", "listOutroActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.ListOutro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Entity>("editorTutorials", "next", (Func<Entity>) (() => this.m_TutorialSystem.nextListTutorial)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Entity>("editorTutorials", "advisorPanelVisible", (Func<Entity>) (() => this.m_TutorialSystem.nextListTutorial)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TutorialCategoriesBinding = new RawValueBinding("editorTutorials", "categories", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated method
        NativeList<UIObjectInfo> sortedCategories = this.GetSortedCategories(Allocator.Temp);
        writer.ArrayBegin(sortedCategories.Length);
        for (int index = 0; index < sortedCategories.Length; ++index)
        {
          UIObjectInfo uiObjectInfo = sortedCategories[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          UITutorialGroupPrefab prefab = this.m_PrefabSystem.GetPrefab<UITutorialGroupPrefab>(uiObjectInfo.entity);
          writer.TypeBegin(TypeNames.kAdvisorCategory);
          writer.PropertyName("entity");
          writer.Write(uiObjectInfo.entity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("shown");
          writer.Write(this.EntityManager.HasComponent<TutorialShown>(uiObjectInfo.entity));
          writer.PropertyName("locked");
          writer.Write(false);
          writer.PropertyName("children");
          // ISSUE: reference to a compiler-generated method
          this.BindTutorialGroup(writer, uiObjectInfo.entity);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedCategories.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ActiveTutorialBinding = new RawValueBinding("editorTutorials", "activeTutorial", (Action<IJsonWriter>) (writer => this.BindTutorial(writer, this.m_TutorialSystem.activeTutorial)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ActiveTutorialPhaseBinding = new RawValueBinding("editorTutorials", "activeTutorialPhase", (Action<IJsonWriter>) (writer => this.BindTutorialPhase(writer, this.m_TutorialSystem.activeTutorialPhase)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ActiveTutorialListBinding = new RawValueBinding("editorTutorials", "activeList", new Action<IJsonWriter>(((TutorialsUISystem) this).BindActiveTutorialList))));
      this.AddBinding((IBinding) new TriggerBinding<bool>("editorTutorials", "completeListIntro", (Action<bool>) (value =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.mode = TutorialMode.Default;
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.tutorialEnabled = value;
      })));
      this.AddBinding((IBinding) new TriggerBinding("editorTutorials", "toggleTutorials", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.tutorialEnabled = !this.m_TutorialSystem.tutorialEnabled;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_TutorialSystem.tutorialEnabled)
          return;
        // ISSUE: reference to a compiler-generated method
        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EditorTutorialSystem>().OnResetTutorials();
      })));
    }

    private NativeList<UIObjectInfo> GetSortedCategories(Allocator allocator)
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_TutorialCategoryQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<UIObjectData> componentDataArray = this.m_TutorialCategoryQuery.ToComponentDataArray<UIObjectData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<UIObjectInfo> list = new NativeList<UIObjectInfo>((AllocatorManager.AllocatorHandle) allocator);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        if (componentDataArray[index].m_Group == Entity.Null)
          list.Add(new UIObjectInfo(entityArray[index], componentDataArray[index].m_Priority));
      }
      list.Sort<UIObjectInfo>();
      entityArray.Dispose();
      componentDataArray.Dispose();
      return list;
    }

    private void BindCategories(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      NativeList<UIObjectInfo> sortedCategories = this.GetSortedCategories(Allocator.Temp);
      writer.ArrayBegin(sortedCategories.Length);
      for (int index = 0; index < sortedCategories.Length; ++index)
      {
        UIObjectInfo uiObjectInfo = sortedCategories[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        UITutorialGroupPrefab prefab = this.m_PrefabSystem.GetPrefab<UITutorialGroupPrefab>(uiObjectInfo.entity);
        writer.TypeBegin(TypeNames.kAdvisorCategory);
        writer.PropertyName("entity");
        writer.Write(uiObjectInfo.entity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("shown");
        writer.Write(this.EntityManager.HasComponent<TutorialShown>(uiObjectInfo.entity));
        writer.PropertyName("locked");
        writer.Write(false);
        writer.PropertyName("children");
        // ISSUE: reference to a compiler-generated method
        this.BindTutorialGroup(writer, uiObjectInfo.entity);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      sortedCategories.Dispose();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (GameManager.instance.gameMode.IsGame())
        return;
      // ISSUE: reference to a compiler-generated method
      base.OnUpdate();
    }

    private void CompleteEditorIntro(bool value)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.mode = TutorialMode.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.tutorialEnabled = value;
    }

    protected override void CompleteActiveTutorialPhase()
    {
      if (!GameManager.instance.gameMode.IsEditor())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.CompleteCurrentTutorialPhase();
    }

    private void ToggleTutorials()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.tutorialEnabled = !this.m_TutorialSystem.tutorialEnabled;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TutorialSystem.tutorialEnabled)
        return;
      // ISSUE: reference to a compiler-generated method
      World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EditorTutorialSystem>().OnResetTutorials();
    }

    [Preserve]
    public EditorTutorialsUISystem()
    {
    }
  }
}
