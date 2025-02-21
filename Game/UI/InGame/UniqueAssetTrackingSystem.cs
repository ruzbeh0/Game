// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UniqueAssetTrackingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class UniqueAssetTrackingSystem : GameSystemBase, IUniqueAssetTrackingSystem
  {
    private EntityQuery m_LoadedUniqueAssetQuery;
    private EntityQuery m_DeletedUniqueAssetQuery;
    private EntityQuery m_PlacedUniqueAssetQuery;
    private bool m_Loaded;

    public NativeParallelHashSet<Entity> placedUniqueAssets { get; private set; }

    public Action<Entity, bool> EventUniqueAssetStatusChanged { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_LoadedUniqueAssetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>());
      this.m_DeletedUniqueAssetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      this.m_PlacedUniqueAssetQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Objects.UniqueObject>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      this.placedUniqueAssets = new NativeParallelHashSet<Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.placedUniqueAssets.Dispose();
      base.OnDestroy();
    }

    private bool GetLoaded()
    {
      if (!this.m_Loaded)
        return false;
      this.m_Loaded = false;
      return true;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      if (this.GetLoaded() && !this.m_LoadedUniqueAssetQuery.IsEmptyIgnoreFilter)
      {
        NativeArray<PrefabRef> componentDataArray = this.m_LoadedUniqueAssetQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          this.placedUniqueAssets.Add(componentDataArray[index].m_Prefab);
          Action<Entity, bool> assetStatusChanged = this.EventUniqueAssetStatusChanged;
          if (assetStatusChanged != null)
            assetStatusChanged(componentDataArray[index].m_Prefab, true);
        }
        componentDataArray.Dispose();
      }
      if (!this.m_PlacedUniqueAssetQuery.IsEmptyIgnoreFilter)
      {
        NativeArray<PrefabRef> componentDataArray = this.m_PlacedUniqueAssetQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          this.placedUniqueAssets.Add(componentDataArray[index].m_Prefab);
          Action<Entity, bool> assetStatusChanged = this.EventUniqueAssetStatusChanged;
          if (assetStatusChanged != null)
            assetStatusChanged(componentDataArray[index].m_Prefab, true);
        }
        componentDataArray.Dispose();
      }
      if (this.m_DeletedUniqueAssetQuery.IsEmptyIgnoreFilter)
        return;
      NativeArray<PrefabRef> componentDataArray1 = this.m_DeletedUniqueAssetQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index = 0; index < componentDataArray1.Length; ++index)
      {
        this.placedUniqueAssets.Remove(componentDataArray1[index].m_Prefab);
        Action<Entity, bool> assetStatusChanged = this.EventUniqueAssetStatusChanged;
        if (assetStatusChanged != null)
          assetStatusChanged(componentDataArray1[index].m_Prefab, false);
      }
      componentDataArray1.Dispose();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      this.placedUniqueAssets.Clear();
      this.m_Loaded = true;
    }

    public bool IsPlacedUniqueAsset(Entity entity)
    {
      PlaceableObjectData component;
      return this.EntityManager.TryGetComponent<PlaceableObjectData>(entity, out component) && (component.m_Flags & PlacementFlags.Unique) != PlacementFlags.None && this.placedUniqueAssets.Contains(entity);
    }

    [Preserve]
    public UniqueAssetTrackingSystem()
    {
    }
  }
}
