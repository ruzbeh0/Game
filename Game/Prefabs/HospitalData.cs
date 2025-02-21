// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.HospitalData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct HospitalData : 
    IComponentData,
    IQueryTypeParameter,
    ICombineData<HospitalData>,
    ISerializable
  {
    public int m_AmbulanceCapacity;
    public int m_MedicalHelicopterCapacity;
    public int m_PatientCapacity;
    public int m_TreatmentBonus;
    public int2 m_HealthRange;
    public bool m_TreatDiseases;
    public bool m_TreatInjuries;

    public void Combine(HospitalData otherData)
    {
      this.m_AmbulanceCapacity += otherData.m_AmbulanceCapacity;
      this.m_MedicalHelicopterCapacity += otherData.m_MedicalHelicopterCapacity;
      this.m_PatientCapacity += otherData.m_PatientCapacity;
      this.m_TreatmentBonus += otherData.m_TreatmentBonus;
      this.m_HealthRange.x = math.min(this.m_HealthRange.x, otherData.m_HealthRange.x);
      this.m_HealthRange.y = math.max(this.m_HealthRange.y, otherData.m_HealthRange.y);
      this.m_TreatDiseases |= otherData.m_TreatDiseases;
      this.m_TreatInjuries |= otherData.m_TreatInjuries;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_AmbulanceCapacity);
      writer.Write(this.m_MedicalHelicopterCapacity);
      writer.Write(this.m_PatientCapacity);
      writer.Write(this.m_TreatmentBonus);
      writer.Write(this.m_HealthRange);
      writer.Write(this.m_TreatDiseases);
      writer.Write(this.m_TreatInjuries);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_AmbulanceCapacity);
      reader.Read(out this.m_MedicalHelicopterCapacity);
      reader.Read(out this.m_PatientCapacity);
      reader.Read(out this.m_TreatmentBonus);
      reader.Read(out this.m_HealthRange);
      reader.Read(out this.m_TreatDiseases);
      reader.Read(out this.m_TreatInjuries);
    }
  }
}
