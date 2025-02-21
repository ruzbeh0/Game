// Decompiled with JetBrains decompiler
// Type: Game.Notifications.IconClusterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Serialization;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Notifications
{
  [CompilerGenerated]
  public class IconClusterSystem : GameSystemBase, IPreDeserialize
  {
    private EntityQuery m_IconQuery;
    private EntityQuery m_ModifiedQuery;
    private EntityQuery m_ModifiedAndTempQuery;
    private NativeQuadTree<int, IconClusterSystem.TreeBounds> m_ClusterTree;
    private NativeHeapAllocator m_IconAllocator;
    private NativeList<IconClusterSystem.IconCluster> m_IconClusters;
    private NativeList<IconClusterSystem.ClusterIcon> m_ClusterIcons;
    private NativeList<int> m_RootClusters;
    private NativeList<int> m_FreeClusterIndices;
    private JobHandle m_ClusterReadDeps;
    private JobHandle m_ClusterWriteDeps;
    private bool m_Loaded;
    private IconClusterSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(ComponentType.ReadOnly<Icon>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<DisallowCluster>(), ComponentType.Exclude<Animation>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Icon>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ModifiedAndTempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Icon>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterTree = new NativeQuadTree<int, IconClusterSystem.TreeBounds>(1f, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IconAllocator = new NativeHeapAllocator(1024U, 1U, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_IconClusters = new NativeList<IconClusterSystem.IconCluster>(1024, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterIcons = new NativeList<IconClusterSystem.ClusterIcon>(1024, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_RootClusters = new NativeList<int>(8, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_FreeClusterIndices = new NativeList<int>(128, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      ref NativeList<IconClusterSystem.IconCluster> local1 = ref this.m_IconClusters;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconClusterSystem.IconCluster iconCluster = new IconClusterSystem.IconCluster();
      ref IconClusterSystem.IconCluster local2 = ref iconCluster;
      local1.Add(in local2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterIcons.ResizeUninitialized((int) this.m_IconAllocator.Size);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_IconClusters.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterReadDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterWriteDeps.Complete();
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterTree.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_IconAllocator.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_IconClusters.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterIcons.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_RootClusters.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_FreeClusterIndices.Dispose();
      }
      base.OnDestroy();
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((loaded ? this.m_IconQuery : this.m_ModifiedQuery).IsEmptyIgnoreFilter)
        return;
      if (loaded)
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearData();
      }
      NativeList<UnsafeHashSet<int>> nativeList1 = new NativeList<UnsafeHashSet<int>>(64, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<IconClusterSystem.TempIconCluster> nativeList2 = new NativeList<IconClusterSystem.TempIconCluster>(100, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconClusterSystem.IconData iconData = new IconClusterSystem.IconData()
      {
        m_ClusterTree = this.m_ClusterTree,
        m_IconAllocator = this.m_IconAllocator,
        m_IconClusters = this.m_IconClusters,
        m_ClusterIcons = this.m_ClusterIcons,
        m_RootClusters = this.m_RootClusters,
        m_FreeClusterIndices = this.m_FreeClusterIndices
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_DisallowCluster_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconClusterSystem.IconChunkJob jobData1 = new IconClusterSystem.IconChunkJob()
      {
        m_Chunks = (loaded ? this.m_IconQuery : this.m_ModifiedAndTempQuery).ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_DisallowClusterType = this.__TypeHandle.__Game_Notifications_DisallowCluster_RO_ComponentTypeHandle,
        m_AnimationType = this.__TypeHandle.__Game_Notifications_Animation_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_IconDisplayData = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup,
        m_MovingData = this.__TypeHandle.__Game_Objects_Moving_RO_ComponentLookup,
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentTypeHandle,
        m_IconData = iconData,
        m_Orphans = nativeList1,
        m_TempBuffer = nativeList2
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconClusterSystem.IconClusterJob jobData2 = new IconClusterSystem.IconClusterJob()
      {
        m_IconData = iconData,
        m_Orphans = nativeList1,
        m_TempBuffer = nativeList2
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps1 = jobData1.Schedule<IconClusterSystem.IconChunkJob>(JobHandle.CombineDependencies(outJobHandle, this.m_ClusterReadDeps, this.m_ClusterWriteDeps));
      JobHandle dependsOn = inputDeps1;
      JobHandle inputDeps2 = jobData2.Schedule<IconClusterSystem.IconClusterJob>(dependsOn);
      // ISSUE: reference to a compiler-generated field
      jobData1.m_Chunks.Dispose(inputDeps1);
      nativeList1.Dispose(inputDeps2);
      nativeList2.Dispose(inputDeps2);
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterWriteDeps = inputDeps2;
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterReadDeps = new JobHandle();
      this.Dependency = inputDeps1;
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated method
      this.ClearData();
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    public IconClusterSystem.ClusterData GetIconClusterData(
      bool readOnly,
      out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = readOnly ? this.m_ClusterWriteDeps : JobHandle.CombineDependencies(this.m_ClusterReadDeps, this.m_ClusterWriteDeps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new IconClusterSystem.ClusterData(this.m_IconClusters, this.m_ClusterIcons, this.m_RootClusters);
    }

    public void AddIconClusterReader(JobHandle jobHandle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterReadDeps = JobHandle.CombineDependencies(this.m_ClusterReadDeps, jobHandle);
    }

    public void AddIconClusterWriter(JobHandle jobHandle) => this.m_ClusterWriteDeps = jobHandle;

    public void RecalculateClusters() => this.m_Loaded = true;

    private void ClearData()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_IconClusters.IsCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterReadDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterWriteDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterReadDeps = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterWriteDeps = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterTree.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_IconAllocator.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_IconClusters.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterIcons.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_RootClusters.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_FreeClusterIndices.Clear();
      // ISSUE: reference to a compiler-generated field
      ref NativeList<IconClusterSystem.IconCluster> local1 = ref this.m_IconClusters;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconClusterSystem.IconCluster iconCluster = new IconClusterSystem.IconCluster();
      ref IconClusterSystem.IconCluster local2 = ref iconCluster;
      local1.Add(in local2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ClusterIcons.ResizeUninitialized((int) this.m_IconAllocator.Size);
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
    public IconClusterSystem()
    {
    }

    public struct ClusterData
    {
      private NativeList<IconClusterSystem.IconCluster> m_Clusters;
      private NativeList<IconClusterSystem.ClusterIcon> m_Icons;
      private NativeList<int> m_Roots;

      public ClusterData(
        NativeList<IconClusterSystem.IconCluster> clusters,
        NativeList<IconClusterSystem.ClusterIcon> icons,
        NativeList<int> roots)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Clusters = clusters;
        // ISSUE: reference to a compiler-generated field
        this.m_Icons = icons;
        // ISSUE: reference to a compiler-generated field
        this.m_Roots = roots;
      }

      public bool isEmpty => this.m_Clusters.Length == 0;

      public bool GetRoot(ref int index, out IconClusterSystem.IconCluster cluster)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_Roots.Length > index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cluster = this.m_Clusters[this.m_Roots[index++]];
          return true;
        }
        // ISSUE: object of a compiler-generated type is created
        cluster = new IconClusterSystem.IconCluster();
        return false;
      }

      public IconClusterSystem.IconCluster GetCluster(int index) => this.m_Clusters[index];

      public NativeArray<IconClusterSystem.ClusterIcon> GetIcons(int firstIcon, int iconCount)
      {
        // ISSUE: reference to a compiler-generated field
        return this.m_Icons.AsArray().GetSubArray(firstIcon, iconCount);
      }
    }

    public struct IconCluster : IEquatable<IconClusterSystem.IconCluster>
    {
      private float3 m_Center;
      private float3 m_Size;
      private int2 m_SubClusters;
      private float m_DistanceFactor;
      private float m_Radius;
      private int m_ParentCluster;
      private NativeHeapBlock m_IconAllocation;
      private int m_IconCount;
      private int m_Level;
      private int m_PrefabIndex;
      private IconClusterLayer m_Layer;
      private IconFlags m_Flags;
      private bool m_IsMoving;
      private bool m_IsTemp;

      public IconCluster(
        float3 center,
        float3 size,
        int parentCluster,
        int2 subClusters,
        float radius,
        float distanceFactor,
        NativeHeapBlock iconAllocation,
        IconClusterLayer layer,
        IconFlags flags,
        int iconCount,
        int level,
        int prefabIndex,
        bool isMoving,
        bool isTemp)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Center = center;
        // ISSUE: reference to a compiler-generated field
        this.m_Size = size;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentCluster = parentCluster;
        // ISSUE: reference to a compiler-generated field
        this.m_SubClusters = subClusters;
        // ISSUE: reference to a compiler-generated field
        this.m_DistanceFactor = distanceFactor;
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = radius;
        // ISSUE: reference to a compiler-generated field
        this.m_IconAllocation = iconAllocation;
        // ISSUE: reference to a compiler-generated field
        this.m_Layer = layer;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = flags;
        // ISSUE: reference to a compiler-generated field
        this.m_IconCount = iconCount;
        // ISSUE: reference to a compiler-generated field
        this.m_Level = level;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabIndex = prefabIndex;
        // ISSUE: reference to a compiler-generated field
        this.m_IsMoving = isMoving;
        // ISSUE: reference to a compiler-generated field
        this.m_IsTemp = isTemp;
      }

      public IconCluster(
        IconClusterSystem.IconCluster cluster1,
        IconClusterSystem.IconCluster cluster2,
        int index1,
        int index2,
        NativeHeapBlock iconAllocation,
        int iconCount,
        int level)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3_1 = math.min(cluster1.m_Center - cluster1.m_Size, cluster2.m_Center - cluster2.m_Size) * 0.5f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3_2 = math.max(cluster1.m_Center + cluster1.m_Size, cluster2.m_Center + cluster2.m_Size) * 0.5f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num1 = math.distance(cluster1.m_Center, cluster2.m_Center);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num2 = math.select(1f, 0.5f, iconCount == cluster1.m_IconCount + cluster2.m_IconCount);
        // ISSUE: reference to a compiler-generated field
        this.m_Center = float3_1 + float3_2;
        // ISSUE: reference to a compiler-generated field
        this.m_Size = float3_2 - float3_1;
        // ISSUE: reference to a compiler-generated field
        this.m_ParentCluster = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_SubClusters = new int2(index1, index2);
        // ISSUE: reference to a compiler-generated field
        this.m_IconAllocation = iconAllocation;
        // ISSUE: reference to a compiler-generated field
        this.m_IconCount = iconCount;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = math.max(cluster1.m_Radius, cluster2.m_Radius);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DistanceFactor = math.max(cluster1.m_DistanceFactor, cluster2.m_DistanceFactor);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_DistanceFactor = math.max(this.m_DistanceFactor, num1 / (this.m_Radius * num2));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabIndex = cluster1.m_PrefabIndex == cluster2.m_PrefabIndex ? cluster1.m_PrefabIndex : -1;
        // ISSUE: reference to a compiler-generated field
        this.m_Level = level;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IsMoving = cluster1.m_IsMoving && cluster2.m_IsMoving;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_IsTemp = cluster1.m_IsTemp;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Layer = cluster1.m_Layer;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = cluster1.m_Flags & cluster2.m_Flags;
      }

      public static void SetParent(ref IconClusterSystem.IconCluster cluster, int parent)
      {
        // ISSUE: reference to a compiler-generated field
        cluster.m_ParentCluster = parent;
      }

      public float3 center => this.m_Center;

      public float3 size => this.m_Size;

      public float distanceFactor => this.m_DistanceFactor;

      public IconClusterLayer layer => this.m_Layer;

      public IconFlags flags => this.m_Flags;

      public bool isMoving => this.m_IsMoving;

      public bool isTemp => this.m_IsTemp;

      public int parentCluster => this.m_ParentCluster;

      public int level => this.m_Level;

      public int prefabIndex => this.m_PrefabIndex;

      public bool GetSubClusters(out int2 subClusters)
      {
        // ISSUE: reference to a compiler-generated field
        subClusters = this.m_SubClusters;
        // ISSUE: reference to a compiler-generated field
        return this.m_SubClusters.x != 0;
      }

      public float GetRadius(float distance)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return IconClusterSystem.IconCluster.CalculateRadius(this.m_Radius, distance);
      }

      public static float CalculateRadius(float radius, float distance)
      {
        return (float) ((double) radius * (double) math.pow(distance, 0.6f) * 0.063000001013278961);
      }

      public Bounds3 GetBounds(float distance, float3 cameraUp)
      {
        // ISSUE: reference to a compiler-generated method
        float radius = this.GetRadius(distance);
        // ISSUE: reference to a compiler-generated field
        float3 float3_1 = this.m_Center + cameraUp * radius;
        // ISSUE: reference to a compiler-generated field
        float3 float3_2 = this.m_Size + radius;
        return new Bounds3(float3_1 - float3_2, float3_1 + float3_2);
      }

      public bool KeepCluster(float distance)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_SubClusters.x < 0 || (double) math.pow(distance, 0.6f) * 0.18900001049041748 * (1.0 + (double) distance * 1.9999999494757503E-05) > (double) this.m_DistanceFactor;
      }

      public NativeArray<IconClusterSystem.ClusterIcon> GetIcons(
        IconClusterSystem.ClusterData clusterData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return clusterData.GetIcons((int) this.m_IconAllocation.Begin, this.m_IconCount);
      }

      public NativeHeapBlock GetIcons(out int firstIcon, out int iconCount)
      {
        // ISSUE: reference to a compiler-generated field
        firstIcon = (int) this.m_IconAllocation.Begin;
        // ISSUE: reference to a compiler-generated field
        iconCount = this.m_IconCount;
        // ISSUE: reference to a compiler-generated field
        return this.m_IconAllocation;
      }

      public bool Equals(IconClusterSystem.IconCluster other)
      {
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
        return this.m_Center.Equals(other.m_Center) && this.m_Size.Equals(other.m_Size) && this.m_SubClusters.Equals(other.m_SubClusters) && this.m_DistanceFactor.Equals(other.m_DistanceFactor) && this.m_Radius.Equals(other.m_Radius) && this.m_ParentCluster.Equals(other.m_ParentCluster) && this.m_IconAllocation.Begin.Equals(other.m_IconAllocation.Begin) && this.m_IconCount.Equals(other.m_IconCount) && this.m_Level.Equals(other.m_Level) && this.m_PrefabIndex.Equals(other.m_PrefabIndex) && this.m_Layer == other.m_Layer && this.m_Flags == other.m_Flags && this.m_IsMoving == other.m_IsMoving && this.m_IsTemp == other.m_IsTemp;
      }
    }

    public struct ClusterIcon
    {
      private Entity m_Icon;
      private Entity m_Prefab;
      private float2 m_Order;
      private IconPriority m_Priority;
      private IconFlags m_Flags;

      public ClusterIcon(
        Entity icon,
        Entity prefab,
        float2 order,
        IconPriority priority,
        IconFlags flags)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = icon;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_Order = order;
        // ISSUE: reference to a compiler-generated field
        this.m_Priority = priority;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = flags;
      }

      public Entity icon => this.m_Icon;

      public Entity prefab => this.m_Prefab;

      public float2 order => this.m_Order;

      public IconPriority priority => this.m_Priority;

      public IconFlags flags => this.m_Flags;
    }

    private struct TempIconCluster : IComparable<IconClusterSystem.TempIconCluster>
    {
      public float3 m_Center;
      public float3 m_Size;
      public Entity m_Icon;
      public Entity m_Prefab;
      public int2 m_SubClusters;
      public float m_Radius;
      public int m_FriendIndex;
      public int m_MovingGroup;
      public IconPriority m_Priority;
      public IconClusterLayer m_ClusterLayer;
      public IconFlags m_Flags;

      public TempIconCluster(
        Entity entity,
        Entity prefab,
        Icon icon,
        NotificationIconDisplayData displayData,
        int movingGroup)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Center = icon.m_Location;
        // ISSUE: reference to a compiler-generated field
        this.m_Size = new float3();
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = entity;
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = prefab;
        // ISSUE: reference to a compiler-generated field
        this.m_SubClusters = (int2) 0;
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = math.lerp(displayData.m_MinParams.x, displayData.m_MaxParams.x, (float) icon.m_Priority * 0.003921569f);
        // ISSUE: reference to a compiler-generated field
        this.m_FriendIndex = 0;
        // ISSUE: reference to a compiler-generated field
        this.m_MovingGroup = movingGroup;
        // ISSUE: reference to a compiler-generated field
        this.m_Priority = icon.m_Priority;
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterLayer = icon.m_ClusterLayer;
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = icon.m_Flags;
      }

      public TempIconCluster(
        in IconClusterSystem.TempIconCluster cluster1,
        in IconClusterSystem.TempIconCluster cluster2,
        int index1,
        int index2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3_1 = math.min(cluster1.m_Center - cluster1.m_Size, cluster2.m_Center - cluster2.m_Size) * 0.5f;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3_2 = math.max(cluster1.m_Center + cluster1.m_Size, cluster2.m_Center + cluster2.m_Size) * 0.5f;
        // ISSUE: reference to a compiler-generated field
        this.m_Center = float3_1 + float3_2;
        // ISSUE: reference to a compiler-generated field
        this.m_Size = float3_2 - float3_1;
        // ISSUE: reference to a compiler-generated field
        this.m_Icon = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Prefab = cluster1.m_Prefab == cluster2.m_Prefab ? cluster1.m_Prefab : Entity.Null;
        // ISSUE: reference to a compiler-generated field
        this.m_SubClusters = new int2(index1, index2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Radius = math.max(cluster1.m_Radius, cluster2.m_Radius);
        // ISSUE: reference to a compiler-generated field
        this.m_FriendIndex = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MovingGroup = math.select(-1, cluster1.m_MovingGroup, cluster1.m_MovingGroup == cluster2.m_MovingGroup);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Priority = (IconPriority) math.max((int) cluster1.m_Priority, (int) cluster2.m_Priority);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterLayer = cluster1.m_ClusterLayer;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Flags = cluster1.m_Flags & cluster2.m_Flags;
      }

      public int CompareTo(IconClusterSystem.TempIconCluster other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return math.select(this.m_Icon.Index - other.m_Icon.Index, math.select(-1, 1, (double) this.m_Center.x > (double) other.m_Center.x), (double) this.m_Center.x != (double) other.m_Center.x);
      }
    }

    private struct TreeBounds : 
      IEquatable<IconClusterSystem.TreeBounds>,
      IBounds2<IconClusterSystem.TreeBounds>
    {
      public Bounds3 m_Bounds;
      public ulong m_LevelMask;
      public ulong m_LayerMask;

      public void Reset()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Bounds = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
        // ISSUE: reference to a compiler-generated field
        this.m_LevelMask = 0UL;
        // ISSUE: reference to a compiler-generated field
        this.m_LayerMask = 0UL;
      }

      public float2 Center() => MathUtils.Center(this.m_Bounds.xz);

      public float2 Size() => MathUtils.Size(this.m_Bounds.xz);

      public IconClusterSystem.TreeBounds Merge(IconClusterSystem.TreeBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        return new IconClusterSystem.TreeBounds()
        {
          m_Bounds = this.m_Bounds | other.m_Bounds,
          m_LevelMask = this.m_LevelMask | other.m_LevelMask,
          m_LayerMask = this.m_LayerMask | other.m_LayerMask
        };
      }

      public bool Intersect(IconClusterSystem.TreeBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(this.m_Bounds, other.m_Bounds) && ((long) this.m_LevelMask & (long) other.m_LevelMask) != 0L && (this.m_LayerMask & other.m_LayerMask) > 0UL;
      }

      public bool Equals(IconClusterSystem.TreeBounds other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_Bounds.Equals(other.m_Bounds) && (long) this.m_LevelMask == (long) other.m_LevelMask && (long) this.m_LayerMask == (long) other.m_LayerMask;
      }
    }

    [BurstCompile]
    private struct IconChunkJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<DisallowCluster> m_DisallowClusterType;
      [ReadOnly]
      public ComponentTypeHandle<Animation> m_AnimationType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Moving> m_MovingData;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> m_IconDisplayData;
      public ComponentTypeHandle<Icon> m_IconType;
      public IconClusterSystem.IconData m_IconData;
      public NativeList<UnsafeHashSet<int>> m_Orphans;
      public NativeList<IconClusterSystem.TempIconCluster> m_TempBuffer;

      public void Execute()
      {
        NativeList<int> tempList = new NativeList<int>(100, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconData.HandleOldRoots(this.m_Orphans, tempList);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.HandleChunks(this.m_Orphans, this.m_TempBuffer);
        tempList.Dispose();
      }

      public void HandleChunks(
        NativeList<UnsafeHashSet<int>> orphans,
        NativeList<IconClusterSystem.TempIconCluster> tempBuffer)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool c = this.m_IconData.m_IconClusters.Length <= 1;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Icon> nativeArray1 = chunk.GetNativeArray<Icon>(ref this.m_IconType);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType) || chunk.Has<DisallowCluster>(ref this.m_DisallowClusterType) || chunk.Has<Animation>(ref this.m_AnimationType))
          {
            // ISSUE: reference to a compiler-generated field
            if (!chunk.Has<Temp>(ref this.m_TempType))
            {
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                ref Icon local = ref nativeArray1.ElementAt<Icon>(index2);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_IconData.Remove(local.m_ClusterIndex, 0, -1, orphans);
                local.m_ClusterIndex = 0;
              }
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
            // ISSUE: reference to a compiler-generated field
            NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Temp>(ref this.m_TempType))
            {
              for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
              {
                Entity entity = nativeArray2[index3];
                Icon icon = nativeArray1[index3];
                PrefabRef prefabRef = nativeArray4[index3];
                // ISSUE: reference to a compiler-generated field
                if (this.m_IconDisplayData.IsComponentEnabled(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  NotificationIconDisplayData displayData = this.m_IconDisplayData[prefabRef.m_Prefab];
                  int movingGroup = -1;
                  Owner owner;
                  // ISSUE: reference to a compiler-generated field
                  if (CollectionUtils.TryGet<Owner>(nativeArray3, index3, out owner) && this.m_MovingData.HasComponent(owner.m_Owner))
                    movingGroup = owner.m_Owner.Index;
                  ref NativeList<IconClusterSystem.TempIconCluster> local1 = ref tempBuffer;
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  IconClusterSystem.TempIconCluster tempIconCluster = new IconClusterSystem.TempIconCluster(entity, prefabRef.m_Prefab, icon, displayData, movingGroup);
                  ref IconClusterSystem.TempIconCluster local2 = ref tempIconCluster;
                  local1.Add(in local2);
                }
              }
            }
            else
            {
              for (int index4 = 0; index4 < nativeArray2.Length; ++index4)
              {
                Entity icon = nativeArray2[index4];
                PrefabRef prefabRef = nativeArray4[index4];
                ref Icon local3 = ref nativeArray1.ElementAt<Icon>(index4);
                local3.m_ClusterIndex = math.select(local3.m_ClusterIndex, 0, c);
                // ISSUE: reference to a compiler-generated field
                if (this.m_IconDisplayData.IsComponentEnabled(prefabRef.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[prefabRef.m_Prefab];
                  if (local3.m_ClusterIndex == 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    local3.m_ClusterIndex = this.m_IconData.GetNewClusterIndex();
                  }
                  float num1 = math.lerp(notificationIconDisplayData.m_MinParams.x, notificationIconDisplayData.m_MaxParams.x, (float) local3.m_Priority * 0.003921569f);
                  int num2 = -1;
                  Owner owner;
                  // ISSUE: reference to a compiler-generated field
                  if (CollectionUtils.TryGet<Owner>(nativeArray3, index4, out owner) && this.m_MovingData.HasComponent(owner.m_Owner))
                    num2 = owner.m_Owner.Index;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ref IconClusterSystem.IconCluster local4 = ref this.m_IconData.m_IconClusters.ElementAt(local3.m_ClusterIndex);
                  int firstIcon;
                  int iconCount;
                  // ISSUE: reference to a compiler-generated method
                  NativeHeapBlock iconAllocation = local4.GetIcons(out firstIcon, out iconCount);
                  if (iconCount == 0)
                  {
                    iconCount = 1;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    iconAllocation = this.m_IconData.AllocateIcons(iconCount);
                    firstIcon = (int) iconAllocation.Begin;
                  }
                  for (int index5 = 0; index5 < iconCount; ++index5)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: object of a compiler-generated type is created
                    this.m_IconData.m_ClusterIcons.ElementAt(firstIcon + index5) = new IconClusterSystem.ClusterIcon(icon, prefabRef.m_Prefab, local3.m_Location.xz, local3.m_Priority, local3.m_Flags);
                  }
                  // ISSUE: object of a compiler-generated type is created
                  // ISSUE: variable of a compiler-generated type
                  IconClusterSystem.IconCluster iconCluster = new IconClusterSystem.IconCluster(local3.m_Location, (float3) num1, local4.parentCluster, (int2) 0, num1, 0.0f, iconAllocation, local3.m_ClusterLayer, local3.m_Flags, iconCount, 0, prefabRef.m_Prefab.Index, num2 != -1, false);
                  // ISSUE: reference to a compiler-generated method
                  if (!iconCluster.Equals(local4))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_IconData.Remove(local4.parentCluster, local3.m_ClusterIndex, -1, orphans);
                    local4 = iconCluster;
                    // ISSUE: reference to a compiler-generated method
                    IconClusterSystem.IconCluster.SetParent(ref local4, 0);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_IconData.AddOrphan(local3.m_ClusterIndex, 0, orphans);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: object of a compiler-generated type is created
                    this.m_IconData.m_ClusterTree.AddOrUpdate(local3.m_ClusterIndex, new IconClusterSystem.TreeBounds()
                    {
                      m_Bounds = new Bounds3(local4.center - local4.size, local4.center + local4.size),
                      m_LevelMask = 1UL,
                      m_LayerMask = 1UL << (int) (local4.layer & (IconClusterLayer) 63)
                    });
                  }
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    private struct IconClusterJob : IJob
    {
      public IconClusterSystem.IconData m_IconData;
      public NativeList<UnsafeHashSet<int>> m_Orphans;
      public NativeList<IconClusterSystem.TempIconCluster> m_TempBuffer;

      public void Execute()
      {
        NativeQuadTreeSelectorBuffer<float> selectorBuffer = new NativeQuadTreeSelectorBuffer<float>(Allocator.Temp);
        NativeList<IconClusterSystem.ClusterIcon> tempIcons = new NativeList<IconClusterSystem.ClusterIcon>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconData.HandleOrphans(this.m_Orphans, selectorBuffer, tempIcons);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconData.HandleTemps(this.m_TempBuffer, tempIcons);
        selectorBuffer.Dispose();
        tempIcons.Dispose();
      }
    }

    private struct IconData
    {
      public NativeQuadTree<int, IconClusterSystem.TreeBounds> m_ClusterTree;
      public NativeHeapAllocator m_IconAllocator;
      public NativeList<IconClusterSystem.IconCluster> m_IconClusters;
      public NativeList<IconClusterSystem.ClusterIcon> m_ClusterIcons;
      public NativeList<int> m_RootClusters;
      public NativeList<int> m_FreeClusterIndices;

      public void ValidateClusters()
      {
        NativeList<int> nativeList = new NativeList<int>(64, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeHashSet<int> nativeHashSet = new NativeHashSet<int>(64, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_RootClusters.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          nativeList.Add(this.m_RootClusters[index]);
        }
        bool flag = false;
        while (nativeList.Length != 0)
        {
          if (!nativeHashSet.Add(nativeList[nativeList.Length - 1]))
          {
            flag = true;
            nativeList.Clear();
            nativeHashSet.Clear();
            break;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster = this.m_IconClusters[nativeList[nativeList.Length - 1]];
          nativeList.RemoveAtSwapBack(nativeList.Length - 1);
          int2 subClusters;
          // ISSUE: reference to a compiler-generated method
          if (iconCluster.GetSubClusters(out subClusters))
          {
            nativeList.Add(in subClusters.x);
            nativeList.Add(in subClusters.y);
          }
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_RootClusters.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          nativeList.Add(this.m_RootClusters[index]);
        }
        while (nativeList.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster1 = this.m_IconClusters[nativeList[nativeList.Length - 1]];
          int2 subClusters1;
          // ISSUE: reference to a compiler-generated method
          iconCluster1.GetSubClusters(out subClusters1);
          UnityEngine.Debug.Log((object) string.Format("{0}: {1} -> {2}, {3}", (object) iconCluster1.level, (object) nativeList[nativeList.Length - 1], (object) subClusters1.x, (object) subClusters1.y));
          if (!nativeHashSet.Add(nativeList[nativeList.Length - 1]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ClusterTree.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_IconAllocator.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_IconClusters.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_ClusterIcons.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_RootClusters.Clear();
            // ISSUE: reference to a compiler-generated field
            this.m_FreeClusterIndices.Clear();
            // ISSUE: reference to a compiler-generated field
            ref NativeList<IconClusterSystem.IconCluster> local1 = ref this.m_IconClusters;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            IconClusterSystem.IconCluster iconCluster2 = new IconClusterSystem.IconCluster();
            ref IconClusterSystem.IconCluster local2 = ref iconCluster2;
            local1.Add(in local2);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ClusterIcons.ResizeUninitialized((int) this.m_IconAllocator.Size);
            break;
          }
          nativeList.RemoveAtSwapBack(nativeList.Length - 1);
          int2 subClusters2;
          // ISSUE: reference to a compiler-generated method
          if (iconCluster1.GetSubClusters(out subClusters2))
          {
            nativeList.Add(in subClusters2.x);
            nativeList.Add(in subClusters2.y);
          }
        }
      }

      public void HandleOldRoots(NativeList<UnsafeHashSet<int>> orphans, NativeList<int> tempList)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_RootClusters.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int rootCluster = this.m_RootClusters[index];
          // ISSUE: reference to a compiler-generated field
          ref IconClusterSystem.IconCluster local = ref this.m_IconClusters.ElementAt(rootCluster);
          if (local.isTemp)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveTemp(rootCluster, tempList);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddOrphan(rootCluster, local.level, orphans);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_RootClusters.Clear();
      }

      public void HandleOrphans(
        NativeList<UnsafeHashSet<int>> orphans,
        NativeQuadTreeSelectorBuffer<float> selectorBuffer,
        NativeList<IconClusterSystem.ClusterIcon> tempIcons)
      {
        int num = 0;
        UnsafeHashSet<int> unsafeHashSet1 = new UnsafeHashSet<int>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        IconClusterSystem.IconData.Selector selector = new IconClusterSystem.IconData.Selector()
        {
          m_IconClusters = this.m_IconClusters
        };
        for (; num < orphans.Length; ++num)
        {
          UnsafeHashSet<int> orphan = orphans[num];
          // ISSUE: reference to a compiler-generated field
          selector.m_LevelMask = 1UL << num;
          while (orphan.IsCreated)
          {
            UnsafeHashSet<int> unsafeHashSet2 = orphan;
            UnsafeHashSet<int>.Enumerator enumerator = unsafeHashSet2.GetEnumerator();
            orphans[num] = unsafeHashSet1;
            while (enumerator.MoveNext())
            {
              // ISSUE: reference to a compiler-generated field
              ref IconClusterSystem.IconCluster local1 = ref this.m_IconClusters.ElementAt(enumerator.Current);
              if (local1.parentCluster == 0)
              {
                if (num != 63)
                {
                  // ISSUE: reference to a compiler-generated field
                  selector.m_LayerMask = 1UL << (int) (local1.layer & (IconClusterLayer) 63);
                  // ISSUE: reference to a compiler-generated field
                  selector.m_BestCost = float.MaxValue;
                  // ISSUE: reference to a compiler-generated field
                  selector.m_BestDistance = float.MaxValue;
                  // ISSUE: reference to a compiler-generated field
                  selector.m_BestClusterIndex = 0;
                  // ISSUE: reference to a compiler-generated field
                  selector.m_IgnoreClusterIndex = enumerator.Current;
                  // ISSUE: reference to a compiler-generated field
                  selector.m_Cluster = local1;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ClusterTree.Select<IconClusterSystem.IconData.Selector, float>(ref selector, selectorBuffer);
                  // ISSUE: reference to a compiler-generated field
                  if (selector.m_BestClusterIndex != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    float bestCost = selector.m_BestCost;
                    // ISSUE: reference to a compiler-generated field
                    int bestClusterIndex = selector.m_BestClusterIndex;
                    // ISSUE: reference to a compiler-generated field
                    ref IconClusterSystem.IconCluster local2 = ref this.m_IconClusters.ElementAt(bestClusterIndex);
                    // ISSUE: reference to a compiler-generated method
                    if (local2.parentCluster == 0 || (double) bestCost < (double) this.GetCurrentCost(bestClusterIndex, ref local2))
                    {
                      // ISSUE: reference to a compiler-generated field
                      selector.m_BestCost = float.MaxValue;
                      // ISSUE: reference to a compiler-generated field
                      selector.m_BestDistance = float.MaxValue;
                      // ISSUE: reference to a compiler-generated field
                      selector.m_BestClusterIndex = 0;
                      // ISSUE: reference to a compiler-generated field
                      selector.m_IgnoreClusterIndex = bestClusterIndex;
                      // ISSUE: reference to a compiler-generated field
                      selector.m_Cluster = local2;
                      // ISSUE: reference to a compiler-generated field
                      this.m_ClusterTree.Select<IconClusterSystem.IconData.Selector, float>(ref selector, selectorBuffer);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      if (selector.m_BestClusterIndex == enumerator.Current || (double) selector.m_BestCost == (double) bestCost)
                      {
                        if (local2.parentCluster != 0)
                        {
                          // ISSUE: reference to a compiler-generated method
                          this.Remove(local2.parentCluster, bestClusterIndex, num, orphans);
                        }
                        // ISSUE: reference to a compiler-generated method
                        int newClusterIndex = this.GetNewClusterIndex();
                        // ISSUE: reference to a compiler-generated field
                        ref IconClusterSystem.IconCluster local3 = ref this.m_IconClusters.ElementAt(enumerator.Current);
                        // ISSUE: reference to a compiler-generated field
                        ref IconClusterSystem.IconCluster local4 = ref this.m_IconClusters.ElementAt(bestClusterIndex);
                        // ISSUE: reference to a compiler-generated field
                        ref IconClusterSystem.IconCluster local5 = ref this.m_IconClusters.ElementAt(newClusterIndex);
                        // ISSUE: reference to a compiler-generated method
                        this.AddIcons(local3, local4, tempIcons);
                        // ISSUE: reference to a compiler-generated method
                        NativeHeapBlock iconAllocation = this.AllocateIcons(tempIcons.Length);
                        for (int index = 0; index < tempIcons.Length; ++index)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_ClusterIcons[(int) iconAllocation.Begin + index] = tempIcons[index];
                        }
                        // ISSUE: object of a compiler-generated type is created
                        local5 = new IconClusterSystem.IconCluster(local3, local4, enumerator.Current, bestClusterIndex, iconAllocation, tempIcons.Length, num + 1);
                        tempIcons.Clear();
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: object of a compiler-generated type is created
                        this.m_ClusterTree.AddOrUpdate(newClusterIndex, new IconClusterSystem.TreeBounds()
                        {
                          m_Bounds = new Bounds3(local5.center - local5.size, local5.center + local5.size),
                          m_LevelMask = 1UL << num + 1,
                          m_LayerMask = 1UL << (int) (local3.layer & (IconClusterLayer) 63)
                        });
                        // ISSUE: reference to a compiler-generated method
                        this.UpdateLevelMask(bestClusterIndex, (ulong) (-1L << local4.level & ~(-1L << num + 1)));
                        // ISSUE: reference to a compiler-generated method
                        IconClusterSystem.IconCluster.SetParent(ref local3, newClusterIndex);
                        // ISSUE: reference to a compiler-generated method
                        IconClusterSystem.IconCluster.SetParent(ref local4, newClusterIndex);
                        // ISSUE: reference to a compiler-generated method
                        this.AddOrphan(newClusterIndex, num + 1, orphans);
                        continue;
                      }
                    }
                    ulong a = ulong.MaxValue << local1.level;
                    ulong levelMask = math.select(a, a & (ulong) ~(-1L << num + 2), num < 62);
                    // ISSUE: reference to a compiler-generated method
                    this.UpdateLevelMask(enumerator.Current, levelMask);
                    // ISSUE: reference to a compiler-generated method
                    this.AddOrphan(enumerator.Current, num + 1, orphans);
                    continue;
                  }
                }
                // ISSUE: reference to a compiler-generated method
                this.UpdateLevelMask(enumerator.Current, 1UL << local1.level);
                // ISSUE: reference to a compiler-generated field
                this.m_RootClusters.Add(enumerator.Current);
              }
            }
            enumerator.Dispose();
            unsafeHashSet1 = unsafeHashSet2;
            unsafeHashSet1.Clear();
            orphan = orphans[num];
            if (orphan.IsCreated && orphan.IsEmpty)
              orphan.Dispose();
          }
        }
        if (!unsafeHashSet1.IsCreated)
          return;
        unsafeHashSet1.Dispose();
      }

      public void HandleTemps(
        NativeList<IconClusterSystem.TempIconCluster> tempBuffer,
        NativeList<IconClusterSystem.ClusterIcon> tempIcons)
      {
        int length = tempBuffer.Length;
        if (length > 1)
          tempBuffer.Sort<IconClusterSystem.TempIconCluster>();
        NativeArray<IconClusterSystem.TempIconCluster> nativeArray = new NativeArray<IconClusterSystem.TempIconCluster>(length, Allocator.Temp);
        NativeArray<IconClusterSystem.TempIconCluster> a1 = tempBuffer.AsArray();
        NativeArray<IconClusterSystem.TempIconCluster> b = nativeArray;
        while (length > 1)
        {
          for (int index1 = 0; index1 < length; ++index1)
          {
            ref IconClusterSystem.TempIconCluster local1 = ref a1.ElementAt<IconClusterSystem.TempIconCluster>(index1);
            // ISSUE: reference to a compiler-generated field
            local1.m_FriendIndex = index1;
            float num1 = float.MaxValue;
            float num2 = float.MaxValue;
            bool flag = true;
            int index2 = index1 - 1;
            int index3 = index1 + 1;
            while (index2 >= 0 || index3 < length)
            {
              if (index2 >= 0)
              {
                ref IconClusterSystem.TempIconCluster local2 = ref a1.ElementAt<IconClusterSystem.TempIconCluster>(index2);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                double num3 = (double) local1.m_Center.x - (double) local2.m_Center.x;
                if (num3 * num3 > (double) num2)
                {
                  index2 = -1;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (local1.m_ClusterLayer == local2.m_ClusterLayer)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    bool c = local2.m_Prefab == local1.m_Prefab && ((local2.m_Flags | local1.m_Flags) & IconFlags.Unique) == (IconFlags) 0;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float a2 = math.distancesq(local2.m_Center, local1.m_Center);
                    float num4 = math.select(a2, a2 * 0.25f, c);
                    // ISSUE: reference to a compiler-generated field
                    if ((double) num4 < (double) num1 || (double) num4 == (double) num1 && local2.m_FriendIndex == index1)
                    {
                      // ISSUE: reference to a compiler-generated field
                      local1.m_FriendIndex = index2;
                      num1 = num4;
                      num2 = math.select(a2 * 4.01f, a2 * 1.01f, c);
                      // ISSUE: reference to a compiler-generated field
                      flag = local2.m_FriendIndex != index1;
                    }
                  }
                  --index2;
                }
              }
              if (index3 < length)
              {
                ref IconClusterSystem.TempIconCluster local3 = ref a1.ElementAt<IconClusterSystem.TempIconCluster>(index3);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                double num5 = (double) local3.m_Center.x - (double) local1.m_Center.x;
                if (num5 * num5 > (double) num2)
                {
                  index3 = length;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (local1.m_ClusterLayer == local3.m_ClusterLayer)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    bool c = local1.m_Prefab == local3.m_Prefab && ((local1.m_Flags | local3.m_Flags) & IconFlags.Unique) == (IconFlags) 0;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    float a3 = math.distancesq(local1.m_Center, local3.m_Center);
                    float num6 = math.select(a3, a3 * 0.25f, c);
                    if ((double) num6 < (double) num1 || (double) num6 == (double) num1 & flag)
                    {
                      // ISSUE: reference to a compiler-generated field
                      local1.m_FriendIndex = index3;
                      num1 = num6;
                      num2 = math.select(a3 * 4.01f, a3 * 1.01f, c);
                      flag = false;
                    }
                  }
                  ++index3;
                }
              }
            }
          }
          int num = 0;
          for (int index = 0; index < length; ++index)
          {
            ref IconClusterSystem.TempIconCluster local4 = ref a1.ElementAt<IconClusterSystem.TempIconCluster>(index);
            // ISSUE: reference to a compiler-generated field
            ref IconClusterSystem.TempIconCluster local5 = ref a1.ElementAt<IconClusterSystem.TempIconCluster>(local4.m_FriendIndex);
            // ISSUE: reference to a compiler-generated field
            if (local5.m_FriendIndex == index)
            {
              // ISSUE: reference to a compiler-generated field
              if (local4.m_FriendIndex < index)
              {
                // ISSUE: reference to a compiler-generated method
                int newClusterIndex1 = this.GetNewClusterIndex();
                // ISSUE: reference to a compiler-generated method
                int newClusterIndex2 = this.GetNewClusterIndex();
                // ISSUE: object of a compiler-generated type is created
                b[num++] = new IconClusterSystem.TempIconCluster(in local5, in local4, newClusterIndex2, newClusterIndex1);
                // ISSUE: reference to a compiler-generated method
                this.AddCluster(in local5, newClusterIndex2, false, tempIcons);
                // ISSUE: reference to a compiler-generated method
                this.AddCluster(in local4, newClusterIndex1, false, tempIcons);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (local4.m_FriendIndex == index)
                {
                  // ISSUE: reference to a compiler-generated method
                  // ISSUE: reference to a compiler-generated method
                  this.AddCluster(in local4, this.GetNewClusterIndex(), true, tempIcons);
                }
              }
            }
            else
              b[num++] = local4;
          }
          CommonUtils.Swap<NativeArray<IconClusterSystem.TempIconCluster>>(ref a1, ref b);
          if (length != num)
          {
            length = num;
            if (length > 1)
              a1.GetSubArray(0, length).Sort<IconClusterSystem.TempIconCluster>();
          }
          else
            break;
        }
        for (int index = 0; index < length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.AddCluster(in a1.ElementAt<IconClusterSystem.TempIconCluster>(index), this.GetNewClusterIndex(), true, tempIcons);
        }
        nativeArray.Dispose();
      }

      private void AddCluster(
        in IconClusterSystem.TempIconCluster tempCluster,
        int index,
        bool isRoot,
        NativeList<IconClusterSystem.ClusterIcon> tempIcons)
      {
        int iconCount;
        NativeHeapBlock iconAllocation;
        float distanceFactor;
        // ISSUE: reference to a compiler-generated field
        if (tempCluster.m_SubClusters.x != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster1 = this.m_IconClusters[tempCluster.m_SubClusters.x];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster2 = this.m_IconClusters[tempCluster.m_SubClusters.y];
          // ISSUE: reference to a compiler-generated method
          int num1 = this.AddIcons(iconCluster1, iconCluster2, tempIcons);
          iconCount = tempIcons.Length;
          // ISSUE: reference to a compiler-generated method
          iconAllocation = this.AllocateIcons(iconCount);
          for (int index1 = 0; index1 < tempIcons.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ClusterIcons[(int) iconAllocation.Begin + index1] = tempIcons[index1];
          }
          tempIcons.Clear();
          float num2 = math.distance(iconCluster1.center, iconCluster2.center);
          float num3 = math.select(1f, 0.5f, iconCount == num1);
          // ISSUE: reference to a compiler-generated field
          distanceFactor = math.max(math.max(iconCluster1.distanceFactor, iconCluster2.distanceFactor), num2 / (tempCluster.m_Radius * num3));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          iconAllocation = this.AllocateIcons(1);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ClusterIcons[(int) iconAllocation.Begin] = new IconClusterSystem.ClusterIcon(tempCluster.m_Icon, tempCluster.m_Prefab, tempCluster.m_Center.xz, tempCluster.m_Priority, tempCluster.m_Flags);
          iconCount = 1;
          distanceFactor = 0.0f;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        this.m_IconClusters[index] = new IconClusterSystem.IconCluster(tempCluster.m_Center, tempCluster.m_Size, -1, tempCluster.m_SubClusters, tempCluster.m_Radius, distanceFactor, iconAllocation, tempCluster.m_ClusterLayer, tempCluster.m_Flags, iconCount, -1, -1, tempCluster.m_MovingGroup != -1, true);
        if (!isRoot)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_RootClusters.Add(in index);
      }

      private int AddIcons(
        IconClusterSystem.IconCluster subCluster1,
        IconClusterSystem.IconCluster subCluster2,
        NativeList<IconClusterSystem.ClusterIcon> tempIcons)
      {
        int2 int2_1;
        int2 x1;
        // ISSUE: reference to a compiler-generated method
        subCluster1.GetIcons(out int2_1.x, out x1.x);
        // ISSUE: reference to a compiler-generated method
        subCluster2.GetIcons(out int2_1.y, out x1.y);
        int num = math.csum(x1);
        int2 int2_2 = x1 + int2_1;
        while (math.all(int2_1 < int2_2))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.ClusterIcon clusterIcon1 = this.m_ClusterIcons[int2_1.x];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.ClusterIcon clusterIcon2 = this.m_ClusterIcons[int2_1.y];
          if (clusterIcon1.priority >= clusterIcon2.priority)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddIcon(clusterIcon1, tempIcons);
            ++int2_1.x;
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.AddIcon(clusterIcon2, tempIcons);
            ++int2_1.y;
          }
        }
        for (int x2 = int2_1.x; x2 < int2_2.x; ++x2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddIcon(this.m_ClusterIcons[x2], tempIcons);
        }
        for (int y = int2_1.y; y < int2_2.y; ++y)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddIcon(this.m_ClusterIcons[y], tempIcons);
        }
        return num;
      }

      private void UpdateLevelMask(int clusterIndex, ulong levelMask)
      {
        // ISSUE: variable of a compiler-generated type
        IconClusterSystem.TreeBounds bounds;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ClusterTree.TryGet(clusterIndex, out bounds) || (long) bounds.m_LevelMask == (long) levelMask)
          return;
        // ISSUE: reference to a compiler-generated field
        bounds.m_LevelMask = levelMask;
        // ISSUE: reference to a compiler-generated field
        this.m_ClusterTree.Update(clusterIndex, bounds);
      }

      public NativeHeapBlock AllocateIcons(int iconCount)
      {
        NativeHeapBlock nativeHeapBlock;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (nativeHeapBlock = this.m_IconAllocator.Allocate((uint) iconCount); nativeHeapBlock.Empty; nativeHeapBlock = this.m_IconAllocator.Allocate((uint) iconCount))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconAllocator.Resize(this.m_IconAllocator.Size + 1024U);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ClusterIcons.ResizeUninitialized((int) this.m_IconAllocator.Size);
        }
        return nativeHeapBlock;
      }

      private void AddIcon(
        IconClusterSystem.ClusterIcon icon,
        NativeList<IconClusterSystem.ClusterIcon> icons)
      {
        if ((icon.flags & IconFlags.Unique) == (IconFlags) 0)
        {
          for (int index = 0; index < icons.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            IconClusterSystem.ClusterIcon icon1 = icons[index];
            if (icon1.prefab == icon.prefab && (icon1.flags & IconFlags.Unique) == (IconFlags) 0)
              return;
          }
        }
        icons.Add(in icon);
      }

      public int GetNewClusterIndex()
      {
        int newClusterIndex;
        // ISSUE: reference to a compiler-generated field
        if (this.m_FreeClusterIndices.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          newClusterIndex = this.m_FreeClusterIndices[this.m_FreeClusterIndices.Length - 1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_FreeClusterIndices.RemoveAt(this.m_FreeClusterIndices.Length - 1);
          // ISSUE: reference to a compiler-generated field
          ref NativeList<IconClusterSystem.IconCluster> local = ref this.m_IconClusters;
          int index = newClusterIndex;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster1 = new IconClusterSystem.IconCluster();
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster2 = iconCluster1;
          local[index] = iconCluster2;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          newClusterIndex = this.m_IconClusters.Length;
          // ISSUE: reference to a compiler-generated field
          ref NativeList<IconClusterSystem.IconCluster> local1 = ref this.m_IconClusters;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster = new IconClusterSystem.IconCluster();
          ref IconClusterSystem.IconCluster local2 = ref iconCluster;
          local1.Add(in local2);
        }
        return newClusterIndex;
      }

      private float GetCurrentCost(int clusterIndex, ref IconClusterSystem.IconCluster cluster)
      {
        int2 subClusters;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IconClusters.ElementAt(cluster.parentCluster).GetSubClusters(out subClusters);
        // ISSUE: reference to a compiler-generated field
        ref IconClusterSystem.IconCluster local = ref this.m_IconClusters.ElementAt(math.select(subClusters.x, subClusters.y, clusterIndex == subClusters.x));
        bool c = local.prefabIndex == cluster.prefabIndex && ((local.flags | cluster.flags) & IconFlags.Unique) == (IconFlags) 0;
        double a = (double) math.distancesq(local.center, cluster.center);
        return math.select((float) a, (float) (a * 0.25), c);
      }

      public void Remove(
        int clusterIndex,
        int subCluster,
        int subLevel,
        NativeList<UnsafeHashSet<int>> orphans)
      {
        // ISSUE: variable of a reference type
        IconClusterSystem.IconCluster& local1;
        for (; clusterIndex != 0; clusterIndex = local1.parentCluster)
        {
          // ISSUE: reference to a compiler-generated field
          local1 = ref this.m_IconClusters.ElementAt(clusterIndex);
          // ISSUE: reference to a compiler-generated field
          this.m_ClusterTree.TryRemove(clusterIndex);
          // ISSUE: reference to a compiler-generated method
          this.RemoveOrphan(clusterIndex, local1.level, orphans);
          // ISSUE: reference to a compiler-generated method
          NativeHeapBlock icons = local1.GetIcons(out int _, out int _);
          if (!icons.Empty)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconAllocator.Release(icons);
          }
          // ISSUE: reference to a compiler-generated field
          if (clusterIndex == this.m_IconClusters.Length - 1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconClusters.RemoveAt(clusterIndex);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_FreeClusterIndices.Add(in clusterIndex);
          }
          int2 subClusters;
          // ISSUE: reference to a compiler-generated method
          if (local1.GetSubClusters(out subClusters))
          {
            if (subClusters.x != subCluster)
            {
              // ISSUE: reference to a compiler-generated field
              ref IconClusterSystem.IconCluster local2 = ref this.m_IconClusters.ElementAt(subClusters.x);
              // ISSUE: reference to a compiler-generated method
              IconClusterSystem.IconCluster.SetParent(ref local2, 0);
              int level = math.max(local2.level, subLevel);
              // ISSUE: reference to a compiler-generated method
              this.AddOrphan(subClusters.x, level, orphans);
              // ISSUE: reference to a compiler-generated method
              this.UpdateLevelMask(subClusters.x, (ulong) (-1L << local2.level & ~(-1L << level + 1)));
            }
            if (subClusters.y != subCluster)
            {
              // ISSUE: reference to a compiler-generated field
              ref IconClusterSystem.IconCluster local3 = ref this.m_IconClusters.ElementAt(subClusters.y);
              // ISSUE: reference to a compiler-generated method
              IconClusterSystem.IconCluster.SetParent(ref local3, 0);
              int level = math.max(local3.level, subLevel);
              // ISSUE: reference to a compiler-generated method
              this.AddOrphan(subClusters.y, level, orphans);
              // ISSUE: reference to a compiler-generated method
              this.UpdateLevelMask(subClusters.y, (ulong) (-1L << local3.level & ~(-1L << level + 1)));
            }
          }
          subCluster = clusterIndex;
        }
      }

      private void RemoveTemp(int clusterIndex, NativeList<int> tempList)
      {
        tempList.Add(in clusterIndex);
        while (!tempList.IsEmpty)
        {
          clusterIndex = tempList[tempList.Length - 1];
          tempList.RemoveAt(tempList.Length - 1);
          // ISSUE: reference to a compiler-generated field
          ref IconClusterSystem.IconCluster local = ref this.m_IconClusters.ElementAt(clusterIndex);
          // ISSUE: reference to a compiler-generated method
          NativeHeapBlock icons = local.GetIcons(out int _, out int _);
          if (!icons.Empty)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconAllocator.Release(icons);
          }
          // ISSUE: reference to a compiler-generated field
          if (clusterIndex == this.m_IconClusters.Length - 1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_IconClusters.RemoveAt(clusterIndex);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_FreeClusterIndices.Add(in clusterIndex);
          }
          int2 subClusters;
          // ISSUE: reference to a compiler-generated method
          if (local.GetSubClusters(out subClusters))
          {
            tempList.Add(in subClusters.x);
            tempList.Add(in subClusters.y);
          }
        }
      }

      public void AddOrphan(int clusterIndex, int level, NativeList<UnsafeHashSet<int>> orphans)
      {
        if (orphans.Length <= level)
          orphans.Length = level + 1;
        ref UnsafeHashSet<int> local = ref orphans.ElementAt(level);
        if (!local.IsCreated)
          local = new UnsafeHashSet<int>(10, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        local.Add(clusterIndex);
      }

      private void RemoveOrphan(
        int clusterIndex,
        int level,
        NativeList<UnsafeHashSet<int>> orphans)
      {
        if (orphans.Length <= level)
          return;
        ref UnsafeHashSet<int> local = ref orphans.ElementAt(level);
        if (!local.IsCreated)
          return;
        local.Remove(clusterIndex);
      }

      private struct Selector : 
        INativeQuadTreeSelector<int, IconClusterSystem.TreeBounds, float>,
        IUnsafeQuadTreeSelector<int, IconClusterSystem.TreeBounds, float>
      {
        public float m_BestCost;
        public float m_BestDistance;
        public int m_BestClusterIndex;
        public int m_IgnoreClusterIndex;
        public ulong m_LevelMask;
        public ulong m_LayerMask;
        public IconClusterSystem.IconCluster m_Cluster;
        public NativeList<IconClusterSystem.IconCluster> m_IconClusters;

        public bool Check(IconClusterSystem.TreeBounds bounds, out float priority)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          priority = MathUtils.DistanceSquared(bounds.m_Bounds, this.m_Cluster.center);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return (double) priority <= (double) this.m_BestDistance && ((long) bounds.m_LevelMask & (long) this.m_LevelMask) != 0L && (bounds.m_LayerMask & this.m_LayerMask) > 0UL;
        }

        public bool Check(float priority) => (double) priority <= (double) this.m_BestDistance;

        public bool Better(float priority1, float priority2)
        {
          return (double) priority1 < (double) priority2;
        }

        public void Select(IconClusterSystem.TreeBounds bounds, int item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) MathUtils.DistanceSquared(bounds.m_Bounds, this.m_Cluster.center) > (double) this.m_BestDistance || ((long) bounds.m_LevelMask & (long) this.m_LevelMask) == 0L || ((long) bounds.m_LayerMask & (long) this.m_LayerMask) == 0L || item == this.m_IgnoreClusterIndex)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster = this.m_IconClusters[item];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool c = iconCluster.prefabIndex == this.m_Cluster.prefabIndex && ((iconCluster.flags | this.m_Cluster.flags) & IconFlags.Unique) == (IconFlags) 0;
          // ISSUE: reference to a compiler-generated field
          float a = math.distancesq(iconCluster.center, this.m_Cluster.center);
          float num = math.select(a, a * 0.25f, c);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if ((double) num >= (double) this.m_BestCost && ((double) num != (double) this.m_BestCost || iconCluster.parentCluster != 0))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_BestClusterIndex = item;
          // ISSUE: reference to a compiler-generated field
          this.m_BestCost = num;
          // ISSUE: reference to a compiler-generated field
          this.m_BestDistance = math.select(a * 4.01f, a * 1.01f, c);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DisallowCluster> __Game_Notifications_DisallowCluster_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Animation> __Game_Notifications_Animation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Moving> __Game_Objects_Moving_RO_ComponentLookup;
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_DisallowCluster_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DisallowCluster>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Animation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Animation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RO_ComponentLookup = state.GetComponentLookup<NotificationIconDisplayData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RO_ComponentLookup = state.GetComponentLookup<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>();
      }
    }
  }
}
