// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.GraphicsUtilities
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
namespace Game.Rendering.Utilities
{
  public static class GraphicsUtilities
  {
    public static void GenerateRandomWindowsTexture()
    {
      byte[] collection = new byte[25]
      {
        byte.MaxValue,
        (byte) 245,
        (byte) 235,
        (byte) 224,
        (byte) 214,
        (byte) 204,
        (byte) 194,
        (byte) 184,
        (byte) 173,
        (byte) 163,
        (byte) 153,
        (byte) 143,
        (byte) 133,
        (byte) 122,
        (byte) 112,
        (byte) 102,
        (byte) 92,
        (byte) 82,
        (byte) 71,
        (byte) 61,
        (byte) 51,
        (byte) 41,
        (byte) 31,
        (byte) 20,
        (byte) 0
      };
      int num1 = 125;
      Texture2D tex = new Texture2D(num1, num1, TextureFormat.RGB24, false);
      Color32[] pixels32 = tex.GetPixels32();
      int num2 = 0;
      int num3 = 0;
      for (int index1 = 0; index1 < 625; ++index1)
      {
        List<byte> byteList = new List<byte>((IEnumerable<byte>) collection);
        Color32 color32 = (Color32) ColorUtils.NiceRandomColor();
        for (int index2 = 0; index2 < 5; ++index2)
        {
          for (int index3 = 0; index3 < 5; ++index3)
          {
            int index4 = index3 != 0 || index2 != 0 ? (index3 != 4 || index2 != 4 ? Random.Range(0, byteList.Count - 1) : byteList.Count - 1) : 0;
            pixels32[num2 + index3 + (num3 + index2) * num1] = color32;
            byteList.RemoveAt(index4);
          }
        }
        num2 += 5;
        if (num2 == num1)
        {
          num2 = 0;
          num3 += 5;
        }
      }
      tex.SetPixels32(pixels32);
      File.WriteAllBytes(Path.Combine(Application.dataPath, "Art/Resources/Textures/WindowsIdxMapGeneratedDebug.png"), tex.EncodeToPNG());
      Object.DestroyImmediate((Object) tex);
    }
  }
}
