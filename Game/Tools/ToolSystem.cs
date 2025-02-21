// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Input;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ToolSystem : GameSystemBase, IPreDeserialize
  {
    protected const string kToolKeyGroup = "tool";
    protected const string kToolCancelKeyGroup = "tool cancel";
    protected const string kToolApplyKeyAction = "tool apply";
    protected const string kToolCancelKeyAction = "tool cancel";
    public Action<ToolBaseSystem> EventToolChanged;
    public Action<PrefabBase> EventPrefabChanged;
    public Action<InfoviewPrefab> EventInfoviewChanged;
    public System.Action EventInfomodesChanged;
    private ToolBaseSystem m_ActiveTool;
    private Entity m_Selected;
    private ToolBaseSystem m_LastTool;
    private InfoviewPrefab m_CurrentInfoview;
    private InfoviewPrefab m_LastToolInfoview;
    private PrefabSystem m_PrefabSystem;
    private UpdateSystem m_UpdateSystem;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private List<ToolBaseSystem> m_Tools;
    private List<InfomodePrefab> m_LastToolInfomodes;
    private Dictionary<InfoviewPrefab, List<InfomodeInfo>> m_InfomodeMap;
    private Vector4[] m_InfomodeColors;
    private Vector4[] m_InfomodeParams;
    private int[] m_InfomodeCounts;
    private NativeList<Entity> m_Infomodes;
    private float m_InfoviewTimer;
    private bool m_FullUpdateRequired;
    private bool m_InfoviewUpdateRequired;
    private bool m_IsUpdating;
    private InputBarrier m_ToolActionBarrier;

    public ToolBaseSystem activeTool
    {
      get => this.m_ActiveTool;
      set
      {
        if (value == this.m_ActiveTool)
          return;
        this.m_ActiveTool = value;
        this.RequireFullUpdate();
        Action<ToolBaseSystem> eventToolChanged = this.EventToolChanged;
        if (eventToolChanged == null)
          return;
        eventToolChanged(value);
      }
    }

    public Entity selected
    {
      get => this.m_Selected;
      set => this.m_Selected = value;
    }

    public int selectedIndex { get; set; }

    [CanBeNull]
    public PrefabBase activePrefab => this.m_ActiveTool.GetPrefab();

    public InfoviewPrefab infoview
    {
      get => this.m_CurrentInfoview;
      set
      {
        if (!((UnityEngine.Object) value != (UnityEngine.Object) this.m_CurrentInfoview))
          return;
        this.SetInfoview(value, (List<InfomodePrefab>) null);
      }
    }

    public InfoviewPrefab activeInfoview
    {
      get
      {
        return !((UnityEngine.Object) this.m_CurrentInfoview != (UnityEngine.Object) null) || !this.m_CurrentInfoview.isValid ? (InfoviewPrefab) null : this.m_CurrentInfoview;
      }
    }

    public GameMode actionMode { get; private set; } = GameMode.Other;

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      this.actionMode = mode;
    }

    public ApplyMode applyMode
    {
      get => this.m_LastTool == null ? ApplyMode.None : this.m_LastTool.applyMode;
    }

    public bool ignoreErrors { get; set; }

    public bool fullUpdateRequired { get; private set; }

    public List<ToolBaseSystem> tools
    {
      get
      {
        if (this.m_Tools == null)
          this.m_Tools = new List<ToolBaseSystem>();
        return this.m_Tools;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      // ISSUE: reference to a compiler-generated field
      this.m_LastToolInfomodes = new List<InfomodePrefab>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeMap = new Dictionary<InfoviewPrefab, List<InfomodeInfo>>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeColors = new Vector4[303];
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeParams = new Vector4[101];
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeCounts = new int[3];
      // ISSUE: reference to a compiler-generated field
      this.m_Infomodes = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolActionBarrier = InputManager.instance.CreateMapBarrier("Tool", nameof (ToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_ToolActionBarrier.blocked = true;
      Shader.SetGlobalInt("colossal_InfoviewOn", 0);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated method
      this.ClearInfomodes();
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentInfoview = (InfoviewPrefab) null;
      Shader.SetGlobalInt("colossal_InfoviewOn", 0);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Infomodes.Dispose();
      Shader.SetGlobalInt("colossal_InfoviewOn", 0);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolActionBarrier.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ToolActionBarrier.blocked = !GameManager.instance.gameMode.IsGameOrEditor() || GameManager.instance.isGameLoading;
      // ISSUE: reference to a compiler-generated field
      this.m_IsUpdating = true;
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PreTool);
      // ISSUE: reference to a compiler-generated method
      this.ToolUpdate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.PostTool);
      // ISSUE: reference to a compiler-generated field
      this.fullUpdateRequired = this.m_FullUpdateRequired;
      // ISSUE: reference to a compiler-generated field
      this.m_FullUpdateRequired = false;
      // ISSUE: reference to a compiler-generated field
      this.m_IsUpdating = false;
    }

    public bool ActivatePrefabTool([CanBeNull] PrefabBase prefab)
    {
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        foreach (ToolBaseSystem tool in this.tools)
        {
          // ISSUE: reference to a compiler-generated method
          if (tool.TrySetPrefab(prefab))
          {
            this.activeTool = tool;
            return true;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      return false;
    }

    public void RequireFullUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsUpdating)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FullUpdateRequired = true;
      }
      else
        this.fullUpdateRequired = true;
    }

    private void ToolUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewTimer += UnityEngine.Time.deltaTime;
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewTimer %= 60f;
      // ISSUE: reference to a compiler-generated field
      if (this.activeTool != this.m_LastTool)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_LastTool != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastTool.Enabled = false;
          // ISSUE: reference to a compiler-generated field
          this.m_LastTool.Update();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastTool = this.activeTool;
      }
      InfoviewPrefab infoview = (InfoviewPrefab) null;
      List<InfomodePrefab> infomodePrefabList = (List<InfomodePrefab>) null;
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastTool != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastTool.Enabled = true;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateSystem.Update(SystemUpdatePhase.ToolUpdate);
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastTool != null)
      {
        // ISSUE: reference to a compiler-generated field
        infoview = this.m_LastTool.infoview;
        // ISSUE: reference to a compiler-generated field
        infomodePrefabList = this.m_LastTool.infomodes;
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) infoview != (UnityEngine.Object) this.m_LastToolInfoview)
      {
        // ISSUE: reference to a compiler-generated method
        this.SetInfoview(infoview, infomodePrefabList);
        // ISSUE: reference to a compiler-generated field
        this.m_LastToolInfoview = infoview;
        // ISSUE: reference to a compiler-generated field
        this.m_LastToolInfomodes.Clear();
        if (infomodePrefabList != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastToolInfomodes.AddRange((IEnumerable<InfomodePrefab>) infomodePrefabList);
        }
      }
      else if ((UnityEngine.Object) infoview != (UnityEngine.Object) null && (UnityEngine.Object) infoview == (UnityEngine.Object) this.activeInfoview)
      {
        // ISSUE: reference to a compiler-generated field
        if (infomodePrefabList != null && infomodePrefabList.Count != 0 || this.m_LastToolInfomodes.Count != 0)
        {
          // ISSUE: reference to a compiler-generated method
          List<InfomodeInfo> infomodes = this.GetInfomodes(infoview);
          for (int index = 0; index < infomodes.Count; ++index)
          {
            InfomodeInfo infomodeInfo = infomodes[index];
            if (infomodeInfo.m_Supplemental || infomodeInfo.m_Optional && this.actionMode.IsGame())
            {
              // ISSUE: reference to a compiler-generated field
              int num1 = this.m_LastToolInfomodes.Contains(infomodeInfo.m_Mode) ? 1 : 0;
              bool active = infomodePrefabList != null && infomodePrefabList.Contains(infomodeInfo.m_Mode);
              int num2 = active ? 1 : 0;
              if (num1 != num2)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated method
                this.SetInfomodeActive(this.m_PrefabSystem.GetEntity((PrefabBase) infomodeInfo.m_Mode), active, infomodeInfo.m_Priority);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LastToolInfomodes.Clear();
        if (infomodePrefabList != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastToolInfomodes.AddRange((IEnumerable<InfomodePrefab>) infomodePrefabList);
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_InfoviewUpdateRequired)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InfoviewUpdateRequired = false;
        // ISSUE: reference to a compiler-generated method
        this.UpdateInfoviewColors();
      }
      // ISSUE: reference to a compiler-generated field
      Shader.SetGlobalFloat("colossal_InfoviewTime", this.m_InfoviewTimer);
    }

    private void SetInfoview(InfoviewPrefab value, List<InfomodePrefab> infomodes)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentInfoview = value;
      // ISSUE: reference to a compiler-generated method
      this.ClearInfomodes();
      if ((UnityEngine.Object) this.activeInfoview != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        List<InfomodeInfo> infomodes1 = this.GetInfomodes(value);
        for (int index1 = 0; index1 < infomodes1.Count; ++index1)
        {
          InfomodeInfo infomodeInfo = infomodes1[index1];
          if ((!infomodeInfo.m_Supplemental || infomodes != null && infomodes.Contains(infomodeInfo.m_Mode)) && (!infomodeInfo.m_Optional || !this.actionMode.IsGame() || infomodes == null || infomodes.Contains(infomodeInfo.m_Mode)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) infomodeInfo.m_Mode);
            int colorGroup = infomodeInfo.m_Mode.GetColorGroup();
            // ISSUE: reference to a compiler-generated field
            int index2 = colorGroup * 4 + ++this.m_InfomodeCounts[colorGroup];
            this.EntityManager.AddComponentData<InfomodeActive>(entity, new InfomodeActive(infomodeInfo.m_Priority, index2));
            // ISSUE: reference to a compiler-generated field
            this.m_Infomodes.Add(in entity);
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewTimer = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_InfoviewUpdateRequired = true;
      // ISSUE: reference to a compiler-generated field
      Action<InfoviewPrefab> eventInfoviewChanged = this.EventInfoviewChanged;
      if (eventInfoviewChanged == null)
        return;
      eventInfoviewChanged(value);
    }

    private void ClearInfomodes()
    {
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_InfomodeCounts.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeCounts[index] = 0;
      }
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.RemoveComponent<InfomodeActive>(this.m_Infomodes.AsArray());
      // ISSUE: reference to a compiler-generated field
      this.m_Infomodes.Clear();
    }

    private void UpdateInfoviewColors()
    {
      if ((UnityEngine.Object) this.activeInfoview != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeColors[0] = (Vector4) this.activeInfoview.m_DefaultColor.linear;
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeColors[1] = (Vector4) this.activeInfoview.m_DefaultColor.linear;
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeColors[2] = (Vector4) this.activeInfoview.m_SecondaryColor.linear;
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeParams[0] = new Vector4(1f, 0.0f, 0.0f, 0.0f);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Infomodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity infomode = this.m_Infomodes[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          InfomodePrefab prefab = this.m_PrefabSystem.GetPrefab<InfomodePrefab>(infomode);
          InfomodeActive componentData = this.EntityManager.GetComponentData<InfomodeActive>(infomode);
          Color color1;
          ref Color local1 = ref color1;
          Color color2;
          ref Color local2 = ref color2;
          Color color3;
          ref Color local3 = ref color3;
          float x;
          ref float local4 = ref x;
          float y;
          ref float local5 = ref y;
          float z;
          ref float local6 = ref z;
          float w;
          ref float local7 = ref w;
          prefab.GetColors(out local1, out local2, out local3, out local4, out local5, out local6, out local7);
          // ISSUE: reference to a compiler-generated field
          this.m_InfomodeColors[componentData.m_Index * 3] = (Vector4) color1.linear;
          // ISSUE: reference to a compiler-generated field
          this.m_InfomodeColors[componentData.m_Index * 3 + 1] = (Vector4) color2.linear;
          // ISSUE: reference to a compiler-generated field
          this.m_InfomodeColors[componentData.m_Index * 3 + 2] = (Vector4) color3.linear;
          // ISSUE: reference to a compiler-generated field
          this.m_InfomodeParams[componentData.m_Index] = new Vector4(x, y, z, w);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeCounts.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          for (int infomodeCount = this.m_InfomodeCounts[index1]; infomodeCount < 4; ++infomodeCount)
          {
            int index2 = 1 + index1 * 4 + infomodeCount;
            // ISSUE: reference to a compiler-generated field
            this.m_InfomodeColors[index2 * 3] = new Vector4();
            // ISSUE: reference to a compiler-generated field
            this.m_InfomodeColors[index2 * 3 + 1] = new Vector4();
            // ISSUE: reference to a compiler-generated field
            this.m_InfomodeColors[index2 * 3 + 2] = new Vector4();
            // ISSUE: reference to a compiler-generated field
            this.m_InfomodeParams[index2] = new Vector4(1f, 0.0f, 0.0f, 0.0f);
          }
        }
        Shader.SetGlobalInt("colossal_InfoviewOn", 1);
        // ISSUE: reference to a compiler-generated field
        Shader.SetGlobalVectorArray("colossal_InfomodeColors", this.m_InfomodeColors);
        // ISSUE: reference to a compiler-generated field
        Shader.SetGlobalVectorArray("colossal_InfomodeParams", this.m_InfomodeParams);
      }
      else
        Shader.SetGlobalInt("colossal_InfoviewOn", 0);
    }

    [CanBeNull]
    public List<InfomodeInfo> GetInfomodes(InfoviewPrefab infoview)
    {
      if ((UnityEngine.Object) infoview == (UnityEngine.Object) null)
        return (List<InfomodeInfo>) null;
      List<InfomodeInfo> infomodes;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InfomodeMap.TryGetValue(infoview, out infomodes))
      {
        infomodes = new List<InfomodeInfo>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        DynamicBuffer<InfoviewMode> buffer = this.m_PrefabSystem.GetBuffer<InfoviewMode>((PrefabBase) infoview, true);
        for (int index = 0; index < buffer.Length; ++index)
        {
          InfoviewMode infoviewMode = buffer[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          infomodes.Add(new InfomodeInfo()
          {
            m_Mode = this.m_PrefabSystem.GetPrefab<InfomodePrefab>(infoviewMode.m_Mode),
            m_Priority = infoviewMode.m_Priority,
            m_Supplemental = infoviewMode.m_Supplemental,
            m_Optional = infoviewMode.m_Optional
          });
        }
        infomodes.Sort();
        // ISSUE: reference to a compiler-generated field
        this.m_InfomodeMap.Add(infoview, infomodes);
      }
      return infomodes;
    }

    public List<InfomodeInfo> GetInfoviewInfomodes() => this.GetInfomodes(this.activeInfoview);

    public bool IsInfomodeActive(InfomodePrefab prefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.EntityManager.HasComponent<InfomodeActive>(this.m_PrefabSystem.GetEntity((PrefabBase) prefab));
    }

    public void SetInfomodeActive(InfomodePrefab prefab, bool active, int priority)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.SetInfomodeActive(this.m_PrefabSystem.GetEntity((PrefabBase) prefab), active, priority);
    }

    public void SetInfomodeActive(Entity entity, bool active, int priority)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      if (!active)
      {
        // ISSUE: reference to a compiler-generated field
        int index1 = this.m_Infomodes.IndexOf<Entity, Entity>(entity);
        if (index1 < 0)
          return;
        InfomodeActive componentData1 = this.EntityManager.GetComponentData<InfomodeActive>(entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        int colorGroup = this.m_PrefabSystem.GetPrefab<InfomodePrefab>(entity).GetColorGroup();
        // ISSUE: reference to a compiler-generated field
        int num = colorGroup * 4 + this.m_InfomodeCounts[colorGroup]--;
        EntityManager entityManager;
        if (componentData1.m_Index < num)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index2 = 0; index2 < this.m_Infomodes.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            Entity infomode = this.m_Infomodes[index2];
            entityManager = this.EntityManager;
            InfomodeActive componentData2 = entityManager.GetComponentData<InfomodeActive>(infomode);
            if (componentData2.m_Index == num)
            {
              componentData2.m_Index = componentData1.m_Index;
              entityManager = this.EntityManager;
              entityManager.SetComponentData<InfomodeActive>(infomode, componentData2);
              break;
            }
          }
        }
        entityManager = this.EntityManager;
        entityManager.RemoveComponent<InfomodeActive>(entity);
        // ISSUE: reference to a compiler-generated field
        this.m_Infomodes.RemoveAtSwapBack(index1);
        // ISSUE: reference to a compiler-generated field
        this.m_InfoviewUpdateRequired = true;
        // ISSUE: reference to a compiler-generated field
        System.Action infomodesChanged = this.EventInfomodesChanged;
        if (infomodesChanged == null)
          return;
        infomodesChanged();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Infomodes.Contains<Entity, Entity>(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        InfomodePrefab prefab1 = this.m_PrefabSystem.GetPrefab<InfomodePrefab>(entity);
        int colorGroup = prefab1.GetColorGroup();
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Infomodes.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity infomode = this.m_Infomodes[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          InfomodePrefab prefab2 = this.m_PrefabSystem.GetPrefab<InfomodePrefab>(infomode);
          if (prefab2.GetColorGroup() == colorGroup && !prefab1.CanActivateBoth(prefab2))
          {
            InfomodeActive componentData = this.EntityManager.GetComponentData<InfomodeActive>(infomode);
            this.EntityManager.RemoveComponent<InfomodeActive>(infomode);
            this.EntityManager.AddComponentData<InfomodeActive>(entity, new InfomodeActive(priority, componentData.m_Index));
            // ISSUE: reference to a compiler-generated field
            this.m_Infomodes[index] = entity;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated field
          int index = colorGroup * 4 + ++this.m_InfomodeCounts[colorGroup];
          this.EntityManager.AddComponentData<InfomodeActive>(entity, new InfomodeActive(priority, index));
          // ISSUE: reference to a compiler-generated field
          this.m_Infomodes.Add(in entity);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_InfoviewUpdateRequired = true;
        // ISSUE: reference to a compiler-generated field
        System.Action infomodesChanged = this.EventInfomodesChanged;
        if (infomodesChanged == null)
          return;
        infomodesChanged();
      }
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearInfomodes();
      // ISSUE: reference to a compiler-generated field
      this.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
    }

    [Preserve]
    public ToolSystem()
    {
    }
  }
}
