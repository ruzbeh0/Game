// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.FeatureUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class FeatureUISystem : UISystemBase
  {
    private const string kGroup = "feature";
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private EntityQuery m_UnlockedFeatureQuery;
    private EntityQuery m_UnlocksQuery;
    private RawValueBinding m_FeaturesBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      this.m_UnlockedFeatureQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<FeatureData>(), ComponentType.ReadOnly<Locked>());
      this.m_UnlocksQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      this.AddBinding((IBinding) (this.m_FeaturesBinding = new RawValueBinding("feature", "lockedFeatures", new Action<IJsonWriter>(this.BindLockedFeatures))));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (!PrefabUtils.HasUnlockedPrefab<FeatureData>(this.EntityManager, this.m_UnlocksQuery))
        return;
      this.m_FeaturesBinding.Update();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      this.m_FeaturesBinding.Update();
    }

    private void BindLockedFeatures(IJsonWriter writer)
    {
      NativeArray<Entity> entityArray = this.m_UnlockedFeatureQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      NativeArray<PrefabData> componentDataArray = this.m_UnlockedFeatureQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
      writer.ArrayBegin(componentDataArray.Length);
      for (int index = 0; index < entityArray.Length; ++index)
      {
        Entity prefabEntity = entityArray[index];
        // ISSUE: reference to a compiler-generated method
        FeaturePrefab prefab = this.m_PrefabSystem.GetPrefab<FeaturePrefab>(componentDataArray[index]);
        writer.TypeBegin("Game.UI.InGame.LockedFeature");
        writer.PropertyName("name");
        writer.Write(prefab.name);
        writer.PropertyName("requirements");
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabUISystem.BindPrefabRequirements(writer, prefabEntity);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public FeatureUISystem()
    {
    }
  }
}
