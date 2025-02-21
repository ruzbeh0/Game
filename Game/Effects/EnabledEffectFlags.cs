// Decompiled with JetBrains decompiler
// Type: Game.Effects.EnabledEffectFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Effects
{
  [Flags]
  public enum EnabledEffectFlags : uint
  {
    IsEnabled = 1,
    EnabledUpdated = 2,
    Deleted = 4,
    IsLight = 8,
    IsVFX = 16, // 0x00000010
    IsAudio = 32, // 0x00000020
    AudioDisabled = 64, // 0x00000040
    EditorContainer = 128, // 0x00000080
    RandomTransform = 256, // 0x00000100
    TempOwner = 512, // 0x00000200
    DynamicTransform = 1024, // 0x00000400
    RandomColor = 2048, // 0x00000800
    OwnerUpdated = 4096, // 0x00001000
    OwnerCollapsed = 8192, // 0x00002000
    WrongPrefab = 16384, // 0x00004000
  }
}
