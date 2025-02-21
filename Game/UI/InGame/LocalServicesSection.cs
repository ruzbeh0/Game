// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LocalServicesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LocalServicesSection : InfoSectionBase
  {
    private ImageSystem m_ImageSystem;
    private EntityQuery m_ServiceDistrictBuildingQuery;

    protected override string group => nameof (LocalServicesSection);

    private NativeList<Entity> localServiceBuildings { get; set; }

    private NativeList<Entity> prefabs { get; set; }

    protected override void Reset()
    {
      this.localServiceBuildings.Clear();
      this.prefabs.Clear();
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDistrictBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<ServiceDistrict>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      this.localServiceBuildings = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.prefabs = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.localServiceBuildings.Dispose();
      this.prefabs.Dispose();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.visible = this.EntityManager.HasComponent<District>(this.selectedEntity) && this.EntityManager.HasComponent<Area>(this.selectedEntity);
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_ServiceDistrictBuildingQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      NativeArray<PrefabRef> componentDataArray = this.m_ServiceDistrictBuildingQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        for (int index1 = 0; index1 < entityArray.Length; ++index1)
        {
          DynamicBuffer<ServiceDistrict> buffer = this.EntityManager.GetBuffer<ServiceDistrict>(entityArray[index1], true);
          for (int index2 = 0; index2 < buffer.Length; ++index2)
          {
            if (buffer[index2].m_District == this.selectedEntity)
            {
              NativeList<Entity> nativeList = this.localServiceBuildings;
              nativeList.Add(entityArray[index1]);
              nativeList = this.prefabs;
              nativeList.Add(in componentDataArray[index1].m_Prefab);
              break;
            }
          }
        }
      }
      finally
      {
        entityArray.Dispose();
        componentDataArray.Dispose();
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("localServiceBuildings");
      writer.ArrayBegin(this.localServiceBuildings.Length);
      int index = 0;
      while (true)
      {
        int num = index;
        NativeList<Entity> nativeList = this.localServiceBuildings;
        int length = nativeList.Length;
        if (num < length)
        {
          writer.TypeBegin("selectedInfo.LocalServiceBuilding");
          writer.PropertyName("name");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          NameSystem nameSystem = this.m_NameSystem;
          IJsonWriter writer1 = writer;
          nativeList = this.localServiceBuildings;
          Entity entity1 = nativeList[index];
          // ISSUE: reference to a compiler-generated method
          nameSystem.BindName(writer1, entity1);
          writer.PropertyName("serviceIcon");
          IJsonWriter jsonWriter = writer;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a compiler-generated type
          ImageSystem imageSystem = this.m_ImageSystem;
          nativeList = this.prefabs;
          Entity prefabEntity = nativeList[index];
          // ISSUE: reference to a compiler-generated method
          string groupIcon = imageSystem.GetGroupIcon(prefabEntity);
          jsonWriter.Write(groupIcon);
          writer.PropertyName("entity");
          IJsonWriter writer2 = writer;
          nativeList = this.localServiceBuildings;
          Entity entity2 = nativeList[index];
          writer2.Write(entity2);
          writer.TypeEnd();
          ++index;
        }
        else
          break;
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public LocalServicesSection()
    {
    }
  }
}
