// Decompiled with JetBrains decompiler
// Type: Game.Objects.AlignSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class AlignSystem : GameSystemBase
  {
    private ModificationBarrier4 m_ModificationBarrier;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_UpdateQuery;
    private ComponentTypeSet m_AppliedTypes;
    private AlignSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Aligned>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdateQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stack_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new AlignSystem.AlignJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AlignedType = this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_AlignedData = this.__TypeHandle.__Game_Objects_Aligned_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabPillarData = this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabStackData = this.__TypeHandle.__Game_Prefabs_StackData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_PrefabPlaceholderObjects = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_StackData = this.__TypeHandle.__Game_Objects_Stack_RW_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_AppliedTypes = this.m_AppliedTypes,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(true),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<AlignSystem.AlignJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, deps));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      this.Dependency = jobHandle;
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
    public AlignSystem()
    {
    }

    [BurstCompile]
    private struct AlignJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Aligned> m_AlignedType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Aligned> m_AlignedData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PillarData> m_PrefabPillarData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<StackData> m_PrefabStackData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderObjects;
      public ComponentLookup<Transform> m_TransformData;
      public ComponentLookup<Stack> m_StackData;
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Aligned> nativeArray2 = chunk.GetNativeArray<Aligned>(ref this.m_AlignedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Attached> nativeArray3 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          bool isTemp = chunk.Has<Temp>(ref this.m_TempType);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            Aligned aligned = nativeArray2[index2];
            Owner owner = nativeArray4[index2];
            PrefabRef prefabRef = nativeArray5[index2];
            Attached attached = new Attached();
            if (nativeArray3.Length != 0)
              attached = nativeArray3[index2];
            if (attached.m_Parent == Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              Transform transform1 = this.m_TransformData[entity];
              Transform transform2 = transform1;
              // ISSUE: reference to a compiler-generated method
              this.Align(entity, aligned, owner, prefabRef, isTemp, ref transform2);
              if (!transform2.Equals(transform1))
              {
                // ISSUE: reference to a compiler-generated method
                this.MoveObject(entity, transform1, transform2);
              }
            }
          }
        }
      }

      private void MoveObject(Entity entity, Transform oldTransform, Transform newTransform)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData[entity] = newTransform;
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        Transform inverseParentTransform = ObjectUtils.InverseTransform(oldTransform);
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(subObject, out componentData) && !(componentData.m_Owner != entity) && this.m_UpdatedData.HasComponent(subObject) && !this.m_AlignedData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[subObject];
            Transform world = ObjectUtils.LocalToWorld(newTransform, ObjectUtils.WorldToLocal(inverseParentTransform, transform));
            if (!world.Equals(transform))
            {
              // ISSUE: reference to a compiler-generated method
              this.MoveObject(subObject, transform, world);
            }
          }
        }
      }

      private void Align(
        Entity entity,
        Aligned aligned,
        Owner owner,
        PrefabRef prefabRef,
        bool isTemp,
        ref Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef ownerPrefabRef = this.m_PrefabRefData[owner.m_Owner];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubObjects.HasBuffer(ownerPrefabRef.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Prefabs.SubObject> prefabSubObject1 = this.m_PrefabSubObjects[ownerPrefabRef.m_Prefab];
        if (prefabSubObject1.Length <= (int) aligned.m_SubObjectIndex)
          return;
        Game.Prefabs.SubObject prefabSubObject2 = prefabSubObject1[(int) aligned.m_SubObjectIndex];
        PillarData pillarData = new PillarData()
        {
          m_Type = PillarType.None
        };
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabPillarData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          pillarData = this.m_PrefabPillarData[prefabRef.m_Prefab];
          switch (pillarData.m_Type)
          {
            case PillarType.Vertical:
              return;
            case PillarType.Standalone:
              prefabSubObject2.m_Flags |= SubObjectFlags.AnchorTop;
              prefabSubObject2.m_Flags |= SubObjectFlags.OnGround;
              break;
            case PillarType.Base:
              prefabSubObject2.m_Flags |= SubObjectFlags.OnGround;
              break;
          }
        }
        Transform transform1 = new Transform(float3.zero, quaternion.identity);
        // ISSUE: reference to a compiler-generated field
        if (this.m_CurveData.HasComponent(owner.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[owner.m_Owner];
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[owner.m_Owner];
          float3 x;
          float3 y;
          if ((prefabSubObject2.m_Flags & SubObjectFlags.MiddlePlacement) == (SubObjectFlags) 0)
          {
            if ((double) math.distancesq(transform.m_Position, curve.m_Bezier.a) < (double) math.distancesq(transform.m_Position, curve.m_Bezier.d))
            {
              x = edgeGeometry.m_Start.m_Right.a;
              y = edgeGeometry.m_Start.m_Left.a;
            }
            else
            {
              x = edgeGeometry.m_End.m_Left.d;
              y = edgeGeometry.m_End.m_Right.d;
            }
          }
          else
          {
            x = edgeGeometry.m_Start.m_Left.d;
            y = edgeGeometry.m_Start.m_Right.d;
          }
          transform1 = new Transform(math.lerp(x, y, 0.5f), NetUtils.GetNodeRotation(new float3()
          {
            xz = MathUtils.Right(y.xz - x.xz)
          }));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_NodeData.HasComponent(owner.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            NodeGeometry nodeGeometry = this.m_NodeGeometryData[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            Game.Net.Node node = this.m_NodeData[owner.m_Owner];
            if ((prefabSubObject2.m_Flags & SubObjectFlags.EdgePlacement) != (SubObjectFlags) 0)
              prefabSubObject2.m_Position.z = 0.0f;
            transform1 = new Transform(node.m_Position, node.m_Rotation);
            transform1.m_Position.y = nodeGeometry.m_Position;
          }
        }
        transform = ObjectUtils.LocalToWorld(transform1, prefabSubObject2.m_Position, prefabSubObject2.m_Rotation);
        if (pillarData.m_Type == PillarType.Horizontal)
        {
          Game.Prefabs.SubObject prefabSubObject3 = prefabSubObject2;
          prefabSubObject3.m_Flags |= SubObjectFlags.AnchorTop;
          prefabSubObject3.m_Flags |= SubObjectFlags.OnGround;
          // ISSUE: reference to a compiler-generated method
          this.AlignVerticalPillars(entity, aligned, owner, ownerPrefabRef, transform1, prefabSubObject3, isTemp, ref prefabRef, ref transform);
        }
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData prefabGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated method
        this.AlignHeight(entity, owner, prefabRef, prefabSubObject2, prefabGeometryData, transform1, ref transform);
      }

      private void AlignVerticalPillars(
        Entity entity,
        Aligned aligned,
        Owner owner,
        PrefabRef ownerPrefabRef,
        Transform parentTransform,
        Game.Prefabs.SubObject prefabSubObject,
        bool isTemp,
        ref PrefabRef prefabRef,
        ref Transform transform)
      {
        Entity pillar1;
        Entity pillar2;
        // ISSUE: reference to a compiler-generated method
        if (!this.FindVerticalPillars(entity, aligned, owner, isTemp, out pillar1, out pillar2))
          return;
        if (pillar2 != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated method
          this.AlignDoubleVerticalPillars(entity, pillar1, pillar2, owner, ownerPrefabRef, parentTransform, prefabSubObject, ref prefabRef, ref transform);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.AlignSingleVerticalPillar(entity, pillar1, true, owner, ownerPrefabRef, parentTransform, prefabSubObject, ref prefabRef, ref transform);
        }
      }

      private void AlignSingleVerticalPillar(
        Entity entity,
        Entity pillar1,
        bool selectPrefab,
        Owner owner,
        PrefabRef ownerPrefabRef,
        Transform parentTransform,
        Game.Prefabs.SubObject prefabSubObject,
        ref PrefabRef prefabRef,
        ref Transform transform)
      {
        Attached attached = new Attached();
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(pillar1))
        {
          // ISSUE: reference to a compiler-generated field
          attached = this.m_AttachedData[pillar1];
        }
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[pillar1];
        if (attached.m_Parent == Entity.Null)
          transform1 = transform;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[pillar1];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData prefabGeometryData = this.m_PrefabObjectGeometryData[prefabRef1.m_Prefab];
        float num1 = prefabGeometryData.m_Size.x * 0.5f;
        float targetWidth = 0.0f;
        Bounds1 bounds1 = new Bounds1(0.0f, 1000000f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabNetGeometryData.HasComponent(ownerPrefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          targetWidth = this.m_PrefabNetGeometryData[ownerPrefabRef.m_Prefab].m_ElevatedWidth - 1f;
        }
        if (attached.m_Parent != Entity.Null)
        {
          float3 x = (transform1.m_Position - transform.m_Position) with
          {
            y = 0.0f
          };
          float num2 = math.length(x);
          if ((double) num2 >= 0.10000000149011612)
          {
            x /= num2;
            float num3 = (float) (-(double) targetWidth * 0.5);
            float num4 = math.max(targetWidth * 0.5f, num2 + num1);
            float num5 = (float) (((double) num3 + (double) num4) * 0.5);
            targetWidth = num4 - num3;
            bounds1 = new Bounds1((float2) math.abs(num2 - num5));
            float3 float3 = new float3();
            float3.xz = MathUtils.Left(x.xz);
            if ((double) math.dot(math.forward(parentTransform.m_Rotation), float3) < 0.0)
              float3 = -float3;
            transform.m_Position.xz += x.xz * num5;
            transform.m_Rotation = quaternion.LookRotation(float3, math.up());
          }
          // ISSUE: reference to a compiler-generated method
          this.AlignRotation(transform.m_Rotation, ref transform1);
        }
        else
          transform1 = transform;
        if (selectPrefab)
        {
          // ISSUE: reference to a compiler-generated method
          this.SelectHorizontalPillar(entity, prefabSubObject, targetWidth, bounds1, bounds1, ref prefabRef);
        }
        transform1.m_Position.y = transform.m_Position.y;
        // ISSUE: reference to a compiler-generated method
        this.AlignHeight(pillar1, owner, prefabRef1, prefabSubObject, prefabGeometryData, parentTransform, ref transform1);
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData[pillar1] = transform1;
      }

      private void AlignDoubleVerticalPillars(
        Entity entity,
        Entity pillar1,
        Entity pillar2,
        Owner owner,
        PrefabRef ownerPrefabRef,
        Transform parentTransform,
        Game.Prefabs.SubObject prefabSubObject,
        ref PrefabRef prefabRef,
        ref Transform transform)
      {
        Attached a = new Attached();
        Attached b = new Attached();
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(pillar1))
        {
          // ISSUE: reference to a compiler-generated field
          a = this.m_AttachedData[pillar1];
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(pillar2))
        {
          // ISSUE: reference to a compiler-generated field
          b = this.m_AttachedData[pillar2];
        }
        if (b.m_Parent != Entity.Null && a.m_Parent == Entity.Null)
        {
          CommonUtils.Swap<Entity>(ref pillar1, ref pillar2);
          CommonUtils.Swap<Attached>(ref a, ref b);
        }
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[pillar1];
        // ISSUE: reference to a compiler-generated field
        Transform transform2 = this.m_TransformData[pillar2];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_PrefabRefData[pillar1];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef2 = this.m_PrefabRefData[pillar2];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData prefabGeometryData1 = this.m_PrefabObjectGeometryData[prefabRef1.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData prefabGeometryData2 = this.m_PrefabObjectGeometryData[prefabRef2.m_Prefab];
        float num1 = prefabGeometryData1.m_Size.x * 0.5f;
        float num2 = prefabGeometryData2.m_Size.x * 0.5f;
        float num3 = num1 + num2;
        float targetWidth1 = 0.0f;
        Bounds1 offsetRange1 = new Bounds1(0.0f, 1000000f);
        Bounds1 offsetRange2 = new Bounds1(0.0f, 1000000f);
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabNetGeometryData.HasComponent(ownerPrefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          targetWidth1 = this.m_PrefabNetGeometryData[ownerPrefabRef.m_Prefab].m_ElevatedWidth - 1f;
        }
        if (a.m_Parent != Entity.Null && b.m_Parent != Entity.Null)
        {
          float3 x = (transform2.m_Position - transform1.m_Position) with
          {
            y = 0.0f
          };
          float num4 = math.length(x);
          if ((double) num4 < (double) num3)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveObject(pillar2, b, owner);
            // ISSUE: reference to a compiler-generated method
            this.AlignSingleVerticalPillar(entity, pillar1, true, owner, ownerPrefabRef, parentTransform, prefabSubObject, ref prefabRef, ref transform);
            return;
          }
          float3 float3_1 = x / num4;
          float num5 = math.dot(float3_1.xz, parentTransform.m_Position.xz - transform1.m_Position.xz);
          float num6 = math.min(num5 - targetWidth1 * 0.5f, -num1);
          float num7 = math.max(num5 + targetWidth1 * 0.5f, num4 + num2);
          float num8 = (float) (((double) num6 + (double) num7) * 0.5);
          float targetWidth2 = num7 - num6;
          offsetRange1 = new Bounds1((float2) math.abs(-num8));
          offsetRange2 = new Bounds1((float2) math.abs(num4 - num8));
          // ISSUE: reference to a compiler-generated method
          this.SelectHorizontalPillar(entity, prefabSubObject, targetWidth2, offsetRange1, offsetRange2, ref prefabRef);
          float3 float3_2 = new float3();
          float3_2.xz = MathUtils.Left(float3_1.xz);
          if ((double) math.dot(math.forward(parentTransform.m_Rotation), float3_2) < 0.0)
            float3_2 = -float3_2;
          transform.m_Position.xz = transform1.m_Position.xz + float3_1.xz * num8;
          transform.m_Rotation = quaternion.LookRotation(float3_2, math.up());
          // ISSUE: reference to a compiler-generated method
          this.AlignRotation(transform.m_Rotation, ref transform1);
          // ISSUE: reference to a compiler-generated method
          this.AlignRotation(transform.m_Rotation, ref transform2);
        }
        else if (a.m_Parent != Entity.Null)
        {
          float3 x1 = (transform1.m_Position - transform.m_Position) with
          {
            y = 0.0f
          };
          float num9 = math.length(x1);
          if ((double) num9 < (double) num3 * 0.5)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveObject(pillar2, b, owner);
            // ISSUE: reference to a compiler-generated method
            this.AlignSingleVerticalPillar(entity, pillar1, true, owner, ownerPrefabRef, parentTransform, prefabSubObject, ref prefabRef, ref transform);
            return;
          }
          float3 float3_3 = x1 / num9;
          float num10 = math.min((float) (-(double) targetWidth1 * 0.5), -num2);
          float num11 = math.max(targetWidth1 * 0.5f, num9 + num1);
          float num12 = (float) (((double) num10 + (double) num11) * 0.5);
          float targetWidth3 = num11 - num10;
          offsetRange1 = new Bounds1((float2) math.abs(num9 - num12));
          // ISSUE: reference to a compiler-generated method
          this.SelectHorizontalPillar(entity, prefabSubObject, targetWidth3, offsetRange1, offsetRange2, ref prefabRef);
          // ISSUE: reference to a compiler-generated field
          float x2 = -math.max(MathUtils.Center(this.m_PrefabPillarData[prefabRef.m_Prefab].m_OffsetRange), num3 - num9);
          float3 float3_4 = new float3();
          float3_4.xz = MathUtils.Left(float3_3.xz);
          if ((double) math.dot(math.forward(parentTransform.m_Rotation), float3_4) < 0.0)
          {
            float3_4 = -float3_4;
            x2 = -x2;
          }
          transform.m_Position.xz += float3_3.xz * num12;
          transform.m_Rotation = quaternion.LookRotation(float3_4, math.up());
          transform2.m_Rotation = transform.m_Rotation;
          transform2.m_Position = ObjectUtils.LocalToWorld(transform, new float3(x2, 0.0f, 0.0f));
          // ISSUE: reference to a compiler-generated method
          this.AlignRotation(transform.m_Rotation, ref transform1);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.SelectHorizontalPillar(entity, prefabSubObject, targetWidth1, offsetRange1, offsetRange2, ref prefabRef);
          // ISSUE: reference to a compiler-generated field
          PillarData pillarData = this.m_PrefabPillarData[prefabRef.m_Prefab];
          if ((double) pillarData.m_OffsetRange.min <= 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.RemoveObject(pillar2, b, owner);
            // ISSUE: reference to a compiler-generated method
            this.AlignSingleVerticalPillar(entity, pillar1, false, owner, ownerPrefabRef, parentTransform, prefabSubObject, ref prefabRef, ref transform);
            return;
          }
          Transform inverseParentTransform = ObjectUtils.InverseTransform(parentTransform);
          float x3 = ObjectUtils.WorldToLocal(inverseParentTransform, transform1.m_Position).x;
          float x4 = ObjectUtils.WorldToLocal(inverseParentTransform, transform2.m_Position).x;
          float num13 = math.max(MathUtils.Center(pillarData.m_OffsetRange), num3 * 0.5f);
          float x5;
          float x6;
          if ((double) x4 >= (double) x3)
          {
            x5 = -num13;
            x6 = num13;
          }
          else
          {
            x5 = num13;
            x6 = -num13;
          }
          transform1.m_Rotation = transform.m_Rotation;
          transform2.m_Rotation = transform.m_Rotation;
          transform1.m_Position = ObjectUtils.LocalToWorld(transform, new float3(x5, 0.0f, 0.0f));
          transform2.m_Position = ObjectUtils.LocalToWorld(transform, new float3(x6, 0.0f, 0.0f));
        }
        transform1.m_Position.y = transform.m_Position.y;
        transform2.m_Position.y = transform.m_Position.y;
        // ISSUE: reference to a compiler-generated method
        this.AlignHeight(pillar1, owner, prefabRef1, prefabSubObject, prefabGeometryData1, parentTransform, ref transform1);
        // ISSUE: reference to a compiler-generated method
        this.AlignHeight(pillar2, owner, prefabRef2, prefabSubObject, prefabGeometryData2, parentTransform, ref transform2);
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData[pillar1] = transform1;
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData[pillar2] = transform2;
      }

      private void SelectHorizontalPillar(
        Entity entity,
        Game.Prefabs.SubObject prefabSubObject,
        float targetWidth,
        Bounds1 offsetRange1,
        Bounds1 offsetRange2,
        ref PrefabRef prefabRef)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabPlaceholderObjects.HasBuffer(prefabSubObject.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PlaceholderObjectElement> placeholderObject = this.m_PrefabPlaceholderObjects[prefabSubObject.m_Prefab];
        float num1 = float.MinValue;
        Entity entity1 = prefabRef.m_Prefab;
        for (int index = 0; index < placeholderObject.Length; ++index)
        {
          Entity entity2 = placeholderObject[index].m_Object;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPillarData.HasComponent(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            PillarData pillarData = this.m_PrefabPillarData[entity2];
            // ISSUE: reference to a compiler-generated field
            if (pillarData.m_Type == PillarType.Horizontal && this.m_PrefabObjectGeometryData.HasComponent(entity2))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[entity2];
              float max = pillarData.m_OffsetRange.max;
              float num2 = (float) (1.0 / (1.0 + (double) math.abs(objectGeometryData.m_Size.x - targetWidth))) + (float) (0.0099999997764825821 / (1.0 + (double) math.max(0.0f, max)));
              if (!MathUtils.Intersect(pillarData.m_OffsetRange, offsetRange1))
                num2 -= 1f + math.max(offsetRange1.min - pillarData.m_OffsetRange.max, pillarData.m_OffsetRange.min - offsetRange1.max);
              if (!MathUtils.Intersect(pillarData.m_OffsetRange, offsetRange2))
                num2 -= 1f + math.max(offsetRange2.min - pillarData.m_OffsetRange.max, pillarData.m_OffsetRange.min - offsetRange2.max);
              if ((double) num2 > (double) num1)
              {
                num1 = num2;
                entity1 = entity2;
              }
            }
          }
        }
        if (!(entity1 != prefabRef.m_Prefab))
          return;
        prefabRef.m_Prefab = entity1;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(entity, prefabRef);
      }

      private bool FindVerticalPillars(
        Entity entity,
        Aligned aligned,
        Owner owner,
        bool isTemp,
        out Entity pillar1,
        out Entity pillar2)
      {
        pillar1 = Entity.Null;
        pillar2 = Entity.Null;
        if (owner.m_Owner == Entity.Null)
          return false;
        // ISSUE: reference to a compiler-generated field
        if (isTemp && !this.m_TempData.HasComponent(owner.m_Owner))
        {
          // ISSUE: reference to a compiler-generated field
          for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
          {
            // ISSUE: reference to a compiler-generated field
            ArchetypeChunk chunk = this.m_Chunks[index1];
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<Temp>(ref this.m_TempType))
            {
              // ISSUE: reference to a compiler-generated field
              NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Aligned> nativeArray2 = chunk.GetNativeArray<Aligned>(ref this.m_AlignedType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
              // ISSUE: reference to a compiler-generated field
              NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                Entity entity1 = nativeArray1[index2];
                if (!(entity1 == entity) && (int) nativeArray2[index2].m_SubObjectIndex == (int) aligned.m_SubObjectIndex && !(nativeArray3[index2].m_Owner != owner.m_Owner))
                {
                  PrefabRef prefabRef = nativeArray4[index2];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabPillarData.HasComponent(prefabRef.m_Prefab) && this.m_PrefabPillarData[prefabRef.m_Prefab].m_Type == PillarType.Vertical)
                  {
                    if (pillar1 == Entity.Null)
                      pillar1 = entity1;
                    else if (pillar2 == Entity.Null)
                      pillar2 = entity1;
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_SubObjects.HasBuffer(owner.m_Owner))
            return false;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<SubObject> subObject1 = this.m_SubObjects[owner.m_Owner];
          for (int index = 0; index < subObject1.Length; ++index)
          {
            SubObject subObject2 = subObject1[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(subObject2.m_SubObject == entity) && this.m_AlignedData.HasComponent(subObject2.m_SubObject) && (int) this.m_AlignedData[subObject2.m_SubObject].m_SubObjectIndex == (int) aligned.m_SubObjectIndex && this.m_OwnerData.HasComponent(subObject2.m_SubObject) && !(this.m_OwnerData[subObject2.m_SubObject].m_Owner != owner.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef = this.m_PrefabRefData[subObject2.m_SubObject];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabPillarData.HasComponent(prefabRef.m_Prefab) && this.m_PrefabPillarData[prefabRef.m_Prefab].m_Type == PillarType.Vertical)
              {
                if (pillar1 == Entity.Null)
                  pillar1 = subObject2.m_SubObject;
                else if (pillar2 == Entity.Null)
                  pillar2 = subObject2.m_SubObject;
              }
            }
          }
        }
        return pillar1 != Entity.Null;
      }

      private void AlignHeight(
        Entity entity,
        Owner owner,
        PrefabRef prefabRef,
        Game.Prefabs.SubObject prefabSubObject,
        ObjectGeometryData prefabGeometryData,
        Transform parentTransform,
        ref Transform transform)
      {
        if ((prefabSubObject.m_Flags & SubObjectFlags.AnchorTop) != (SubObjectFlags) 0)
        {
          transform.m_Position.y -= prefabGeometryData.m_Bounds.max.y;
          PlaceableObjectData componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPlaceableObjectData.TryGetComponent(prefabRef.m_Prefab, out componentData))
            transform.m_Position.y += componentData.m_PlacementOffset.y;
        }
        else if ((prefabSubObject.m_Flags & SubObjectFlags.AnchorCenter) != (SubObjectFlags) 0)
        {
          float num = (float) (((double) prefabGeometryData.m_Bounds.max.y - (double) prefabGeometryData.m_Bounds.min.y) * 0.5);
          transform.m_Position.y -= num;
        }
        float terrainHeight = transform.m_Position.y;
        float num1 = transform.m_Position.y;
        if ((prefabSubObject.m_Flags & SubObjectFlags.OnGround) != (SubObjectFlags) 0)
        {
          float waterHeight;
          float waterDepth;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, transform.m_Position, out terrainHeight, out waterHeight, out waterDepth);
          num1 = math.select(terrainHeight, waterHeight, (double) waterDepth >= 0.20000000298023224);
        }
        Stack componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_StackData.TryGetComponent(entity, out componentData1))
        {
          componentData1.m_Range.min = num1 - transform.m_Position.y + prefabGeometryData.m_Bounds.min.y;
          componentData1.m_Range.max = prefabGeometryData.m_Bounds.max.y;
          StackData componentData2;
          // ISSUE: reference to a compiler-generated field
          if ((double) num1 > (double) terrainHeight && this.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          {
            componentData1.m_Range.min = math.min(componentData1.m_Range.min, componentData1.m_Range.max - MathUtils.Size(componentData2.m_FirstBounds) - MathUtils.Size(componentData2.m_LastBounds));
            componentData1.m_Range.min = math.max(componentData1.m_Range.min, terrainHeight - transform.m_Position.y + prefabGeometryData.m_Bounds.min.y);
          }
          // ISSUE: reference to a compiler-generated field
          this.m_StackData[entity] = componentData1;
        }
        else
        {
          if ((prefabSubObject.m_Flags & (SubObjectFlags.AnchorTop | SubObjectFlags.AnchorCenter | SubObjectFlags.OnGround)) != SubObjectFlags.OnGround)
            return;
          transform.m_Position.y = num1;
        }
      }

      private void AlignRotation(quaternion targetRotation, ref Transform transform)
      {
        quaternion b1 = math.mul(quaternion.RotateY(1.57079637f), transform.m_Rotation);
        quaternion b2 = math.mul(quaternion.RotateY(3.14159274f), transform.m_Rotation);
        quaternion b3 = math.mul(quaternion.RotateY(-1.57079637f), transform.m_Rotation);
        float num1 = MathUtils.RotationAngle(targetRotation, transform.m_Rotation);
        float num2 = MathUtils.RotationAngle(targetRotation, b1);
        float num3 = MathUtils.RotationAngle(targetRotation, b2);
        float num4 = MathUtils.RotationAngle(targetRotation, b3);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          transform.m_Rotation = b1;
        }
        if ((double) num3 < (double) num1)
        {
          num1 = num3;
          transform.m_Rotation = b2;
        }
        if ((double) num4 >= (double) num1)
          return;
        transform.m_Rotation = b3;
      }

      private void RemoveObject(Entity entity, Attached attached, Owner owner)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(entity, in this.m_AppliedTypes);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(entity);
        // ISSUE: reference to a compiler-generated field
        if (attached.m_Parent != owner.m_Owner && this.m_SubObjects.HasBuffer(attached.m_Parent))
        {
          // ISSUE: reference to a compiler-generated field
          CollectionUtils.RemoveValue<SubObject>(this.m_SubObjects[attached.m_Parent], new SubObject(entity));
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.HasBuffer(owner.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        CollectionUtils.RemoveValue<SubObject>(this.m_SubObjects[owner.m_Owner], new SubObject(entity));
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Aligned> __Game_Objects_Aligned_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Aligned> __Game_Objects_Aligned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PillarData> __Game_Prefabs_PillarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StackData> __Game_Prefabs_StackData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<Stack> __Game_Objects_Stack_RW_ComponentLookup;
      public BufferLookup<SubObject> __Game_Objects_SubObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Aligned_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Aligned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Aligned_RO_ComponentLookup = state.GetComponentLookup<Aligned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PillarData_RO_ComponentLookup = state.GetComponentLookup<PillarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StackData_RO_ComponentLookup = state.GetComponentLookup<StackData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stack_RW_ComponentLookup = state.GetComponentLookup<Stack>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RW_BufferLookup = state.GetBufferLookup<SubObject>();
      }
    }
  }
}
