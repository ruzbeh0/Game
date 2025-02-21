// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.EditorAssetCategory
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.UI.Localization;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.Editor
{
  public class EditorAssetCategory
  {
    public static readonly string kNameFormat = "Editor.ASSET_CATEGORY_TITLE[{0}]";
    private List<EditorAssetCategory> m_SubCategories = new List<EditorAssetCategory>();

    public IReadOnlyList<EditorAssetCategory> subCategories
    {
      get => (IReadOnlyList<EditorAssetCategory>) this.m_SubCategories;
    }

    public string id { get; set; }

    public string path { get; set; }

    public EntityQuery entityQuery { get; set; }

    public EditorAssetCategorySystem.IEditorAssetCategoryFilter filter { get; set; }

    private HashSet<Entity> exclude { get; set; }

    private List<Entity> include { get; set; }

    public string icon { get; set; }

    public bool includeChildCategories { get; set; } = true;

    public bool defaultSelection { get; set; }

    public void AddSubCategory(EditorAssetCategory category) => this.m_SubCategories.Add(category);

    public HashSet<PrefabBase> GetPrefabs(
      EntityManager entityManager,
      PrefabSystem prefabSystem,
      EntityTypeHandle entityType)
    {
      HashSet<PrefabBase> prefabs = new HashSet<PrefabBase>();
      foreach (Entity entity in this.GetEntities(entityManager, prefabSystem, entityType))
      {
        PrefabBase prefab;
        // ISSUE: reference to a compiler-generated method
        if (prefabSystem.TryGetPrefab<PrefabBase>(entity, out prefab))
          prefabs.Add(prefab);
      }
      return prefabs;
    }

    public IEnumerable<Entity> GetEntities(
      EntityManager entityManager,
      PrefabSystem prefabSystem,
      EntityTypeHandle entityType)
    {
      if (this.entityQuery != new EntityQuery())
      {
        NativeArray<ArchetypeChunk> chunks = this.entityQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        try
        {
          for (int i = 0; i < chunks.Length; ++i)
          {
            NativeArray<Entity> entities = chunks[i].GetNativeArray(entityType);
            for (int j = 0; j < entities.Length; ++j)
            {
              if (this.CheckFilters(entities[j], entityManager, prefabSystem))
                yield return entities[j];
            }
            entities = new NativeArray<Entity>();
          }
        }
        finally
        {
          chunks.Dispose();
        }
        chunks = new NativeArray<ArchetypeChunk>();
      }
      if (this.includeChildCategories)
      {
        foreach (EditorAssetCategory subCategory in this.m_SubCategories)
        {
          foreach (Entity entity in subCategory.GetEntities(entityManager, prefabSystem, entityType))
            yield return entity;
        }
      }
      if (this.include != null)
      {
        foreach (Entity entity in this.include)
          yield return entity;
      }
    }

    public bool IsEmpty(
      EntityManager entityManager,
      PrefabSystem prefabSystem,
      EntityTypeHandle entityType)
    {
      return (!(this.entityQuery != new EntityQuery()) || this.entityQuery.IsEmptyIgnoreFilter || this.filter != null || this.exclude != null) && !this.GetEntities(entityManager, prefabSystem, entityType).Any<Entity>();
    }

    public void AddExclusion(Entity entity)
    {
      if (this.exclude == null)
        this.exclude = new HashSet<Entity>(1);
      this.exclude.Add(entity);
    }

    public void AddEntity(Entity entity)
    {
      if (this.include == null)
        this.include = new List<Entity>(1);
      this.include.Add(entity);
    }

    public string GetLocalizationID()
    {
      return string.Format(EditorAssetCategory.kNameFormat, (object) this.path);
    }

    private bool CheckFilters(
      Entity entity,
      EntityManager entityManager,
      PrefabSystem prefabSystem)
    {
      // ISSUE: reference to a compiler-generated method
      if (this.filter != null && !this.filter.Contains(entity, entityManager, prefabSystem))
        return false;
      return this.exclude == null || !this.exclude.Contains(entity);
    }

    public HierarchyItem<EditorAssetCategory> ToHierarchyItem(int level = 0)
    {
      string localizationId = this.GetLocalizationID();
      return new HierarchyItem<EditorAssetCategory>()
      {
        m_Data = this,
        m_DisplayName = LocalizedString.IdWithFallback(localizationId, this.id),
        m_Level = level,
        m_Icon = this.icon,
        m_Selectable = true,
        m_Selected = this.defaultSelection,
        m_Expandable = this.m_SubCategories.Count > 0,
        m_Expanded = false
      };
    }
  }
}
