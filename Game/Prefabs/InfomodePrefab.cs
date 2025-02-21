// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfomodePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public abstract class InfomodePrefab : PrefabBase
  {
    public int m_Priority;

    public virtual string infomodeTypeLocaleKey => "ObjectColor";

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfomodeData>());
    }

    public virtual void GetColors(
      out Color color0,
      out Color color1,
      out Color color2,
      out float steps,
      out float speed,
      out float tiling,
      out float fill)
    {
      color0 = new Color();
      color1 = new Color();
      color2 = new Color();
      steps = 1f;
      speed = 0.0f;
      tiling = 0.0f;
      fill = 0.0f;
    }

    public virtual int GetColorGroup() => 2;

    public virtual bool CanActivateBoth(InfomodePrefab other) => true;
  }
}
