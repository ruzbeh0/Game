// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CompositionFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;

#nullable disable
namespace Game.Prefabs
{
  public struct CompositionFlags : ISerializable, IEquatable<CompositionFlags>
  {
    public CompositionFlags.General m_General;
    public CompositionFlags.Side m_Left;
    public CompositionFlags.Side m_Right;
    private const CompositionFlags.General NODE_MASK_GENERAL = CompositionFlags.General.Node | CompositionFlags.General.DeadEnd | CompositionFlags.General.Intersection | CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing | CompositionFlags.General.MedianBreak | CompositionFlags.General.TrafficLights | CompositionFlags.General.RemoveTrafficLights | CompositionFlags.General.AllWayStop;
    private const CompositionFlags.General OPTION_MASK_GENERAL = CompositionFlags.General.WideMedian | CompositionFlags.General.PrimaryMiddleBeautification | CompositionFlags.General.SecondaryMiddleBeautification;
    private const CompositionFlags.Side NODE_MASK_SIDE = CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition;
    private const CompositionFlags.Side OPTION_MASK_SIDE = CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk;

    public CompositionFlags(
      CompositionFlags.General general,
      CompositionFlags.Side left,
      CompositionFlags.Side right)
    {
      this.m_General = general;
      this.m_Left = left;
      this.m_Right = right;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((uint) this.m_General);
      writer.Write((uint) this.m_Left);
      writer.Write((uint) this.m_Right);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Game.Version.compositionFlagRefactoring)
      {
        uint num1;
        reader.Read(out num1);
        uint num2;
        reader.Read(out num2);
        uint num3;
        reader.Read(out num3);
        this.m_General = (CompositionFlags.General) num1;
        this.m_Left = (CompositionFlags.Side) num2;
        this.m_Right = (CompositionFlags.Side) num3;
      }
      else
      {
        uint num;
        reader.Read(out num);
        if (((int) num & 4096) != 0)
          this.m_Right |= CompositionFlags.Side.PrimaryTrack;
        if (((int) num & 8192) == 0)
          return;
        this.m_Left |= CompositionFlags.Side.PrimaryTrack;
      }
    }

    public static CompositionFlags operator |(CompositionFlags lhs, CompositionFlags rhs)
    {
      return new CompositionFlags(lhs.m_General | rhs.m_General, lhs.m_Left | rhs.m_Left, lhs.m_Right | rhs.m_Right);
    }

    public static CompositionFlags operator &(CompositionFlags lhs, CompositionFlags rhs)
    {
      return new CompositionFlags(lhs.m_General & rhs.m_General, lhs.m_Left & rhs.m_Left, lhs.m_Right & rhs.m_Right);
    }

    public static CompositionFlags operator ^(CompositionFlags lhs, CompositionFlags rhs)
    {
      return new CompositionFlags(lhs.m_General ^ rhs.m_General, lhs.m_Left ^ rhs.m_Left, lhs.m_Right ^ rhs.m_Right);
    }

    public static bool operator ==(CompositionFlags lhs, CompositionFlags rhs)
    {
      return lhs.m_General == rhs.m_General && lhs.m_Left == rhs.m_Left && lhs.m_Right == rhs.m_Right;
    }

    public static bool operator !=(CompositionFlags lhs, CompositionFlags rhs)
    {
      return lhs.m_General != rhs.m_General || lhs.m_Left != rhs.m_Left || lhs.m_Right != rhs.m_Right;
    }

    public static CompositionFlags operator ~(CompositionFlags rhs)
    {
      return new CompositionFlags(~rhs.m_General, ~rhs.m_Left, ~rhs.m_Right);
    }

    public bool Equals(CompositionFlags other) => this == other;

    public override bool Equals(object obj) => obj is CompositionFlags other && this.Equals(other);

