// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.City;
using Game.Companies;
using Game.Economy;
using Game.Policies;
using Game.Prefabs;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CitySystem : 
    GameSystemBase,
    ICitySystem,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    private EntityQuery m_ServiceFeeParameterQuery;
    private EntityQuery m_EconomyParameterQuery;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private Entity m_City;
    private int m_Money;
    private int m_XP;

    public Entity City => this.m_City;

    public int moneyAmount => this.m_Money;

    public int XP => this.m_XP;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceFeeParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceFeeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!(this.m_City != Entity.Null))
        return;
      EntityManager entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Money = entityManager.GetComponentData<PlayerMoney>(this.m_City).money;
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_XP = entityManager.GetComponentData<Game.City.XP>(this.m_City).m_XP;
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_City);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      reader.Read(out this.m_City);
      // ISSUE: reference to a compiler-generated field
      this.m_Money = 0;
    }

    public void SetDefaults(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_City = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_Money = 0;
    }

    public void PostDeserialize(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      EconomyParameterData singleton1 = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>();
      EntityManager entityManager1;
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_City == Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_City = this.EntityManager.CreateEntity((ComponentType) typeof (Game.City.City));
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<MilestoneLevel>(this.m_City, new MilestoneLevel()
          {
            m_AchievedMilestone = 0
          });
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<Game.City.XP>(this.m_City, new Game.City.XP()
          {
            m_XP = 0
          });
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<DevTreePoints>(this.m_City, new DevTreePoints()
          {
            m_Points = 0
          });
          EntityManager entityManager2 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager2.AddBuffer<Policy>(this.m_City);
          entityManager2 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager2.AddBuffer<CityModifier>(this.m_City);
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<Loan>(this.m_City, new Loan());
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<Creditworthiness>(this.m_City, new Creditworthiness());
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddComponentData<DangerLevel>(this.m_City, new DangerLevel());
          // ISSUE: reference to a compiler-generated field
          this.EntityManager.AddBuffer<TradeCost>(this.m_City);
          // ISSUE: reference to a compiler-generated field
          ServiceFeeParameterData singleton2 = this.m_ServiceFeeParameterQuery.GetSingleton<ServiceFeeParameterData>();
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<ServiceFee> dynamicBuffer = this.EntityManager.AddBuffer<ServiceFee>(this.m_City);
          foreach (ServiceFee defaultFee in singleton2.GetDefaultFees())
            dynamicBuffer.Add(defaultFee);
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.AddComponentData<PlayerMoney>(this.m_City, new PlayerMoney(singleton1.m_PlayerStartMoney));
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.AddBuffer<SpecializationBonus>(this.m_City);
          Population componentData1 = new Population();
          componentData1.SetDefaults(context);
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.AddComponentData<Population>(this.m_City, componentData1);
          Tourism componentData2 = new Tourism();
          componentData2.SetDefaults(context);
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.AddComponentData<Tourism>(this.m_City, componentData2);
        }
        else
        {
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.SetComponentData<PlayerMoney>(this.m_City, new PlayerMoney(singleton1.m_PlayerStartMoney));
        }
      }
      if (context.purpose == Colossal.Serialization.Entities.Purpose.LoadGame && context.version < Version.loanComponent)
      {
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager1.AddComponentData<Loan>(this.m_City, new Loan());
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager1.AddComponentData<Creditworthiness>(this.m_City, new Creditworthiness());
      }
      if (context.purpose == Colossal.Serialization.Entities.Purpose.NewGame || context.purpose == Colossal.Serialization.Entities.Purpose.LoadGame)
      {
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PlayerMoney componentData = entityManager1.GetComponentData<PlayerMoney>(this.m_City) with
        {
          m_Unlimited = this.m_CityConfigurationSystem.unlimitedMoney
        };
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager1.SetComponentData<PlayerMoney>(this.m_City, componentData);
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        if (entityManager1.HasComponent<Resources>(this.m_City))
        {
          entityManager1 = this.EntityManager;
          // ISSUE: reference to a compiler-generated field
          entityManager1.RemoveComponent<Resources>(this.m_City);
        }
      }
      if (context.purpose == Colossal.Serialization.Entities.Purpose.LoadGame && context.version < Version.dangerLevel)
      {
        entityManager1 = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager1.AddComponentData<DangerLevel>(this.m_City, new DangerLevel());
      }
      if (!(context.version < Version.cityTradeCost) || context.purpose != Colossal.Serialization.Entities.Purpose.NewGame && context.purpose != Colossal.Serialization.Entities.Purpose.LoadGame)
        return;
      entityManager1 = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<TradeCost> costs = entityManager1.AddBuffer<TradeCost>(this.m_City);
      ResourceIterator iterator = ResourceIterator.GetIterator();
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.World.GetOrCreateSystemManaged<ResourceSystem>().GetPrefabs();
      int num1 = 20000;
      while (iterator.Next())
      {
        long resource = (long) iterator.resource;
        int amount = num1;
        entityManager1 = this.EntityManager;
        double weight = (double) entityManager1.GetComponentData<ResourceData>(prefabs[iterator.resource]).m_Weight;
        float num2 = (float) EconomyUtils.GetTransportCost(10000f, (Resource) resource, amount, (float) weight) / (float) num1;
        EconomyUtils.SetTradeCost(iterator.resource, new TradeCost()
        {
          m_BuyCost = num2,
          m_SellCost = num2,
          m_Resource = iterator.resource
        }, costs, true);
      }
    }

    [Preserve]
    public CitySystem()
    {
    }
  }
}
