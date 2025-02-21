// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectGeometryPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Rendering;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public abstract class ObjectGeometryPrefab : ObjectPrefab
  {
    public ObjectMeshInfo[] m_Meshes;
    public bool m_Circular;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_Meshes == null)
        return;
      for (int index = 0; index < this.m_Meshes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Meshes[index].m_Mesh);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectGeometryData>());
      components.Add(ComponentType.ReadWrite<SubMesh>());
      bool flag1 = false;
      bool flag2 = false;
      if (this.m_Meshes != null)
      {
        for (int index = 0; index < this.m_Meshes.Length; ++index)
        {
          RenderPrefabBase mesh = this.m_Meshes[index].m_Mesh;
          if (!((UnityEngine.Object) mesh == (UnityEngine.Object) null))
          {
            flag1 |= mesh.Has<StackProperties>();
            flag2 |= mesh is CharacterGroup;
          }
        }
      }
      if (flag1)
        components.Add(ComponentType.ReadWrite<StackData>());
      if (!flag2)
        return;
      components.Add(ComponentType.ReadWrite<SubMeshGroup>());
      components.Add(ComponentType.ReadWrite<CharacterElement>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      components.Add(ComponentType.ReadWrite<ObjectGeometry>());
      components.Add(ComponentType.ReadWrite<CullingInfo>());
      components.Add(ComponentType.ReadWrite<MeshBatch>());
      components.Add(ComponentType.ReadWrite<PseudoRandomSeed>());
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      if (this.m_Meshes != null)
      {
        for (int index1 = 0; index1 < this.m_Meshes.Length; ++index1)
        {
          RenderPrefabBase mesh = this.m_Meshes[index1].m_Mesh;
          if (!((UnityEngine.Object) mesh == (UnityEngine.Object) null))
          {
            flag1 |= mesh.Has<ColorProperties>();
            flag2 |= mesh.Has<StackProperties>();
            flag3 |= mesh.Has<AnimationProperties>();
            flag7 |= mesh is CharacterGroup;
            ProceduralAnimationProperties component = mesh.GetComponent<ProceduralAnimationProperties>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              flag4 = true;
              if (component.m_Bones != null)
              {
                for (int index2 = 0; index2 < component.m_Bones.Length; ++index2)
                {
                  switch (component.m_Bones[index2].m_Type)
                  {
                    case BoneType.LookAtDirection:
                    case BoneType.WindTurbineRotation:
                    case BoneType.WindSpeedRotation:
                    case BoneType.PoweredRotation:
                    case BoneType.TrafficBarrierDirection:
                    case BoneType.LookAtRotation:
                    case BoneType.LookAtAim:
                    case BoneType.LengthwiseLookAtRotation:
                    case BoneType.WorkingRotation:
                    case BoneType.OperatingRotation:
                    case BoneType.LookAtMovementX:
                    case BoneType.LookAtMovementY:
                    case BoneType.LookAtMovementZ:
                    case BoneType.LookAtRotationSide:
                      flag5 = true;
                      break;
                  }
                }
              }
            }
            if ((UnityEngine.Object) mesh.GetComponent<EmissiveProperties>() != (UnityEngine.Object) null)
              flag6 = true;
          }
        }
      }
      if (flag1 | flag7)
        components.Add(ComponentType.ReadWrite<MeshColor>());
      if (flag2)
        components.Add(ComponentType.ReadWrite<Stack>());
      if (flag3)
        components.Add(ComponentType.ReadWrite<Animated>());
      if (flag4)
      {
        components.Add(ComponentType.ReadWrite<Skeleton>());
        components.Add(ComponentType.ReadWrite<Bone>());
        components.Add(ComponentType.ReadWrite<BoneHistory>());
        if (flag5)
          components.Add(ComponentType.ReadWrite<Momentum>());
      }
      if (flag6)
      {
        components.Add(ComponentType.ReadWrite<Emissive>());
        components.Add(ComponentType.ReadWrite<LightState>());
      }
      if (!flag7)
        return;
      components.Add(ComponentType.ReadWrite<MeshGroup>());
      components.Add(ComponentType.ReadWrite<Animated>());
    }
  }
}
