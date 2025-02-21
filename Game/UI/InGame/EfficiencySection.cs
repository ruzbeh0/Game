// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.EfficiencySection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class EfficiencySection : InfoSectionBase
  {
    protected override string group => nameof (EfficiencySection);

    private int efficiency { get; set; }

    private List<EfficiencySection.EfficiencyFactor> factors { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.factors = new List<EfficiencySection.EfficiencyFactor>(28);
    }

    protected override void Reset()
    {
      this.efficiency = 0;
      this.factors.Clear();
    }

    private bool Visible()
    {
      if (!this.EntityManager.HasComponent<Building>(this.selectedEntity) || !this.EntityManager.HasComponent<Efficiency>(this.selectedEntity) || this.EntityManager.HasComponent<Abandoned>(this.selectedEntity) || this.EntityManager.HasComponent<Game.Common.Destroyed>(this.selectedEntity))
        return false;
      Entity company;
      return !CompanyUIUtils.HasCompany(this.EntityManager, this.selectedEntity, this.selectedPrefab, out company) || company != Entity.Null;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
      if (!this.visible)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = (int) math.round(100f * BuildingUtils.GetEfficiency(this.EntityManager.GetBuffer<Efficiency>(this.selectedEntity, true))) != this.efficiency;
    }

    protected override void OnProcess()
    {
      DynamicBuffer<Efficiency> buffer = this.EntityManager.GetBuffer<Efficiency>(this.selectedEntity, true);
      this.efficiency = (int) math.round(100f * BuildingUtils.GetEfficiency(buffer));
      using (NativeArray<Efficiency> nativeArray = buffer.ToNativeArray((AllocatorManager.AllocatorHandle) Allocator.Temp))
      {
        nativeArray.Sort<Efficiency>();
        this.factors.Clear();
        if (nativeArray.Length == 0)
          return;
        if (this.efficiency > 0)
        {
          float x = 100f;
          foreach (Efficiency efficiency in nativeArray)
          {
            float num1 = math.max(0.0f, efficiency.m_Efficiency);
            x *= num1;
            int num2 = math.max(-99, (int) math.round(100f * num1) - 100);
            int result = math.max(1, (int) math.round(x));
            if (num2 != 0)
            {
              // ISSUE: object of a compiler-generated type is created
              this.factors.Add(new EfficiencySection.EfficiencyFactor(efficiency.m_Factor, num2, result));
            }
          }
        }
        else
        {
          foreach (Efficiency efficiency in nativeArray)
          {
            if ((double) math.max(0.0f, efficiency.m_Efficiency) == 0.0)
            {
              // ISSUE: object of a compiler-generated type is created
              this.factors.Add(new EfficiencySection.EfficiencyFactor(efficiency.m_Factor, -100, -100));
              if (efficiency.m_Factor <= Game.Buildings.EfficiencyFactor.Fire)
                break;
            }
          }
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("efficiency");
      writer.Write(this.efficiency);
      writer.PropertyName("factors");
      writer.ArrayBegin(this.factors.Count);
      for (int index = 0; index < this.factors.Count; ++index)
        writer.Write<EfficiencySection.EfficiencyFactor>(this.factors[index]);
      writer.ArrayEnd();
    }

    [Preserve]
    public EfficiencySection()
    {
    }

    private struct EfficiencyFactor : IJsonWritable
    {
      private Game.Buildings.EfficiencyFactor factor;
      private int value;
      private int result;

      public EfficiencyFactor(Game.Buildings.EfficiencyFactor factor, int value, int result)
      {
        // ISSUE: reference to a compiler-generated field
        this.factor = factor;
        // ISSUE: reference to a compiler-generated field
        this.value = value;
        // ISSUE: reference to a compiler-generated field
        this.result = result;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(typeof (EfficiencySection.EfficiencyFactor).FullName);
        writer.PropertyName("factor");
        // ISSUE: reference to a compiler-generated field
        writer.Write(Enum.GetName(typeof (Game.Buildings.EfficiencyFactor), (object) this.factor));
        writer.PropertyName("value");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.value);
        writer.PropertyName("result");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.result);
        writer.TypeEnd();
      }
    }
  }
}
