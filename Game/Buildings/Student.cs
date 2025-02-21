// Decompiled with JetBrains decompiler
// Type: Game.Buildings.Student
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  public struct Student : IBufferElementData, IEmptySerializable, IEquatable<Student>
  {
    public Entity m_Student;

    public Student(Entity student) => this.m_Student = student;

    public bool Equals(Student other) => this.m_Student.Equals(other.m_Student);

    public static implicit operator Entity(Student student) => student.m_Student;
  }
}
