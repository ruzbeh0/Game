// Decompiled with JetBrains decompiler
// Type: Game.CinemachineDollyCartLookExtension
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Cinemachine;
using UnityEngine;

#nullable disable
namespace Game
{
  public class CinemachineDollyCartLookExtension : CinemachineExtension
  {
    public CinemachineDollyCartLookExtension.DollyLookAngleOverride[] m_Angles;

    public float GetMaxPos(bool looped)
    {
      int num = this.m_Angles.Length - 1;
      return num < 1 ? 0.0f : (looped ? (float) (num + 1) : (float) num);
    }

    public virtual float StandardizePos(float pos, bool looped)
    {
      float maxPos = this.GetMaxPos(looped);
      if (!looped || (double) maxPos <= 0.0)
        return Mathf.Clamp(pos, 0.0f, maxPos);
      pos %= maxPos;
      if ((double) pos < 0.0)
        pos += maxPos;
      return pos;
    }

    private float GetBoundingIndices(float pos, bool looped, out int indexA, out int indexB)
    {
      pos = this.StandardizePos(pos, looped);
      int length = this.m_Angles.Length;
      if (length < 2)
      {
        indexA = indexB = 0;
      }
      else
      {
        indexA = Mathf.FloorToInt(pos);
        if (indexA >= length)
        {
          pos -= this.GetMaxPos(looped);
          indexA = 0;
        }
        indexB = indexA + 1;
        if (indexB == length)
        {
          if (looped)
          {
            indexB = 0;
          }
          else
          {
            --indexB;
            --indexA;
          }
        }
      }
      return pos;
    }

    protected override void PostPipelineStageCallback(
      CinemachineVirtualCameraBase vcam,
      CinemachineCore.Stage stage,
      ref CameraState state,
      float deltaTime)
    {
      if (stage != CinemachineCore.Stage.Aim)
        return;
      CinemachineDollyCart component = this.GetComponent<CinemachineDollyCart>();
      if (this.m_Angles.Length == 0 || component.m_PositionUnits != CinemachinePathBase.PositionUnits.PathUnits)
        return;
      CinemachinePathBase path = component.m_Path;
      int indexA;
      int indexB;
      float boundingIndices = this.GetBoundingIndices(component.m_Position, path.Looped, out indexA, out indexB);
      Vector3 eulerAngles1 = (indexA != indexB ? Quaternion.Slerp(this.GetAngleOffset(path, indexA), this.GetAngleOffset(path, indexB), boundingIndices - (float) indexA) : this.GetAngleOffset(path, indexA)).eulerAngles with
      {
        z = 0.0f
      };
      Vector3 eulerAngles2 = component.m_Path.EvaluateOrientation(boundingIndices).eulerAngles with
      {
        z = 0.0f
      };
      state.RawOrientation = Quaternion.Euler(eulerAngles2 + eulerAngles1);
    }

    private Quaternion GetAngleOffset(CinemachinePathBase path, int t)
    {
      if (!this.m_Angles[t].m_OverrideLookAngle)
        return Quaternion.Euler(0.0f, 0.0f, 0.0f);
      Vector3 eulerAngles = path.EvaluateOrientation((float) t).eulerAngles with
      {
        z = 0.0f
      };
      return Quaternion.Euler(this.m_Angles[t].m_Angle - eulerAngles);
    }

    public struct DollyLookAngleOverride
    {
      public bool m_OverrideLookAngle;
      public Vector3 m_Angle;
    }
  }
}
