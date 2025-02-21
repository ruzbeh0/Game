// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CityInfoUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CityInfoUISystem : UISystemBase, IDefaultSerializable, ISerializable
  {
    public const string kGroup = "cityInfo";
    private SimulationSystem m_SimulationSystem;
    private ResidentialDemandSystem m_ResidentialDemandSystem;
    private CommercialDemandSystem m_CommercialDemandSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private CitySystem m_CitySystem;
    private CitizenHappinessSystem m_CitizenHappinessSystem;
    private RawValueBinding m_ResidentialLowFactors;
    private RawValueBinding m_ResidentialMediumFactors;
    private RawValueBinding m_ResidentialHighFactors;
    private RawValueBinding m_CommercialFactors;
    private RawValueBinding m_IndustrialFactors;
    private RawValueBinding m_OfficeFactors;
    private RawValueBinding m_HappinessFactors;
    private float m_ResidentialLowDemand;
    private float m_ResidentialMediumDemand;
    private float m_ResidentialHighDemand;
    private float m_CommercialDemand;
    private float m_IndustrialDemand;
    private float m_OfficeDemand;
    private uint m_LastFrameIndex;
    private int m_AvgHappiness;
    private UIUpdateState m_UpdateState;
    private CityInfoUISystem.TypeHandle __TypeHandle;

    private float m_ResidentialLowDemandBindingValue
    {
      get => MathUtils.Snap(this.m_ResidentialLowDemand, 1f / 1000f);
    }

    private float m_ResidentialMediumDemandBindingValue
    {
      get => MathUtils.Snap(this.m_ResidentialMediumDemand, 1f / 1000f);
    }

    private float m_ResidentialHighDemandBindingValue
    {
      get => MathUtils.Snap(this.m_ResidentialHighDemand, 1f / 1000f);
    }

    private float m_CommercialDemandBindingValue
    {
      get => MathUtils.Snap(this.m_CommercialDemand, 1f / 1000f);
    }

    private float m_IndustrialDemandBindingValue
    {
      get => MathUtils.Snap(this.m_IndustrialDemand, 1f / 1000f);
    }

    private float m_OfficeDemandBindingValue => MathUtils.Snap(this.m_OfficeDemand, 1f / 1000f);

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialDemandSystem = this.World.GetOrCreateSystemManaged<ResidentialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialDemandSystem = this.World.GetOrCreateSystemManaged<CommercialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitizenHappinessSystem = this.World.GetOrCreateSystemManaged<CitizenHappinessSystem>();
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "residentialLowDemand", (Func<float>) (() => this.m_ResidentialLowDemandBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "residentialMediumDemand", (Func<float>) (() => this.m_ResidentialMediumDemandBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "residentialHighDemand", (Func<float>) (() => this.m_ResidentialHighDemandBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "commercialDemand", (Func<float>) (() => this.m_CommercialDemandBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "industrialDemand", (Func<float>) (() => this.m_IndustrialDemandBindingValue)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("cityInfo", "officeDemand", (Func<float>) (() => this.m_OfficeDemandBindingValue)));
      // ISSUE: reference to a compiler-generated field
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("cityInfo", "happiness", (Func<int>) (() => this.m_AvgHappiness)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResidentialLowFactors = new RawValueBinding("cityInfo", "residentialLowFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetLowDensityDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, densityDemandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResidentialMediumFactors = new RawValueBinding("cityInfo", "residentialMediumFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetMediumDensityDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, densityDemandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResidentialHighFactors = new RawValueBinding("cityInfo", "residentialHighFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetHighDensityDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, densityDemandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CommercialFactors = new RawValueBinding("cityInfo", "commercialFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> demandFactors = this.m_CommercialDemandSystem.GetDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, demandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_IndustrialFactors = new RawValueBinding("cityInfo", "industrialFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> industrialDemandFactors = this.m_IndustrialDemandSystem.GetIndustrialDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, industrialDemandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_OfficeFactors = new RawValueBinding("cityInfo", "officeFactors", (Action<IJsonWriter>) (writer =>
      {
        JobHandle deps;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> officeDemandFactors = this.m_IndustrialDemandSystem.GetOfficeDemandFactors(out deps);
        // ISSUE: reference to a compiler-generated method
        this.WriteDemandFactors(writer, officeDemandFactors, deps);
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_HappinessFactors = new RawValueBinding("cityInfo", "happinessFactors", (Action<IJsonWriter>) (writer =>
      {
        NativeList<FactorInfo> list = new NativeList<FactorInfo>(25, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        EntityQuery entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<HappinessFactorParameterData>());
        FactorInfo factorInfo;
        if (!entityQuery.IsEmptyIgnoreFilter)
        {
          DynamicBuffer<HappinessFactorParameterData> buffer = this.EntityManager.GetBuffer<HappinessFactorParameterData>(entityQuery.GetSingletonEntity(), true);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentLookup<Locked> roComponentLookup = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup;
          for (int factor = 0; factor < 25; ++factor)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int weight = Mathf.RoundToInt(this.m_CitizenHappinessSystem.GetHappinessFactor((CitizenHappinessSystem.HappinessFactor) factor, buffer, ref roComponentLookup).x);
            if (weight != 0)
            {
              ref NativeList<FactorInfo> local1 = ref list;
              factorInfo = new FactorInfo(factor, weight);
              ref FactorInfo local2 = ref factorInfo;
              local1.Add(in local2);
            }
          }
        }
        list.Sort<FactorInfo>();
        try
        {
          int size = math.min(10, list.Length);
          writer.ArrayBegin(size);
          for (int index = 0; index < size; ++index)
          {
            factorInfo = list[index];
            factorInfo.WriteHappinessFactor(writer);
          }
          writer.ArrayEnd();
        }
        finally
        {
          list.Dispose();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateState = UIUpdateState.Create(this.World, 256);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateState.ForceUpdate();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResidentialLowDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResidentialMediumDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_ResidentialHighDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_CommercialDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_IndustrialDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_OfficeDemand);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_LastFrameIndex);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_AvgHappiness);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Game.Version.residentialDemandSplitUI)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_ResidentialLowDemand);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_ResidentialMediumDemand);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_ResidentialHighDemand);
      }
      else
      {
        float num;
        reader.Read(out num);
        // ISSUE: reference to a compiler-generated field
        this.m_ResidentialLowDemand = num / 3f;
        // ISSUE: reference to a compiler-generated field
        this.m_ResidentialMediumDemand = num / 3f;
        // ISSUE: reference to a compiler-generated field
        this.m_ResidentialHighDemand = num / 3f;
      }
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_CommercialDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_IndustrialDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_OfficeDemand);
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_LastFrameIndex);
      if (!(reader.context.version >= Game.Version.populationComponent))
        return;
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_AvgHappiness);
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialLowDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialMediumDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialHighDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeDemand = 0.0f;
      // ISSUE: reference to a compiler-generated field
      this.m_LastFrameIndex = 0U;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint delta = this.m_SimulationSystem.frameIndex - this.m_LastFrameIndex;
      if (delta > 0U)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LastFrameIndex = this.m_SimulationSystem.frameIndex;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResidentialLowDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_ResidentialLowDemand, this.m_ResidentialDemandSystem.buildingDemand.x, delta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResidentialMediumDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_ResidentialMediumDemand, this.m_ResidentialDemandSystem.buildingDemand.y, delta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResidentialHighDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_ResidentialHighDemand, this.m_ResidentialDemandSystem.buildingDemand.z, delta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_CommercialDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_CommercialDemand, this.m_CommercialDemandSystem.buildingDemand, delta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IndustrialDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_IndustrialDemand, math.max(this.m_IndustrialDemandSystem.industrialBuildingDemand, this.m_IndustrialDemandSystem.storageBuildingDemand), delta);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_OfficeDemand = CityInfoUISystem.AdvanceSmoothDemand(this.m_OfficeDemand, this.m_IndustrialDemandSystem.officeBuildingDemand, delta);
        EntityManager entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        if (entityManager.HasComponent<Population>(this.m_CitySystem.City))
        {
          entityManager = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_AvgHappiness = entityManager.GetComponentData<Population>(this.m_CitySystem.City).m_AverageHappiness;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AvgHappiness = 50;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_UpdateState.Advance())
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialLowFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialMediumFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResidentialHighFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeFactors.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_HappinessFactors.Update();
    }

    private static float AdvanceSmoothDemand(float current, int target, uint delta)
    {
      return math.clamp((float) target / 100f, current - 0.000625f * (float) delta, current + 0.000125f * (float) delta);
    }

    public void RequestUpdate() => this.m_UpdateState.ForceUpdate();

    private void WriteResidentialLowFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetLowDensityDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, densityDemandFactors, deps);
    }

    private void WriteResidentialMediumFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetMediumDensityDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, densityDemandFactors, deps);
    }

    private void WriteResidentialHighFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> densityDemandFactors = this.m_ResidentialDemandSystem.GetHighDensityDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, densityDemandFactors, deps);
    }

    private void WriteCommercialFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> demandFactors = this.m_CommercialDemandSystem.GetDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, demandFactors, deps);
    }

    private void WriteIndustrialFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> industrialDemandFactors = this.m_IndustrialDemandSystem.GetIndustrialDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, industrialDemandFactors, deps);
    }

    private void WriteOfficeFactors(IJsonWriter writer)
    {
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> officeDemandFactors = this.m_IndustrialDemandSystem.GetOfficeDemandFactors(out deps);
      // ISSUE: reference to a compiler-generated method
      this.WriteDemandFactors(writer, officeDemandFactors, deps);
    }

    private void WriteDemandFactors(IJsonWriter writer, NativeArray<int> factors, JobHandle deps)
    {
      deps.Complete();
      NativeList<FactorInfo> list = FactorInfo.FromFactorArray(factors, Allocator.Temp);
      list.Sort<FactorInfo>();
      try
      {
        int size = math.min(5, list.Length);
        writer.ArrayBegin(size);
        for (int index = 0; index < size; ++index)
          list[index].WriteDemandFactor(writer);
        writer.ArrayEnd();
      }
      finally
      {
        list.Dispose();
      }
    }

    private void WriteHappinessFactors(IJsonWriter writer)
    {
      NativeList<FactorInfo> list = new NativeList<FactorInfo>(25, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      EntityQuery entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<HappinessFactorParameterData>());
      FactorInfo factorInfo;
      if (!entityQuery.IsEmptyIgnoreFilter)
      {
        DynamicBuffer<HappinessFactorParameterData> buffer = this.EntityManager.GetBuffer<HappinessFactorParameterData>(entityQuery.GetSingletonEntity(), true);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<Locked> roComponentLookup = this.__TypeHandle.__Game_Prefabs_Locked_RO_ComponentLookup;
        for (int factor = 0; factor < 25; ++factor)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          int weight = Mathf.RoundToInt(this.m_CitizenHappinessSystem.GetHappinessFactor((CitizenHappinessSystem.HappinessFactor) factor, buffer, ref roComponentLookup).x);
          if (weight != 0)
          {
            ref NativeList<FactorInfo> local1 = ref list;
            factorInfo = new FactorInfo(factor, weight);
            ref FactorInfo local2 = ref factorInfo;
            local1.Add(in local2);
          }
        }
      }
      list.Sort<FactorInfo>();
      try
      {
        int size = math.min(10, list.Length);
        writer.ArrayBegin(size);
        for (int index = 0; index < size; ++index)
        {
          factorInfo = list[index];
          factorInfo.WriteHappinessFactor(writer);
        }
        writer.ArrayEnd();
      }
      finally
      {
        list.Dispose();
      }
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

    [Preserve]
    public CityInfoUISystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Locked> __Game_Prefabs_Locked_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Locked_RO_ComponentLookup = state.GetComponentLookup<Locked>(true);
      }
    }
  }
}
