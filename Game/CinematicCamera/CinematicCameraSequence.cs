// Decompiled with JetBrains decompiler
// Type: Game.CinematicCamera.CinematicCameraSequence
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Json;
using Colossal.UI.Binding;
using Game.Rendering;
using Game.Rendering.CinematicCamera;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.CinematicCamera
{
  public class CinematicCameraSequence : IJsonWritable, IJsonReadable
  {
    private bool m_Loop;

    public List<CinematicCameraSequence.CinematicCameraCurveModifier> modifiers { get; set; } = new List<CinematicCameraSequence.CinematicCameraCurveModifier>();

    public CinematicCameraSequence.CinematicCameraCurveModifier[] transforms { get; set; } = new CinematicCameraSequence.CinematicCameraCurveModifier[5];

    public float playbackDuration { get; set; } = 30f;

    public bool loop
    {
      get => this.m_Loop;
      set
      {
        if (this.m_Loop == value)
          return;
        this.m_Loop = value;
        if (!value)
          return;
        this.AfterModifications(true);
      }
    }

    public CinematicCameraSequence() => this.Reset();

    public void Reset()
    {
      this.modifiers.Clear();
      for (int index = 0; index < this.transforms.Length; ++index)
        this.transforms[index] = new CinematicCameraSequence.CinematicCameraCurveModifier()
        {
          id = ((CinematicCameraSequence.TransformCurveKey) index).ToString(),
          curve = new AnimationCurve()
        };
    }

    public float timelineLength
    {
      get
      {
        float a1 = 0.0f;
        for (int index = 0; index < this.transforms.Length; ++index)
        {
          if (this.transforms[index].curve.length > 0)
            a1 = Mathf.Max(a1, this.transforms[index].curve[this.transforms[index].curve.length - 1].time);
        }
        for (int index1 = 0; index1 < this.modifiers.Count; ++index1)
        {
          CinematicCameraSequence.CinematicCameraCurveModifier modifier = this.modifiers[index1];
          if (modifier.curve.length > 0)
          {
            double a2 = (double) a1;
            modifier = this.modifiers[index1];
            AnimationCurve curve = modifier.curve;
            modifier = this.modifiers[index1];
            int index2 = modifier.curve.length - 1;
            double time = (double) curve[index2].time;
            a1 = Mathf.Max((float) a2, (float) time);
          }
        }
        return a1;
      }
    }

    public void RemoveModifier(string id)
    {
      int index = this.modifiers.FindIndex((Predicate<CinematicCameraSequence.CinematicCameraCurveModifier>) (m => m.id == id));
      if (index < 0)
        return;
      this.modifiers.RemoveAt(index);
    }

    public int transformCount
    {
      get
      {
        int a = 0;
        for (int index = 0; index < this.transforms.Length; ++index)
        {
          if (this.transforms[index].curve != null)
            a = Mathf.Max(a, this.transforms[index].curve.length);
        }
        return a;
      }
    }

    public bool SampleTransform(
      IGameCameraController controller,
      float t,
      out Vector3 position,
      out Vector3 rotation)
    {
      if (this.transformCount == 0)
      {
        position = Vector3.zero;
        rotation = Vector3.zero;
        return false;
      }
      position = controller.position;
      rotation = controller.rotation;
      if (this.transforms[0].curve.keys.Length != 0)
        position.x = this.transforms[0].curve.Evaluate(t);
      if (this.transforms[1].curve.keys.Length != 0)
        position.y = this.transforms[1].curve.Evaluate(t);
      if (this.transforms[2].curve.keys.Length != 0)
        position.z = this.transforms[2].curve.Evaluate(t);
      if (this.transforms[3].curve.keys.Length != 0)
        rotation.x = this.transforms[3].curve.Evaluate(t);
      if (this.transforms[4].curve.keys.Length != 0)
        rotation.y = this.transforms[4].curve.Evaluate(t);
      rotation.z = 0.0f;
      return true;
    }

    public void RemoveCameraTransform(int curveIndex, int index)
    {
      if (curveIndex >= this.transforms.Length || curveIndex < 0 || index >= this.transforms[curveIndex].curve.keys.Length || index < 0)
        return;
      if (this.transforms[curveIndex].curve.keys.Length == 1)
      {
        this.transforms[curveIndex] = new CinematicCameraSequence.CinematicCameraCurveModifier()
        {
          id = ((CinematicCameraSequence.TransformCurveKey) curveIndex).ToString(),
          curve = new AnimationCurve()
        };
      }
      else
      {
        this.transforms[curveIndex].curve.RemoveKey(index);
        this.AfterModifications(curveIndex == 4);
      }
    }

    public void RemoveModifierKey(string id, int idx)
    {
      int index = this.modifiers.FindIndex((Predicate<CinematicCameraSequence.CinematicCameraCurveModifier>) (m => m.id == id));
      if (index < 0)
        return;
      int num = idx;
      CinematicCameraSequence.CinematicCameraCurveModifier modifier = this.modifiers[index];
      int length = modifier.curve.length;
      if (num < length)
      {
        modifier = this.modifiers[index];
        modifier.curve.RemoveKey(idx);
      }
      modifier = this.modifiers[index];
      if (modifier.curve.length == 0)
        this.RemoveModifier(id);
      this.AfterModifications();
    }

    public int AddModifierKey(string id, float t, float value, float min, float max)
    {
      int index = this.modifiers.FindIndex((Predicate<CinematicCameraSequence.CinematicCameraCurveModifier>) (m => m.id == id));
      if (index >= 0)
        return this.modifiers[index].curve.AddKey(t, value);
      this.modifiers.Add(new CinematicCameraSequence.CinematicCameraCurveModifier()
      {
        curve = new AnimationCurve(new Keyframe[1]
        {
          new Keyframe(t, value)
        }),
        id = id,
        min = min,
        max = max
      });
      this.AfterModifications();
      return 0;
    }

    public int AddModifierKey(string id, float t, float value)
    {
      int index = this.modifiers.FindIndex((Predicate<CinematicCameraSequence.CinematicCameraCurveModifier>) (m => m.id == id));
      if (index >= 0)
        return this.modifiers[index].curve.AddKey(t, value);
      this.modifiers.Add(new CinematicCameraSequence.CinematicCameraCurveModifier()
      {
        curve = new AnimationCurve(new Keyframe[1]
        {
          new Keyframe(t, value)
        }),
        id = id
      });
      this.AfterModifications();
      return 0;
    }

    public void Refresh(
      float t,
      IDictionary<string, PhotoModeProperty> properties,
      IGameCameraController controller)
    {
      foreach (CinematicCameraSequence.CinematicCameraCurveModifier modifier in this.modifiers)
      {
        PhotoModeProperty photoModeProperty;
        if (properties.TryGetValue(modifier.id, out photoModeProperty))
          photoModeProperty.setValue(modifier.curve.Evaluate(t));
      }
      Vector3 position;
      Vector3 rotation;
      if (!this.SampleTransform(controller, t, out position, out rotation))
        return;
      controller.rotation = rotation;
      controller.position = position;
    }

    public int AddCameraTransform(float t, Vector3 position, Vector3 rotation)
    {
      int num = this.transforms[0].AddKey(t, position.x);
      this.transforms[1].AddKey(t, position.y);
      this.transforms[2].AddKey(t, position.z);
      this.transforms[3].AddKey(t, (double) rotation.x > 90.0 ? rotation.x - 360f : rotation.x);
      this.transforms[4].AddKey(t, rotation.y);
      this.AfterModifications(true);
      return num;
    }

    public int MoveKeyframe(
      CinematicCameraSequence.CinematicCameraCurveModifier modifier,
      int index,
      Keyframe keyframe)
    {
      bool flag = true;
      if (modifier.curve == null)
        return -1;
      AnimationCurve curve = modifier.curve;
      if (index > 0 && (double) curve[index - 1].time == (double) keyframe.time)
        flag = false;
      if (index < curve.length - 1 && (double) curve[index + 1].time == (double) keyframe.time)
        flag = false;
      if ((double) modifier.min != (double) modifier.max)
        keyframe.value = Mathf.Clamp(keyframe.value, modifier.min, modifier.max);
      if (flag)
        index = curve.MoveKey(index, keyframe);
      this.AfterModifications(modifier.id.StartsWith("Rotation"));
      return index;
    }

    public void AfterModifications(bool rotationsChanged = false)
    {
      bool flag = this.EnsureLoop();
      if (!(rotationsChanged | flag))
        return;
      this.PatchRotations();
    }

    private void PatchRotations()
    {
      for (int index = 1; index < this.transforms[4].curve.keys.Length; ++index)
      {
        float time = this.transforms[4].curve.keys[index].time;
        float num1 = this.transforms[4].curve.keys[index - 1].value;
        float num2 = (float) (((double) this.transforms[4].curve.keys[index].value - (double) num1 + 180.0) % 360.0 - 180.0);
        float num3 = (double) num2 < -180.0 ? num2 + 360f : num2;
        this.transforms[4].curve.MoveKey(index, new Keyframe(time, num1 + num3));
      }
    }

    private bool EnsureLoop()
    {
      bool flag = false;
      if (this.loop)
      {
        foreach (CinematicCameraSequence.CinematicCameraCurveModifier transform in this.transforms)
          flag |= this.EnsureLoop(transform.curve);
        foreach (CinematicCameraSequence.CinematicCameraCurveModifier modifier in this.modifiers)
          flag |= this.EnsureLoop(modifier.curve);
      }
      return flag;
    }

    private bool EnsureLoop(AnimationCurve curve)
    {
      bool flag = false;
      if (curve.keys.Length != 0)
      {
        float num = curve.Evaluate(0.0f);
        if ((double) curve.keys[0].time > 0.10000000149011612)
        {
          curve.AddKey(0.0f, num);
          flag = true;
        }
        if ((double) curve.keys[curve.keys.Length - 1].time < (double) this.playbackDuration)
        {
          flag = true;
          curve.AddKey(this.playbackDuration, num);
        }
        if ((double) curve.keys[curve.keys.Length - 1].time == (double) this.playbackDuration)
        {
          Keyframe key = curve.keys[curve.keys.Length - 1];
          flag |= (double) key.value != (double) num;
          key.time = this.playbackDuration;
          key.value = num;
          curve.MoveKey(curve.keys.Length - 1, key);
        }
      }
      return flag;
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("modifiers");
      writer.Write<CinematicCameraSequence.CinematicCameraCurveModifier>((IList<CinematicCameraSequence.CinematicCameraCurveModifier>) this.modifiers);
      writer.PropertyName("transforms");
      writer.Write<CinematicCameraSequence.CinematicCameraCurveModifier>((IList<CinematicCameraSequence.CinematicCameraCurveModifier>) this.transforms);
      writer.TypeEnd();
    }

    public void Read(IJsonReader reader)
    {
      long num = (long) reader.ReadMapBegin();
      reader.ReadProperty("modifiers");
      ulong capacity = reader.ReadArrayBegin();
      this.modifiers = new List<CinematicCameraSequence.CinematicCameraCurveModifier>((int) capacity);
      for (ulong index = 0; index < capacity; ++index)
      {
        CinematicCameraSequence.CinematicCameraCurveModifier cameraCurveModifier = new CinematicCameraSequence.CinematicCameraCurveModifier();
        cameraCurveModifier.Read(reader);
        this.modifiers.Add(cameraCurveModifier);
      }
      reader.ReadArrayEnd();
      reader.ReadProperty("transforms");
      ulong length = reader.ReadArrayBegin();
      this.transforms = new CinematicCameraSequence.CinematicCameraCurveModifier[length];
      for (ulong index = 0; index < length; ++index)
      {
        CinematicCameraSequence.CinematicCameraCurveModifier cameraCurveModifier = new CinematicCameraSequence.CinematicCameraCurveModifier();
        cameraCurveModifier.Read(reader);
        this.transforms[index] = cameraCurveModifier;
      }
      reader.ReadArrayEnd();
    }

    private static void SupportValueTypesForAOT()
    {
      JSON.SupportTypeForAOT<CinematicCameraSequence>();
      JSON.SupportTypeForAOT<CinematicCameraSequence.CinematicCameraCurveModifier>();
    }

    public enum TransformCurveKey
    {
      PositionX,
      PositionY,
      PositionZ,
      RotationX,
      RotationY,
      Count,
    }

    public struct CinematicCameraCurveModifier : IJsonWritable, IJsonReadable
    {
      public string id { get; set; }

      public AnimationCurve curve { get; set; }

      public float min { get; set; }

      public float max { get; set; }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("id");
        writer.Write(this.id);
        if (this.curve != null)
        {
          writer.PropertyName("curve");
          writer.Write(this.curve);
        }
        writer.PropertyName("min");
        writer.Write(this.min);
        writer.PropertyName("max");
        writer.Write(this.max);
        writer.TypeEnd();
      }

      public void Read(IJsonReader reader)
      {
        long num1 = (long) reader.ReadMapBegin();
        reader.ReadProperty("id");
        string str;
        reader.Read(out str);
        this.id = str;
        reader.ReadProperty("curve");
        AnimationCurve animationCurve;
        reader.Read(out animationCurve);
        this.curve = animationCurve;
        reader.ReadProperty("min");
        float num2;
        reader.Read(out num2);
        this.min = num2;
        reader.ReadProperty("max");
        float num3;
        reader.Read(out num3);
        this.max = num3;
        reader.ReadMapEnd();
      }

      public int AddKey(float t, float value)
      {
        if (this.curve == null)
        {
          AnimationCurve animationCurve;
          this.curve = animationCurve = new AnimationCurve();
        }
        return this.curve.AddKey(t, value);
      }
    }
  }
}
