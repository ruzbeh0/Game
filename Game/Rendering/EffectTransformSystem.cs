// Decompiled with JetBrains decompiler
// Type: Game.Rendering.EffectTransformSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Effects;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class EffectTransformSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private EffectControlSystem m_EffectControlSystem;
    private PreCullingSystem m_PreCullingSystem;
    private EffectTransformSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PreCullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectAnimation_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_BoneHistory_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_Event_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectColorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RandomTransformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EffectTransformSystem.EffectTransformJob jobData = new EffectTransformSystem.EffectTransformJob()
      {
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RO_ComponentLookup,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_EditorContainerData = this.__TypeHandle.__Game_Tools_EditorContainer_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RelativeData = this.__TypeHandle.__Game_Objects_Relative_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_EffectDatas = this.__TypeHandle.__Game_Prefabs_EffectData_RO_ComponentLookup,
        m_RandomTransformDatas = this.__TypeHandle.__Game_Prefabs_RandomTransformData_RO_ComponentLookup,
        m_EffectColorDatas = this.__TypeHandle.__Game_Prefabs_EffectColorData_RO_ComponentLookup,
        m_EventData = this.__TypeHandle.__Game_Events_Event_RO_ComponentLookup,
        m_BoneHistories = this.__TypeHandle.__Game_Rendering_BoneHistory_RO_BufferLookup,
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
        m_Effects = this.__TypeHandle.__Game_Prefabs_Effect_RO_BufferLookup,
        m_EffectAnimations = this.__TypeHandle.__Game_Prefabs_EffectAnimation_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_EnabledData = this.m_EffectControlSystem.GetEnabledData(false, out dependencies1),
        m_CullingData = this.m_PreCullingSystem.GetCullingData(true, out dependencies2),
        m_FrameIndex = this.m_RenderingSystem.frameIndex,
        m_FrameTime = this.m_RenderingSystem.frameTime
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle dependencies3 = jobData.Schedule<EffectTransformSystem.EffectTransformJob, EnabledEffectData>(jobData.m_EnabledData, 16, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.AddEnabledDataWriter(dependencies3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PreCullingSystem.AddCullingDataReader(dependencies3);
      this.Dependency = dependencies3;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public EffectTransformSystem()
    {
    }

    [BurstCompile]
    private struct EffectTransformJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> m_EditorContainerData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Relative> m_RelativeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<EffectData> m_EffectDatas;
      [ReadOnly]
      public ComponentLookup<RandomTransformData> m_RandomTransformDatas;
      [ReadOnly]
      public ComponentLookup<EffectColorData> m_EffectColorDatas;
      [ReadOnly]
      public ComponentLookup<Game.Events.Event> m_EventData;
      [ReadOnly]
      public BufferLookup<BoneHistory> m_BoneHistories;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColors;
      [ReadOnly]
      public BufferLookup<Effect> m_Effects;
      [ReadOnly]
      public BufferLookup<EffectAnimation> m_EffectAnimations;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [NativeDisableParallelForRestriction]
      public NativeList<EnabledEffectData> m_EnabledData;
      [ReadOnly]
      public NativeList<PreCullingData> m_CullingData;
      [ReadOnly]
      public uint m_FrameIndex;
      [ReadOnly]
      public float m_FrameTime;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        ref EnabledEffectData local = ref this.m_EnabledData.ElementAt(index);
        if ((local.m_Flags & EnabledEffectFlags.IsEnabled) == (EnabledEffectFlags) 0 || (local.m_Flags & (EnabledEffectFlags.EnabledUpdated | EnabledEffectFlags.DynamicTransform | EnabledEffectFlags.OwnerUpdated)) == (EnabledEffectFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefab = this.m_Prefabs[local.m_Owner];
        // ISSUE: reference to a compiler-generated field
        EffectData effectData = this.m_EffectDatas[local.m_Prefab];
        Effect prefabEffect;
        if ((local.m_Flags & EnabledEffectFlags.EditorContainer) != (EnabledEffectFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          Game.Tools.EditorContainer editorContainer = this.m_EditorContainerData[local.m_Owner];
          prefabEffect = new Effect();
          prefabEffect.m_Effect = editorContainer.m_Prefab;
          prefabEffect.m_Scale = editorContainer.m_Scale;
          prefabEffect.m_Intensity = editorContainer.m_Intensity;
          prefabEffect.m_AnimationIndex = editorContainer.m_GroupIndex;
          prefabEffect.m_Rotation = quaternion.identity;
          prefabEffect.m_BoneIndex = (int2) -1;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          prefabEffect = this.m_Effects[prefab.m_Prefab][local.m_EffectIndex];
        }
        Entity entity = local.m_Owner;
        Temp componentData1;
        // ISSUE: reference to a compiler-generated field
        if ((local.m_Flags & EnabledEffectFlags.TempOwner) != (EnabledEffectFlags) 0 && this.m_TempData.TryGetComponent(entity, out componentData1) && componentData1.m_Original != Entity.Null)
          entity = componentData1.m_Original;
        Unity.Mathematics.Random fromIndex = Unity.Mathematics.Random.CreateFromIndex((uint) (entity.Index ^ local.m_EffectIndex));
        if ((local.m_Flags & EnabledEffectFlags.RandomTransform) != (EnabledEffectFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          RandomTransformData randomTransformData = this.m_RandomTransformDatas[prefabEffect.m_Effect];
          float3 xyz = fromIndex.NextFloat3(randomTransformData.m_AngleRange.min, randomTransformData.m_AngleRange.max);
          prefabEffect.m_Rotation = math.mul(prefabEffect.m_Rotation, quaternion.Euler(xyz));
          prefabEffect.m_Position += fromIndex.NextFloat3(randomTransformData.m_PositionRange.min, randomTransformData.m_PositionRange.max);
        }
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        int num1 = effectData.m_OwnerCulling || this.IsNearCamera(local.m_Owner) ? 1 : (this.m_EventData.HasComponent(local.m_Owner) ? 1 : 0);
        bool flag = (local.m_Flags & EnabledEffectFlags.IsLight) > (EnabledEffectFlags) 0;
        local.m_Scale = prefabEffect.m_Scale;
        local.m_Intensity = prefabEffect.m_Intensity;
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          EffectColorData effectColorData = this.m_EffectColorDatas[prefabEffect.m_Effect];
          UnityEngine.Color color = effectColorData.m_Color;
          DynamicBuffer<MeshColor> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (effectColorData.m_Source != EffectColorSource.Effect && prefabEffect.m_ParentMesh >= 0 && this.m_MeshColors.TryGetBuffer(local.m_Owner, out bufferData) && bufferData.Length > prefabEffect.m_ParentMesh)
            color = bufferData[prefabEffect.m_ParentMesh].m_ColorSet[(int) (effectColorData.m_Source - 1)] * effectColorData.m_Color.a;
          if (math.any(effectColorData.m_VaritationRanges != 0.0f))
          {
            // ISSUE: reference to a compiler-generated method
            this.Randomize(ref color, ref fromIndex, 1f - effectColorData.m_VaritationRanges, 1f + effectColorData.m_VaritationRanges);
          }
          local.m_Scale = new float3(color.r, color.g, color.b);
        }
        InterpolatedTransform componentData2;
        // ISSUE: reference to a compiler-generated field
        if (num1 != 0 && this.m_InterpolatedTransformData.TryGetComponent(local.m_Owner, out componentData2))
        {
          // ISSUE: reference to a compiler-generated method
          Game.Objects.Transform effectTransform = this.GetEffectTransform(prefabEffect, local.m_Owner);
          Game.Objects.Transform world = ObjectUtils.LocalToWorld(componentData2.ToTransform(), effectTransform);
          if ((local.m_Flags & EnabledEffectFlags.OwnerCollapsed) != (EnabledEffectFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            world.m_Position.y = math.max(world.m_Position.y, this.m_TransformData[local.m_Owner].m_Position.y);
          }
          local.m_Position = world.m_Position;
          local.m_Rotation = world.m_Rotation;
          if (flag && ((effectData.m_Flags.m_RequiredFlags | effectData.m_Flags.m_ForbiddenFlags) & (EffectConditionFlags.MainLights | EffectConditionFlags.ExtraLights | EffectConditionFlags.WarningLights)) != EffectConditionFlags.None)
          {
            // ISSUE: reference to a compiler-generated method
            bool c = this.TestFlags(effectData.m_Flags.m_RequiredFlags, effectData.m_Flags.m_ForbiddenFlags, componentData2.m_Flags);
            local.m_Intensity = math.select(0.0f, local.m_Intensity, c);
          }
        }
        else
        {
          Relative componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RelativeData.TryGetComponent(local.m_Owner, out componentData3))
          {
            // ISSUE: reference to a compiler-generated field
            Owner owner = this.m_OwnerData[local.m_Owner];
            // ISSUE: reference to a compiler-generated method
            Game.Objects.Transform relativeTransform = this.GetRelativeTransform(componentData3, owner.m_Owner);
            Game.Objects.Transform transform = new Game.Objects.Transform(prefabEffect.m_Position, prefabEffect.m_Rotation);
            Game.Objects.Transform componentData4;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Game.Objects.Transform world = ObjectUtils.LocalToWorld(!this.m_InterpolatedTransformData.TryGetComponent(owner.m_Owner, out componentData2) ? (!this.m_TransformData.TryGetComponent(owner.m_Owner, out componentData4) ? relativeTransform : ObjectUtils.LocalToWorld(componentData4, relativeTransform)) : ObjectUtils.LocalToWorld(componentData2.ToTransform(), relativeTransform), transform);
            local.m_Position = world.m_Position;
            local.m_Rotation = world.m_Rotation;
          }
          else
          {
            Curve componentData5;
            // ISSUE: reference to a compiler-generated field
            if (this.m_CurveData.TryGetComponent(local.m_Owner, out componentData5))
            {
              local.m_Position = MathUtils.Position(componentData5.m_Bezier, 0.5f);
              local.m_Rotation = quaternion.identity;
            }
            else
            {
              Game.Objects.Transform componentData6;
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformData.TryGetComponent(local.m_Owner, out componentData6))
              {
                // ISSUE: reference to a compiler-generated method
                Game.Objects.Transform effectTransform = this.GetEffectTransform(prefabEffect, local.m_Owner);
                Game.Objects.Transform world = ObjectUtils.LocalToWorld(componentData6, effectTransform);
                if ((local.m_Flags & EnabledEffectFlags.OwnerCollapsed) != (EnabledEffectFlags) 0)
                  world.m_Position.y = componentData6.m_Position.y;
                local.m_Position = world.m_Position;
                local.m_Rotation = world.m_Rotation;
              }
            }
          }
        }
        DynamicBuffer<EffectAnimation> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (prefabEffect.m_AnimationIndex < 0 || (double) local.m_Intensity == 0.0 || !this.m_EffectAnimations.TryGetBuffer(prefab.m_Prefab, out bufferData1) || bufferData1.Length <= prefabEffect.m_AnimationIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        Unity.Mathematics.Random random = this.m_PseudoRandomSeedData[local.m_Owner].GetRandom((uint) PseudoRandomSeed.kLightState);
        EffectAnimation effectAnimation = bufferData1[prefabEffect.m_AnimationIndex];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num2 = (float) ((this.m_FrameIndex + random.NextUInt(effectAnimation.m_DurationFrames)) % effectAnimation.m_DurationFrames) + this.m_FrameTime;
        local.m_Intensity *= effectAnimation.m_AnimationCurve.Evaluate(num2 / (float) effectAnimation.m_DurationFrames);
      }

      private void Randomize(ref UnityEngine.Color color, ref Unity.Mathematics.Random random, float3 min, float3 max)
      {
        float3 float3_1;
        UnityEngine.Color.RGBToHSV(color, out float3_1.x, out float3_1.y, out float3_1.z);
        float3 float3_2 = random.NextFloat3(min, max);
        float3_1.x = math.frac(float3_1.x + float3_2.x);
        float3_1.yz *= float3_2.yz;
        float3_1.y = math.saturate(float3_1.y);
        float3_1.z = math.max(0.0f, float3_1.z);
        color = UnityEngine.Color.HSVToRGB(float3_1.x, float3_1.y, float3_1.z);
      }

      private bool IsNearCamera(Entity entity)
      {
        CullingInfo componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_CullingInfoData.TryGetComponent(entity, out componentData) && componentData.m_CullingIndex != 0 && (this.m_CullingData[componentData.m_CullingIndex].m_Flags & PreCullingFlags.NearCamera) > (PreCullingFlags) 0;
      }

      private Game.Objects.Transform GetEffectTransform(Effect prefabEffect, Entity owner)
      {
        if (prefabEffect.m_BoneIndex.x >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BoneHistory> boneHistory = this.m_BoneHistories[owner];
          if (boneHistory.Length >= prefabEffect.m_BoneIndex.x)
          {
            float4x4 matrix = boneHistory[prefabEffect.m_BoneIndex.x].m_Matrix;
            float3 float3 = math.transform(matrix, prefabEffect.m_Position);
            quaternion quaternion = quaternion.LookRotation(math.rotate(matrix, math.forward(prefabEffect.m_Rotation)), math.rotate(matrix, math.mul(prefabEffect.m_Rotation, math.up())));
            if (prefabEffect.m_BoneIndex.y >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SubMesh subMesh = this.m_SubMeshes[this.m_Prefabs[owner].m_Prefab][prefabEffect.m_BoneIndex.y];
              float3 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, float3);
              quaternion = math.mul(subMesh.m_Rotation, quaternion);
            }
            return new Game.Objects.Transform(float3, quaternion);
          }
        }
        return new Game.Objects.Transform(prefabEffect.m_Position, prefabEffect.m_Rotation);
      }

      private Game.Objects.Transform GetRelativeTransform(Relative relative, Entity owner)
      {
        if (relative.m_BoneIndex.y >= 0)
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<BoneHistory> boneHistory = this.m_BoneHistories[owner];
          if (boneHistory.Length > relative.m_BoneIndex.y)
          {
            float4x4 matrix = boneHistory[relative.m_BoneIndex.y].m_Matrix;
            float3 float3 = math.transform(matrix, relative.m_Position);
            quaternion quaternion = quaternion.LookRotation(math.rotate(matrix, math.forward(relative.m_Rotation)), math.rotate(matrix, math.mul(relative.m_Rotation, math.up())));
            if (relative.m_BoneIndex.z >= 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SubMesh subMesh = this.m_SubMeshes[this.m_Prefabs[owner].m_Prefab][relative.m_BoneIndex.z];
              float3 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, float3);
              quaternion = math.mul(subMesh.m_Rotation, quaternion);
            }
            return new Game.Objects.Transform(float3, quaternion);
          }
        }
        return new Game.Objects.Transform(relative.m_Position, relative.m_Rotation);
      }

      private bool TestFlags(
        EffectConditionFlags requiredFlags,
        EffectConditionFlags forbiddenFlags,
        TransformFlags transformFlags)
      {
        int4 int4_1 = new int4(65536, 131072, 262144, 524288);
        int4 int4_2 = new int4(1, 2, 1024, 4096);
        bool4 bool4_1 = (int4_1 & (int) requiredFlags) != 0;
        bool4 bool4_2 = (int4_1 & (int) forbiddenFlags) != 0;
        bool4 bool4_3 = (int4_2 & (int) transformFlags) != 0;
        return math.any(bool4_1 & bool4_3) & !math.any(bool4_2 & bool4_3);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Tools.EditorContainer> __Game_Tools_EditorContainer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Relative> __Game_Objects_Relative_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectData> __Game_Prefabs_EffectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RandomTransformData> __Game_Prefabs_RandomTransformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EffectColorData> __Game_Prefabs_EffectColorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Events.Event> __Game_Events_Event_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<BoneHistory> __Game_Rendering_BoneHistory_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Effect> __Game_Prefabs_Effect_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<EffectAnimation> __Game_Prefabs_EffectAnimation_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RO_ComponentLookup = state.GetComponentLookup<CullingInfo>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_EditorContainer_RO_ComponentLookup = state.GetComponentLookup<Game.Tools.EditorContainer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Relative_RO_ComponentLookup = state.GetComponentLookup<Relative>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectData_RO_ComponentLookup = state.GetComponentLookup<EffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RandomTransformData_RO_ComponentLookup = state.GetComponentLookup<RandomTransformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectColorData_RO_ComponentLookup = state.GetComponentLookup<EffectColorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_Event_RO_ComponentLookup = state.GetComponentLookup<Game.Events.Event>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_BoneHistory_RO_BufferLookup = state.GetBufferLookup<BoneHistory>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Effect_RO_BufferLookup = state.GetBufferLookup<Effect>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EffectAnimation_RO_BufferLookup = state.GetBufferLookup<EffectAnimation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
      }
    }
  }
}
