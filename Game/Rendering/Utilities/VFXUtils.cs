// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.VFXUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.VFX;

#nullable disable
namespace Game.Rendering.Utilities
{
  public static class VFXUtils
  {
    public static bool SetCheckedFloat(this VisualEffect effect, int id, float v)
    {
      if (!effect.HasFloat(id))
        return false;
      effect.SetFloat(id, v);
      return true;
    }

    public static bool SetCheckedVector3(this VisualEffect effect, int id, Vector3 v)
    {
      if (!effect.HasVector3(id))
        return false;
      effect.SetVector3(id, v);
      return true;
    }

    public static bool SetCheckedVector4(this VisualEffect effect, int id, Vector4 v)
    {
      if (!effect.HasVector4(id))
        return false;
      effect.SetVector4(id, v);
      return true;
    }

    public static bool SetCheckedTexture(this VisualEffect effect, int id, Texture v)
    {
      if (!effect.HasTexture(id))
        return false;
      effect.SetTexture(id, v);
      return true;
    }

    public static bool SetCheckedInt(this VisualEffect effect, int id, int v)
    {
      if (!effect.HasInt(id))
        return false;
      effect.SetInt(id, v);
      return true;
    }
  }
}
