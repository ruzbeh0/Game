// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.BuildingLotFieldBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Prefabs;
using Game.Reflection;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Editor
{
  public abstract class BuildingLotFieldBase : IFieldBuilderFactory
  {
    private static readonly int kMaxSize = 1500;
    private static readonly int kMinSize = 1;

    public abstract FieldBuilder TryCreate(System.Type memberType, object[] attributes);

    protected FieldBuilder TryCreate(System.Type memberType, object[] attributes, bool horizontal)
    {
      return (FieldBuilder) (accessor =>
      {
        IntInputField intInputField = new IntInputField()
        {
          displayName = (LocalizedString) (horizontal ? "Lot Width" : "Lot Depth"),
          accessor = (ITypedValueAccessor<int>) new CastAccessor<int>(accessor, (Converter<object, int>) (input => (int) input), (Converter<int, object>) (input => (object) input)),
          min = BuildingLotFieldBase.kMinSize,
          max = BuildingLotFieldBase.kMaxSize
        };
        BuildingPrefab prefab;
        if (!BuildingLotFieldBase.TryGetBuildingPrefab(accessor, out prefab))
          return (IWidget) intInputField;
        Button button1 = new Button()
        {
          displayName = (LocalizedString) (horizontal ? "Expand Left" : "Expand Front")
        };
        button1.action = (Action) (() => BuildingLotFieldBase.AddCells(prefab, (IWidget) button1, new int2(horizontal ? -1 : 0, horizontal ? 0 : -1)));
        button1.disabled = (Func<bool>) (() => !horizontal ? prefab.m_LotDepth >= BuildingLotFieldBase.kMaxSize : prefab.m_LotWidth >= BuildingLotFieldBase.kMaxSize);
        Button button2 = new Button()
        {
          displayName = (LocalizedString) (horizontal ? "Expand Right" : "Expand Back")
        };
        button2.action = (Action) (() => BuildingLotFieldBase.AddCells(prefab, (IWidget) button2, new int2(horizontal ? 1 : 0, horizontal ? 0 : 1)));
        button2.disabled = (Func<bool>) (() => !horizontal ? prefab.m_LotDepth >= BuildingLotFieldBase.kMaxSize : prefab.m_LotWidth >= BuildingLotFieldBase.kMaxSize);
        Button button3 = new Button()
        {
          displayName = (LocalizedString) (horizontal ? "Shrink Left" : "Shrink Front")
        };
        button3.action = (Action) (() => BuildingLotFieldBase.AddCells(prefab, (IWidget) button3, new int2(horizontal ? -1 : 0, horizontal ? 0 : -1), -1));
        button3.disabled = (Func<bool>) (() => !horizontal ? prefab.m_LotDepth <= BuildingLotFieldBase.kMinSize : prefab.m_LotWidth <= BuildingLotFieldBase.kMinSize);
        Button button4 = new Button()
        {
          displayName = (LocalizedString) (horizontal ? "Shrink Right" : "Shrink Back")
        };
        button4.action = (Action) (() => BuildingLotFieldBase.AddCells(prefab, (IWidget) button4, new int2(horizontal ? 1 : 0, horizontal ? 0 : 1), -1));
        button4.disabled = (Func<bool>) (() => !horizontal ? prefab.m_LotDepth <= BuildingLotFieldBase.kMinSize : prefab.m_LotWidth <= BuildingLotFieldBase.kMinSize);
        return (IWidget) new Column()
        {
          children = (IList<IWidget>) new IWidget[3]
          {
            (IWidget) intInputField,
            (IWidget) new ButtonRow()
            {
              children = new Button[2]{ button1, button3 }
            },
            (IWidget) new ButtonRow()
            {
              children = new Button[2]{ button2, button4 }
            }
          }
        };
      });
    }

    private static void AddCells(BuildingPrefab prefab, IWidget widget, int2 dir, int count = 1)
    {
      int2 int2_1 = new int2(prefab.m_LotWidth, prefab.m_LotDepth);
      int2 int2_2 = new int2(int2_1.x + math.abs(dir.x) * count, int2_1.y + math.abs(dir.y) * count);
      float2 float2 = 4f * (float2) dir * (float2) (int2_2 - int2_1);
      float3 float3 = new float3(float2.x, 0.0f, float2.y);
      prefab.m_LotWidth = int2_2.x;
      prefab.m_LotDepth = int2_2.y;
      // ISSUE: reference to a compiler-generated method
      Entity entity = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().GetEntity((PrefabBase) prefab);
      EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
      foreach (ObjectMeshInfo mesh in prefab.m_Meshes)
        mesh.m_Position += float3;
      DynamicBuffer<SubMesh> buffer = entityManager.GetBuffer<SubMesh>(entity);
      for (int index = 0; index < buffer.Length; ++index)
      {
        SubMesh subMesh = buffer[index];
        subMesh.m_Position += float3;
        buffer[index] = subMesh;
      }
      ObjectSubObjects component1;
      if (prefab.TryGet<ObjectSubObjects>(out component1))
      {
        for (int index = component1.m_SubObjects.Length - 1; index >= 0; --index)
          component1.m_SubObjects[index].m_Position += float3;
      }
      ObjectSubAreas component2;
      if (prefab.TryGet<ObjectSubAreas>(out component2))
      {
        for (int index1 = component2.m_SubAreas.Length - 1; index1 >= 0; --index1)
        {
          ObjectSubAreaInfo subArea = component2.m_SubAreas[index1];
          for (int index2 = 0; index2 < subArea.m_NodePositions.Length; ++index2)
            subArea.m_NodePositions[index2] += float3;
        }
      }
      ObjectSubLanes component3;
      if (prefab.TryGet<ObjectSubLanes>(out component3))
      {
        for (int index = component3.m_SubLanes.Length - 1; index >= 0; --index)
        {
          ObjectSubLaneInfo subLane = component3.m_SubLanes[index];
          subLane.m_BezierCurve = new Bezier4x3()
          {
            a = subLane.m_BezierCurve.a + float3,
            b = subLane.m_BezierCurve.b + float3,
            c = subLane.m_BezierCurve.c + float3,
            d = subLane.m_BezierCurve.d + float3
          };
        }
      }
      ObjectSubNets component4;
      if (prefab.TryGet<ObjectSubNets>(out component4))
      {
        for (int index = component4.m_SubNets.Length - 1; index >= 0; --index)
        {
          ObjectSubNetInfo subNet = component4.m_SubNets[index];
          subNet.m_BezierCurve = new Bezier4x3()
          {
            a = subNet.m_BezierCurve.a + float3,
            b = subNet.m_BezierCurve.b + float3,
            c = subNet.m_BezierCurve.c + float3,
            d = subNet.m_BezierCurve.d + float3
          };
        }
      }
      EffectSource component5;
      if (prefab.TryGet<EffectSource>(out component5))
      {
        for (int index = component5.m_Effects.Count - 1; index >= 0; --index)
          component5.m_Effects[index].m_PositionOffset += float3;
      }
      World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<EditorPanelUISystem>()?.OnValueChanged(widget);
    }

    private static bool TryGetBuildingPrefab(IValueAccessor accessor, out BuildingPrefab prefab)
    {
      if (accessor is FieldAccessor fieldAccessor && fieldAccessor.parent is ObjectAccessor<object> parent)
      {
        prefab = parent.GetValue() as BuildingPrefab;
        return (UnityEngine.Object) prefab != (UnityEngine.Object) null;
      }
      prefab = (BuildingPrefab) null;
      return false;
    }
  }
}
