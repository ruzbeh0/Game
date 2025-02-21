// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AssetPackItem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Asset Packs/", new System.Type[] {typeof (ZonePrefab), typeof (ObjectPrefab), typeof (NetPrefab), typeof (AreaPrefab), typeof (RoutePrefab), typeof (NetLanePrefab)})]
  public class AssetPackItem : ComponentBase
  {
    [NotNull]
    public AssetPackPrefab[] m_Packs;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Packs == null)
        return;
      for (int index = 0; index < this.m_Packs.Length; ++index)
      {
        if ((UnityEngine.Object) this.m_Packs[index] != (UnityEngine.Object) null)
          prefabs.Add((PrefabBase) this.m_Packs[index]);
      }
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<AssetPackElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      DynamicBuffer<AssetPackElement> buffer = entityManager.GetBuffer<AssetPackElement>(entity);
      buffer.Clear();
      if (this.m_Packs == null || this.m_Packs.Length == 0)
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      for (int index = 0; index < this.m_Packs.Length; ++index)
      {
        AssetPackPrefab pack = this.m_Packs[index];
        if (!((UnityEngine.Object) pack == (UnityEngine.Object) null))
        {
          AssetPackElement elem;
          // ISSUE: reference to a compiler-generated method
          elem.m_Pack = existingSystemManaged.GetEntity((PrefabBase) pack);
          buffer.Add(elem);
        }
      }
    }
  }
}
