// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CompanyInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CompanyInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "companyInfoview";
    private ResourceSystem m_ResourceSystem;
    private EntityQuery m_CommercialQuery;
    private EntityQuery m_IndustrialQuery;
    private GetterValueBinding<IndicatorValue> m_CommercialProfitability;
    private GetterValueBinding<IndicatorValue> m_IndustrialProfitability;
    private GetterValueBinding<IndicatorValue> m_OfficeProfitability;
    private CompanyInfoviewUISystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialQuery = this.GetEntityQuery(ComponentType.ReadOnly<Profitability>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<CommercialCompany>());
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialQuery = this.GetEntityQuery(ComponentType.ReadOnly<Profitability>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<IndustrialCompany>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_CommercialProfitability = new GetterValueBinding<IndicatorValue>("companyInfoview", "commercialProfitability", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_CommercialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        float current = 0.0f;
        try
        {
          int num1 = 0;
          int num2 = 0;
          for (int index = 0; index < entityArray.Length; ++index)
          {
            Profitability component;
            if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component))
            {
              ++num1;
              num2 += (int) component.m_Profitability;
            }
          }
          current = num1 == 0 ? 0.0f : (float) num2 / (float) num1;
        }
        finally
        {
          entityArray.Dispose();
        }
        return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_IndustrialProfitability = new GetterValueBinding<IndicatorValue>("companyInfoview", "industrialProfitability", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_IndustrialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<IndustrialProcessData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
        float current = 0.0f;
        try
        {
          int num3 = 0;
          int num4 = 0;
          for (int index = 0; index < entityArray.Length; ++index)
          {
            Profitability component;
            if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component) && roComponentLookup1.HasComponent(entityArray[index]))
            {
              PrefabRef prefabRef = roComponentLookup1[entityArray[index]];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (roComponentLookup3.HasComponent(prefabRef.m_Prefab) && (double) Math.Abs(EconomyUtils.GetWeight(roComponentLookup3[prefabRef.m_Prefab].m_Output.m_Resource, this.m_ResourceSystem.GetPrefabs(), ref roComponentLookup2)) >= 1.4012984643248171E-45)
              {
                ++num3;
                num4 += (int) component.m_Profitability;
              }
            }
          }
          current = num3 == 0 ? 0.0f : (float) num4 / (float) num3;
        }
        finally
        {
          entityArray.Dispose();
        }
        return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_OfficeProfitability = new GetterValueBinding<IndicatorValue>("companyInfoview", "officeProfitability", (Func<IndicatorValue>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_IndustrialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<PrefabRef> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<ResourceData> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentLookup<IndustrialProcessData> roComponentLookup6 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
        float current = 0.0f;
        try
        {
          int num5 = 0;
          int num6 = 0;
          for (int index = 0; index < entityArray.Length; ++index)
          {
            Profitability component;
            if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component) && roComponentLookup4.HasComponent(entityArray[index]))
            {
              PrefabRef prefabRef = roComponentLookup4[entityArray[index]];
              if (roComponentLookup6.HasComponent(prefabRef.m_Prefab) && (double) Math.Abs(EconomyUtils.GetWeight(roComponentLookup6[prefabRef.m_Prefab].m_Output.m_Resource, prefabs, ref roComponentLookup5)) <= 1.4012984643248171E-45)
              {
                ++num5;
                num6 += (int) component.m_Profitability;
              }
            }
          }
          current = num5 == 0 ? 0.0f : (float) num6 / (float) num5;
        }
        finally
        {
          entityArray.Dispose();
        }
        return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
      }), (IWriter<IndicatorValue>) new ValueWriter<IndicatorValue>())));
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_CommercialProfitability.active || this.m_IndustrialProfitability.active || this.m_OfficeProfitability.active;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_CommercialProfitability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialProfitability.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_OfficeProfitability.Update();
    }

    private IndicatorValue GetCommercialProfitability()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_CommercialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      float current = 0.0f;
      try
      {
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Profitability component;
          if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component))
          {
            ++num1;
            num2 += (int) component.m_Profitability;
          }
        }
        current = num1 == 0 ? 0.0f : (float) num2 / (float) num1;
      }
      finally
      {
        entityArray.Dispose();
      }
      return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
    }

    private IndicatorValue GetOfficeProfitability()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_IndustrialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<IndustrialProcessData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      float current = 0.0f;
      try
      {
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Profitability component;
          if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component) && roComponentLookup1.HasComponent(entityArray[index]))
          {
            PrefabRef prefabRef = roComponentLookup1[entityArray[index]];
            if (roComponentLookup3.HasComponent(prefabRef.m_Prefab) && (double) Math.Abs(EconomyUtils.GetWeight(roComponentLookup3[prefabRef.m_Prefab].m_Output.m_Resource, prefabs, ref roComponentLookup2)) <= 1.4012984643248171E-45)
            {
              ++num1;
              num2 += (int) component.m_Profitability;
            }
          }
        }
        current = num1 == 0 ? 0.0f : (float) num2 / (float) num1;
      }
      finally
      {
        entityArray.Dispose();
      }
      return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
    }

    private IndicatorValue GetIndustrialProfitability()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_IndustrialQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<IndustrialProcessData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      float current = 0.0f;
      try
      {
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Profitability component;
          if (this.EntityManager.TryGetComponent<Profitability>(entityArray[index], out component) && roComponentLookup1.HasComponent(entityArray[index]))
          {
            PrefabRef prefabRef = roComponentLookup1[entityArray[index]];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if (roComponentLookup3.HasComponent(prefabRef.m_Prefab) && (double) Math.Abs(EconomyUtils.GetWeight(roComponentLookup3[prefabRef.m_Prefab].m_Output.m_Resource, this.m_ResourceSystem.GetPrefabs(), ref roComponentLookup2)) >= 1.4012984643248171E-45)
            {
              ++num1;
              num2 += (int) component.m_Profitability;
            }
          }
        }
        current = num1 == 0 ? 0.0f : (float) num2 / (float) num1;
      }
      finally
      {
        entityArray.Dispose();
      }
      return new IndicatorValue(0.0f, (float) byte.MaxValue, current);
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
    public CompanyInfoviewUISystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
      }
    }
  }
}
