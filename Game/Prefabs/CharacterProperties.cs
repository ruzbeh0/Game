// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.CharacterProperties
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new Type[] {typeof (RenderPrefab)})]
  public class CharacterProperties : ComponentBase
  {
    public CharacterProperties.BodyPart m_BodyParts;
    public string m_CorrectiveAnimationName;
    public string m_AnimatedPropName;
    public CharacterOverlay[] m_Overlays;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Overlays == null || this.m_Overlays.Length == 0)
        return;
      for (int index = 0; index < this.m_Overlays.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Overlays[index]);
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      if (this.m_Overlays == null || this.m_Overlays.Length == 0)
        return;
      components.Add(ComponentType.ReadWrite<OverlayElement>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      if (this.m_Overlays == null || this.m_Overlays.Length == 0)
        return;
      // ISSUE: variable of a compiler-generated type
      PrefabSystem existingSystemManaged = entityManager.World.GetExistingSystemManaged<PrefabSystem>();
      DynamicBuffer<OverlayElement> buffer = entityManager.GetBuffer<OverlayElement>(entity);
      int num = 0;
      for (int index = 0; index < this.m_Overlays.Length; ++index)
      {
        CharacterOverlay overlay = this.m_Overlays[index];
        num = math.max(num, overlay.m_Index + 1);
      }
      buffer.Resize(num, NativeArrayOptions.ClearMemory);
      for (int index = 0; index < this.m_Overlays.Length; ++index)
      {
        CharacterOverlay overlay = this.m_Overlays[index];
        // ISSUE: reference to a compiler-generated method
        buffer[overlay.m_Index] = new OverlayElement()
        {
          m_Overlay = existingSystemManaged.GetEntity((PrefabBase) overlay),
          m_SortOrder = overlay.m_SortOrder
        };
      }
    }

    [Flags]
    public enum BodyPart
    {
      Torso = 1,
      Head = 2,
      Face = 4,
      Legs = 8,
      Feet = 16, // 0x00000010
      Beard = 32, // 0x00000020
      Neck = 64, // 0x00000040
    }
  }
}
