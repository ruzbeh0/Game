// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetCompositionMeshRefSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class NetCompositionMeshRefSystem : GameSystemBase
  {
    private NetCompositionMeshSystem m_NetCompositionMeshSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private EntityQuery m_CompositionQuery;
    private EntityArchetype m_MeshArchetype;
    private NetCompositionMeshRefSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NetCompositionMeshSystem = this.World.GetOrCreateSystemManaged<NetCompositionMeshSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompositionQuery = this.GetEntityQuery(ComponentType.ReadOnly<NetCompositionData>(), ComponentType.ReadOnly<NetCompositionPiece>(), ComponentType.ReadOnly<NetCompositionMeshRef>(), ComponentType.ReadOnly<Created>());
      // ISSUE: reference to a compiler-generated field
      this.m_MeshArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<NetCompositionMeshData>(), ComponentType.ReadWrite<NetCompositionPiece>(), ComponentType.ReadWrite<MeshMaterial>(), ComponentType.ReadWrite<BatchGroup>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompositionQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_CompositionQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshMaterial_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new NetCompositionMeshRefSystem.CompositionMeshRefJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentTypeHandle,
        m_MeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_CompositionMeshData = this.__TypeHandle.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup,
        m_CompositionPieces = this.__TypeHandle.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup,
        m_LodMeshes = this.__TypeHandle.__Game_Prefabs_LodMesh_RO_BufferLookup,
        m_MeshMaterials = this.__TypeHandle.__Game_Prefabs_MeshMaterial_RO_BufferLookup,
        m_Chunks = archetypeChunkListAsync,
        m_MeshArchetype = this.m_MeshArchetype,
        m_MeshEntities = this.m_NetCompositionMeshSystem.GetMeshEntities(out dependencies),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer()
      }.Schedule<NetCompositionMeshRefSystem.CompositionMeshRefJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetCompositionMeshSystem.AddMeshEntityReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
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
    public NetCompositionMeshRefSystem()
    {
    }

    private struct NewMeshData
    {
      public Entity m_Entity;
      public CompositionFlags m_Flags;
      public unsafe void* m_Pieces;
      public int m_PieceCount;
    }

    [BurstCompile]
    private struct CompositionMeshRefJob : IJob
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<NetCompositionData> m_CompositionType;
      [ReadOnly]
      public ComponentLookup<MeshData> m_MeshData;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> m_CompositionMeshData;
      [ReadOnly]
      public BufferLookup<NetCompositionPiece> m_CompositionPieces;
      [ReadOnly]
      public BufferLookup<LodMesh> m_LodMeshes;
      [ReadOnly]
      public BufferLookup<MeshMaterial> m_MeshMaterials;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public EntityArchetype m_MeshArchetype;
      [ReadOnly]
      public NativeParallelMultiHashMap<int, Entity> m_MeshEntities;
      public EntityCommandBuffer m_CommandBuffer;

      public unsafe void Execute()
      {
        NativeParallelMultiHashMap<int, NetCompositionMeshRefSystem.NewMeshData> newMeshes = new NativeParallelMultiHashMap<int, NetCompositionMeshRefSystem.NewMeshData>();
        NativeList<NetCompositionPiece> tempPieces = new NativeList<NetCompositionPiece>();
        NativeList<NetCompositionPiece> tempPieces2 = new NativeList<NetCompositionPiece>();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<NetCompositionData> nativeArray2 = chunk.GetNativeArray<NetCompositionData>(ref this.m_CompositionType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Entity entity1 = nativeArray1[index2];
            NetCompositionData netCompositionData = nativeArray2[index2];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionPiece> compositionPiece = this.m_CompositionPieces[entity1];
            bool hasMesh;
            // ISSUE: reference to a compiler-generated method
            int hashCode = this.GetHashCode(netCompositionData.m_Flags, compositionPiece.AsNativeArray(), out hasMesh);
            NetCompositionMeshRef compositionMeshRef;
            if (hasMesh)
            {
              Entity entity2;
              bool rotate;
              // ISSUE: reference to a compiler-generated method
              if (this.TryFindComposition(newMeshes, hashCode, netCompositionData.m_Flags, compositionPiece.GetUnsafeReadOnlyPtr(), compositionPiece.Length, ref tempPieces, out entity2, out rotate))
              {
                // ISSUE: reference to a compiler-generated field
                ref EntityCommandBuffer local = ref this.m_CommandBuffer;
                Entity e = entity1;
                compositionMeshRef = new NetCompositionMeshRef();
                compositionMeshRef.m_Mesh = entity2;
                compositionMeshRef.m_Rotate = rotate;
                NetCompositionMeshRef component = compositionMeshRef;
                local.SetComponent<NetCompositionMeshRef>(e, component);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                entity2 = this.m_CommandBuffer.CreateEntity(this.m_MeshArchetype);
                // ISSUE: reference to a compiler-generated field
                ref EntityCommandBuffer local = ref this.m_CommandBuffer;
                Entity e = entity1;
                compositionMeshRef = new NetCompositionMeshRef();
                compositionMeshRef.m_Mesh = entity2;
                NetCompositionMeshRef component = compositionMeshRef;
                local.SetComponent<NetCompositionMeshRef>(e, component);
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<NetCompositionPiece> dynamicBuffer = this.m_CommandBuffer.SetBuffer<NetCompositionPiece>(entity2);
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<MeshMaterial> materials = this.m_CommandBuffer.SetBuffer<MeshMaterial>(entity2);
                // ISSUE: reference to a compiler-generated method
                this.CopyMeshPieces(compositionPiece, dynamicBuffer);
                NetCompositionMeshData compositionMeshData = new NetCompositionMeshData()
                {
                  m_Flags = netCompositionData.m_Flags,
                  m_Width = netCompositionData.m_Width,
                  m_MiddleOffset = netCompositionData.m_MiddleOffset,
                  m_HeightRange = netCompositionData.m_HeightRange,
                  m_Hash = hashCode
                };
                // ISSUE: reference to a compiler-generated method
                this.CalculatePieceData(dynamicBuffer, materials, out compositionMeshData.m_DefaultLayers, out compositionMeshData.m_State, out compositionMeshData.m_IndexFactor, out compositionMeshData.m_LodBias, out compositionMeshData.m_ShadowBias);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<NetCompositionMeshData>(entity2, compositionMeshData);
                if (!newMeshes.IsCreated)
                  newMeshes = new NativeParallelMultiHashMap<int, NetCompositionMeshRefSystem.NewMeshData>(50, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                NetCompositionMeshRefSystem.NewMeshData newMeshData = new NetCompositionMeshRefSystem.NewMeshData()
                {
                  m_Entity = entity2,
                  m_Flags = netCompositionData.m_Flags,
                  m_Pieces = dynamicBuffer.GetUnsafePtr(),
                  m_PieceCount = dynamicBuffer.Length
                };
                newMeshes.Add(hashCode, newMeshData);
                // ISSUE: reference to a compiler-generated method
                this.InitializeLods(entity2, compositionMeshData, dynamicBuffer, newMeshes, ref tempPieces, ref tempPieces2);
              }
            }
          }
        }
        if (newMeshes.IsCreated)
          newMeshes.Dispose();
        if (tempPieces.IsCreated)
          tempPieces.Dispose();
        if (!tempPieces2.IsCreated)
          return;
        tempPieces2.Dispose();
      }

      private void CopyMeshPieces(
        DynamicBuffer<NetCompositionPiece> source,
        DynamicBuffer<NetCompositionPiece> target)
      {
        int length = 0;
        for (int index = 0; index < source.Length; ++index)
        {
          NetCompositionPiece compositionPiece = source[index];
          length += math.select(0, 1, (compositionPiece.m_PieceFlags & NetPieceFlags.HasMesh) != (NetPieceFlags) 0 && (compositionPiece.m_SectionFlags & NetSectionFlags.Hidden) == (NetSectionFlags) 0);
        }
        target.ResizeUninitialized(length);
        int num = 0;
        for (int index = 0; index < source.Length; ++index)
        {
          NetCompositionPiece compositionPiece = source[index];
          if ((compositionPiece.m_PieceFlags & NetPieceFlags.HasMesh) != (NetPieceFlags) 0 && (compositionPiece.m_SectionFlags & NetSectionFlags.Hidden) == (NetSectionFlags) 0)
            target[num++] = compositionPiece;
        }
      }

      private void CalculatePieceData(
        DynamicBuffer<NetCompositionPiece> pieces,
        DynamicBuffer<MeshMaterial> materials,
        out MeshLayer defaultLayers,
        out MeshFlags meshFlags,
        out float indexFactor,
        out float lodBias,
        out float shadowBias)
      {
        defaultLayers = (MeshLayer) 0;
        meshFlags = (MeshFlags) 0;
        indexFactor = 0.0f;
        lodBias = 0.0f;
        shadowBias = 0.0f;
        for (int index1 = 0; index1 < pieces.Length; ++index1)
        {
          NetCompositionPiece piece = pieces[index1];
          // ISSUE: reference to a compiler-generated field
          MeshData meshData = this.m_MeshData[piece.m_Piece];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<MeshMaterial> meshMaterial = this.m_MeshMaterials[piece.m_Piece];
          defaultLayers |= meshData.m_DefaultLayers;
          meshFlags |= meshData.m_State;
          indexFactor += (float) meshData.m_IndexCount / math.max(1f, MathUtils.Size(meshData.m_Bounds.z));
          lodBias += meshData.m_LodBias;
          shadowBias += meshData.m_ShadowBias;
label_8:
          for (int index2 = 0; index2 < meshMaterial.Length; ++index2)
          {
            int materialIndex = meshMaterial[index2].m_MaterialIndex;
            for (int index3 = 0; index3 < materials.Length; ++index3)
            {
              if (materials[index3].m_MaterialIndex == materialIndex)
                goto label_8;
            }
            materials.Add(new MeshMaterial()
            {
              m_MaterialIndex = materialIndex
            });
          }
        }
        if (pieces.Length == 0)
          return;
        lodBias /= (float) pieces.Length;
        shadowBias /= (float) pieces.Length;
      }

      private unsafe void InitializeLods(
        Entity mesh,
        NetCompositionMeshData meshData,
        DynamicBuffer<NetCompositionPiece> pieces,
        NativeParallelMultiHashMap<int, NetCompositionMeshRefSystem.NewMeshData> newMeshes,
        ref NativeList<NetCompositionPiece> tempPieces,
        ref NativeList<NetCompositionPiece> tempPieces2)
      {
        int num = 0;
        for (int index = 0; index < pieces.Length; ++index)
        {
          NetCompositionPiece piece = pieces[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LodMeshes.HasBuffer(piece.m_Piece))
          {
            // ISSUE: reference to a compiler-generated field
            num = math.max(num, this.m_LodMeshes[piece.m_Piece].Length);
          }
        }
        if (num == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<LodMesh> dynamicBuffer = this.m_CommandBuffer.AddBuffer<LodMesh>(mesh);
        dynamicBuffer.ResizeUninitialized(num);
        for (int index1 = 0; index1 < num; ++index1)
        {
          if (tempPieces.IsCreated)
            tempPieces.Clear();
          else
            tempPieces = new NativeList<NetCompositionPiece>(pieces.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          for (int index2 = 0; index2 < pieces.Length; ++index2)
          {
            NetCompositionPiece piece = pieces[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LodMeshes.HasBuffer(piece.m_Piece))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<LodMesh> lodMesh = this.m_LodMeshes[piece.m_Piece];
              if (index1 < lodMesh.Length)
                piece.m_Piece = lodMesh[index1].m_LodMesh;
            }
            tempPieces.Add(in piece);
          }
          bool flag;
          // ISSUE: reference to a compiler-generated method
          meshData.m_Hash = this.GetHashCode(meshData.m_Flags, tempPieces.AsArray(), out flag);
          Entity entity;
          LodMesh lodMesh1;
          // ISSUE: reference to a compiler-generated method
          if (this.TryFindComposition(newMeshes, meshData.m_Hash, meshData.m_Flags, (void*) tempPieces.GetUnsafeReadOnlyPtr<NetCompositionPiece>(), tempPieces.Length, ref tempPieces2, out entity, out flag))
          {
            ref DynamicBuffer<LodMesh> local = ref dynamicBuffer;
            int index3 = index1;
            lodMesh1 = new LodMesh();
            lodMesh1.m_LodMesh = entity;
            LodMesh lodMesh2 = lodMesh1;
            local[index3] = lodMesh2;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            entity = this.m_CommandBuffer.CreateEntity(this.m_MeshArchetype);
            ref DynamicBuffer<LodMesh> local = ref dynamicBuffer;
            int index4 = index1;
            lodMesh1 = new LodMesh();
            lodMesh1.m_LodMesh = entity;
            LodMesh lodMesh3 = lodMesh1;
            local[index4] = lodMesh3;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionPiece> pieces1 = this.m_CommandBuffer.SetBuffer<NetCompositionPiece>(entity);
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<MeshMaterial> materials = this.m_CommandBuffer.SetBuffer<MeshMaterial>(entity);
            pieces1.CopyFrom(tempPieces.AsArray());
            // ISSUE: reference to a compiler-generated method
            this.CalculatePieceData(pieces1, materials, out meshData.m_DefaultLayers, out meshData.m_State, out meshData.m_IndexFactor, out meshData.m_LodBias, out meshData.m_ShadowBias);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<NetCompositionMeshData>(entity, meshData);
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            NetCompositionMeshRefSystem.NewMeshData newMeshData = new NetCompositionMeshRefSystem.NewMeshData()
            {
              m_Entity = entity,
              m_Flags = meshData.m_Flags,
              m_Pieces = pieces1.GetUnsafePtr(),
              m_PieceCount = pieces1.Length
            };
            newMeshes.Add(meshData.m_Hash, newMeshData);
            // ISSUE: reference to a compiler-generated method
            this.InitializeLods(entity, meshData, pieces1, newMeshes, ref tempPieces, ref tempPieces2);
          }
        }
      }

      private unsafe bool TryFindComposition(
        NativeParallelMultiHashMap<int, NetCompositionMeshRefSystem.NewMeshData> newMeshes,
        int hash,
        CompositionFlags flags,
        void* pieces,
        int pieceCount,
        ref NativeList<NetCompositionPiece> tempPieces,
        out Entity entity,
        out bool rotate)
      {
        NativeParallelMultiHashMapIterator<int> it;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MeshEntities.TryGetFirstValue(hash, out entity, out it))
        {
          // ISSUE: reference to a compiler-generated field
          do
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionMeshData compositionMeshData = this.m_CompositionMeshData[entity];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionPiece> compositionPiece = this.m_CompositionPieces[entity];
            // ISSUE: reference to a compiler-generated method
            if (this.Equals(flags, compositionMeshData.m_Flags, pieces, compositionPiece.GetUnsafeReadOnlyPtr(), pieceCount, compositionPiece.Length, ref tempPieces, false))
            {
              rotate = false;
              return true;
            }
            // ISSUE: reference to a compiler-generated method
            if ((flags.m_General & CompositionFlags.General.Node) == (CompositionFlags.General) 0 && this.Equals(flags, compositionMeshData.m_Flags, pieces, compositionPiece.GetUnsafeReadOnlyPtr(), pieceCount, compositionPiece.Length, ref tempPieces, true))
            {
              rotate = true;
              return true;
            }
          }
          while (this.m_MeshEntities.TryGetNextValue(out entity, ref it));
        }
        // ISSUE: variable of a compiler-generated type
        NetCompositionMeshRefSystem.NewMeshData newMeshData;
        if (newMeshes.IsCreated && newMeshes.TryGetFirstValue(hash, out newMeshData, out it))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          while (!this.Equals(flags, newMeshData.m_Flags, pieces, newMeshData.m_Pieces, pieceCount, newMeshData.m_PieceCount, ref tempPieces, false))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            if ((flags.m_General & CompositionFlags.General.Node) == (CompositionFlags.General) 0 && this.Equals(flags, newMeshData.m_Flags, pieces, newMeshData.m_Pieces, pieceCount, newMeshData.m_PieceCount, ref tempPieces, true))
            {
              // ISSUE: reference to a compiler-generated field
              entity = newMeshData.m_Entity;
              rotate = true;
              return true;
            }
            if (!newMeshes.TryGetNextValue(out newMeshData, ref it))
              goto label_12;
          }
          // ISSUE: reference to a compiler-generated field
          entity = newMeshData.m_Entity;
          rotate = false;
          return true;
        }
