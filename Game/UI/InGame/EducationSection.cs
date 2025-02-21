// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EducationSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class EducationSection : InfoSectionBase
  {
    protected override string group => nameof (EducationSection);

    private int studentCount { get; set; }

    private int studentCapacity { get; set; }

    private float graduationTime { get; set; }

    private float failProbability { get; set; }

    protected override void Reset()
    {
      this.studentCount = 0;
      this.studentCapacity = 0;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.School>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      SchoolData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<SchoolData>(this.selectedEntity, this.selectedPrefab, out data))
        this.studentCapacity = data.m_StudentCapacity;
      DynamicBuffer<Student> buffer;
      if (this.EntityManager.TryGetBuffer<Student>(this.selectedEntity, true, out buffer))
        this.studentCount = buffer.Length;
      Game.Buildings.School component;
      if (!this.EntityManager.TryGetComponent<Game.Buildings.School>(this.selectedEntity, out component))
        return;
      this.graduationTime = (double) component.m_AverageGraduationTime > 0.0 ? component.m_AverageGraduationTime : 0.5f;
      this.failProbability = component.m_AverageFailProbability;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("studentCount");
      writer.Write(this.studentCount);
      writer.PropertyName("studentCapacity");
      writer.Write(this.studentCapacity);
      writer.PropertyName("graduationTime");
      writer.Write(this.graduationTime);
      writer.PropertyName("failProbability");
      writer.Write(this.failProbability);
    }

    [Preserve]
    public EducationSection()
    {
    }
  }
}