    public override int GetHashCode()
    {
      uint num1 = (uint) this.m_General;
      int num2 = num1.GetHashCode() * 31;
      num1 = (uint) this.m_Left;
      int hashCode1 = num1.GetHashCode();
      int num3 = (num2 + hashCode1) * 31;
      num1 = (uint) this.m_Right;
      int hashCode2 = num1.GetHashCode();
      return num3 + hashCode2;
    }

    public static CompositionFlags nodeMask
    {
      get
      {
        return new CompositionFlags(CompositionFlags.General.Node | CompositionFlags.General.DeadEnd | CompositionFlags.General.Intersection | CompositionFlags.General.Roundabout | CompositionFlags.General.LevelCrossing | CompositionFlags.General.MedianBreak | CompositionFlags.General.TrafficLights | CompositionFlags.General.RemoveTrafficLights | CompositionFlags.General.AllWayStop, CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition, CompositionFlags.Side.LowTransition | CompositionFlags.Side.HighTransition);
      }
    }

    public static CompositionFlags optionMask
    {
      get
      {
        return new CompositionFlags(CompositionFlags.General.WideMedian | CompositionFlags.General.PrimaryMiddleBeautification | CompositionFlags.General.SecondaryMiddleBeautification, CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk, CompositionFlags.Side.PrimaryBeautification | CompositionFlags.Side.SecondaryBeautification | CompositionFlags.Side.WideSidewalk);
      }
    }

    [Flags]
    public enum General : uint
    {
      Node = 1,
      Edge = 2,
      Invert = 4,
      Flip = 8,
      DeadEnd = 16, // 0x00000010
      Intersection = 32, // 0x00000020
      Roundabout = 64, // 0x00000040
      LevelCrossing = 128, // 0x00000080
      Crosswalk = 256, // 0x00000100
      MedianBreak = 512, // 0x00000200
      TrafficLights = 1024, // 0x00000400
      Spillway = 2048, // 0x00000800
      Opening = 4096, // 0x00001000
      Front = 8192, // 0x00002000
      Back = 16384, // 0x00004000
      RemoveTrafficLights = 32768, // 0x00008000
      AllWayStop = 16777216, // 0x01000000
      Pavement = 33554432, // 0x02000000
      Gravel = 67108864, // 0x04000000
      Tiles = 134217728, // 0x08000000
      Lighting = 268435456, // 0x10000000
      Inside = 536870912, // 0x20000000
      Elevated = 65536, // 0x00010000
      Tunnel = 131072, // 0x00020000
      MiddlePlatform = 262144, // 0x00040000
      WideMedian = 524288, // 0x00080000
      PrimaryMiddleBeautification = 1048576, // 0x00100000
      SecondaryMiddleBeautification = 2097152, // 0x00200000
    }

    [Flags]
    public enum Side : uint
    {
      Raised = 1,
      Lowered = 2,
      LowTransition = 4,
      HighTransition = 8,
      PrimaryTrack = 16, // 0x00000010
      SecondaryTrack = 32, // 0x00000020
      TertiaryTrack = 64, // 0x00000040
      QuaternaryTrack = 128, // 0x00000080
      PrimaryStop = 256, // 0x00000100
      SecondaryStop = 512, // 0x00000200
      PrimaryBeautification = 4096, // 0x00001000
      SecondaryBeautification = 8192, // 0x00002000
      Sidewalk = 65536, // 0x00010000
      WideSidewalk = 131072, // 0x00020000
      ParkingSpaces = 262144, // 0x00040000
      SoundBarrier = 524288, // 0x00080000
      PrimaryLane = 1048576, // 0x00100000
      SecondaryLane = 2097152, // 0x00200000
      TertiaryLane = 4194304, // 0x00400000
      QuaternaryLane = 8388608, // 0x00800000
      ForbidLeftTurn = 16777216, // 0x01000000
      ForbidRightTurn = 33554432, // 0x02000000
      AddCrosswalk = 67108864, // 0x04000000
      RemoveCrosswalk = 134217728, // 0x08000000
      ForbidStraight = 268435456, // 0x10000000
    }
  }
}
