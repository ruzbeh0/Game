// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResourceExporterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ResourceExporterSystem : GameSystemBase
  {
    private EntityQuery m_ExporterQuery;
    private EntityQuery m_EconomyParameterQuery;
    private PathfindSetupSystem m_PathfindSetupSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private NativeQueue<ResourceExporterSystem.ExportEvent> m_ExportQueue;
    private ResourceExporterSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindSetupSystem = this.World.GetOrCreateSystemManaged<PathfindSetupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ExporterQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResourceExporter>(), ComponentType.ReadOnly<TaxPayer>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<Game.Economy.Resources>(), ComponentType.Exclude<ResourceBuyer>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExportQueue = new NativeQueue<ResourceExporterSystem.ExportEvent>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ExporterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ExportQueue.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ResourceExporter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResourceExporterSystem.ExportJob jobData1 = new ResourceExporterSystem.ExportJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ResourceExporterType = this.__TypeHandle.__Game_Companies_ResourceExporter_RO_ComponentTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_PathInformation = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_StorageCompanies = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_ExportQueue = this.m_ExportQueue.AsParallelWriter(),
        m_PathfindQueue = this.m_PathfindSetupSystem.GetQueue((object) this, 64).AsParallelWriter(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<ResourceExporterSystem.ExportJob>(this.m_ExporterQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindSetupSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResourceExporterSystem.HandleExportsJob jobData2 = new ResourceExporterSystem.HandleExportsJob()
      {
        m_Resources = this.__TypeHandle.__Game_Economy_Resources_RW_BufferLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_Storages = this.__TypeHandle.__Game_Companies_StorageCompany_RO_ComponentLookup,
        m_TradeCosts = this.__TypeHandle.__Game_Companies_TradeCost_RW_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ExportQueue = this.m_ExportQueue
      };
      this.Dependency = jobData2.Schedule<ResourceExporterSystem.HandleExportsJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
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
    public ResourceExporterSystem()
    {
    }

    [BurstCompile]
    private struct ExportJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ResourceExporter> m_ResourceExporterType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformation;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_StorageCompanies;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      public NativeQueue<ResourceExporterSystem.ExportEvent>.ParallelWriter m_ExportQueue;
      public NativeQueue<SetupQueueItem>.ParallelWriter m_PathfindQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResourceExporter> nativeArray1 = chunk.GetNativeArray<ResourceExporter>(ref this.m_ResourceExporterType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray2[index1];
          ResourceExporter resourceExporter = nativeArray1[index1];
          DynamicBuffer<TripNeeded> dynamicBuffer = bufferAccessor[index1];
          bool flag = false;
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            if (dynamicBuffer[index2].m_Purpose == Game.Citizens.Purpose.Exporting)
            {
              flag = true;
              break;
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResourceDatas.HasComponent(this.m_ResourcePrefabs[resourceExporter.m_Resource]) && (double) EconomyUtils.GetWeight(resourceExporter.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) == 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<ResourceExporter>(unfilteredChunkIndex, entity);
          }
          else if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<ResourceExporter>(unfilteredChunkIndex, entity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PathInformation.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              PathInformation pathInformation = this.m_PathInformation[entity];
              if ((pathInformation.m_State & PathFlags.Pending) == (PathFlags) 0)
              {
                Entity destination = pathInformation.m_Destination;
                // ISSUE: reference to a compiler-generated field
                if (this.m_StorageCompanies.HasComponent(destination))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: object of a compiler-generated type is created
                  this.m_ExportQueue.Enqueue(new ResourceExporterSystem.ExportEvent()
                  {
                    m_Seller = entity,
                    m_Buyer = destination,
                    m_Distance = pathInformation.m_Distance,
                    m_Amount = resourceExporter.m_Amount,
                    m_Resource = resourceExporter.m_Resource
                  });
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<ResourceExporter>(unfilteredChunkIndex, entity);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PathInformation>(unfilteredChunkIndex, entity);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PathElement>(unfilteredChunkIndex, entity);
                  dynamicBuffer.Add(new TripNeeded()
                  {
                    m_TargetAgent = destination,
                    m_Purpose = Game.Citizens.Purpose.Exporting,
                    m_Resource = resourceExporter.m_Resource,
                    m_Data = resourceExporter.m_Amount
                  });
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<ResourceExporter>(unfilteredChunkIndex, entity);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PathInformation>(unfilteredChunkIndex, entity);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<PathElement>(unfilteredChunkIndex, entity);
                }
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.FindTarget(unfilteredChunkIndex, entity, resourceExporter.m_Resource, resourceExporter.m_Amount);
            }
          }
        }
      }

      private void FindTarget(int chunkIndex, Entity exporter, Resource resource, int amount)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<PathInformation>(chunkIndex, exporter, new PathInformation()
        {
          m_State = PathFlags.Pending
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddBuffer<PathElement>(chunkIndex, exporter);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float transportCost = EconomyUtils.GetTransportCost(1f, amount, this.m_ResourceDatas[this.m_ResourcePrefabs[resource]].m_Weight, StorageTransferFlags.Car);
        PathfindParameters parameters = new PathfindParameters()
        {
          m_MaxSpeed = (float2) 111.111115f,
          m_WalkSpeed = (float2) 5.555556f,
          m_Weights = new PathfindWeights(0.01f, 0.01f, transportCost, 0.01f),
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_IgnoredRules = RuleFlags.ForbidSlowTraffic
        };
        SetupQueueTarget origin = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.CurrentLocation,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car
        };
        SetupQueueTarget destination = new SetupQueueTarget()
        {
          m_Type = SetupTargetType.ResourceExport,
          m_Methods = PathMethod.Road | PathMethod.CargoLoading,
          m_RoadTypes = RoadTypes.Car,
          m_Resource = resource,
          m_Value = amount
        };
        // ISSUE: reference to a compiler-generated field
        this.m_PathfindQueue.Enqueue(new SetupQueueItem(exporter, parameters, origin, destination));
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct ExportEvent
    {
      public Resource m_Resource;
      public Entity m_Seller;
      public int m_Amount;
      public Entity m_Buyer;
      public float m_Distance;
    }

    [BurstCompile]
    private struct HandleExportsJob : IJob
    {
      public BufferLookup<Game.Economy.Resources> m_Resources;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> m_Storages;
      public BufferLookup<TradeCost> m_TradeCosts;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public NativeQueue<ResourceExporterSystem.ExportEvent> m_ExportQueue;

      public void Execute()
      {
        // ISSUE: variable of a compiler-generated type
        ResourceExporterSystem.ExportEvent exportEvent;
        // ISSUE: reference to a compiler-generated field
        while (this.m_ExportQueue.TryDequeue(out exportEvent))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int resources = EconomyUtils.GetResources(exportEvent.m_Resource, this.m_Resources[exportEvent.m_Seller]);
          // ISSUE: reference to a compiler-generated field
          if (exportEvent.m_Amount > 0 && resources > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            exportEvent.m_Amount = math.min(exportEvent.m_Amount, resources);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int amount = Mathf.RoundToInt(EconomyUtils.GetMarketPrice(exportEvent.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) * (float) exportEvent.m_Amount);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            float weight = EconomyUtils.GetWeight(exportEvent.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) weight != 0.0 && this.m_Storages.HasComponent(exportEvent.m_Buyer))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TradeCost> tradeCost1 = this.m_TradeCosts[exportEvent.m_Buyer];
              // ISSUE: reference to a compiler-generated field
              TradeCost tradeCost2 = EconomyUtils.GetTradeCost(exportEvent.m_Resource, tradeCost1);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Assert.IsTrue(exportEvent.m_Amount != 0 && !float.IsNaN(tradeCost2.m_BuyCost), string.Format("NaN error of Entity:{0}", (object) exportEvent.m_Buyer.Index));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float num = (float) EconomyUtils.GetTransportCost(exportEvent.m_Distance, exportEvent.m_Resource, exportEvent.m_Amount, weight) / (float) exportEvent.m_Amount;
              tradeCost2.m_BuyCost = math.lerp(tradeCost2.m_BuyCost, num, 0.5f);
              // ISSUE: reference to a compiler-generated field
              Assert.IsTrue(!float.IsNaN(tradeCost2.m_BuyCost), string.Format("NaN error of Entity:{0}", (object) exportEvent.m_Buyer.Index));
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.SetTradeCost(exportEvent.m_Resource, tradeCost2, tradeCost1, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TradeCost> tradeCost3 = this.m_TradeCosts[exportEvent.m_Seller];
              tradeCost2.m_SellCost = math.lerp(tradeCost2.m_SellCost, num, 0.5f);
              // ISSUE: reference to a compiler-generated field
              EconomyUtils.SetTradeCost(exportEvent.m_Resource, tradeCost2, tradeCost3, true);
              amount -= Mathf.RoundToInt(num);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(exportEvent.m_Resource, -exportEvent.m_Amount, this.m_Resources[exportEvent.m_Seller]);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EconomyUtils.AddResources(Resource.Money, amount, this.m_Resources[exportEvent.m_Seller]);
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ResourceExporter> __Game_Companies_ResourceExporter_RO_ComponentTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Companies.StorageCompany> __Game_Companies_StorageCompany_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      public BufferLookup<Game.Economy.Resources> __Game_Economy_Resources_RW_BufferLookup;
      public BufferLookup<TradeCost> __Game_Companies_TradeCost_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ResourceExporter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceExporter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageCompany_RO_ComponentLookup = state.GetComponentLookup<Game.Companies.StorageCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferLookup = state.GetBufferLookup<Game.Economy.Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_TradeCost_RW_BufferLookup = state.GetBufferLookup<TradeCost>();
      }
    }
  }
}
