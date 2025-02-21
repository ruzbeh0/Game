// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CitizenCondition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.InGame
{
  public readonly struct CitizenCondition : IJsonWritable, IComparable<CitizenCondition>
  {
    private static readonly string[] kConditionPaths = new string[7]
    {
      "Media/Game/Icons/ConditionSick.svg",
      "Media/Game/Icons/ConditionInjured.svg",
      "Media/Game/Icons/ConditionHomeless.svg",
      "Media/Game/Icons/ConditionMalcontent.svg",
      "Media/Game/Icons/ConditionWeak.svg",
      "Media/Game/Icons/ConditionInDistress.svg",
      "Media/Game/Icons/ConditionEvacuated.svg"
    };

    private CitizenConditionKey key { get; }

    public CitizenCondition(CitizenConditionKey key) => this.key = key;

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(typeof (CitizenCondition).FullName);
      writer.PropertyName("key");
      writer.Write(Enum.GetName(typeof (CitizenConditionKey), (object) this.key));
      writer.PropertyName("iconPath");
      writer.Write(CitizenCondition.kConditionPaths[(int) this.key]);
      writer.TypeEnd();
    }

    public int CompareTo(CitizenCondition other) => this.key.CompareTo((object) other.key);
  }
}
