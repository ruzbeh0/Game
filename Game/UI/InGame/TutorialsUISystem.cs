// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TutorialsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Input;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Tutorials;
using Game.UI.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TutorialsUISystem : UISystemBase
  {
    private const string kGroup = "tutorials";
    protected PrefabSystem m_PrefabSystem;
    protected ITutorialSystem m_TutorialSystem;
    private ITutorialSystem m_EditorTutorialSystem;
    protected ITutorialUIActivationSystem m_ActivationSystem;
    protected ITutorialUIDeactivationSystem m_DeactivationSystem;
    protected ITutorialUITriggerSystem m_TriggerSystem;
    private EntityQuery m_TutorialConfigurationQuery;
    private EntityQuery m_TutorialCategoryQuery;
    protected EntityQuery m_UnlockQuery;
    protected RawValueBinding m_ActiveTutorialListBinding;
    protected RawValueBinding m_TutorialCategoriesBinding;
    private RawMapBinding<Entity> m_TutorialsBinding;
    protected RawValueBinding m_ActiveTutorialBinding;
    protected RawValueBinding m_ActiveTutorialPhaseBinding;
    private GetterValueBinding<Entity> m_TutorialPendingBinding;
    private int m_TutorialActiveVersion;
    private int m_PhaseActiveVersion;
    private int m_TriggerActiveVersion;
    private int m_TriggerCompletedVersion;
    private int m_TutorialShownVersion;
    private int m_PhaseShownVersion;
    private int m_PhaseCompletedVersion;
    private bool m_WasEnabled;
    private TutorialsUISystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.Enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem = (ITutorialSystem) this.World.GetOrCreateSystemManaged<TutorialSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ActivationSystem = (ITutorialUIActivationSystem) this.World.GetOrCreateSystemManaged<TutorialUIActivationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeactivationSystem = (ITutorialUIDeactivationSystem) this.World.GetOrCreateSystemManaged<TutorialUIDeactivationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = (ITutorialUITriggerSystem) this.World.GetOrCreateSystemManaged<TutorialUITriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialCategoryQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITutorialGroupData>(), ComponentType.Exclude<UIEditorTutorialGroupData>(), ComponentType.ReadOnly<UIObjectData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActiveTutorialListBinding = new RawValueBinding("tutorials", "activeList", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        Entity activeTutorialList = this.m_TutorialSystem.activeTutorialList;
        if (activeTutorialList != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TutorialListPrefab prefab = this.m_PrefabSystem.GetPrefab<TutorialListPrefab>(activeTutorialList);
          // ISSUE: reference to a compiler-generated method
          NativeList<Entity> visibleListTutorials = this.GetVisibleListTutorials(activeTutorialList, Allocator.Temp);
          // ISSUE: reference to a compiler-generated method
          NativeList<Entity> listHintTutorials = this.GetListHintTutorials(activeTutorialList, Allocator.Temp);
          try
          {
            writer.TypeBegin(TypeNames.kTutorialList);
            writer.PropertyName("entity");
            writer.Write(activeTutorialList);
            writer.PropertyName("name");
            writer.Write(prefab.name);
            writer.PropertyName("tutorials");
            writer.ArrayBegin(visibleListTutorials.Length);
            foreach (Entity tutorialEntity in visibleListTutorials)
            {
              // ISSUE: reference to a compiler-generated method
              this.BindTutorial(writer, tutorialEntity);
            }
            writer.ArrayEnd();
            writer.PropertyName("hints");
            writer.ArrayBegin(listHintTutorials.Length);
            foreach (Entity tutorialEntity in listHintTutorials)
            {
              // ISSUE: reference to a compiler-generated method
              this.BindTutorial(writer, tutorialEntity);
            }
            writer.ArrayEnd();
            writer.PropertyName("intro");
            // ISSUE: reference to a compiler-generated field
            writer.Write(this.m_TutorialSystem.showListReminder);
            writer.TypeEnd();
          }
          finally
          {
            visibleListTutorials.Dispose();
          }
        }
        else
          writer.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<Entity>("tutorials", "activateTutorial", (Action<Entity>) (tutorial => this.m_TutorialSystem.SetTutorial(tutorial, Entity.Null))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<Entity, Entity>("tutorials", "activateTutorialPhase", (Action<Entity, Entity>) ((tutorial, phase) => this.m_TutorialSystem.SetTutorial(tutorial, phase))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<Entity, Entity, bool>("tutorials", "forceTutorial", (Action<Entity, Entity, bool>) ((tutorial, phase, advisorActivation) => this.m_TutorialSystem.ForceTutorial(tutorial, phase, advisorActivation))));
      this.AddBinding((IBinding) new TriggerBinding("tutorials", "completeActiveTutorialPhase", (System.Action) (() =>
      {
        if (!GameManager.instance.gameMode.IsGame())
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.CompleteCurrentTutorialPhase();
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding("tutorials", "completeActiveTutorial", (System.Action) (() => this.m_TutorialSystem.CompleteTutorial(this.m_TutorialSystem.activeTutorial))));
      this.AddBinding((IBinding) new TriggerBinding<string, bool>("tutorials", "setTutorialTagActive", (Action<string, bool>) ((tag, active) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActivationSystem.SetTag(tag, active);
        if (active)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_DeactivationSystem.DeactivateTag(tag);
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding<string>("tutorials", "activateTutorialTrigger", (Action<string>) (trigger => this.m_TriggerSystem.ActivateTrigger(trigger))));
      if (this.GetType() == typeof (EditorTutorialsUISystem))
        return;
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding("tutorials", "completeListIntro", (System.Action) (() => this.m_TutorialSystem.mode = TutorialMode.Default)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) new TriggerBinding("tutorials", "completeListOutro", (System.Action) (() => this.m_TutorialSystem.mode = TutorialMode.Default)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("tutorials", "completeIntro", (Action<bool>) (tutorialEnabled =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.mode = TutorialMode.Default;
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialSystem.tutorialEnabled = tutorialEnabled;
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tutorials", "tutorialsEnabled", (Func<bool>) (() => this.m_TutorialSystem.tutorialEnabled)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tutorials", "introActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.Intro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tutorials", "listIntroActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.ListIntro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tutorials", "listOutroActive", (Func<bool>) (() => this.m_TutorialSystem.mode == TutorialMode.ListOutro)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Entity>("tutorials", "next", (Func<Entity>) (() => this.m_TutorialSystem.nextListTutorial)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TutorialCategoriesBinding = new RawValueBinding("tutorials", "categories", (Action<IJsonWriter>) (writer =>
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
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity));
          writer.PropertyName("children");
          // ISSUE: reference to a compiler-generated method
          this.BindTutorialGroup(writer, uiObjectInfo.entity);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedCategories.Dispose();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TutorialsBinding = new RawMapBinding<Entity>("tutorials", "tutorials", (Action<IJsonWriter, Entity>) ((writer, tutorialEntity) =>
      {
        if (this.EntityManager.HasComponent<TutorialData>(tutorialEntity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TutorialPrefab prefab = this.m_PrefabSystem.GetPrefab<TutorialPrefab>(tutorialEntity);
          writer.TypeBegin(TypeNames.kTutorial);
          writer.PropertyName("entity");
          writer.Write(tutorialEntity);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          // ISSUE: reference to a compiler-generated method
          writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(tutorialEntity));
          writer.PropertyName("priority");
          writer.Write(prefab.m_Priority);
          writer.PropertyName("active");
          writer.Write(this.EntityManager.HasComponent<TutorialActive>(tutorialEntity));
          writer.PropertyName("completed");
          // ISSUE: reference to a compiler-generated method
          writer.Write(this.EntityManager.HasComponent<TutorialCompleted>(tutorialEntity) || this.AlternativeCompleted(tutorialEntity));
          writer.PropertyName("shown");
          writer.Write(this.EntityManager.HasComponent<TutorialShown>(tutorialEntity));
          writer.PropertyName("mandatory");
          writer.Write(prefab.m_Mandatory);
          writer.PropertyName("advisorActivation");
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.EntityManager.HasComponent<AdvisorActivation>(this.m_TutorialSystem.activeTutorial));
          DynamicBuffer<TutorialPhaseRef> buffer1 = this.EntityManager.GetBuffer<TutorialPhaseRef>(tutorialEntity, true);
          writer.PropertyName("phases");
          int size = 0;
          for (int index = 0; index < buffer1.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (TutorialSystem.IsValidControlScheme(buffer1[index].m_Phase, this.m_PrefabSystem))
              ++size;
          }
          writer.ArrayBegin(size);
          for (int index = 0; index < buffer1.Length; ++index)
          {
            Entity phase = buffer1[index].m_Phase;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (TutorialSystem.IsValidControlScheme(phase, this.m_PrefabSystem))
            {
              // ISSUE: reference to a compiler-generated method
              this.BindTutorialPhase(writer, phase);
            }
          }
          writer.ArrayEnd();
          writer.PropertyName("filters");
          // ISSUE: reference to a compiler-generated method
          writer.Write(TutorialsUISystem.GetFilters(prefab));
          writer.PropertyName("alternatives");
          DynamicBuffer<Game.Tutorials.TutorialAlternative> buffer2;
          if (this.EntityManager.TryGetBuffer<Game.Tutorials.TutorialAlternative>(tutorialEntity, true, out buffer2))
          {
            writer.ArrayBegin(buffer2.Length);
            for (int index = 0; index < buffer2.Length; ++index)
              writer.Write(buffer2[index].m_Alternative);
            writer.ArrayEnd();
          }
          else
            writer.WriteNull();
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
      }))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_TutorialPendingBinding = new GetterValueBinding<Entity>("tutorials", "pending", (Func<Entity>) (() => this.m_TutorialSystem.tutorialPending))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ActiveTutorialBinding = new RawValueBinding("tutorials", "activeTutorial", (Action<IJsonWriter>) (writer => this.BindTutorial(writer, this.m_TutorialSystem.activeTutorial)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ActiveTutorialPhaseBinding = new RawValueBinding("tutorials", "activeTutorialPhase", (Action<IJsonWriter>) (writer => this.BindTutorialPhase(writer, this.m_TutorialSystem.activeTutorialPhase)))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WasEnabled = this.m_TutorialSystem.tutorialEnabled;
      // ISSUE: reference to a compiler-generated field
      GameManager.instance.inputManager.EventControlSchemeChanged += (Action<InputManager.ControlScheme>) (controlScheme => this.m_TutorialsBinding.UpdateAll());
    }

    private void OnControlSchemeChanged(InputManager.ControlScheme controlScheme)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialsBinding.UpdateAll();
    }

    private void CompleteIntro() => this.m_TutorialSystem.mode = TutorialMode.Default;

    private void CompleteOutro() => this.m_TutorialSystem.mode = TutorialMode.Default;

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      int componentOrderVersion1 = this.EntityManager.GetComponentOrderVersion<TutorialActive>();
      int componentOrderVersion2 = this.EntityManager.GetComponentOrderVersion<TutorialPhaseActive>();
      int componentOrderVersion3 = this.EntityManager.GetComponentOrderVersion<TutorialPhaseCompleted>();
      int componentOrderVersion4 = this.EntityManager.GetComponentOrderVersion<TutorialPhaseShown>();
      int componentOrderVersion5 = this.EntityManager.GetComponentOrderVersion<TriggerActive>();
      int componentOrderVersion6 = this.EntityManager.GetComponentOrderVersion<TriggerCompleted>();
      int componentOrderVersion7 = this.EntityManager.GetComponentOrderVersion<TutorialShown>();
      // ISSUE: reference to a compiler-generated field
      bool flag1 = componentOrderVersion1 != this.m_TutorialActiveVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag2 = componentOrderVersion2 != this.m_PhaseActiveVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag3 = componentOrderVersion5 != this.m_TriggerActiveVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag4 = componentOrderVersion6 != this.m_TriggerCompletedVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag5 = componentOrderVersion7 != this.m_TutorialShownVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag6 = componentOrderVersion4 != this.m_PhaseShownVersion;
      // ISSUE: reference to a compiler-generated field
      bool flag7 = componentOrderVersion3 != this.m_PhaseCompletedVersion;
      if (flag1)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveTutorialListBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialsBinding != null && flag1 | flag2 | flag3 | flag4 | flag7)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveTutorialBinding.Update();
        // ISSUE: reference to a compiler-generated field
        this.m_ActiveTutorialPhaseBinding.Update();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialsBinding.Update(this.m_TutorialSystem.activeTutorial);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (((PrefabUtils.HasUnlockedPrefabAny<TutorialData, TutorialPhaseData, TutorialTriggerData, TutorialListData>(this.EntityManager, this.m_UnlockQuery) ? 1 : (this.m_WasEnabled != this.m_TutorialSystem.tutorialEnabled ? 1 : 0)) | (flag5 ? 1 : 0) | (flag6 ? 1 : 0)) != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialCategoriesBinding.Update();
        // ISSUE: reference to a compiler-generated field
        if (this.m_TutorialsBinding != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_TutorialsBinding.UpdateAll();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialPendingBinding != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TutorialPendingBinding.Update();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialActiveVersion = componentOrderVersion1;
      // ISSUE: reference to a compiler-generated field
      this.m_PhaseActiveVersion = componentOrderVersion2;
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerActiveVersion = componentOrderVersion5;
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerCompletedVersion = componentOrderVersion6;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialShownVersion = componentOrderVersion7;
      // ISSUE: reference to a compiler-generated field
      this.m_PhaseShownVersion = componentOrderVersion4;
      // ISSUE: reference to a compiler-generated field
      this.m_PhaseCompletedVersion = componentOrderVersion3;
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialSystem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_WasEnabled = this.m_TutorialSystem.tutorialEnabled;
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
        writer.Write(this.EntityManager.HasEnabledComponent<Locked>(uiObjectInfo.entity));
        writer.PropertyName("children");
        // ISSUE: reference to a compiler-generated method
        this.BindTutorialGroup(writer, uiObjectInfo.entity);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      sortedCategories.Dispose();
    }

    protected void BindTutorialGroup(IJsonWriter writer, Entity entity)
    {
      DynamicBuffer<UIGroupElement> buffer;
      if (this.EntityManager.TryGetBuffer<UIGroupElement>(entity, true, out buffer))
      {
        NativeList<UIObjectInfo> sortedObjects = UIObjectInfo.GetSortedObjects(this.EntityManager, buffer, Allocator.TempJob);
        writer.ArrayBegin(sortedObjects.Length);
        for (int index = 0; index < sortedObjects.Length; ++index)
        {
          Entity entity1 = sortedObjects[index].entity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(sortedObjects[index].prefabData);
          UIObject component = prefab.GetComponent<UIObject>();
          writer.TypeBegin(TypeNames.kAdvisorItem);
          writer.PropertyName(nameof (entity));
          writer.Write(entity1);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("icon");
          if (component.m_Icon == null)
            writer.WriteNull();
          else
            writer.Write(component.m_Icon);
          writer.PropertyName("type");
          writer.Write(prefab is TutorialPrefab ? 0 : 1);
          writer.PropertyName("shown");
          writer.Write(this.EntityManager.HasComponent<TutorialShown>(entity1));
          writer.PropertyName("locked");
          writer.Write(this.EntityManager.HasEnabledComponent<Locked>(entity1));
          writer.PropertyName("children");
          // ISSUE: reference to a compiler-generated method
          this.BindTutorialGroup(writer, entity1);
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        sortedObjects.Dispose();
      }
      else
        writer.WriteEmptyArray();
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

    protected void BindActiveTutorialList(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      Entity activeTutorialList = this.m_TutorialSystem.activeTutorialList;
      if (activeTutorialList != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TutorialListPrefab prefab = this.m_PrefabSystem.GetPrefab<TutorialListPrefab>(activeTutorialList);
        // ISSUE: reference to a compiler-generated method
        NativeList<Entity> visibleListTutorials = this.GetVisibleListTutorials(activeTutorialList, Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        NativeList<Entity> listHintTutorials = this.GetListHintTutorials(activeTutorialList, Allocator.Temp);
        try
        {
          writer.TypeBegin(TypeNames.kTutorialList);
          writer.PropertyName("entity");
          writer.Write(activeTutorialList);
          writer.PropertyName("name");
          writer.Write(prefab.name);
          writer.PropertyName("tutorials");
          writer.ArrayBegin(visibleListTutorials.Length);
          foreach (Entity tutorialEntity in visibleListTutorials)
          {
            // ISSUE: reference to a compiler-generated method
            this.BindTutorial(writer, tutorialEntity);
          }
          writer.ArrayEnd();
          writer.PropertyName("hints");
          writer.ArrayBegin(listHintTutorials.Length);
          foreach (Entity tutorialEntity in listHintTutorials)
          {
            // ISSUE: reference to a compiler-generated method
            this.BindTutorial(writer, tutorialEntity);
          }
          writer.ArrayEnd();
          writer.PropertyName("intro");
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_TutorialSystem.showListReminder);
          writer.TypeEnd();
        }
        finally
        {
          visibleListTutorials.Dispose();
        }
      }
      else
        writer.WriteNull();
    }

    private NativeList<Entity> GetVisibleListTutorials(Entity listEntity, Allocator allocator)
    {
      DynamicBuffer<TutorialRef> buffer = this.EntityManager.GetBuffer<TutorialRef>(listEntity, true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_TutorialActivationData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<TutorialActivationData> rwComponentLookup = this.__TypeHandle.__Game_Tutorials_TutorialActivationData_RW_ComponentLookup;
      NativeList<Entity> visibleListTutorials = new NativeList<Entity>(buffer.Length, (AllocatorManager.AllocatorHandle) allocator);
      foreach (TutorialRef tutorialRef in buffer)
      {
        if (!rwComponentLookup.HasComponent(tutorialRef.m_Tutorial))
          visibleListTutorials.Add(in tutorialRef.m_Tutorial);
      }
      return visibleListTutorials;
    }

    private NativeList<Entity> GetListHintTutorials(Entity listEntity, Allocator allocator)
    {
      DynamicBuffer<TutorialRef> buffer = this.EntityManager.GetBuffer<TutorialRef>(listEntity, true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_TutorialActivationData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<TutorialActivationData> rwComponentLookup = this.__TypeHandle.__Game_Tutorials_TutorialActivationData_RW_ComponentLookup;
      NativeList<Entity> listHintTutorials = new NativeList<Entity>(buffer.Length, (AllocatorManager.AllocatorHandle) allocator);
      foreach (TutorialRef tutorialRef in buffer)
      {
        if (rwComponentLookup.HasComponent(tutorialRef.m_Tutorial))
          listHintTutorials.Add(in tutorialRef.m_Tutorial);
      }
      return listHintTutorials;
    }

    private bool AlternativeCompleted(Entity tutorial)
    {
      DynamicBuffer<Game.Tutorials.TutorialAlternative> buffer;
      if (this.EntityManager.TryGetBuffer<Game.Tutorials.TutorialAlternative>(tutorial, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (this.EntityManager.HasComponent<TutorialCompleted>(buffer[index].m_Alternative))
            return true;
        }
      }
      return false;
    }

    protected void BindTutorial(IJsonWriter writer, Entity tutorialEntity)
    {
      if (this.EntityManager.HasComponent<TutorialData>(tutorialEntity))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TutorialPrefab prefab = this.m_PrefabSystem.GetPrefab<TutorialPrefab>(tutorialEntity);
        writer.TypeBegin(TypeNames.kTutorial);
        writer.PropertyName("entity");
        writer.Write(tutorialEntity);
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("icon");
        // ISSUE: reference to a compiler-generated method
        writer.Write(ImageSystem.GetIcon((PrefabBase) prefab));
        writer.PropertyName("locked");
        writer.Write(this.EntityManager.HasEnabledComponent<Locked>(tutorialEntity));
        writer.PropertyName("priority");
        writer.Write(prefab.m_Priority);
        writer.PropertyName("active");
        writer.Write(this.EntityManager.HasComponent<TutorialActive>(tutorialEntity));
        writer.PropertyName("completed");
        // ISSUE: reference to a compiler-generated method
        writer.Write(this.EntityManager.HasComponent<TutorialCompleted>(tutorialEntity) || this.AlternativeCompleted(tutorialEntity));
        writer.PropertyName("shown");
        writer.Write(this.EntityManager.HasComponent<TutorialShown>(tutorialEntity));
        writer.PropertyName("mandatory");
        writer.Write(prefab.m_Mandatory);
        writer.PropertyName("advisorActivation");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.EntityManager.HasComponent<AdvisorActivation>(this.m_TutorialSystem.activeTutorial));
        DynamicBuffer<TutorialPhaseRef> buffer1 = this.EntityManager.GetBuffer<TutorialPhaseRef>(tutorialEntity, true);
        writer.PropertyName("phases");
        int size = 0;
        for (int index = 0; index < buffer1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (TutorialSystem.IsValidControlScheme(buffer1[index].m_Phase, this.m_PrefabSystem))
            ++size;
        }
        writer.ArrayBegin(size);
        for (int index = 0; index < buffer1.Length; ++index)
        {
          Entity phase = buffer1[index].m_Phase;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (TutorialSystem.IsValidControlScheme(phase, this.m_PrefabSystem))
          {
            // ISSUE: reference to a compiler-generated method
            this.BindTutorialPhase(writer, phase);
          }
        }
        writer.ArrayEnd();
        writer.PropertyName("filters");
        // ISSUE: reference to a compiler-generated method
        writer.Write(TutorialsUISystem.GetFilters(prefab));
        writer.PropertyName("alternatives");
        DynamicBuffer<Game.Tutorials.TutorialAlternative> buffer2;
        if (this.EntityManager.TryGetBuffer<Game.Tutorials.TutorialAlternative>(tutorialEntity, true, out buffer2))
        {
          writer.ArrayBegin(buffer2.Length);
          for (int index = 0; index < buffer2.Length; ++index)
            writer.Write(buffer2[index].m_Alternative);
          writer.ArrayEnd();
        }
        else
          writer.WriteNull();
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    protected void BindTutorialPhase(IJsonWriter writer, Entity phaseEntity)
    {
      TutorialPhaseData component1;
      if (this.EntityManager.TryGetComponent<TutorialPhaseData>(phaseEntity, out component1))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TutorialPhasePrefab prefab1 = this.m_PrefabSystem.GetPrefab<TutorialPhasePrefab>(phaseEntity);
        TutorialBalloonPrefab tutorialBalloonPrefab = prefab1 as TutorialBalloonPrefab;
        writer.TypeBegin(TypeNames.kTutorialPhase);
        writer.PropertyName("entity");
        writer.Write(phaseEntity);
        writer.PropertyName("name");
        writer.Write(prefab1.name);
        writer.PropertyName("type");
        writer.Write((int) component1.m_Type);
        writer.PropertyName("active");
        writer.Write(this.EntityManager.HasComponent<TutorialPhaseActive>(phaseEntity));
        writer.PropertyName("shown");
        writer.Write(this.EntityManager.HasComponent<TutorialPhaseShown>(phaseEntity));
        writer.PropertyName("completed");
        writer.Write(this.EntityManager.HasComponent<TutorialPhaseCompleted>(phaseEntity));
        writer.PropertyName("forcesCompletion");
        writer.Write(this.EntityManager.HasComponent<Game.Tutorials.ForceTutorialCompletion>(phaseEntity));
        writer.PropertyName("isBranch");
        writer.Write(this.EntityManager.HasComponent<TutorialPhaseBranch>(phaseEntity));
        writer.PropertyName("image");
        writer.Write(!string.IsNullOrWhiteSpace(prefab1.m_Image) ? prefab1.m_Image : (string) null);
        writer.PropertyName("overrideImagePS");
        writer.Write(!string.IsNullOrWhiteSpace(prefab1.m_OverrideImagePS) ? prefab1.m_OverrideImagePS : (string) null);
        writer.PropertyName("overrideImageXbox");
        writer.Write(!string.IsNullOrWhiteSpace(prefab1.m_OverrideImageXBox) ? prefab1.m_OverrideImageXBox : (string) null);
        writer.PropertyName("icon");
        writer.Write(!string.IsNullOrWhiteSpace(prefab1.m_Icon) ? prefab1.m_Icon : (string) null);
        writer.PropertyName("titleVisible");
        writer.Write(prefab1.m_TitleVisible);
        writer.PropertyName("descriptionVisible");
        writer.Write(prefab1.m_DescriptionVisible);
        writer.PropertyName("balloonTargets");
        if ((UnityEngine.Object) tutorialBalloonPrefab == (UnityEngine.Object) null)
          writer.WriteEmptyArray();
        else
          writer.Write<TutorialBalloonPrefab.BalloonUITarget>((IList<TutorialBalloonPrefab.BalloonUITarget>) tutorialBalloonPrefab.m_UITargets);
        writer.PropertyName("controlScheme");
        writer.Write((int) prefab1.m_ControlScheme);
        writer.PropertyName("trigger");
        TutorialTrigger component2;
        if (this.EntityManager.TryGetComponent<TutorialTrigger>(phaseEntity, out component2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TutorialTriggerPrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<TutorialTriggerPrefabBase>(component2.m_Trigger);
          writer.TypeBegin(TypeNames.kTutorialTrigger);
          writer.PropertyName("entity");
          writer.Write(component2.m_Trigger);
          writer.PropertyName("name");
          writer.Write(prefab2.name);
          Dictionary<int, List<string>> blinkTags = prefab2.GetBlinkTags();
          List<int> list = blinkTags.Keys.ToList<int>();
          list.Sort();
          writer.PropertyName("blinkTags");
          writer.ArrayBegin(list.Count);
          foreach (int key in list)
          {
            List<string> stringList = blinkTags[key];
            writer.Write((IList<string>) stringList);
          }
          writer.ArrayEnd();
          writer.PropertyName("displayUI");
          writer.Write(prefab2.m_DisplayUI);
          writer.PropertyName("active");
          IJsonWriter jsonWriter1 = writer;
          EntityManager entityManager = this.EntityManager;
          int num1 = entityManager.HasComponent<TriggerActive>(component2.m_Trigger) ? 1 : 0;
          jsonWriter1.Write(num1 != 0);
          writer.PropertyName("completed");
          IJsonWriter jsonWriter2 = writer;
          entityManager = this.EntityManager;
          int num2 = entityManager.HasComponent<TriggerCompleted>(component2.m_Trigger) ? 1 : 0;
          jsonWriter2.Write(num2 != 0);
          writer.PropertyName("preCompleted");
          IJsonWriter jsonWriter3 = writer;
          entityManager = this.EntityManager;
          int num3 = entityManager.HasComponent<TriggerPreCompleted>(component2.m_Trigger) ? 1 : 0;
          jsonWriter3.Write(num3 != 0);
          writer.PropertyName("phaseBranching");
          writer.Write(prefab2.phaseBranching);
          writer.TypeEnd();
        }
        else
          writer.WriteNull();
        writer.TypeEnd();
      }
      else
        writer.WriteNull();
    }

    private void ActivateTutorial(Entity tutorial)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.SetTutorial(tutorial, Entity.Null);
    }

    private void ActivateTutorialPhase(Entity tutorial, Entity phase)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.SetTutorial(tutorial, phase);
    }

    private void ForceTutorial(Entity tutorial, Entity phase, bool advisorActivation)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.ForceTutorial(tutorial, phase, advisorActivation);
    }

    protected virtual void CompleteActiveTutorialPhase()
    {
      if (!GameManager.instance.gameMode.IsGame())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.CompleteCurrentTutorialPhase();
    }

    private void CompleteActiveTutorial()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.CompleteTutorial(this.m_TutorialSystem.activeTutorial);
    }

    private void CompleteIntro(bool tutorialEnabled)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.mode = TutorialMode.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialSystem.tutorialEnabled = tutorialEnabled;
    }

    private void OnSetTutorialTagActive(string tag, bool active)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActivationSystem.SetTag(tag, active);
      if (active)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_DeactivationSystem.DeactivateTag(tag);
    }

    private void ActivateTutorialTrigger(string trigger)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem.ActivateTrigger(trigger);
    }

    private static string[] GetFilters(TutorialPrefab prefab)
    {
      TutorialControlSchemeActivation component = prefab.GetComponent<TutorialControlSchemeActivation>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return (string[]) null;
      return new string[1]
      {
        component.m_ControlScheme.ToString()
      };
    }

    protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode gameMode)
    {
      base.OnGameLoadingComplete(purpose, gameMode);
      this.Enabled = gameMode.IsGameOrEditor();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public TutorialsUISystem()
    {
    }

    internal static class BindingNames
    {
      internal const string kTutorialsDisabled = "tutorialsDisabled";
      internal const string kTutorialsEnabled = "tutorialsEnabled";
      internal const string kIntroActive = "introActive";
      internal const string kNext = "next";
      internal const string kActiveTutorial = "activeTutorial";
      internal const string kActiveTutorialPhase = "activeTutorialPhase";
      internal const string kCategories = "categories";
      internal const string kTutorials = "tutorials";
      internal const string kPending = "pending";
      internal const string kActiveList = "activeList";
      internal const string kActivateTutorial = "activateTutorial";
      internal const string kActivateTutorialPhase = "activateTutorialPhase";
      internal const string kForceTutorial = "forceTutorial";
      internal const string kActivateTutorialTrigger = "activateTutorialTrigger";
      internal const string kSetTutorialTagActive = "setTutorialTagActive";
      internal const string kCompleteActiveTutorialPhase = "completeActiveTutorialPhase";
      internal const string kCompleteActiveTutorial = "completeActiveTutorial";
      internal const string kCompleteIntro = "completeIntro";
      internal const string kCompleteListIntro = "completeListIntro";
      internal const string kCompleteListOutro = "completeListOutro";
      internal const string kListIntroActive = "listIntroActive";
      internal const string kListOutroActive = "listOutroActive";
      internal const string kControlScheme = "controlScheme";
      internal const string kAdvisorPanelVisible = "advisorPanelVisible";
      internal const string kToggleTutorials = "toggleTutorials";
    }

    private enum AdvisorItemType
    {
      Tutorial,
      Group,
    }

    private struct TypeHandle
    {
      public ComponentLookup<TutorialActivationData> __Game_Tutorials_TutorialActivationData_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_TutorialActivationData_RW_ComponentLookup = state.GetComponentLookup<TutorialActivationData>();
      }
    }
  }
}
