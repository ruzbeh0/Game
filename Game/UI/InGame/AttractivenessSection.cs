// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AttractivenessSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Prefabs;
using Game.Simulation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class AttractivenessSection : InfoSectionBase
  {
    private TerrainAttractivenessSystem m_TerrainAttractivenessSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_SettingsQuery;

    protected override string group => nameof (AttractivenessSection);

    private float baseAttractiveness { get; set; }

    private float attractiveness { get; set; }

    private List<AttractivenessSection.AttractivenessFactor> factors { get; set; }

    protected override void Reset()
    {
      this.baseAttractiveness = 0.0f;
      this.factors.Clear();
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.factors = new List<AttractivenessSection.AttractivenessFactor>(5);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractivenessSystem = this.World.GetOrCreateSystemManaged<TerrainAttractivenessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessParameterData>());
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<AttractionData>(this.selectedPrefab) || this.EntityManager.HasComponent<AttractivenessProvider>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      NativeArray<int> factors = new NativeArray<int>(5, Allocator.TempJob);
      AttractionData data1;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<AttractionData>(this.selectedEntity, this.selectedPrefab, out data1))
        this.baseAttractiveness = (float) data1.m_Attractiveness;
      this.attractiveness = this.baseAttractiveness;
      DynamicBuffer<Efficiency> buffer;
      if (!this.EntityManager.HasComponent<Game.Buildings.Signature>(this.selectedEntity) && this.EntityManager.TryGetBuffer<Efficiency>(this.selectedEntity, true, out buffer))
      {
        float efficiency = BuildingUtils.GetEfficiency(buffer);
        this.attractiveness *= efficiency;
        // ISSUE: reference to a compiler-generated method
        AttractionSystem.SetFactor(factors, AttractionSystem.AttractivenessFactor.Efficiency, (float) (((double) efficiency - 1.0) * 100.0));
      }
      Game.Buildings.Park component1;
      ParkData data2;
      // ISSUE: reference to a compiler-generated method
      if (this.EntityManager.TryGetComponent<Game.Buildings.Park>(this.selectedEntity, out component1) && this.TryGetComponentWithUpgrades<ParkData>(this.selectedEntity, this.selectedPrefab, out data2))
      {
        float num = Mathf.Min(1f, (float) (0.25 + 0.25 * (double) Mathf.FloorToInt((data2.m_MaintenancePool > (short) 0 ? (float) component1.m_Maintenance / (float) data2.m_MaintenancePool : 0.0f) / 0.3f)));
        this.attractiveness *= num;
        // ISSUE: reference to a compiler-generated method
        AttractionSystem.SetFactor(factors, AttractionSystem.AttractivenessFactor.Maintenance, (float) (((double) num - 1.0) * 100.0));
      }
      Game.Objects.Transform component2;
      if (this.EntityManager.TryGetComponent<Game.Objects.Transform>(this.selectedEntity, out component2))
      {
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        CellMapData<TerrainAttractiveness> data3 = this.m_TerrainAttractivenessSystem.GetData(true, out dependencies);
        this.Dependency = JobHandle.CombineDependencies(this.Dependency, dependencies);
        this.Dependency.Complete();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
        // ISSUE: reference to a compiler-generated field
        AttractivenessParameterData singleton = this.m_SettingsQuery.GetSingleton<AttractivenessParameterData>();
        // ISSUE: reference to a compiler-generated method
        this.attractiveness *= (float) (1.0 + 0.0099999997764825821 * (double) TerrainAttractivenessSystem.EvaluateAttractiveness(component2.m_Position, data3, heightData, singleton, factors));
      }
      for (int index = 0; index < factors.Length; ++index)
      {
        if (factors[index] != 0)
        {
          // ISSUE: object of a compiler-generated type is created
          this.factors.Add(new AttractivenessSection.AttractivenessFactor(index, (float) factors[index]));
        }
      }
      factors.Dispose();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("attractiveness");
      writer.Write(this.attractiveness);
      writer.PropertyName("baseAttractiveness");
      writer.Write(this.baseAttractiveness);
      writer.PropertyName("factors");
      writer.ArrayBegin(this.factors.Count);
      for (int index = 0; index < this.factors.Count; ++index)
        writer.Write<AttractivenessSection.AttractivenessFactor>(this.factors[index]);
      writer.ArrayEnd();
    }

    [Preserve]
    public AttractivenessSection()
    {
    }

    private readonly struct AttractivenessFactor : 
      IJsonWritable,
      IComparable<AttractivenessSection.AttractivenessFactor>
    {
      private int localeKey { get; }

      private float delta { get; }

      public AttractivenessFactor(int factor, float delta)
      {
        this.localeKey = factor;
        this.delta = delta;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (AttractionSystem.AttractivenessFactor).FullName);
        writer.PropertyName("localeKey");
        writer.Write(Enum.GetName(typeof (AttractionSystem.AttractivenessFactor), (object) (AttractionSystem.AttractivenessFactor) this.localeKey));
        writer.PropertyName("delta");
        writer.Write(this.delta);
        writer.TypeEnd();
      }

      public int CompareTo(AttractivenessSection.AttractivenessFactor other)
      {
        return this.delta.CompareTo(other.delta);
      }
    }
  }
}
