// Decompiled with JetBrains decompiler
// Type: Game.Rendering.WaterRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [FormerlySerializedAs("Colossal.Terrain.WaterRenderSystem, Game")]
  [CompilerGenerated]
  public class WaterRenderSystem : GameSystemBase
  {
    private TerrainRenderSystem m_TerrainRenderSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private RenderingSystem m_RenderingSystem;

    public Texture overrideOverlaymap { get; set; }

    public Texture overlayExtramap { get; set; }

    public float4 overlayPollutionMask { get; set; }

    public float4 overlayArrowMask { get; set; }

    public Texture waterTexture => (Texture) this.m_WaterSystem.WaterRenderTexture;

    public Texture flowTexture => this.m_WaterSystem.FlowTextureUpdated;

    public bool IsAsync { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
    }

    [Preserve]
    protected override void OnDestroy() => base.OnDestroy();

    [Preserve]
    protected override void OnUpdate()
    {
      int baseLOD;
      float4x4 areas;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.GetCascadeInfo(out int _, out baseLOD, out areas, out float4 _, out float4 _);
      foreach (WaterSurface instance in WaterSurface.instances)
      {
        // ISSUE: reference to a compiler-generated field
        float num = this.m_RenderingSystem.frameDelta / math.max(1E-06f, this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime * 60f);
        instance.timeMultiplier = num;
        instance.CascadeArea = (Matrix4x4) areas;
        instance.WaterSimArea = baseLOD != 0 ? new Vector4(areas.c0.y, areas.c1.y, areas.c2.y - areas.c0.y, areas.c3.y - areas.c1.y) : new Vector4(areas.c0.x, areas.c1.x, areas.c2.x - areas.c0.x, areas.c3.x - areas.c1.x);
        // ISSUE: reference to a compiler-generated field
        instance.TerrainScaleOffset = (Vector2) this.m_TerrainSystem.heightScaleOffset;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        instance.TerrainCascadeTexture = this.m_TerrainSystem.GetCascadeTexture();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        instance.WaterSimulationTexture = !this.m_WaterSystem.Loaded ? (Texture) Texture2D.blackTexture : (Texture) this.m_WaterSystem.WaterTexture;
        if ((bool) (Object) instance.customMaterial)
        {
          instance.customMaterial.SetVector(TerrainRenderSystem.ShaderID._OverlayArrowMask, (Vector4) this.overlayArrowMask);
          instance.customMaterial.SetVector(TerrainRenderSystem.ShaderID._OverlayPollutionMask, (Vector4) this.overlayPollutionMask);
          if ((Object) this.overlayExtramap != (Object) null)
          {
            if ((Object) this.overrideOverlaymap == (Object) null)
              this.overrideOverlaymap = (Texture) Texture2D.whiteTexture;
            instance.customMaterial.EnableKeyword("OVERRIDE_OVERLAY_EXTRA");
          }
          else
            instance.customMaterial.DisableKeyword("OVERRIDE_OVERLAY_EXTRA");
        }
      }
      // ISSUE: reference to a compiler-generated field
      int num1 = this.m_WaterSystem.Loaded ? 1 : 0;
    }

    [Preserve]
    public WaterRenderSystem()
    {
    }
  }
}
