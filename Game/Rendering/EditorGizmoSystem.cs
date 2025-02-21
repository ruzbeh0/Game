// Decompiled with JetBrains decompiler
// Type: Game.Rendering.EditorGizmoSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class EditorGizmoSystem : GameSystemBase
  {
    private EntityQuery m_RenderQuery;
    private EntityQuery m_RenderingSettingsQuery;
    private GizmosSystem m_GizmosSystem;
    private PreCullingSystem m_PreCullingSystem;
    private RenderingSystem m_RenderingSystem;
    private EditorGizmoSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GizmosSystem = this.World.GetOrCreateSystemManaged<GizmosSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Tools.EditorContainer>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Hidden>());
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<RenderingSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_RenderQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_RenderingSystem.hideOverlay)
        return;
      UnityEngine.Color color1 = new UnityEngine.Color(0.5f, 0.5f, 1f, 1f);
      UnityEngine.Color color2 = new UnityEngine.Color(1f, 0.5f, 0.5f, 1f);
      UnityEngine.Color color3 = new UnityEngine.Color(1f, 1f, 0.5f, 1f);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        RenderingSettingsData singleton = this.m_RenderingSettingsQuery.GetSingleton<RenderingSettingsData>();
        color1 = singleton.m_HoveredColor;
        color2 = singleton.m_ErrorColor;
        color3 = singleton.m_WarningColor;
        color1.a = 1f;
        color2.a = 1f;
        color3.a = 1f;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new EditorGizmoSystem.EditorGizmoJob()
      {
        m_NetNodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_NetCurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_ErrorType = this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle,
        m_WarningType = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle,
        m_HighlightedType = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle,
        m_CullingInfoType = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle,
        m_HoveredColor = color1,
        m_ErrorColor = color2,
        m_WarningColor = color3,
        m_CullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies1),
        m_GizmoBatcher = this.m_GizmosSystem.GetGizmosBatcher(out dependencies2)
      }.ScheduleParallel<EditorGizmoSystem.EditorGizmoJob>(this.m_RenderQuery, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated field
      this.m_GizmosSystem.AddGizmosBatcherWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(jobHandle);
      this.Dependency = jobHandle;
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
    public EditorGizmoSystem()
    {
    }

    [BurstCompile]
    private struct EditorGizmoJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Node> m_NetNodeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_NetCurveType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Error> m_ErrorType;
      [ReadOnly]
      public ComponentTypeHandle<Warning> m_WarningType;
      [ReadOnly]
      public ComponentTypeHandle<Highlighted> m_HighlightedType;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> m_CullingInfoType;
      [ReadOnly]
      public UnityEngine.Color m_HoveredColor;
      [ReadOnly]
      public UnityEngine.Color m_ErrorColor;
      [ReadOnly]
      public UnityEngine.Color m_WarningColor;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      public GizmoBatcher m_GizmoBatcher;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Node> nativeArray1 = chunk.GetNativeArray<Node>(ref this.m_NetNodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_NetCurveType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CullingInfo> nativeArray5 = chunk.GetNativeArray<CullingInfo>(ref this.m_CullingInfoType);
        bool flag;
        UnityEngine.Color color1;
        UnityEngine.Color color2;
        UnityEngine.Color color3;
        UnityEngine.Color color4;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType))
        {
          flag = false;
          // ISSUE: reference to a compiler-generated field
          color1 = this.m_ErrorColor;
          // ISSUE: reference to a compiler-generated field
          color2 = this.m_ErrorColor;
          // ISSUE: reference to a compiler-generated field
          color3 = this.m_ErrorColor;
          // ISSUE: reference to a compiler-generated field
          color4 = this.m_ErrorColor;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Warning>(ref this.m_WarningType))
          {
            flag = false;
            // ISSUE: reference to a compiler-generated field
            color1 = this.m_WarningColor;
            // ISSUE: reference to a compiler-generated field
            color2 = this.m_WarningColor;
            // ISSUE: reference to a compiler-generated field
            color3 = this.m_WarningColor;
            // ISSUE: reference to a compiler-generated field
            color4 = this.m_WarningColor;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Highlighted>(ref this.m_HighlightedType))
            {
              flag = false;
              // ISSUE: reference to a compiler-generated field
              color1 = this.m_HoveredColor;
              // ISSUE: reference to a compiler-generated field
              color2 = this.m_HoveredColor;
              // ISSUE: reference to a compiler-generated field
              color3 = this.m_HoveredColor;
              // ISSUE: reference to a compiler-generated field
              color4 = this.m_HoveredColor;
            }
            else
            {
              flag = nativeArray4.Length != 0;
              color1 = UnityEngine.Color.white;
              color2 = UnityEngine.Color.red;
              color3 = UnityEngine.Color.green;
              color4 = UnityEngine.Color.blue;
            }
          }
        }
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsNearCamera(nativeArray5[index]))
          {
            UnityEngine.Color color5 = color1;
            if (flag)
            {
              Temp temp = nativeArray4[index];
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color5 = this.m_HoveredColor;
                }
              }
              else
                continue;
            }
            Node node = nativeArray1[index];
            float3 float3_1 = math.rotate(node.m_Rotation, math.right());
            float3 float3_2 = math.rotate(node.m_Rotation, math.up());
            float3 float3_3 = math.rotate(node.m_Rotation, math.forward());
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawLine(node.m_Position - float3_1, node.m_Position + float3_1, color5);
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawLine(node.m_Position - float3_2, node.m_Position + float3_2, color5);
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawLine(node.m_Position - float3_3, node.m_Position + float3_3, color5);
          }
        }
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsNearCamera(nativeArray5[index]))
          {
            UnityEngine.Color color6 = color1;
            if (flag)
            {
              Temp temp = nativeArray4[index];
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color6 = this.m_HoveredColor;
                }
              }
              else
                continue;
            }
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawCurve(nativeArray2[index], color6);
          }
        }
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.IsNearCamera(nativeArray5[index]))
          {
            UnityEngine.Color color7 = color2;
            UnityEngine.Color color8 = color3;
            UnityEngine.Color color9 = color4;
            if (flag)
            {
              Temp temp = nativeArray4[index];
              if ((temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace)) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color7 = this.m_HoveredColor;
                  // ISSUE: reference to a compiler-generated field
                  color8 = this.m_HoveredColor;
                  // ISSUE: reference to a compiler-generated field
                  color9 = this.m_HoveredColor;
                }
              }
              else
                continue;
            }
            Game.Objects.Transform transform = nativeArray3[index];
            float3 float3_4 = math.rotate(transform.m_Rotation, math.right());
            float3 float3_5 = math.rotate(transform.m_Rotation, math.up());
            float3 float3_6 = math.rotate(transform.m_Rotation, math.forward());
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawArrow(transform.m_Position - float3_4, transform.m_Position + float3_4, color7);
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawArrow(transform.m_Position - float3_5, transform.m_Position + float3_5, color8);
            // ISSUE: reference to a compiler-generated field
            this.m_GizmoBatcher.DrawArrow(transform.m_Position - float3_6, transform.m_Position + float3_6, color9);
          }
        }
      }

      private bool IsNearCamera(CullingInfo cullingInfo)
      {
        // ISSUE: reference to a compiler-generated field
        return cullingInfo.m_CullingIndex != 0 && (this.m_CullingData[cullingInfo.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Error> __Game_Tools_Error_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Warning> __Game_Tools_Warning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Highlighted> __Game_Tools_Highlighted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Highlighted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Highlighted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CullingInfo>(true);
      }
    }
  }
}
