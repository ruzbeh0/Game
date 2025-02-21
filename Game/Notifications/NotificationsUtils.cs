// Decompiled with JetBrains decompiler
// Type: Game.Notifications.NotificationsUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Notifications
{
  public static class NotificationsUtils
  {
    public const float ICON_VISIBLE_THROUGH_DISTANCE = 100f;

    public static IconLayerMask GetIconLayerMask(IconClusterLayer layer)
    {
      return (IconLayerMask) (1 << (int) (layer & (IconClusterLayer) 31));
    }
  }
}
