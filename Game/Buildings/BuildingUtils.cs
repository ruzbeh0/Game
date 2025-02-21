// Decompiled with JetBrains decompiler
// Type: Game.Buildings.BuildingUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Citizens;
using Game.City;
using Game.Economy;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Vehicles;
using Game.Zones;
using System;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Buildings
{
  public static class BuildingUtils
  {
    public const float MAX_ROAD_CONNECTION_DISTANCE = 8.4f;
    public const float GEOMETRY_SIZE_OFFSET = 0.4f;
    public const float MIN_BUILDING_HEIGHT = 5f;
    public const float MIN_CONSTRUCTION_HEIGHT = 15f;
    public const float RANDOM_CONSTRUCTION_HEIGHT = 5f;
    public const float COLLAPSE_ACCELERATION = 5f;

    public static Quad3 CalculateCorners(Game.Objects.Transform transform, int2 lotSize)
    {
      return BuildingUtils.CalculateCorners(transform.m_Position, transform.m_Rotation, (float2) lotSize * 4f);
    }

    public static Quad3 CalculateCorners(float3 position, quaternion rotation, float2 halfLotSize)
    {
      float3 float3_1 = math.mul(rotation, new float3(0.0f, 0.0f, -1f));
      float3 float3_2 = math.mul(rotation, new float3(-1f, 0.0f, 0.0f));
      float3 float3_3 = float3_1 * halfLotSize.y;
      double x = (double) halfLotSize.x;
      float3 float3_4 = float3_2 * (float) x;
      float3 float3_5 = position + float3_3;
      float3 float3_6 = position - float3_3;
      return new Quad3(float3_5 - float3_4, float3_5 + float3_4, float3_6 + float3_4, float3_6 - float3_4);
    }

    public static float3 CalculateFrontPosition(Game.Objects.Transform transform, int lotDepth)
    {
      float3 position = new float3(0.0f, 0.0f, (float) lotDepth * 4f);
      return ObjectUtils.LocalToWorld(transform, position);
    }

    public static float GetEfficiency(BufferAccessor<Efficiency> bufferAccessor, int i)
    {
      return bufferAccessor.Length == 0 ? 1f : BuildingUtils.GetEfficiency(bufferAccessor[i]);
    }

    public static float GetImmediateEfficiency(BufferAccessor<Efficiency> bufferAccessor, int i)
    {
      return bufferAccessor.Length == 0 ? 1f : BuildingUtils.GetImmediateEfficiency(bufferAccessor[i]);
    }

    public static float GetEfficiency(Entity entity, ref BufferLookup<Efficiency> bufferLookup)
    {
      DynamicBuffer<Efficiency> bufferData;
      return !bufferLookup.TryGetBuffer(entity, out bufferData) ? 1f : BuildingUtils.GetEfficiency(bufferData);
    }

    public static float GetEfficiency(DynamicBuffer<Efficiency> buffer)
    {
      float num = 1f;
      foreach (Efficiency efficiency in buffer)
        num *= math.max(0.0f, efficiency.m_Efficiency);
      return (double) num <= 0.0 ? 0.0f : math.max(0.01f, math.round(100f * num) / 100f);
    }

    public static float GetImmediateEfficiency(DynamicBuffer<Efficiency> buffer)
    {
      float num = 1f;
      foreach (Efficiency efficiency in buffer)
      {
        switch (efficiency.m_Factor)
        {
          case EfficiencyFactor.Destroyed:
          case EfficiencyFactor.Abandoned:
          case EfficiencyFactor.Disabled:
          case EfficiencyFactor.ServiceBudget:
            num *= math.max(0.0f, efficiency.m_Efficiency);
            continue;
          default:
            continue;
        }
      }
      return (double) num <= 0.0 ? 0.0f : math.max(0.01f, math.round(100f * num) / 100f);
    }

    public static float GetEfficiency(Span<float> factors)
    {
      float num = 1f;
      Span<float> span = factors;
      for (int index = 0; index < span.Length; ++index)
      {
        float y = span[index];
        num *= math.max(0.0f, y);
      }
      return (double) num <= 0.0 ? 0.0f : math.max(0.01f, math.round(100f * num) / 100f);
    }

    public static void GetEfficiencyFactors(DynamicBuffer<Efficiency> buffer, Span<float> factors)
    {
      factors.Fill(1f);
      foreach (Efficiency efficiency in buffer)
        factors[(int) efficiency.m_Factor] = efficiency.m_Efficiency;
    }

    public static void SetEfficiencyFactors(DynamicBuffer<Efficiency> buffer, Span<float> factors)
    {
      buffer.Clear();
      for (int factor = 0; factor < factors.Length; ++factor)
      {
        if ((double) math.abs(factors[factor] - 1f) > 0.001)
          buffer.Add(new Efficiency((EfficiencyFactor) factor, factors[factor]));
      }
    }

    public static void SetEfficiencyFactor(
      DynamicBuffer<Efficiency> buffer,
      EfficiencyFactor factor,
      float efficiency)
    {
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index].m_Factor == factor)
        {
          if ((double) math.abs(efficiency - 1f) > 1.0 / 1000.0)
          {
            buffer[index] = new Efficiency(factor, efficiency);
            return;
          }
          buffer.RemoveAt(index);
          return;
        }
      }
      if ((double) math.abs(efficiency - 1f) <= 1.0 / 1000.0)
        return;
      buffer.Add(new Efficiency(factor, efficiency));
    }

    public static float2 ApproximateEfficiencyFactors(float targetEfficiency, float2 weights)
    {
      Assert.IsTrue((double) targetEfficiency >= 0.0 && (double) targetEfficiency <= 1.0);
      Assert.IsTrue((double) math.cmin(weights) >= 0.0);
      bool2 bool2 = weights > 1f / 1000f;
      if ((double) targetEfficiency == 1.0 || !math.all(bool2))
        return math.select((float2) 1f, (float2) targetEfficiency, bool2);
      if ((double) targetEfficiency == 0.0)
        return math.select((float2) 1f, (float2) 0.0f, bool2);
      double num = ((double) weights.x + (double) weights.y) / (2.0 * (double) weights.x * (double) weights.y);
      return 1f - ((float) num - math.sqrt((float) (num * num) - (float) ((1.0 - (double) targetEfficiency) / ((double) weights.x * (double) weights.y)))) * weights;
    }

    public static float4 ApproximateEfficiencyFactors(float targetEfficiency, float4 weights)
    {
      Assert.IsTrue((double) targetEfficiency >= 0.0 && (double) targetEfficiency <= 1.0);
      Assert.IsTrue((double) math.cmin(weights) >= 0.0);
      float num1 = math.cmax(weights);
      if ((double) targetEfficiency == 1.0 || (double) num1 == 0.0)
        return (float4) 1f;
      if ((double) targetEfficiency == 0.0)
        return math.select((float4) 1f, (float4) 0.0f, weights > 1.1920929E-07f);
      float a1 = -1f / num1;
      float a2 = 0.0f;
      float4 float4 = new float4();
      for (int index = 0; index < 16; ++index)
      {
        float b = (float) (((double) a1 + (double) a2) / 2.0);
        float4 = b * weights + 1f;
        float num2 = float4.x * float4.y * float4.z * float4.w;
        a1 = math.select(a1, b, (double) num2 < (double) targetEfficiency);
        a2 = math.select(a2, b, (double) num2 > (double) targetEfficiency);
      }
      return float4;
    }

    public static float GetEfficiency(byte rawValue) => (float) rawValue / 100f;

    public static int GetLevelingCost(
      AreaType areaType,
      BuildingPropertyData propertyData,
      int currentlevel,
      DynamicBuffer<CityModifier> cityEffects)
    {
      int num = propertyData.CountProperties();
      float f;
      switch (areaType)
      {
        case AreaType.Residential:
          f = currentlevel <= 4 ? (float) (num * Mathf.RoundToInt(math.pow(2f, (float) (2 * currentlevel)) * 40f)) : 1.07374182E+09f;
          break;
        case AreaType.Commercial:
        case AreaType.Industrial:
          f = currentlevel <= 4 ? (float) (num * Mathf.RoundToInt(math.pow(2f, (float) (2 * currentlevel)) * 160f)) : 1.07374182E+09f;
          if (propertyData.m_AllowedStored != Resource.NoResource)
          {
            f *= 4f;
            break;
          }
          break;
        default:
          f = 1.07374182E+09f;
          break;
      }
      CityUtils.ApplyModifier(ref f, cityEffects, CityModifierType.BuildingLevelingCost);
      return Mathf.RoundToInt(f);
    }

    public static AreaType GetAreaType(
      Entity buildPrefab,
      ref ComponentLookup<SpawnableBuildingData> spawnableBuildingDatas,
      ref ComponentLookup<ZoneData> zoneDatas)
    {
      return spawnableBuildingDatas.HasComponent(buildPrefab) && zoneDatas.HasComponent(spawnableBuildingDatas[buildPrefab].m_ZonePrefab) ? zoneDatas[spawnableBuildingDatas[buildPrefab].m_ZonePrefab].m_AreaType : AreaType.None;
    }

    public static bool CheckOption(Building building, BuildingOption option)
    {
      return (building.m_OptionMask & 1U << (int) (option & (BuildingOption) 31)) > 0U;
    }

    public static bool CheckOption(InstalledUpgrade installedUpgrade, BuildingOption option)
    {
      return (installedUpgrade.m_OptionMask & 1U << (int) (option & (BuildingOption) 31)) > 0U;
    }

    public static void ApplyModifier(
      ref float value,
      DynamicBuffer<BuildingModifier> modifiers,
      BuildingModifierType type)
    {
      if ((BuildingModifierType) modifiers.Length <= type)
        return;
      float2 delta = modifiers[(int) type].m_Delta;
      value += delta.x;
      value += value * delta.y;
    }

    public static bool HasOption(BuildingOptionData optionData, BuildingOption option)
    {
      return (optionData.m_OptionMask & 1U << (int) (option & (BuildingOption) 31)) > 0U;
    }

    public static int GetVehicleCapacity(float efficiency, int capacity)
    {
      return math.select(0, math.clamp(Mathf.RoundToInt(efficiency * (float) capacity), 1, capacity), (double) efficiency > 1.0 / 1000.0 & capacity > 0);
    }

    public static bool GetAddress(
      EntityManager entityManager,
      Entity entity,
      out Entity road,
      out int number)
    {
      Building component1;
      if (entityManager.TryGetComponent<Building>(entity, out component1))
        return BuildingUtils.GetAddress(entityManager, entity, component1.m_RoadEdge, component1.m_CurvePosition, out road, out number);
      Attached component2;
      if (entityManager.TryGetComponent<Attached>(entity, out component2))
        return BuildingUtils.GetAddress(entityManager, entity, component2.m_Parent, component2.m_CurvePosition, out road, out number);
      road = Entity.Null;
      number = 0;
      return false;
    }

    public static bool GetRandomOutsideConnectionByParameters(
      ref NativeList<Entity> outsideConnections,
      ref ComponentLookup<OutsideConnectionData> outsideConnectionDatas,
      ref ComponentLookup<PrefabRef> prefabRefs,
      Unity.Mathematics.Random random,
      float4 outsideConnectionSpawnParameters,
      out Entity result)
    {
      OutsideConnectionTransferType ocTransferType = OutsideConnectionTransferType.None;
      float num = random.NextFloat(1f);
      if ((double) num < (double) outsideConnectionSpawnParameters.x)
        ocTransferType = OutsideConnectionTransferType.Road;
      else if ((double) num < (double) outsideConnectionSpawnParameters.x + (double) outsideConnectionSpawnParameters.y)
        ocTransferType = OutsideConnectionTransferType.Train;
      else if ((double) num < (double) outsideConnectionSpawnParameters.x + (double) outsideConnectionSpawnParameters.y + (double) outsideConnectionSpawnParameters.z)
        ocTransferType = OutsideConnectionTransferType.Air;
      else if ((double) num < (double) outsideConnectionSpawnParameters.x + (double) outsideConnectionSpawnParameters.y + (double) outsideConnectionSpawnParameters.z + (double) outsideConnectionSpawnParameters.w)
        ocTransferType = OutsideConnectionTransferType.Ship;
      return BuildingUtils.GetRandomOutsideConnectionByTransferType(ref outsideConnections, ref outsideConnectionDatas, ref prefabRefs, random, ocTransferType, out result);
    }

    public static bool GetRandomOutsideConnectionByTransferType(
      ref NativeList<Entity> outsideConnections,
      ref ComponentLookup<OutsideConnectionData> outsideConnectionDatas,
      ref ComponentLookup<PrefabRef> prefabRefs,
      Unity.Mathematics.Random random,
      OutsideConnectionTransferType ocTransferType,
      out Entity result)
    {
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (ocTransferType != OutsideConnectionTransferType.None)
      {
        for (int index = 0; index < outsideConnections.Length; ++index)
        {
          Entity prefab = prefabRefs[outsideConnections[index]].m_Prefab;
          if (outsideConnectionDatas.HasComponent(prefab) && (ocTransferType & outsideConnectionDatas[prefab].m_Type) != OutsideConnectionTransferType.None)
            nativeList.Add(outsideConnections[index]);
        }
      }
      result = Entity.Null;
      if (nativeList.Length <= 0)
        return false;
      result = nativeList[random.NextInt(nativeList.Length)];
      return true;
    }

    public static OutsideConnectionTransferType GetOutsideConnectionType(
      Entity building,
      ref ComponentLookup<PrefabRef> prefabRefs,
      ref ComponentLookup<OutsideConnectionData> outsideConnectionDatas)
    {
      return outsideConnectionDatas.HasComponent(prefabRefs[building].m_Prefab) ? outsideConnectionDatas[prefabRefs[building].m_Prefab].m_Type : OutsideConnectionTransferType.None;
    }

    public static bool GetAddress(
      EntityManager entityManager,
      Entity entity,
      Entity edge,
      float curvePos,
      out Entity road,
      out int number)
    {
      Aggregated component1;
      DynamicBuffer<AggregateElement> buffer;
      if (entityManager.TryGetComponent<Aggregated>(edge, out component1) && entityManager.TryGetBuffer<AggregateElement>(component1.m_Aggregate, true, out buffer))
      {
        float x1 = 0.0f;
        for (int index = 0; index < buffer.Length; ++index)
        {
          AggregateElement aggregateElement = buffer[index];
          float y1 = x1;
          Curve component2;
          Composition component3;
          NetCompositionData component4;
          float3 float3;
          if (entityManager.TryGetComponent<Curve>(aggregateElement.m_Edge, out component2) && entityManager.TryGetComponent<Composition>(aggregateElement.m_Edge, out component3) && entityManager.TryGetComponent<NetCompositionData>(component3.m_Edge, out component4))
          {
            float3 = MathUtils.StartTangent(component2.m_Bezier);
            float2 x2 = math.normalizesafe(float3.xz);
            float3 = MathUtils.EndTangent(component2.m_Bezier);
            float2 float2 = math.normalizesafe(float3.xz);
            float cellWidth = (float) ZoneUtils.GetCellWidth(component4.m_Width);
            float2 y2 = float2;
            float num = math.acos(math.clamp(math.dot(x2, y2), -1f, 1f));
            y1 += component2.m_Length + (float) ((double) cellWidth * (double) num * 0.5);
          }
          bool flag1 = index == 0;
          bool flag2 = index == buffer.Length - 1;
          bool flag3 = aggregateElement.m_Edge == edge;
          bool c = false;
          if (flag3 | flag1 | flag2)
          {
            if (!flag1)
            {
              Game.Net.Edge component5;
              Game.Net.Edge component6;
              if (entityManager.TryGetComponent<Game.Net.Edge>(aggregateElement.m_Edge, out component5) && entityManager.TryGetComponent<Game.Net.Edge>(buffer[index - 1].m_Edge, out component6) && (component5.m_End == component6.m_Start || component5.m_End == component6.m_End))
                c = true;
            }
            else
            {
              Game.Net.Edge component7;
              Game.Net.Edge component8;
              if (!flag2 && entityManager.TryGetComponent<Game.Net.Edge>(aggregateElement.m_Edge, out component7) && entityManager.TryGetComponent<Game.Net.Edge>(buffer[index + 1].m_Edge, out component8) && (component7.m_Start == component8.m_Start || component7.m_Start == component8.m_End))
                c = true;
            }
            Game.Net.Edge component9;
            Roundabout component10;
            if (flag1 && entityManager.TryGetComponent<Game.Net.Edge>(aggregateElement.m_Edge, out component9) && entityManager.TryGetComponent<Roundabout>(c ? component9.m_End : component9.m_Start, out component10))
              x1 += component10.m_Radius;
            if (flag3)
            {
              Bounds1 t = new Bounds1(c ? curvePos : 0.0f, c ? 1f : curvePos);
              float s = math.saturate(MathUtils.Length(component2.m_Bezier, t) / math.max(1f, component2.m_Length));
              float num = math.lerp(x1, y1, s);
              bool flag4 = false;
              Game.Objects.Transform component11;
              if (entityManager.TryGetComponent<Game.Objects.Transform>(entity, out component11))
              {
                Game.Net.Edge component12;
                Roundabout component13;
                PrefabRef component14;
                Game.Prefabs.BuildingData component15;
                if ((double) s < 0.0099999997764825821 && entityManager.TryGetComponent<Game.Net.Edge>(aggregateElement.m_Edge, out component12) && entityManager.TryGetComponent<Roundabout>(component12.m_Start, out component13) && entityManager.TryGetComponent<PrefabRef>(entity, out component14) && entityManager.TryGetComponent<Game.Prefabs.BuildingData>(component14.m_Prefab, out component15))
                {
                  float3 frontPosition = BuildingUtils.CalculateFrontPosition(component11, component15.m_LotSize.y);
                  float3 = MathUtils.StartTangent(component2.m_Bezier);
                  float2 xz = float3.xz;
                  if (MathUtils.TryNormalize(ref xz))
                  {
                    float b = math.clamp(math.dot(xz, component2.m_Bezier.a.xz - frontPosition.xz), 0.0f, component13.m_Radius);
                    num += math.select(-b, b, c);
                  }
                }
                Game.Net.Edge component16;
                Roundabout component17;
                PrefabRef component18;
                Game.Prefabs.BuildingData component19;
                if ((double) s > 0.99000000953674316 && entityManager.TryGetComponent<Game.Net.Edge>(aggregateElement.m_Edge, out component16) && entityManager.TryGetComponent<Roundabout>(component16.m_End, out component17) && entityManager.TryGetComponent<PrefabRef>(entity, out component18) && entityManager.TryGetComponent<Game.Prefabs.BuildingData>(component18.m_Prefab, out component19))
                {
                  float3 frontPosition = BuildingUtils.CalculateFrontPosition(component11, component19.m_LotSize.y);
                  float3 = MathUtils.EndTangent(component2.m_Bezier);
                  float2 xz = float3.xz;
                  if (MathUtils.TryNormalize(ref xz))
                  {
                    float a = math.clamp(math.dot(xz, frontPosition.xz - component2.m_Bezier.d.xz), 0.0f, component17.m_Radius);
                    num += math.select(a, -a, c);
                  }
                }
                float2 xz1 = component11.m_Position.xz;
                float3 = MathUtils.Position(component2.m_Bezier, curvePos);
                float2 xz2 = float3.xz;
                float2 x3 = xz1 - xz2;
                float3 = MathUtils.Tangent(component2.m_Bezier, curvePos);
                float2 y3 = MathUtils.Right(float3.xz);
                flag4 = (double) math.dot(x3, y3) > 0.0 != c;
              }
              road = component1.m_Aggregate;
              number = Mathf.RoundToInt(num / 8f) * 2 + (flag4 ? 2 : 1);
              return true;
            }
          }
          x1 = y1;
        }
      }
      road = Entity.Null;
      number = 0;
      return false;
    }

    public static BuildingUtils.LotInfo CalculateLotInfo(
      float2 extents,
      Game.Objects.Transform transform,
      Game.Objects.Elevation elevation,
      Lot lot,
      PrefabRef prefabRef,
      DynamicBuffer<InstalledUpgrade> upgrades,
      ComponentLookup<Game.Objects.Transform> transforms,
      ComponentLookup<PrefabRef> prefabRefs,
      ComponentLookup<ObjectGeometryData> objectGeometryDatas,
      ComponentLookup<BuildingTerraformData> buildingTerraformDatas,
      ComponentLookup<BuildingExtensionData> buildingExtensionDatas,
      bool defaultNoSmooth,
      out bool hasExtensionLots)
    {
      BuildingUtils.LotInfo lotInfo = new BuildingUtils.LotInfo()
      {
        m_Position = transform.m_Position,
        m_Extents = extents,
        m_Rotation = transform.m_Rotation,
        m_Radius = math.length(extents),
        m_Circular = 0.0f,
        m_FrontHeights = lot.m_FrontHeights,
        m_RightHeights = lot.m_RightHeights,
        m_BackHeights = lot.m_BackHeights,
        m_LeftHeights = lot.m_LeftHeights,
        m_FlatX0 = (float3) -extents.x,
        m_FlatZ0 = (float3) -extents.y,
        m_FlatX1 = (float3) extents.x,
        m_FlatZ1 = (float3) extents.y,
        m_MinLimit = new float4(-extents.xy, extents.xy),
        m_MaxLimit = new float4(-extents.xy, extents.xy)
      };
      ObjectGeometryData componentData1;
      if (objectGeometryDatas.TryGetComponent(prefabRef.m_Prefab, out componentData1))
      {
        bool flag = (componentData1.m_Flags & Game.Objects.GeometryFlags.Standing) != 0;
        bool c = (componentData1.m_Flags & (flag ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular)) != 0;
        lotInfo.m_Circular = math.select(0.0f, 1f, c);
      }
      BuildingTerraformData componentData2;
      if (buildingTerraformDatas.TryGetComponent(prefabRef.m_Prefab, out componentData2))
      {
        lotInfo.m_Position.y += componentData2.m_HeightOffset;
        lotInfo.m_FlatX0 = componentData2.m_FlatX0;
        lotInfo.m_FlatZ0 = componentData2.m_FlatZ0;
        lotInfo.m_FlatX1 = componentData2.m_FlatX1;
        lotInfo.m_FlatZ1 = componentData2.m_FlatZ1;
        lotInfo.m_MinLimit = componentData2.m_Smooth;
        lotInfo.m_MaxLimit = componentData2.m_Smooth;
      }
      else
      {
        componentData2.m_DontLower = defaultNoSmooth;
        componentData2.m_DontRaise = defaultNoSmooth;
      }
      hasExtensionLots = false;
      if (upgrades.IsCreated)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          Entity upgrade = upgrades[index].m_Upgrade;
          PrefabRef prefabRef1 = prefabRefs[upgrade];
          BuildingExtensionData componentData3;
          BuildingTerraformData componentData4;
          if (buildingExtensionDatas.TryGetComponent(prefabRef1.m_Prefab, out componentData3) && !componentData3.m_External && buildingTerraformDatas.TryGetComponent(prefabRef1.m_Prefab, out componentData4))
          {
            float3 float3 = transforms[upgrade].m_Position - transform.m_Position;
            float num = 0.0f;
            ObjectGeometryData componentData5;
            if (objectGeometryDatas.TryGetComponent(prefabRef1.m_Prefab, out componentData5))
            {
              bool flag = (componentData5.m_Flags & Game.Objects.GeometryFlags.Standing) != 0;
              num = math.select(0.0f, 1f, (componentData5.m_Flags & (flag ? Game.Objects.GeometryFlags.CircularLeg : Game.Objects.GeometryFlags.Circular)) != 0);
            }
            lotInfo.m_FlatX0 = math.min(lotInfo.m_FlatX0, componentData4.m_FlatX0 + float3.x);
            lotInfo.m_FlatZ0 = math.min(lotInfo.m_FlatZ0, componentData4.m_FlatZ0 + float3.z);
            lotInfo.m_FlatX1 = math.max(lotInfo.m_FlatX1, componentData4.m_FlatX1 + float3.x);
            lotInfo.m_FlatZ1 = math.max(lotInfo.m_FlatZ1, componentData4.m_FlatZ1 + float3.z);
            if (!math.all(componentData4.m_Smooth + float3.xzxz == lotInfo.m_MaxLimit) || (double) num != (double) lotInfo.m_Circular)
              hasExtensionLots = true;
          }
        }
      }
      lotInfo.m_MinLimit.xy = math.min(new float2(lotInfo.m_FlatX0.y, lotInfo.m_FlatZ0.y), lotInfo.m_MinLimit.xy);
      lotInfo.m_MinLimit.zw = math.max(new float2(lotInfo.m_FlatX1.y, lotInfo.m_FlatZ1.y), lotInfo.m_MinLimit.zw);
      extents = math.max(extents, (float2) 8f);
      if (componentData2.m_DontLower)
        lotInfo.m_MinLimit = new float4(extents.xy, -extents.xy);
      if ((double) elevation.m_Elevation > 0.0 || componentData2.m_DontRaise)
        lotInfo.m_MaxLimit = new float4(extents.xy, -extents.xy);
      return lotInfo;
    }

    public static float SampleHeight(ref BuildingUtils.LotInfo lotInfo, float3 position)
    {
      position = math.mul(math.inverse(lotInfo.m_Rotation), position - lotInfo.m_Position);
      Bezier4x2 curve1 = new Bezier4x2(new float2(lotInfo.m_RightHeights.x, lotInfo.m_FrontHeights.x), new float2(lotInfo.m_RightHeights.y, lotInfo.m_LeftHeights.z), new float2(lotInfo.m_RightHeights.z, lotInfo.m_LeftHeights.y), new float2(lotInfo.m_BackHeights.x, lotInfo.m_LeftHeights.x));
      Bezier4x2 curve2 = new Bezier4x2(new float2(lotInfo.m_RightHeights.x, lotInfo.m_BackHeights.x), new float2(lotInfo.m_FrontHeights.z, lotInfo.m_BackHeights.y), new float2(lotInfo.m_FrontHeights.y, lotInfo.m_BackHeights.z), new float2(lotInfo.m_FrontHeights.x, lotInfo.m_LeftHeights.x));
      float2 float2_1 = math.clamp(position.xz, -lotInfo.m_Extents, lotInfo.m_Extents);
      float2 float2_2 = 0.5f / math.max((float2) 0.01f, lotInfo.m_Extents);
      float2 x1 = position.xz * float2_2 + 0.5f;
      float2 float2_3 = math.saturate(x1);
      float2 x2 = position.xz - float2_1;
      float2 float2_4 = x1 - math.sign(x2) * float2_2 * 2f;
      float2 float2_5 = (float2_4 - 0.5f) * (lotInfo.m_Extents * 2f);
      float2 float2_6 = 8f - 8f / (1f + math.abs(x2) * 0.125f);
      float4 a1 = new float4(lotInfo.m_FlatX0.xy, lotInfo.m_FlatZ0.yx);
      float4 a2 = new float4(lotInfo.m_FlatZ0.xy, lotInfo.m_FlatX0.yx);
      float4 a3 = new float4(lotInfo.m_FlatX1.xy, lotInfo.m_FlatZ0.yz);
      float4 a4 = new float4(lotInfo.m_FlatZ1.xy, lotInfo.m_FlatX0.yz);
      float4 a5 = math.select(a1, new float4(lotInfo.m_FlatX0.yy, lotInfo.m_FlatZ0.x, lotInfo.m_FlatZ1.x), (double) float2_1.y > (double) a1.w);
      float4 a6 = math.select(a2, new float4(lotInfo.m_FlatZ0.yy, lotInfo.m_FlatX0.x, lotInfo.m_FlatX1.x), (double) float2_1.x > (double) a2.w);
      float4 a7 = math.select(a3, new float4(lotInfo.m_FlatX1.yy, lotInfo.m_FlatZ0.z, lotInfo.m_FlatZ1.z), (double) float2_1.y > (double) a3.w);
      float4 a8 = math.select(a4, new float4(lotInfo.m_FlatZ1.yy, lotInfo.m_FlatX0.z, lotInfo.m_FlatX1.z), (double) float2_1.x > (double) a4.w);
      float4 float4_1 = math.select(a5, new float4(lotInfo.m_FlatX0.yz, lotInfo.m_FlatZ1.xy), (double) float2_1.y > (double) a5.w);
      float4 float4_2 = math.select(a6, new float4(lotInfo.m_FlatZ0.yz, lotInfo.m_FlatX1.xy), (double) float2_1.x > (double) a6.w);
      float4 float4_3 = math.select(a7, new float4(lotInfo.m_FlatX1.yz, lotInfo.m_FlatZ1.zy), (double) float2_1.y > (double) a7.w);
      float4 float4_4 = math.select(a8, new float4(lotInfo.m_FlatZ1.yz, lotInfo.m_FlatX1.zy), (double) float2_1.x > (double) a8.w);
      float4 x3 = new float4(float4_1.x, float4_2.x, float4_3.x, float4_4.x);
      float4 y = new float4(float4_1.y, float4_2.y, float4_3.y, float4_4.y);
      float4 float4_5 = new float4(float4_1.z, float4_2.z, float4_3.z, float4_4.z);
      float4 float4_6 = new float4(float4_1.w, float4_2.w, float4_3.w, float4_4.w);
      float4 s1 = (float2_1.yxyx - float4_5) / math.max(float4_6 - float4_5, (float4) 0.1f);
      float4 float4_7 = math.lerp(x3, y, s1);
      float4_7 = math.saturate(new float4(float4_7.xy - float2_1, float2_1 - float4_7.zw) / math.max(new float4(float4_7.xy + lotInfo.m_Extents, lotInfo.m_Extents - float4_7.zw), (float4) 0.1f));
      float4 s2 = (float2_5.yxyx - float4_5) / math.max(float4_6 - float4_5, (float4) 0.1f);
      float4 float4_8 = math.lerp(x3, y, s2);
      float4_8 = math.saturate(new float4(float4_8.xy - float2_5, float2_5 - float4_8.zw) / math.max(new float4(float4_8.xy + lotInfo.m_Extents, lotInfo.m_Extents - float4_8.zw), (float4) 0.1f));
      float4 float4_9 = new float4()
      {
        xz = MathUtils.Position(curve1, float2_3.y),
        yw = MathUtils.Position(curve2, float2_3.x)
      } * float4_7;
      float4_9.xy += float4_9.zw;
      float4 float4_10 = new float4()
      {
        xz = MathUtils.Position(curve1, float2_4.y),
        yw = MathUtils.Position(curve2, float2_4.x)
      } * float4_8;
      float4_10.xy += float4_10.zw;
      float4_9.xy += (float4_9.xy - float4_10.xy) * float2_6.xy * 0.5f;
      float4_7.xy = math.max(float4_7.xy, float4_7.zw);
      float4_7.xy /= math.max(1f, float4_7.x + float4_7.y);
      float4_7.x = math.select(float4_7.y, 1f - float4_7.x, (double) float4_7.x > (double) float4_7.y);
      float4_9.x = math.lerp(float4_9.x, float4_9.y, float4_7.x);
      position.y = float4_9.x;
      return lotInfo.m_Position.y + position.y;
    }

    public static float GetCollapseTime(float height) => math.sqrt(math.max(0.0f, height) * 0.4f);

    public static float GetCollapseHeight(float time) => 2.5f * math.lengthsq(time);

    public static MaintenanceType GetMaintenanceType(
      Entity entity,
      ref ComponentLookup<Park> parks,
      ref ComponentLookup<NetCondition> netConditions,
      ref ComponentLookup<Game.Net.Edge> edges,
      ref ComponentLookup<Surface> surfaces,
      ref ComponentLookup<Vehicle> vehicles)
    {
      if (parks.HasComponent(entity))
        return MaintenanceType.Park;
      if (netConditions.HasComponent(entity))
      {
        Surface componentData1;
        Game.Net.Edge componentData2;
        Surface componentData3;
        Surface componentData4;
        if (!surfaces.TryGetComponent(entity, out componentData1) && edges.TryGetComponent(entity, out componentData2) && surfaces.TryGetComponent(componentData2.m_Start, out componentData3) && surfaces.TryGetComponent(componentData2.m_End, out componentData4))
          componentData1.m_AccumulatedSnow = (byte) ((int) componentData3.m_AccumulatedSnow + (int) componentData4.m_AccumulatedSnow + 1 >> 1);
        return componentData1.m_AccumulatedSnow >= (byte) 15 ? MaintenanceType.Snow : MaintenanceType.Road;
      }
      return vehicles.HasComponent(entity) ? MaintenanceType.Vehicle : MaintenanceType.None;
    }

    public static void CalculateUpgradeRangeValues(
      quaternion rotation,
      Game.Prefabs.BuildingData ownerBuildingData,
      Game.Prefabs.BuildingData buildingData,
      ServiceUpgradeData serviceUpgradeData,
      out float3 forward,
      out float width,
      out float length,
      out float roundness,
      out bool circular)
    {
      forward = math.forward(rotation);
      if (ownerBuildingData.m_LotSize.y < ownerBuildingData.m_LotSize.x)
      {
        ownerBuildingData.m_LotSize = ownerBuildingData.m_LotSize.yx;
        forward.xz = MathUtils.Right(forward.xz);
      }
      float num = serviceUpgradeData.m_MaxPlacementDistance + (float) buildingData.m_LotSize.y * 8f;
      width = (float) ((double) ownerBuildingData.m_LotSize.x * 8.0 + (double) num * 2.0);
      length = (float) ((double) ownerBuildingData.m_LotSize.y * 8.0 + (double) num * 2.0);
      roundness = (float) ((double) math.max(0.0f, num - 40f) * 1.2000000476837158 + 8.0);
      width = math.min(length, math.max(width, roundness * 2f));
      roundness = math.min(roundness, width * 0.5f);
      circular = (double) length * 0.5 - (double) roundness < 1.0;
    }

    public static bool IsHomelessShelterBuilding(
      Entity propertyEntity,
      ref ComponentLookup<Park> parks,
      ref ComponentLookup<Abandoned> abandoneds)
    {
      return parks.HasComponent(propertyEntity) || abandoneds.HasComponent(propertyEntity);
    }

    public static bool IsHomelessHousehold(
      Household household,
      Entity propertyEntity,
      ref ComponentLookup<Park> parks,
      ref ComponentLookup<Abandoned> abandoneds)
    {
      if ((household.m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None)
        return false;
      return propertyEntity == Entity.Null || BuildingUtils.IsHomelessShelterBuilding(propertyEntity, ref parks, ref abandoneds);
    }

    public static bool IsHomelessHousehold(EntityManager entityManager, Entity householdEntity)
    {
      Household component1;
      if (!entityManager.TryGetComponent<Household>(householdEntity, out component1) || (component1.m_Flags & HouseholdFlags.MovedIn) == HouseholdFlags.None)
        return false;
      PropertyRenter component2;
      return !entityManager.TryGetComponent<PropertyRenter>(householdEntity, out component2) || component2.m_Property == Entity.Null || entityManager.HasComponent<Park>(component2.m_Property) || entityManager.HasComponent<Abandoned>(component2.m_Property);
    }

    public static Entity GetPropertyFromRenter(
      Entity renter,
      ref ComponentLookup<HomelessHousehold> homelessHouseholds,
      ref ComponentLookup<PropertyRenter> propertyRenters)
    {
      if (homelessHouseholds.HasComponent(renter))
        return homelessHouseholds[renter].m_TempHome;
      return propertyRenters.HasComponent(renter) ? propertyRenters[renter].m_Property : Entity.Null;
    }

    public struct LotInfo
    {
      public float3 m_Position;
      public float2 m_Extents;
      public float m_Radius;
      public float m_Circular;
      public quaternion m_Rotation;
      public float3 m_FrontHeights;
      public float3 m_RightHeights;
      public float3 m_BackHeights;
      public float3 m_LeftHeights;
      public float3 m_FlatX0;
      public float3 m_FlatZ0;
      public float3 m_FlatX1;
      public float3 m_FlatZ1;
      public float4 m_MinLimit;
      public float4 m_MaxLimit;
    }
  }
}
