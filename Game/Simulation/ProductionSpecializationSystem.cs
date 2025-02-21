// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ProductionSpecializationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.City;
using Game.Economy;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ProductionSpecializationSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    public static readonly int kUpdatesPerDay = 512;
    private CitySystem m_CitySystem;
    private EntityQuery m_BonusQuery;
    private NativeQueue<ProductionSpecializationSystem.ProducedResource> m_ProductionQueue;
    private JobHandle m_QueueWriters;
    private ProductionSpecializationSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / ProductionSpecializationSystem.kUpdatesPerDay;
    }

    public NativeQueue<ProductionSpecializationSystem.ProducedResource> GetQueue(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_QueueWriters;
      // ISSUE: reference to a compiler-generated field
      return this.m_ProductionQueue;
    }

    public void AddQueueWriter(JobHandle handle)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriters = JobHandle.CombineDependencies(this.m_QueueWriters, handle);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BonusQuery = this.GetEntityQuery(ComponentType.ReadOnly<SpecializationBonus>());
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionQueue = new NativeQueue<ProductionSpecializationSystem.ProducedResource>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionQueue.Dispose();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_SpecializationBonus_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProductionSpecializationSystem.SpecializationJob jobData = new ProductionSpecializationSystem.SpecializationJob()
      {
        m_Bonuses = this.__TypeHandle.__Game_City_SpecializationBonus_RW_BufferLookup,
        m_Queue = this.m_ProductionQueue,
        m_City = this.m_CitySystem.City
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<ProductionSpecializationSystem.SpecializationJob>(JobHandle.CombineDependencies(this.m_QueueWriters, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriters = this.Dependency;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionQueue.Clear();
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      this.m_QueueWriters.Complete();
      NativeArray<int> nativeArray = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
      int x = -1;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_ProductionQueue.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        ProductionSpecializationSystem.ProducedResource producedResource = this.m_ProductionQueue.Dequeue();
        // ISSUE: reference to a compiler-generated field
        int resourceIndex = EconomyUtils.GetResourceIndex(producedResource.m_Resource);
        // ISSUE: reference to a compiler-generated field
        nativeArray[resourceIndex] += producedResource.m_Amount;
        x = math.max(x, resourceIndex);
      }
      writer.Write(x + 1);
      for (int index = 0; index <= x; ++index)
        writer.Write(nativeArray[index]);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ProductionQueue.Clear();
      int num1;
      reader.Read(out num1);
      for (int index = 0; index < num1; ++index)
      {
        int num2;
        reader.Read(out num2);
        if (num2 > 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_ProductionQueue.Enqueue(new ProductionSpecializationSystem.ProducedResource()
          {
            m_Resource = EconomyUtils.GetResource(index),
            m_Amount = num2
          });
        }
      }
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      if (context.purpose != Purpose.NewGame && context.purpose != Purpose.LoadGame || !this.m_BonusQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      this.EntityManager.AddBuffer<SpecializationBonus>(this.m_CitySystem.City);
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
    public ProductionSpecializationSystem()
    {
    }

    public struct ProducedResource
    {
      public Resource m_Resource;
      public int m_Amount;
    }

    [BurstCompile]
    private struct SpecializationJob : IJob
    {
      public BufferLookup<SpecializationBonus> m_Bonuses;
      public NativeQueue<ProductionSpecializationSystem.ProducedResource> m_Queue;
      public Entity m_City;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<SpecializationBonus> bonuse = this.m_Bonuses[this.m_City];
        // ISSUE: variable of a compiler-generated type
        ProductionSpecializationSystem.ProducedResource producedResource;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out producedResource))
        {
          // ISSUE: reference to a compiler-generated field
          int resourceIndex = EconomyUtils.GetResourceIndex(producedResource.m_Resource);
          while (bonuse.Length <= resourceIndex)
            bonuse.Add(new SpecializationBonus()
            {
              m_Value = 0
            });
          SpecializationBonus specializationBonus = bonuse[resourceIndex];
          // ISSUE: reference to a compiler-generated field
          specializationBonus.m_Value += producedResource.m_Amount;
          bonuse[resourceIndex] = specializationBonus;
        }
        for (int index = 0; index < bonuse.Length; ++index)
        {
          SpecializationBonus specializationBonus = bonuse[index];
          specializationBonus.m_Value = Mathf.FloorToInt(0.999f * (float) specializationBonus.m_Value);
          bonuse[index] = specializationBonus;
        }
      }
    }

    private struct TypeHandle
    {
      public BufferLookup<SpecializationBonus> __Game_City_SpecializationBonus_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_SpecializationBonus_RW_BufferLookup = state.GetBufferLookup<SpecializationBonus>();
      }
    }
  }
}
