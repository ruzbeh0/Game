// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.OutsideConnectionsInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class OutsideConnectionsInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "outsideInfo";
    private CommercialDemandSystem m_CommercialDemandSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private CountCompanyDataSystem m_CountCompanyDataSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_ResourceQuery;
    private RawValueBinding m_TopImportNames;
    private RawValueBinding m_TopExportNames;
    private RawValueBinding m_TopImportColors;
    private RawValueBinding m_TopExportColors;
    private RawValueBinding m_TopImportData;
    private RawValueBinding m_TopExportData;
    private List<OutsideConnectionsInfoviewUISystem.TopResource> m_TopImports;
    private List<OutsideConnectionsInfoviewUISystem.TopResource> m_TopExports;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_CommercialDemandSystem = this.World.GetOrCreateSystemManaged<CommercialDemandSystem>();
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      this.m_CountCompanyDataSystem = this.World.GetOrCreateSystemManaged<CountCompanyDataSystem>();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_ResourceQuery = this.GetEntityQuery(ComponentType.ReadOnly<ResourceData>(), ComponentType.ReadOnly<TaxableResourceData>(), ComponentType.ReadOnly<PrefabData>());
      this.m_TopImports = new List<OutsideConnectionsInfoviewUISystem.TopResource>(41);
      this.m_TopExports = new List<OutsideConnectionsInfoviewUISystem.TopResource>(41);
      this.UpdateCache();
      this.AddBinding((IBinding) (this.m_TopImportNames = new RawValueBinding("outsideInfo", "topImportNames", new Action<IJsonWriter>(this.UpdateImportNames))));
      this.AddBinding((IBinding) (this.m_TopImportColors = new RawValueBinding("outsideInfo", "topImportColors", new Action<IJsonWriter>(this.UpdateImportColors))));
      this.AddBinding((IBinding) (this.m_TopImportData = new RawValueBinding("outsideInfo", "topImportData", new Action<IJsonWriter>(this.UpdateImportData))));
      this.AddBinding((IBinding) (this.m_TopExportNames = new RawValueBinding("outsideInfo", "topExportNames", new Action<IJsonWriter>(this.UpdateExportNames))));
      this.AddBinding((IBinding) (this.m_TopExportColors = new RawValueBinding("outsideInfo", "topExportColors", new Action<IJsonWriter>(this.UpdateExportColors))));
      this.AddBinding((IBinding) (this.m_TopExportData = new RawValueBinding("outsideInfo", "topExportData", new Action<IJsonWriter>(this.UpdateExportData))));
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_TopImportNames.active || this.m_TopImportColors.active || this.m_TopImportData.active || this.m_TopExportNames.active || this.m_TopExportColors.active || this.m_TopExportData.active;
      }
    }

    protected override void PerformUpdate()
    {
      this.UpdateCache();
      this.m_TopImportNames.Update();
      this.m_TopImportColors.Update();
      this.m_TopImportData.Update();
      this.m_TopExportNames.Update();
      this.m_TopExportColors.Update();
      this.m_TopExportData.Update();
    }

    private void UpdateCache()
    {
      NativeArray<Entity> entityArray = this.m_ResourceQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeArray<PrefabData> componentDataArray = this.m_ResourceQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle deps1;
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> production = this.m_CountCompanyDataSystem.GetProduction(out deps1);
      JobHandle deps2;
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> consumption1 = this.m_IndustrialDemandSystem.GetConsumption(out deps2);
      JobHandle deps3;
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> consumption2 = this.m_CommercialDemandSystem.GetConsumption(out deps3);
      JobHandle.CompleteAll(ref deps1, ref deps2, ref deps3);
      this.m_TopImports.Clear();
      this.m_TopExports.Clear();
      try
      {
        for (int index = 0; index < entityArray.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          ResourcePrefab prefab = this.m_PrefabSystem.GetPrefab<ResourcePrefab>(componentDataArray[index]);
          int resourceIndex = EconomyUtils.GetResourceIndex(EconomyUtils.GetResource(prefab.m_Resource));
          int y1 = production[resourceIndex];
          int x1 = consumption1[resourceIndex];
          int x2 = consumption2[resourceIndex];
          int y2 = math.min(x1 + x2, y1);
          int num1 = math.min(x1, y2);
          int num2 = x1 - num1;
          int num3 = math.min(x2, y2 - num1);
          int amount1 = x2 - num3 + num2;
          int amount2 = y1 - y2;
          this.m_TopImports.Add(new OutsideConnectionsInfoviewUISystem.TopResource(prefab.name, amount1, prefab.m_Color));
          this.m_TopExports.Add(new OutsideConnectionsInfoviewUISystem.TopResource(prefab.name, amount2, prefab.m_Color));
        }
        this.m_TopImports.Sort();
        this.m_TopExports.Sort();
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
    }

    private void UpdateImportNames(IJsonWriter binder)
    {
      int size = 10;
      if (this.m_TopImports.Count < size)
        size = this.m_TopImports.Count;
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        binder.Write(this.m_TopImports[index].id);
      binder.ArrayEnd();
    }

    private void UpdateImportColors(IJsonWriter binder)
    {
      int size = 10;
      if (this.m_TopImports.Count < size)
        size = this.m_TopImports.Count;
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        binder.Write(this.m_TopImports[index].color.ToHexCode());
      binder.ArrayEnd();
    }

    private void UpdateImportData(IJsonWriter binder)
    {
      int num = 0;
      int size = 10;
      if (this.m_TopImports.Count < size)
        size = this.m_TopImports.Count;
      binder.TypeBegin("infoviews.ChartData");
      binder.PropertyName("values");
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
      {
        binder.Write(this.m_TopImports[index].amount);
        num += this.m_TopImports[index].amount;
      }
      binder.ArrayEnd();
      binder.PropertyName("total");
      binder.Write(num);
      binder.TypeEnd();
    }

    private void UpdateExportNames(IJsonWriter binder)
    {
      int size = 10;
      if (this.m_TopExports.Count < size)
        size = this.m_TopExports.Count;
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        binder.Write(this.m_TopExports[index].id);
      binder.ArrayEnd();
    }

    private void UpdateExportColors(IJsonWriter binder)
    {
      int size = 10;
      if (this.m_TopExports.Count < size)
        size = this.m_TopExports.Count;
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
        binder.Write(this.m_TopExports[index].color.ToHexCode());
      binder.ArrayEnd();
    }

    private void UpdateExportData(IJsonWriter binder)
    {
      int num = 0;
      int size = 10;
      if (this.m_TopExports.Count < size)
        size = this.m_TopExports.Count;
      binder.TypeBegin("infoviews.ChartData");
      binder.PropertyName("values");
      binder.ArrayBegin(size);
      for (int index = 0; index < size; ++index)
      {
        binder.Write(this.m_TopExports[index].amount);
        num += this.m_TopExports[index].amount;
      }
      binder.ArrayEnd();
      binder.PropertyName("total");
      binder.Write(num);
      binder.TypeEnd();
    }

    [Preserve]
    public OutsideConnectionsInfoviewUISystem()
    {
    }

    public struct TopResource : IComparable<OutsideConnectionsInfoviewUISystem.TopResource>
    {
      public string id;
      public int amount;
      public Color color;

      public TopResource(string id, int amount, Color color)
      {
        this.id = id;
        this.amount = amount;
        this.color = color;
      }

      public int CompareTo(
        OutsideConnectionsInfoviewUISystem.TopResource other)
      {
        int num = other.amount - this.amount;
        return num == 0 ? string.Compare(this.id, other.id, StringComparison.Ordinal) : num;
      }
    }
  }
}
