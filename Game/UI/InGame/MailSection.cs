// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.MailSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Economy;
using Game.Prefabs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class MailSection : InfoSectionBase
  {
    protected override string group => nameof (MailSection);

    private int sortingRate { get; set; }

    private int sortingCapacity { get; set; }

    private int localAmount { get; set; }

    private int unsortedAmount { get; set; }

    private int outgoingAmount { get; set; }

    private int storedAmount { get; set; }

    private int storageCapacity { get; set; }

    private MailSection.MailKey localKey { get; set; }

    private MailSection.MailKey unsortedKey { get; set; }

    private MailSection.Type type { get; set; }

    protected override void Reset()
    {
      this.sortingRate = 0;
      this.sortingCapacity = 0;
      this.localAmount = 0;
      this.unsortedAmount = 0;
      this.outgoingAmount = 0;
      this.storedAmount = 0;
      this.storageCapacity = 0;
    }

    private bool Visible()
    {
      if (this.EntityManager.HasComponent<Game.Buildings.PostFacility>(this.selectedEntity))
        return true;
      return this.EntityManager.HasComponent<Game.Routes.MailBox>(this.selectedEntity) && this.EntityManager.HasComponent<MailBoxData>(this.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      PostFacilityData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<PostFacilityData>(this.selectedEntity, this.selectedPrefab, out data))
      {
        this.type = MailSection.Type.PostFacility;
        Game.Buildings.PostFacility componentData = this.EntityManager.GetComponentData<Game.Buildings.PostFacility>(this.selectedEntity);
        this.sortingRate = (data.m_SortingRate * (int) componentData.m_ProcessingFactor + 50) / 100;
        this.sortingCapacity = data.m_SortingRate;
        DynamicBuffer<Game.Economy.Resources> buffer = this.EntityManager.GetBuffer<Game.Economy.Resources>(this.selectedEntity, true);
        this.unsortedAmount = EconomyUtils.GetResources(Resource.UnsortedMail, buffer);
        this.localAmount = EconomyUtils.GetResources(Resource.LocalMail, buffer);
        this.outgoingAmount = EconomyUtils.GetResources(Resource.OutgoingMail, buffer);
        Game.Routes.MailBox component;
        if (this.EntityManager.TryGetComponent<Game.Routes.MailBox>(this.selectedEntity, out component))
          this.unsortedAmount += component.m_MailAmount;
        this.localKey = data.m_PostVanCapacity > 0 ? MailSection.MailKey.ToDeliver : MailSection.MailKey.Local;
        this.unsortedKey = data.m_PostVanCapacity > 0 ? MailSection.MailKey.Collected : MailSection.MailKey.Unsorted;
        this.storedAmount = this.unsortedAmount + this.localAmount + this.outgoingAmount;
        this.storageCapacity = data.m_MailCapacity;
        List<string> tooltipKeys1 = this.tooltipKeys;
        // ISSUE: variable of a compiler-generated type
        MailSection.MailKey mailKey = this.localKey;
        string str1 = mailKey.ToString();
        tooltipKeys1.Add(str1);
        if (this.sortingCapacity > 0 || this.outgoingAmount > 0)
          this.tooltipKeys.Add("Outgoing");
        List<string> tooltipKeys2 = this.tooltipKeys;
        mailKey = this.unsortedKey;
        string str2 = mailKey.ToString();
        tooltipKeys2.Add(str2);
        if (this.sortingCapacity > 0)
          this.tooltipKeys.Add("Sorting");
        if (this.storageCapacity <= 0)
          return;
        this.tooltipKeys.Add("Storage");
      }
      else
      {
        Game.Routes.MailBox component1;
        MailBoxData component2;
        if (!this.EntityManager.TryGetComponent<Game.Routes.MailBox>(this.selectedEntity, out component1) || !this.EntityManager.TryGetComponent<MailBoxData>(this.selectedPrefab, out component2))
          return;
        this.type = MailSection.Type.MailBox;
        this.storageCapacity = component2.m_MailCapacity;
        this.storedAmount = component1.m_MailAmount;
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("sortingRate");
      writer.Write(this.sortingRate);
      writer.PropertyName("sortingCapacity");
      writer.Write(this.sortingCapacity);
      writer.PropertyName("localAmount");
      writer.Write(this.localAmount);
      writer.PropertyName("unsortedAmount");
      writer.Write(this.unsortedAmount);
      writer.PropertyName("outgoingAmount");
      writer.Write(this.outgoingAmount);
      writer.PropertyName("storedAmount");
      writer.Write(this.storedAmount);
      writer.PropertyName("storageCapacity");
      writer.Write(this.storageCapacity);
      writer.PropertyName("localKey");
      writer.Write(Enum.GetName(typeof (MailSection.MailKey), (object) this.localKey));
      writer.PropertyName("unsortedKey");
      writer.Write(Enum.GetName(typeof (MailSection.MailKey), (object) this.unsortedKey));
      writer.PropertyName("type");
      writer.Write((int) this.type);
    }

    [Preserve]
    public MailSection()
    {
    }

    private enum MailKey
    {
      ToDeliver,
      Collected,
      Unsorted,
      Local,
    }

    private enum Type
    {
      PostFacility,
      MailBox,
    }
  }
}
