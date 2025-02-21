// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.NetCourseTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Tools;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class NetCourseTooltipSystem : TooltipSystemBase
  {
    private const float kMinLength = 12f;
    private ToolSystem m_ToolSystem;
    private NetToolSystem m_NetTool;
    private EntityQuery m_NetCourseQuery;
    private TooltipGroup m_Group;
    private FloatTooltip m_Length;
    private FloatTooltip m_Slope;
    private NetCourseTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetTool = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetCourseQuery = this.GetEntityQuery(ComponentType.ReadOnly<CreationDefinition>(), ComponentType.ReadOnly<NetCourse>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_NetCourseQuery);
      FloatTooltip floatTooltip1 = new FloatTooltip();
      floatTooltip1.icon = "Media/Glyphs/Length.svg";
      floatTooltip1.unit = "length";
      // ISSUE: reference to a compiler-generated field
      this.m_Length = floatTooltip1;
      FloatTooltip floatTooltip2 = new FloatTooltip();
      floatTooltip2.icon = "Media/Glyphs/Slope.svg";
      floatTooltip2.unit = "percentageSingleFraction";
      floatTooltip2.signed = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Slope = floatTooltip2;
      TooltipGroup tooltipGroup = new TooltipGroup();
      tooltipGroup.path = (PathSegment) "tempNetEdgeStart";
      tooltipGroup.horizontalAlignment = TooltipGroup.Alignment.Center;
      tooltipGroup.verticalAlignment = TooltipGroup.Alignment.Center;
      tooltipGroup.category = TooltipGroup.Category.Network;
      // ISSUE: reference to a compiler-generated field
      this.m_Group = tooltipGroup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Group.children.Add((IWidget) this.m_Length);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Group.children.Add((IWidget) this.m_Slope);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != this.m_NetTool || this.m_NetTool.mode == NetToolSystem.Mode.Replace || !((Object) Camera.main != (Object) null))
        return;
      this.CompleteDependency();
      // ISSUE: reference to a compiler-generated field
      NativeList<NetCourse> courses = new NativeList<NetCourse>(this.m_NetCourseQuery.CalculateEntityCount(), (AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_NetCourseQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NetCourse> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_NetCourse_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<CreationDefinition> componentTypeHandle2 = this.__TypeHandle.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle;
        float num1 = 0.0f;
        float num2 = 0.0f;
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          NativeArray<NetCourse> nativeArray1 = archetypeChunk.GetNativeArray<NetCourse>(ref componentTypeHandle1);
          NativeArray<CreationDefinition> nativeArray2 = archetypeChunk.GetNativeArray<CreationDefinition>(ref componentTypeHandle2);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            NetCourse netCourse = nativeArray1[index];
            CreationDefinition creationDefinition = nativeArray2[index];
            if (!(creationDefinition.m_Original != Entity.Null) && (creationDefinition.m_Flags & (CreationFlags.Permanent | CreationFlags.Delete | CreationFlags.Upgrade | CreationFlags.Invert | CreationFlags.Align)) == (CreationFlags) 0 && (netCourse.m_StartPosition.m_Flags & CoursePosFlags.IsParallel) == (CoursePosFlags) 0)
            {
              num1 += netCourse.m_Length;
              float2 t = new float2(netCourse.m_StartPosition.m_CourseDelta, netCourse.m_EndPosition.m_CourseDelta);
              Bezier4x2 xz = MathUtils.Cut(netCourse.m_Curve, t).xz;
              num2 += MathUtils.Length(xz);
              courses.Add(in netCourse);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Length.value = num2;
        if (courses.Length == 0 || (double) num2 < 12.0)
          return;
        float y = courses[0].m_StartPosition.m_Position.y;
        ref NativeList<NetCourse> local = ref courses;
        // ISSUE: reference to a compiler-generated field
        this.m_Slope.value = (float) (100.0 * ((double) local[local.Length - 1].m_EndPosition.m_Position.y - (double) y)) / num2;
        // ISSUE: reference to a compiler-generated method
        NetCourseTooltipSystem.SortCourses(courses);
        float length = num1 / 2f;
        bool onScreen;
        // ISSUE: reference to a compiler-generated method
        float2 tooltipPos = TooltipSystemBase.WorldToTooltipPos((Vector3) NetCourseTooltipSystem.GetWorldPosition(courses, length), out onScreen);
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Group.position.Equals(tooltipPos))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Group.position = tooltipPos;
          // ISSUE: reference to a compiler-generated field
          this.m_Group.SetChildrenChanged();
        }
        if (onScreen)
        {
          // ISSUE: reference to a compiler-generated field
          this.AddGroup(this.m_Group);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Length);
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Slope);
        }
      }
      finally
      {
        courses.Dispose();
        archetypeChunkArray.Dispose();
      }
    }

    private static float3 GetWorldPosition(NativeList<NetCourse> courses, float length)
    {
      float num = -length;
      foreach (NetCourse course in courses)
      {
        num += course.m_Length;
        if ((double) num >= 0.0 && (double) course.m_Length != 0.0)
        {
          float t = math.lerp(course.m_StartPosition.m_CourseDelta, course.m_EndPosition.m_CourseDelta, (float) (1.0 - (double) num / (double) course.m_Length));
          return MathUtils.Position(course.m_Curve, t);
        }
      }
      ref NativeList<NetCourse> local = ref courses;
      return local[local.Length - 1].m_EndPosition.m_Position;
    }

    private static void SortCourses(NativeList<NetCourse> courses)
    {
      NativeArray<NetCourse> nativeArray = courses.AsArray();
      for (int index = 0; index < nativeArray.Length; ++index)
      {
        NetCourse course = courses[index];
        if ((course.m_StartPosition.m_Flags & CoursePosFlags.IsFirst) != (CoursePosFlags) 0)
        {
          courses[index] = courses[0];
          courses[0] = course;
          break;
        }
      }
      for (int index1 = 0; index1 < courses.Length - 1; ++index1)
      {
        NetCourse course1 = courses[index1];
        for (int index2 = index1 + 1; index2 < courses.Length; ++index2)
        {
          NetCourse course2 = courses[index2];
          if (course1.m_EndPosition.m_Position.Equals(course2.m_StartPosition.m_Position))
          {
            courses[index2] = courses[index1 + 1];
            courses[index1 + 1] = course2;
            break;
          }
        }
      }
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
    public NetCourseTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<NetCourse> __Game_Tools_NetCourse_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CreationDefinition> __Game_Tools_CreationDefinition_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_NetCourse_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCourse>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_CreationDefinition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CreationDefinition>(true);
      }
    }
  }
}
