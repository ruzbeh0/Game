// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Audio;
using Game.City;
using Game.Input;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Serialization;
using Game.Settings;
using Game.Simulation;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialSystem : GameSystemBase, ITutorialSystem, IPreDeserialize
  {
    private PrefabSystem m_PrefabSystem;
    private AudioManager m_AudioManager;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private MapTilePurchaseSystem m_MapTilePurchaseSystem;
    private static readonly float kBalloonCompletionDelay = 0.0f;
    private static readonly float kCompletionDelay = 1.5f;
    private static readonly float kActivationDelay = 3f;
    private static readonly string kWelcomeIntroKey = "WelcomeIntro";
    protected static readonly string kListIntroKey = "ListIntro";
    private static readonly string kListOutroKey = "ListOutro";
    private EntityQuery m_TutorialConfigurationQuery;
    protected EntityQuery m_TutorialQuery;
    private EntityQuery m_TutorialListQuery;
    private EntityQuery m_TutorialPhaseQuery;
    private EntityQuery m_ActiveTutorialListQuery;
    protected EntityQuery m_ActiveTutorialQuery;
    private EntityQuery m_ActiveTutorialPhaseQuery;
    protected EntityQuery m_PendingTutorialListQuery;
    protected EntityQuery m_PendingTutorialQuery;
    protected EntityQuery m_PendingPriorityTutorialQuery;
    protected EntityQuery m_LockedTutorialQuery;
    private EntityQuery m_LockedTutorialPhaseQuery;
    private EntityQuery m_LockedTutorialTriggerQuery;
    private EntityQuery m_LockedTutorialListQuery;
    private EntityQuery m_SoundQuery;
    private EntityArchetype m_UnlockEventArchetype;
    private float m_AccumulatedDelay;
    protected TutorialMode m_Mode;
    protected Setting m_Setting = (Setting) SharedSettings.instance.userState;
    private TutorialSystem.TypeHandle __TypeHandle;

    protected virtual Dictionary<string, bool> ShownTutorials
    {
      get => SharedSettings.instance.userState.shownTutorials;
    }

    public TutorialMode mode
    {
      get => this.m_Mode;
      set
      {
        if (value != this.m_Mode)
        {
          if (this.m_Mode == TutorialMode.Intro)
            this.UpdateSettings(TutorialSystem.kWelcomeIntroKey, true);
          else if (this.m_Mode == TutorialMode.ListIntro)
            this.UpdateSettings(TutorialSystem.kListIntroKey, true);
          else if (this.m_Mode == TutorialMode.ListOutro)
            this.UpdateSettings(TutorialSystem.kListOutroKey, true);
        }
        this.m_Mode = value;
      }
    }

    public Entity activeTutorial
    {
      get
      {
        return !this.m_ActiveTutorialQuery.IsEmptyIgnoreFilter ? this.m_ActiveTutorialQuery.GetSingletonEntity() : Entity.Null;
      }
    }

    public Entity activeTutorialPhase
    {
      get
      {
        return !this.m_ActiveTutorialPhaseQuery.IsEmptyIgnoreFilter ? this.m_ActiveTutorialPhaseQuery.GetSingletonEntity() : Entity.Null;
      }
    }

    public virtual bool tutorialEnabled
    {
      get => SharedSettings.instance.gameplay.showTutorials;
      set
      {
        SharedSettings.instance.gameplay.showTutorials = value;
        if (value)
          return;
        this.mode = TutorialMode.Default;
      }
    }

    public Entity activeTutorialList
    {
      get
      {
        return !this.m_ActiveTutorialListQuery.IsEmptyIgnoreFilter ? this.m_ActiveTutorialListQuery.GetSingletonEntity() : Entity.Null;
      }
    }

    public Entity tutorialPending => this.FindNextTutorial();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.Enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilePurchaseSystem = this.World.GetOrCreateSystemManaged<MapTilePurchaseSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialsConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.Exclude<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialPhaseQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialPhaseData>(), ComponentType.Exclude<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialListQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialListData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialListQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialListData>(), ComponentType.ReadOnly<TutorialRef>(), ComponentType.ReadOnly<TutorialActive>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialActive>());
      // ISSUE: reference to a compiler-generated field
      this.m_ActiveTutorialPhaseQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialPhaseData>(), ComponentType.ReadOnly<TutorialPhaseActive>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialListQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialListData>(), ComponentType.ReadOnly<TutorialRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialPhaseRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_PendingPriorityTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<TutorialPhaseRef>(), ComponentType.ReadOnly<TutorialActivated>(), ComponentType.ReadOnly<ReplaceActiveData>(), ComponentType.Exclude<TutorialActive>(), ComponentType.Exclude<TutorialCompleted>(), ComponentType.Exclude<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialData>(), ComponentType.ReadOnly<Locked>(), ComponentType.Exclude<EditorTutorial>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedTutorialPhaseQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialPhaseData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedTutorialTriggerQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialTriggerData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedTutorialListQuery = this.GetEntityQuery(ComponentType.ReadOnly<TutorialListData>(), ComponentType.ReadOnly<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<Unlock>());
    }

    public Entity nextListTutorial
    {
      get
      {
        Entity activeTutorialList = this.activeTutorialList;
        if (activeTutorialList != Entity.Null)
        {
          DynamicBuffer<TutorialRef> buffer = this.EntityManager.GetBuffer<TutorialRef>(activeTutorialList, true);
          ComponentLookup<TutorialCompleted> componentLookup = this.GetComponentLookup<TutorialCompleted>(true);
          BufferLookup<TutorialAlternative> bufferLookup = this.GetBufferLookup<TutorialAlternative>(true);
          foreach (TutorialRef tutorialRef in buffer)
          {
            if (!this.IsCompleted(tutorialRef.m_Tutorial, bufferLookup, componentLookup))
            {
              if (!this.EntityManager.HasComponent<TutorialActive>(tutorialRef.m_Tutorial))
                return tutorialRef.m_Tutorial;
              break;
            }
          }
        }
        return Entity.Null;
      }
    }

    public bool showListReminder
    {
      get
      {
        return this.activeTutorialList == this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>().m_TutorialsIntroList;
      }
    }

    public virtual void OnResetTutorials()
    {
      if (!GameManager.instance.gameMode.IsGameOrEditor())
        return;
      // ISSUE: reference to a compiler-generated method
      this.ResetState();
      // ISSUE: reference to a compiler-generated method
      this.ClearComponents();
      // ISSUE: reference to a compiler-generated field
      this.m_Mode = TutorialMode.Intro;
    }

    private bool IsCompleted(
      Entity tutorial,
      BufferLookup<TutorialAlternative> alternativeData,
      ComponentLookup<TutorialCompleted> completionData)
    {
      if (completionData.HasComponent(tutorial))
        return true;
      DynamicBuffer<TutorialAlternative> bufferData;
      if (alternativeData.TryGetBuffer(tutorial, out bufferData))
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          if (completionData.HasComponent(bufferData[index].m_Alternative))
            return true;
        }
      }
      return false;
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode gameMode)
    {
      base.OnGamePreload(purpose, gameMode);
      // ISSUE: reference to a compiler-generated method
      this.ResetState();
      this.Enabled = gameMode.IsGame();
    }

    protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode gameMode)
    {
      // ISSUE: reference to a compiler-generated field
      if (gameMode == GameMode.Game && this.tutorialEnabled && !this.ShownTutorials.ContainsKey(TutorialSystem.kWelcomeIntroKey))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mode = TutorialMode.Intro;
      }
      // ISSUE: reference to a compiler-generated method
      this.ReadSettings();
      bool flag;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityConfigurationSystem.unlockMapTiles || this.tutorialEnabled && !(this.ShownTutorials.TryGetValue(TutorialSystem.kListIntroKey, out flag) & flag))
        return;
      // ISSUE: reference to a compiler-generated field
      TutorialsConfigurationData singleton = this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_MapTilesFeature))
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.SetComponentData<Unlock>(this.EntityManager.CreateEntity(this.m_UnlockEventArchetype), new Unlock(singleton.m_MapTilesFeature));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_MapTilePurchaseSystem.UnlockMapTiles();
    }

    private void ResetState()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Mode = TutorialMode.Default;
      // ISSUE: reference to a compiler-generated field
      this.m_AccumulatedDelay = 0.0f;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorial(Entity.Null);
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialList(Entity.Null);
    }

    private void ReadSettings()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_TutorialListQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      foreach (Entity entity in entityArray1)
      {
        PrefabBase prefab;
        bool flag;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(entity, out prefab) && this.ShownTutorials.TryGetValue(prefab.name, out flag))
        {
          if (flag)
          {
            // ISSUE: reference to a compiler-generated method
            this.CleanupTutorialList(entity, true, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.SetTutorialShown(entity, false);
          }
        }
      }
      entityArray1.Dispose();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray2 = this.m_TutorialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < entityArray2.Length; ++index1)
      {
        Entity entity = entityArray2[index1];
        PrefabBase prefab1;
        bool flag;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(entity, out prefab1) && this.ShownTutorials.TryGetValue(prefab1.name, out flag))
        {
          if (flag)
          {
            // ISSUE: reference to a compiler-generated method
            this.CleanupTutorial(entity, true, false);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.SetTutorialShown(entity, false);
            NativeArray<TutorialPhaseRef> nativeArray = this.EntityManager.GetBuffer<TutorialPhaseRef>(entity, true).ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
            for (int index2 = 0; index2 < nativeArray.Length; ++index2)
            {
              Entity phase = nativeArray[index2].m_Phase;
              PrefabBase prefab2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(phase, out prefab2) && this.ShownTutorials.ContainsKey(prefab2.name))
              {
                // ISSUE: reference to a compiler-generated method
                this.SetTutorialShown(phase, false);
              }
            }
            nativeArray.Dispose();
          }
        }
      }
      entityArray2.Dispose();
    }

    private void SetTutorialShown(Entity entity, bool updateSettings = true)
    {
      if (this.EntityManager.HasComponent<TutorialPhaseData>(entity))
      {
        this.EntityManager.AddComponent<TutorialPhaseShown>(entity);
        if (updateSettings)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateSettings(entity);
        }
      }
      else
      {
        this.EntityManager.AddComponent<TutorialShown>(entity);
        if (updateSettings)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateSettings(entity);
        }
      }
      UIObjectData component;
      if (!this.EntityManager.TryGetComponent<UIObjectData>(entity, out component) || !(component.m_Group != Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialShown(component.m_Group, false);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.mode != TutorialMode.Default)
        return;
      if (this.tutorialEnabled)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateActiveTutorialList();
      }
      if (this.tutorialEnabled || this.EntityManager.HasComponent<AdvisorActivation>(this.activeTutorial))
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateActiveTutorial();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearTutorialLocks();
        // ISSUE: reference to a compiler-generated method
        this.SetTutorial(Entity.Null);
        // ISSUE: reference to a compiler-generated method
        this.SetTutorialList(Entity.Null);
      }
    }

    public void ForceTutorial(Entity tutorial, Entity phase, bool advisorActivation)
    {
      if (tutorial != Entity.Null)
      {
        EntityManager entityManager = this.EntityManager;
        entityManager.AddComponent<ForceActivation>(tutorial);
        if (advisorActivation)
        {
          entityManager = this.EntityManager;
          entityManager.AddComponent<AdvisorActivation>(tutorial);
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.SetTutorial(tutorial, phase, false);
    }

    private void SetTutorial(Entity tutorial, bool passed = false)
    {
      // ISSUE: reference to a compiler-generated method
      this.SetTutorial(tutorial, Entity.Null, passed);
    }

    public void SetTutorial(Entity tutorial, Entity phase, bool passed)
    {
      Entity activeTutorial = this.activeTutorial;
      Entity activeTutorialPhase = this.activeTutorialPhase;
      if (tutorial != activeTutorial)
      {
        if (activeTutorial != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.CleanupTutorial(activeTutorial, passed);
        }
        if (tutorial != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetTutorialShown(tutorial);
          this.EntityManager.AddComponent<TutorialActive>(tutorial);
          // ISSUE: reference to a compiler-generated method
          Entity firstTutorialPhase = this.GetFirstTutorialPhase(tutorial);
          // ISSUE: reference to a compiler-generated method
          this.SetTutorialPhase(phase == Entity.Null ? firstTutorialPhase : phase, passed);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.SetTutorialPhase(Entity.Null, passed);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_AccumulatedDelay = 0.0f;
      }
      else
      {
        if (!(phase != activeTutorialPhase))
          return;
        // ISSUE: reference to a compiler-generated method
        this.SetTutorialPhase(phase, passed);
        // ISSUE: reference to a compiler-generated field
        this.m_AccumulatedDelay = 0.0f;
      }
    }

    public void SetTutorial(Entity tutorial, Entity phase)
    {
      // ISSUE: reference to a compiler-generated method
      this.SetTutorial(tutorial, phase, false);
    }

    private void SetTutorialPhase(Entity tutorialPhase, bool passed)
    {
      Entity activeTutorialPhase = this.activeTutorialPhase;
      if (!(tutorialPhase != activeTutorialPhase))
        return;
      if (activeTutorialPhase != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.CleanupTutorialPhase(activeTutorialPhase, passed);
      }
      if (!(tutorialPhase != Entity.Null))
        return;
      this.EntityManager.AddComponent<TutorialPhaseActive>(tutorialPhase);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TutorialSystem.ManualUnlock(tutorialPhase, this.m_UnlockEventArchetype, this.EntityManager);
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialShown(tutorialPhase);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SoundQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TutorialStartedSound);
      }
      TutorialTrigger component;
      if (!this.EntityManager.TryGetComponent<TutorialTrigger>(tutorialPhase, out component))
        return;
      this.EntityManager.AddComponent<TriggerActive>(component.m_Trigger);
    }

    public void CompleteCurrentTutorialPhase()
    {
      Entity activeTutorialPhase = this.activeTutorialPhase;
      TutorialTrigger component1;
      TutorialNextPhase component2;
      if (activeTutorialPhase != Entity.Null && this.EntityManager.TryGetComponent<TutorialTrigger>(activeTutorialPhase, out component1) && this.EntityManager.TryGetComponent<TutorialNextPhase>(component1.m_Trigger, out component2))
      {
        // ISSUE: reference to a compiler-generated method
        this.CompleteCurrentTutorialPhase(component2.m_NextPhase);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.CompleteCurrentTutorialPhase(Entity.Null);
      }
    }

    public void CompleteCurrentTutorialPhase(Entity nextPhase)
    {
      Entity activeTutorial = this.activeTutorial;
      if (!(activeTutorial != Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated method
      Entity nextPhase1 = this.GetNextPhase(activeTutorial, this.activeTutorialPhase, nextPhase);
      if (this.EntityManager.HasComponent<ForceTutorialCompletion>(this.activeTutorialPhase))
        nextPhase1 = Entity.Null;
      if (nextPhase1 != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.SetTutorialPhase(nextPhase1, true);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.CompleteTutorial(activeTutorial);
      }
    }

    private void UpdateActiveTutorial()
    {
      if (this.activeTutorial != Entity.Null)
      {
        Entity nextPhase;
        // ISSUE: reference to a compiler-generated method
        if (this.CheckCurrentPhaseCompleted(out nextPhase))
        {
          // ISSUE: reference to a compiler-generated method
          this.CompleteCurrentTutorialPhase(nextPhase);
        }
        // ISSUE: reference to a compiler-generated method
        if (this.ShouldReplaceActiveTutorial())
        {
          // ISSUE: reference to a compiler-generated method
          this.ActivateNextTutorial();
        }
      }
      if (!(this.activeTutorial == Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated method
      this.ActivateNextTutorial(true);
    }

    private bool ShouldReplaceActiveTutorial()
    {
      Entity activeTutorial = this.activeTutorial;
      if (!(activeTutorial != Entity.Null) || this.EntityManager.HasComponent<ForceActivation>(activeTutorial))
        return false;
      if (!this.EntityManager.HasComponent<TutorialActivated>(activeTutorial))
        return true;
      // ISSUE: reference to a compiler-generated field
      return !this.EntityManager.HasComponent<ReplaceActiveData>(activeTutorial) && !this.m_PendingPriorityTutorialQuery.IsEmptyIgnoreFilter;
    }

    private void CleanupTutorial(Entity tutorial, bool passed = false, bool updateSettings = true)
    {
      if (!(tutorial != Entity.Null))
        return;
      EntityManager entityManager = this.EntityManager;
      entityManager.RemoveComponent<AdvisorActivation>(tutorial);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TutorialActive>(tutorial);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<ForceActivation>(tutorial);
      entityManager = this.EntityManager;
      NativeArray<TutorialPhaseRef> nativeArray = entityManager.GetBuffer<TutorialPhaseRef>(tutorial, true).ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < nativeArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.CleanupTutorialPhase(nativeArray[index].m_Phase, passed, updateSettings);
      }
      nativeArray.Dispose();
      if (!passed)
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialShown(tutorial);
      entityManager = this.EntityManager;
      entityManager.AddComponent<TutorialCompleted>(tutorial);
      if (updateSettings)
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateSettings(tutorial, true);
        Game.PSI.Telemetry.TutorialEvent(tutorial);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TutorialSystem.ManualUnlock(tutorial, this.m_UnlockEventArchetype, this.EntityManager);
    }

    private void UpdateSettings(Entity tutorial, bool passed = false)
    {
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_PrefabSystem.TryGetPrefab<PrefabBase>(tutorial, out prefab))
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateSettings(prefab.name, passed);
    }

    private void UpdateSettings(string name, bool passed)
    {
      // ISSUE: reference to a compiler-generated field
      Setting setting = this.m_Setting;
      if (!this.ShownTutorials.ContainsKey(name))
      {
        this.ShownTutorials[name] = passed;
        setting.ApplyAndSave();
      }
      else
      {
        if (!passed)
          return;
        this.ShownTutorials[name] = true;
        setting.ApplyAndSave();
      }
    }

    private void CleanupTutorialPhase(Entity tutorialPhase, bool passed = false, bool updateSettings = true)
    {
      if (!(tutorialPhase != Entity.Null))
        return;
      EntityManager entityManager = this.EntityManager;
      entityManager.RemoveComponent<TutorialPhaseActive>(tutorialPhase);
      if (passed)
      {
        // ISSUE: reference to a compiler-generated method
        this.SetTutorialShown(tutorialPhase);
        entityManager = this.EntityManager;
        entityManager.AddComponent<TutorialPhaseCompleted>(tutorialPhase);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TutorialSystem.ManualUnlock(tutorialPhase, this.m_UnlockEventArchetype, this.EntityManager);
        if (updateSettings)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateSettings(tutorialPhase, true);
        }
      }
      TutorialTrigger component;
      if (!this.EntityManager.TryGetComponent<TutorialTrigger>(tutorialPhase, out component))
        return;
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TriggerActive>(component.m_Trigger);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TriggerCompleted>(component.m_Trigger);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TriggerPreCompleted>(component.m_Trigger);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TutorialNextPhase>(component.m_Trigger);
      if (!passed)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TutorialSystem.ManualUnlock(component.m_Trigger, this.m_UnlockEventArchetype, this.EntityManager);
    }

    private void ActivateNextTutorial(bool delay = false)
    {
      if (delay)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AccumulatedDelay += UnityEngine.Time.deltaTime;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_AccumulatedDelay < (double) TutorialSystem.kActivationDelay)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_AccumulatedDelay = 0.0f;
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.SetTutorial(this.FindNextTutorial());
    }

    private Entity FindNextTutorial()
    {
      int num1 = -1;
      int num2 = -1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity nextTutorial = this.FindNextTutorial(this.m_PendingPriorityTutorialQuery);
      if (nextTutorial == Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        nextTutorial = this.FindNextTutorial(this.m_PendingTutorialQuery);
      }
      EntityManager entityManager;
      if (nextTutorial != Entity.Null)
      {
        entityManager = this.EntityManager;
        num1 = entityManager.GetComponentData<TutorialData>(nextTutorial).m_Priority;
      }
      Entity activeTutorialList = this.activeTutorialList;
      if (activeTutorialList != Entity.Null)
      {
        entityManager = this.EntityManager;
        num2 = entityManager.GetComponentData<TutorialListData>(activeTutorialList).m_Priority;
      }
      if (activeTutorialList != Entity.Null && (num2 < num1 || nextTutorial == Entity.Null))
      {
        Entity nextListTutorial = this.nextListTutorial;
        entityManager = this.EntityManager;
        if (entityManager.HasComponent<TutorialActivated>(nextListTutorial))
          return nextListTutorial;
      }
      else if (nextTutorial != Entity.Null && (num1 <= num2 || activeTutorialList == Entity.Null))
        return nextTutorial;
      return Entity.Null;
    }

    private Entity FindNextTutorial(EntityQuery query)
    {
      if (query.IsEmptyIgnoreFilter)
        return Entity.Null;
      NativeArray<TutorialData> componentDataArray = query.ToComponentDataArray<TutorialData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      int index1 = 0;
      for (int index2 = 0; index2 < componentDataArray.Length; ++index2)
      {
        if (componentDataArray[index2].m_Priority < componentDataArray[index1].m_Priority)
          index1 = index2;
      }
      Entity nextTutorial = entityArray[index1];
      componentDataArray.Dispose();
      entityArray.Dispose();
      return nextTutorial;
    }

    private bool CheckCurrentPhaseCompleted(out Entity nextPhase)
    {
      nextPhase = Entity.Null;
      TutorialPhaseData component1;
      TutorialTrigger component2;
      if (!this.EntityManager.TryGetComponent<TutorialPhaseData>(this.activeTutorialPhase, out component1) || !this.EntityManager.TryGetComponent<TutorialTrigger>(this.activeTutorialPhase, out component2) || !this.EntityManager.HasComponent<TriggerCompleted>(component2.m_Trigger))
        return false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if ((double) this.m_AccumulatedDelay < (double) TutorialSystem.GetCompletionDelay(component1))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AccumulatedDelay += UnityEngine.Time.deltaTime;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AccumulatedDelay = 0.0f;
      TutorialNextPhase component3;
      if (this.EntityManager.TryGetComponent<TutorialNextPhase>(this.activeTutorialPhase, out component3))
        nextPhase = component3.m_NextPhase;
      if (this.EntityManager.TryGetComponent<TutorialNextPhase>(component2.m_Trigger, out component3))
        nextPhase = component3.m_NextPhase;
      return true;
    }

    private static float GetCompletionDelay(TutorialPhaseData phase)
    {
      if ((double) phase.m_OverrideCompletionDelay >= 0.0)
        return phase.m_OverrideCompletionDelay;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return phase.m_Type == TutorialPhaseType.Balloon ? TutorialSystem.kBalloonCompletionDelay : TutorialSystem.kCompletionDelay;
    }

    private Entity GetNextPhase(Entity tutorial, Entity currentPhase, Entity nextPhase)
    {
      NativeArray<TutorialPhaseRef> nativeArray = this.EntityManager.GetBuffer<TutorialPhaseRef>(tutorial, true).ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < nativeArray.Length; ++index1)
      {
        Entity phase = nativeArray[index1].m_Phase;
        if (nextPhase == Entity.Null)
        {
          if (phase == currentPhase)
          {
            for (int index2 = index1; index2 < nativeArray.Length - 1; ++index2)
            {
              nextPhase = nativeArray[index2 + 1].m_Phase;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (TutorialSystem.IsValidControlScheme(nextPhase, this.m_PrefabSystem))
              {
                nativeArray.Dispose();
                return nextPhase;
              }
            }
          }
        }
        else if (phase == nextPhase)
        {
          nativeArray.Dispose();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return !TutorialSystem.IsValidControlScheme(nextPhase, this.m_PrefabSystem) ? Entity.Null : nextPhase;
        }
      }
      nativeArray.Dispose();
      return Entity.Null;
    }

    private Entity GetFirstTutorialPhase(Entity tutorial)
    {
      DynamicBuffer<TutorialPhaseRef> buffer = this.EntityManager.GetBuffer<TutorialPhaseRef>(tutorial, true);
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity phase = buffer[index].m_Phase;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (TutorialSystem.IsValidControlScheme(phase, this.m_PrefabSystem))
          return phase;
      }
      return Entity.Null;
    }

    public static bool IsValidControlScheme(Entity phase, PrefabSystem prefabSystem)
    {
      // ISSUE: reference to a compiler-generated method
      TutorialPhasePrefab prefab = prefabSystem.GetPrefab<TutorialPhasePrefab>(phase);
      if (prefab != null && (prefab.m_ControlScheme & TutorialPhasePrefab.ControlScheme.All) == TutorialPhasePrefab.ControlScheme.All || GameManager.instance.inputManager.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse && prefab != null && (prefab.m_ControlScheme & TutorialPhasePrefab.ControlScheme.KeyboardAndMouse) == TutorialPhasePrefab.ControlScheme.KeyboardAndMouse)
        return true;
      return GameManager.instance.inputManager.activeControlScheme == InputManager.ControlScheme.Gamepad && prefab != null && (prefab.m_ControlScheme & TutorialPhasePrefab.ControlScheme.Gamepad) == TutorialPhasePrefab.ControlScheme.Gamepad;
    }

    private void ClearTutorialLocks()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearLocks(this.m_LockedTutorialQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearLocks(this.m_LockedTutorialPhaseQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearLocks(this.m_LockedTutorialTriggerQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.ClearLocks(this.m_LockedTutorialListQuery);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CityConfigurationSystem.unlockMapTiles)
        return;
      // ISSUE: reference to a compiler-generated field
      TutorialsConfigurationData singleton = this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>();
      if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_MapTilesFeature))
      {
        EntityManager entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        Entity entity = entityManager.CreateEntity(this.m_UnlockEventArchetype);
        entityManager = this.EntityManager;
        entityManager.SetComponentData<Unlock>(entity, new Unlock(singleton.m_MapTilesFeature));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_MapTilePurchaseSystem.UnlockMapTiles();
    }

    private void ClearLocks(EntityQuery query)
    {
      if (query.IsEmptyIgnoreFilter)
        return;
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearLock(entityArray[index]);
      }
      entityArray.Dispose();
    }

    private void ClearLock(Entity entity)
    {
      NativeArray<UnlockRequirement> nativeArray = this.EntityManager.GetBuffer<UnlockRequirement>(entity, true).ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < nativeArray.Length; ++index)
      {
        UnlockRequirement unlockRequirement = nativeArray[index];
        if (unlockRequirement.m_Prefab == entity && (unlockRequirement.m_Flags & UnlockFlags.RequireAll) != (UnlockFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          TutorialSystem.ManualUnlock(entity, this.m_UnlockEventArchetype, this.EntityManager);
          nativeArray.Dispose();
          return;
        }
      }
      nativeArray.Dispose();
    }

    public void SetAllTutorialsShown()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_TutorialQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_TutorialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.SetTutorialShown(entityArray[index]);
        }
        entityArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_TutorialPhaseQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray1 = this.m_TutorialPhaseQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < entityArray1.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        this.SetTutorialShown(entityArray1[index]);
      }
      entityArray1.Dispose();
    }

    public void CompleteTutorial(Entity tutorial)
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_SoundQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TutorialCompletedSound);
      }
      // ISSUE: reference to a compiler-generated method
      this.CleanupTutorial(tutorial, true);
    }

    public static void ManualUnlock(
      Entity entity,
      EntityArchetype unlockEventArchetype,
      EntityManager entityManager)
    {
      DynamicBuffer<UnlockRequirement> buffer1;
      if (!entityManager.TryGetBuffer<UnlockRequirement>(entity, true, out buffer1) || buffer1.Length <= 0 || !(buffer1[0].m_Prefab == entity) || (buffer1[0].m_Flags & UnlockFlags.RequireAll) == (UnlockFlags) 0)
        return;
      Entity entity1 = entityManager.CreateEntity(unlockEventArchetype);
      entityManager.SetComponentData<Unlock>(entity1, new Unlock(entity));
      DynamicBuffer<ForceUIGroupUnlockData> buffer2;
      if (!entityManager.TryGetBuffer<ForceUIGroupUnlockData>(entity, true, out buffer2))
        return;
      NativeArray<ForceUIGroupUnlockData> nativeArray = buffer2.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < nativeArray.Length; ++index)
      {
        Entity entity2 = entityManager.CreateEntity(unlockEventArchetype);
        entityManager.SetComponentData<Unlock>(entity2, new Unlock(nativeArray[index].m_Entity));
      }
      nativeArray.Dispose();
    }

    public static void ManualUnlock(
      Entity entity,
      EntityArchetype unlockEventArchetype,
      EntityManager entityManager,
      EntityCommandBuffer commandBuffer)
    {
      DynamicBuffer<UnlockRequirement> buffer1;
      if (!entityManager.TryGetBuffer<UnlockRequirement>(entity, true, out buffer1) || buffer1.Length <= 0 || !(buffer1[0].m_Prefab == entity) || (buffer1[0].m_Flags & UnlockFlags.RequireAll) == (UnlockFlags) 0)
        return;
      Entity entity1 = commandBuffer.CreateEntity(unlockEventArchetype);
      commandBuffer.SetComponent<Unlock>(entity1, new Unlock(entity));
      DynamicBuffer<ForceUIGroupUnlockData> buffer2;
      if (!entityManager.TryGetBuffer<ForceUIGroupUnlockData>(entity, true, out buffer2))
        return;
      for (int index = 0; index < buffer2.Length; ++index)
      {
        Entity entity2 = commandBuffer.CreateEntity(unlockEventArchetype);
        commandBuffer.SetComponent<Unlock>(entity2, new Unlock(buffer2[index].m_Entity));
      }
    }

    public static void ManualUnlock(
      Entity entity,
      EntityArchetype unlockEventArchetype,
      ref BufferLookup<ForceUIGroupUnlockData> forcedUnlocksFromEntity,
      ref BufferLookup<UnlockRequirement> unlockRequirementsFromEntity,
      EntityCommandBuffer.ParallelWriter commandBuffer,
      int sortKey)
    {
      DynamicBuffer<UnlockRequirement> bufferData1;
      if (!unlockRequirementsFromEntity.TryGetBuffer(entity, out bufferData1) || bufferData1.Length <= 0 || !(bufferData1[0].m_Prefab == entity) || (bufferData1[0].m_Flags & UnlockFlags.RequireAll) == (UnlockFlags) 0)
        return;
      Entity entity1 = commandBuffer.CreateEntity(sortKey, unlockEventArchetype);
      commandBuffer.SetComponent<Unlock>(sortKey, entity1, new Unlock(entity));
      DynamicBuffer<ForceUIGroupUnlockData> bufferData2;
      if (!forcedUnlocksFromEntity.TryGetBuffer(entity, out bufferData2))
        return;
      for (int index = 0; index < bufferData2.Length; ++index)
      {
        Entity entity2 = commandBuffer.CreateEntity(sortKey, unlockEventArchetype);
        commandBuffer.SetComponent<Unlock>(sortKey, entity2, new Unlock(bufferData2[index].m_Entity));
      }
    }

    private void UpdateActiveTutorialList()
    {
      if (this.activeTutorialList != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.CheckListIntro();
        // ISSUE: reference to a compiler-generated method
        if (this.CheckCurrentTutorialListCompleted())
        {
          // ISSUE: reference to a compiler-generated method
          this.CompleteCurrentTutorialList();
        }
        // ISSUE: reference to a compiler-generated method
        if (!this.ShouldReplaceActiveTutorialList())
          return;
        // ISSUE: reference to a compiler-generated method
        this.ActivateNextTutorialList();
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.ActivateNextTutorialList();
      }
    }

    private void CompleteCurrentTutorialList()
    {
      Entity activeTutorialList = this.activeTutorialList;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CityConfigurationSystem.unlockMapTiles)
      {
        // ISSUE: reference to a compiler-generated field
        TutorialsConfigurationData singleton = this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>();
        if (this.activeTutorialList == singleton.m_TutorialsIntroList)
        {
          if (this.EntityManager.HasEnabledComponent<Locked>(singleton.m_MapTilesFeature))
          {
            // ISSUE: reference to a compiler-generated field
            this.EntityManager.SetComponentData<Unlock>(this.EntityManager.CreateEntity(this.m_UnlockEventArchetype), new Unlock(singleton.m_MapTilesFeature));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_MapTilePurchaseSystem.UnlockMapTiles();
        }
      }
      Entity entity = Entity.Null;
      if (!(activeTutorialList != entity))
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialList(Entity.Null, true);
    }

    private void ActivateNextTutorialList()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialList(this.FindNextTutorialList(this.m_PendingTutorialListQuery));
    }

    private void CheckListIntro()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!(this.activeTutorialList == this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>().m_TutorialsIntroList) || this.ShownTutorials.ContainsKey(TutorialSystem.kListIntroKey) || !(this.activeTutorial == Entity.Null) || this.NonListTutorialPending())
        return;
      this.mode = TutorialMode.ListIntro;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.UpdateSettings(TutorialSystem.kListIntroKey, true);
    }

    private bool NonListTutorialPending()
    {
      // ISSUE: reference to a compiler-generated method
      Entity nextTutorial = this.FindNextTutorial();
      if (!(nextTutorial != Entity.Null))
        return false;
      DynamicBuffer<TutorialRef> buffer = this.EntityManager.GetBuffer<TutorialRef>(this.activeTutorialList, true);
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index].m_Tutorial == nextTutorial)
          return false;
      }
      return true;
    }

    private bool CheckCurrentTutorialListCompleted()
    {
      if (this.EntityManager.HasComponent<TutorialRef>(this.activeTutorialList))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_TutorialCompleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<TutorialCompleted> roComponentLookup = this.__TypeHandle.__Game_Tutorials_TutorialCompleted_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tutorials_TutorialAlternative_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        BufferLookup<TutorialAlternative> alternativeRoBufferLookup = this.__TypeHandle.__Game_Tutorials_TutorialAlternative_RO_BufferLookup;
        DynamicBuffer<TutorialRef> buffer = this.EntityManager.GetBuffer<TutorialRef>(this.activeTutorialList, true);
        for (int index = 0; index < buffer.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (!this.IsCompleted(buffer[index].m_Tutorial, alternativeRoBufferLookup, roComponentLookup))
            return false;
        }
      }
      return true;
    }

    private bool ShouldReplaceActiveTutorialList()
    {
      Entity activeTutorialList = this.activeTutorialList;
      return activeTutorialList != Entity.Null && !this.EntityManager.HasComponent<TutorialActivated>(activeTutorialList);
    }

    private Entity FindNextTutorialList(EntityQuery query)
    {
      if (query.IsEmptyIgnoreFilter)
        return Entity.Null;
      NativeArray<TutorialListData> componentDataArray = query.ToComponentDataArray<TutorialListData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      int index1 = 0;
      for (int index2 = 0; index2 < componentDataArray.Length; ++index2)
      {
        if (componentDataArray[index2].m_Priority < componentDataArray[index1].m_Priority)
          index1 = index2;
      }
      Entity nextTutorialList = entityArray[index1];
      componentDataArray.Dispose();
      entityArray.Dispose();
      return nextTutorialList;
    }

    private void SetTutorialList(Entity tutorialList, bool passed = false, bool updateSettings = true)
    {
      Entity activeTutorialList = this.activeTutorialList;
      if (!(tutorialList != activeTutorialList))
        return;
      if (activeTutorialList != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.CleanupTutorialList(activeTutorialList, passed, updateSettings);
        if (passed)
        {
          // ISSUE: reference to a compiler-generated field
          TutorialsConfigurationData singleton = this.m_TutorialConfigurationQuery.GetSingleton<TutorialsConfigurationData>();
          // ISSUE: reference to a compiler-generated field
          if (updateSettings && activeTutorialList == singleton.m_TutorialsIntroList && !this.ShownTutorials.ContainsKey(TutorialSystem.kListOutroKey))
          {
            this.mode = TutorialMode.ListOutro;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateSettings(TutorialSystem.kListOutroKey, true);
          }
        }
      }
      if (!(tutorialList != Entity.Null))
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialShown(tutorialList, updateSettings);
      this.EntityManager.AddComponent<TutorialActive>(tutorialList);
    }

    private void CleanupTutorialList(Entity tutorialList, bool passed = false, bool updateSettings = true)
    {
      if (!(tutorialList != Entity.Null))
        return;
      EntityManager entityManager = this.EntityManager;
      entityManager.RemoveComponent<TutorialActivated>(tutorialList);
      entityManager = this.EntityManager;
      entityManager.RemoveComponent<TutorialActive>(tutorialList);
      if (!passed)
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetTutorialShown(tutorialList);
      entityManager = this.EntityManager;
      entityManager.AddComponent<TutorialCompleted>(tutorialList);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TutorialSystem.ManualUnlock(tutorialList, this.m_UnlockEventArchetype, this.EntityManager);
      entityManager = this.EntityManager;
      if (entityManager.HasComponent<TutorialRef>(tutorialList))
      {
        entityManager = this.EntityManager;
        NativeArray<TutorialRef> nativeArray = entityManager.GetBuffer<TutorialRef>(tutorialList, true).ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CleanupTutorial(nativeArray[index].m_Tutorial, passed, updateSettings);
        }
        nativeArray.Dispose();
      }
      if (!updateSettings)
        return;
      // ISSUE: reference to a compiler-generated method
      this.UpdateSettings(tutorialList, true);
    }

    public void SkipActiveList() => this.CompleteCurrentTutorialList();

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearComponents();
    }

    private void ClearComponents()
    {
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialActive>(this.m_TutorialListQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialCompleted>(this.m_TutorialListQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialShown>(this.m_TutorialListQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<AdvisorActivation>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialActive>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialCompleted>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialShown>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<ForceActivation>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialActivated>(this.m_TutorialQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialPhaseActive>(this.m_TutorialPhaseQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialPhaseCompleted>(this.m_TutorialPhaseQuery);
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<TutorialPhaseShown>(this.m_TutorialPhaseQuery);
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
    public TutorialSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<TutorialCompleted> __Game_Tutorials_TutorialCompleted_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<TutorialAlternative> __Game_Tutorials_TutorialAlternative_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_TutorialCompleted_RO_ComponentLookup = state.GetComponentLookup<TutorialCompleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_TutorialAlternative_RO_BufferLookup = state.GetBufferLookup<TutorialAlternative>(true);
      }
    }
  }
}
