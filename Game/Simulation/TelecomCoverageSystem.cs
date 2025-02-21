// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TelecomCoverageSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TelecomCoverageSystem : CellMapSystem<TelecomCoverage>, IJobSerializable
  {
    public const int TEXTURE_SIZE = 128;
    private TerrainSystem m_TerrainSystem;
    private CitySystem m_CitySystem;
    private EntityQuery m_DensityQuery;
    private EntityQuery m_FacilityQuery;
    private NativeArray<TelecomStatus> m_Status;
    private TelecomCoverageSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4096;

    public int2 TextureSize => new int2(128, 128);

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DensityQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<HouseholdCitizen>(),
          ComponentType.ReadOnly<Employee>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_FacilityQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.TelecomFacility>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Game.Buildings.ServiceUpgrade>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_Status = new NativeArray<TelecomStatus>(0, Allocator.Persistent);
      this.CreateTextures(128);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Status.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_TerrainSystem.GetHeightData().isCreated)
        return;
      JobHandle outJobHandle1;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync1 = this.m_DensityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle1);
      JobHandle outJobHandle2;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync2 = this.m_FacilityQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle2);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new TelecomCoverageSystem.TelecomCoverageJob()
      {
        m_DensityChunks = archetypeChunkListAsync1,
        m_FacilityChunks = archetypeChunkListAsync2,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_City = this.m_CitySystem.City,
        m_Preview = false,
        m_TelecomCoverage = this.GetMap(false, out dependencies),
        m_TelecomStatus = this.m_Status,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_TelecomFacilityType = this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle,
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_HouseholdCitizenType = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_TelecomFacilityData = this.__TypeHandle.__Game_Buildings_TelecomFacility_RO_ComponentLookup,
        m_BuildingEfficiencyData = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabTelecomFacilityData = this.__TypeHandle.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RO_BufferLookup
      }.Schedule<TelecomCoverageSystem.TelecomCoverageJob>(JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(outJobHandle1, outJobHandle2, dependencies)));
      archetypeChunkListAsync1.Dispose(jobHandle);
      archetypeChunkListAsync2.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      this.AddWriter(jobHandle);
      this.Dependency = jobHandle;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public TelecomCoverageSystem()
    {
    }

    private struct CellDensityData
    {
      public ushort m_Density;
    }

    private struct CellFacilityData
    {
      public float m_SignalStrength;
      public float m_AccumulatedSignalStrength;
      public float m_NetworkCapacity;
    }

    [BurstCompile]
    public struct TelecomCoverageJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_DensityChunks;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_FacilityChunks;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public Entity m_City;
      [ReadOnly]
      public bool m_Preview;
      public NativeArray<TelecomCoverage> m_TelecomCoverage;
      public NativeArray<TelecomStatus> m_TelecomStatus;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> m_TelecomFacilityType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> m_HouseholdCitizenType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TelecomFacility> m_TelecomFacilityData;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<TelecomFacilityData> m_PrefabTelecomFacilityData;
      [ReadOnly]
      public BufferLookup<CityModifier> m_CityModifiers;

      public void Execute()
      {
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData = new NativeArray<TelecomCoverageSystem.CellDensityData>(16384, Allocator.Temp);
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData = new NativeArray<TelecomCoverageSystem.CellFacilityData>(16384, Allocator.Temp);
        NativeArray<float> obstructSlopes = new NativeArray<float>(16384, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        NativeList<float> signalStrengths = new NativeList<float>(16384, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<CityModifier> cityModifier = this.m_CityModifiers[this.m_City];
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_DensityChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddDensity(densityData, this.m_DensityChunks[index]);
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_FacilityChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CalculateSignalStrength(facilityData, obstructSlopes, signalStrengths, this.m_FacilityChunks[index], cityModifier);
        }
        int arrayIndex = 0;
        TelecomStatus status = new TelecomStatus();
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_FacilityChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.AddNetworkCapacity(densityData, facilityData, signalStrengths, this.m_FacilityChunks[index], ref arrayIndex, ref status, cityModifier);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TelecomCoverage.Length != 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.CalculateTelecomCoverage(facilityData);
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_TelecomStatus.Length != 0)
        {
          // ISSUE: reference to a compiler-generated method
          status.m_Quality = this.CalculateTelecomQuality(densityData, facilityData);
          // ISSUE: reference to a compiler-generated field
          this.m_TelecomStatus[0] = status;
        }
        densityData.Dispose();
        facilityData.Dispose();
        obstructSlopes.Dispose();
        signalStrengths.Dispose();
      }

      private void CalculateTelecomCoverage(
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData)
      {
        int num = 0;
        for (int index1 = 0; index1 < 128; ++index1)
        {
          for (int index2 = 0; index2 < 128; ++index2)
          {
            int index3 = num + index2;
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellFacilityData cellFacilityData = facilityData[index3];
            TelecomCoverage telecomCoverage;
            // ISSUE: reference to a compiler-generated field
            telecomCoverage.m_SignalStrength = (byte) math.clamp((int) ((double) cellFacilityData.m_SignalStrength * (double) byte.MaxValue), 0, (int) byte.MaxValue);
            // ISSUE: reference to a compiler-generated field
            telecomCoverage.m_NetworkLoad = (byte) math.clamp((int) (127.5 / (double) math.max(0.0001f, cellFacilityData.m_NetworkCapacity)), 0, (int) byte.MaxValue);
            // ISSUE: reference to a compiler-generated field
            this.m_TelecomCoverage[index3] = telecomCoverage;
          }
          num += 128;
        }
      }

      private float CalculateTelecomQuality(
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData,
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData)
      {
        float2 float2 = (float2) 0.0f;
        int num1 = 0;
        for (int index1 = 0; index1 < 128; ++index1)
        {
          for (int index2 = 0; index2 < 128; ++index2)
          {
            int index3 = num1 + index2;
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellDensityData cellDensityData = densityData[index3];
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellFacilityData cellFacilityData = facilityData[index3];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float num2 = math.min(1f, cellFacilityData.m_SignalStrength * 2f / (1f + 1f / math.max(0.0001f, cellFacilityData.m_NetworkCapacity)));
            // ISSUE: reference to a compiler-generated field
            float density = (float) cellDensityData.m_Density;
            float2 += new float2(num2 * density, density);
          }
          num1 += 128;
        }
        if ((double) float2.y != 0.0)
          float2.x /= float2.y;
        return float2.x;
      }

      private void AddNetworkCapacity(
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData,
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData,
        NativeList<float> signalStrengths,
        ArchetypeChunk chunk,
        ref int arrayIndex,
        ref TelecomStatus status,
        DynamicBuffer<CityModifier> cityModifiers)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray1 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TelecomFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TelecomFacility>(ref this.m_TelecomFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Transform transform = nativeArray1[index];
          // ISSUE: reference to a compiler-generated field
          TelecomFacilityData data = this.m_PrefabTelecomFacilityData[nativeArray3[index].m_Prefab];
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<TelecomFacilityData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabTelecomFacilityData);
          }
          // ISSUE: reference to a compiler-generated method
          float efficiencyFactor = this.GetEfficiencyFactor(nativeArray2, nativeArray4, bufferAccessor1, index);
          CityUtils.ApplyModifier(ref data.m_NetworkCapacity, cityModifiers, CityModifierType.TelecomCapacity);
          data.m_Range *= math.sqrt(efficiencyFactor);
          data.m_NetworkCapacity *= efficiencyFactor;
          if ((double) data.m_Range >= 1.0 && (double) data.m_NetworkCapacity >= 1.0)
          {
            int2 min = math.max(CellMapSystem<TelecomCoverage>.GetCell(transform.m_Position - data.m_Range, CellMapSystem<TelecomCoverage>.kMapSize, 128), (int2) 0);
            int2 max = math.min(CellMapSystem<TelecomCoverage>.GetCell(transform.m_Position + data.m_Range, CellMapSystem<TelecomCoverage>.kMapSize, 128) + 1, (int2) 128);
            int2 int2 = max - min;
            if (!math.any(int2 <= 0))
            {
              NativeArray<float> subArray = signalStrengths.AsArray().GetSubArray(arrayIndex, int2.x * int2.y);
              arrayIndex += int2.x * int2.y;
              // ISSUE: reference to a compiler-generated method
              float networkUsers = this.CalculateNetworkUsers(densityData, facilityData, subArray, min, max);
              float capacity = data.m_NetworkCapacity / math.max(1f, networkUsers);
              // ISSUE: reference to a compiler-generated method
              this.AddNetworkCapacity(facilityData, subArray, min, max, capacity);
              status.m_Capacity += data.m_NetworkCapacity;
              status.m_Load += networkUsers;
            }
          }
        }
      }

      private void AddNetworkCapacity(
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData,
        NativeArray<float> signalStrengthArray,
        int2 min,
        int2 max,
        float capacity)
      {
        int2 int2 = max - min;
        int num1 = 128 * min.y;
        int num2 = -min.x;
        for (int y = min.y; y < max.y; ++y)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            float signalStrength = signalStrengthArray[num2 + x];
            int index = num1 + x;
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellFacilityData cellFacilityData = facilityData[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cellFacilityData.m_NetworkCapacity = math.select(cellFacilityData.m_NetworkCapacity, cellFacilityData.m_NetworkCapacity + capacity * (signalStrength / cellFacilityData.m_AccumulatedSignalStrength), (double) cellFacilityData.m_AccumulatedSignalStrength > 9.9999997473787516E-05);
            facilityData[index] = cellFacilityData;
          }
          num1 += 128;
          num2 += int2.x;
        }
      }

      private float CalculateNetworkUsers(
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData,
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData,
        NativeArray<float> signalStrengthArray,
        int2 min,
        int2 max)
      {
        float networkUsers = 0.0f;
        int2 int2 = max - min;
        int num1 = 128 * min.y;
        int num2 = -min.x;
        for (int y = min.y; y < max.y; ++y)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            float signalStrength = signalStrengthArray[num2 + x];
            int index = num1 + x;
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellDensityData cellDensityData = densityData[index];
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellFacilityData cellFacilityData = facilityData[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            networkUsers += math.select(0.0f, (float) cellDensityData.m_Density * (signalStrength / cellFacilityData.m_AccumulatedSignalStrength), (double) cellFacilityData.m_AccumulatedSignalStrength > 9.9999997473787516E-05);
          }
          num1 += 128;
          num2 += int2.x;
        }
        return networkUsers;
      }

      private void CalculateSignalStrength(
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData,
        NativeArray<float> obstructSlopes,
        NativeList<float> signalStrengths,
        ArchetypeChunk chunk,
        DynamicBuffer<CityModifier> cityModifiers)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray1 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TelecomFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.TelecomFacility>(ref this.m_TelecomFacilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray4 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Transform transform = nativeArray1[index];
          PrefabRef prefabRef = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_ObjectGeometryData[prefabRef.m_Prefab];
          // ISSUE: reference to a compiler-generated field
          TelecomFacilityData data = this.m_PrefabTelecomFacilityData[prefabRef.m_Prefab];
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<TelecomFacilityData>(ref data, bufferAccessor2[index], ref this.m_PrefabRefData, ref this.m_PrefabTelecomFacilityData);
          }
          // ISSUE: reference to a compiler-generated method
          float efficiencyFactor = this.GetEfficiencyFactor(nativeArray2, nativeArray4, bufferAccessor1, index);
          CityUtils.ApplyModifier(ref data.m_NetworkCapacity, cityModifiers, CityModifierType.TelecomCapacity);
          data.m_Range *= math.sqrt(efficiencyFactor);
          data.m_NetworkCapacity *= efficiencyFactor;
          if ((double) data.m_Range >= 1.0 && (double) data.m_NetworkCapacity >= 1.0)
          {
            float3 position = transform.m_Position;
            position.y += objectGeometryData.m_Size.y;
            int2 min = math.max(CellMapSystem<TelecomCoverage>.GetCell(position - data.m_Range, CellMapSystem<TelecomCoverage>.kMapSize, 128), (int2) 0);
            int2 max = math.min(CellMapSystem<TelecomCoverage>.GetCell(position + data.m_Range, CellMapSystem<TelecomCoverage>.kMapSize, 128) + 1, (int2) 128);
            int2 int2_1 = max - min;
            if (!math.any(int2_1 <= 0))
            {
              int length = signalStrengths.Length;
              signalStrengths.Resize(length + int2_1.x * int2_1.y, NativeArrayOptions.UninitializedMemory);
              NativeArray<float> subArray = signalStrengths.AsArray().GetSubArray(length, int2_1.x * int2_1.y);
              if (data.m_PenetrateTerrain)
              {
                // ISSUE: reference to a compiler-generated method
                this.CalculateSignalStrength(subArray, min, max, data.m_Range, position);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                this.ResetObstructAngles(obstructSlopes, min, max);
                int2 cell = math.clamp(CellMapSystem<TelecomCoverage>.GetCell(position, CellMapSystem<TelecomCoverage>.kMapSize, 128), (int2) 0, (int2) (int) sbyte.MaxValue);
                // ISSUE: reference to a compiler-generated method
                this.CalculateCellSignalStrength(obstructSlopes, subArray, cell, min, max, data.m_Range, position);
                int2 int2_2 = cell;
                int2 int2_3 = cell + 1;
                while (math.any(int2_2 > min | int2_3 < max))
                {
                  if (int2_2.y > min.y)
                  {
                    --int2_2.y;
                    for (int x = cell.x; x < int2_3.x; ++x)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(x, int2_2.y), min, max, data.m_Range, position);
                    }
                    for (int x = cell.x - 1; x >= int2_2.x; --x)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(x, int2_2.y), min, max, data.m_Range, position);
                    }
                  }
                  if (int2_3.y < max.y)
                  {
                    for (int x = cell.x; x < int2_3.x; ++x)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(x, int2_3.y), min, max, data.m_Range, position);
                    }
                    for (int x = cell.x - 1; x >= int2_2.x; --x)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(x, int2_3.y), min, max, data.m_Range, position);
                    }
                    ++int2_3.y;
                  }
                  if (int2_2.x > min.x)
                  {
                    --int2_2.x;
                    for (int y = cell.y; y < int2_3.y; ++y)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(int2_2.x, y), min, max, data.m_Range, position);
                    }
                    for (int y = cell.y - 1; y >= int2_2.y; --y)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(int2_2.x, y), min, max, data.m_Range, position);
                    }
                  }
                  if (int2_3.x < max.x)
                  {
                    for (int y = cell.y; y < int2_3.y; ++y)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(int2_3.x, y), min, max, data.m_Range, position);
                    }
                    for (int y = cell.y - 1; y >= int2_2.y; --y)
                    {
                      // ISSUE: reference to a compiler-generated method
                      this.CalculateCellSignalStrength(obstructSlopes, subArray, new int2(int2_3.x, y), min, max, data.m_Range, position);
                    }
                    ++int2_3.x;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.AddSignalStrengths(facilityData, subArray, min, max);
            }
          }
        }
      }

      private float GetEfficiencyFactor(
        NativeArray<Game.Buildings.TelecomFacility> telecomFacilities,
        NativeArray<Temp> temps,
        BufferAccessor<Efficiency> efficiencyAccessor,
        int i)
      {
        float efficiencyFactor = 1f;
        if (temps.Length != 0)
        {
          Temp temp = temps[i];
          DynamicBuffer<Efficiency> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingEfficiencyData.TryGetBuffer(temp.m_Original, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            Game.Buildings.TelecomFacility telecomFacility = this.m_TelecomFacilityData[temp.m_Original];
            // ISSUE: reference to a compiler-generated field
            if (!this.m_Preview || (telecomFacility.m_Flags & TelecomFacilityFlags.HasCoverage) != (TelecomFacilityFlags) 0)
              efficiencyFactor = BuildingUtils.GetEfficiency(bufferData);
          }
        }
        else if (efficiencyAccessor.Length != 0)
        {
          Game.Buildings.TelecomFacility telecomFacility = telecomFacilities[i];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Preview || (telecomFacility.m_Flags & TelecomFacilityFlags.HasCoverage) != (TelecomFacilityFlags) 0)
            efficiencyFactor = BuildingUtils.GetEfficiency(efficiencyAccessor[i]);
        }
        return efficiencyFactor;
      }

      private void AddSignalStrengths(
        NativeArray<TelecomCoverageSystem.CellFacilityData> facilityData,
        NativeArray<float> signalStrengthArray,
        int2 min,
        int2 max)
      {
        int2 int2 = max - min;
        int num1 = 128 * min.y;
        int num2 = -min.x;
        for (int y = min.y; y < max.y; ++y)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            float signalStrength = signalStrengthArray[num2 + x];
            int index = num1 + x;
            // ISSUE: variable of a compiler-generated type
            TelecomCoverageSystem.CellFacilityData cellFacilityData = facilityData[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cellFacilityData.m_SignalStrength = (float) (1.0 - (1.0 - (double) cellFacilityData.m_SignalStrength) * (1.0 - (double) signalStrength));
            // ISSUE: reference to a compiler-generated field
            cellFacilityData.m_AccumulatedSignalStrength += signalStrength;
            facilityData[index] = cellFacilityData;
          }
          num1 += 128;
          num2 += int2.x;
        }
      }

      private void CalculateSignalStrength(
        NativeArray<float> signalStrengthArray,
        int2 min,
        int2 max,
        float range,
        float3 position)
      {
        int2 int2 = max - min;
        int num = -min.x;
        for (int y = min.y; y < max.y; ++y)
        {
          for (int x = min.x; x < max.x; ++x)
          {
            float3 cellCenter = CellMapSystem<TelecomCoverage>.GetCellCenter(new int2(x, y), 128);
            float distance = math.length((position - cellCenter).xz);
            // ISSUE: reference to a compiler-generated method
            signalStrengthArray[num + x] = math.max(0.0f, this.CalculateSignalStrength(distance, range));
          }
          num += int2.x;
        }
      }

      private void ResetObstructAngles(NativeArray<float> obstructAngles, int2 min, int2 max)
      {
        int2 int2 = max - min;
        int num = int2.x * int2.y;
        for (int index = 0; index < num; ++index)
          obstructAngles[index] = float.MaxValue;
      }

      private float CalculateSignalStrength(float distance, float range)
      {
        float num = distance / range;
        return 1f - num * num;
      }

      private void CalculateCellSignalStrength(
        NativeArray<float> obstructSlopes,
        NativeArray<float> signalStrengthArray,
        int2 cell,
        int2 min,
        int2 max,
        float range,
        float3 position)
      {
        int2 int2_1 = cell - min;
        int2 int2_2 = max - min;
        int index = int2_1.x + int2_2.x * int2_1.y;
        float3 cellCenter = CellMapSystem<TelecomCoverage>.GetCellCenter(cell, 128);
        float3 float3 = position - cellCenter;
        float num1 = math.length(float3.xz);
        // ISSUE: reference to a compiler-generated method
        float signalStrength = this.CalculateSignalStrength(num1, range);
        if ((double) signalStrength <= 0.0)
        {
          signalStrengthArray[index] = 0.0f;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          cellCenter.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cellCenter);
          float3.y = position.y - cellCenter.y;
          float y = float3.y / math.max(1f, num1);
          float num2 = (float) CellMapSystem<TelecomCoverage>.kMapSize / 128f;
          float2 float2_1 = math.abs(float3.xz);
          int2 int2_3 = math.clamp(int2_1 + math.select((int2) math.sign(float3.xz), (int2) 0, math.all(float2_1 < num2)), (int2) 0, int2_2 - 1);
          int2 int2_4;
          float s;
          if ((double) float2_1.x >= (double) float2_1.y)
          {
            int2_4 = int2_3.x + int2_2.x * new int2(int2_1.y, int2_3.y);
            s = float2_1.y / math.max(1f, float2_1.x);
          }
          else
          {
            int2_4 = new int2(int2_1.x, int2_3.x) + int2_2.x * int2_3.y;
            s = float2_1.x / math.max(1f, float2_1.y);
          }
          float2 float2_2 = new float2(obstructSlopes[int2_4.x], obstructSlopes[int2_4.y]);
          float2 float2_3 = math.saturate((float2_2 - y) * 20f + 1f);
          obstructSlopes[index] = math.min(math.lerp(float2_2.x, float2_2.y, s), y);
          signalStrengthArray[index] = signalStrength * math.lerp(float2_3.x, float2_3.y, s);
        }
      }

      private void AddDensity(
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData,
        ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray1 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<HouseholdCitizen> bufferAccessor1 = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Employee> bufferAccessor2 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
          for (int index = 0; index < bufferAccessor1.Length; ++index)
          {
            PropertyRenter propertyRenter = nativeArray1[index];
            DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor1[index];
            // ISSUE: reference to a compiler-generated field
            if (dynamicBuffer.Length != 0 && this.m_TransformData.HasComponent(propertyRenter.m_Property))
            {
              // ISSUE: reference to a compiler-generated field
              Transform transform = this.m_TransformData[propertyRenter.m_Property];
              // ISSUE: reference to a compiler-generated method
              this.AddDensity(densityData, dynamicBuffer.Length, transform.m_Position);
            }
          }
          for (int index = 0; index < bufferAccessor2.Length; ++index)
          {
            PropertyRenter propertyRenter = nativeArray1[index];
            DynamicBuffer<Employee> dynamicBuffer = bufferAccessor2[index];
            // ISSUE: reference to a compiler-generated field
            if (dynamicBuffer.Length != 0 && this.m_TransformData.HasComponent(propertyRenter.m_Property))
            {
              // ISSUE: reference to a compiler-generated field
              Transform transform = this.m_TransformData[propertyRenter.m_Property];
              // ISSUE: reference to a compiler-generated method
              this.AddDensity(densityData, dynamicBuffer.Length, transform.m_Position);
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
          if (nativeArray2.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<HouseholdCitizen> bufferAccessor3 = chunk.GetBufferAccessor<HouseholdCitizen>(ref this.m_HouseholdCitizenType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Employee> bufferAccessor4 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
          for (int index = 0; index < bufferAccessor3.Length; ++index)
          {
            Transform transform = nativeArray2[index];
            DynamicBuffer<HouseholdCitizen> dynamicBuffer = bufferAccessor3[index];
            if (dynamicBuffer.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddDensity(densityData, dynamicBuffer.Length, transform.m_Position);
            }
          }
          for (int index = 0; index < bufferAccessor4.Length; ++index)
          {
            Transform transform = nativeArray2[index];
            DynamicBuffer<Employee> dynamicBuffer = bufferAccessor4[index];
            if (dynamicBuffer.Length != 0)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddDensity(densityData, dynamicBuffer.Length, transform.m_Position);
            }
          }
        }
      }

      private void AddDensity(
        NativeArray<TelecomCoverageSystem.CellDensityData> densityData,
        int density,
        float3 position)
      {
        int2 int2 = math.clamp(CellMapSystem<TelecomCoverage>.GetCell(position, CellMapSystem<TelecomCoverage>.kMapSize, 128), (int2) 0, (int2) (int) sbyte.MaxValue);
        int index = int2.x + 128 * int2.y;
        // ISSUE: variable of a compiler-generated type
        TelecomCoverageSystem.CellDensityData cellDensityData = densityData[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cellDensityData.m_Density = (ushort) math.min((int) ushort.MaxValue, (int) cellDensityData.m_Density + density);
        densityData[index] = cellDensityData;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> __Game_Buildings_TelecomFacility_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.TelecomFacility> __Game_Buildings_TelecomFacility_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TelecomFacilityData> __Game_Prefabs_TelecomFacilityData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<CityModifier> __Game_City_CityModifier_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TelecomFacility_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TelecomFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferTypeHandle = state.GetBufferTypeHandle<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TelecomFacility_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.TelecomFacility>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TelecomFacilityData_RO_ComponentLookup = state.GetComponentLookup<TelecomFacilityData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RO_BufferLookup = state.GetBufferLookup<CityModifier>(true);
      }
    }
  }
}
