// Decompiled with JetBrains decompiler
// Type: Game.Reflection.ValueAccessorUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Reflection
{
  public static class ValueAccessorUtils
  {
    [CanBeNull]
    public static IValueAccessor CreateMemberAccessor(IValueAccessor parent, MemberInfo member)
    {
      FieldInfo field = member as FieldInfo;
      if ((object) field != null)
      {
        if (!(field.FieldType == typeof (NativePerThreadSumInt)))
          return (IValueAccessor) new FieldAccessor(parent, field);
        PropertyInfo property = typeof (NativePerThreadSumInt).GetProperty("Count");
        MethodInfo getMethod = property.GetGetMethod(true);
        MethodInfo setMethod = property.GetSetMethod(true);
        return (IValueAccessor) new PropertyAccessor((IValueAccessor) new ObjectAccessor<object>(field.GetValue(parent.GetValue())), getMethod, setMethod);
      }
      PropertyInfo propertyInfo = member as PropertyInfo;
      if ((object) propertyInfo != null)
      {
        MethodInfo getMethod = propertyInfo.GetGetMethod(true);
        MethodInfo setMethod = propertyInfo.GetSetMethod(true);
        return (IValueAccessor) new PropertyAccessor(parent, getMethod, setMethod);
      }
      MethodInfo getter = member as MethodInfo;
      if ((object) getter == null)
        return (IValueAccessor) null;
      ParameterInfo[] parameters1 = getter.GetParameters();
      object[] parameters2 = new object[parameters1.Length];
      int depsIndex = -1;
      for (int index = 0; index < parameters1.Length; ++index)
      {
        ParameterInfo parameterInfo = parameters1[index];
        if (parameterInfo.Name == "readOnly" && parameterInfo.ParameterType == typeof (bool))
          parameters2[index] = (object) true;
        else if (parameterInfo.ParameterType == typeof (JobHandle).MakeByRefType() && parameterInfo.IsOut)
        {
          if (depsIndex != -1)
          {
            Debug.LogWarning((object) string.Format("Found multiple JobHandle out parameters in {0}", (object) getter));
            return (IValueAccessor) null;
          }
          depsIndex = index;
          parameters2[index] = (object) new JobHandle();
        }
        else
        {
          Debug.LogWarning((object) string.Format("Unknown parameter: {0}", (object) parameterInfo));
          return (IValueAccessor) null;
        }
      }
      return (IValueAccessor) new GetterWithDepsAccessor(parent, getter, parameters2, depsIndex);
    }

    [CanBeNull]
    public static IValueAccessor CreateNativeArrayItemAccessor(IValueAccessor accessor, int index)
    {
      System.Type valueType = accessor.valueType;
      if (valueType == typeof (NativeArray<int>))
        return (IValueAccessor) new NativeArrayElementAccessor<int>((ITypedValueAccessor<NativeArray<int>>) new CastAccessor<NativeArray<int>>(accessor), index);
      if (valueType == typeof (NativeArray<int2>))
        return (IValueAccessor) new NativeArrayElementAccessor<int2>((ITypedValueAccessor<NativeArray<int2>>) new CastAccessor<NativeArray<int2>>(accessor), index);
      if (valueType == typeof (NativeArray<int3>))
        return (IValueAccessor) new NativeArrayElementAccessor<int3>((ITypedValueAccessor<NativeArray<int3>>) new CastAccessor<NativeArray<int3>>(accessor), index);
      if (valueType == typeof (NativeArray<uint>))
        return (IValueAccessor) new NativeArrayElementAccessor<uint>((ITypedValueAccessor<NativeArray<uint>>) new CastAccessor<NativeArray<uint>>(accessor), index);
      if (valueType == typeof (NativeArray<uint2>))
        return (IValueAccessor) new NativeArrayElementAccessor<uint2>((ITypedValueAccessor<NativeArray<uint2>>) new CastAccessor<NativeArray<uint2>>(accessor), index);
      if (valueType == typeof (NativeArray<uint3>))
        return (IValueAccessor) new NativeArrayElementAccessor<uint3>((ITypedValueAccessor<NativeArray<uint3>>) new CastAccessor<NativeArray<uint3>>(accessor), index);
      if (valueType == typeof (NativeArray<float>))
        return (IValueAccessor) new NativeArrayElementAccessor<float>((ITypedValueAccessor<NativeArray<float>>) new CastAccessor<NativeArray<float>>(accessor), index);
      if (valueType == typeof (NativeArray<float2>))
        return (IValueAccessor) new NativeArrayElementAccessor<float2>((ITypedValueAccessor<NativeArray<float2>>) new CastAccessor<NativeArray<float2>>(accessor), index);
      return valueType == typeof (NativeArray<float3>) ? (IValueAccessor) new NativeArrayElementAccessor<float3>((ITypedValueAccessor<NativeArray<float3>>) new CastAccessor<NativeArray<float3>>(accessor), index) : (IValueAccessor) null;
    }
  }
}
