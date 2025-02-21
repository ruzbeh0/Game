// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorPhotoTool
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorPhotoTool : EditorTool
  {
    public EditorPhotoTool(World world)
      : base(world)
    {
      this.id = "PhotoTool";
      this.icon = "Media/Editor/CinematicCameraOff.svg";
      this.panel = (IEditorPanel) world.GetOrCreateSystemManaged<EditorPhotoModePanel>();
    }
  }
}
