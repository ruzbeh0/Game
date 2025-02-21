// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.SignatureBuildingUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.City;
using Game.Prefabs;
using Game.Serialization;
using Game.Settings;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class SignatureBuildingUISystem : UISystemBase, IPreDeserialize
  {
    private const string kGroup = "signatureBuildings";
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_UnlockedSignatureBuildingQuery;
    private ValueBinding<List<Entity>> m_UnlockSignaturesBinding;
    private bool m_SkipUpdate = true;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedSignatureBuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UnlockSignaturesBinding = new ValueBinding<List<Entity>>("signatureBuildings", "unlockedSignatures", new List<Entity>(), (IWriter<List<Entity>>) new ListWriter<Entity>())));
      this.AddBinding((IBinding) new TriggerBinding("signatureBuildings", "clearUnlockedSignatures", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UnlockSignaturesBinding.value.Clear();
        // ISSUE: reference to a compiler-generated field
        this.m_UnlockSignaturesBinding.TriggerUpdate();
      })));
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SkipUpdate)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SkipUpdate = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!SharedSettings.instance.userInterface.blockingPopupsEnabled || this.m_CityConfigurationSystem.unlockAll || this.m_UnlockedSignatureBuildingQuery.IsEmptyIgnoreFilter)
          return;
        bool flag = false;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Unlock> componentDataArray = this.m_UnlockedSignatureBuildingQuery.ToComponentDataArray<Unlock>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          if (this.EntityManager.HasComponent<SignatureBuildingData>(componentDataArray[index].m_Prefab) && this.EntityManager.HasComponent<UIObjectData>(componentDataArray[index].m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UnlockSignaturesBinding.value.Insert(0, componentDataArray[index].m_Prefab);
            flag = true;
          }
        }
        componentDataArray.Dispose();
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_UnlockSignaturesBinding.TriggerUpdate();
      }
    }

    private void ClearUnlockSignatures()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockSignaturesBinding.value.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockSignaturesBinding.TriggerUpdate();
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockSignaturesBinding.value.Clear();
      // ISSUE: reference to a compiler-generated field
      this.m_SkipUpdate = false;
    }

    public void SkipUpdate() => this.m_SkipUpdate = true;

    [Preserve]
    public SignatureBuildingUISystem()
    {
    }
  }
}
