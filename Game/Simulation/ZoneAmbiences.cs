// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ZoneAmbiences
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct ZoneAmbiences : IStrideSerializable, ISerializable
  {
    public float m_ResidentialLow;
    public float m_CommercialLow;
    public float m_Industrial;
    public float m_Agriculture;
    public float m_Forestry;
    public float m_Oil;
    public float m_Ore;
    public float m_OfficeLow;
    public float m_OfficeHigh;
    public float m_ResidentialMedium;
    public float m_ResidentialHigh;
    public float m_ResidentialMixed;
    public float m_CommercialHigh;
    public float m_ResidentialLowRent;
    public float m_Forest;
    public float m_WaterfrontLow;

    public float GetAmbience(GroupAmbienceType type)
    {
      switch (type)
      {
        case GroupAmbienceType.ResidentialLow:
          return this.m_ResidentialLow;
        case GroupAmbienceType.CommercialLow:
          return this.m_CommercialLow;
        case GroupAmbienceType.Industrial:
          return this.m_Industrial;
        case GroupAmbienceType.Agriculture:
          return this.m_Agriculture;
        case GroupAmbienceType.Forestry:
          return this.m_Forestry;
        case GroupAmbienceType.Oil:
          return this.m_Oil;
        case GroupAmbienceType.Ore:
          return this.m_Ore;
        case GroupAmbienceType.OfficeLow:
          return this.m_OfficeLow;
        case GroupAmbienceType.OfficeHigh:
          return this.m_OfficeHigh;
        case GroupAmbienceType.ResidentialMedium:
          return this.m_ResidentialMedium;
        case GroupAmbienceType.ResidentialHigh:
          return this.m_ResidentialHigh;
        case GroupAmbienceType.ResidentialMixed:
          return this.m_ResidentialMixed;
        case GroupAmbienceType.CommercialHigh:
          return this.m_CommercialHigh;
        case GroupAmbienceType.ResidentialLowRent:
          return this.m_ResidentialLowRent;
        case GroupAmbienceType.Forest:
          return this.m_Forest;
        case GroupAmbienceType.WaterfrontLow:
          return this.m_WaterfrontLow;
        default:
          return 0.0f;
      }
    }

    public void AddAmbience(GroupAmbienceType type, float value)
    {
      switch (type)
      {
        case GroupAmbienceType.ResidentialLow:
          this.m_ResidentialLow += value;
          break;
        case GroupAmbienceType.CommercialLow:
          this.m_CommercialLow += value;
          break;
        case GroupAmbienceType.Industrial:
          this.m_Industrial += value;
          break;
        case GroupAmbienceType.Agriculture:
          this.m_Agriculture += value;
          break;
        case GroupAmbienceType.Forestry:
          this.m_Forestry += value;
          break;
        case GroupAmbienceType.Oil:
          this.m_Oil += value;
          break;
        case GroupAmbienceType.Ore:
          this.m_Ore += value;
          break;
        case GroupAmbienceType.OfficeLow:
          this.m_OfficeLow += value;
          break;
        case GroupAmbienceType.OfficeHigh:
          this.m_OfficeHigh += value;
          break;
        case GroupAmbienceType.ResidentialMedium:
          this.m_ResidentialMedium += value;
          break;
        case GroupAmbienceType.ResidentialHigh:
          this.m_ResidentialHigh += value;
          break;
        case GroupAmbienceType.ResidentialMixed:
          this.m_ResidentialMixed += value;
          break;
        case GroupAmbienceType.CommercialHigh:
          this.m_CommercialHigh += value;
          break;
        case GroupAmbienceType.ResidentialLowRent:
          this.m_ResidentialLowRent += value;
          break;
        case GroupAmbienceType.Forest:
          this.m_Forest += value;
          break;
        case GroupAmbienceType.WaterfrontLow:
          this.m_WaterfrontLow += value;
          break;
      }
    }

    public static ZoneAmbiences operator +(ZoneAmbiences a, ZoneAmbiences b)
    {
      return new ZoneAmbiences()
      {
        m_ResidentialLow = a.m_ResidentialLow + b.m_ResidentialLow,
        m_CommercialLow = a.m_CommercialLow + b.m_CommercialLow,
        m_Industrial = a.m_Industrial + b.m_Industrial,
        m_Agriculture = a.m_Agriculture + b.m_Agriculture,
        m_Forestry = a.m_Forestry + b.m_Forestry,
        m_Oil = a.m_Oil + b.m_Oil,
        m_Ore = a.m_Ore + b.m_Ore,
        m_OfficeLow = a.m_OfficeLow + b.m_OfficeLow,
        m_OfficeHigh = a.m_OfficeHigh + b.m_OfficeHigh,
        m_ResidentialMedium = a.m_ResidentialMedium + b.m_ResidentialMedium,
        m_ResidentialHigh = a.m_ResidentialHigh + b.m_ResidentialHigh,
        m_ResidentialMixed = a.m_ResidentialMixed + b.m_ResidentialMixed,
        m_CommercialHigh = a.m_CommercialHigh + b.m_CommercialHigh,
        m_ResidentialLowRent = a.m_ResidentialLowRent + b.m_ResidentialLowRent,
        m_Forest = a.m_Forest + b.m_Forest,
        m_WaterfrontLow = a.m_WaterfrontLow + b.m_WaterfrontLow
      };
    }

    public static ZoneAmbiences operator /(ZoneAmbiences a, float b)
    {
      return new ZoneAmbiences()
      {
        m_ResidentialLow = a.m_ResidentialLow / b,
        m_CommercialLow = a.m_CommercialLow / b,
        m_Industrial = a.m_Industrial / b,
        m_Agriculture = a.m_Agriculture / b,
        m_Forestry = a.m_Forestry / b,
        m_Oil = a.m_Oil / b,
        m_Ore = a.m_Ore / b,
        m_OfficeLow = a.m_OfficeLow / b,
        m_OfficeHigh = a.m_OfficeHigh / b,
        m_ResidentialMedium = a.m_ResidentialMedium / b,
        m_ResidentialHigh = a.m_ResidentialHigh / b,
        m_ResidentialMixed = a.m_ResidentialMixed / b,
        m_CommercialHigh = a.m_CommercialHigh / b,
        m_ResidentialLowRent = a.m_ResidentialLowRent / b,
        m_Forest = a.m_Forest / b,
        m_WaterfrontLow = a.m_WaterfrontLow / b
      };
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_ResidentialLow);
      writer.Write(this.m_CommercialHigh);
      writer.Write(this.m_Industrial);
      writer.Write(this.m_Agriculture);
      writer.Write(this.m_Forestry);
      writer.Write(this.m_Oil);
      writer.Write(this.m_Ore);
      writer.Write(this.m_OfficeLow);
      writer.Write(this.m_OfficeHigh);
      writer.Write(this.m_ResidentialMedium);
      writer.Write(this.m_ResidentialHigh);
      writer.Write(this.m_ResidentialMixed);
      writer.Write(this.m_CommercialHigh);
      writer.Write(this.m_ResidentialLowRent);
      writer.Write(this.m_Forest);
      writer.Write(this.m_WaterfrontLow);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_ResidentialLow);
      reader.Read(out this.m_CommercialHigh);
      reader.Read(out this.m_Industrial);
      reader.Read(out this.m_Agriculture);
      reader.Read(out this.m_Forestry);
      reader.Read(out this.m_Oil);
      reader.Read(out this.m_Ore);
      if (!(reader.context.version > Version.zoneAmbience))
        return;
      reader.Read(out this.m_OfficeLow);
      reader.Read(out this.m_OfficeHigh);
      reader.Read(out this.m_ResidentialMedium);
      reader.Read(out this.m_ResidentialHigh);
      reader.Read(out this.m_ResidentialMixed);
      reader.Read(out this.m_CommercialHigh);
      reader.Read(out this.m_ResidentialLowRent);
      Context context;
      if (reader.context.version > Version.forestAmbience)
      {
        reader.Read(out this.m_Forest);
        context = reader.context;
        if (context.version > Version.waterfrontAmbience)
          reader.Read(out this.m_WaterfrontLow);
      }
      context = reader.context;
      if (!(context.version < Version.forestAmbientFix))
        return;
      this.m_Forest *= 1f / 16f;
    }

    public int GetStride(Context context) => 60;
  }
}
