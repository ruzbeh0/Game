// Decompiled with JetBrains decompiler
// Type: Game.Rendering.UndergroundViewSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class UndergroundViewSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private UtilityLodUpdateSystem m_UtilityLodUpdateSystem;
    private RenderingSystem m_RenderingSystem;
    private EntityQuery m_InfomodeQuery;
    private bool m_LastWasWaterways;
    private bool m_LastWasMarkers;
    private bool m_Loaded;
    private UtilityTypes m_LastUtilityTypes;
    private UndergroundViewSystem.TypeHandle __TypeHandle;

    public bool undergroundOn { get; private set; }

    public bool tunnelsOn { get; private set; }

    public bool pipelinesOn { get; private set; }

    public bool subPipelinesOn { get; private set; }

    public bool waterwaysOn { get; private set; }

    public bool contourLinesOn { get; private set; }

    public bool markersOn { get; private set; }

    public UtilityTypes utilityTypes { get; private set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UtilityLodUpdateSystem = this.World.GetOrCreateSystemManaged<UtilityLodUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<InfomodeActive>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<InfoviewNetGeometryData>(),
          ComponentType.ReadOnly<InfoviewNetStatusData>(),
          ComponentType.ReadOnly<InfoviewCoverageData>()
        }
      });
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
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

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      bool loaded = this.GetLoaded();
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != null)
      {
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.activeTool.GetAvailableSnapMask(out onMask, out offMask);
        // ISSUE: reference to a compiler-generated field
        this.undergroundOn = this.m_ToolSystem.activeTool.requireUnderground;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.tunnelsOn = this.m_ToolSystem.activeTool.requireUnderground || (this.m_ToolSystem.activeTool.requireNet & (Layer.Road | Layer.TrainTrack | Layer.Pathway | Layer.TramTrack | Layer.SubwayTrack | Layer.PublicTransportRoad)) > Layer.None;
        // ISSUE: reference to a compiler-generated field
        this.subPipelinesOn = (this.m_ToolSystem.activeTool.requireNet & (Layer.PowerlineLow | Layer.PowerlineHigh | Layer.WaterPipe | Layer.SewagePipe)) > Layer.None;
        // ISSUE: reference to a compiler-generated field
        this.pipelinesOn = this.m_ToolSystem.activeTool.requirePipelines || this.subPipelinesOn || this.undergroundOn && this.tunnelsOn;
        // ISSUE: reference to a compiler-generated field
        this.waterwaysOn = (this.m_ToolSystem.activeTool.requireNet & Layer.Waterway) > Layer.None;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.contourLinesOn = (ToolBaseSystem.GetActualSnap(this.m_ToolSystem.activeTool.selectedSnap, onMask, offMask) & Snap.ContourLines) > Snap.None;
      }
      else
      {
        this.undergroundOn = false;
        this.tunnelsOn = false;
        this.pipelinesOn = false;
        this.subPipelinesOn = false;
        this.waterwaysOn = false;
        this.contourLinesOn = false;
      }
      // ISSUE: reference to a compiler-generated field
      this.markersOn = !this.m_RenderingSystem.hideOverlay;
      this.utilityTypes = UtilityTypes.None;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_InfomodeQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewNetGeometryData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewNetStatusData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<InfoviewCoverageData> componentTypeHandle3 = this.__TypeHandle.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_InfomodeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<InfoviewNetGeometryData> nativeArray1 = archetypeChunk.GetNativeArray<InfoviewNetGeometryData>(ref componentTypeHandle1);
          NativeArray<InfoviewNetStatusData> nativeArray2 = archetypeChunk.GetNativeArray<InfoviewNetStatusData>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            switch (nativeArray1[index2].m_Type)
            {
              case NetType.Road:
                this.tunnelsOn = true;
                break;
              case NetType.TrainTrack:
                this.tunnelsOn = true;
                break;
              case NetType.TramTrack:
                this.tunnelsOn = true;
                break;
              case NetType.Waterway:
                this.waterwaysOn = true;
                break;
              case NetType.SubwayTrack:
                this.tunnelsOn = true;
                break;
            }
          }
          for (int index3 = 0; index3 < nativeArray2.Length; ++index3)
          {
            switch (nativeArray2[index3].m_Type)
            {
              case NetStatusType.Wear:
                this.tunnelsOn = true;
                break;
              case NetStatusType.TrafficFlow:
                this.tunnelsOn = true;
                break;
              case NetStatusType.TrafficVolume:
                this.tunnelsOn = true;
                break;
              case NetStatusType.LowVoltageFlow:
                this.pipelinesOn = true;
                this.subPipelinesOn = true;
                this.utilityTypes |= UtilityTypes.LowVoltageLine;
                break;
              case NetStatusType.HighVoltageFlow:
                this.pipelinesOn = true;
                this.subPipelinesOn = true;
                this.utilityTypes |= UtilityTypes.HighVoltageLine;
                break;
              case NetStatusType.PipeWaterFlow:
                this.pipelinesOn = true;
                this.subPipelinesOn = true;
                this.utilityTypes |= UtilityTypes.WaterPipe;
                break;
              case NetStatusType.PipeSewageFlow:
                this.pipelinesOn = true;
                this.subPipelinesOn = true;
                this.utilityTypes |= UtilityTypes.SewagePipe;
                break;
            }
          }
          if (archetypeChunk.Has<InfoviewCoverageData>(ref componentTypeHandle3))
            this.tunnelsOn = true;
        }
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.utilityTypes != this.m_LastUtilityTypes)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastUtilityTypes = this.utilityTypes;
        if (!loaded)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UtilityLodUpdateSystem.Update();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.waterwaysOn != this.m_LastWasWaterways)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LastWasWaterways = this.waterwaysOn;
        Camera main = Camera.main;
        if ((Object) main != (Object) null)
        {
          if (this.waterwaysOn)
            main.cullingMask |= 1 << LayerMask.NameToLayer("Waterway");
          else
            main.cullingMask &= ~(1 << LayerMask.NameToLayer("Waterway"));
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.markersOn == this.m_LastWasMarkers)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_LastWasMarkers = this.markersOn;
      Camera main1 = Camera.main;
      if (!((Object) main1 != (Object) null))
        return;
      if (this.markersOn)
        main1.cullingMask |= 1 << LayerMask.NameToLayer("Marker");
      else
        main1.cullingMask &= ~(1 << LayerMask.NameToLayer("Marker"));
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
    public UndergroundViewSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetGeometryData> __Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewNetStatusData> __Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewCoverageData> __Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetGeometryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewNetStatusData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewNetStatusData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewCoverageData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewCoverageData>(true);
      }
    }
  }
}
