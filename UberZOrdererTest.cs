// Decompiled with JetBrains decompiler
// Type: UberZOrdererTest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase.VirtualTexturing;
using System;
using UnityEngine;

#nullable disable
public class UberZOrdererTest : MonoBehaviour
{
  private UberZOrderer m_UberZOrderer;

  private void TestSmall()
  {
    int num1 = 16;
    int maxNbTextures = num1 * num1 * 4 * 4;
    UberZOrderer uberZorderer = new UberZOrderer(512, 512 * num1, maxNbTextures, 1);
    int num2 = uberZorderer.ReserveRect(1024, 4096) == 0 ? uberZorderer.ReserveRect(8192, 8192) : throw new Exception();
    int num3 = uberZorderer.ReserveRect(8192, 8192);
    int num4 = uberZorderer.ReserveRect(8192, 8192);
    if (num2 != 1024)
      throw new Exception();
    if (num3 != 1280)
      throw new Exception();
    if (num4 != 1536)
      throw new Exception();
    int num5 = uberZorderer.ReserveRect(512, 512);
    int num6 = uberZorderer.ReserveRect(512, 512);
    if (num5 != 2048)
      throw new Exception();
    if (num6 != 2049)
      throw new Exception();
    int num7 = uberZorderer.ReserveRect(8192, 8192);
    int num8 = uberZorderer.ReserveRect(8192, 8192);
    if (num7 != 1792)
      throw new Exception();
    if (num8 != 3072)
      throw new Exception();
    if (uberZorderer.ReserveRect(512, 1024) != -1)
      throw new Exception();
  }

  private void OnEnable()
  {
    this.m_UberZOrderer = new UberZOrderer(512, 8192, 4194304, 3);
    int num1 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num2 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num3 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num4 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num5 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num6 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num7 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num8 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num9 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    int num10 = this.m_UberZOrderer.ReserveRect(1024, 4096);
    if (num1 != 0)
      throw new Exception();
    if (num2 != 4)
      throw new Exception();
    if (num3 != 16)
      throw new Exception();
    if (num4 != 20)
      throw new Exception();
    if (num5 != 64)
      throw new Exception();
    if (num6 != 68)
      throw new Exception();
    if (num7 != 80)
      throw new Exception();
    if (num8 != 84)
      throw new Exception();
    if (num9 != 128)
      throw new Exception();
    if (num10 != 132)
      throw new Exception();
    int index1 = this.m_UberZOrderer.GetIndex(0, 0);
    int index2 = this.m_UberZOrderer.GetIndex(0, 512);
    int index3 = this.m_UberZOrderer.GetIndex(0, 1024);
    int index4 = this.m_UberZOrderer.GetIndex(0, 1536);
    int index5 = this.m_UberZOrderer.GetIndex(512, 0);
    int index6 = this.m_UberZOrderer.GetIndex(512, 512);
    int index7 = this.m_UberZOrderer.GetIndex(512, 1024);
    int index8 = this.m_UberZOrderer.GetIndex(512, 1536);
    int index9 = this.m_UberZOrderer.GetIndex(1024, 0);
    int num11 = num1;
    if (index1 != num11)
      throw new Exception();
    if (index2 != num1)
      throw new Exception();
    if (index3 != num1)
      throw new Exception();
    if (index4 != num1)
      throw new Exception();
    if (index5 != num1)
      throw new Exception();
    if (index6 != num1)
      throw new Exception();
    if (index7 != num1)
      throw new Exception();
    if (index8 != num1)
      throw new Exception();
    if (index9 != num2)
      throw new Exception();
    int num12 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num13 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num14 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num15 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num16 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num17 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num18 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num19 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num20 = this.m_UberZOrderer.ReserveRect(512, 2048);
    int num21 = this.m_UberZOrderer.ReserveRect(512, 2048);
    if (num13 != num12 + 1)
      throw new Exception();
    if (num14 != num12 + 4)
      throw new Exception();
    int num22 = num12 + 5;
    if (num15 != num22)
      throw new Exception();
    if (num16 != num12 + 16)
      throw new Exception();
    if (num17 != num12 + 17)
      throw new Exception();
    if (num18 != num12 + 20)
      throw new Exception();
    if (num19 != num12 + 21)
      throw new Exception();
    if (num20 != num12 + 32)
      throw new Exception();
    if (num21 != num12 + 33)
      throw new Exception();
    int x = 65536;
    int index10 = this.m_UberZOrderer.GetIndex(x, 0);
    int index11 = this.m_UberZOrderer.GetIndex(x, 512);
    int index12 = this.m_UberZOrderer.GetIndex(x, 1024);
    int index13 = this.m_UberZOrderer.GetIndex(x, 1536);
    int index14 = this.m_UberZOrderer.GetIndex(x + 512, 0);
    int index15 = this.m_UberZOrderer.GetIndex(x + 512, 512);
    int index16 = this.m_UberZOrderer.GetIndex(x + 512, 1024);
    int index17 = this.m_UberZOrderer.GetIndex(x + 512, 1536);
    int index18 = this.m_UberZOrderer.GetIndex(x + 1024, 0);
    int num23 = num12;
    if (index10 != num23)
      throw new Exception();
    if (index11 != num12)
      throw new Exception();
    if (index12 != num12)
      throw new Exception();
    if (index13 != num12)
      throw new Exception();
    if (index14 != num13)
      throw new Exception();
    if (index15 != num13)
      throw new Exception();
    if (index16 != num13)
      throw new Exception();
    if (index17 != num13)
      throw new Exception();
    if (index18 != num14)
      throw new Exception();
    Debug.Log((object) "All UberZOrdererTests passed");
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(2048, 512));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(2048, 512));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(2048, 512));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(4096, 4096));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(4096, 4096));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(512, 512));
    Debug.Log((object) this.m_UberZOrderer.ReserveRect(512, 512));
  }

  private void Update()
  {
  }
}
