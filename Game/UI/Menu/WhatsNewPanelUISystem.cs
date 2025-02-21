// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.WhatsNewPanelUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Settings;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Menu
{
  [CompilerGenerated]
  public class WhatsNewPanelUISystem : UISystemBase
  {
    private const string kGroup = "whatsnew";
    private RawValueBinding m_PanelBinding;
    private ValueBinding<bool> m_VisibilityBinding;
    private ValueBinding<int> m_InitialTabBinding;
    private EntityQuery m_Query;
    private PrefabSystem m_PrefabSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PanelBinding = new RawValueBinding("whatsnew", "panel", (Action<IJsonWriter>) (writer =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        List<UIWhatsNewPanelPrefab> sortedWhatsNewTabs = this.GetSortedWhatsNewTabs(this.m_Query);
        writer.ArrayBegin(sortedWhatsNewTabs.Count);
        for (int index1 = 0; index1 < sortedWhatsNewTabs.Count; ++index1)
        {
          UIWhatsNewPanelPrefab whatsNewPanelPrefab = sortedWhatsNewTabs[index1];
          DlcRequirement component = whatsNewPanelPrefab.GetComponent<DlcRequirement>();
          writer.TypeBegin(typeof (UIWhatsNewPanelPrefab).FullName);
          writer.PropertyName("id");
          writer.Write(component.m_Dlc.id);
          writer.PropertyName("dlc");
          writer.Write(PlatformManager.instance.GetDlcName(component.m_Dlc));
          writer.PropertyName("pages");
          writer.ArrayBegin(whatsNewPanelPrefab.m_Pages.Length);
          for (int index2 = 0; index2 < whatsNewPanelPrefab.m_Pages.Length; ++index2)
          {
            UIWhatsNewPanelPrefab.UIWhatsNewPanelPage page = whatsNewPanelPrefab.m_Pages[index2];
            writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelPage).FullName);
            writer.PropertyName("items");
            writer.ArrayBegin(page.m_Items.Length);
            for (int index3 = 0; index3 < page.m_Items.Length; ++index3)
            {
              UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem newPanelPageItem = page.m_Items[index3];
              writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem).FullName);
              writer.PropertyName("images");
              writer.ArrayBegin(newPanelPageItem.m_Images.Length);
              for (int index4 = 0; index4 < newPanelPageItem.m_Images.Length; ++index4)
              {
                UIWhatsNewPanelPrefab.UIWhatsNewPanelImage image = newPanelPageItem.m_Images[index4];
                writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelImage).FullName);
                writer.PropertyName("image");
                writer.Write(image.m_Uri);
                writer.PropertyName("aspectRatio");
                writer.Write(image.m_AspectRatio);
                writer.PropertyName("width");
                writer.Write(image.m_Width);
                writer.TypeEnd();
              }
              writer.ArrayEnd();
              writer.PropertyName("title");
              if (newPanelPageItem.m_TitleId != null)
                writer.Write(newPanelPageItem.m_TitleId);
              else
                writer.WriteNull();
              writer.PropertyName("subtitle");
              if (newPanelPageItem.m_SubTitleId != null)
                writer.Write(newPanelPageItem.m_SubTitleId);
              else
                writer.WriteNull();
              writer.PropertyName("paragraphs");
              if (newPanelPageItem.m_ParagraphsId != null)
                writer.Write(newPanelPageItem.m_ParagraphsId);
              else
                writer.WriteNull();
              writer.PropertyName("justify");
              writer.Write((int) newPanelPageItem.m_Justify);
              writer.TypeEnd();
            }
            writer.ArrayEnd();
            writer.TypeEnd();
          }
          writer.ArrayEnd();
          writer.TypeEnd();
        }
        writer.ArrayEnd();
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_VisibilityBinding = new ValueBinding<bool>("whatsnew", "visible", false)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_InitialTabBinding = new ValueBinding<int>("whatsnew", "initialTab", 0)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("whatsnew", "close", (Action<bool>) (dismiss =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_VisibilityBinding.Update(false);
        if (!dismiss)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_Query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: object of a compiler-generated type is created
        entityArray.Sort<Entity, WhatsNewPanelUISystem.DlcComparer>(new WhatsNewPanelUISystem.DlcComparer(this.EntityManager));
        foreach (Entity entity in entityArray)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          UIWhatsNewPanelPrefab prefab = this.m_PrefabSystem.GetPrefab<UIWhatsNewPanelPrefab>(entity);
          DlcRequirement component = (UnityEngine.Object) prefab == (UnityEngine.Object) null ? (DlcRequirement) null : prefab.GetComponent<DlcRequirement>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CheckRequirement())
          {
            string dlcName = PlatformManager.instance.GetDlcName(component.m_Dlc);
            if (!SharedSettings.instance.userState.seenWhatsNew.Contains(dlcName))
              SharedSettings.instance.userState.seenWhatsNew.Add(dlcName);
          }
        }
        SharedSettings.instance.userInterface.showWhatsNewPanel = false;
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<UIWhatsNewPanelPrefabData>());
    }

    private void OnClose(bool dismiss)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_VisibilityBinding.Update(false);
      if (!dismiss)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_Query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: object of a compiler-generated type is created
      entityArray.Sort<Entity, WhatsNewPanelUISystem.DlcComparer>(new WhatsNewPanelUISystem.DlcComparer(this.EntityManager));
      foreach (Entity entity in entityArray)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        UIWhatsNewPanelPrefab prefab = this.m_PrefabSystem.GetPrefab<UIWhatsNewPanelPrefab>(entity);
        DlcRequirement component = (UnityEngine.Object) prefab == (UnityEngine.Object) null ? (DlcRequirement) null : prefab.GetComponent<DlcRequirement>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CheckRequirement())
        {
          string dlcName = PlatformManager.instance.GetDlcName(component.m_Dlc);
          if (!SharedSettings.instance.userState.seenWhatsNew.Contains(dlcName))
            SharedSettings.instance.userState.seenWhatsNew.Add(dlcName);
        }
      }
      SharedSettings.instance.userInterface.showWhatsNewPanel = false;
    }

    protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      if (mode != GameMode.MainMenu)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      List<UIWhatsNewPanelPrefab> sortedWhatsNewTabs = this.GetSortedWhatsNewTabs(this.m_Query);
      int initialTab = int.MaxValue;
      bool flag = false;
      foreach (UIWhatsNewPanelPrefab whatsNewPanelPrefab in sortedWhatsNewTabs)
      {
        DlcRequirement component = (UnityEngine.Object) whatsNewPanelPrefab == (UnityEngine.Object) null ? (DlcRequirement) null : whatsNewPanelPrefab.GetComponent<DlcRequirement>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CheckRequirement() && !SharedSettings.instance.userState.seenWhatsNew.Contains(PlatformManager.instance.GetDlcName(component.m_Dlc)))
        {
          if (component.m_Dlc.id < initialTab)
            initialTab = component.m_Dlc.id;
          flag = true;
        }
      }
      if (flag)
      {
        int index = sortedWhatsNewTabs.FindIndex((Predicate<UIWhatsNewPanelPrefab>) (p => p.GetComponent<DlcRequirement>().m_Dlc.id == initialTab));
        // ISSUE: reference to a compiler-generated field
        this.m_InitialTabBinding.Update(index >= 0 ? index : sortedWhatsNewTabs.Count - 1);
        SharedSettings.instance.userInterface.showWhatsNewPanel = true;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InitialTabBinding.Update(sortedWhatsNewTabs.Count - 1);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_PanelBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_VisibilityBinding.Update(SharedSettings.instance.userInterface.showWhatsNewPanel);
    }

    private void BindPanel(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      List<UIWhatsNewPanelPrefab> sortedWhatsNewTabs = this.GetSortedWhatsNewTabs(this.m_Query);
      writer.ArrayBegin(sortedWhatsNewTabs.Count);
      for (int index1 = 0; index1 < sortedWhatsNewTabs.Count; ++index1)
      {
        UIWhatsNewPanelPrefab whatsNewPanelPrefab = sortedWhatsNewTabs[index1];
        DlcRequirement component = whatsNewPanelPrefab.GetComponent<DlcRequirement>();
        writer.TypeBegin(typeof (UIWhatsNewPanelPrefab).FullName);
        writer.PropertyName("id");
        writer.Write(component.m_Dlc.id);
        writer.PropertyName("dlc");
        writer.Write(PlatformManager.instance.GetDlcName(component.m_Dlc));
        writer.PropertyName("pages");
        writer.ArrayBegin(whatsNewPanelPrefab.m_Pages.Length);
        for (int index2 = 0; index2 < whatsNewPanelPrefab.m_Pages.Length; ++index2)
        {
          UIWhatsNewPanelPrefab.UIWhatsNewPanelPage page = whatsNewPanelPrefab.m_Pages[index2];
          writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelPage).FullName);
          writer.PropertyName("items");
          writer.ArrayBegin(page.m_Items.Length);
          for (int index3 = 0; index3 < page.m_Items.Length; ++index3)
          {
            UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem newPanelPageItem = page.m_Items[index3];
            writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelPageItem).FullName);
            writer.PropertyName("images");
            writer.ArrayBegin(newPanelPageItem.m_Images.Length);
            for (int index4 = 0; index4 < newPanelPageItem.m_Images.Length; ++index4)
            {
              UIWhatsNewPanelPrefab.UIWhatsNewPanelImage image = newPanelPageItem.m_Images[index4];
              writer.TypeBegin(typeof (UIWhatsNewPanelPrefab.UIWhatsNewPanelImage).FullName);
              writer.PropertyName("image");
              writer.Write(image.m_Uri);
              writer.PropertyName("aspectRatio");
              writer.Write(image.m_AspectRatio);
              writer.PropertyName("width");
              writer.Write(image.m_Width);
              writer.TypeEnd();
            }
            writer.ArrayEnd();
            writer.PropertyName("title");
            if (newPanelPageItem.m_TitleId != null)
              writer.Write(newPanelPageItem.m_TitleId);
            else
              writer.WriteNull();
            writer.PropertyName("subtitle");
            if (newPanelPageItem.m_SubTitleId != null)
              writer.Write(newPanelPageItem.m_SubTitleId);
            else
              writer.WriteNull();
            writer.PropertyName("paragraphs");
            if (newPanelPageItem.m_ParagraphsId != null)
              writer.Write(newPanelPageItem.m_ParagraphsId);
            else
              writer.WriteNull();
            writer.PropertyName("justify");
            writer.Write((int) newPanelPageItem.m_Justify);
            writer.TypeEnd();
          }
          writer.ArrayEnd();
          writer.TypeEnd();
        }
        writer.ArrayEnd();
        writer.TypeEnd();
      }
      writer.ArrayEnd();
    }

    private List<UIWhatsNewPanelPrefab> GetSortedWhatsNewTabs(EntityQuery query)
    {
      NativeArray<Entity> entityArray = query.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
      // ISSUE: object of a compiler-generated type is created
      entityArray.Sort<Entity, WhatsNewPanelUISystem.DlcComparer>(new WhatsNewPanelUISystem.DlcComparer(this.EntityManager));
      List<UIWhatsNewPanelPrefab> sortedWhatsNewTabs = new List<UIWhatsNewPanelPrefab>();
      foreach (Entity entity in entityArray)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        UIWhatsNewPanelPrefab prefab = this.m_PrefabSystem.GetPrefab<UIWhatsNewPanelPrefab>(entity);
        if (PlatformManager.instance.IsDlcOwned(prefab.GetComponent<DlcRequirement>().m_Dlc))
          sortedWhatsNewTabs.Add(prefab);
      }
      return sortedWhatsNewTabs;
    }

    [Preserve]
    public WhatsNewPanelUISystem()
    {
    }

    private struct DlcComparer : IComparer<Entity>
    {
      private EntityManager m_EntityManager;

      public DlcComparer(EntityManager entityManager) => this.m_EntityManager = entityManager;

      public int Compare(Entity a, Entity b)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = this.m_EntityManager.GetComponentData<UIWhatsNewPanelPrefabData>(a).m_Id.CompareTo(this.m_EntityManager.GetComponentData<UIWhatsNewPanelPrefabData>(b).m_Id);
        return num == 0 ? a.CompareTo(b) : num;
      }
    }
  }
}
