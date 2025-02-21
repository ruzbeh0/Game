// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.InfoviewUISystemBase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Serialization;
using Unity.Collections;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public abstract class InfoviewUISystemBase : UISystemBase, IPreDeserialize
  {
    private UIUpdateState m_UpdateState;
    private bool m_Clear;

    public override GameMode gameMode => GameMode.Game;

    protected virtual bool Active => this.m_Clear;

    protected virtual bool Modified => false;

    protected void ResetResults<T>(NativeArray<T> results) where T : struct
    {
      for (int index = 0; index < results.Length; ++index)
        results[index] = default (T);
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UpdateState = UIUpdateState.Create(this.World, 256);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (!this.Active || !this.Modified && !this.m_UpdateState.Advance())
        return;
      this.PerformUpdate();
      this.m_Clear = false;
    }

    protected abstract void PerformUpdate();

    public void RequestUpdate() => this.m_UpdateState.ForceUpdate();

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      this.m_Clear = true;
      this.m_UpdateState.ForceUpdate();
    }

    [Preserve]
    protected InfoviewUISystemBase()
    {
    }
  }
}
