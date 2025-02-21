// Decompiled with JetBrains decompiler
// Type: Game.Companies.Workplaces
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Serialization.Entities;
using System;
using Unity.Collections;

#nullable disable
namespace Game.Companies
{
  public struct Workplaces : IAccumulable<Workplaces>, ISerializable
  {
    public int m_Uneducated;
    public int m_PoorlyEducated;
    public int m_Educated;
    public int m_WellEducated;
    public int m_HighlyEducated;

    public int TotalCount
    {
      get
      {
        return this.m_Uneducated + this.m_PoorlyEducated + this.m_Educated + this.m_WellEducated + this.m_HighlyEducated;
      }
    }

    public int SimpleWorkplacesCount => this.m_Uneducated + this.m_PoorlyEducated;

    public int ComplexWorkplacesCount
    {
      get => this.m_Educated + this.m_WellEducated + this.m_HighlyEducated;
    }

    public int this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.m_Uneducated;
          case 1:
            return this.m_PoorlyEducated;
          case 2:
            return this.m_Educated;
          case 3:
            return this.m_WellEducated;
          case 4:
            return this.m_HighlyEducated;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.m_Uneducated = value;
            break;
          case 1:
            this.m_PoorlyEducated = value;
            break;
          case 2:
            this.m_Educated = value;
            break;
          case 3:
            this.m_WellEducated = value;
            break;
          case 4:
            this.m_HighlyEducated = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public void ToArray(NativeArray<int> array)
    {
      array[0] = this.m_Uneducated;
      array[1] = this.m_PoorlyEducated;
      array[2] = this.m_Educated;
      array[3] = this.m_WellEducated;
      array[4] = this.m_HighlyEducated;
    }

    public void Accumulate(Workplaces other)
    {
      this.m_Uneducated += other.m_Uneducated;
      this.m_PoorlyEducated += other.m_PoorlyEducated;
      this.m_Educated += other.m_Educated;
      this.m_WellEducated += other.m_WellEducated;
      this.m_HighlyEducated += other.m_HighlyEducated;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Uneducated);
      writer.Write(this.m_PoorlyEducated);
      writer.Write(this.m_Educated);
      writer.Write(this.m_WellEducated);
      writer.Write(this.m_HighlyEducated);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Uneducated);
      reader.Read(out this.m_PoorlyEducated);
      reader.Read(out this.m_Educated);
      reader.Read(out this.m_WellEducated);
      reader.Read(out this.m_HighlyEducated);
    }
  }
}
