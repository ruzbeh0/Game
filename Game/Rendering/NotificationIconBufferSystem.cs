// Decompiled with JetBrains decompiler
// Type: Game.Rendering.NotificationIconBufferSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class NotificationIconBufferSystem : GameSystemBase
  {
    private EntityQuery m_IconQuery;
    private EntityQuery m_ConfigurationQuery;
    private IconClusterSystem m_IconClusterSystem;
    private ToolSystem m_ToolSystem;
    private PrefabSystem m_PrefabSystem;
    private NativeList<NotificationIconBufferSystem.InstanceData> m_InstanceData;
    private NativeValue<Bounds3> m_IconBounds;
    private JobHandle m_InstanceDataDeps;
    private NotificationIconBufferSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconClusterSystem = this.World.GetOrCreateSystemManaged<IconClusterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Icon>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<DisallowCluster>(),
          ComponentType.ReadOnly<Game.Notifications.Animation>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Hidden>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<IconConfigurationData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceData.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceDataDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceData.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_IconBounds.Dispose();
      }
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      Camera main = Camera.main;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) main == (UnityEngine.Object) null || this.m_ConfigurationQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InstanceData.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceData = new NativeList<NotificationIconBufferSystem.InstanceData>(64, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
        // ISSUE: reference to a compiler-generated field
        this.m_IconBounds = new NativeValue<Bounds3>(Allocator.Persistent);
      }
      UnityEngine.Transform transform = main.transform;
      uint num = uint.MaxValue;
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_ToolSystem.activeInfoview != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        num = this.m_PrefabSystem.GetComponentData<InfoviewData>((PrefabBase) this.m_ToolSystem.activeInfoview).m_NotificationMask | 2147483648U;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceDataDeps.Complete();
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_IconQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IconAnimationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_DisallowCluster_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NotificationIconBufferSystem.NotificationIconBufferJob jobData1 = new NotificationIconBufferSystem.NotificationIconBufferJob()
      {
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_HiddenType = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_IconDisplayData = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_AnimationData = this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentLookup,
        m_DisallowClusterData = this.__TypeHandle.__Game_Notifications_DisallowCluster_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_IconAnimations = this.__TypeHandle.__Game_Prefabs_IconAnimationElement_RO_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_CameraPosition = (float3) transform.position,
        m_CameraUp = (float3) transform.up,
        m_CameraRight = (float3) transform.right,
        m_ConfigurationEntity = this.m_ConfigurationQuery.GetSingletonEntity(),
        m_CategoryMask = num,
        m_ClusterData = this.m_IconClusterSystem.GetIconClusterData(false, out dependencies),
        m_InstanceData = this.m_InstanceData,
        m_IconBounds = this.m_IconBounds
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NotificationIconBufferSystem.NotificationIconSortJob jobData2 = new NotificationIconBufferSystem.NotificationIconSortJob()
      {
        m_InstanceData = this.m_InstanceData
      };
      JobHandle jobHandle1 = jobData1.Schedule<NotificationIconBufferSystem.NotificationIconBufferJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      JobHandle dependsOn = jobHandle1;
      JobHandle jobHandle2 = jobData2.Schedule<NotificationIconBufferSystem.NotificationIconSortJob>(dependsOn);
      archetypeChunkListAsync.Dispose(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconClusterSystem.AddIconClusterWriter(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceDataDeps = jobHandle2;
      this.Dependency = jobHandle1;
    }

    public NotificationIconBufferSystem.IconData GetIconData()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceDataDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceDataDeps = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceData.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new NotificationIconBufferSystem.IconData()
        {
          m_InstanceData = this.m_InstanceData.AsArray(),
          m_IconBounds = this.m_IconBounds
        };
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NotificationIconBufferSystem.IconData iconData = new NotificationIconBufferSystem.IconData();
      return iconData;
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

    [UnityEngine.Scripting.Preserve]
    public NotificationIconBufferSystem()
    {
    }

    public struct IconData
    {
      public NativeArray<NotificationIconBufferSystem.InstanceData> m_InstanceData;
      public NativeValue<Bounds3> m_IconBounds;
    }

    public struct InstanceData : IComparable<NotificationIconBufferSystem.InstanceData>
    {
      public float3 m_Position;
      public float4 m_Params;
      public float m_Icon;
      public float m_Distance;

      public int CompareTo(NotificationIconBufferSystem.InstanceData other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (int) math.sign(other.m_Distance - this.m_Distance);
      }
    }

    private struct HiddenPositionData
    {
      public float3 m_Position;
      public float m_Radius;
      public float m_Distance;
    }

    [BurstCompile]
    private struct NotificationIconBufferJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Notifications.Animation> m_AnimationType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> m_HiddenType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> m_IconDisplayData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<Game.Notifications.Animation> m_AnimationData;
      [ReadOnly]
      public ComponentLookup<DisallowCluster> m_DisallowClusterData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public BufferLookup<IconAnimationElement> m_IconAnimations;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public float3 m_CameraPosition;
      [ReadOnly]
      public float3 m_CameraUp;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public Entity m_ConfigurationEntity;
      [ReadOnly]
      public uint m_CategoryMask;
      public IconClusterSystem.ClusterData m_ClusterData;
      public NativeList<NotificationIconBufferSystem.InstanceData> m_InstanceData;
      public NativeValue<Bounds3> m_IconBounds;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceData.Clear();
        Bounds3 bounds3 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<IconAnimationElement> iconAnimation = this.m_IconAnimations[this.m_ConfigurationEntity];
        NativeParallelHashMap<Entity, NotificationIconBufferSystem.HiddenPositionData> nativeParallelHashMap = new NativeParallelHashMap<Entity, NotificationIconBufferSystem.HiddenPositionData>();
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ClusterData.isEmpty)
        {
          NativeList<IconClusterSystem.IconCluster> nativeList = new NativeList<IconClusterSystem.IconCluster>(64, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          int index1 = 0;
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster cluster1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          while (this.m_ClusterData.GetRoot(ref index1, out cluster1))
          {
            // ISSUE: reference to a compiler-generated field
            float distance = math.distance(this.m_CameraPosition, cluster1.center);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            bounds3 |= cluster1.GetBounds(distance, this.m_CameraUp);
            nativeList.Add(in cluster1);
          }
          while (nativeList.Length != 0)
          {
            // ISSUE: variable of a compiler-generated type
            IconClusterSystem.IconCluster iconCluster = nativeList[nativeList.Length - 1];
            nativeList.RemoveAtSwapBack(nativeList.Length - 1);
            // ISSUE: reference to a compiler-generated field
            float distance = math.distance(this.m_CameraPosition, iconCluster.center);
            // ISSUE: reference to a compiler-generated method
            if (iconCluster.KeepCluster(distance))
            {
              // ISSUE: reference to a compiler-generated method
              float radius = iconCluster.GetRadius(distance);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              NativeArray<IconClusterSystem.ClusterIcon> icons = iconCluster.GetIcons(this.m_ClusterData);
              bool flag;
              do
              {
                flag = false;
                // ISSUE: variable of a compiler-generated type
                IconClusterSystem.ClusterIcon clusterIcon1 = icons[0];
                for (int index2 = 1; index2 < icons.Length; ++index2)
                {
                  // ISSUE: variable of a compiler-generated type
                  IconClusterSystem.ClusterIcon clusterIcon2 = icons[index2];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (clusterIcon1.priority == clusterIcon2.priority && (double) math.dot(this.m_CameraRight.xz, clusterIcon1.order) > (double) math.dot(this.m_CameraRight.xz, clusterIcon2.order))
                  {
                    icons[index2] = clusterIcon1;
                    icons[index2 - 1] = clusterIcon2;
                    flag = true;
                  }
                  else
                    clusterIcon1 = clusterIcon2;
                }
              }
              while (flag);
              // ISSUE: reference to a compiler-generated field
              float3 float3_1 = this.m_CameraRight * (radius * 0.5f);
              float3 center = iconCluster.center;
              if (iconCluster.isMoving)
              {
                // ISSUE: variable of a compiler-generated type
                IconClusterSystem.ClusterIcon clusterIcon = icons[0];
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnerData.HasComponent(clusterIcon.icon))
                {
                  // ISSUE: reference to a compiler-generated field
                  Owner owner = this.m_OwnerData[clusterIcon.icon];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_InterpolatedTransformData.HasComponent(owner.m_Owner))
                  {
                    // ISSUE: reference to a compiler-generated field
                    PrefabRef prefabRef = this.m_PrefabRefData[owner.m_Owner];
                    // ISSUE: reference to a compiler-generated field
                    Game.Objects.Transform transform = this.m_InterpolatedTransformData[owner.m_Owner].ToTransform();
                    // ISSUE: reference to a compiler-generated field
                    Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, this.m_ObjectGeometryData[prefabRef.m_Prefab]);
                    center.xz = transform.m_Position.xz;
                    center.y = bounds.max.y;
                  }
                }
              }
              float3 float3_2 = center + float3_1 * ((float) (icons.Length - 1) * 0.5f);
              for (int index3 = icons.Length - 1; index3 >= 0; --index3)
              {
                // ISSUE: variable of a compiler-generated type
                IconClusterSystem.ClusterIcon clusterIcon = icons[index3];
                float num = 1E-06f * (float) ((byte) index3 - clusterIcon.priority);
                // ISSUE: reference to a compiler-generated field
                if (this.m_HiddenData.HasComponent(clusterIcon.icon))
                {
                  if (!nativeParallelHashMap.IsCreated)
                    nativeParallelHashMap = new NativeParallelHashMap<Entity, NotificationIconBufferSystem.HiddenPositionData>(16, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                  // ISSUE: object of a compiler-generated type is created
                  nativeParallelHashMap.TryAdd(clusterIcon.icon, new NotificationIconBufferSystem.HiddenPositionData()
                  {
                    m_Position = float3_2,
                    m_Radius = radius,
                    m_Distance = distance * (1f + num)
                  });
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[clusterIcon.prefab];
                  float s = (float) clusterIcon.priority * 0.003921569f;
                  // ISSUE: reference to a compiler-generated field
                  float4 float4 = new float4(math.lerp(notificationIconDisplayData.m_MinParams, notificationIconDisplayData.m_MaxParams, s), math.select((float2) 1f, new float2(0.5f, 0.0f), ((int) notificationIconDisplayData.m_CategoryMask & (int) this.m_CategoryMask) == 0 && !iconCluster.isTemp));
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_InstanceData.Add(new NotificationIconBufferSystem.InstanceData()
                  {
                    m_Position = float3_2,
                    m_Params = float4,
                    m_Icon = (float) notificationIconDisplayData.m_IconIndex,
                    m_Distance = distance * (1f + num)
                  });
                }
                float3_2 -= float3_1;
              }
            }
            else
            {
              int2 subClusters;
              // ISSUE: reference to a compiler-generated method
              if (iconCluster.GetSubClusters(out subClusters))
              {
                ref NativeList<IconClusterSystem.IconCluster> local1 = ref nativeList;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: variable of a compiler-generated type
                IconClusterSystem.IconCluster cluster2 = this.m_ClusterData.GetCluster(subClusters.x);
                ref IconClusterSystem.IconCluster local2 = ref cluster2;
                local1.Add(in local2);
                ref NativeList<IconClusterSystem.IconCluster> local3 = ref nativeList;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                cluster2 = this.m_ClusterData.GetCluster(subClusters.y);
                ref IconClusterSystem.IconCluster local4 = ref cluster2;
                local3.Add(in local4);
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index4 = 0; index4 < this.m_Chunks.Length; ++index4)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index4];
          // ISSUE: reference to a compiler-generated field
          if (!chunk.Has<Hidden>(ref this.m_HiddenType))
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Icon> nativeArray1 = chunk.GetNativeArray<Icon>(ref this.m_IconType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Game.Notifications.Animation> nativeArray2 = chunk.GetNativeArray<Game.Notifications.Animation>(ref this.m_AnimationType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
            // ISSUE: variable of a compiler-generated type
            NotificationIconBufferSystem.InstanceData instanceData;
            if (nativeArray4.Length != 0)
            {
              for (int index5 = 0; index5 < nativeArray1.Length; ++index5)
              {
                Icon icon1 = nativeArray1[index5];
                PrefabRef prefabRef = nativeArray3[index5];
                Temp temp = nativeArray4[index5];
                // ISSUE: reference to a compiler-generated field
                if (this.m_IconDisplayData.IsComponentEnabled(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[prefabRef.m_Prefab];
                  // ISSUE: reference to a compiler-generated field
                  float distance = math.distance(icon1.m_Location, this.m_CameraPosition);
                  if (temp.m_Original != Entity.Null)
                  {
                    // ISSUE: variable of a compiler-generated type
                    NotificationIconBufferSystem.HiddenPositionData hiddenPositionData;
                    if (nativeParallelHashMap.IsCreated && nativeParallelHashMap.TryGetValue(temp.m_Original, out hiddenPositionData))
                    {
                      // ISSUE: reference to a compiler-generated field
                      Icon icon2 = this.m_IconData[temp.m_Original];
                      // ISSUE: reference to a compiler-generated field
                      if ((double) math.distance(icon1.m_Location, icon2.m_Location) < (double) hiddenPositionData.m_Radius * 0.10000000149011612)
                      {
                        // ISSUE: reference to a compiler-generated field
                        icon1.m_Location = hiddenPositionData.m_Position;
                        // ISSUE: reference to a compiler-generated field
                        distance = hiddenPositionData.m_Distance;
                      }
                    }
                    else
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (this.m_DisallowClusterData.HasComponent(temp.m_Original) && !this.m_DeletedData.HasComponent(temp.m_Original))
                      {
                        // ISSUE: reference to a compiler-generated field
                        Icon icon3 = this.m_IconData[temp.m_Original];
                        icon1.m_Location = icon3.m_Location;
                        // ISSUE: reference to a compiler-generated field
                        distance = math.distance(icon1.m_Location, this.m_CameraPosition);
                      }
                      else
                        continue;
                    }
                  }
                  float s = (float) icon1.m_Priority * 0.003921569f;
                  // ISSUE: reference to a compiler-generated field
                  float4 iconParams = new float4(math.lerp(notificationIconDisplayData.m_MinParams, notificationIconDisplayData.m_MaxParams, s), math.select((float2) 1f, new float2(0.5f, 0.0f), ((int) notificationIconDisplayData.m_CategoryMask & (int) this.m_CategoryMask) == 0));
                  if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Select)) != (TempFlags) 0)
                  {
                    iconParams.x *= 1.1f;
                    iconParams.y = 0.0f;
                  }
                  // ISSUE: reference to a compiler-generated method
                  float radius = IconClusterSystem.IconCluster.CalculateRadius(iconParams.x, distance);
                  // ISSUE: reference to a compiler-generated field
                  if (temp.m_Original != Entity.Null && this.m_AnimationData.HasComponent(temp.m_Original))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.Animate(ref icon1.m_Location, ref iconParams, radius, this.m_AnimationData[temp.m_Original], iconAnimation);
                  }
                  if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Select)) != (TempFlags) 0)
                    distance *= 0.99f;
                  else if ((icon1.m_Flags & IconFlags.OnTop) != (IconFlags) 0)
                    distance *= 0.995f;
                  // ISSUE: reference to a compiler-generated field
                  ref NativeList<NotificationIconBufferSystem.InstanceData> local5 = ref this.m_InstanceData;
                  // ISSUE: object of a compiler-generated type is created
                  instanceData = new NotificationIconBufferSystem.InstanceData();
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Position = icon1.m_Location;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Params = iconParams;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Icon = (float) notificationIconDisplayData.m_IconIndex;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Distance = distance;
                  ref NotificationIconBufferSystem.InstanceData local6 = ref instanceData;
                  local5.Add(in local6);
                  bounds3 |= new Bounds3(icon1.m_Location - radius, icon1.m_Location + radius);
                }
              }
            }
            else
            {
              for (int index6 = 0; index6 < nativeArray1.Length; ++index6)
              {
                Icon icon = nativeArray1[index6];
                PrefabRef prefabRef = nativeArray3[index6];
                // ISSUE: reference to a compiler-generated field
                if (this.m_IconDisplayData.IsComponentEnabled(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[prefabRef.m_Prefab];
                  float s = (float) icon.m_Priority * 0.003921569f;
                  // ISSUE: reference to a compiler-generated field
                  float4 iconParams = new float4(math.lerp(notificationIconDisplayData.m_MinParams, notificationIconDisplayData.m_MaxParams, s), math.select((float2) 1f, new float2(0.5f, 0.0f), ((int) notificationIconDisplayData.m_CategoryMask & (int) this.m_CategoryMask) == 0));
                  // ISSUE: reference to a compiler-generated field
                  float distance = math.distance(icon.m_Location, this.m_CameraPosition);
                  // ISSUE: reference to a compiler-generated method
                  float radius = IconClusterSystem.IconCluster.CalculateRadius(iconParams.x, distance);
                  if (nativeArray2.Length != 0)
                  {
                    Game.Notifications.Animation animation = nativeArray2[index6];
                    if ((double) animation.m_Timer > 0.0)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.Animate(ref icon.m_Location, ref iconParams, radius, animation, iconAnimation);
                    }
                    else
                      continue;
                  }
                  if ((icon.m_Flags & IconFlags.OnTop) != (IconFlags) 0)
                    distance *= 0.995f;
                  // ISSUE: reference to a compiler-generated field
                  ref NativeList<NotificationIconBufferSystem.InstanceData> local7 = ref this.m_InstanceData;
                  // ISSUE: object of a compiler-generated type is created
                  instanceData = new NotificationIconBufferSystem.InstanceData();
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Position = icon.m_Location;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Params = iconParams;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Icon = (float) notificationIconDisplayData.m_IconIndex;
                  // ISSUE: reference to a compiler-generated field
                  instanceData.m_Distance = distance;
                  ref NotificationIconBufferSystem.InstanceData local8 = ref instanceData;
                  local7.Add(in local8);
                  bounds3 |= new Bounds3(icon.m_Location - radius, icon.m_Location + radius);
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_IconBounds.value = bounds3;
      }

      private void Animate(
        ref float3 location,
        ref float4 iconParams,
        float radius,
        Game.Notifications.Animation animation,
        DynamicBuffer<IconAnimationElement> iconAnimations)
      {
        float3 float3 = iconAnimations[(int) animation.m_Type].m_AnimationCurve.Evaluate(animation.m_Timer / animation.m_Duration);
        iconParams.xz *= float3.xy;
        // ISSUE: reference to a compiler-generated field
        location += this.m_CameraUp * (radius * float3.z);
      }
    }

    [BurstCompile]
    private struct NotificationIconSortJob : IJob
    {
      public NativeList<NotificationIconBufferSystem.InstanceData> m_InstanceData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_InstanceData.Length < 2)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceData.Sort<NotificationIconBufferSystem.InstanceData>();
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Notifications.Animation> __Game_Notifications_Animation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Hidden> __Game_Tools_Hidden_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Notifications.Animation> __Game_Notifications_Animation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<DisallowCluster> __Game_Notifications_DisallowCluster_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<IconAnimationElement> __Game_Prefabs_IconAnimationElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Animation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Notifications.Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconDisplayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Animation_RO_ComponentLookup = state.GetComponentLookup<Game.Notifications.Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_DisallowCluster_RO_ComponentLookup = state.GetComponentLookup<DisallowCluster>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IconAnimationElement_RO_BufferLookup = state.GetBufferLookup<IconAnimationElement>(true);
      }
    }
  }
}
