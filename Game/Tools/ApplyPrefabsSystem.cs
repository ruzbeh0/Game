// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyPrefabsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyPrefabsSystem : GameSystemBase
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_SaveInstanceQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SaveInstanceQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.Object>(), ComponentType.ReadOnly<SaveInstance>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SaveInstanceQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_SaveInstanceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.RemoveComponent<SaveInstance>(this.m_SaveInstanceQuery);
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          EntityManager entityManager = this.EntityManager;
          entityManager.RemoveComponent<SaveInstance>(entity);
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Game.Objects.SubObject>(entity))
          {
            entityManager = this.EntityManager;
            if (!entityManager.HasComponent<Game.Net.SubNet>(entity))
            {
              entityManager = this.EntityManager;
              if (!entityManager.HasComponent<Game.Areas.SubArea>(entity))
                continue;
            }
          }
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(entityManager.GetComponentData<PrefabRef>(entity));
          uint constructionCost = 0;
          uint upKeepCost = 0;
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectSubObjects(entity, prefab, ref constructionCost, ref upKeepCost);
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectSubNets(entity, prefab, ref constructionCost, ref upKeepCost);
          // ISSUE: reference to a compiler-generated method
          this.UpdateObjectSubAreas(entity, prefab, ref constructionCost, ref upKeepCost);
          if (prefab is AssetStampPrefab)
          {
            AssetStampPrefab assetStampPrefab = prefab as AssetStampPrefab;
            assetStampPrefab.m_ConstructionCost = constructionCost;
            assetStampPrefab.m_UpKeepCost = upKeepCost;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabSystem.UpdatePrefab(prefab, entity);
          prefab.asset?.MarkDirty();
        }
      }
    }

    private void UpdateObjectSubObjects(
      Entity instanceEntity,
      PrefabBase prefabBase,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      List<ObjectSubObjectInfo> subObjectList;
      List<EffectSource.EffectSettings> subEffectList;
      List<Game.Prefabs.ActivityLocation.LocationInfo> subActivityList;
      // ISSUE: reference to a compiler-generated method
      this.ListObjectSubObjects(instanceEntity, out subObjectList, out subEffectList, out subActivityList, ref constructionCost, ref upKeepCost);
      if (subObjectList != null && subObjectList.Count != 0)
      {
        ObjectSubObjects objectSubObjects = prefabBase.GetComponent<ObjectSubObjects>();
        if ((UnityEngine.Object) objectSubObjects == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          objectSubObjects = ApplyPrefabsSystem.AddComponent<ObjectSubObjects>(prefabBase);
        }
        objectSubObjects.m_SubObjects = subObjectList.ToArray();
      }
      else if ((UnityEngine.Object) prefabBase.GetComponent<ObjectSubObjects>() != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<ObjectSubObjects>(prefabBase);
      }
      if (subEffectList != null && subEffectList.Count != 0)
      {
        EffectSource effectSource = prefabBase.GetComponent<EffectSource>();
        if ((UnityEngine.Object) effectSource == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          effectSource = ApplyPrefabsSystem.AddComponent<EffectSource>(prefabBase);
        }
        effectSource.m_Effects = new List<EffectSource.EffectSettings>();
        effectSource.m_Effects.AddRange((IEnumerable<EffectSource.EffectSettings>) subEffectList);
      }
      else if ((UnityEngine.Object) prefabBase.GetComponent<EffectSource>() != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<EffectSource>(prefabBase);
      }
      if (subActivityList != null && subActivityList.Count != 0)
      {
        Game.Prefabs.ActivityLocation activityLocation = prefabBase.GetComponent<Game.Prefabs.ActivityLocation>();
        if ((UnityEngine.Object) activityLocation == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          activityLocation = ApplyPrefabsSystem.AddComponent<Game.Prefabs.ActivityLocation>(prefabBase);
        }
        activityLocation.m_Locations = subActivityList.ToArray();
      }
      else
      {
        if (!((UnityEngine.Object) prefabBase.GetComponent<Game.Prefabs.ActivityLocation>() != (UnityEngine.Object) null))
          return;
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<Game.Prefabs.ActivityLocation>(prefabBase);
      }
    }

    private void ListObjectSubObjects(
      Entity instanceEntity,
      out List<ObjectSubObjectInfo> subObjectList,
      out List<EffectSource.EffectSettings> subEffectList,
      out List<Game.Prefabs.ActivityLocation.LocationInfo> subActivityList,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      if (this.EntityManager.HasComponent<Game.Objects.SubObject>(instanceEntity))
      {
        DynamicBuffer<Game.Objects.SubObject> buffer = this.EntityManager.GetBuffer<Game.Objects.SubObject>(instanceEntity, true);
        if (buffer.Length != 0)
        {
          Game.Objects.Transform inverseParentTransform = new Game.Objects.Transform();
          bool flag = false;
          Game.Objects.Transform component1;
          if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(instanceEntity, out component1))
          {
            inverseParentTransform.m_Position = -component1.m_Position;
            inverseParentTransform.m_Rotation = math.inverse(component1.m_Rotation);
            flag = true;
          }
          subObjectList = new List<ObjectSubObjectInfo>(buffer.Length);
          subEffectList = new List<EffectSource.EffectSettings>(buffer.Length);
          subActivityList = new List<Game.Prefabs.ActivityLocation.LocationInfo>(buffer.Length);
          for (int index = 0; index < buffer.Length; ++index)
          {
            Entity subObject = buffer[index].m_SubObject;
            Owner component2;
            if (this.EntityManager.TryGetComponent<Owner>(subObject, out component2) && !this.EntityManager.HasComponent<Secondary>(subObject) && !this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(subObject) && component2.m_Owner == instanceEntity)
            {
              int num1 = 0;
              int num2 = 0;
              Game.Objects.Transform component3;
              float3 float3;
              quaternion quaternion;
              int num3;
              if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(subObject, out component3))
              {
                if (flag)
                {
                  Game.Objects.Transform local = ObjectUtils.WorldToLocal(inverseParentTransform, component3);
                  float3 = local.m_Position;
                  quaternion = local.m_Rotation;
                }
                else
                {
                  float3 = component3.m_Position;
                  quaternion = component3.m_Rotation;
                }
                LocalTransformCache component4;
                if (this.EntityManager.TryGetComponent<LocalTransformCache>(subObject, out component4))
                {
                  // ISSUE: reference to a compiler-generated method
                  ApplyPrefabsSystem.CheckCachedValue(ref float3, component4.m_Position);
                  // ISSUE: reference to a compiler-generated method
                  ApplyPrefabsSystem.CheckCachedValue(ref quaternion, component4.m_Rotation);
                  num3 = component4.m_ParentMesh;
                  num1 = component4.m_GroupIndex;
                  num2 = component4.m_Probability;
                  EditorContainer component5;
                  if (this.EntityManager.TryGetComponent<EditorContainer>(subObject, out component5))
                  {
                    if (this.EntityManager.HasComponent<EffectData>(component5.m_Prefab))
                      component4.m_PrefabSubIndex = subEffectList.Count;
                    else if (this.EntityManager.HasComponent<ActivityLocationData>(component5.m_Prefab))
                      component4.m_PrefabSubIndex = subActivityList.Count;
                  }
                  else
                    component4.m_PrefabSubIndex = subObjectList.Count;
                  this.EntityManager.SetComponentData<LocalTransformCache>(subObject, component4);
                }
                else
                {
                  Game.Objects.Elevation component6;
                  num3 = !this.EntityManager.TryGetComponent<Game.Objects.Elevation>(subObject, out component6) ? -1 : ObjectUtils.GetSubParentMesh(component6.m_Flags);
                }
              }
              else
              {
                float3 = float3.zero;
                quaternion = quaternion.identity;
                num3 = -1;
              }
              EditorContainer component7;
              if (this.EntityManager.TryGetComponent<EditorContainer>(subObject, out component7))
              {
                if (this.EntityManager.HasComponent<EffectData>(component7.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  subEffectList.Add(new EffectSource.EffectSettings()
                  {
                    m_Effect = this.m_PrefabSystem.GetPrefab<EffectPrefab>(component7.m_Prefab),
                    m_PositionOffset = float3,
                    m_Rotation = quaternion,
                    m_Scale = component7.m_Scale,
                    m_Intensity = component7.m_Intensity,
                    m_ParentMesh = num3,
                    m_AnimationIndex = component7.m_GroupIndex
                  });
                }
                else if (this.EntityManager.HasComponent<ActivityLocationData>(component7.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  subActivityList.Add(new Game.Prefabs.ActivityLocation.LocationInfo()
                  {
                    m_Activity = this.m_PrefabSystem.GetPrefab<ActivityLocationPrefab>(component7.m_Prefab),
                    m_Position = float3,
                    m_Rotation = quaternion
                  });
                }
              }
              else
              {
                ObjectSubObjectInfo objectSubObjectInfo = new ObjectSubObjectInfo();
                PrefabRef componentData = this.EntityManager.GetComponentData<PrefabRef>(subObject);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                objectSubObjectInfo.m_Object = this.m_PrefabSystem.GetPrefab<ObjectPrefab>(componentData);
                PlaceableObjectData component8;
                if (this.EntityManager.TryGetComponent<PlaceableObjectData>(componentData.m_Prefab, out component8))
                  constructionCost += component8.m_ConstructionCost;
                ConsumptionData component9;
                if (this.EntityManager.TryGetComponent<ConsumptionData>(componentData.m_Prefab, out component9))
                  upKeepCost += (uint) component9.m_Upkeep;
                objectSubObjectInfo.m_Position = float3;
                objectSubObjectInfo.m_Rotation = quaternion;
                objectSubObjectInfo.m_ParentMesh = num3;
                objectSubObjectInfo.m_GroupIndex = num1;
                objectSubObjectInfo.m_Probability = num2;
                subObjectList.Add(objectSubObjectInfo);
              }
            }
          }
          return;
        }
      }
      subObjectList = (List<ObjectSubObjectInfo>) null;
      subEffectList = (List<EffectSource.EffectSettings>) null;
      subActivityList = (List<Game.Prefabs.ActivityLocation.LocationInfo>) null;
    }

    private void UpdateObjectSubNets(
      Entity instanceEntity,
      PrefabBase prefabBase,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      List<ObjectSubNetInfo> subNetList;
      List<ObjectSubLaneInfo> subLaneList;
      // ISSUE: reference to a compiler-generated method
      this.ListObjectSubNets(instanceEntity, out subNetList, out subLaneList, ref constructionCost, ref upKeepCost);
      if (subNetList != null && subNetList.Count != 0)
      {
        ObjectSubNets objectSubNets = prefabBase.GetComponent<ObjectSubNets>();
        if ((UnityEngine.Object) objectSubNets == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          objectSubNets = ApplyPrefabsSystem.AddComponent<ObjectSubNets>(prefabBase);
        }
        // ISSUE: reference to a compiler-generated field
        if (objectSubNets.m_InvertWhen == NetInvertMode.LefthandTraffic && this.m_CityConfigurationSystem.leftHandTraffic)
        {
          objectSubNets.m_InvertWhen = NetInvertMode.RighthandTraffic;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (objectSubNets.m_InvertWhen == NetInvertMode.RighthandTraffic && !this.m_CityConfigurationSystem.leftHandTraffic)
            objectSubNets.m_InvertWhen = NetInvertMode.LefthandTraffic;
        }
        objectSubNets.m_SubNets = subNetList.ToArray();
      }
      else if ((UnityEngine.Object) prefabBase.GetComponent<ObjectSubNets>() != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<ObjectSubNets>(prefabBase);
      }
      if (subLaneList != null && subLaneList.Count != 0)
      {
        ObjectSubLanes objectSubLanes = prefabBase.GetComponent<ObjectSubLanes>();
        if ((UnityEngine.Object) objectSubLanes == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          objectSubLanes = ApplyPrefabsSystem.AddComponent<ObjectSubLanes>(prefabBase);
        }
        objectSubLanes.m_SubLanes = subLaneList.ToArray();
      }
      else
      {
        if (!((UnityEngine.Object) prefabBase.GetComponent<ObjectSubLanes>() != (UnityEngine.Object) null))
          return;
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<ObjectSubLanes>(prefabBase);
      }
    }

    private NetPieceRequirements[] CreateRequirementMap()
    {
      NetPieceRequirements[] requirementMap = new NetPieceRequirements[96];
      foreach (NetPieceRequirements requirement in Enum.GetValues(typeof (NetPieceRequirements)))
      {
        CompositionFlags compositionFlags = new CompositionFlags();
        NetSectionFlags sectionFlags = (NetSectionFlags) 0;
        NetCompositionHelpers.GetRequirementFlags(requirement, ref compositionFlags, ref sectionFlags);
        if (compositionFlags.m_Left != (CompositionFlags.Side) 0)
        {
          for (int index = 0; index < 32; ++index)
          {
            if ((compositionFlags.m_Left & (CompositionFlags.Side) (1 << index)) != (CompositionFlags.Side) 0)
              requirementMap[index] = requirement;
          }
        }
        if (compositionFlags.m_General != (CompositionFlags.General) 0)
        {
          for (int index = 0; index < 32; ++index)
          {
            if ((compositionFlags.m_General & (CompositionFlags.General) (1 << index)) != (CompositionFlags.General) 0)
              requirementMap[index + 32] = requirement;
          }
        }
        if (compositionFlags.m_Right != (CompositionFlags.Side) 0)
        {
          for (int index = 0; index < 32; ++index)
          {
            if ((compositionFlags.m_Right & (CompositionFlags.Side) (1 << index)) != (CompositionFlags.Side) 0)
              requirementMap[index + 64] = requirement;
          }
        }
      }
      return requirementMap;
    }

    private NetPieceRequirements[] CreateRequirementArray(
      NetPieceRequirements[] requirementMap,
      CompositionFlags flags)
    {
      List<NetPieceRequirements> pieceRequirementsList = new List<NetPieceRequirements>(10);
      if (flags.m_Left != (CompositionFlags.Side) 0)
      {
        for (int index = 0; index < 32; ++index)
        {
          if ((flags.m_Left & (CompositionFlags.Side) (1 << index)) != (CompositionFlags.Side) 0)
            pieceRequirementsList.Add(requirementMap[index]);
        }
      }
      if (flags.m_General != (CompositionFlags.General) 0)
      {
        for (int index = 0; index < 32; ++index)
        {
          if ((flags.m_General & (CompositionFlags.General) (1 << index)) != (CompositionFlags.General) 0)
            pieceRequirementsList.Add(requirementMap[index + 32]);
        }
      }
      if (flags.m_Right != (CompositionFlags.Side) 0)
      {
        for (int index = 0; index < 32; ++index)
        {
          if ((flags.m_Right & (CompositionFlags.Side) (1 << index)) != (CompositionFlags.Side) 0)
            pieceRequirementsList.Add(requirementMap[index + 64]);
        }
      }
      return pieceRequirementsList.Count != 0 ? pieceRequirementsList.ToArray() : (NetPieceRequirements[]) null;
    }

    private void ListObjectSubNets(
      Entity instanceEntity,
      out List<ObjectSubNetInfo> subNetList,
      out List<ObjectSubLaneInfo> subLaneList,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      if (this.EntityManager.HasComponent<Game.Net.SubNet>(instanceEntity))
      {
        DynamicBuffer<Game.Net.SubNet> buffer1 = this.EntityManager.GetBuffer<Game.Net.SubNet>(instanceEntity, true);
        if (buffer1.Length != 0)
        {
          Game.Objects.Transform transform = new Game.Objects.Transform();
          bool flag = false;
          Game.Objects.Transform component1;
          if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(instanceEntity, out component1))
          {
            transform.m_Position = -component1.m_Position;
            transform.m_Rotation = math.inverse(component1.m_Rotation);
            flag = true;
          }
          subNetList = new List<ObjectSubNetInfo>(buffer1.Length);
          subLaneList = new List<ObjectSubLaneInfo>(buffer1.Length);
          Dictionary<Entity, int> dictionary1 = new Dictionary<Entity, int>(buffer1.Length * 2);
          Dictionary<Entity, int> dictionary2 = new Dictionary<Entity, int>(buffer1.Length * 2);
          NetPieceRequirements[] requirementMap = (NetPieceRequirements[]) null;
          for (int index = 0; index < buffer1.Length; ++index)
          {
            Entity subNet = buffer1[index].m_SubNet;
            Owner component2;
            if (this.EntityManager.TryGetComponent<Owner>(subNet, out component2) && component2.m_Owner == instanceEntity)
            {
              NetPieceRequirements[] pieceRequirementsArray = (NetPieceRequirements[]) null;
              CompositionFlags compositionFlags = new CompositionFlags();
              EntityManager entityManager = this.EntityManager;
              Composition component3;
              NetCompositionData component4;
              if (entityManager.HasComponent<Game.Net.Edge>(subNet) && this.EntityManager.TryGetComponent<Composition>(subNet, out component3) && this.EntityManager.TryGetComponent<NetCompositionData>(component3.m_Edge, out component4))
                compositionFlags.m_General |= component4.m_Flags.m_General & CompositionFlags.General.Elevated;
              Upgraded component5;
              if (this.EntityManager.TryGetComponent<Upgraded>(subNet, out component5) || compositionFlags != new CompositionFlags())
              {
                if (requirementMap == null)
                {
                  // ISSUE: reference to a compiler-generated method
                  requirementMap = this.CreateRequirementMap();
                }
                // ISSUE: reference to a compiler-generated method
                pieceRequirementsArray = this.CreateRequirementArray(requirementMap, component5.m_Flags | compositionFlags);
              }
              Game.Net.Edge component6;
              if (this.EntityManager.TryGetComponent<Game.Net.Edge>(subNet, out component6))
              {
                Bezier4x3 bezier4x3 = new Bezier4x3();
                Curve component7;
                if (this.EntityManager.TryGetComponent<Curve>(subNet, out component7))
                {
                  if (flag)
                  {
                    bezier4x3.a = math.mul(transform.m_Rotation, component7.m_Bezier.a + transform.m_Position);
                    bezier4x3.b = math.mul(transform.m_Rotation, component7.m_Bezier.b + transform.m_Position);
                    bezier4x3.c = math.mul(transform.m_Rotation, component7.m_Bezier.c + transform.m_Position);
                    bezier4x3.d = math.mul(transform.m_Rotation, component7.m_Bezier.d + transform.m_Position);
                  }
                  else
                    bezier4x3 = component7.m_Bezier;
                  LocalCurveCache component8;
                  if (this.EntityManager.TryGetComponent<LocalCurveCache>(subNet, out component8))
                  {
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref bezier4x3.a, component8.m_Curve.a);
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref bezier4x3.b, component8.m_Curve.b);
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref bezier4x3.c, component8.m_Curve.c);
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref bezier4x3.d, component8.m_Curve.d);
                  }
                  Composition component9;
                  PlaceableNetComposition component10;
                  if (this.EntityManager.TryGetComponent<Composition>(subNet, out component9) && this.EntityManager.TryGetComponent<PlaceableNetComposition>(component9.m_Edge, out component10))
                  {
                    Game.Net.Elevation component11;
                    this.EntityManager.TryGetComponent<Game.Net.Elevation>(component6.m_Start, out component11);
                    Game.Net.Elevation component12;
                    this.EntityManager.TryGetComponent<Game.Net.Elevation>(component6.m_End, out component12);
                    constructionCost += (uint) NetUtils.GetConstructionCost(component7, component11, component12, component10);
                    upKeepCost += (uint) NetUtils.GetUpkeepCost(component7, component10);
                  }
                }
                EditorContainer component13;
                if (this.EntityManager.TryGetComponent<EditorContainer>(subNet, out component13))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  subLaneList.Add(new ObjectSubLaneInfo()
                  {
                    m_LanePrefab = this.m_PrefabSystem.GetPrefab<NetLanePrefab>(component13.m_Prefab),
                    m_BezierCurve = bezier4x3,
                    m_NodeIndex = new int2(this.GetNodeIndex(component6.m_Start, dictionary2), this.GetNodeIndex(component6.m_End, dictionary2)),
                    m_ParentMesh = new int2(this.GetParentMesh(component6.m_Start), this.GetParentMesh(component6.m_End))
                  });
                }
                else
                {
                  List<ObjectSubNetInfo> objectSubNetInfoList = subNetList;
                  ObjectSubNetInfo objectSubNetInfo = new ObjectSubNetInfo();
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: variable of a compiler-generated type
                  PrefabSystem prefabSystem = this.m_PrefabSystem;
                  entityManager = this.EntityManager;
                  PrefabRef componentData = entityManager.GetComponentData<PrefabRef>(subNet);
                  // ISSUE: reference to a compiler-generated method
                  objectSubNetInfo.m_NetPrefab = prefabSystem.GetPrefab<NetPrefab>(componentData);
                  objectSubNetInfo.m_BezierCurve = bezier4x3;
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  objectSubNetInfo.m_NodeIndex = new int2(this.GetNodeIndex(component6.m_Start, dictionary1), this.GetNodeIndex(component6.m_End, dictionary1));
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  objectSubNetInfo.m_ParentMesh = new int2(this.GetParentMesh(component6.m_Start), this.GetParentMesh(component6.m_End));
                  objectSubNetInfo.m_Upgrades = pieceRequirementsArray;
                  objectSubNetInfoList.Add(objectSubNetInfo);
                }
              }
              else
              {
                Game.Net.Node component14;
                DynamicBuffer<ConnectedEdge> buffer2;
                // ISSUE: reference to a compiler-generated method
                if (this.EntityManager.TryGetComponent<Game.Net.Node>(subNet, out component14) && (pieceRequirementsArray != null || !this.EntityManager.TryGetBuffer<ConnectedEdge>(subNet, true, out buffer2) || !this.HasEdgeStartOrEnd(buffer2, subNet, instanceEntity)))
                {
                  Bezier4x3 bezier4x3 = new Bezier4x3();
                  bezier4x3.a = !flag ? component14.m_Position : math.mul(transform.m_Rotation, component14.m_Position + transform.m_Position);
                  LocalTransformCache component15;
                  if (this.EntityManager.TryGetComponent<LocalTransformCache>(subNet, out component15))
                  {
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref bezier4x3.a, component15.m_Position);
                  }
                  bezier4x3.b = bezier4x3.a;
                  bezier4x3.c = bezier4x3.a;
                  bezier4x3.d = bezier4x3.a;
                  EditorContainer component16;
                  if (this.EntityManager.TryGetComponent<EditorContainer>(subNet, out component16))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: reference to a compiler-generated method
                    subLaneList.Add(new ObjectSubLaneInfo()
                    {
                      m_LanePrefab = this.m_PrefabSystem.GetPrefab<NetLanePrefab>(component16.m_Prefab),
                      m_BezierCurve = bezier4x3,
                      m_NodeIndex = new int2(this.GetNodeIndex(subNet, dictionary2)),
                      m_ParentMesh = new int2(this.GetParentMesh(subNet))
                    });
                  }
                  else
                  {
                    List<ObjectSubNetInfo> objectSubNetInfoList = subNetList;
                    ObjectSubNetInfo objectSubNetInfo = new ObjectSubNetInfo();
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: variable of a compiler-generated type
                    PrefabSystem prefabSystem = this.m_PrefabSystem;
                    entityManager = this.EntityManager;
                    PrefabRef componentData = entityManager.GetComponentData<PrefabRef>(subNet);
                    // ISSUE: reference to a compiler-generated method
                    objectSubNetInfo.m_NetPrefab = prefabSystem.GetPrefab<NetPrefab>(componentData);
                    objectSubNetInfo.m_BezierCurve = bezier4x3;
                    // ISSUE: reference to a compiler-generated method
                    objectSubNetInfo.m_NodeIndex = new int2(this.GetNodeIndex(subNet, dictionary1));
                    // ISSUE: reference to a compiler-generated method
                    objectSubNetInfo.m_ParentMesh = new int2(this.GetParentMesh(subNet));
                    objectSubNetInfo.m_Upgrades = pieceRequirementsArray;
                    objectSubNetInfoList.Add(objectSubNetInfo);
                  }
                }
              }
            }
          }
          return;
        }
      }
      subNetList = (List<ObjectSubNetInfo>) null;
      subLaneList = (List<ObjectSubLaneInfo>) null;
    }

    private int GetNodeIndex(Entity node, Dictionary<Entity, int> dictionary)
    {
      int count;
      if (!dictionary.TryGetValue(node, out count))
      {
        count = dictionary.Count;
        dictionary.Add(node, count);
      }
      return count;
    }

    private int GetParentMesh(Entity node)
    {
      LocalTransformCache component;
      return this.EntityManager.TryGetComponent<LocalTransformCache>(node, out component) ? component.m_ParentMesh : -1;
    }

    private bool HasEdgeStartOrEnd(
      DynamicBuffer<ConnectedEdge> connectedEdges,
      Entity node,
      Entity instanceEntity)
    {
      for (int index = 0; index < connectedEdges.Length; ++index)
      {
        Entity edge = connectedEdges[index].m_Edge;
        Game.Net.Edge component1;
        Owner component2;
        if (this.EntityManager.TryGetComponent<Game.Net.Edge>(edge, out component1) && (component1.m_Start == node || component1.m_End == node) && this.EntityManager.TryGetComponent<Owner>(edge, out component2) && component2.m_Owner == instanceEntity)
          return true;
      }
      return false;
    }

    private void UpdateObjectSubAreas(
      Entity instanceEntity,
      PrefabBase prefabBase,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      // ISSUE: reference to a compiler-generated method
      List<ObjectSubAreaInfo> objectSubAreaInfoList = this.ListObjectSubAreas(instanceEntity, ref constructionCost, ref upKeepCost);
      if (objectSubAreaInfoList != null && objectSubAreaInfoList.Count != 0)
      {
        ObjectSubAreas objectSubAreas = prefabBase.GetComponent<ObjectSubAreas>();
        if ((UnityEngine.Object) objectSubAreas == (UnityEngine.Object) null)
        {
          // ISSUE: reference to a compiler-generated method
          objectSubAreas = ApplyPrefabsSystem.AddComponent<ObjectSubAreas>(prefabBase);
        }
        objectSubAreas.m_SubAreas = objectSubAreaInfoList.ToArray();
      }
      else
      {
        if (!((UnityEngine.Object) prefabBase.GetComponent<ObjectSubAreas>() != (UnityEngine.Object) null))
          return;
        // ISSUE: reference to a compiler-generated method
        ApplyPrefabsSystem.RemoveComponent<ObjectSubAreas>(prefabBase);
      }
    }

    private List<ObjectSubAreaInfo> ListObjectSubAreas(
      Entity instanceEntity,
      ref uint constructionCost,
      ref uint upKeepCost)
    {
      if (this.EntityManager.HasComponent<Game.Areas.SubArea>(instanceEntity))
      {
        DynamicBuffer<Game.Areas.SubArea> buffer1 = this.EntityManager.GetBuffer<Game.Areas.SubArea>(instanceEntity, true);
        if (buffer1.Length != 0)
        {
          Game.Objects.Transform inverseParentTransform = new Game.Objects.Transform();
          bool flag1 = false;
          Game.Objects.Transform component1;
          if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(instanceEntity, out component1))
          {
            inverseParentTransform.m_Position = -component1.m_Position;
            inverseParentTransform.m_Rotation = math.inverse(component1.m_Rotation);
            flag1 = true;
          }
          List<ObjectSubAreaInfo> objectSubAreaInfoList = new List<ObjectSubAreaInfo>(buffer1.Length);
          for (int index1 = 0; index1 < buffer1.Length; ++index1)
          {
            Entity area = buffer1[index1].m_Area;
            Owner component2;
            if (this.EntityManager.TryGetComponent<Owner>(area, out component2) && !this.EntityManager.HasComponent<Secondary>(area) && component2.m_Owner == instanceEntity)
            {
              ObjectSubAreaInfo objectSubAreaInfo = new ObjectSubAreaInfo();
              PrefabRef componentData = this.EntityManager.GetComponentData<PrefabRef>(area);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              objectSubAreaInfo.m_AreaPrefab = this.m_PrefabSystem.GetPrefab<AreaPrefab>(componentData);
              DynamicBuffer<Game.Areas.Node> buffer2;
              if (this.EntityManager.TryGetBuffer<Game.Areas.Node>(area, true, out buffer2))
              {
                DynamicBuffer<LocalNodeCache> dynamicBuffer = new DynamicBuffer<LocalNodeCache>();
                if (this.EntityManager.HasComponent<LocalNodeCache>(area))
                  dynamicBuffer = this.EntityManager.GetBuffer<LocalNodeCache>(area, true);
                objectSubAreaInfo.m_NodePositions = new float3[buffer2.Length];
                objectSubAreaInfo.m_ParentMeshes = new int[buffer2.Length];
                bool flag2 = false;
                for (int index2 = 0; index2 < buffer2.Length; ++index2)
                {
                  objectSubAreaInfo.m_NodePositions[index2] = !flag1 ? buffer2[index2].m_Position : ObjectUtils.WorldToLocal(inverseParentTransform, buffer2[index2].m_Position);
                  if (dynamicBuffer.IsCreated)
                  {
                    // ISSUE: reference to a compiler-generated method
                    ApplyPrefabsSystem.CheckCachedValue(ref objectSubAreaInfo.m_NodePositions[index2], dynamicBuffer[index2].m_Position);
                    objectSubAreaInfo.m_ParentMeshes[index2] = dynamicBuffer[index2].m_ParentMesh;
                    flag2 |= dynamicBuffer[index2].m_ParentMesh >= 0;
                  }
                  else
                    objectSubAreaInfo.m_ParentMeshes[index2] = -1;
                }
                if (!flag2)
                  objectSubAreaInfo.m_ParentMeshes = (int[]) null;
              }
              objectSubAreaInfoList.Add(objectSubAreaInfo);
            }
          }
          return objectSubAreaInfoList;
        }
      }
      return (List<ObjectSubAreaInfo>) null;
    }

    private static void CheckCachedValue(ref float3 value, float3 cached)
    {
      if ((double) math.distance(value, cached) >= 0.0099999997764825821)
        return;
      value = cached;
    }

    private static void CheckCachedValue(ref quaternion value, quaternion cached)
    {
      if ((double) MathUtils.RotationAngle(value, cached) >= Math.PI / 180.0)
        return;
      value = cached;
    }

    public static T AddComponent<T>(PrefabBase asset) where T : ComponentBase
    {
      return asset.AddComponent<T>();
    }

    public static void RemoveComponent<T>(PrefabBase asset) where T : ComponentBase
    {
      // ISSUE: reference to a compiler-generated method
      ApplyPrefabsSystem.RemoveComponent(asset, typeof (T));
    }

    public static void RemoveComponent(PrefabBase asset, System.Type componentType)
    {
      ComponentBase componentExactly = asset.GetComponentExactly(componentType);
      if ((UnityEngine.Object) componentExactly == (UnityEngine.Object) null)
        return;
      asset.Remove(componentType);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) componentExactly, true);
    }

    [Preserve]
    public ApplyPrefabsSystem()
    {
    }
  }
}
