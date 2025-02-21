// Decompiled with JetBrains decompiler
// Type: Game.Notifications.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Notifications
{
  public static class RaycastJobs
  {
    [BurstCompile]
    public struct RaycastIconsJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public float3 m_CameraUp;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public IconClusterSystem.ClusterData m_ClusterData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_IconChunks;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Static> m_StaticData;
      [ReadOnly]
      public ComponentLookup<Object> m_ObjectData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<NotificationIconDisplayData> m_IconDisplayData;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        RaycastInput input = this.m_Input[index];
        if ((input.m_TypeMask & (TypeMask.StaticObjects | TypeMask.MovingObjects | TypeMask.Icons)) == TypeMask.None)
          return;
        if (!this.m_ClusterData.isEmpty)
          this.CheckClusters(index, input);
        if (this.m_IconChunks.Length == 0)
          return;
        this.CheckChunks(index, input);
      }

      private void CheckClusters(int raycastIndex, RaycastInput input)
      {
        IconLayerMask iconLayerMask = input.m_IconLayerMask;
        if ((input.m_TypeMask & TypeMask.StaticObjects) != TypeMask.None)
          iconLayerMask |= IconLayerMask.Marker;
        float3 a = input.m_Line.a;
        NativeList<IconClusterSystem.IconCluster> nativeList = new NativeList<IconClusterSystem.IconCluster>(64, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        int index1 = 0;
        // ISSUE: variable of a compiler-generated type
        IconClusterSystem.IconCluster cluster1;
        // ISSUE: reference to a compiler-generated method
        while (this.m_ClusterData.GetRoot(ref index1, out cluster1))
        {
          if ((NotificationsUtils.GetIconLayerMask(cluster1.layer) & iconLayerMask) != IconLayerMask.None)
            nativeList.Add(in cluster1);
        }
        while (nativeList.Length != 0)
        {
          // ISSUE: variable of a compiler-generated type
          IconClusterSystem.IconCluster iconCluster = nativeList[nativeList.Length - 1];
          nativeList.RemoveAtSwapBack(nativeList.Length - 1);
          float distance = math.distance(a, iconCluster.center);
          // ISSUE: reference to a compiler-generated method
          if (iconCluster.KeepCluster(distance))
          {
            // ISSUE: reference to a compiler-generated method
            float radius = iconCluster.GetRadius(distance);
            // ISSUE: reference to a compiler-generated method
            NativeArray<IconClusterSystem.ClusterIcon> icons = iconCluster.GetIcons(this.m_ClusterData);
            float3 float3_1 = this.m_CameraRight * (radius * 0.5f);
            float3 float3_2 = iconCluster.center + float3_1 * ((float) (icons.Length - 1) * 0.5f);
            float3 float3_3 = this.m_CameraUp * radius;
            RaycastResult result = new RaycastResult();
            float num1 = radius;
            for (int index2 = icons.Length - 1; index2 >= 0; --index2)
            {
              // ISSUE: variable of a compiler-generated type
              IconClusterSystem.ClusterIcon clusterIcon = icons[index2];
              if (!this.m_TempData.HasComponent(clusterIcon.icon))
              {
                float t;
                float num2 = MathUtils.Distance(input.m_Line, float3_2 + float3_3, out t);
                if ((double) num2 < (double) num1)
                {
                  num1 = num2;
                  result.m_Owner = clusterIcon.icon;
                  result.m_Hit.m_HitEntity = clusterIcon.icon;
                  result.m_Hit.m_Position = float3_2;
                  result.m_Hit.m_HitPosition = MathUtils.Position(input.m_Line, t);
                  result.m_Hit.m_NormalizedDistance = t - 100f / math.max(1f, MathUtils.Length(input.m_Line));
                }
              }
              float3_2 -= float3_1;
            }
            if (result.m_Owner != Entity.Null)
              this.ValidateResult(raycastIndex, input, result, iconCluster.layer);
          }
          else
          {
            int2 subClusters;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            if (iconCluster.GetSubClusters(out subClusters) && MathUtils.Intersect(iconCluster.GetBounds(distance, this.m_CameraUp), input.m_Line, out float2 _))
            {
              ref NativeList<IconClusterSystem.IconCluster> local1 = ref nativeList;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: variable of a compiler-generated type
              IconClusterSystem.IconCluster cluster2 = this.m_ClusterData.GetCluster(subClusters.x);
              ref IconClusterSystem.IconCluster local2 = ref cluster2;
              local1.Add(in local2);
              ref NativeList<IconClusterSystem.IconCluster> local3 = ref nativeList;
              // ISSUE: reference to a compiler-generated method
              cluster2 = this.m_ClusterData.GetCluster(subClusters.y);
              ref IconClusterSystem.IconCluster local4 = ref cluster2;
              local3.Add(in local4);
            }
          }
        }
        nativeList.Dispose();
      }

      private void CheckChunks(int raycastIndex, RaycastInput input)
      {
        IconLayerMask iconLayerMask = input.m_IconLayerMask;
        if ((input.m_TypeMask & (TypeMask.StaticObjects | TypeMask.MovingObjects)) != TypeMask.None)
          iconLayerMask |= IconLayerMask.Marker;
        float3 a = input.m_Line.a;
        for (int index1 = 0; index1 < this.m_IconChunks.Length; ++index1)
        {
          ArchetypeChunk iconChunk = this.m_IconChunks[index1];
          NativeArray<Entity> nativeArray1 = iconChunk.GetNativeArray(this.m_EntityType);
          NativeArray<Icon> nativeArray2 = iconChunk.GetNativeArray<Icon>(ref this.m_IconType);
          NativeArray<PrefabRef> nativeArray3 = iconChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Icon icon = nativeArray2[index2];
            if ((NotificationsUtils.GetIconLayerMask(icon.m_ClusterLayer) & iconLayerMask) != IconLayerMask.None)
            {
              PrefabRef prefabRef = nativeArray3[index2];
              if (this.m_IconDisplayData.IsComponentEnabled(prefabRef.m_Prefab))
              {
                NotificationIconDisplayData notificationIconDisplayData = this.m_IconDisplayData[prefabRef.m_Prefab];
                float s = (float) icon.m_Priority * 0.003921569f;
                // ISSUE: reference to a compiler-generated method
                float radius = IconClusterSystem.IconCluster.CalculateRadius(math.lerp(notificationIconDisplayData.m_MinParams, notificationIconDisplayData.m_MaxParams, s).x, math.distance(icon.m_Location, a));
                float3 float3 = this.m_CameraUp * radius;
                float t;
                if ((double) MathUtils.Distance(input.m_Line, icon.m_Location + float3, out t) < (double) radius)
                {
                  RaycastResult result = new RaycastResult()
                  {
                    m_Owner = nativeArray1[index2]
                  };
                  result.m_Hit.m_HitEntity = result.m_Owner;
                  result.m_Hit.m_Position = icon.m_Location;
                  result.m_Hit.m_HitPosition = MathUtils.Position(input.m_Line, t);
                  result.m_Hit.m_NormalizedDistance = t - 100f / math.max(1f, MathUtils.Length(input.m_Line));
                  if ((icon.m_Flags & IconFlags.OnTop) != (IconFlags) 0)
                    result.m_Hit.m_NormalizedDistance *= 0.999f;
                  this.ValidateResult(raycastIndex, input, result, icon.m_ClusterLayer);
                }
              }
            }
          }
        }
      }

      private void ValidateResult(
        int raycastIndex,
        RaycastInput input,
        RaycastResult result,
        IconClusterLayer layer)
      {
        if ((input.m_IconLayerMask & NotificationsUtils.GetIconLayerMask(layer)) != IconLayerMask.None)
          this.m_Results.Accumulate(raycastIndex, result);
        while (this.m_OwnerData.HasComponent(result.m_Owner))
        {
          result.m_Owner = this.m_OwnerData[result.m_Owner].m_Owner;
          if (this.m_ObjectData.HasComponent(result.m_Owner))
          {
            if (this.m_StaticData.HasComponent(result.m_Owner))
            {
              if ((input.m_TypeMask & TypeMask.StaticObjects) == TypeMask.None)
                break;
              if (this.CheckPlaceholder(input, ref result.m_Owner))
              {
                result.m_Hit.m_Position = this.m_TransformData[result.m_Owner].m_Position;
                this.m_Results.Accumulate(raycastIndex, result);
                break;
              }
            }
            else
            {
              if ((input.m_TypeMask & TypeMask.MovingObjects) == TypeMask.None)
                break;
              result.m_Hit.m_Position = MathUtils.Center(this.m_CullingInfoData[result.m_Owner].m_Bounds);
              this.m_Results.Accumulate(raycastIndex, result);
              break;
            }
          }
        }
      }

      private bool CheckPlaceholder(RaycastInput input, ref Entity entity)
      {
        if ((input.m_Flags & RaycastFlags.Placeholders) != (RaycastFlags) 0 || !this.m_PlaceholderData.HasComponent(entity))
          return true;
        if (this.m_AttachmentData.HasComponent(entity))
        {
          Attachment attachment = this.m_AttachmentData[entity];
          if (this.m_TransformData.HasComponent(attachment.m_Attached))
          {
            entity = attachment.m_Attached;
            return true;
          }
        }
        return false;
      }
    }
  }
}
