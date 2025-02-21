// Decompiled with JetBrains decompiler
// Type: Game.Rendering.WindTextureSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class WindTextureSystem : GameSystemBase
  {
    private WindSystem m_WindSystem;
    private Texture2D m_WindTexture;
    private JobHandle m_UpdateHandle;
    private bool m_RequireUpdate;
    private bool m_RequireApply;

    public Texture2D WindTexture => this.m_WindTexture;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      Texture2D texture2D = new Texture2D(WindSystem.kTextureSize, WindSystem.kTextureSize, TextureFormat.RGFloat, false, true);
      texture2D.name = "WindTexture";
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      this.m_WindTexture = texture2D;
    }

    public void RequireUpdate() => this.m_RequireUpdate = true;

    public void CompleteUpdate()
    {
      if (!this.m_RequireApply)
        return;
      this.m_RequireApply = false;
      this.m_UpdateHandle.Complete();
      this.m_WindTexture.Apply();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      if (!this.m_RequireUpdate)
        return;
      this.m_RequireUpdate = false;
      this.m_RequireApply = true;
      JobHandle dependencies;
      this.m_UpdateHandle = new WindTextureSystem.WindTextureJob()
      {
        m_WindMap = this.m_WindSystem.GetMap(true, out dependencies),
        m_WindTexture = this.m_WindTexture.GetRawTextureData<float2>()
      }.Schedule<WindTextureSystem.WindTextureJob>(WindSystem.kTextureSize * WindSystem.kTextureSize, dependencies);
      this.m_WindSystem.AddReader(this.m_UpdateHandle);
    }

    [UnityEngine.Scripting.Preserve]
    public WindTextureSystem()
    {
    }

    [BurstCompile]
    private struct WindTextureJob : IJobFor
    {
      [ReadOnly]
      public NativeArray<Wind> m_WindMap;
      public NativeArray<float2> m_WindTexture;

      public void Execute(int index) => this.m_WindTexture[index] = this.m_WindMap[index].m_Wind;
    }
  }
}
