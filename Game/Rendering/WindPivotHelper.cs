// Decompiled with JetBrains decompiler
// Type: Game.Rendering.WindPivotHelper
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class WindPivotHelper : MonoBehaviour
  {
    public WindPivotHelper.PivotBakeMode m_BakedMode;
    public bool m_ShowBasePivot = true;
    public bool m_ShowLevel0Pivot = true;
    public bool m_ShowLevel0Guide = true;
    public bool m_ShowLevel1Pivot = true;
    public bool m_ShowLevel1Guide = true;
    public List<Vector3> m_PivotsP0 = new List<Vector3>();
    public List<Vector3> m_PivotsN0 = new List<Vector3>();
    public List<float> m_PivotsH0 = new List<float>();
    public List<Vector3> m_PivotsR1 = new List<Vector3>();
    public List<Vector3> m_PivotsP1 = new List<Vector3>();
    public List<Vector3> m_PivotsN1 = new List<Vector3>();
    public List<float> m_PivotsH1 = new List<float>();

    public void Clear()
    {
      this.m_PivotsP0.Clear();
      this.m_PivotsN0.Clear();
      this.m_PivotsH0.Clear();
      this.m_PivotsR1.Clear();
      this.m_PivotsP1.Clear();
      this.m_PivotsN1.Clear();
      this.m_PivotsH1.Clear();
    }

    private void OnDrawGizmosSelected()
    {
      Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
      float num = Mathf.Max(this.transform.lossyScale.x, Mathf.Max(this.transform.lossyScale.y, this.transform.lossyScale.z));
      if (this.m_ShowLevel1Pivot || this.m_ShowLevel1Guide)
      {
        Color color = new Color(0.0f, 1f, 1f, 0.1f);
        for (int index = 0; index < this.m_PivotsP1.Count; ++index)
        {
          Vector3 to = localToWorldMatrix.MultiplyPoint(this.m_PivotsR1[index]);
          Vector3 vector3_1 = localToWorldMatrix.MultiplyPoint(this.m_PivotsP1[index]);
          Vector3 vector3_2 = localToWorldMatrix.MultiplyVector(this.m_PivotsN1[index] * this.m_PivotsH1[index]);
          if (this.m_ShowLevel1Guide)
          {
            Gizmos.color = color;
            Gizmos.DrawLine(vector3_1, to);
          }
          if (this.m_ShowLevel1Pivot)
          {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(vector3_1, vector3_1 + vector3_2);
            Gizmos.DrawSphere(vector3_1, 0.01f * num);
          }
        }
      }
      if (this.m_ShowLevel0Pivot || this.m_ShowLevel0Guide)
      {
        Color color = new Color(1f, 1f, 0.0f, 0.1f);
        for (int index = 0; index < this.m_PivotsP0.Count; ++index)
        {
          Vector3 vector3_3 = localToWorldMatrix.MultiplyPoint(this.m_PivotsP0[index]);
          Vector3 vector3_4 = localToWorldMatrix.MultiplyVector(this.m_PivotsN0[index] * this.m_PivotsH0[index]);
          if (this.m_ShowLevel0Guide)
          {
            Gizmos.color = color;
            Gizmos.DrawLine(vector3_3, this.transform.position);
          }
          if (this.m_ShowLevel0Pivot)
          {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(vector3_3, vector3_3 + vector3_4);
            Gizmos.DrawSphere(vector3_3, 0.01f * num);
          }
        }
      }
      if (!this.m_ShowBasePivot)
        return;
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(this.transform.position, 0.1f * num);
    }

    public enum PivotBakeMode
    {
      SingleDecompose,
      HierarchyDecompose,
    }
  }
}
