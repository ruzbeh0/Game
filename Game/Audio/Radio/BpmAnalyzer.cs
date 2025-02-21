// Decompiled with JetBrains decompiler
// Type: Game.Audio.Radio.BpmAnalyzer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
namespace Game.Audio.Radio
{
  public class BpmAnalyzer
  {
    private static ILog log = LogManager.GetLogger("Radio");
    private const int MIN_BPM = 60;
    private const int MAX_BPM = 400;
    private const int BASE_FREQUENCY = 44100;
    private const int BASE_CHANNELS = 2;
    private const int BASE_SPLIT_SAMPLE_SIZE = 2205;
    private static BpmAnalyzer.BpmMatchData[] bpmMatchDatas = new BpmAnalyzer.BpmMatchData[341];

    public static int AnalyzeBpm(AudioClip clip)
    {
      for (int index = 0; index < BpmAnalyzer.bpmMatchDatas.Length; ++index)
        BpmAnalyzer.bpmMatchDatas[index].match = 0.0f;
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        return -1;
      BpmAnalyzer.log.InfoFormat("AnalyzeBpm audioClipName: {0}", (object) clip.name);
      int frequency = clip.frequency;
      BpmAnalyzer.log.InfoFormat("Frequency: {0}", (object) frequency);
      int channels = clip.channels;
      BpmAnalyzer.log.InfoFormat("Channels: {0}", (object) channels);
      int splitFrameSize = Mathf.FloorToInt((float) ((double) frequency / 44100.0 * ((double) channels / 2.0) * 2205.0));
      float[] numArray = new float[clip.samples * channels];
      clip.GetData(numArray, 0);
      int p1 = BpmAnalyzer.SearchBpm(BpmAnalyzer.CreateVolumeArray(numArray, frequency, channels, splitFrameSize), frequency, splitFrameSize);
      BpmAnalyzer.log.InfoFormat("Matched BPM: {0}", (object) p1);
      StringBuilder stringBuilder = new StringBuilder("BPM Match Data List\n");
      for (int index = 0; index < BpmAnalyzer.bpmMatchDatas.Length; ++index)
        stringBuilder.Append("bpm : " + BpmAnalyzer.bpmMatchDatas[index].bpm.ToString() + ", match : " + Mathf.FloorToInt(BpmAnalyzer.bpmMatchDatas[index].match * 10000f).ToString() + "\n");
      BpmAnalyzer.log.Info((object) stringBuilder.ToString());
      return p1;
    }

    private static float[] CreateVolumeArray(
      float[] allSamples,
      int frequency,
      int channels,
      int splitFrameSize)
    {
      float[] source = new float[Mathf.CeilToInt((float) allSamples.Length / (float) splitFrameSize)];
      int index1 = 0;
      for (int index2 = 0; index2 < allSamples.Length; index2 += splitFrameSize)
      {
        float num1 = 0.0f;
        for (int index3 = index2; index3 < index2 + splitFrameSize && allSamples.Length > index3; ++index3)
        {
          float num2 = Mathf.Abs(allSamples[index3]);
          if ((double) num2 <= 1.0)
            num1 += num2 * num2;
        }
        source[index1] = Mathf.Sqrt(num1 / (float) splitFrameSize);
        ++index1;
      }
      float num = ((IEnumerable<float>) source).Max();
      for (int index4 = 0; index4 < source.Length; ++index4)
        source[index4] = source[index4] / num;
      return source;
    }

    private static int SearchBpm(float[] volumeArr, int frequency, int splitFrameSize)
    {
      List<float> floatList = new List<float>();
      for (int index = 1; index < volumeArr.Length; ++index)
        floatList.Add(Mathf.Max(volumeArr[index] - volumeArr[index - 1], 0.0f));
      int index1 = 0;
      float num1 = (float) frequency / (float) splitFrameSize;
      for (int index2 = 60; index2 <= 400; ++index2)
      {
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = (float) index2 / 60f;
        if (floatList.Count > 0)
        {
          for (int index3 = 0; index3 < floatList.Count; ++index3)
          {
            num2 += floatList[index3] * Mathf.Cos((float) ((double) index3 * 2.0 * 3.1415927410125732) * num4 / num1);
            num3 += floatList[index3] * Mathf.Sin((float) ((double) index3 * 2.0 * 3.1415927410125732) * num4 / num1);
          }
          num2 *= 1f / (float) floatList.Count;
          num3 *= 1f / (float) floatList.Count;
        }
        float num5 = Mathf.Sqrt((float) ((double) num2 * (double) num2 + (double) num3 * (double) num3));
        BpmAnalyzer.bpmMatchDatas[index1].bpm = index2;
        BpmAnalyzer.bpmMatchDatas[index1].match = num5;
        ++index1;
      }
      int index4 = Array.FindIndex<BpmAnalyzer.BpmMatchData>(BpmAnalyzer.bpmMatchDatas, (Predicate<BpmAnalyzer.BpmMatchData>) (x => (double) x.match == (double) ((IEnumerable<BpmAnalyzer.BpmMatchData>) BpmAnalyzer.bpmMatchDatas).Max<BpmAnalyzer.BpmMatchData>((Func<BpmAnalyzer.BpmMatchData, float>) (y => y.match))));
      return BpmAnalyzer.bpmMatchDatas[index4].bpm;
    }

    public struct BpmMatchData
    {
      public int bpm;
      public float match;
    }
  }
}
