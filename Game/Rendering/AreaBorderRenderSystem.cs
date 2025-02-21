// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaBorderRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
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
  public class AreaBorderRenderSystem : GameSystemBase
  {
    private OverlayRenderSystem m_OverlayRenderSystem;
    private ToolSystem m_ToolSystem;
    private EntityQuery m_AreaBorderQuery;
    private EntityQuery m_RenderingSettingsQuery;
    private AreaBorderRenderSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaBorderQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Area>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Error>(),
          ComponentType.ReadOnly<Warning>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Hidden>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<RenderingSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AreaBorderQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      RenderingSettingsData renderingSettingsData = new RenderingSettingsData()
      {
        m_HoveredColor = new Color(0.5f, 0.5f, 1f, 0.5f),
        m_ErrorColor = new Color(1f, 0.25f, 0.25f, 0.5f),
        m_WarningColor = new Color(1f, 1f, 0.25f, 0.5f),
        m_OwnerColor = new Color(0.5f, 1f, 0.5f, 0.5f)
      };
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RenderingSettingsQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        RenderingSettingsData singleton = this.m_RenderingSettingsQuery.GetSingleton<RenderingSettingsData>();
        renderingSettingsData.m_HoveredColor = singleton.m_HoveredColor;
        renderingSettingsData.m_ErrorColor = singleton.m_ErrorColor;
        renderingSettingsData.m_WarningColor = singleton.m_WarningColor;
        renderingSettingsData.m_OwnerColor = singleton.m_OwnerColor;
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_AreaBorderQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new AreaBorderRenderSystem.AreaBorderRenderJob()
      {
        m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
        m_LotType = this.__TypeHandle.__Game_Areas_Lot_RO_ComponentTypeHandle,
        m_MapTileType = this.__TypeHandle.__Game_Areas_MapTile_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_WarningType = this.__TypeHandle.__Game_Tools_Warning_RO_ComponentTypeHandle,
        m_ErrorType = this.__TypeHandle.__Game_Tools_Error_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_RenderingSettingsData = renderingSettingsData,
        m_Chunks = archetypeChunkListAsync,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies)
      }.Schedule<AreaBorderRenderSystem.AreaBorderRenderJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
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
    public AreaBorderRenderSystem()
    {
    }

    private struct Border : IEquatable<AreaBorderRenderSystem.Border>
    {
      public float3 m_StartPos;
      public float3 m_EndPos;

      public bool Equals(AreaBorderRenderSystem.Border other)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_StartPos.Equals(other.m_StartPos) & this.m_EndPos.Equals(other.m_EndPos);
      }

      public override int GetHashCode() => this.m_StartPos.GetHashCode();
    }

    [BurstCompile]
    private struct AreaBorderRenderJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Lot> m_LotType;
      [ReadOnly]
      public ComponentTypeHandle<MapTile> m_MapTileType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Warning> m_WarningType;
      [ReadOnly]
      public ComponentTypeHandle<Error> m_ErrorType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Node> m_NodeType;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public RenderingSettingsData m_RenderingSettingsData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public bool m_EditorMode;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;

      public void Execute()
      {
        int capacity = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Area> nativeArray = chunk.GetNativeArray<Area>(ref this.m_AreaType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Node> bufferAccessor = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            if ((nativeArray[index2].m_Flags & AreaFlags.Slave) == (AreaFlags) 0)
              capacity += bufferAccessor[index2].Length;
          }
        }
        NativeParallelHashSet<AreaBorderRenderSystem.Border> borderMap = new NativeParallelHashSet<AreaBorderRenderSystem.Border>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        NativeParallelHashSet<float3> nodeMap = new NativeParallelHashSet<float3>(capacity, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddBorders(this.m_Chunks[index], borderMap);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.DrawBorders(this.m_Chunks[index], borderMap, nodeMap);
        }
        borderMap.Dispose();
        nodeMap.Dispose();
      }

      private void AddBorders(
        ArchetypeChunk chunk,
        NativeParallelHashSet<AreaBorderRenderSystem.Border> borderMap)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Area> nativeArray1 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Node> bufferAccessor = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag = !chunk.Has<Error>(ref this.m_ErrorType) && !chunk.Has<Warning>(ref this.m_WarningType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          if (flag)
          {
            Temp temp = nativeArray2[index1];
            if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) == (TempFlags) 0 || (temp.m_Flags & TempFlags.Hidden) != (TempFlags) 0)
              continue;
          }
          Area area = nativeArray1[index1];
          if ((area.m_Flags & AreaFlags.Slave) == (AreaFlags) 0)
          {
            DynamicBuffer<Node> dynamicBuffer = bufferAccessor[index1];
            float3 float3 = dynamicBuffer[0].m_Position;
            for (int index2 = 1; index2 < dynamicBuffer.Length; ++index2)
            {
              float3 position = dynamicBuffer[index2].m_Position;
              if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
              {
                // ISSUE: object of a compiler-generated type is created
                borderMap.Add(new AreaBorderRenderSystem.Border()
                {
                  m_StartPos = position,
                  m_EndPos = float3
                });
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                borderMap.Add(new AreaBorderRenderSystem.Border()
                {
                  m_StartPos = float3,
                  m_EndPos = position
                });
              }
              float3 = position;
            }
            if ((area.m_Flags & AreaFlags.Complete) != (AreaFlags) 0)
            {
              float3 position = dynamicBuffer[0].m_Position;
              if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
              {
                // ISSUE: object of a compiler-generated type is created
                borderMap.Add(new AreaBorderRenderSystem.Border()
                {
                  m_StartPos = position,
                  m_EndPos = float3
                });
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                borderMap.Add(new AreaBorderRenderSystem.Border()
                {
                  m_StartPos = float3,
                  m_EndPos = position
                });
              }
            }
          }
        }
      }

      private void DrawBorders(
        ArchetypeChunk chunk,
        NativeParallelHashSet<AreaBorderRenderSystem.Border> borderMap,
        NativeParallelHashSet<float3> nodeMap)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Area> nativeArray1 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Node> bufferAccessor = chunk.GetBufferAccessor<Node>(ref this.m_NodeType);
        OverlayRenderSystem.StyleFlags styleFlags = (OverlayRenderSystem.StyleFlags) 0;
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Lot>(ref this.m_LotType);
        bool flag2;
        bool dashedLines;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<MapTile>(ref this.m_MapTileType))
        {
          styleFlags |= OverlayRenderSystem.StyleFlags.Projected;
          // ISSUE: reference to a compiler-generated field
          if (this.m_EditorMode)
          {
            flag2 = true;
            dashedLines = false;
          }
          else
          {
            flag2 = false;
            dashedLines = true;
          }
        }
        else
        {
          flag2 = true;
          dashedLines = false;
        }
        bool flag3;
        Color color1;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Error>(ref this.m_ErrorType))
        {
          flag3 = false;
          // ISSUE: reference to a compiler-generated field
          color1 = this.m_RenderingSettingsData.m_ErrorColor;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Warning>(ref this.m_WarningType))
          {
            flag3 = false;
            // ISSUE: reference to a compiler-generated field
            color1 = this.m_RenderingSettingsData.m_WarningColor;
          }
          else
          {
            flag3 = true;
            dashedLines = false;
            // ISSUE: reference to a compiler-generated field
            color1 = this.m_RenderingSettingsData.m_HoveredColor;
          }
        }
        Color color2 = new Color(1f, 1f, 1f, 0.5f);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Color color3 = color1;
          bool flag4 = flag2;
          bool flag5;
          if (nativeArray2.Length != 0)
          {
            Temp temp = nativeArray2[index1];
            if (flag3)
            {
              if ((temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify | TempFlags.Replace | TempFlags.Parent)) != (TempFlags) 0 && (temp.m_Flags & TempFlags.Hidden) == (TempFlags) 0)
              {
                if ((temp.m_Flags & TempFlags.Parent) != (TempFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  color3 = this.m_RenderingSettingsData.m_OwnerColor;
                }
              }
              else
                continue;
            }
            flag5 = flag4 & (temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) > (TempFlags) 0;
          }
          else
            flag5 = false;
          Area area = nativeArray1[index1];
          if ((area.m_Flags & AreaFlags.Slave) == (AreaFlags) 0)
          {
            Entity prefab = nativeArray3[index1].m_Prefab;
            DynamicBuffer<Node> dynamicBuffer = bufferAccessor[index1];
            // ISSUE: reference to a compiler-generated field
            AreaGeometryData geometryData = this.m_PrefabGeometryData[prefab];
            float3 float3 = dynamicBuffer[0].m_Position;
            if (dynamicBuffer.Length == 1)
            {
              if (flag5 && nodeMap.Add(float3))
              {
                // ISSUE: reference to a compiler-generated method
                this.DrawNode(color2, float3, geometryData, styleFlags);
              }
            }
            else
            {
              for (int index2 = 1; index2 < dynamicBuffer.Length; ++index2)
              {
                float3 position = dynamicBuffer[index2].m_Position;
                // ISSUE: variable of a compiler-generated type
                AreaBorderRenderSystem.Border border;
                if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
                {
                  // ISSUE: object of a compiler-generated type is created
                  border = new AreaBorderRenderSystem.Border()
                  {
                    m_StartPos = float3,
                    m_EndPos = position
                  };
                }
                else
                {
                  // ISSUE: object of a compiler-generated type is created
                  border = new AreaBorderRenderSystem.Border()
                  {
                    m_StartPos = position,
                    m_EndPos = float3
                  };
                }
                if (flag5 && nodeMap.Add(float3))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DrawNode(color2, float3, geometryData, styleFlags);
                }
                if (!borderMap.Contains(border))
                {
                  if (flag1 && index2 == 1)
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.DrawEdge(color3 * 0.5f, float3, position, geometryData, dashedLines, styleFlags);
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.DrawEdge(color3, float3, position, geometryData, dashedLines, styleFlags);
                  }
                }
                if (flag5 && nodeMap.Add(position))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DrawNode(color2, position, geometryData, styleFlags);
                }
                float3 = position;
              }
              if ((area.m_Flags & AreaFlags.Complete) != (AreaFlags) 0)
              {
                float3 position = dynamicBuffer[0].m_Position;
                // ISSUE: variable of a compiler-generated type
                AreaBorderRenderSystem.Border border;
                if ((area.m_Flags & AreaFlags.CounterClockwise) != (AreaFlags) 0)
                {
                  // ISSUE: object of a compiler-generated type is created
                  border = new AreaBorderRenderSystem.Border()
                  {
                    m_StartPos = float3,
                    m_EndPos = position
                  };
                }
                else
                {
                  // ISSUE: object of a compiler-generated type is created
                  border = new AreaBorderRenderSystem.Border()
                  {
                    m_StartPos = position,
                    m_EndPos = float3
                  };
                }
                if (flag5 && nodeMap.Add(float3))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DrawNode(color2, float3, geometryData, styleFlags);
                }
                if (!borderMap.Contains(border))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DrawEdge(color3, float3, position, geometryData, dashedLines, styleFlags);
                }
                if (flag5 && nodeMap.Add(position))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.DrawNode(color2, position, geometryData, styleFlags);
                }
              }
            }
          }
        }
      }

      private void DrawNode(
        Color color,
        float3 position,
        AreaGeometryData geometryData,
        OverlayRenderSystem.StyleFlags styleFlags)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_OverlayBuffer.DrawCircle(color, color, 0.0f, styleFlags, new float2(0.0f, 1f), position, geometryData.m_SnapDistance * 0.3f);
      }

      private void DrawEdge(
        Color color,
        float3 startPos,
        float3 endPos,
        AreaGeometryData geometryData,
        bool dashedLines,
        OverlayRenderSystem.StyleFlags styleFlags)
      {
        Line3.Segment line = new Line3.Segment(startPos, endPos);
        if (dashedLines)
        {
          float num1 = math.distance(startPos.xz, endPos.xz);
          float num2 = num1 / math.max(1f, math.round(num1 / (geometryData.m_SnapDistance * 1.25f)));
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawDashedLine(color, color, 0.0f, styleFlags, line, geometryData.m_SnapDistance * 0.2f, num2 * 0.55f, num2 * 0.45f);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_OverlayBuffer.DrawLine(color, color, 0.0f, styleFlags, line, geometryData.m_SnapDistance * 0.3f, (float2) 1f);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Area> __Game_Areas_Area_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Lot> __Game_Areas_Lot_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<MapTile> __Game_Areas_MapTile_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Warning> __Game_Tools_Warning_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Error> __Game_Tools_Error_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Lot_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapTile_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MapTile>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Warning_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Warning>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Error_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Error>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
      }
    }
  }
}
