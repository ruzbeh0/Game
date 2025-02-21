// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LinesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Objects;
using Game.Routes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LinesSection : InfoSectionBase
  {
    private TransportationOverviewUISystem m_TransportationOverviewUISystem;
    private NativeArray<bool> m_BoolResult;
    private NativeArray<int> m_PassengersResult;
    private NativeList<Entity> m_LinesResult;
    private LinesSection.TypeHandle __TypeHandle;

    protected override string group => nameof (LinesSection);

    protected override bool displayForOutsideConnections => true;

    private NativeList<Entity> lines { get; set; }

    protected override void Reset()
    {
      this.lines.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_LinesResult.Clear();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportationOverviewUISystem = this.World.GetOrCreateSystemManaged<TransportationOverviewUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BoolResult = new NativeArray<bool>(2, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_PassengersResult = new NativeArray<int>(1, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_LinesResult = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.lines = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.AddBinding((IBinding) new TriggerBinding<Entity, bool>(this.group, "toggle", (Action<Entity, bool>) ((entity, state) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TransportationOverviewUISystem.SetLineState(entity, state);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.RequestUpdate();
      })));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      this.lines.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_LinesResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_PassengersResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_BoolResult.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      new LinesSection.LinesJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_Owners = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_WaitingPassengers = this.__TypeHandle.__Game_Routes_WaitingPassengers_RO_ComponentLookup,
        m_SubObjectBuffers = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ConnectedRouteBuffers = this.__TypeHandle.__Game_Routes_ConnectedRoute_RO_BufferLookup,
        m_BoolResult = this.m_BoolResult,
        m_LinesResult = this.m_LinesResult,
        m_PassengersResult = this.m_PassengersResult
      }.Schedule<LinesSection.LinesJob>(this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.visible = this.m_BoolResult[0] || this.m_BoolResult[1];
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.SetRoutesVisible();
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < this.m_LinesResult.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.lines.Add(this.m_LinesResult[index]);
      }
      List<string> tooltipTags1 = this.tooltipTags;
      TooltipTags tooltipTags2 = TooltipTags.CargoRoute;
      string str1 = tooltipTags2.ToString();
      tooltipTags1.Add(str1);
      List<string> tooltipTags3 = this.tooltipTags;
      tooltipTags2 = TooltipTags.TransportLine;
      string str2 = tooltipTags2.ToString();
      tooltipTags3.Add(str2);
      List<string> tooltipTags4 = this.tooltipTags;
      tooltipTags2 = TooltipTags.TransportStop;
      string str3 = tooltipTags2.ToString();
      tooltipTags4.Add(str3);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("hasLines");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_BoolResult[0]);
      writer.PropertyName("lines");
      writer.ArrayBegin(this.lines.Length);
      int index = 0;
      while (true)
      {
        int num = index;
        NativeList<Entity> lines = this.lines;
        int length = lines.Length;
        if (num < length)
        {
          writer.TypeBegin("Game.UI.LinesSection.Line");
          writer.PropertyName("name");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          NameSystem nameSystem = this.m_NameSystem;
          IJsonWriter writer1 = writer;
          lines = this.lines;
          Entity entity1 = lines[index];
          // ISSUE: reference to a compiler-generated method
          nameSystem.BindName(writer1, entity1);
          writer.PropertyName("color");
          EntityManager entityManager1 = this.EntityManager;
          lines = this.lines;
          Entity entity2 = lines[index];
          Game.Routes.Color color;
          ref Game.Routes.Color local1 = ref color;
          if (entityManager1.TryGetComponent<Game.Routes.Color>(entity2, out local1))
            writer.Write(color.m_Color);
          else
            writer.Write(UnityEngine.Color.white);
          writer.PropertyName("entity");
          IJsonWriter writer2 = writer;
          lines = this.lines;
          Entity entity3 = lines[index];
          writer2.Write(entity3);
          EntityManager entityManager2 = this.EntityManager;
          ref EntityManager local2 = ref entityManager2;
          lines = this.lines;
          Entity entity4 = lines[index];
          bool flag = !RouteUtils.CheckOption(local2.GetComponentData<Route>(entity4), RouteOption.Inactive);
          writer.PropertyName("active");
          writer.Write(flag);
          writer.TypeEnd();
          ++index;
        }
        else
          break;
      }
      writer.ArrayEnd();
      writer.PropertyName("hasPassengers");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_BoolResult[1]);
      writer.PropertyName("passengers");
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_PassengersResult[0]);
    }

    private void OnToggle(Entity entity, bool state)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TransportationOverviewUISystem.SetLineState(entity, state);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.RequestUpdate();
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
    public LinesSection()
    {
    }

    private enum Result
    {
      HasRoutes,
      HasPassengers,
    }

    [BurstCompile]
    private struct LinesJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public ComponentLookup<Owner> m_Owners;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> m_WaitingPassengers;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjectBuffers;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> m_ConnectedRouteBuffers;
      public NativeArray<bool> m_BoolResult;
      public NativeList<Entity> m_LinesResult;
      public NativeArray<int> m_PassengersResult;

      public void Execute()
      {
        bool supportRoutes = false;
        bool supportPassengers = false;
        int passengerCount = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.CheckEntity(this.m_SelectedEntity, ref supportRoutes, ref supportPassengers, ref passengerCount);
        // ISSUE: reference to a compiler-generated field
        this.m_PassengersResult[0] = passengerCount;
        // ISSUE: reference to a compiler-generated field
        this.m_BoolResult[0] = supportRoutes;
        // ISSUE: reference to a compiler-generated field
        this.m_BoolResult[1] = supportPassengers;
      }

      private void CheckEntity(
        Entity entity,
        ref bool supportRoutes,
        ref bool supportPassengers,
        ref int passengerCount)
      {
        DynamicBuffer<ConnectedRoute> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedRouteBuffers.TryGetBuffer(entity, out bufferData1))
        {
          supportRoutes = true;
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            WaitingPassengers componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaitingPassengers.TryGetComponent(bufferData1[index].m_Waypoint, out componentData1))
            {
              supportPassengers = true;
              passengerCount += componentData1.m_Count;
            }
            Owner componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Owners.TryGetComponent(bufferData1[index].m_Waypoint, out componentData2) && !this.m_LinesResult.Contains<Entity, Entity>(componentData2.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_LinesResult.Add(in componentData2.m_Owner);
            }
          }
        }
        WaitingPassengers componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_WaitingPassengers.TryGetComponent(entity, out componentData))
        {
          supportPassengers = true;
          passengerCount += componentData.m_Count;
        }
        DynamicBuffer<SubObject> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjectBuffers.TryGetBuffer(entity, out bufferData2))
          return;
        for (int index = 0; index < bufferData2.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.CheckEntity(bufferData2[index].m_SubObject, ref supportRoutes, ref supportPassengers, ref passengerCount);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaitingPassengers> __Game_Routes_WaitingPassengers_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedRoute> __Game_Routes_ConnectedRoute_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_WaitingPassengers_RO_ComponentLookup = state.GetComponentLookup<WaitingPassengers>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_ConnectedRoute_RO_BufferLookup = state.GetBufferLookup<ConnectedRoute>(true);
      }
    }
  }
}
