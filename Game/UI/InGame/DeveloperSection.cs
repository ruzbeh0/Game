// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DeveloperSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System.Collections.Generic;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class DeveloperSection : InfoSectionBase, ISubsectionProvider, ISectionSource, IJsonWritable
  {
    protected override string group => nameof (DeveloperSection);

    public List<ISubsectionSource> subsections { get; private set; }

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForOutsideConnections => true;

    protected override bool displayForUnderConstruction => true;

    protected override bool displayForUpgrades => true;

    public void AddSubsection(ISubsectionSource subsection) => this.subsections.Add(subsection);

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.subsections = new List<ISubsectionSource>();
    }

    protected override void Reset()
    {
    }

    private bool Visible()
    {
      bool flag = false;
      for (int index = 0; index < this.subsections.Count; ++index)
      {
        if (this.subsections[index].DisplayFor(this.selectedEntity, this.selectedPrefab))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      for (int index = 0; index < this.subsections.Count; ++index)
      {
        if (this.subsections[index].DisplayFor(this.selectedEntity, this.selectedPrefab))
          this.subsections[index].OnRequestUpdate(this.selectedEntity, this.selectedPrefab);
      }
    }

    private int GetSubsectionCount()
    {
      int subsectionCount = 0;
      for (int index = 0; index < this.subsections.Count; ++index)
      {
        if (this.subsections[index].DisplayFor(this.selectedEntity, this.selectedPrefab))
          ++subsectionCount;
      }
      return subsectionCount;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("subsections");
      writer.ArrayBegin(this.GetSubsectionCount());
      for (int index = 0; index < this.subsections.Count; ++index)
      {
        if (this.subsections[index].DisplayFor(this.selectedEntity, this.selectedPrefab))
          writer.Write<ISubsectionSource>(this.subsections[index]);
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public DeveloperSection()
    {
    }
  }
}
