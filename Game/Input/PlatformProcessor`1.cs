// Decompiled with JetBrains decompiler
// Type: Game.Input.PlatformProcessor`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
namespace Game.Input
{
  public abstract class PlatformProcessor<TValue> : InputProcessor<TValue> where TValue : struct
  {
    public Platform m_Platform = Platform.All;
    public ProcessorDeviceType m_DeviceType;
    private bool? m_NeedProcess;

    protected bool needProcess
    {
      get
      {
        bool valueOrDefault = this.m_NeedProcess.GetValueOrDefault();
        if (this.m_NeedProcess.HasValue)
          return valueOrDefault;
        bool needProcess = this.m_Platform.IsPlatformSet(Application.platform);
        this.m_NeedProcess = new bool?(needProcess);
        return needProcess;
      }
    }
  }
}
