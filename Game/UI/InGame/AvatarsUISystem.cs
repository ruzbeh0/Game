// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.AvatarsUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Companies;
using Game.Prefabs;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class AvatarsUISystem : UISystemBase
  {
    private const string kGroup = "avatars";
    private const int kIconSize = 32;
    private PrefabSystem m_PrefabSystem;
    private NameSystem m_NameSystem;
    private EntityQuery m_ColorsQuery;
    [UsedImplicitly]
    private RawMapBinding<Entity> m_AvatarsBinding;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NameSystem = this.World.GetOrCreateSystemManaged<NameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ColorsQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIAvatarColorData>());
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvatarsBinding = new RawMapBinding<Entity>("avatars", "avatarsMap", (Action<IJsonWriter, Entity>) ((writer, entity) =>
      {
        writer.TypeBegin("avatars.AvatarData");
        writer.PropertyName("picture");
        // ISSUE: reference to a compiler-generated method
        writer.Write(this.GetPicture(entity));
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, entity);
        // ISSUE: reference to a compiler-generated method
        Color32 color = this.GetColor(entity);
        writer.PropertyName("color");
        writer.Write(color);
        writer.TypeEnd();
      }))));
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    private void BindAvatar(IJsonWriter writer, Entity entity)
    {
      writer.TypeBegin("avatars.AvatarData");
      writer.PropertyName("picture");
      // ISSUE: reference to a compiler-generated method
      writer.Write(this.GetPicture(entity));
      writer.PropertyName("name");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindName(writer, entity);
      // ISSUE: reference to a compiler-generated method
      Color32 color = this.GetColor(entity);
      writer.PropertyName("color");
      writer.Write(color);
      writer.TypeEnd();
    }

    private Color32 GetColor(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<UIAvatarColorData> singletonBuffer = this.m_ColorsQuery.GetSingletonBuffer<UIAvatarColorData>();
      // ISSUE: reference to a compiler-generated method
      int randomIndex = this.GetRandomIndex(entity);
      return randomIndex <= 0 ? (Color32) Color.white : singletonBuffer[randomIndex % singletonBuffer.Length].m_Color;
    }

    [Colossal.Annotations.CanBeNull]
    private string GetPicture(Entity entity)
    {
      CompanyData component1;
      if (this.EntityManager.TryGetComponent<CompanyData>(entity, out component1))
        entity = component1.m_Brand;
      PrefabData component2;
      PrefabBase prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.EntityManager.TryGetComponent<PrefabData>(entity, out component2) && this.m_PrefabSystem.TryGetPrefab<PrefabBase>(component2, out prefab))
      {
        // ISSUE: reference to a compiler-generated method
        string icon = ImageSystem.GetIcon(prefab);
        if (icon != null)
          return icon;
        switch (prefab)
        {
          case ChirperAccount chirperAccount when (UnityEngine.Object) chirperAccount.m_InfoView != (UnityEngine.Object) null && chirperAccount.m_InfoView.m_IconPath != null:
            return chirperAccount.m_InfoView.m_IconPath;
          case BrandPrefab brandPrefab:
            return string.Format("{0}?width={1}&height={2}", (object) brandPrefab.thumbnailUrl, (object) 32, (object) 32);
        }
      }
      return (string) null;
    }

    private int GetRandomIndex(Entity entity)
    {
      DynamicBuffer<RandomLocalizationIndex> buffer;
      return this.EntityManager.TryGetBuffer<RandomLocalizationIndex>(entity, true, out buffer) && buffer.Length > 0 ? buffer[0].m_Index : 0;
    }

    [Preserve]
    public AvatarsUISystem()
    {
    }
  }
}
