// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Net;
using Game.Rendering;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public static class NetCompositionHelpers
  {
    public static void GetRequirementFlags(
      NetPieceRequirements[] requirements,
      out CompositionFlags compositionFlags,
      out NetSectionFlags sectionFlags)
    {
      compositionFlags = new CompositionFlags();
      sectionFlags = (NetSectionFlags) 0;
      if (requirements == null)
        return;
      for (int index = 0; index < requirements.Length; ++index)
        NetCompositionHelpers.GetRequirementFlags(requirements[index], ref compositionFlags, ref sectionFlags);
    }

    public static void GetRequirementFlags(
      NetPieceRequirements requirement,
      ref CompositionFlags compositionFlags,
      ref NetSectionFlags sectionFlags)
    {
      switch (requirement)
      {
        case NetPieceRequirements.Node:
          compositionFlags.m_General |= CompositionFlags.General.Node;
          break;
        case NetPieceRequirements.Intersection:
          compositionFlags.m_General |= CompositionFlags.General.Intersection;
          break;
        case NetPieceRequirements.DeadEnd:
          compositionFlags.m_General |= CompositionFlags.General.DeadEnd;
          break;
        case NetPieceRequirements.Crosswalk:
          compositionFlags.m_General |= CompositionFlags.General.Crosswalk;
          break;
        case NetPieceRequirements.BusStop:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.Median:
          sectionFlags |= NetSectionFlags.Median;
          break;
        case NetPieceRequirements.TrainStop:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.OppositeTrainStop:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.Inverted:
          sectionFlags |= NetSectionFlags.Invert;
          break;
        case NetPieceRequirements.TaxiStand:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.LevelCrossing:
          compositionFlags.m_General |= CompositionFlags.General.LevelCrossing;
          break;
        case NetPieceRequirements.Elevated:
          compositionFlags.m_General |= CompositionFlags.General.Elevated;
          break;
        case NetPieceRequirements.Tunnel:
          compositionFlags.m_General |= CompositionFlags.General.Tunnel;
          break;
        case NetPieceRequirements.Raised:
          compositionFlags.m_Right |= CompositionFlags.Side.Raised;
          break;
        case NetPieceRequirements.Lowered:
          compositionFlags.m_Right |= CompositionFlags.Side.Lowered;
          break;
        case NetPieceRequirements.LowTransition:
          compositionFlags.m_Right |= CompositionFlags.Side.LowTransition;
          break;
        case NetPieceRequirements.HighTransition:
          compositionFlags.m_Right |= CompositionFlags.Side.HighTransition;
          break;
        case NetPieceRequirements.WideMedian:
          compositionFlags.m_General |= CompositionFlags.General.WideMedian;
          break;
        case NetPieceRequirements.TramTrack:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryTrack;
          break;
        case NetPieceRequirements.TramStop:
          compositionFlags.m_Right |= CompositionFlags.Side.SecondaryStop;
          break;
        case NetPieceRequirements.OppositeTramTrack:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryTrack;
          break;
        case NetPieceRequirements.OppositeTramStop:
          compositionFlags.m_Left |= CompositionFlags.Side.SecondaryStop;
          break;
        case NetPieceRequirements.MedianBreak:
          compositionFlags.m_General |= CompositionFlags.General.MedianBreak;
          break;
        case NetPieceRequirements.ShipStop:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.Sidewalk:
          compositionFlags.m_Right |= CompositionFlags.Side.Sidewalk;
          break;
        case NetPieceRequirements.Edge:
          compositionFlags.m_General |= CompositionFlags.General.Edge;
          break;
        case NetPieceRequirements.SubwayStop:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.OppositeSubwayStop:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.MiddlePlatform:
          compositionFlags.m_General |= CompositionFlags.General.MiddlePlatform;
          break;
        case NetPieceRequirements.Underground:
          sectionFlags |= NetSectionFlags.Underground;
          break;
        case NetPieceRequirements.Roundabout:
          compositionFlags.m_General |= CompositionFlags.General.Roundabout;
          break;
        case NetPieceRequirements.OppositeSidewalk:
          compositionFlags.m_Left |= CompositionFlags.Side.Sidewalk;
          break;
        case NetPieceRequirements.SoundBarrier:
          compositionFlags.m_Right |= CompositionFlags.Side.SoundBarrier;
          break;
        case NetPieceRequirements.Overhead:
          sectionFlags |= NetSectionFlags.Overhead;
          break;
        case NetPieceRequirements.TrafficLights:
          compositionFlags.m_General |= CompositionFlags.General.TrafficLights;
          break;
        case NetPieceRequirements.PublicTransportLane:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryLane;
          break;
        case NetPieceRequirements.OppositePublicTransportLane:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryLane;
          break;
        case NetPieceRequirements.Spillway:
          compositionFlags.m_General |= CompositionFlags.General.Spillway;
          break;
        case NetPieceRequirements.MiddleGrass:
          compositionFlags.m_General |= CompositionFlags.General.PrimaryMiddleBeautification;
          break;
        case NetPieceRequirements.MiddleTrees:
          compositionFlags.m_General |= CompositionFlags.General.SecondaryMiddleBeautification;
          break;
        case NetPieceRequirements.WideSidewalk:
          compositionFlags.m_Right |= CompositionFlags.Side.WideSidewalk;
          break;
        case NetPieceRequirements.SideGrass:
          compositionFlags.m_Right |= CompositionFlags.Side.PrimaryBeautification;
          break;
        case NetPieceRequirements.SideTrees:
          compositionFlags.m_Right |= CompositionFlags.Side.SecondaryBeautification;
          break;
        case NetPieceRequirements.OppositeGrass:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryBeautification;
          break;
        case NetPieceRequirements.OppositeTrees:
          compositionFlags.m_Left |= CompositionFlags.Side.SecondaryBeautification;
          break;
        case NetPieceRequirements.Opening:
          compositionFlags.m_General |= CompositionFlags.General.Opening;
          break;
        case NetPieceRequirements.Front:
          compositionFlags.m_General |= CompositionFlags.General.Front;
          break;
        case NetPieceRequirements.Back:
          compositionFlags.m_General |= CompositionFlags.General.Back;
          break;
        case NetPieceRequirements.Flipped:
          sectionFlags |= NetSectionFlags.FlipMesh;
          break;
        case NetPieceRequirements.RemoveTrafficLights:
          compositionFlags.m_General |= CompositionFlags.General.RemoveTrafficLights;
          break;
        case NetPieceRequirements.AllWayStop:
          compositionFlags.m_General |= CompositionFlags.General.AllWayStop;
          break;
        case NetPieceRequirements.Pavement:
          compositionFlags.m_General |= CompositionFlags.General.Pavement;
          break;
        case NetPieceRequirements.Gravel:
          compositionFlags.m_General |= CompositionFlags.General.Gravel;
          break;
        case NetPieceRequirements.Tiles:
          compositionFlags.m_General |= CompositionFlags.General.Tiles;
          break;
        case NetPieceRequirements.ForbidLeftTurn:
          compositionFlags.m_Right |= CompositionFlags.Side.ForbidLeftTurn;
          break;
        case NetPieceRequirements.ForbidRightTurn:
          compositionFlags.m_Right |= CompositionFlags.Side.ForbidRightTurn;
          break;
        case NetPieceRequirements.OppositeWideSidewalk:
          compositionFlags.m_Left |= CompositionFlags.Side.WideSidewalk;
          break;
        case NetPieceRequirements.OppositeForbidLeftTurn:
          compositionFlags.m_Left |= CompositionFlags.Side.ForbidLeftTurn;
          break;
        case NetPieceRequirements.OppositeForbidRightTurn:
          compositionFlags.m_Left |= CompositionFlags.Side.ForbidRightTurn;
          break;
        case NetPieceRequirements.OppositeSoundBarrier:
          compositionFlags.m_Left |= CompositionFlags.Side.SoundBarrier;
          break;
        case NetPieceRequirements.SidePlatform:
          compositionFlags.m_Right |= CompositionFlags.Side.Sidewalk;
          break;
        case NetPieceRequirements.AddCrosswalk:
          compositionFlags.m_Right |= CompositionFlags.Side.AddCrosswalk;
          break;
        case NetPieceRequirements.RemoveCrosswalk:
          compositionFlags.m_Right |= CompositionFlags.Side.RemoveCrosswalk;
          break;
        case NetPieceRequirements.Lighting:
          compositionFlags.m_General |= CompositionFlags.General.Lighting;
          break;
        case NetPieceRequirements.OppositeBusStop:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.OppositeTaxiStand:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.OppositeRaised:
          compositionFlags.m_Left |= CompositionFlags.Side.Raised;
          break;
        case NetPieceRequirements.OppositeLowered:
          compositionFlags.m_Left |= CompositionFlags.Side.Lowered;
          break;
        case NetPieceRequirements.OppositeLowTransition:
          compositionFlags.m_Left |= CompositionFlags.Side.LowTransition;
          break;
        case NetPieceRequirements.OppositeHighTransition:
          compositionFlags.m_Left |= CompositionFlags.Side.HighTransition;
          break;
        case NetPieceRequirements.OppositeShipStop:
          compositionFlags.m_Left |= CompositionFlags.Side.PrimaryStop;
          break;
        case NetPieceRequirements.OppositePlatform:
          compositionFlags.m_Left |= CompositionFlags.Side.Sidewalk;
          break;
        case NetPieceRequirements.OppositeAddCrosswalk:
          compositionFlags.m_Left |= CompositionFlags.Side.AddCrosswalk;
          break;
        case NetPieceRequirements.OppositeRemoveCrosswalk:
          compositionFlags.m_Left |= CompositionFlags.Side.RemoveCrosswalk;
          break;
        case NetPieceRequirements.Inside:
          compositionFlags.m_General |= CompositionFlags.General.Inside;
          break;
        case NetPieceRequirements.ForbidStraight:
          compositionFlags.m_Right |= CompositionFlags.Side.ForbidStraight;
          break;
        case NetPieceRequirements.OppositeForbidStraight:
          compositionFlags.m_Left |= CompositionFlags.Side.ForbidStraight;
          break;
        case NetPieceRequirements.Hidden:
          sectionFlags |= NetSectionFlags.Hidden;
          break;
        case NetPieceRequirements.ParkingSpaces:
          compositionFlags.m_Right |= CompositionFlags.Side.ParkingSpaces;
          break;
        case NetPieceRequirements.OppositeParkingSpaces:
          compositionFlags.m_Left |= CompositionFlags.Side.ParkingSpaces;
          break;
      }
    }

    public static CompositionFlags InvertCompositionFlags(CompositionFlags flags)
    {
      return new CompositionFlags(flags.m_General, flags.m_Right, flags.m_Left);
    }

    public static NetSectionFlags InvertSectionFlags(NetSectionFlags flags) => flags;

    public static bool TestSectionFlags(
      NetGeometrySection section,
      CompositionFlags compositionFlags)
    {
      if (((section.m_CompositionAll | section.m_CompositionNone) & compositionFlags) != section.m_CompositionAll)
        return false;
      return section.m_CompositionAny == new CompositionFlags() || (section.m_CompositionAny & compositionFlags) != new CompositionFlags();
    }

    public static bool TestSubSectionFlags(
      NetSubSection subSection,
      CompositionFlags compositionFlags,
      NetSectionFlags sectionFlags)
    {
      if ((sectionFlags & NetSectionFlags.Median) == (NetSectionFlags) 0)
        compositionFlags.m_General &= ~CompositionFlags.General.MedianBreak;
      if (((subSection.m_CompositionAll | subSection.m_CompositionNone) & compositionFlags) != subSection.m_CompositionAll || ((subSection.m_SectionAll | subSection.m_SectionNone) & sectionFlags) != subSection.m_SectionAll)
        return false;
      return subSection.m_CompositionAny == new CompositionFlags() && subSection.m_SectionAny == (NetSectionFlags) 0 || (subSection.m_CompositionAny & compositionFlags) != new CompositionFlags() || (subSection.m_SectionAny & sectionFlags) != 0;
    }

    public static bool TestPieceFlags(
      NetSectionPiece piece,
      CompositionFlags compositionFlags,
      NetSectionFlags sectionFlags)
    {
      if ((sectionFlags & NetSectionFlags.Median) == (NetSectionFlags) 0)
        compositionFlags.m_General &= ~CompositionFlags.General.MedianBreak;
      if (((piece.m_CompositionAll | piece.m_CompositionNone) & compositionFlags) != piece.m_CompositionAll || ((piece.m_SectionAll | piece.m_SectionNone) & sectionFlags) != piece.m_SectionAll)
        return false;
      return piece.m_CompositionAny == new CompositionFlags() && piece.m_SectionAny == (NetSectionFlags) 0 || (piece.m_CompositionAny & compositionFlags) != new CompositionFlags() || (piece.m_SectionAny & sectionFlags) != 0;
    }

    public static bool TestPieceFlags2(
      NetSectionPiece piece,
      CompositionFlags compositionFlags,
      NetSectionFlags sectionFlags)
    {
      if ((compositionFlags.m_General & CompositionFlags.General.Roundabout) != (CompositionFlags.General) 0 && (piece.m_Flags & NetPieceFlags.Side) != (NetPieceFlags) 0)
      {
        CompositionFlags compositionFlags1 = compositionFlags;
        if ((compositionFlags1.m_General & CompositionFlags.General.Elevated) != (CompositionFlags.General) 0)
        {
          if ((compositionFlags1.m_Left & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Elevated;
            compositionFlags1.m_Left &= ~CompositionFlags.Side.HighTransition;
          }
          else if ((compositionFlags1.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Elevated;
            compositionFlags1.m_Left &= ~CompositionFlags.Side.LowTransition;
            compositionFlags1.m_Left |= CompositionFlags.Side.Raised;
          }
          if ((compositionFlags1.m_Right & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Elevated;
            compositionFlags1.m_Right &= ~CompositionFlags.Side.HighTransition;
          }
          else if ((compositionFlags1.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Elevated;
            compositionFlags1.m_Right &= ~CompositionFlags.Side.LowTransition;
            compositionFlags1.m_Right |= CompositionFlags.Side.Raised;
          }
        }
        else if ((compositionFlags1.m_General & CompositionFlags.General.Tunnel) != (CompositionFlags.General) 0)
        {
          if ((compositionFlags1.m_Left & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Tunnel;
            compositionFlags1.m_Left &= ~CompositionFlags.Side.HighTransition;
          }
          else if ((compositionFlags1.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Tunnel;
            compositionFlags1.m_Left &= ~CompositionFlags.Side.LowTransition;
            compositionFlags1.m_Left |= CompositionFlags.Side.Lowered;
          }
          if ((compositionFlags1.m_Right & CompositionFlags.Side.HighTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Tunnel;
            compositionFlags1.m_Right &= ~CompositionFlags.Side.HighTransition;
          }
          else if ((compositionFlags1.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            compositionFlags1.m_General &= ~CompositionFlags.General.Tunnel;
            compositionFlags1.m_Right &= ~CompositionFlags.Side.LowTransition;
            compositionFlags1.m_Right |= CompositionFlags.Side.Lowered;
          }
        }
        else
        {
          if ((compositionFlags1.m_Left & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            if ((compositionFlags1.m_Left & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Left &= ~(CompositionFlags.Side.Raised | CompositionFlags.Side.LowTransition);
            else if ((compositionFlags1.m_Left & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Left &= ~(CompositionFlags.Side.Lowered | CompositionFlags.Side.LowTransition);
            else if ((compositionFlags1.m_Left & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Left &= ~(CompositionFlags.Side.LowTransition | CompositionFlags.Side.SoundBarrier);
          }
          if ((compositionFlags1.m_Right & CompositionFlags.Side.LowTransition) != (CompositionFlags.Side) 0)
          {
            if ((compositionFlags1.m_Right & CompositionFlags.Side.Raised) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Right &= ~(CompositionFlags.Side.Raised | CompositionFlags.Side.LowTransition);
            else if ((compositionFlags1.m_Right & CompositionFlags.Side.Lowered) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Right &= ~(CompositionFlags.Side.Lowered | CompositionFlags.Side.LowTransition);
            else if ((compositionFlags1.m_Right & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
              compositionFlags1.m_Right &= ~(CompositionFlags.Side.LowTransition | CompositionFlags.Side.SoundBarrier);
          }
        }
        if (compositionFlags != compositionFlags1)
          return NetCompositionHelpers.TestPieceFlags(piece, compositionFlags1, sectionFlags);
      }
      return false;
    }

    public static bool TestObjectFlags(
      NetPieceObject _object,
      CompositionFlags compositionFlags,
      NetSectionFlags sectionFlags)
    {
      if ((sectionFlags & NetSectionFlags.Median) == (NetSectionFlags) 0)
        compositionFlags.m_General &= ~CompositionFlags.General.MedianBreak;
      if (((_object.m_CompositionAll | _object.m_CompositionNone) & compositionFlags) != _object.m_CompositionAll || ((_object.m_SectionAll | _object.m_SectionNone) & sectionFlags) != _object.m_SectionAll)
        return false;
      return _object.m_CompositionAny == new CompositionFlags() && _object.m_SectionAny == (NetSectionFlags) 0 || (_object.m_CompositionAny & compositionFlags) != new CompositionFlags() || (_object.m_SectionAny & sectionFlags) != 0;
    }

    public static bool TestLaneFlags(AuxiliaryNetLane lane, CompositionFlags compositionFlags)
    {
      if (((lane.m_CompositionAll | lane.m_CompositionNone) & compositionFlags) != lane.m_CompositionAll)
        return false;
      return lane.m_CompositionAny == new CompositionFlags() || (lane.m_CompositionAny & compositionFlags) != new CompositionFlags();
    }

    public static bool TestEdgeFlags(
      NetGeometryEdgeState edgeState,
      CompositionFlags compositionFlags)
    {
      if (((edgeState.m_CompositionAll | edgeState.m_CompositionNone) & compositionFlags) != edgeState.m_CompositionAll)
        return false;
      return edgeState.m_CompositionAny == new CompositionFlags() || (edgeState.m_CompositionAny & compositionFlags) != new CompositionFlags();
    }

    public static bool TestEdgeFlags(
      NetGeometryNodeState nodeState,
      CompositionFlags compositionFlags)
    {
      if (((nodeState.m_CompositionAll | nodeState.m_CompositionNone) & compositionFlags) != nodeState.m_CompositionAll)
        return false;
      return nodeState.m_CompositionAny == new CompositionFlags() || (nodeState.m_CompositionAny & compositionFlags) != new CompositionFlags();
    }

    public static bool TestEdgeFlags(
      ElectricityConnectionData electricityConnectionData,
      CompositionFlags compositionFlags)
    {
      if (((electricityConnectionData.m_CompositionAll | electricityConnectionData.m_CompositionNone) & compositionFlags) != electricityConnectionData.m_CompositionAll)
        return false;
      return electricityConnectionData.m_CompositionAny == new CompositionFlags() || (electricityConnectionData.m_CompositionAny & compositionFlags) != new CompositionFlags();
    }

    public static bool TestEdgeMatch(NetGeometryNodeState nodeState, bool2 match)
    {
      switch (nodeState.m_MatchType)
      {
        case NetEdgeMatchType.Both:
          return math.all(match);
        case NetEdgeMatchType.Any:
          return math.any(match);
        case NetEdgeMatchType.Exclusive:
          return match.x != match.y;
        default:
          return false;
      }
    }

    public static void GetCompositionPieces(
      NativeList<NetCompositionPiece> resultBuffer,
      NativeArray<NetGeometrySection> geometrySections,
      CompositionFlags flags,
      BufferLookup<NetSubSection> subSectionData,
      BufferLookup<NetSectionPiece> sectionPieceData)
    {
      int num1 = 0;
      int num2 = 0;
      CompositionFlags compositionFlags1 = NetCompositionHelpers.InvertCompositionFlags(flags);
      for (int index1 = 0; index1 < geometrySections.Length; ++index1)
      {
        NetGeometrySection geometrySection;
        if ((flags.m_General & CompositionFlags.General.Invert) != (CompositionFlags.General) 0)
        {
          geometrySection = geometrySections[geometrySections.Length - 1 - index1];
          geometrySection.m_Flags ^= NetSectionFlags.Invert | NetSectionFlags.FlipLanes;
          if ((geometrySection.m_Flags & NetSectionFlags.Left) != (NetSectionFlags) 0)
          {
            geometrySection.m_Flags &= ~NetSectionFlags.Left;
            geometrySection.m_Flags |= NetSectionFlags.Right;
          }
          else if ((geometrySection.m_Flags & NetSectionFlags.Right) != (NetSectionFlags) 0)
          {
            geometrySection.m_Flags &= ~NetSectionFlags.Right;
            geometrySection.m_Flags |= NetSectionFlags.Left;
          }
        }
        else
          geometrySection = geometrySections[index1];
        if ((flags.m_General & CompositionFlags.General.Flip) != (CompositionFlags.General) 0)
          geometrySection.m_Flags ^= NetSectionFlags.FlipLanes;
        CompositionFlags compositionFlags2 = (geometrySection.m_Flags & NetSectionFlags.Invert) != (NetSectionFlags) 0 ? compositionFlags1 : flags;
        NetPieceFlags netPieceFlags = (NetPieceFlags) 0;
        if ((geometrySection.m_Flags & NetSectionFlags.HiddenSurface) != (NetSectionFlags) 0)
          netPieceFlags |= NetPieceFlags.Surface;
        if ((geometrySection.m_Flags & NetSectionFlags.HiddenBottom) != (NetSectionFlags) 0)
          netPieceFlags |= NetPieceFlags.Bottom;
        if ((geometrySection.m_Flags & NetSectionFlags.HiddenTop) != (NetSectionFlags) 0)
          netPieceFlags |= NetPieceFlags.Top;
        if ((geometrySection.m_Flags & NetSectionFlags.HiddenSide) != (NetSectionFlags) 0)
          netPieceFlags |= NetPieceFlags.Side;
        if (NetCompositionHelpers.TestSectionFlags(geometrySection, compositionFlags2))
        {
          NetSectionFlags sectionFlags = (geometrySection.m_Flags & NetSectionFlags.Invert) != (NetSectionFlags) 0 ? NetCompositionHelpers.InvertSectionFlags(geometrySection.m_Flags) : geometrySection.m_Flags;
label_19:
          DynamicBuffer<NetSubSection> dynamicBuffer1 = subSectionData[geometrySection.m_Section];
          for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
          {
            NetSubSection subSection = dynamicBuffer1[index2];
            if (NetCompositionHelpers.TestSubSectionFlags(subSection, compositionFlags2, sectionFlags))
            {
              geometrySection.m_Section = subSection.m_SubSection;
              goto label_19;
            }
          }
          DynamicBuffer<NetSectionPiece> dynamicBuffer2 = sectionPieceData[geometrySection.m_Section];
          for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
          {
            NetSectionPiece piece = dynamicBuffer2[index3];
            NetPieceFlags flags1 = piece.m_Flags;
            if (!NetCompositionHelpers.TestPieceFlags(piece, compositionFlags2, sectionFlags))
            {
              if (NetCompositionHelpers.TestPieceFlags2(piece, compositionFlags2, sectionFlags))
                flags1 |= NetPieceFlags.SkipBottomHalf;
              else
                continue;
            }
            NetCompositionPiece compositionPiece = new NetCompositionPiece();
            compositionPiece.m_Piece = piece.m_Piece;
            compositionPiece.m_SectionFlags = geometrySection.m_Flags;
            compositionPiece.m_PieceFlags = flags1;
            compositionPiece.m_SectionIndex = num1;
            compositionPiece.m_Offset = geometrySection.m_Offset + piece.m_Offset;
            if ((netPieceFlags & flags1) != (NetPieceFlags) 0)
              compositionPiece.m_SectionFlags |= NetSectionFlags.Hidden;
            resultBuffer.Add(in compositionPiece);
            ++num2;
          }
          if (num2 != 0)
          {
            ++num1;
            num2 = 0;
          }
        }
      }
    }

    public static void CalculateCompositionData(
      ref NetCompositionData compositionData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<NetPieceData> netPieceData,
      ComponentLookup<NetLaneData> netLaneData,
      ComponentLookup<NetVertexMatchData> netVertexMatchData,
      BufferLookup<NetPieceLane> netPieceLanes)
    {
      NetCompositionHelpers.CalculateCompositionPieceOffsets(ref compositionData, pieces, netPieceData);
      NetCompositionHelpers.CalculateSyncVertexOffsets(ref compositionData, pieces, netVertexMatchData);
      NetCompositionHelpers.CalculateRoundaboutSize(ref compositionData, pieces, netLaneData, netPieceLanes);
    }

    public static void CalculateMinLod(
      ref NetCompositionData compositionData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<MeshData> meshDatas)
    {
      float bias = 0.0f;
      for (int index = 0; index < pieces.Length; ++index)
      {
        NetCompositionPiece piece = pieces[index];
        MeshData meshData = meshDatas[piece.m_Piece];
        bias += meshData.m_LodBias;
      }
      if (pieces.Length != 0)
        bias /= (float) pieces.Length;
      float2 size = new float2(compositionData.m_Width, MathUtils.Size(compositionData.m_HeightRange));
      compositionData.m_MinLod = RenderingUtils.CalculateLodLimit(RenderingUtils.GetRenderingSize(size), bias);
    }

    private static void CalculateCompositionPieceOffsets(
      ref NetCompositionData compositionData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<NetPieceData> netPieceData)
    {
      compositionData.m_Width = 0.0f;
      compositionData.m_MiddleOffset = 0.0f;
      compositionData.m_WidthOffset = 0.0f;
      compositionData.m_NodeOffset = 0.0f;
      compositionData.m_HeightRange = new Bounds1(float.MaxValue, float.MinValue);
      compositionData.m_SurfaceHeight = new Bounds1(float.MaxValue, float.MinValue);
      bool c1 = (compositionData.m_Flags.m_General & CompositionFlags.General.Invert) > (CompositionFlags.General) 0;
      float x1 = 0.0f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float y = 0.0f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      float num5 = 0.0f;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      int index1;
      for (int index2 = 0; index2 < pieces.Length; index2 = index1)
      {
        NetCompositionPiece piece1 = pieces[index2];
        bool flag5 = (piece1.m_SectionFlags & NetSectionFlags.Underground) != 0;
        bool flag6 = (piece1.m_SectionFlags & NetSectionFlags.Overhead) != 0;
        bool c2 = (piece1.m_SectionFlags & NetSectionFlags.Invert) != 0;
        bool c3 = (piece1.m_SectionFlags & NetSectionFlags.FlipMesh) != 0;
        bool2 bool2 = new bool2((piece1.m_SectionFlags & NetSectionFlags.Left) != 0, (piece1.m_SectionFlags & NetSectionFlags.Right) != 0);
        NetPieceData netPieceData1 = netPieceData[piece1.m_Piece];
        float x2 = netPieceData1.m_Width;
        compositionData.m_HeightRange |= piece1.m_Offset.y + netPieceData1.m_HeightRange;
        if (!flag5 && !flag6 && (piece1.m_PieceFlags & NetPieceFlags.PreserveShape) == (NetPieceFlags) 0)
        {
          if ((piece1.m_PieceFlags & NetPieceFlags.Side) != (NetPieceFlags) 0)
          {
            float4 a = math.select(netPieceData1.m_SurfaceHeights, netPieceData1.m_SurfaceHeights.yxwz, c2);
            float4 b = math.select(a, a.zwxy, c3);
            compositionData.m_EdgeHeights = math.select(compositionData.m_EdgeHeights, b, bool2.xyxy);
          }
          if ((piece1.m_PieceFlags & NetPieceFlags.Surface) != (NetPieceFlags) 0)
          {
            compositionData.m_SurfaceHeight.min = math.min(compositionData.m_SurfaceHeight.min, piece1.m_Offset.y + math.cmin(netPieceData1.m_SurfaceHeights));
            compositionData.m_SurfaceHeight.max = math.max(compositionData.m_SurfaceHeight.max, piece1.m_Offset.y + math.cmax(netPieceData1.m_SurfaceHeights));
            flag1 = true;
          }
        }
        compositionData.m_WidthOffset = math.max(compositionData.m_WidthOffset, netPieceData1.m_WidthOffset);
        compositionData.m_NodeOffset = math.max(compositionData.m_NodeOffset, netPieceData1.m_NodeOffset);
        piece1.m_Size.x = netPieceData1.m_Width;
        piece1.m_Size.y = netPieceData1.m_HeightRange.max - netPieceData1.m_HeightRange.min;
        piece1.m_Size.z = netPieceData1.m_Length;
        for (index1 = index2 + 1; index1 < pieces.Length; ++index1)
        {
          NetCompositionPiece piece2 = pieces[index1];
          if (piece2.m_SectionIndex == piece1.m_SectionIndex)
          {
            NetPieceData netPieceData2 = netPieceData[piece2.m_Piece];
            x2 = math.max(x2, netPieceData2.m_Width);
            compositionData.m_HeightRange |= piece2.m_Offset.y + netPieceData2.m_HeightRange;
            if (!flag5 && !flag6 && (piece2.m_PieceFlags & NetPieceFlags.PreserveShape) == (NetPieceFlags) 0)
            {
              if ((piece2.m_PieceFlags & NetPieceFlags.Side) != (NetPieceFlags) 0)
              {
                float4 a = math.select(netPieceData2.m_SurfaceHeights, netPieceData2.m_SurfaceHeights.yxwz, c2);
                float4 b = math.select(a, a.zwxy, c3);
                compositionData.m_EdgeHeights = math.select(compositionData.m_EdgeHeights, b, bool2.xyxy);
              }
              if ((piece2.m_PieceFlags & NetPieceFlags.Surface) != (NetPieceFlags) 0)
              {
                compositionData.m_SurfaceHeight.min = math.min(compositionData.m_SurfaceHeight.min, piece2.m_Offset.y + math.cmin(netPieceData2.m_SurfaceHeights));
                compositionData.m_SurfaceHeight.max = math.max(compositionData.m_SurfaceHeight.max, piece2.m_Offset.y + math.cmax(netPieceData2.m_SurfaceHeights));
                flag1 = true;
              }
            }
            compositionData.m_WidthOffset = math.max(compositionData.m_WidthOffset, netPieceData2.m_WidthOffset);
            compositionData.m_NodeOffset = math.max(compositionData.m_NodeOffset, netPieceData2.m_NodeOffset);
            piece2.m_Size.x = netPieceData2.m_Width;
            piece2.m_Size.y = netPieceData2.m_HeightRange.max - netPieceData2.m_HeightRange.min;
            piece2.m_Size.z = netPieceData2.m_Length;
            pieces[index1] = piece2;
          }
          else
            break;
        }
        float x3 = piece1.m_Offset.x;
        if (flag5)
        {
          piece1.m_Offset.x += x1 + x2 * 0.5f;
          x1 += x2;
          if ((piece1.m_SectionFlags & (NetSectionFlags.Median | NetSectionFlags.AlignCenter)) == NetSectionFlags.Median)
          {
            num1 = piece1.m_Offset.x - math.select(x3 * 2f, 0.0f, c1);
            num2 = x3;
            flag3 = true;
          }
          else if ((piece1.m_SectionFlags & (NetSectionFlags.Right | NetSectionFlags.AlignCenter)) == NetSectionFlags.Right && !flag3)
          {
            num1 = piece1.m_Offset.x - piece1.m_Size.x * 0.5f - math.select(x3 * 2f, 0.0f, c1);
            num2 = x3;
            flag3 = true;
          }
        }
        else if (flag6)
        {
          piece1.m_Offset.x += y + x2 * 0.5f;
          y += x2;
          if ((piece1.m_SectionFlags & (NetSectionFlags.Median | NetSectionFlags.AlignCenter)) == NetSectionFlags.Median)
          {
            num3 = piece1.m_Offset.x - math.select(x3 * 2f, 0.0f, c1);
            num4 = x3;
            flag4 = true;
          }
          else if ((piece1.m_SectionFlags & (NetSectionFlags.Right | NetSectionFlags.AlignCenter)) == NetSectionFlags.Right && !flag4)
          {
            num3 = piece1.m_Offset.x - piece1.m_Size.x * 0.5f - math.select(x3 * 2f, 0.0f, c1);
            num4 = x3;
            flag4 = true;
          }
        }
        else
        {
          piece1.m_Offset.x += compositionData.m_Width + x2 * 0.5f;
          compositionData.m_Width += x2;
          if ((piece1.m_SectionFlags & (NetSectionFlags.Median | NetSectionFlags.AlignCenter)) == NetSectionFlags.Median)
          {
            compositionData.m_MiddleOffset = piece1.m_Offset.x - math.select(x3 * 2f, 0.0f, c1);
            num5 = x3;
            flag2 = true;
          }
          else if ((piece1.m_SectionFlags & (NetSectionFlags.Right | NetSectionFlags.AlignCenter)) == NetSectionFlags.Right && !flag2)
          {
            compositionData.m_MiddleOffset = piece1.m_Offset.x - piece1.m_Size.x * 0.5f - math.select(x3 * 2f, 0.0f, c1);
            num5 = x3;
            flag2 = true;
          }
        }
        pieces[index2] = piece1;
        for (int index3 = index2 + 1; index3 < index1; ++index3)
        {
          NetCompositionPiece piece3 = pieces[index3];
          piece3.m_Offset.x = piece1.m_Offset.x;
          pieces[index3] = piece3;
        }
        if ((piece1.m_PieceFlags & NetPieceFlags.Side) != (NetPieceFlags) 0 && index1 > index2 + 1 && (piece1.m_SectionFlags & (NetSectionFlags.Left | NetSectionFlags.Right)) != (NetSectionFlags) 0)
        {
          bool c4 = (piece1.m_SectionFlags & NetSectionFlags.Right) != 0;
          for (int index4 = index2; index4 < index1; ++index4)
          {
            NetCompositionPiece piece4 = pieces[index4];
            float b = (float) (((double) netPieceData[piece4.m_Piece].m_Width - (double) x2) * 0.5);
            piece4.m_Offset.x += math.select(-b, b, c4);
            pieces[index4] = piece4;
          }
        }
      }
      if (flag3)
        num1 -= x1 * 0.5f;
      if (flag4)
        num3 -= y * 0.5f;
      if (flag2)
        compositionData.m_MiddleOffset -= compositionData.m_Width * 0.5f;
      if ((compositionData.m_Flags.m_General & (CompositionFlags.General.DeadEnd | CompositionFlags.General.LevelCrossing)) == CompositionFlags.General.LevelCrossing || (compositionData.m_Flags.m_General & (CompositionFlags.General.DeadEnd | CompositionFlags.General.Intersection | CompositionFlags.General.Crosswalk)) == CompositionFlags.General.Crosswalk)
        compositionData.m_State |= CompositionState.BlockUTurn;
      for (int index5 = 0; index5 < pieces.Length; ++index5)
      {
        NetCompositionPiece piece = pieces[index5];
        int num6 = (piece.m_SectionFlags & NetSectionFlags.Underground) != 0 ? 1 : 0;
        bool flag7 = (piece.m_SectionFlags & NetSectionFlags.Overhead) != 0;
        if ((piece.m_PieceFlags & (NetPieceFlags.PreserveShape | NetPieceFlags.BlockTraffic)) == NetPieceFlags.BlockTraffic)
          compositionData.m_State |= CompositionState.BlockUTurn;
        if ((piece.m_PieceFlags & NetPieceFlags.LowerBottomToTerrain) != (NetPieceFlags) 0)
          compositionData.m_State |= CompositionState.LowerToTerrain;
        if ((piece.m_PieceFlags & NetPieceFlags.RaiseTopToTerrain) != (NetPieceFlags) 0)
          compositionData.m_State |= CompositionState.RaiseToTerrain;
        if (num6 != 0)
        {
          piece.m_Offset.x -= x1 * 0.5f + num2;
          piece.m_Offset.x += compositionData.m_MiddleOffset - num1;
        }
        else if (flag7)
        {
          piece.m_Offset.x -= y * 0.5f + num4;
          piece.m_Offset.x += compositionData.m_MiddleOffset - num3;
        }
        else
          piece.m_Offset.x -= compositionData.m_Width * 0.5f + num5;
        pieces[index5] = piece;
      }
      compositionData.m_Width = math.max(compositionData.m_Width, math.max(x1, y));
      if (flag1)
      {
        compositionData.m_State |= CompositionState.HasSurface;
        if ((compositionData.m_Flags.m_General & (CompositionFlags.General.Elevated | CompositionFlags.General.Tunnel)) != (CompositionFlags.General) 0)
          return;
        compositionData.m_State |= CompositionState.ExclusiveGround;
      }
      else
      {
        float num7 = MathUtils.Center(compositionData.m_HeightRange);
        compositionData.m_SurfaceHeight = new Bounds1(num7, num7);
      }
    }

    private static void CalculateSyncVertexOffsets(
      ref NetCompositionData compositionData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<NetVertexMatchData> netVertexMatchData)
    {
      float4 float4_1 = new float4(0.0f, 0.0f, 0.0f, 1f);
      float4 float4_2 = new float4(0.0f, 1f, 1f, 1f);
      float middleOffset = compositionData.m_MiddleOffset;
      float num1 = compositionData.m_Width * 0.5f + middleOffset;
      float num2 = compositionData.m_Width * 0.5f - middleOffset;
      bool flag = false;
      for (int index1 = 0; index1 < pieces.Length; ++index1)
      {
        NetCompositionPiece piece = pieces[index1];
        if ((piece.m_SectionFlags & (NetSectionFlags.Underground | NetSectionFlags.Overhead)) == (NetSectionFlags) 0)
        {
          if ((piece.m_SectionFlags & NetSectionFlags.Median) != (NetSectionFlags) 0)
          {
            if (netVertexMatchData.HasComponent(piece.m_Piece))
            {
              NetVertexMatchData netVertexMatchData1 = netVertexMatchData[piece.m_Piece];
              if (!math.any(math.isnan(netVertexMatchData1.m_Offsets.xy)))
              {
                float2 float2;
                float2.x = netVertexMatchData1.m_Offsets.x;
                float2.y = math.select(netVertexMatchData1.m_Offsets.z, netVertexMatchData1.m_Offsets.y, math.isnan(netVertexMatchData1.m_Offsets.z));
                if ((piece.m_SectionFlags & NetSectionFlags.Invert) != (NetSectionFlags) 0)
                  float2 = -float2.yx;
                float2 += piece.m_Offset.x;
                if ((double) num1 > 0.0)
                  float4_1.w = (float) (((double) float2.x - (double) middleOffset) / (double) num1 + 1.0);
                if ((double) num2 > 0.0)
                  float4_2.x = (float2.y - middleOffset) / num2;
                flag = true;
              }
            }
            if (!flag)
            {
              float2 x = (float2) piece.m_Offset.x;
              x.x -= piece.m_Size.x * 0.5f;
              x.y += piece.m_Size.x * 0.5f;
              if ((double) num1 > 0.0)
                float4_1.w = (float) (((double) x.x - (double) middleOffset) / (double) num1 + 1.0);
              if ((double) num2 > 0.0)
                float4_2.x = (x.y - middleOffset) / num2;
            }
          }
          else if (netVertexMatchData.HasComponent(piece.m_Piece))
          {
            NetVertexMatchData netVertexMatchData2 = netVertexMatchData[piece.m_Piece];
            if (!math.isnan(netVertexMatchData2.m_Offsets.x))
            {
              float num3 = netVertexMatchData2.m_Offsets.x;
              for (int index2 = 0; index2 < 3; ++index2)
              {
                if ((piece.m_SectionFlags & NetSectionFlags.Invert) != (NetSectionFlags) 0)
                  num3 = -num3;
                float num4 = num3 + piece.m_Offset.x;
                if ((piece.m_SectionFlags & NetSectionFlags.Right) != (NetSectionFlags) 0)
                {
                  float num5 = (num4 - middleOffset) / num2;
                  if ((double) float4_2.z != 1.0)
                    float4_2.w = num5;
                  else if ((double) float4_2.y != 1.0)
                    float4_2.z = num5;
                  else
                    float4_2.y = num5;
                }
                else
                {
                  float num6 = (float) (((double) num4 - (double) middleOffset) / (double) num1 + 1.0);
                  if ((double) float4_1.y != 0.0)
                    float4_1.x = num6;
                  else if ((double) float4_1.z != 0.0)
                    float4_1.y = num6;
                  else
                    float4_1.z = num6;
                }
                if (index2 == 0)
                {
                  if (!math.isnan(netVertexMatchData2.m_Offsets.y))
                    num3 = netVertexMatchData2.m_Offsets.y;
                  else
                    break;
                }
                else if (!math.isnan(netVertexMatchData2.m_Offsets.z))
                  num3 = netVertexMatchData2.m_Offsets.z;
                else
                  break;
              }
            }
          }
        }
      }
      if ((double) float4_1.x > (double) float4_1.y)
        float4_1.xy = float4_1.yx;
      if ((double) float4_2.z > (double) float4_2.w)
        float4_2.zw = float4_2.wz;
      if ((double) float4_1.y > (double) float4_1.z)
        float4_1.yz = float4_1.zy;
      if ((double) float4_2.y > (double) float4_2.z)
        float4_2.yz = float4_2.zy;
      if ((double) float4_1.x > (double) float4_1.y)
        float4_1.xy = float4_1.yx;
      if ((double) float4_2.z > (double) float4_2.w)
        float4_2.zw = float4_2.wz;
      if ((double) float4_1.z <= (double) float4_1.x)
        float4_1.z = math.lerp(float4_1.x, float4_1.w, 0.6666667f);
      if ((double) float4_2.y >= (double) float4_2.w)
        float4_2.y = math.lerp(float4_2.w, float4_2.x, 0.6666667f);
      if ((double) float4_1.y <= (double) float4_1.x)
        float4_1.y = math.lerp(float4_1.x, float4_1.z, 0.5f);
      if ((double) float4_2.z >= (double) float4_2.w)
        float4_2.z = math.lerp(float4_2.w, float4_2.y, 0.5f);
      if ((double) float4_1.y < (double) float4_1.x + 9.9999997473787516E-06)
        float4_1.y = float4_1.x;
      if ((double) float4_1.w < (double) float4_1.z + 9.9999997473787516E-06)
        float4_1.z = float4_1.w;
      if ((double) float4_2.y < (double) float4_2.x + 9.9999997473787516E-06)
        float4_2.y = float4_2.x;
      if ((double) float4_2.w < (double) float4_2.z + 9.9999997473787516E-06)
        float4_2.z = float4_2.w;
      compositionData.m_SyncVertexOffsetsLeft = float4_1;
      compositionData.m_SyncVertexOffsetsRight = float4_2;
    }

    public static void CalculatePlaceableData(
      ref PlaceableNetComposition placeableData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<PlaceableNetPieceData> placeableNetPieceData)
    {
      placeableData.m_ConstructionCost = 0U;
      placeableData.m_UpkeepCost = 0.0f;
      for (int index = 0; index < pieces.Length; ++index)
      {
        NetCompositionPiece piece = pieces[index];
        if (placeableNetPieceData.HasComponent(piece.m_Piece))
        {
          PlaceableNetPieceData placeableNetPieceData1 = placeableNetPieceData[piece.m_Piece];
          placeableData.m_ConstructionCost += placeableNetPieceData1.m_ConstructionCost;
          placeableData.m_ElevationCost += placeableNetPieceData1.m_ElevationCost;
          placeableData.m_UpkeepCost += placeableNetPieceData1.m_UpkeepCost;
        }
      }
    }

    public static void AddCompositionLanes<TNetCompositionPieceList>(
      Entity entity,
      ref NetCompositionData compositionData,
      TNetCompositionPieceList pieces,
      NativeList<NetCompositionLane> netLanes,
      DynamicBuffer<NetCompositionCarriageway> carriageways,
      ComponentLookup<NetLaneData> netLaneData,
      BufferLookup<NetPieceLane> netPieceLanes)
      where TNetCompositionPieceList : INativeList<NetCompositionPiece>
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      bool flag1 = true;
      Bounds3 bounds3_1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
      Bounds3 bounds3_2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
      bool2 x1 = new bool2();
      bool2 x2 = new bool2();
      NetCompositionCarriageway elem = new NetCompositionCarriageway();
      NativeList<NetCompositionHelpers.TempLaneData> nativeList1 = new NativeList<NetCompositionHelpers.TempLaneData>(256, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeList<NetCompositionHelpers.TempLaneGroup> nativeList2 = new NativeList<NetCompositionHelpers.TempLaneGroup>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      for (int index = 0; index < pieces.Length; ++index)
      {
        NetCompositionPiece piece = pieces[index];
        if ((piece.m_PieceFlags & NetPieceFlags.BlockTraffic) != (NetPieceFlags) 0 && num3 != 0)
        {
          ++num2;
          num3 = 0;
          if (math.all(x2))
            flag1 = false;
          if (math.any(x1 | x2) && carriageways.IsCreated)
          {
            Bounds3 bounds = bounds3_1 | bounds3_2;
            bool2 x3 = x1 | x2;
            if (math.any(x1 != x2))
            {
              if (math.all(x2) & math.any(x1))
              {
                bounds = bounds3_1;
                x3 = x1;
              }
              else if (math.any(x2))
              {
                bounds = bounds3_2;
                x3 = x2;
              }
            }
            elem.m_Position = MathUtils.Center(bounds);
            elem.m_Width = MathUtils.Size(bounds).x;
            if (math.all(x3))
            {
              elem.m_Flags &= ~LaneFlags.Invert;
              elem.m_Flags |= LaneFlags.Twoway;
            }
            else if (x3.x)
            {
              elem.m_Flags &= ~(LaneFlags.Invert | LaneFlags.Twoway);
            }
            else
            {
              elem.m_Flags &= ~LaneFlags.Twoway;
              elem.m_Flags |= LaneFlags.Invert;
            }
            carriageways.Add(elem);
          }
          bounds3_1 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
          bounds3_2 = new Bounds3((float3) float.MaxValue, (float3) float.MinValue);
          x1 = new bool2();
          x2 = new bool2();
          elem = new NetCompositionCarriageway();
        }
        if (netPieceLanes.HasBuffer(piece.m_Piece))
        {
          DynamicBuffer<NetPieceLane> netPieceLane1 = netPieceLanes[piece.m_Piece];
          bool c = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
          bool flag2 = (piece.m_SectionFlags & NetSectionFlags.FlipLanes) != 0;
          for (int a = 0; a < netPieceLane1.Length; ++a)
          {
            NetPieceLane netPieceLane2 = netPieceLane1[math.select(a, netPieceLane1.Length - a - 1, c)];
            NetLaneData netLaneData1 = netLaneData[netPieceLane2.m_Lane];
            NetCompositionHelpers.TempLaneData tempLaneData = new NetCompositionHelpers.TempLaneData();
            NetCompositionHelpers.TempLaneGroup tempLaneGroup = new NetCompositionHelpers.TempLaneGroup();
            netLaneData1.m_Flags |= netPieceLane2.m_ExtraFlags;
            if (c)
              netPieceLane2.m_Position.x = -netPieceLane2.m_Position.x;
            if ((netLaneData1.m_Flags & LaneFlags.Twoway) != (LaneFlags) 0)
            {
              x1 |= (netLaneData1.m_Flags & LaneFlags.Track) != 0;
              x2 |= (netLaneData1.m_Flags & LaneFlags.Road) != 0;
            }
            else if (c != flag2)
            {
              netLaneData1.m_Flags |= LaneFlags.Invert;
              x1.y |= (netLaneData1.m_Flags & LaneFlags.Track) != 0;
              x2.y |= (netLaneData1.m_Flags & LaneFlags.Road) != 0;
            }
            else
            {
              x1.x |= (netLaneData1.m_Flags & LaneFlags.Track) != 0;
              x2.x |= (netLaneData1.m_Flags & LaneFlags.Road) != 0;
            }
            float3 float3_1 = piece.m_Offset + netPieceLane2.m_Position;
            tempLaneGroup.m_Flags = netLaneData1.m_Flags;
            if ((netLaneData1.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
            {
              float3 float3_2 = new float3(netLaneData1.m_Width * 0.5f, 0.0f, 0.0f);
              bounds3_2 |= new Bounds3(float3_1 - float3_2, float3_1 + float3_2);
              elem.m_Flags |= netLaneData1.m_Flags;
            }
            if ((netLaneData1.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
            {
              float3 float3_3 = new float3(netLaneData1.m_Width * 0.5f, 0.0f, 0.0f);
              bounds3_1 |= new Bounds3(float3_1 - float3_3, float3_1 + float3_3);
              elem.m_Flags |= netLaneData1.m_Flags;
            }
            if (num3 != 0)
            {
              NetCompositionHelpers.TempLaneGroup other = nativeList2[nativeList2.Length - 1];
              if (tempLaneGroup.IsCompatible(other))
              {
                tempLaneData.m_GroupIndex = nativeList2.Length - 1;
                nativeList1.Add(in tempLaneData);
                if ((other.m_Flags & (LaneFlags.DisconnectedStart | LaneFlags.DisconnectedEnd)) != (LaneFlags) 0)
                {
                  other.m_Prefab = netPieceLane2.m_Lane;
                  other.m_Flags = tempLaneGroup.m_Flags & (other.m_Flags | LaneFlags.Track);
                }
                else
                  other.m_Flags &= tempLaneGroup.m_Flags | LaneFlags.Track;
                other.m_Position += float3_1;
                ++other.m_LaneCount;
                nativeList2[nativeList2.Length - 1] = other;
                continue;
              }
            }
            tempLaneData.m_GroupIndex = num1++;
            nativeList1.Add(in tempLaneData);
            tempLaneGroup.m_Prefab = netPieceLane2.m_Lane;
            tempLaneGroup.m_Position = float3_1;
            tempLaneGroup.m_LaneCount = 1;
            tempLaneGroup.m_CarriagewayIndex = num2;
            nativeList2.Add(in tempLaneGroup);
            ++num3;
          }
        }
      }
      if (num3 != 0)
      {
        if (math.all(x2))
          flag1 = false;
        if (math.any(x1 | x2) && carriageways.IsCreated)
        {
          Bounds3 bounds = bounds3_1 | bounds3_2;
          bool2 x4 = x1 | x2;
          if (math.any(x1 != x2))
          {
            if (math.all(x2) & math.any(x1))
            {
              bounds = bounds3_1;
              x4 = x1;
            }
            else if (math.any(x2))
            {
              bounds = bounds3_2;
              x4 = x2;
            }
          }
          elem.m_Position = MathUtils.Center(bounds);
          elem.m_Width = MathUtils.Size(bounds).x;
          if (math.all(x4))
          {
            elem.m_Flags &= ~LaneFlags.Invert;
            elem.m_Flags |= LaneFlags.Twoway;
          }
          else if (x4.x)
          {
            elem.m_Flags &= ~(LaneFlags.Invert | LaneFlags.Twoway);
          }
          else
          {
            elem.m_Flags &= ~LaneFlags.Twoway;
            elem.m_Flags |= LaneFlags.Invert;
          }
          carriageways.Add(elem);
        }
      }
      if (flag1)
        compositionData.m_State |= CompositionState.SeparatedCarriageways;
      int index1 = 0;
      int num4 = -1;
      int num5 = 0;
      for (int index2 = 0; index2 < pieces.Length; ++index2)
      {
        NetCompositionPiece piece = pieces[index2];
        if (netPieceLanes.HasBuffer(piece.m_Piece))
        {
          DynamicBuffer<NetPieceLane> netPieceLane3 = netPieceLanes[piece.m_Piece];
          bool c = (piece.m_SectionFlags & NetSectionFlags.Invert) != 0;
          bool flag3 = (piece.m_SectionFlags & NetSectionFlags.FlipLanes) != 0;
          for (int a = 0; a < netPieceLane3.Length; ++a)
          {
            NetPieceLane netPieceLane4 = netPieceLane3[math.select(a, netPieceLane3.Length - a - 1, c)];
            NetLaneData netLaneData2 = netLaneData[netPieceLane4.m_Lane];
            NetCompositionHelpers.TempLaneData tempLaneData = nativeList1[index1];
            NetCompositionHelpers.TempLaneGroup tempLaneGroup = nativeList2[tempLaneData.m_GroupIndex];
            netLaneData2.m_Flags |= netPieceLane4.m_ExtraFlags;
            if (c)
              netPieceLane4.m_Position.x = -netPieceLane4.m_Position.x;
            if (c != flag3)
              netLaneData2.m_Flags |= LaneFlags.Invert;
            NetCompositionLane netCompositionLane = new NetCompositionLane();
            netCompositionLane.m_Lane = netPieceLane4.m_Lane;
            netCompositionLane.m_Position = piece.m_Offset + netPieceLane4.m_Position;
            netCompositionLane.m_Carriageway = (byte) tempLaneGroup.m_CarriagewayIndex;
            netCompositionLane.m_Group = (byte) tempLaneData.m_GroupIndex;
            netCompositionLane.m_Index = (byte) index1;
            netCompositionLane.m_Flags = netLaneData2.m_Flags;
            if (tempLaneGroup.m_LaneCount > 1)
              netCompositionLane.m_Flags |= LaneFlags.Slave;
            if (tempLaneData.m_GroupIndex != num4)
            {
              num4 = tempLaneData.m_GroupIndex;
              netCompositionLane.m_Flags |= c != flag3 ? LaneFlags.RightLimit : LaneFlags.LeftLimit;
              num5 = 0;
            }
            if (++num5 == tempLaneGroup.m_LaneCount)
              netCompositionLane.m_Flags |= c != flag3 ? LaneFlags.LeftLimit : LaneFlags.RightLimit;
            netLanes.Add(in netCompositionLane);
            ++index1;
          }
        }
      }
      int2 int2_1 = (int2) 0;
      int2 int2_2 = (int2) 0;
      for (int index3 = 0; index3 < netLanes.Length; ++index3)
      {
        NetCompositionLane netLane1 = netLanes[index3];
        if ((netLane1.m_Flags & LaneFlags.Parking) != (LaneFlags) 0)
        {
          int closestLane = NetCompositionHelpers.FindClosestLane(netLanes, index3, netLane1.m_Position, LaneFlags.Road);
          if (closestLane != -1)
          {
            NetCompositionLane netLane2 = netLanes[closestLane];
            if (((netLane1.m_Flags ^ netLane2.m_Flags) & LaneFlags.Invert) != (LaneFlags) 0)
              netLane1.m_Flags ^= LaneFlags.Invert;
            LaneFlags laneFlags = closestLane < index3 != ((netLane2.m_Flags & LaneFlags.Invert) != 0) ? LaneFlags.ParkingRight : LaneFlags.ParkingLeft;
            netLane1.m_Flags |= laneFlags;
            netLanes[index3] = netLane1;
            if ((netLane1.m_Flags & LaneFlags.Virtual) == (LaneFlags) 0)
            {
              netLane2.m_Flags |= laneFlags;
              netLanes[closestLane] = netLane2;
            }
          }
        }
        if ((netLane1.m_Flags & LaneFlags.Road) != (LaneFlags) 0)
        {
          if ((netLane1.m_Flags & LaneFlags.Invert) != (LaneFlags) 0)
          {
            compositionData.m_State |= CompositionState.HasBackwardRoadLanes;
            ++int2_2.x;
          }
          else
          {
            compositionData.m_State |= CompositionState.HasForwardRoadLanes;
            ++int2_1.x;
          }
        }
        if ((netLane1.m_Flags & LaneFlags.Track) != (LaneFlags) 0)
        {
          if ((netLane1.m_Flags & LaneFlags.Invert) != (LaneFlags) 0)
          {
            compositionData.m_State |= CompositionState.HasBackwardTrackLanes;
            ++int2_2.y;
          }
          else
          {
            compositionData.m_State |= CompositionState.HasForwardTrackLanes;
            ++int2_1.y;
          }
        }
        if ((netLane1.m_Flags & LaneFlags.Pedestrian) != (LaneFlags) 0)
          compositionData.m_State |= CompositionState.HasPedestrianLanes;
      }
      if (math.any(int2_1 != int2_2))
        compositionData.m_State |= CompositionState.Asymmetric;
      if (math.any(int2_1 > 1) | math.any(int2_2 > 1))
        compositionData.m_State |= CompositionState.Multilane;
      for (int index4 = 0; index4 < nativeList2.Length; ++index4)
      {
        NetCompositionHelpers.TempLaneGroup tempLaneGroup = nativeList2[index4];
        if (tempLaneGroup.m_LaneCount > 1)
        {
          netLanes.Add(new NetCompositionLane()
          {
            m_Lane = tempLaneGroup.m_Prefab,
            m_Position = tempLaneGroup.m_Position / (float) tempLaneGroup.m_LaneCount,
            m_Carriageway = (byte) tempLaneGroup.m_CarriagewayIndex,
            m_Group = (byte) index4,
            m_Index = (byte) index1,
            m_Flags = tempLaneGroup.m_Flags | LaneFlags.Master
          });
          ++index1;
        }
      }
      nativeList1.Dispose();
      nativeList2.Dispose();
      if (index1 >= 256)
        throw new Exception(string.Format("Too many lanes: {0}", (object) entity.Index));
    }

    private static int FindClosestLane(
      NativeList<NetCompositionLane> lanes,
      int startIndex,
      float3 position,
      LaneFlags flags)
    {
      int index1 = startIndex - 1;
      int index2 = startIndex + 1;
      while (true)
      {
        while (index1 < 0 || index2 >= lanes.Length)
        {
          if (index1 >= 0)
          {
            if ((lanes[index1].m_Flags & flags) != (LaneFlags) 0)
              return index1;
            --index1;
          }
          else
          {
            if (index2 >= lanes.Length)
              return -1;
            if ((lanes[index2].m_Flags & flags) != (LaneFlags) 0)
              return index2;
            ++index2;
          }
        }
        NetCompositionLane lane1 = lanes[index1];
        NetCompositionLane lane2 = lanes[index2];
        if ((double) math.lengthsq(lane1.m_Position - position) <= (double) math.lengthsq(lane2.m_Position - position))
        {
          if ((lane1.m_Flags & flags) == (LaneFlags) 0)
            --index1;
          else
            break;
        }
        else if ((lane2.m_Flags & flags) == (LaneFlags) 0)
          ++index2;
        else
          goto label_7;
      }
      return index1;
label_7:
      return index2;
    }

    public static void CalculateRoundaboutSize(
      ref NetCompositionData compositionData,
      NativeArray<NetCompositionPiece> pieces,
      ComponentLookup<NetLaneData> netLaneData,
      BufferLookup<NetPieceLane> netPieceLanes)
    {
      float2 float2 = (float2) 0.0f;
      float2 maxValue = (float2) float.MaxValue;
      float4 float4 = (float4) 0.0f;
      for (int index = 0; index < pieces.Length; ++index)
      {
        NetCompositionPiece piece = pieces[index];
        if (NetCompositionHelpers.HasRoad(piece.m_Piece, netLaneData, netPieceLanes))
        {
          float y = (piece.m_SectionFlags & NetSectionFlags.Invert) == (NetSectionFlags) 0 ? (float) (((double) compositionData.m_Width + (double) piece.m_Size.x) * 0.5) - piece.m_Offset.x : (float) (((double) compositionData.m_Width + (double) piece.m_Size.x) * 0.5) + piece.m_Offset.x;
          if ((piece.m_SectionFlags & NetSectionFlags.Invert) != 0 != ((piece.m_SectionFlags & NetSectionFlags.FlipLanes) != 0))
          {
            float2.x = math.max(float2.x, y);
            maxValue.y = math.min(maxValue.y, y);
            float4.x += piece.m_Size.x;
            float4.w = math.max(float4.w, piece.m_Size.x);
          }
          else
          {
            float2.y = math.max(float2.y, y);
            maxValue.x = math.min(maxValue.x, y);
            float4.y += piece.m_Size.x;
            float4.z = math.max(float4.z, piece.m_Size.x);
          }
        }
      }
      compositionData.m_RoundaboutSize = math.select(float2, math.max(maxValue, float2), maxValue < float.MaxValue);
      compositionData.m_RoundaboutSize = math.select(compositionData.m_RoundaboutSize, (float2) (compositionData.m_Width * 0.5f), compositionData.m_RoundaboutSize == 0.0f);
      compositionData.m_RoundaboutSize += math.max(float4.xy, float4.zw) / 3f;
    }

    private static bool HasRoad(
      Entity piece,
      ComponentLookup<NetLaneData> netLaneData,
      BufferLookup<NetPieceLane> netPieceLanes)
    {
      if (!netPieceLanes.HasBuffer(piece))
        return false;
      DynamicBuffer<NetPieceLane> netPieceLane1 = netPieceLanes[piece];
      for (int index = 0; index < netPieceLane1.Length; ++index)
      {
        NetPieceLane netPieceLane2 = netPieceLane1[index];
        if ((netLaneData[netPieceLane2.m_Lane].m_Flags & LaneFlags.Road) != (LaneFlags) 0)
          return true;
      }
      return false;
    }

    public static CompositionFlags GetElevationFlags(
      Elevation startElevation,
      Elevation middleElevation,
      Elevation endElevation,
      NetGeometryData prefabGeometryData)
    {
      CompositionFlags elevationFlags = new CompositionFlags();
      float2 float2_1 = math.max((float2) math.max(math.cmin(startElevation.m_Elevation), math.cmin(endElevation.m_Elevation)), middleElevation.m_Elevation);
      float3 x1 = new float3(math.cmin(startElevation.m_Elevation), math.cmin(endElevation.m_Elevation), math.cmin(middleElevation.m_Elevation));
      float3 x2 = new float3(math.cmax(startElevation.m_Elevation), math.cmax(endElevation.m_Elevation), math.cmax(middleElevation.m_Elevation));
      float2 float2_2 = math.min((float2) math.min(math.cmax(startElevation.m_Elevation), math.cmax(endElevation.m_Elevation)), middleElevation.m_Elevation);
      if (math.all(float2_1 >= prefabGeometryData.m_ElevationLimit * 2f) || (prefabGeometryData.m_Flags & GeometryFlags.RequireElevated) != (GeometryFlags) 0)
        elevationFlags.m_General |= CompositionFlags.General.Elevated;
      else if ((double) math.cmax(x1) <= (double) prefabGeometryData.m_ElevationLimit * -2.0 && (double) math.cmin(x2) <= (double) prefabGeometryData.m_ElevationLimit * -3.0)
      {
        elevationFlags.m_General |= CompositionFlags.General.Tunnel;
      }
      else
      {
        if ((double) float2_1.x >= (double) prefabGeometryData.m_ElevationLimit)
        {
          if ((prefabGeometryData.m_Flags & GeometryFlags.RaisedIsElevated) != (GeometryFlags) 0)
            elevationFlags.m_General |= CompositionFlags.General.Elevated;
          else
            elevationFlags.m_Left |= CompositionFlags.Side.Raised;
        }
        else if ((double) float2_2.x <= -(double) prefabGeometryData.m_ElevationLimit)
        {
          if ((prefabGeometryData.m_Flags & GeometryFlags.LoweredIsTunnel) != (GeometryFlags) 0)
            elevationFlags.m_General |= CompositionFlags.General.Tunnel;
          else
            elevationFlags.m_Left |= CompositionFlags.Side.Lowered;
        }
        if ((double) float2_1.y >= (double) prefabGeometryData.m_ElevationLimit)
        {
          if ((prefabGeometryData.m_Flags & GeometryFlags.RaisedIsElevated) != (GeometryFlags) 0)
            elevationFlags.m_General |= CompositionFlags.General.Elevated;
          else
            elevationFlags.m_Right |= CompositionFlags.Side.Raised;
        }
        else if ((double) float2_2.y <= -(double) prefabGeometryData.m_ElevationLimit)
        {
          if ((prefabGeometryData.m_Flags & GeometryFlags.LoweredIsTunnel) != (GeometryFlags) 0)
            elevationFlags.m_General |= CompositionFlags.General.Tunnel;
          else
            elevationFlags.m_Right |= CompositionFlags.Side.Lowered;
        }
      }
      return elevationFlags;
    }

    private struct TempLaneData
    {
      public int m_GroupIndex;
    }

    private struct TempLaneGroup
    {
      public Entity m_Prefab;
      public float3 m_Position;
      public LaneFlags m_Flags;
      public int m_LaneCount;
      public int m_CarriagewayIndex;

      public bool IsCompatible(NetCompositionHelpers.TempLaneGroup other)
      {
        LaneFlags laneFlags = LaneFlags.Invert | LaneFlags.Road;
        return (this.m_Flags & laneFlags) == (other.m_Flags & laneFlags) & (this.m_Flags & LaneFlags.Road) != 0;
      }
    }
  }
}
