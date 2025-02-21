// Decompiled with JetBrains decompiler
// Type: VTTestGameManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Logging;
using Game.Rendering;
using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public class VTTestGameManager : MonoBehaviour
{
  [Header("VT Settings")]
  public bool m_OverrideVTSettings;
  public int m_VTMipBias;
  public UnityEngine.Rendering.VirtualTexturing.FilterMode m_VTFilterMode = UnityEngine.Rendering.VirtualTexturing.FilterMode.Trilinear;
  private uint m_FrameIndex;
  private float m_FrameTime;
  private WindControl m_WindControl;
  [Header("Camera")]
  public Camera movingCamera;
  public float cameraSpeed = 0.1f;
  public Transform cameraStart;
  public Transform cameraEnd;
  private float cameraPosition;
  private bool movingBackward;
  private World m_World;
  private TextureStreamingSystem m_TextureStreamingSystem;
  private GizmosSystem m_GizmosSystem;

  private void Awake()
  {
    this.m_WindControl = WindControl.instance;
    LogManager.SetDefaultEffectiveness(Level.Info);
    this.m_World = new World("Game");
    World.DefaultGameObjectInjectionWorld = this.m_World;
    this.m_GizmosSystem = this.m_World.GetOrCreateSystemManaged<GizmosSystem>();
    this.m_TextureStreamingSystem = this.m_World.GetOrCreateSystemManaged<TextureStreamingSystem>();
    if (this.m_OverrideVTSettings)
      this.m_TextureStreamingSystem.Initialize(this.m_VTMipBias, this.m_VTFilterMode);
    else
      this.m_TextureStreamingSystem.Initialize();
    this.cameraPosition = 0.0f;
  }

  private void OnDestroy()
  {
    Colossal.Gizmos.ReleaseResources();
    this.m_World.Dispose();
    this.m_WindControl.Dispose();
  }

  private void Update()
  {
    this.m_FrameIndex += 5U;
    this.m_FrameTime = Time.deltaTime;
    Shader.SetGlobalVector("colossal_SimulationTime", (Vector4) (((float2) (this.m_FrameIndex % new uint2(60U, 3600U)) + new float2(this.m_FrameTime)).xyxy * new float4(0.0166666675f, 0.000277777785f, (float) Math.PI / 30f, 0.00174532935f)));
    Shader.SetGlobalFloat("colossal_SimulationTime2", (float) (this.m_FrameIndex % 216000U) + this.m_FrameTime);
    this.m_TextureStreamingSystem.Update();
    if (!((UnityEngine.Object) this.movingCamera != (UnityEngine.Object) null) || !((UnityEngine.Object) this.cameraEnd != (UnityEngine.Object) null) || !((UnityEngine.Object) this.cameraStart != (UnityEngine.Object) null))
      return;
    Vector3 vector3 = this.cameraEnd.position - this.cameraStart.position;
    float num = this.cameraSpeed * Time.deltaTime;
    if (this.movingBackward)
    {
      this.cameraPosition -= num;
      if ((double) this.cameraPosition < 0.0)
      {
        this.cameraPosition = 0.0f;
        this.movingBackward = false;
      }
    }
    else
    {
      this.cameraPosition += num;
      if ((double) this.cameraPosition > (double) vector3.magnitude)
      {
        this.cameraPosition = vector3.magnitude;
        this.movingBackward = true;
      }
    }
    this.movingCamera.transform.position = this.cameraStart.position + vector3.normalized * this.cameraPosition;
  }

  private void LateUpdate() => this.m_GizmosSystem.Update();
}
