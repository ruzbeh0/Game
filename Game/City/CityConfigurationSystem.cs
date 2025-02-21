// Decompiled with JetBrains decompiler
// Type: Game.City.CityConfigurationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Modding;
using Game.Net;
using Game.Prefabs;
using Game.Rendering;
using Game.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.City
{
  [CompilerGenerated]
  public class CityConfigurationSystem : 
    GameSystemBase,
    IDefaultSerializable,
    ISerializable,
    IPostDeserialize
  {
    private string m_LoadedCityName;
    private bool m_LoadedLeftHandTraffic;
    private bool m_LoadedNaturalDisasters;
    private bool m_UnlockAll;
    private bool m_LoadedUnlockAll;
    private bool m_UnlimitedMoney;
    private bool m_LoadedUnlimitedMoney;
    private bool m_UnlockMapTiles;
    private bool m_LoadedUnlockMapTiles;
    private PrefabSystem m_PrefabSystem;
    private UnlockAllSystem m_UnlockAllSystem;
    private FlipTrafficHandednessSystem m_FlipTrafficHandednessSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private EntityQuery m_ThemeQuery;
    private EntityQuery m_SubLaneQuery;
    public float3 m_CameraPivot;
    public float2 m_CameraAngle;
    public float m_CameraZoom;
    public Entity m_CameraFollow;
    private static readonly float3 kDefaultCameraPivot = new float3(0.0f, 0.0f, 0.0f);
    private static readonly float2 kDefaultCameraAngle = new float2(0.0f, 45f);
    private static readonly float kDefaultCameraZoom = 250f;
    private static readonly Entity kDefaultCameraFollow = Entity.Null;

    public string cityName { get; set; }

    public string overrideCityName { get; set; }

    [CanBeNull]
    public string overrideThemeName { get; set; }

    public Entity defaultTheme { get; set; }

    public Entity loadedDefaultTheme { get; set; }

    public bool leftHandTraffic { get; set; }

    public bool overrideLeftHandTraffic { get; set; }

    public bool naturalDisasters { get; set; }

    public bool overrideNaturalDisasters { get; set; }

    public bool unlockAll
    {
      get => this.m_LoadedUnlockAll || this.m_UnlockAll;
      set => this.m_UnlockAll = value;
    }

    public bool overrideUnlockAll { get; set; }

    public bool unlimitedMoney
    {
      get => this.m_LoadedUnlimitedMoney || this.m_UnlimitedMoney;
      set => this.m_UnlimitedMoney = value;
    }

    public bool overrideUnlimitedMoney { get; set; }

    public bool unlockMapTiles
    {
      get => this.m_LoadedUnlockMapTiles || this.m_UnlockMapTiles;
      set => this.m_UnlockMapTiles = value;
    }

    public bool overrideUnlockMapTiles { get; set; }

    public bool overrideLoadedOptions { get; set; }

    public HashSet<string> usedMods { get; private set; } = new HashSet<string>();

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockAllSystem = this.World.GetOrCreateSystemManaged<UnlockAllSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_FlipTrafficHandednessSystem = this.World.GetOrCreateSystemManaged<FlipTrafficHandednessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ThemeQuery = this.GetEntityQuery(ComponentType.ReadOnly<ThemeData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_SubLaneQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.SubLane>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.cityName = this.overrideCityName;
      this.leftHandTraffic = this.overrideLeftHandTraffic;
      this.naturalDisasters = this.overrideNaturalDisasters;
      this.unlockAll = this.overrideUnlockAll;
      this.unlimitedMoney = this.overrideUnlimitedMoney;
      this.unlockMapTiles = this.overrideUnlockMapTiles;
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockAllSystem.Enabled = this.unlockAll;
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (this.defaultTheme == Entity.Null || !string.IsNullOrEmpty(this.overrideThemeName))
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ThemeQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        try
        {
          if (this.defaultTheme == Entity.Null && entityArray.Length > 0)
            this.defaultTheme = entityArray[0];
          if (!string.IsNullOrEmpty(this.overrideThemeName))
          {
            for (int index = 0; index < entityArray.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_PrefabSystem.GetPrefab<ThemePrefab>(entityArray[index]).name == this.overrideThemeName)
              {
                this.defaultTheme = entityArray[index];
                break;
              }
            }
          }
        }
        finally
        {
          entityArray.Dispose();
          this.overrideThemeName = (string) null;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (this.leftHandTraffic != this.m_LoadedLeftHandTraffic || this.defaultTheme != this.loadedDefaultTheme)
      {
        // ISSUE: reference to a compiler-generated field
        this.EntityManager.AddComponent<Updated>(this.m_SubLaneQuery);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.leftHandTraffic != this.m_LoadedLeftHandTraffic)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_FlipTrafficHandednessSystem.Update();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.Exists(this.m_CameraFollow))
      {
        // ISSUE: reference to a compiler-generated field
        if ((Object) this.m_CameraUpdateSystem.orbitCameraController != (Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.orbitCameraController.pivot = (Vector3) this.m_CameraPivot;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.orbitCameraController.rotation = new Vector3(this.m_CameraAngle.y, this.m_CameraAngle.x, 0.0f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.orbitCameraController.zoom = this.m_CameraZoom;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.orbitCameraController.followedEntity = this.m_CameraFollow;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController;
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((Object) this.m_CameraUpdateSystem.gamePlayController != (Object) null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.gamePlayController.pivot = (Vector3) this.m_CameraPivot;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.gamePlayController.rotation = new Vector3(this.m_CameraAngle.y, this.m_CameraAngle.x, 0.0f);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.gamePlayController.zoom = this.m_CameraZoom;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.gamePlayController;
        }
      }
      string[] modsEnabled = ModManager.GetModsEnabled();
      if (modsEnabled == null)
        return;
      foreach (string str in modsEnabled)
        this.usedMods.Add(str);
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.cityName);
      writer.Write(this.defaultTheme);
      writer.Write(this.leftHandTraffic);
      writer.Write(this.naturalDisasters);
      if (writer.context.purpose == Colossal.Serialization.Entities.Purpose.SaveMap)
      {
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CameraPivot);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CameraAngle);
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.m_CameraZoom);
        // ISSUE: reference to a compiler-generated field
        writer.Write(CityConfigurationSystem.kDefaultCameraFollow);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_CameraUpdateSystem.activeCameraController != null)
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write((float3) this.m_CameraUpdateSystem.activeCameraController.pivot);
          // ISSUE: reference to a compiler-generated field
          Vector3 rotation = this.m_CameraUpdateSystem.activeCameraController.rotation;
          writer.Write(new float2(rotation.y, rotation.x));
          // ISSUE: reference to a compiler-generated field
          writer.Write(this.m_CameraUpdateSystem.activeCameraController.zoom);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.orbitCameraController)
          {
            // ISSUE: reference to a compiler-generated field
            writer.Write(this.m_CameraUpdateSystem.orbitCameraController.followedEntity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            writer.Write(CityConfigurationSystem.kDefaultCameraFollow);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          writer.Write(CityConfigurationSystem.kDefaultCameraPivot);
          // ISSUE: reference to a compiler-generated field
          writer.Write(CityConfigurationSystem.kDefaultCameraAngle);
          // ISSUE: reference to a compiler-generated field
          writer.Write(CityConfigurationSystem.kDefaultCameraZoom);
          // ISSUE: reference to a compiler-generated field
          writer.Write(CityConfigurationSystem.kDefaultCameraFollow);
        }
      }
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_UnlimitedMoney);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_UnlockAll);
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_UnlockMapTiles);
      if (writer.context.purpose == Colossal.Serialization.Entities.Purpose.SaveGame)
      {
        writer.Write(this.usedMods.Count);
        foreach (string usedMod in this.usedMods)
          writer.Write(usedMod);
      }
      else
        writer.Write(0);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.cityNameInConfig)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedCityName);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadedCityName = "";
      }
      Entity entity;
      reader.Read(out entity);
      this.loadedDefaultTheme = entity;
      if (reader.context.version >= Version.leftHandTrafficOption)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedLeftHandTraffic);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadedLeftHandTraffic = false;
      }
      if (reader.context.version >= Version.naturalDisasterOption)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedNaturalDisasters);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadedNaturalDisasters = false;
      }
      if (reader.context.version >= Version.cameraPosition)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_CameraPivot);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_CameraAngle);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_CameraZoom);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_CameraFollow);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.ResetCameraProperties();
      }
      if (reader.context.version >= Version.unlimitedMoneyAndUnlockAllOptions)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedUnlimitedMoney);
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedUnlockAll);
      }
      if (reader.context.version >= Version.unlockMapTilesOption)
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(out this.m_LoadedUnlockMapTiles);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LoadedUnlockMapTiles = false;
      }
      this.usedMods.Clear();
      if (reader.context.version >= Version.saveGameUsedMods)
      {
        int num;
        reader.Read(out num);
        this.usedMods.EnsureCapacity(num);
        for (int index = 0; index < num; ++index)
        {
          string str;
          reader.Read(out str);
          this.usedMods.Add(str);
        }
      }
      if (!this.overrideLoadedOptions)
      {
        // ISSUE: reference to a compiler-generated field
        this.cityName = this.m_LoadedCityName;
        // ISSUE: reference to a compiler-generated field
        this.naturalDisasters = this.m_LoadedNaturalDisasters;
        this.defaultTheme = this.loadedDefaultTheme;
        // ISSUE: reference to a compiler-generated field
        this.leftHandTraffic = this.m_LoadedLeftHandTraffic;
        // ISSUE: reference to a compiler-generated field
        this.unlimitedMoney = this.m_LoadedUnlimitedMoney;
        // ISSUE: reference to a compiler-generated field
        this.unlockAll = this.m_LoadedUnlockAll;
        // ISSUE: reference to a compiler-generated field
        this.unlockMapTiles = this.m_LoadedUnlockMapTiles;
      }
      else if (reader.context.purpose == Colossal.Serialization.Entities.Purpose.LoadGame)
      {
        this.defaultTheme = this.loadedDefaultTheme;
        // ISSUE: reference to a compiler-generated field
        this.leftHandTraffic = this.m_LoadedLeftHandTraffic;
      }
      this.overrideLoadedOptions = false;
    }

    private void ResetCameraProperties()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraPivot = CityConfigurationSystem.kDefaultCameraPivot;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraAngle = CityConfigurationSystem.kDefaultCameraAngle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraZoom = CityConfigurationSystem.kDefaultCameraZoom;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_CameraFollow = CityConfigurationSystem.kDefaultCameraFollow;
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedCityName = "";
      this.loadedDefaultTheme = Entity.Null;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedLeftHandTraffic = false;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedNaturalDisasters = false;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedUnlimitedMoney = false;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedUnlockAll = false;
      // ISSUE: reference to a compiler-generated field
      this.m_LoadedUnlockMapTiles = false;
      if (!this.overrideLoadedOptions)
      {
        // ISSUE: reference to a compiler-generated field
        this.cityName = this.m_LoadedCityName;
        this.defaultTheme = this.loadedDefaultTheme;
        // ISSUE: reference to a compiler-generated field
        this.leftHandTraffic = this.m_LoadedLeftHandTraffic;
        // ISSUE: reference to a compiler-generated field
        this.naturalDisasters = this.m_LoadedNaturalDisasters;
        // ISSUE: reference to a compiler-generated field
        this.unlimitedMoney = this.m_LoadedUnlimitedMoney;
        // ISSUE: reference to a compiler-generated field
        this.unlockAll = this.m_LoadedUnlockAll;
        // ISSUE: reference to a compiler-generated field
        this.unlockMapTiles = this.m_LoadedUnlockMapTiles;
      }
      this.overrideLoadedOptions = false;
      // ISSUE: reference to a compiler-generated method
      this.ResetCameraProperties();
      this.usedMods.Clear();
    }

    [Preserve]
    public CityConfigurationSystem()
    {
    }
  }
}
