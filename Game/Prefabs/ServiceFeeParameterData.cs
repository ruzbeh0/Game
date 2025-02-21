// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ServiceFeeParameterData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.City;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct ServiceFeeParameterData : IComponentData, IQueryTypeParameter
  {
    public FeeParameters m_ElectricityFee;
    public AnimationCurve1 m_ElectricityFeeConsumptionMultiplier;
    public FeeParameters m_HealthcareFee;
    public FeeParameters m_BasicEducationFee;
    public FeeParameters m_SecondaryEducationFee;
    public FeeParameters m_HigherEducationFee;
    public FeeParameters m_GarbageFee;
    public int4 m_GarbageFeeRCIO;
    public FeeParameters m_WaterFee;
    public AnimationCurve1 m_WaterFeeConsumptionMultiplier;
    public FeeParameters m_FireResponseFee;
    public FeeParameters m_PoliceFee;

    public FeeParameters GetFeeParameters(PlayerResource resource)
    {
      switch (resource)
      {
        case PlayerResource.Electricity:
          return this.m_ElectricityFee;
        case PlayerResource.Healthcare:
          return this.m_HealthcareFee;
        case PlayerResource.BasicEducation:
          return this.m_BasicEducationFee;
        case PlayerResource.SecondaryEducation:
          return this.m_SecondaryEducationFee;
        case PlayerResource.HigherEducation:
          return this.m_HigherEducationFee;
        case PlayerResource.Garbage:
          return this.m_GarbageFee;
        case PlayerResource.Water:
          return this.m_WaterFee;
        case PlayerResource.FireResponse:
          return this.m_FireResponseFee;
        case PlayerResource.Police:
          return this.m_PoliceFee;
        default:
          return new FeeParameters();
      }
    }

    public IEnumerable<ServiceFee> GetDefaultFees()
    {
      yield return this.GetDefaultServiceFee(PlayerResource.Healthcare);
      yield return this.GetDefaultServiceFee(PlayerResource.Electricity);
      yield return this.GetDefaultServiceFee(PlayerResource.BasicEducation);
      yield return this.GetDefaultServiceFee(PlayerResource.HigherEducation);
      yield return this.GetDefaultServiceFee(PlayerResource.SecondaryEducation);
      yield return this.GetDefaultServiceFee(PlayerResource.Garbage);
      yield return this.GetDefaultServiceFee(PlayerResource.Water);
      yield return this.GetDefaultServiceFee(PlayerResource.FireResponse);
      yield return this.GetDefaultServiceFee(PlayerResource.Police);
    }

    private ServiceFee GetDefaultServiceFee(PlayerResource resource)
    {
      return new ServiceFee()
      {
        m_Resource = resource,
        m_Fee = this.GetFeeParameters(resource).m_Default
      };
    }
  }
}
