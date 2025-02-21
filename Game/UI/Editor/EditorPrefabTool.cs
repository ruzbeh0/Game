// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPrefabTool
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorPrefabTool : EditorTool
  {
    [CanBeNull]
    private PrefabBase m_LastSelectedPrefab;

    public EditorPrefabTool(World world)
      : base(world)
    {
      this.id = "PrefabTool";
      this.icon = "Media/Editor/Object.svg";
      this.panel = (IEditorPanel) world.GetOrCreateSystemManaged<PrefabToolPanelSystem>();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.ActivatePrefabTool(this.m_LastSelectedPrefab);
    }

    protected override void OnDisable()
    {
      this.m_LastSelectedPrefab = this.m_ToolSystem.activePrefab;
      base.OnDisable();
    }
  }
}
