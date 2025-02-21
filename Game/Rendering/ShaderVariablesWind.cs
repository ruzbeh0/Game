// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ShaderVariablesWind
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  [GenerateHLSL(PackingRules.Exact, true, false, false, 1, false, false, false, -1, "C:\\BuildAgent\\work\\3938c65d62942b1d\\BeverlyHills\\Assets\\Scripts\\Game\\Shaders\\HDRP\\Resources\\Includes\\ShaderVariablesWind.cs", needAccessors = false, generateCBuffer = true, constantRegister = 10)]
  internal struct ShaderVariablesWind
  {
    public Matrix4x4 _WindData_0;
    public Matrix4x4 _WindData_1;
    public Matrix4x4 _WindData_2;
    public Matrix4x4 _WindData_3;
  }
}
