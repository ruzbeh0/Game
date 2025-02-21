// Decompiled with JetBrains decompiler
// Type: Game.Rendering.VegetationRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Simulation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngine.VFX;

#nullable disable
namespace Game.Rendering
{
  public class VegetationRenderSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private TerrainMaterialSystem m_TerrainMaterialSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private static VisualEffectAsset s_FoliageVFXAsset;
    private VisualEffect m_FoliageVFX;

    [Preserve]
    protected override void OnCreate()
    {
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_TerrainMaterialSystem = this.World.GetOrCreateSystemManaged<TerrainMaterialSystem>();
      VegetationRenderSystem.s_FoliageVFXAsset = Resources.Load<VisualEffectAsset>("Vegetation/FoliageVFX");
      this.Enabled = false;
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      base.OnStopRunning();
      CoreUtils.Destroy((Object) this.m_FoliageVFX);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.m_CameraUpdateSystem.activeViewer == null)
        return;
      this.CreateDynamicVFXIfNeeded();
      this.UpdateEffect();
    }

    private void UpdateEffect()
    {
      // ISSUE: reference to a compiler-generated method
      Bounds terrainBounds = this.m_TerrainSystem.GetTerrainBounds();
      this.m_FoliageVFX.SetVector3("TerrainBounds_center", terrainBounds.center);
      this.m_FoliageVFX.SetVector3("TerrainBounds_size", terrainBounds.size);
      this.m_FoliageVFX.SetTexture("Terrain HeightMap", this.m_TerrainSystem.heightmap);
      this.m_FoliageVFX.SetTexture("Terrain SplatMap", this.m_TerrainMaterialSystem.splatmap);
      Vector4 globalVector1 = Shader.GetGlobalVector("colossal_TerrainScale");
      Vector4 globalVector2 = Shader.GetGlobalVector("colossal_TerrainOffset");
      this.m_FoliageVFX.SetVector4("Terrain Offset Scale", new Vector4(globalVector1.x, globalVector1.z, globalVector2.x, globalVector2.z));
      this.m_FoliageVFX.SetVector3("CameraPosition", (Vector3) this.m_CameraUpdateSystem.position);
      this.m_FoliageVFX.SetVector3("CameraDirection", (Vector3) this.m_CameraUpdateSystem.direction);
    }

    private void CreateDynamicVFXIfNeeded()
    {
      if (!((Object) VegetationRenderSystem.s_FoliageVFXAsset != (Object) null) || !((Object) this.m_FoliageVFX == (Object) null))
        return;
      COSystemBase.baseLog.DebugFormat("Creating FoliageVFX");
      this.m_FoliageVFX = new GameObject("FoliageVFX").AddComponent<VisualEffect>();
      this.m_FoliageVFX.visualEffectAsset = VegetationRenderSystem.s_FoliageVFXAsset;
    }

    [Preserve]
    protected override void OnDestroy() => base.OnDestroy();

    [Preserve]
    public VegetationRenderSystem()
    {
    }
  }
}
