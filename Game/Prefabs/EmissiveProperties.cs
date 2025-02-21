// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.EmissiveProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {typeof (RenderPrefab)})]
  public class EmissiveProperties : ComponentBase
  {
    public const float kIntensityMultiplier = 100f;
    public List<EmissiveProperties.SingleLightMapping> m_SingleLights;
    [FormerlySerializedAs("m_LayersMapping")]
    public List<EmissiveProperties.MultiLightMapping> m_MultiLights;
    public List<EmissiveProperties.AnimationProperties> m_AnimationCurves;
    public List<EmissiveProperties.SignalGroupAnimation> m_SignalGroupAnimations;

    public bool hasSingleLights => this.m_SingleLights != null && this.m_SingleLights.Count > 0;

    public bool hasMultiLights => this.m_MultiLights != null && this.m_MultiLights.Count > 0;

    public bool hasAnyLights => this.hasSingleLights || this.hasMultiLights;

    public int lightsCount
    {
      get
      {
        int lightsCount = 0;
        if (this.hasSingleLights)
          lightsCount += this.m_SingleLights.Count;
        if (this.hasMultiLights)
          lightsCount += this.m_MultiLights.Count;
        return lightsCount;
      }
    }

    public int GetSingleLightOffset(int materialId)
    {
      int num = 1;
      if (this.hasMultiLights)
        num += this.m_MultiLights.Count;
      if (this.hasSingleLights)
      {
        for (int index = 0; index < this.m_SingleLights.Count; ++index)
        {
          if (this.m_SingleLights[index].materialId == materialId)
            return num + index;
        }
      }
      return 0;
    }

    public bool IsSingleLightMaterialId(int materialId)
    {
      if (this.hasSingleLights)
      {
        foreach (EmissiveProperties.SingleLightMapping singleLight in this.m_SingleLights)
        {
          if (singleLight.materialId == materialId)
            return true;
        }
      }
      return false;
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<ProceduralLight>());
      if ((this.m_AnimationCurves == null || this.m_AnimationCurves.Count == 0) && (this.m_SignalGroupAnimations == null || this.m_SignalGroupAnimations.Count == 0))
        return;
      components.Add(ComponentType.ReadWrite<LightAnimation>());
    }

    public enum Purpose
    {
      None,
      DaytimeRunningLight,
      Headlight_HighBeam,
      Headlight_LowBeam,
      TurnSignalLeft,
      TurnSignalRight,
      RearLight,
      BrakeLight,
      ReverseLight,
      Clearance,
      DaytimeRunningLightLeft,
      DaytimeRunningLightRight,
      SignalGroup1,
      SignalGroup2,
      SignalGroup3,
      SignalGroup4,
      SignalGroup5,
      SignalGroup6,
      SignalGroup7,
      SignalGroup8,
      SignalGroup9,
      SignalGroup10,
      SignalGroup11,
      Interior1,
      DaytimeRunningLightAlt,
      TrafficLight_Red,
      TrafficLight_Yellow,
      TrafficLight_Green,
      PedestrianLight_Stop,
      PedestrianLight_Walk,
      RailCrossing_Stop,
      Dashboard,
      Clearance2,
      NeonSign,
      DecorativeLight,
      Emergency1,
      Emergency2,
      Emergency3,
      Emergency4,
      Emergency5,
      Emergency6,
      MarkerLights,
      CollectionLights,
      RearAlarmLights,
      FrontAlarmLightsLeft,
      FrontAlarmLightsRight,
      TaxiSign,
      Warning1,
      Warning2,
      WorkLights,
      Emergency7,
      Emergency8,
      Emergency9,
      Emergency10,
      BrakeAndTurnSignalLeft,
      BrakeAndTurnSignalRight,
      TaxiLights,
      LandingLights,
      WingInspectionLights,
      LogoLights,
      PositionLightLeft,
      PositionLightRight,
      PositionLights,
      AntiCollisionLightsRed,
      AntiCollisionLightsWhite,
      SearchLightsFront,
      SearchLights360,
      NumberLight,
      Interior2,
      BoardingLightLeft,
      BoardingLightRight,
      EffectSource,
    }

    [Serializable]
    public class MultiLightMapping : EmissiveProperties.LightProperties
    {
      public int layerId = -1;
    }

    [Serializable]
    public class SingleLightMapping : EmissiveProperties.LightProperties
    {
      public int materialId;
    }

    [Serializable]
    public class LightProperties
    {
      public EmissiveProperties.Purpose purpose;
      public Color color = Color.white;
      public Color colorOff = Color.black;
      public float intensity;
      public float luminance;
      public float responseTime;
      public int animationIndex = -1;
    }

    [Serializable]
    public class AnimationProperties
    {
      public float m_Duration;
      public AnimationCurve m_Curve;
    }

    [Serializable]
    public class SignalGroupAnimation
    {
      public float m_Duration;
      public SignalGroupMask[] m_SignalGroupMasks;
    }
  }
}
