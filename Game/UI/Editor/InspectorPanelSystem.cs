// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.InspectorPanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Mathematics;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Colossal.UI;
using Game.Common;
using Game.Input;
using Game.Objects;
using Game.Prefabs;
using Game.Reflection;
using Game.Rendering;
using Game.SceneFlow;
using Game.Tools;
using Game.UI.InGame;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class InspectorPanelSystem : EditorPanelSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private ObjectToolSystem m_ObjectTool;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private EditorAssetUploadPanel m_AssetUploadPanel;
    private ProxyAction m_MoveAction;
    private ProxyAction m_CloneAction;
    private ProxyAction m_AutoAlignAction;
    private ProxyAction m_AutoConnectAction;
    private ProxyAction m_AlignXAction;
    private ProxyAction m_AlignYAction;
    private ProxyAction m_AlignZAction;
    private EditorGenerator m_EditorGenerator = new EditorGenerator();
    private Button[] m_MeshFooter;
    private Button[] m_InstanceFooter;
    private Button[] m_PrefabFooter;
    private Button[] m_CustomAssetFooter;
    private Entity m_CurrentSelectedEntity;
    [CanBeNull]
    private object m_SelectedObject;
    [CanBeNull]
    private object m_ParentObject;
    [CanBeNull]
    private ObjectSubObjectInfo m_LastSubObject;
    [CanBeNull]
    private ObjectSubObjectInfo m_CurrentSubObject;
    [CanBeNull]
    private LocalizedString m_SelectedName = (LocalizedString) (string) null;
    [CanBeNull]
    private LocalizedString m_ParentName = (LocalizedString) (string) null;
    private List<object> m_SectionObjects = new List<object>();
    private System.Collections.Generic.Dictionary<PrefabBase, InspectorPanelSystem.LocalizationFields> m_WipLocalization = new System.Collections.Generic.Dictionary<PrefabBase, InspectorPanelSystem.LocalizationFields>();
    private PdxSdkPlatform m_Platform;

    private bool DisableSection(object obj, object parent)
    {
      return obj is ComponentBase componentBase1 && componentBase1.prefab.builtin || (obj is ObjectMeshInfo || obj is ObjectSubObjectInfo) && parent is ComponentBase componentBase2 && componentBase2.prefab.builtin;
    }

    private static bool IsBuiltinAsset(AssetData asset)
    {
      return asset != (IAssetData) null && asset.database != null && asset.database is Colossal.IO.AssetDatabase.AssetDatabase<Colossal.IO.AssetDatabase.Game>;
    }

    private InspectorPanelSystem.Mode mode
    {
      get
      {
        return !(this.m_CurrentSelectedEntity == Entity.Null) || !(this.m_SelectedObject is PrefabBase) ? InspectorPanelSystem.Mode.Instance : InspectorPanelSystem.Mode.Prefab;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectTool = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AssetUploadPanel = this.World.GetOrCreateSystemManaged<EditorAssetUploadPanel>();
      // ISSUE: reference to a compiler-generated field
      this.m_MoveAction = InputManager.instance.FindAction("Editor", "Move Selected");
      // ISSUE: reference to a compiler-generated field
      this.m_CloneAction = InputManager.instance.FindAction("Editor", "Clone");
      // ISSUE: reference to a compiler-generated field
      this.m_AutoAlignAction = InputManager.instance.FindAction("Editor", "Auto Align");
      // ISSUE: reference to a compiler-generated field
      this.m_AutoConnectAction = InputManager.instance.FindAction("Editor", "Auto Connect");
      // ISSUE: reference to a compiler-generated field
      this.m_AlignXAction = InputManager.instance.FindAction("Editor", "Align X");
      // ISSUE: reference to a compiler-generated field
      this.m_AlignYAction = InputManager.instance.FindAction("Editor", "Align Y");
      // ISSUE: reference to a compiler-generated field
      this.m_AlignZAction = InputManager.instance.FindAction("Editor", "Align Z");
      Button[] buttonArray1 = new Button[1];
      Button button1 = new Button();
      button1.displayName = (LocalizedString) "Editor.LOCATE";
      button1.action = (Action) (() =>
      {
        int elementIndex = -1;
        float3 position;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!(this.m_CurrentSelectedEntity != Entity.Null) || !SelectedInfoUISystem.TryGetPosition(this.m_CurrentSelectedEntity, this.EntityManager, ref elementIndex, out Entity _, out position, out Bounds3 _, out quaternion _))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.cinematicCameraController)
        {
          // ISSUE: reference to a compiler-generated field
          Vector3 rotation = this.m_CameraUpdateSystem.cinematicCameraController.rotation;
          rotation.x = Mathf.Clamp(rotation.x, 0.0f, 90f);
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.cinematicCameraController.rotation = rotation;
          position = (float3) ((Vector3) position + Quaternion.Euler(rotation) * new Vector3(0.0f, 0.0f, -1000f));
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.cinematicCameraController.position = (Vector3) position;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.activeCameraController.pivot = (Vector3) position;
        }
      });
      buttonArray1[0] = button1;
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceFooter = buttonArray1;
      Button[] buttonArray2 = new Button[2];
      Button button2 = new Button();
      button2.displayName = (LocalizedString) "Editor.DUPLICATE_TEMPLATE";
      button2.action = (Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (!(this.m_SelectedObject is PrefabBase selectedObject2))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.DuplicatePrefab(selectedObject2);
        if (this.mode == InspectorPanelSystem.Mode.Instance)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ToolSystem.ActivatePrefabTool(prefab);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.SelectPrefab(prefab);
        }
      });
      button2.tooltip = new LocalizedString?((LocalizedString) "Editor.DUPLICATE_TEMPLATE_TOOLTIP");
      buttonArray2[0] = button2;
      Button button3 = new Button();
      button3.displayName = (LocalizedString) "Editor.ADD_COMPONENT";
      button3.tooltip = new LocalizedString?((LocalizedString) "Editor.ADD_COMPONENT_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button3.action = (Action) (() => this.activeSubPanel = (IEditorPanel) new TypePickerPanel(new LocalizedString("Editor.ADD_COMPONENT_NAMED", (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
      {
        {
          "NAME",
          (ILocElement) this.m_SelectedName
        }
      }), (LocalizedString) "Editor.COMPONENT_TYPES", (IEnumerable<Item>) this.GetComponentTypeItems().ToList<Item>(), (TypePickerPanel.SelectCallback) (type =>
      {
        this.CloseSubPanel();
        PrefabBase prefabBase = (PrefabBase) null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is PrefabBase selectedObject6)
        {
          prefabBase = selectedObject6;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedObject is ObjectMeshInfo selectedObject5)
            prefabBase = (PrefabBase) selectedObject5.m_Mesh;
        }
        if (!((UnityEngine.Object) prefabBase != (UnityEngine.Object) null) || prefabBase.Has(type))
          return;
        prefabBase.AddComponent(type);
      }), new Action(((EditorPanelSystemBase) this).CloseSubPanel)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button3.disabled = (Func<bool>) (() => this.DisableSection(this.m_SelectedObject, this.m_ParentObject));
      buttonArray2[1] = button3;
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabFooter = buttonArray2;
      Button[] buttonArray3 = new Button[1];
      Button button4 = new Button();
      button4.displayName = (LocalizedString) "Editor.ADD_COMPONENT";
      button4.tooltip = new LocalizedString?((LocalizedString) "Editor.ADD_COMPONENT_TOOLTIP");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button4.action = (Action) (() => this.activeSubPanel = (IEditorPanel) new TypePickerPanel(new LocalizedString("Editor.ADD_COMPONENT_NAMED", (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
      {
        {
          "NAME",
          (ILocElement) this.m_SelectedName
        }
      }), (LocalizedString) "Editor.COMPONENT_TYPES", (IEnumerable<Item>) this.GetComponentTypeItems().ToList<Item>(), (TypePickerPanel.SelectCallback) (type =>
      {
        this.CloseSubPanel();
        PrefabBase prefabBase = (PrefabBase) null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is PrefabBase selectedObject10)
        {
          prefabBase = selectedObject10;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedObject is ObjectMeshInfo selectedObject9)
            prefabBase = (PrefabBase) selectedObject9.m_Mesh;
        }
        if (!((UnityEngine.Object) prefabBase != (UnityEngine.Object) null) || prefabBase.Has(type))
          return;
        prefabBase.AddComponent(type);
      }), new Action(((EditorPanelSystemBase) this).CloseSubPanel)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button4.disabled = (Func<bool>) (() => this.DisableSection(this.m_SelectedObject, this.m_ParentObject));
      buttonArray3[0] = button4;
      // ISSUE: reference to a compiler-generated field
      this.m_MeshFooter = buttonArray3;
      Button[] buttonArray4 = new Button[1];
      Button button5 = new Button();
      button5.displayName = (LocalizedString) "Editor.SAVE_ASSET";
      button5.tooltip = new LocalizedString?((LocalizedString) "Editor.SAVE_ASSET_TOOLTIP");
      button5.action = (Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        Colossal.Hash128? initialSelected = this.m_SelectedObject is PrefabBase selectedObject12 ? selectedObject12.asset?.guid : new Colossal.Hash128?();
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_ASSET", this.GetCustomAssets(), initialSelected, (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveAsset(name, overwriteGuid)), new Action(((EditorPanelSystemBase) this).CloseSubPanel));
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      button5.disabled = (Func<bool>) (() => this.DisableSection(this.m_SelectedObject, this.m_ParentObject));
      buttonArray4[0] = button5;
      // ISSUE: reference to a compiler-generated field
      this.m_CustomAssetFooter = buttonArray4;
      // ISSUE: reference to a compiler-generated field
      this.m_Platform = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      PlatformManager.instance.onPlatformRegistered += (PlatformRegisteredHandler) (psi =>
      {
        if (!(psi is PdxSdkPlatform pdxSdkPlatform2))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Platform = pdxSdkPlatform2;
      });
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      this.activeSubPanel = (IEditorPanel) null;
      // ISSUE: reference to a compiler-generated field
      this.m_MoveAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_CloneAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AutoAlignAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AutoAlignAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AutoConnectAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AlignXAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AlignYAction.enabled = false;
      // ISSUE: reference to a compiler-generated field
      this.m_AlignZAction.enabled = false;
      // ISSUE: reference to a compiler-generated method
      this.OnColorVariationChanged(Entity.Null, (RenderPrefabBase) null, -1, -1);
      // ISSUE: reference to a compiler-generated method
      this.OnEmissiveChanged(Entity.Null, (RenderPrefabBase) null, -1, -1);
      base.OnStopRunning();
    }

    protected override void OnValueChanged(IWidget widget)
    {
      base.OnValueChanged(widget);
      int variationSetIndex;
      int colorIndex;
      RenderPrefabBase mesh;
      // ISSUE: reference to a compiler-generated method
      if (this.IsColorVariationField(widget, out variationSetIndex, out colorIndex, out mesh))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.OnColorVariationChanged(this.m_ToolSystem.selected, mesh, variationSetIndex, colorIndex);
      }
      int singleLightIndex;
      int multiLightIndex;
      // ISSUE: reference to a compiler-generated method
      if (this.IsEmissiveField(widget, out singleLightIndex, out multiLightIndex, out mesh))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.OnEmissiveChanged(this.m_ToolSystem.selected, mesh, singleLightIndex, multiLightIndex);
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(true);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated method
      this.RefreshContent();
      // ISSUE: reference to a compiler-generated method
      this.HandleInput();
    }

    public bool SelectEntity(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      if (entity == this.m_CurrentSelectedEntity)
        return entity != Entity.Null;
      // ISSUE: reference to a compiler-generated field
      object selectedObject1 = this.m_SelectedObject;
      // ISSUE: reference to a compiler-generated field
      object parentObject = this.m_ParentObject;
      this.activeSubPanel = (IEditorPanel) null;
      // ISSUE: reference to a compiler-generated method
      if (this.SelectObjectForEntity(entity))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentSelectedEntity = entity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (parentObject == this.m_ParentObject && selectedObject1 is ObjectSubObjectInfo objectSubObjectInfo && this.m_SelectedObject is ObjectSubObjectInfo selectedObject2)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastSubObject = objectSubObjectInfo;
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentSubObject = selectedObject2;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_LastSubObject = (ObjectSubObjectInfo) null;
          // ISSUE: reference to a compiler-generated field
          this.m_CurrentSubObject = (ObjectSubObjectInfo) null;
        }
        return true;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSubObject = (ObjectSubObjectInfo) null;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject = (ObjectSubObjectInfo) null;
      return false;
    }

    public void SelectPrefab(PrefabBase prefab)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSubObject = (ObjectSubObjectInfo) null;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject = (ObjectSubObjectInfo) null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedObject = (object) prefab;
      // ISSUE: reference to a compiler-generated field
      this.m_ParentObject = (object) prefab;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void SelectInUnityEditor(UnityEngine.Object obj)
    {
    }

    private bool SelectObjectForEntity(Entity entity)
    {
      if (!(entity == Entity.Null))
      {
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<Game.Objects.Object>(entity))
        {
          entityManager = this.EntityManager;
          PrefabRef component1;
          if (!entityManager.HasComponent<Secondary>(entity) && this.EntityManager.TryGetComponent<PrefabRef>(entity, out component1))
          {
            PrefabBase prefab1;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (!this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component1, out prefab1))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SelectedObject = (object) null;
              // ISSUE: reference to a compiler-generated field
              this.m_ParentObject = (object) null;
              return false;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_SelectedObject = (object) prefab1;
            // ISSUE: reference to a compiler-generated field
            this.m_ParentObject = (object) prefab1;
            Owner component2;
            PrefabRef component3;
            if (this.EntityManager.TryGetComponent<Owner>(entity, out component2) && this.EntityManager.TryGetComponent<PrefabRef>(component2.m_Owner, out component3))
            {
              int index = -1;
              LocalTransformCache component4;
              if (this.EntityManager.TryGetComponent<LocalTransformCache>(entity, out component4))
                index = component4.m_PrefabSubIndex;
              if (index == -1)
                return false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(component3);
              Game.Tools.EditorContainer component5;
              if (this.EntityManager.TryGetComponent<Game.Tools.EditorContainer>(entity, out component5))
              {
                entityManager = this.EntityManager;
                EffectSource component6;
                if (entityManager.HasComponent<EffectData>(component5.m_Prefab) && prefab2.TryGet<EffectSource>(out component6) && component6.m_Effects != null && component6.m_Effects.Count > index)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  prefab1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(component5.m_Prefab);
                  EffectSource.EffectSettings effect = component6.m_Effects[index];
                  if (effect != null && (UnityEngine.Object) effect.m_Effect == (UnityEngine.Object) prefab1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_SelectedObject = (object) effect;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ParentObject = (object) prefab2;
                  }
                }
                else
                {
                  entityManager = this.EntityManager;
                  Game.Prefabs.ActivityLocation component7;
                  if (entityManager.HasComponent<ActivityLocationData>(component5.m_Prefab) && prefab2.TryGet<Game.Prefabs.ActivityLocation>(out component7) && component7.m_Locations != null && component7.m_Locations.Length > index)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    prefab1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(component5.m_Prefab);
                    Game.Prefabs.ActivityLocation.LocationInfo location = component7.m_Locations[index];
                    if (location != null && (UnityEngine.Object) location.m_Activity == (UnityEngine.Object) prefab1)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_SelectedObject = (object) location;
                      // ISSUE: reference to a compiler-generated field
                      this.m_ParentObject = (object) prefab2;
                    }
                  }
                }
              }
              else
              {
                ObjectSubObjects component8;
                if (prefab2.TryGet<ObjectSubObjects>(out component8) && component8.m_SubObjects != null && component8.m_SubObjects.Length > index)
                {
                  ObjectSubObjectInfo subObject = component8.m_SubObjects[index];
                  if (subObject != null && (UnityEngine.Object) subObject.m_Object == (UnityEngine.Object) prefab1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_SelectedObject = (object) subObject;
                    // ISSUE: reference to a compiler-generated field
                    this.m_ParentObject = (object) prefab2;
                  }
                }
              }
            }
            else
            {
              Game.Tools.EditorContainer component9;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.EntityManager.TryGetComponent<Game.Tools.EditorContainer>(entity, out component9) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component9.m_Prefab, out prefab1))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SelectedObject = (object) prefab1;
                // ISSUE: reference to a compiler-generated field
                this.m_ParentObject = (object) prefab1;
              }
            }
            return true;
          }
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedObject = (object) null;
      // ISSUE: reference to a compiler-generated field
      this.m_ParentObject = (object) null;
      return false;
    }

    public bool SelectMesh(Entity entity, int meshIndex)
    {
      PrefabRef component;
      PrefabBase prefab1;
      DynamicBuffer<SubMesh> buffer;
      PrefabBase prefab2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetComponent<PrefabRef>(entity, out component) || !this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component.m_Prefab, out prefab1) || !this.EntityManager.TryGetBuffer<SubMesh>(component.m_Prefab, true, out buffer) || !this.m_PrefabSystem.TryGetPrefab<PrefabBase>(buffer[meshIndex].m_SubMesh, out prefab2))
        return false;
      this.activeSubPanel = (IEditorPanel) null;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedObject = (object) prefab2;
      // ISSUE: reference to a compiler-generated field
      this.m_ParentObject = (object) prefab1;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSelectedEntity = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSubObject = (ObjectSubObjectInfo) null;
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject = (ObjectSubObjectInfo) null;
      ObjectMeshInfo info;
      // ISSUE: reference to a compiler-generated method
      if (prefab1 is ObjectGeometryPrefab prefab3 && this.FindObjectMeshInfo(prefab3, prefab2, out info))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedObject = (object) info;
      }
      return true;
    }

    private bool FindObjectMeshInfo(
      ObjectGeometryPrefab prefab,
      PrefabBase meshPrefab,
      out ObjectMeshInfo info)
    {
      for (int index = 0; index < prefab.m_Meshes.Length; ++index)
      {
        if ((UnityEngine.Object) prefab.m_Meshes[index].m_Mesh == (UnityEngine.Object) meshPrefab)
        {
          info = prefab.m_Meshes[index];
          return true;
        }
      }
      info = (ObjectMeshInfo) null;
      return false;
    }

    private void RefreshContent()
    {
      // ISSUE: reference to a compiler-generated method
      this.RefreshTitle();
      // ISSUE: reference to a compiler-generated method
      this.RefreshSections();
    }

    private void RefreshTitle()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LocalizedString objectName1 = this.GetObjectName(this.m_SelectedObject);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LocalizedString objectName2 = this.GetObjectName(this.m_ParentObject);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (objectName1.Equals(this.m_SelectedName) && objectName2.Equals(this.m_ParentName))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedName = objectName1;
      // ISSUE: reference to a compiler-generated field
      this.m_ParentName = objectName2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ParentObject != this.m_SelectedObject)
        this.title = (LocalizedString) (objectName2.value + " > " + objectName1.value);
      else
        this.title = objectName1;
    }

    [CanBeNull]
    private LocalizedString GetObjectName([CanBeNull] object obj)
    {
      if (obj == null)
        return (LocalizedString) (string) null;
      // ISSUE: reference to a compiler-generated field
      return obj is PrefabBase prefab ? EditorPrefabUtils.GetPrefabLabel(prefab) : LocalizedString.Value(this.m_SelectedObject.GetType().Name);
    }

    private void RefreshSections()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_SectionObjects.SequenceEqual<object>(this.GetSectionObjects()))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SectionObjects.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SectionObjects.AddRange(this.GetSectionObjects());
      // ISSUE: reference to a compiler-generated field
      List<IWidget> widgetList = new List<IWidget>(this.m_SectionObjects.Count);
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_SectionObjects.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        object sectionObject = this.m_SectionObjects[index];
        string name = sectionObject.GetType().Name;
        // ISSUE: reference to a compiler-generated field
        List<IWidget> widgets = new List<IWidget>((IEnumerable<IWidget>) this.m_EditorGenerator.BuildMembers((IValueAccessor) new ObjectAccessor<object>(sectionObject), 0, name).ToArray<IWidget>());
        PrefabBase prefabBase = sectionObject as PrefabBase;
        if ((UnityEngine.Object) prefabBase != (UnityEngine.Object) null && !(prefabBase is RenderPrefabBase))
        {
          // ISSUE: reference to a compiler-generated method
          this.BuildLocalizationFields(prefabBase, widgets);
        }
        EditorSection editorSection1 = new EditorSection();
        editorSection1.path = new PathSegment(name);
        editorSection1.displayName = LocalizedString.Value(WidgetReflectionUtils.NicifyVariableName(name));
        editorSection1.tooltip = new LocalizedString?((LocalizedString) name);
        editorSection1.expanded = true;
        editorSection1.children = (IList<IWidget>) widgets;
        EditorSection editorSection2 = editorSection1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.DisableSection(sectionObject, this.m_ParentObject))
        {
          // ISSUE: reference to a compiler-generated method
          InspectorPanelSystem.DisableAllFields((IWidget) editorSection2);
        }
        if ((UnityEngine.Object) prefabBase != (UnityEngine.Object) null)
        {
          editorSection2.primary = true;
          editorSection2.color = new UnityEngine.Color?(EditorSection.kPrefabColor);
          // ISSUE: reference to a compiler-generated method
          editorSection2.active = InspectorPanelSystem.GetActiveAccessor((ComponentBase) prefabBase);
        }
        else
        {
          ComponentBase component = sectionObject as ComponentBase;
          if (component != null)
          {
            // ISSUE: reference to a compiler-generated method
            editorSection2.onDelete = (Action) (() => ApplyPrefabsSystem.RemoveComponent(component.prefab, component.GetType()));
            // ISSUE: reference to a compiler-generated method
            editorSection2.active = InspectorPanelSystem.GetActiveAccessor(component);
          }
        }
        widgetList.Add((IWidget) editorSection2);
      }
      List<IWidget> panelChildren = new List<IWidget>();
      panelChildren.Add((IWidget) Scrollable.WithChildren((IList<IWidget>) widgetList.ToArray()));
      panelChildren.Add((IWidget) new ModdingBetaBanner());
      // ISSUE: reference to a compiler-generated method
      this.RefreshFooter((IList<IWidget>) panelChildren);
      this.children = (IList<IWidget>) panelChildren.ToArray();
    }

    private void RefreshFooter(IList<IWidget> panelChildren)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedObject is ObjectMeshInfo)
      {
        // ISSUE: reference to a compiler-generated field
        panelChildren.Add((IWidget) ButtonRow.WithChildren(this.m_MeshFooter));
      }
      else
      {
        List<Button> buttonList = new List<Button>();
        if (this.mode == InspectorPanelSystem.Mode.Instance)
        {
          // ISSUE: reference to a compiler-generated field
          buttonList.AddRange((IEnumerable<Button>) this.m_InstanceFooter);
        }
        // ISSUE: reference to a compiler-generated field
        buttonList.AddRange((IEnumerable<Button>) this.m_PrefabFooter);
        panelChildren.Add((IWidget) ButtonRow.WithChildren(buttonList.ToArray()));
        // ISSUE: reference to a compiler-generated field
        if (!(this.m_SelectedObject is PrefabBase selectedObject) || selectedObject.builtin)
          return;
        // ISSUE: reference to a compiler-generated field
        panelChildren.Add((IWidget) ButtonRow.WithChildren(this.m_CustomAssetFooter));
      }
    }

    public static void DisableAllFields(IWidget widget)
    {
      if (widget is IDisableCallback disableCallback)
        disableCallback.disabled = (Func<bool>) (() => true);
      if (!(widget is IContainerWidget containerWidget))
        return;
      foreach (IWidget child in (IEnumerable<IWidget>) containerWidget.children)
      {
        // ISSUE: reference to a compiler-generated method
        InspectorPanelSystem.DisableAllFields(child);
      }
    }

    private IEnumerable<object> GetSectionObjects()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedObject != null)
      {
        // ISSUE: reference to a compiler-generated field
        yield return this.m_SelectedObject;
        int i;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is PrefabBase prefab)
        {
          for (i = 0; i < prefab.components.Count; ++i)
          {
            if (prefab.components[i].GetType().GetCustomAttribute<HideInEditorAttribute>() == null)
              yield return (object) prefab.components[i];
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is ObjectMeshInfo meshInfo)
        {
          yield return (object) meshInfo.m_Mesh;
          for (i = 0; i < meshInfo.m_Mesh.components.Count; ++i)
          {
            if (meshInfo.m_Mesh.components[i].GetType().GetCustomAttribute<HideInEditorAttribute>() == null)
              yield return (object) meshInfo.m_Mesh.components[i];
          }
        }
        prefab = (PrefabBase) null;
        meshInfo = (ObjectMeshInfo) null;
      }
    }

    private static ITypedValueAccessor<bool> GetActiveAccessor(ComponentBase component)
    {
      return (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => component.active), (Action<bool>) (value => component.active = value));
    }

    private void ShowAddComponentPicker()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new TypePickerPanel(new LocalizedString("Editor.ADD_COMPONENT_NAMED", (string) null, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
      {
        {
          "NAME",
          (ILocElement) this.m_SelectedName
        }
      }), (LocalizedString) "Editor.COMPONENT_TYPES", (IEnumerable<Item>) this.GetComponentTypeItems().ToList<Item>(), (TypePickerPanel.SelectCallback) (type =>
      {
        this.CloseSubPanel();
        PrefabBase prefabBase = (PrefabBase) null;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is PrefabBase selectedObject4)
        {
          prefabBase = selectedObject4;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedObject is ObjectMeshInfo selectedObject3)
            prefabBase = (PrefabBase) selectedObject3.m_Mesh;
        }
        if (!((UnityEngine.Object) prefabBase != (UnityEngine.Object) null) || prefabBase.Has(type))
          return;
        prefabBase.AddComponent(type);
      }), new Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void OnDuplicate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_SelectedObject is PrefabBase selectedObject))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase prefab = this.m_PrefabSystem.DuplicatePrefab(selectedObject);
      if (this.mode == InspectorPanelSystem.Mode.Instance)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.ActivatePrefabTool(prefab);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.SelectPrefab(prefab);
      }
    }

    private void OnAddComponent(System.Type type)
    {
      this.CloseSubPanel();
      PrefabBase prefabBase = (PrefabBase) null;
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedObject is PrefabBase selectedObject2)
      {
        prefabBase = selectedObject2;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_SelectedObject is ObjectMeshInfo selectedObject1)
          prefabBase = (PrefabBase) selectedObject1.m_Mesh;
      }
      if (!((UnityEngine.Object) prefabBase != (UnityEngine.Object) null) || prefabBase.Has(type))
        return;
      prefabBase.AddComponent(type);
    }

    private IEnumerable<Item> GetComponentTypeItems()
    {
      // ISSUE: reference to a compiler-generated field
      object obj = this.m_SelectedObject;
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedObject is ObjectMeshInfo selectedObject)
        obj = (object) selectedObject.m_Mesh;
      if (obj is PrefabBase selectedPrefab)
      {
        foreach (System.Type type in TypePickerPanel.GetAllConcreteTypesDerivedFrom<ComponentBase>())
        {
          if (!type.IsSubclassOf(typeof (PrefabBase)) && !selectedPrefab.Has(type))
          {
            System.Type prefabType = selectedPrefab.GetType();
            if (!(prefabType == type) && type.GetCustomAttribute<HideInEditorAttribute>() == null)
            {
              ComponentMenu customAttribute = type.GetCustomAttribute<ComponentMenu>();
              if (customAttribute?.requiredPrefab == null || customAttribute == null || customAttribute.requiredPrefab.Length == 0 || ((IEnumerable<System.Type>) customAttribute.requiredPrefab).Any<System.Type>((Func<System.Type, bool>) (t => t.IsAssignableFrom(prefabType))))
                yield return new Item()
                {
                  type = type,
                  name = WidgetReflectionUtils.NicifyVariableName(type.Name),
                  parentDir = customAttribute?.menu
                };
            }
          }
        }
      }
    }

    private void HandleInput()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MoveAction.enabled = this.canMoveSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_MoveAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.MoveSelected();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_CloneAction.enabled = this.canCloneSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_CloneAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.CloneSelected();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AutoAlignAction.enabled = this.canAlignSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AutoAlignAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.AutoAlign();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AutoConnectAction.enabled = this.canAlignSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AutoConnectAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.AutoConnect();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AlignXAction.enabled = this.canAlignSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AlignXAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.AlignX();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AlignYAction.enabled = this.canAlignSelected;
      // ISSUE: reference to a compiler-generated field
      if (this.m_AlignYAction.WasPerformedThisFrame())
      {
        // ISSUE: reference to a compiler-generated method
        this.AlignY();
      }
      // ISSUE: reference to a compiler-generated field
      this.m_AlignZAction.enabled = this.canAlignSelected;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AlignZAction.WasPerformedThisFrame())
        return;
      // ISSUE: reference to a compiler-generated method
      this.AlignZ();
    }

    private bool canMoveSelected
    {
      get
      {
        return this.EntityManager.HasComponent<PrefabRef>(this.m_CurrentSelectedEntity) && this.EntityManager.HasComponent<Game.Objects.Object>(this.m_CurrentSelectedEntity) && this.m_CurrentSelectedEntity == this.m_ToolSystem.selected;
      }
    }

    private void MoveSelected()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectTool.StartMoving(this.m_CurrentSelectedEntity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectTool;
    }

    private bool canCloneSelected
    {
      get
      {
        return this.EntityManager.HasComponent<PrefabRef>(this.m_CurrentSelectedEntity) && this.EntityManager.HasComponent<Game.Objects.Object>(this.m_CurrentSelectedEntity);
      }
    }

    private void CloneSelected()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ObjectPrefab prefab = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(this.EntityManager.GetComponentData<PrefabRef>(this.m_CurrentSelectedEntity));
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectTool.mode = ObjectToolSystem.Mode.Create;
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectTool.prefab = prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectTool;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_CurrentSelectedEntity == this.m_ToolSystem.selected))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.selected = Entity.Null;
    }

    private bool canAlignSelected
    {
      get => this.m_CurrentSubObject != null && this.m_LastSubObject != null;
    }

    private void AutoAlign()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentSubObject == null || this.m_LastSubObject == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float num1 = Mathf.Abs(this.m_CurrentSubObject.m_Position.x - this.m_LastSubObject.m_Position.x);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float num2 = Mathf.Abs(this.m_CurrentSubObject.m_Position.y - this.m_LastSubObject.m_Position.y);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float num3 = Mathf.Abs(this.m_CurrentSubObject.m_Position.z - this.m_LastSubObject.m_Position.z);
      if ((double) num1 < (double) num2 || (double) num1 < (double) num3)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentSubObject.m_Position.x = this.m_LastSubObject.m_Position.x;
      }
      if ((double) num2 < (double) num1 || (double) num2 < (double) num3)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentSubObject.m_Position.y = this.m_LastSubObject.m_Position.y;
      }
      if ((double) num3 < (double) num1 || (double) num3 < (double) num2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentSubObject.m_Position.z = this.m_LastSubObject.m_Position.z;
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(false);
    }

    private void AutoConnect()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentSubObject == null || this.m_LastSubObject == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity entity1 = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_LastSubObject.m_Object);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity entity2 = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_CurrentSubObject.m_Object);
      ObjectGeometryData component1;
      ObjectGeometryData component2;
      if (this.EntityManager.TryGetComponent<ObjectGeometryData>(entity1, out component1) && this.EntityManager.TryGetComponent<ObjectGeometryData>(entity2, out component2))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = Mathf.Abs(this.m_CurrentSubObject.m_Position.x - this.m_LastSubObject.m_Position.x);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num2 = Mathf.Abs(this.m_CurrentSubObject.m_Position.y - this.m_LastSubObject.m_Position.y);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num3 = Mathf.Abs(this.m_CurrentSubObject.m_Position.z - this.m_LastSubObject.m_Position.z);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds3_1 = MathUtils.Bounds(MathUtils.Box(component1.m_Bounds, this.m_LastSubObject.m_Rotation, this.m_LastSubObject.m_Position));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Bounds3 bounds3_2 = MathUtils.Bounds(MathUtils.Box(component2.m_Bounds, this.m_CurrentSubObject.m_Rotation, this.m_CurrentSubObject.m_Position));
        if ((double) num1 > (double) num2 && (double) num1 > (double) num3)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_CurrentSubObject.m_Position.x > (double) this.m_LastSubObject.m_Position.x)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.x += bounds3_1.max.x - bounds3_2.min.x;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.x += bounds3_1.min.x - bounds3_2.max.x;
          }
        }
        else if ((double) num2 > (double) num1 && (double) num2 > (double) num3)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_CurrentSubObject.m_Position.y > (double) this.m_LastSubObject.m_Position.y)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.y += bounds3_1.max.y - bounds3_2.min.y;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.y += bounds3_1.min.y - bounds3_2.max.y;
          }
        }
        else if ((double) num3 > (double) num1 && (double) num3 > (double) num2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_CurrentSubObject.m_Position.z > (double) this.m_LastSubObject.m_Position.z)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.z += bounds3_1.max.z - bounds3_2.min.z;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CurrentSubObject.m_Position.z += bounds3_1.min.z - bounds3_2.max.z;
          }
        }
      }
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(false);
    }

    private void AlignX()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentSubObject == null || this.m_LastSubObject == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject.m_Position.x = this.m_LastSubObject.m_Position.x;
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(false);
    }

    private void AlignY()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentSubObject == null || this.m_LastSubObject == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject.m_Position.y = this.m_LastSubObject.m_Position.y;
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(false);
    }

    private void AlignZ()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CurrentSubObject == null || this.m_LastSubObject == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CurrentSubObject.m_Position.z = this.m_LastSubObject.m_Position.z;
      // ISSUE: reference to a compiler-generated method
      this.UpdateParent(false);
    }

    private void UpdateParent(bool moveSubObjects)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SelectedObject is ObjectMeshInfo selectedObject)
      {
        RenderPrefabBase mesh = selectedObject.m_Mesh;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabSystem.UpdatePrefab((PrefabBase) mesh);
        mesh.asset?.MarkDirty();
      }
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_ParentObject is PrefabBase parentObject))
        return;
      if (moveSubObjects)
      {
        // ISSUE: reference to a compiler-generated method
        this.MoveSubObjects(parentObject);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.UpdatePrefab(parentObject);
      parentObject.asset?.MarkDirty();
    }

    private void MoveSubObjects(PrefabBase prefab)
    {
      DynamicBuffer<SubMesh> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.EntityManager.TryGetBuffer<SubMesh>(this.m_PrefabSystem.GetEntity(prefab), false, out buffer) || !(prefab is ObjectGeometryPrefab objectGeometryPrefab) || objectGeometryPrefab.m_Meshes == null)
        return;
      int num1 = math.min(buffer.Length, objectGeometryPrefab.m_Meshes.Length);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        SubMesh subMesh = buffer[index1];
        ObjectMeshInfo mesh = objectGeometryPrefab.m_Meshes[index1];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!(subMesh.m_SubMesh != this.m_PrefabSystem.GetEntity((PrefabBase) mesh.m_Mesh)) && (!subMesh.m_Position.Equals(mesh.m_Position) || !subMesh.m_Rotation.Equals(mesh.m_Rotation)))
        {
          if (subMesh.m_Rotation.Equals(mesh.m_Rotation))
          {
            float3 float3 = mesh.m_Position - subMesh.m_Position;
            ObjectSubObjects component1;
            if (prefab.TryGet<ObjectSubObjects>(out component1) && component1.m_SubObjects != null)
            {
              for (int index2 = 0; index2 < component1.m_SubObjects.Length; ++index2)
              {
                ObjectSubObjectInfo subObject = component1.m_SubObjects[index2];
                if (subObject.m_ParentMesh % 1000 == index1)
                  subObject.m_Position += float3;
              }
            }
            ObjectSubAreas component2;
            if (prefab.TryGet<ObjectSubAreas>(out component2) && component2.m_SubAreas != null)
            {
              for (int index3 = 0; index3 < component2.m_SubAreas.Length; ++index3)
              {
                ObjectSubAreaInfo subArea = component2.m_SubAreas[index3];
                if (subArea.m_NodePositions != null && subArea.m_ParentMeshes != null)
                {
                  int num2 = math.min(subArea.m_NodePositions.Length, subArea.m_ParentMeshes.Length);
                  for (int index4 = 0; index4 < num2; ++index4)
                  {
                    if (subArea.m_ParentMeshes[index4] == index1)
                      subArea.m_NodePositions[index4] += float3;
                  }
                }
              }
            }
            ObjectSubLanes component3;
            if (prefab.TryGet<ObjectSubLanes>(out component3) && component3.m_SubLanes != null)
            {
              for (int index5 = 0; index5 < component3.m_SubLanes.Length; ++index5)
              {
                ObjectSubLaneInfo subLane = component3.m_SubLanes[index5];
                bool2 x = subLane.m_ParentMesh == index1;
                if (math.all(x))
                  subLane.m_BezierCurve += float3;
                else if (x.x)
                {
                  subLane.m_BezierCurve.a += float3;
                  subLane.m_BezierCurve.b += float3 * 0.6666667f;
                  subLane.m_BezierCurve.c += float3 * 0.333333343f;
                }
                else if (x.y)
                {
                  subLane.m_BezierCurve.d += float3;
                  subLane.m_BezierCurve.c += float3 * 0.6666667f;
                  subLane.m_BezierCurve.b += float3 * 0.333333343f;
                }
              }
            }
            ObjectSubNets component4;
            if (prefab.TryGet<ObjectSubNets>(out component4) && component4.m_SubNets != null)
            {
              for (int index6 = 0; index6 < component4.m_SubNets.Length; ++index6)
              {
                ObjectSubNetInfo subNet = component4.m_SubNets[index6];
                bool2 x = subNet.m_ParentMesh == index1;
                if (math.all(x))
                  subNet.m_BezierCurve += float3;
                else if (x.x)
                {
                  subNet.m_BezierCurve.a += float3;
                  subNet.m_BezierCurve.b += float3 * 0.6666667f;
                  subNet.m_BezierCurve.c += float3 * 0.333333343f;
                }
                else if (x.y)
                {
                  subNet.m_BezierCurve.d += float3;
                  subNet.m_BezierCurve.c += float3 * 0.6666667f;
                  subNet.m_BezierCurve.b += float3 * 0.333333343f;
                }
              }
            }
            EffectSource component5;
            if (prefab.TryGet<EffectSource>(out component5) && component5.m_Effects != null)
            {
              for (int index7 = 0; index7 < component5.m_Effects.Count; ++index7)
              {
                EffectSource.EffectSettings effect = component5.m_Effects[index7];
                if (effect.m_ParentMesh == index1)
                  effect.m_PositionOffset += float3;
              }
            }
          }
          else
          {
            float4x4 m = float4x4.TRS(subMesh.m_Position, subMesh.m_Rotation, (float3) 1f);
            float4x4 a1 = math.mul(float4x4.TRS(mesh.m_Position, mesh.m_Rotation, (float3) 1f), math.inverse(m));
            quaternion a2 = math.mul(mesh.m_Rotation, math.inverse(subMesh.m_Rotation));
            ObjectSubObjects component6;
            if (prefab.TryGet<ObjectSubObjects>(out component6) && component6.m_SubObjects != null)
            {
              for (int index8 = 0; index8 < component6.m_SubObjects.Length; ++index8)
              {
                ObjectSubObjectInfo subObject = component6.m_SubObjects[index8];
                if (subObject.m_ParentMesh % 1000 == index1)
                {
                  subObject.m_Position = math.transform(a1, subObject.m_Position);
                  subObject.m_Rotation = math.normalize(math.mul(a2, subObject.m_Rotation));
                }
              }
            }
            ObjectSubAreas component7;
            if (prefab.TryGet<ObjectSubAreas>(out component7) && component7.m_SubAreas != null)
            {
              for (int index9 = 0; index9 < component7.m_SubAreas.Length; ++index9)
              {
                ObjectSubAreaInfo subArea = component7.m_SubAreas[index9];
                if (subArea.m_NodePositions != null && subArea.m_ParentMeshes != null)
                {
                  int num3 = math.min(subArea.m_NodePositions.Length, subArea.m_ParentMeshes.Length);
                  for (int index10 = 0; index10 < num3; ++index10)
                  {
                    if (subArea.m_ParentMeshes[index10] == index1)
                      subArea.m_NodePositions[index10] = math.transform(a1, subArea.m_NodePositions[index10]);
                  }
                }
              }
            }
            ObjectSubLanes component8;
            if (prefab.TryGet<ObjectSubLanes>(out component8) && component8.m_SubLanes != null)
            {
              for (int index11 = 0; index11 < component8.m_SubLanes.Length; ++index11)
              {
                ObjectSubLaneInfo subLane = component8.m_SubLanes[index11];
                bool2 x = subLane.m_ParentMesh == index1;
                if (math.all(x))
                {
                  subLane.m_BezierCurve.a = math.transform(a1, subLane.m_BezierCurve.a);
                  subLane.m_BezierCurve.b = math.transform(a1, subLane.m_BezierCurve.b);
                  subLane.m_BezierCurve.c = math.transform(a1, subLane.m_BezierCurve.c);
                  subLane.m_BezierCurve.d = math.transform(a1, subLane.m_BezierCurve.d);
                }
                else if (x.x)
                {
                  subLane.m_BezierCurve.a = math.transform(a1, subLane.m_BezierCurve.a);
                  subLane.m_BezierCurve.b = math.lerp(subLane.m_BezierCurve.b, math.transform(a1, subLane.m_BezierCurve.b), 0.6666667f);
                  subLane.m_BezierCurve.c = math.lerp(subLane.m_BezierCurve.c, math.transform(a1, subLane.m_BezierCurve.c), 0.333333343f);
                }
                else if (x.y)
                {
                  subLane.m_BezierCurve.d = math.transform(a1, subLane.m_BezierCurve.d);
                  subLane.m_BezierCurve.c = math.lerp(subLane.m_BezierCurve.c, math.transform(a1, subLane.m_BezierCurve.c), 0.6666667f);
                  subLane.m_BezierCurve.b = math.lerp(subLane.m_BezierCurve.b, math.transform(a1, subLane.m_BezierCurve.b), 0.333333343f);
                }
              }
            }
            ObjectSubNets component9;
            if (prefab.TryGet<ObjectSubNets>(out component9) && component9.m_SubNets != null)
            {
              for (int index12 = 0; index12 < component9.m_SubNets.Length; ++index12)
              {
                ObjectSubNetInfo subNet = component9.m_SubNets[index12];
                bool2 x = subNet.m_ParentMesh == index1;
                if (math.all(x))
                {
                  subNet.m_BezierCurve.a = math.transform(a1, subNet.m_BezierCurve.a);
                  subNet.m_BezierCurve.b = math.transform(a1, subNet.m_BezierCurve.b);
                  subNet.m_BezierCurve.c = math.transform(a1, subNet.m_BezierCurve.c);
                  subNet.m_BezierCurve.d = math.transform(a1, subNet.m_BezierCurve.d);
                }
                else if (x.x)
                {
                  subNet.m_BezierCurve.a = math.transform(a1, subNet.m_BezierCurve.a);
                  subNet.m_BezierCurve.b = math.lerp(subNet.m_BezierCurve.b, math.transform(a1, subNet.m_BezierCurve.b), 0.6666667f);
                  subNet.m_BezierCurve.c = math.lerp(subNet.m_BezierCurve.c, math.transform(a1, subNet.m_BezierCurve.c), 0.333333343f);
                }
                else if (x.y)
                {
                  subNet.m_BezierCurve.d = math.transform(a1, subNet.m_BezierCurve.d);
                  subNet.m_BezierCurve.c = math.lerp(subNet.m_BezierCurve.c, math.transform(a1, subNet.m_BezierCurve.c), 0.6666667f);
                  subNet.m_BezierCurve.b = math.lerp(subNet.m_BezierCurve.b, math.transform(a1, subNet.m_BezierCurve.b), 0.333333343f);
                }
              }
            }
            EffectSource component10;
            if (prefab.TryGet<EffectSource>(out component10) && component10.m_Effects != null)
            {
              for (int index13 = 0; index13 < component10.m_Effects.Count; ++index13)
              {
                EffectSource.EffectSettings effect = component10.m_Effects[index13];
                if (effect.m_ParentMesh == index1)
                {
                  effect.m_PositionOffset = math.transform(a1, effect.m_PositionOffset);
                  effect.m_Rotation = math.normalize(math.mul(a2, effect.m_Rotation));
                }
              }
            }
          }
          subMesh.m_Position = mesh.m_Position;
          subMesh.m_Rotation = mesh.m_Rotation;
          buffer[index1] = subMesh;
        }
      }
    }

    private void OnLocate()
    {
      int elementIndex = -1;
      float3 position;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!(this.m_CurrentSelectedEntity != Entity.Null) || !SelectedInfoUISystem.TryGetPosition(this.m_CurrentSelectedEntity, this.EntityManager, ref elementIndex, out Entity _, out position, out Bounds3 _, out quaternion _))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.cinematicCameraController)
      {
        // ISSUE: reference to a compiler-generated field
        Vector3 rotation = this.m_CameraUpdateSystem.cinematicCameraController.rotation;
        rotation.x = Mathf.Clamp(rotation.x, 0.0f, 90f);
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.rotation = rotation;
        position = (float3) ((Vector3) position + Quaternion.Euler(rotation) * new Vector3(0.0f, 0.0f, -1000f));
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.cinematicCameraController.position = (Vector3) position;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CameraUpdateSystem.activeCameraController.pivot = (Vector3) position;
      }
    }

    private bool IsColorVariationField(
      IWidget widget,
      out int variationSetIndex,
      out int colorIndex,
      out RenderPrefabBase mesh)
    {
      variationSetIndex = -1;
      colorIndex = -1;
      mesh = (RenderPrefabBase) null;
      // ISSUE: reference to a compiler-generated field
      if (widget.path.m_Key == null || !(widget is ColorField) || !(this.m_SelectedObject is ObjectMeshInfo selectedObject))
        return false;
      Match match = Regex.Match(widget.path.m_Key, "^ColorProperties.m_ColorVariations\\[(\\d+)\\].m_Colors\\[(\\d+)\\]$");
      if (!match.Success || !int.TryParse(match.Groups[1].Value, out variationSetIndex) || !int.TryParse(match.Groups[2].Value, out colorIndex))
        return false;
      mesh = selectedObject.m_Mesh;
      return true;
    }

    private bool IsEmissiveField(
      IWidget widget,
      out int singleLightIndex,
      out int multiLightIndex,
      out RenderPrefabBase mesh)
    {
      singleLightIndex = -1;
      multiLightIndex = -1;
      mesh = (RenderPrefabBase) null;
      // ISSUE: reference to a compiler-generated field
      if (widget.path.m_Key == null || !(this.m_SelectedObject is ObjectMeshInfo selectedObject))
        return false;
      Match match1 = Regex.Match(widget.path.m_Key, "^EmissiveProperties.m_SingleLights\\[(\\d+)\\].");
      if (match1.Success && int.TryParse(match1.Groups[1].Value, out singleLightIndex))
      {
        mesh = selectedObject.m_Mesh;
        return true;
      }
      Match match2 = Regex.Match(widget.path.m_Key, "^EmissiveProperties.m_MultiLights\\[(\\d+)\\].");
      if (!match2.Success || !int.TryParse(match2.Groups[1].Value, out multiLightIndex))
        return false;
      mesh = selectedObject.m_Mesh;
      return true;
    }

    private void OnColorVariationChanged(
      Entity entity,
      RenderPrefabBase mesh,
      int variationSetIndex,
      int colorIndex)
    {
      // ISSUE: variable of a compiler-generated type
      MeshColorSystem existingSystemManaged = this.World.GetExistingSystemManaged<MeshColorSystem>();
      // ISSUE: reference to a compiler-generated method
      existingSystemManaged?.SetOverride(entity, mesh, variationSetIndex);
    }

    private void OnEmissiveChanged(
      Entity entity,
      RenderPrefabBase mesh,
      int singleLightIndex,
      int multiLightIndex)
    {
      // ISSUE: variable of a compiler-generated type
      ProceduralUploadSystem existingSystemManaged = this.World.GetExistingSystemManaged<ProceduralUploadSystem>();
      // ISSUE: reference to a compiler-generated method
      existingSystemManaged?.SetOverride(entity, mesh, singleLightIndex, multiLightIndex);
    }

    private void ShowSaveAssetPanel()
    {
      // ISSUE: reference to a compiler-generated field
      Colossal.Hash128? initialSelected = this.m_SelectedObject is PrefabBase selectedObject ? selectedObject.asset?.guid : new Colossal.Hash128?();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_ASSET", this.GetCustomAssets(), initialSelected, (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveAsset(name, overwriteGuid)), new Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private IEnumerable<AssetItem> GetCustomAssets()
    {
      AssetItem customAsset1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      if (this.m_SelectedObject is PrefabBase selectedObject && InspectorPanelSystem.IsBuiltinAsset((AssetData) selectedObject.asset) && this.TryGetAssetItem(selectedObject.asset, out customAsset1))
        yield return customAsset1;
      foreach (PrefabAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.user.GetAssets<PrefabAsset>())
      {
        AssetItem customAsset2;
        // ISSUE: reference to a compiler-generated method
        if (this.TryGetAssetItem(asset, out customAsset2))
          yield return customAsset2;
      }
    }

    private bool TryGetAssetItem(PrefabAsset asset, out AssetItem item)
    {
      try
      {
        PrefabBase prefab = asset.Load() as PrefabBase;
        if (prefab is RenderPrefabBase)
        {
          item = (AssetItem) null;
          return false;
        }
        SourceMeta meta = asset.GetMeta();
        ref AssetItem local = ref item;
        AssetItem assetItem = new AssetItem();
        assetItem.guid = asset.guid;
        assetItem.fileName = meta.fileName;
        assetItem.displayName = (LocalizedString) meta.fileName;
        // ISSUE: reference to a compiler-generated method
        assetItem.image = (UnityEngine.Object) prefab != (UnityEngine.Object) null ? ImageSystem.GetThumbnail(prefab) : (string) null;
        assetItem.badge = meta.remoteStorageSourceName;
        local = assetItem;
        return true;
      }
      catch (Exception ex)
      {
        this.log.Error(ex);
        item = (AssetItem) null;
      }
      item = (AssetItem) null;
      return false;
    }

    private void OnSaveAsset(string name, Colossal.Hash128? overwriteGuid, Action<PrefabAsset> callback = null)
    {
      this.CloseSubPanel();
      if (overwriteGuid.HasValue)
      {
        GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[OverwriteAsset]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), Array.Empty<LocalizedString>()), (Action<int>) (ret =>
        {
          if (ret != 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.SaveAsset(name, Colossal.IO.AssetDatabase.AssetDatabase.global.GetAsset<PrefabAsset>(overwriteGuid.Value), callback);
        }));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.SaveAsset(name, callback: callback);
      }
    }

    private void SaveAsset(string name, PrefabAsset existing = null, Action<PrefabAsset> callback = null)
    {
      // ISSUE: reference to a compiler-generated field
      PrefabBase prefabBase = this.m_SelectedObject as PrefabBase;
      if ((AssetData) prefabBase.asset != (IAssetData) null && ((AssetData) existing == (IAssetData) null || (AssetData) existing != (IAssetData) prefabBase.asset))
      {
        // ISSUE: reference to a compiler-generated method
        prefabBase = this.DuplicatePrefab(prefabBase);
        // ISSUE: reference to a compiler-generated method
        this.SelectPrefab(prefabBase);
      }
      PrefabAsset asset = existing;
      if ((AssetData) existing != (IAssetData) null)
        existing.SetData((ScriptableObject) prefabBase);
      else
        asset = Colossal.IO.AssetDatabase.AssetDatabase.user.AddAsset(AssetDataPath.Create("StreamingData~/" + name, name ?? ""), (ScriptableObject) prefabBase);
      // ISSUE: reference to a compiler-generated method
      this.SaveIcons(prefabBase, name);
      asset.Save(false);
      // ISSUE: reference to a compiler-generated method
      if (!InspectorPanelSystem.IsBuiltinAsset((AssetData) asset))
      {
        // ISSUE: reference to a compiler-generated method
        this.SaveLocalization(prefabBase, name);
      }
      PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.IMadeThis);
      if (callback == null)
        return;
      callback(asset);
    }

    private void ShowShareAssetPanel()
    {
      // ISSUE: reference to a compiler-generated field
      Colossal.Hash128? initialSelected = this.m_SelectedObject is PrefabBase selectedObject ? selectedObject.asset?.guid : new Colossal.Hash128?();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.activeSubPanel = (IEditorPanel) new SaveAssetPanel((LocalizedString) "Editor.SAVE_SHARE", this.GetCustomAssets(), initialSelected, (SaveAssetPanel.SaveCallback) ((name, overwriteGuid) => this.OnSaveAsset(name, overwriteGuid, (Action<PrefabAsset>) (asset =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_AssetUploadPanel.Show((AssetData) asset);
        // ISSUE: reference to a compiler-generated field
        this.activeSubPanel = (IEditorPanel) this.m_AssetUploadPanel;
      }))), new Action(((EditorPanelSystemBase) this).CloseSubPanel), (LocalizedString) "Editor.SAVE_SHARE");
    }

    private void OnShareAsset(PrefabAsset asset)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AssetUploadPanel.Show((AssetData) asset);
      // ISSUE: reference to a compiler-generated field
      this.activeSubPanel = (IEditorPanel) this.m_AssetUploadPanel;
    }

    private PrefabBase DuplicatePrefab(PrefabBase oldPrefab)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      PrefabBase key = this.m_PrefabSystem.DuplicatePrefab(oldPrefab, oldPrefab.name);
      // ISSUE: variable of a compiler-generated type
      InspectorPanelSystem.LocalizationFields localizationFields;
      // ISSUE: reference to a compiler-generated field
      if (this.m_WipLocalization.TryGetValue(oldPrefab, out localizationFields))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WipLocalization.Add(key, localizationFields.Clone());
      }
      key.asset = (PrefabAsset) null;
      return key;
    }

    public void ShowThumbnailPicker(LoadAssetPanel.LoadCallback callback)
    {
      this.activeSubPanel = (IEditorPanel) new LoadAssetPanel((LocalizedString) "Editor.THUMBNAIL", EditorPrefabUtils.GetUserImages(), (LoadAssetPanel.LoadCallback) (hash =>
      {
        callback(hash);
        this.CloseSubPanel();
      }), new Action(((EditorPanelSystemBase) this).CloseSubPanel));
    }

    private void SaveIcons(PrefabBase prefab, string name)
    {
      System.Collections.Generic.Dictionary<ImageAsset, ImageAsset> dictionary = new System.Collections.Generic.Dictionary<ImageAsset, ImageAsset>();
      foreach (EditorPrefabUtils.IconInfo icon in EditorPrefabUtils.GetIcons(prefab))
      {
        // ISSUE: reference to a compiler-generated method
        if (!InspectorPanelSystem.IsBuiltinAsset((AssetData) icon.m_Asset))
        {
          if (!dictionary.ContainsKey(icon.m_Asset))
          {
            ImageAsset imageAsset = icon.m_Asset.Save(ImageAsset.FileFormat.PNG, AssetDataPath.Create(prefab.asset.subPath, icon.m_Asset.name ?? ""), prefab.asset.database);
            dictionary.Add(icon.m_Asset, imageAsset);
          }
          icon.m_Field.SetValue((object) icon.m_Component, (object) dictionary[icon.m_Asset].ToGlobalUri());
        }
      }
    }

    private void BuildLocalizationFields(PrefabBase prefab, List<IWidget> widgets)
    {
      // ISSUE: variable of a compiler-generated type
      InspectorPanelSystem.LocalizationFields localizationFields;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WipLocalization.TryGetValue(prefab, out localizationFields))
      {
        // ISSUE: object of a compiler-generated type is created
        localizationFields = new InspectorPanelSystem.LocalizationFields()
        {
          m_NameLocalization = new LocalizationField((LocalizedString) "Editor.ASSET_NAME"),
          m_DescriptionLocalization = new LocalizationField((LocalizedString) "Editor.ASSET_DESCRIPTION")
        };
        // ISSUE: reference to a compiler-generated method
        List<LocalizationField.LocalizationFieldEntry> entries1 = this.InitializeLocalization(prefab, "Assets.NAME[" + prefab.name + "]");
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_NameLocalization.Initialize((IEnumerable<LocalizationField.LocalizationFieldEntry>) entries1);
        // ISSUE: reference to a compiler-generated method
        List<LocalizationField.LocalizationFieldEntry> entries2 = this.InitializeLocalization(prefab, "Assets.DESCRIPTION[" + prefab.name + "]");
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_DescriptionLocalization.Initialize((IEnumerable<LocalizationField.LocalizationFieldEntry>) entries2);
        // ISSUE: reference to a compiler-generated field
        this.m_WipLocalization[prefab] = localizationFields;
      }
      List<IWidget> widgetList1 = widgets;
      Game.UI.Widgets.Group group1 = new Game.UI.Widgets.Group();
      group1.displayName = (LocalizedString) "Localized Name";
      // ISSUE: reference to a compiler-generated field
      group1.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) localizationFields.m_NameLocalization
      };
      group1.tooltip = new LocalizedString?((LocalizedString) "Editor.LOCALIZED_NAME_TOOLTIP");
      Game.UI.Widgets.Group group2 = group1;
      widgetList1.Add((IWidget) group2);
      List<IWidget> widgetList2 = widgets;
      Game.UI.Widgets.Group group3 = new Game.UI.Widgets.Group();
      group3.displayName = (LocalizedString) "Localized Description";
      // ISSUE: reference to a compiler-generated field
      group3.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) localizationFields.m_DescriptionLocalization
      };
      group3.tooltip = new LocalizedString?((LocalizedString) "Editor.LOCALIZED_DESCRIPTION_TOOLTIP");
      Game.UI.Widgets.Group group4 = group3;
      widgetList2.Add((IWidget) group4);
    }

    private List<LocalizationField.LocalizationFieldEntry> InitializeLocalization(
      PrefabBase prefab,
      string key)
    {
      List<LocalizationField.LocalizationFieldEntry> localizationFieldEntryList = new List<LocalizationField.LocalizationFieldEntry>();
      if ((AssetData) prefab.asset != (IAssetData) null && prefab.asset.database == Colossal.IO.AssetDatabase.AssetDatabase.user)
      {
        foreach (LocaleAsset localeAsset in EditorPrefabUtils.GetLocaleAssets(prefab))
        {
          string str;
          if (localeAsset.data.entries.TryGetValue(key, out str))
            localizationFieldEntryList.Add(new LocalizationField.LocalizationFieldEntry()
            {
              localeId = localeAsset.localeId,
              text = str
            });
        }
      }
      else
      {
        foreach (LocaleAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<LocaleAsset>(new SearchFilter<LocaleAsset>()))
        {
          string str;
          if (asset.data.entries.TryGetValue(key, out str))
            localizationFieldEntryList.Add(new LocalizationField.LocalizationFieldEntry()
            {
              localeId = asset.localeId,
              text = str
            });
        }
      }
      return localizationFieldEntryList;
    }

    private void SaveLocalization(PrefabBase prefab, string name)
    {
      // ISSUE: variable of a compiler-generated type
      InspectorPanelSystem.LocalizationFields localizationFields;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_WipLocalization.TryGetValue(prefab, out localizationFields))
        return;
      List<LocaleAsset> localeAssetList = new List<LocaleAsset>();
      localeAssetList.AddRange(EditorPrefabUtils.GetLocaleAssets(prefab));
      foreach (LocaleAsset asset in localeAssetList)
        Colossal.IO.AssetDatabase.AssetDatabase.user.DeleteAsset<LocaleAsset>(asset);
      LocalizationManager localizationManager = GameManager.instance.localizationManager;
      System.Collections.Generic.Dictionary<string, LocaleData> localeDatas = new System.Collections.Generic.Dictionary<string, LocaleData>();
      // ISSUE: reference to a compiler-generated field
      localizationFields.m_NameLocalization.BuildLocaleData("Assets.NAME[" + prefab.name + "]", localeDatas, prefab.name);
      // ISSUE: reference to a compiler-generated field
      localizationFields.m_DescriptionLocalization.BuildLocaleData("Assets.DESCRIPTION[" + prefab.name + "]", localeDatas);
      if (prefab is UIAssetMenuPrefab || prefab is ServicePrefab)
      {
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_NameLocalization.BuildLocaleData("Services.NAME[" + prefab.name + "]", localeDatas, prefab.name);
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_DescriptionLocalization.BuildLocaleData("Services.DESCRIPTION[" + prefab.name + "]", localeDatas);
      }
      if (prefab is UIAssetCategoryPrefab)
      {
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_NameLocalization.BuildLocaleData("SubServices.NAME[" + prefab.name + "]", localeDatas, prefab.name);
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_DescriptionLocalization.BuildLocaleData("Assets.SUB_SERVICE_DESCRIPTION[" + prefab.name + "]", localeDatas);
      }
      if (prefab.Has<ServiceUpgrade>())
      {
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_NameLocalization.BuildLocaleData("Assets.UPGRADE_NAME[" + prefab.name + "]", localeDatas, prefab.name);
        // ISSUE: reference to a compiler-generated field
        localizationFields.m_DescriptionLocalization.BuildLocaleData("Assets.UPGRADE_DESCRIPTION[" + prefab.name + "]", localeDatas);
      }
      foreach (LocaleData data in localeDatas.Values)
      {
        LocaleAsset localeAsset = prefab.asset.database.AddAsset<LocaleAsset>(AssetDataPath.Create(prefab.asset.subPath, name + "_" + data.localeId));
        localeAsset.SetData(data, localizationManager.LocaleIdToSystemLanguage(data.localeId), GameManager.instance.localizationManager.GetLocalizedName(data.localeId));
        localeAsset.Save(false);
      }
    }

    [Preserve]
    public InspectorPanelSystem()
    {
    }

    private enum Mode
    {
      Instance,
      Prefab,
    }

    private struct LocalizationFields
    {
      public LocalizationField m_NameLocalization;
      public LocalizationField m_DescriptionLocalization;

      public InspectorPanelSystem.LocalizationFields Clone()
      {
        // ISSUE: reference to a compiler-generated field
        LocalizationField localizationField1 = new LocalizationField(this.m_NameLocalization.placeholder);
        // ISSUE: reference to a compiler-generated field
        localizationField1.Initialize((IEnumerable<LocalizationField.LocalizationFieldEntry>) this.m_NameLocalization.localization);
        // ISSUE: reference to a compiler-generated field
        LocalizationField localizationField2 = new LocalizationField(this.m_DescriptionLocalization.placeholder);
        // ISSUE: reference to a compiler-generated field
        localizationField2.Initialize((IEnumerable<LocalizationField.LocalizationFieldEntry>) this.m_DescriptionLocalization.localization);
        // ISSUE: object of a compiler-generated type is created
        return new InspectorPanelSystem.LocalizationFields()
        {
          m_NameLocalization = localizationField1,
          m_DescriptionLocalization = localizationField2
        };
      }
    }
  }
}