label_12:
        rotate = false;
        return false;
      }

      private int GetHashCode(
        CompositionFlags flags,
        NativeArray<NetCompositionPiece> pieces,
        out bool hasMesh)
      {
        // ISSUE: reference to a compiler-generated method
        int hashCode = ((uint) this.GetCompositionFlagMask(flags)).GetHashCode();
        hasMesh = false;
        for (int index = 0; index < pieces.Length; ++index)
        {
          NetCompositionPiece piece = pieces[index];
          if ((piece.m_PieceFlags & NetPieceFlags.HasMesh) != (NetPieceFlags) 0 && (piece.m_SectionFlags & NetSectionFlags.Hidden) == (NetSectionFlags) 0)
          {
            hashCode += piece.m_Piece.GetHashCode();
            hasMesh = true;
          }
        }
        return hashCode;
      }

      private unsafe bool Equals(
        CompositionFlags flags1,
        CompositionFlags flags2,
        void* pieces1,
        void* pieces2,
        int pieceCount1,
        int pieceCount2,
        ref NativeList<NetCompositionPiece> tempPieces,
        bool rotate)
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (this.GetCompositionFlagMask(flags1) != this.GetCompositionFlagMask(flags2))
          return false;
        if (tempPieces.IsCreated)
          tempPieces.Clear();
        else
          tempPieces = new NativeList<NetCompositionPiece>(pieceCount2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < pieceCount2; ++index)
        {
          NetCompositionPiece compositionPiece = UnsafeUtility.ReadArrayElement<NetCompositionPiece>(pieces2, index);
          if ((compositionPiece.m_PieceFlags & NetPieceFlags.HasMesh) != (NetPieceFlags) 0 && (compositionPiece.m_SectionFlags & NetSectionFlags.Hidden) == (NetSectionFlags) 0)
            tempPieces.Add(in compositionPiece);
        }
        for (int index1 = 0; index1 < pieceCount1; ++index1)
        {
          NetCompositionPiece piece1 = UnsafeUtility.ReadArrayElement<NetCompositionPiece>(pieces1, index1);
          if ((piece1.m_PieceFlags & NetPieceFlags.HasMesh) != (NetPieceFlags) 0 && (piece1.m_SectionFlags & NetSectionFlags.Hidden) == (NetSectionFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            NetSectionFlags netSectionFlags1 = this.GetSectionFlagMask(piece1);
            float3 offset = piece1.m_Offset;
            bool flag = false;
            if (rotate)
            {
              if ((netSectionFlags1 & NetSectionFlags.Left) != (NetSectionFlags) 0)
                netSectionFlags1 = netSectionFlags1 & ~NetSectionFlags.Left | NetSectionFlags.Right;
              else if ((netSectionFlags1 & NetSectionFlags.Right) != (NetSectionFlags) 0)
                netSectionFlags1 = netSectionFlags1 & ~NetSectionFlags.Right | NetSectionFlags.Left;
              offset.x = -offset.x;
            }
            for (int index2 = 0; index2 < tempPieces.Length; ++index2)
            {
              NetCompositionPiece piece2 = tempPieces[index2];
              // ISSUE: reference to a compiler-generated method
              if (!(piece1.m_Piece != piece2.m_Piece) && netSectionFlags1 == this.GetSectionFlagMask(piece2) && !math.any(math.abs(offset - piece2.m_Offset) >= 0.1f))
              {
                NetPieceFlags netPieceFlags = piece1.m_PieceFlags | piece2.m_PieceFlags;
                NetSectionFlags netSectionFlags2 = piece1.m_SectionFlags ^ piece2.m_SectionFlags;
                if (!math.any(new bool2((netPieceFlags & NetPieceFlags.AsymmetricMeshX) != 0, (netPieceFlags & NetPieceFlags.AsymmetricMeshZ) != 0) & new bool2((netSectionFlags2 & NetSectionFlags.Invert) != 0, (netSectionFlags2 & NetSectionFlags.FlipMesh) != 0) != rotate))
                {
                  flag = true;
                  tempPieces.RemoveAtSwapBack(index2);
                  break;
                }
              }
            }
            if (!flag)
              return false;
          }
        }
        return tempPieces.Length == 0;
      }

      private CompositionFlags.General GetCompositionFlagMask(CompositionFlags flags)
      {
        return flags.m_General & (CompositionFlags.General.Node | CompositionFlags.General.Roundabout);
      }

      private NetSectionFlags GetSectionFlagMask(NetCompositionPiece piece)
      {
        return piece.m_SectionFlags & (NetSectionFlags.Median | NetSectionFlags.Left | NetSectionFlags.Right | NetSectionFlags.AlignCenter);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionMeshData> __Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionPiece> __Game_Prefabs_NetCompositionPiece_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LodMesh> __Game_Prefabs_LodMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshMaterial> __Game_Prefabs_MeshMaterial_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionMeshData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionMeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionPiece_RO_BufferLookup = state.GetBufferLookup<NetCompositionPiece>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LodMesh_RO_BufferLookup = state.GetBufferLookup<LodMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshMaterial_RO_BufferLookup = state.GetBufferLookup<MeshMaterial>(true);
      }
    }
  }
}
