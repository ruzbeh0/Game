// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TutorialTriggerPrefabBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tutorials;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public abstract class TutorialTriggerPrefabBase : PrefabBase
  {
    private Dictionary<int, List<string>> m_BlinkDict;
    public bool m_DisplayUI = true;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TutorialTriggerData>());
    }

    protected virtual void GenerateBlinkTags()
    {
      if (this.m_BlinkDict == null)
        this.m_BlinkDict = new Dictionary<int, List<string>>();
      else
        this.m_BlinkDict.Clear();
    }

    protected void AddBlinkTag(string tag) => this.AddBlinkTagAtPosition(tag, 0);

    protected void AddBlinkTagAtPosition(string tag, int position)
    {
      if (!this.m_BlinkDict.ContainsKey(position))
        this.m_BlinkDict[position] = new List<string>();
      if (this.m_BlinkDict[position].Contains(tag))
        return;
      this.m_BlinkDict[position].Add(tag);
    }

    public Dictionary<int, List<string>> GetBlinkTags() => this.m_BlinkDict;

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      this.GenerateBlinkTags();
    }

    public virtual bool phaseBranching => false;

    public override bool ignoreUnlockDependencies => true;

    public virtual void GenerateTutorialLinks(
      EntityManager entityManager,
      NativeParallelHashSet<Entity> linkedPrefabs)
    {
    }
  }
}
