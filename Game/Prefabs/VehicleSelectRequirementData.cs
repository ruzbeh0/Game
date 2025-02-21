// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.VehicleSelectRequirementData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct VehicleSelectRequirementData
  {
    private ComponentTypeHandle<Locked> m_LockedType;
    private BufferTypeHandle<ObjectRequirementElement> m_ObjectRequirementType;
    private ComponentLookup<ThemeData> m_ThemeData;
    private Entity m_DefaultTheme;

    public VehicleSelectRequirementData(SystemBase system)
    {
      this.m_LockedType = system.GetComponentTypeHandle<Locked>(true);
      this.m_ObjectRequirementType = system.GetBufferTypeHandle<ObjectRequirementElement>(true);
      this.m_ThemeData = system.GetComponentLookup<ThemeData>(true);
      this.m_DefaultTheme = new Entity();
    }

    public void Update(SystemBase system, CityConfigurationSystem cityConfigurationSystem)
    {
      this.m_LockedType.Update(system);
      this.m_ObjectRequirementType.Update(system);
      this.m_ThemeData.Update(system);
      this.m_DefaultTheme = cityConfigurationSystem.defaultTheme;
    }

    public VehicleSelectRequirementData.Chunk GetChunk(ArchetypeChunk chunk)
    {
      return new VehicleSelectRequirementData.Chunk()
      {
        m_LockedMask = chunk.GetEnabledMask<Locked>(ref this.m_LockedType),
        m_ObjectRequirements = chunk.GetBufferAccessor<ObjectRequirementElement>(ref this.m_ObjectRequirementType)
      };
    }

    public bool CheckRequirements(
      ref VehicleSelectRequirementData.Chunk chunk,
      int index,
      bool ignoreTheme = false)
    {
      if (chunk.m_LockedMask.EnableBit.IsValid && chunk.m_LockedMask[index])
        return false;
      if (chunk.m_ObjectRequirements.Length != 0)
      {
        DynamicBuffer<ObjectRequirementElement> objectRequirement = chunk.m_ObjectRequirements[index];
        int num = -1;
        bool flag = true;
        for (int index1 = 0; index1 < objectRequirement.Length; ++index1)
        {
          ObjectRequirementElement requirementElement = objectRequirement[index1];
          if ((int) requirementElement.m_Group != num)
          {
            if (flag)
            {
              num = (int) requirementElement.m_Group;
              flag = false;
            }
            else
              break;
          }
          flag = ((flag ? 1 : 0) | (this.m_DefaultTheme == requirementElement.m_Requirement ? 1 : (!ignoreTheme ? 0 : (this.m_ThemeData.HasComponent(requirementElement.m_Requirement) ? 1 : 0)))) != 0;
        }
        if (!flag)
          return false;
      }
      return true;
    }

    public struct Chunk
    {
      internal EnabledMask m_LockedMask;
      internal BufferAccessor<ObjectRequirementElement> m_ObjectRequirements;
    }
  }
}
