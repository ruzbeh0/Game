// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.Field`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public abstract class Field<T> : ReadonlyField<T>, ISettable, IWidget, IJsonWritable
  {
    private IReader<T> m_ValueReader;

    protected IReader<T> valueReader
    {
      get => this.m_ValueReader ?? (this.m_ValueReader = ValueReaders.Create<T>());
      set => this.m_ValueReader = value;
    }

    public bool shouldTriggerValueChangedEvent => true;

    public void SetValue(IJsonReader reader)
    {
      T obj;
      this.valueReader.Read(reader, out obj);
      this.SetValue(obj);
    }

    public virtual void SetValue(T value) => this.accessor.SetTypedValue(value);
  }
}
