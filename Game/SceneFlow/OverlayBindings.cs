// Decompiled with JetBrains decompiler
// Type: Game.SceneFlow.OverlayBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Game.SceneFlow
{
  public class OverlayBindings : CompositeBinding
  {
    private const string kGroup = "overlay";
    private readonly ValueBinding<OverlayScreen> m_ActiveScreen;
    private readonly ValueBinding<float[]> m_Progress;
    private readonly ValueBinding<string[]> m_HintMessages;
    private readonly ValueBinding<string[]> m_CorruptDataMessages;
    private readonly SortedSet<OverlayScreen> m_ActiveScreenList = new SortedSet<OverlayScreen>((IComparer<OverlayScreen>) new OverlayScreenComparer());

    public event Action<OverlayScreen> onScreenActivated;

    public OverlayScreen currentlyActiveScreen
    {
      get => this.m_ActiveScreenList.FirstOrDefault<OverlayScreen>();
    }

    public OverlayBindings()
    {
      this.AddBinding((IBinding) (this.m_ActiveScreen = new ValueBinding<OverlayScreen>("overlay", "activeScreen", OverlayScreen.None, (IWriter<OverlayScreen>) new DelegateWriter<OverlayScreen>((WriterDelegate<OverlayScreen>) ((writer, value) => writer.Write((int) value))))));
      this.AddBinding((IBinding) (this.m_Progress = new ValueBinding<float[]>("overlay", "progress", new float[3], (IWriter<float[]>) new ArrayWriter<float>())));
      this.AddBinding((IBinding) (this.m_HintMessages = new ValueBinding<string[]>("overlay", nameof (hintMessages), Array.Empty<string>(), (IWriter<string[]>) new ArrayWriter<string>())));
      this.AddBinding((IBinding) (this.m_CorruptDataMessages = new ValueBinding<string[]>("overlay", nameof (corruptDataMessages), (string[]) null, (IWriter<string[]>) new ArrayWriter<string>().Nullable<string[]>())));
    }

    public OverlayBindings.ScopedScreen ActivateScreenScoped(OverlayScreen screen)
    {
      return new OverlayBindings.ScopedScreen(screen, this);
    }

    private void UpdateScreen()
    {
      OverlayScreen overlayScreen = this.m_ActiveScreenList.FirstOrDefault<OverlayScreen>();
      this.m_ActiveScreen.Update(overlayScreen);
      CompositeBinding.log.DebugFormat("Screen changed to {0}", (object) overlayScreen);
      Action<OverlayScreen> onScreenActivated = this.onScreenActivated;
      if (onScreenActivated == null)
        return;
      onScreenActivated(overlayScreen);
    }

    public void ActivateScreen(OverlayScreen screen)
    {
      this.m_ActiveScreenList.Add(screen);
      this.UpdateScreen();
    }

    public void DeactivateScreen(OverlayScreen screen)
    {
      this.m_ActiveScreenList.Remove(screen);
      this.UpdateScreen();
    }

    public void SwapScreen(OverlayScreen screen1, OverlayScreen screen2)
    {
      this.DeactivateScreen(screen1);
      this.ActivateScreen(screen2);
    }

    public void DeactivateAllScreens()
    {
      this.m_ActiveScreenList.Clear();
      this.UpdateScreen();
    }

    public float GetProgress(OverlayProgressType type) => this.m_Progress.value[(int) type];

    public void SetProgress(OverlayProgressType type, float progress)
    {
      if ((double) this.m_Progress.value[(int) type] == (double) progress)
        return;
      this.m_Progress.value[(int) type] = progress;
      this.m_Progress.TriggerUpdate();
    }

    public string[] hintMessages
    {
      get => this.m_HintMessages.value;
      set => this.m_HintMessages.Update(value);
    }

    public string[] corruptDataMessages
    {
      get => this.m_CorruptDataMessages.value;
      set => this.m_CorruptDataMessages.Update(value);
    }

    public struct ScopedScreen : IDisposable
    {
      private OverlayBindings bindings;
      private OverlayScreen screen;

      public ScopedScreen(OverlayScreen screen, OverlayBindings bindings)
      {
        this.screen = screen;
        this.bindings = bindings;
        this.bindings.ActivateScreen(screen);
      }

      public void Dispose() => this.bindings.DeactivateScreen(this.screen);
    }
  }
}
