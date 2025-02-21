// Decompiled with JetBrains decompiler
// Type: Game.InputExtensions
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine.InputSystem;

#nullable disable
namespace Game
{
  public static class InputExtensions
  {
    public static bool TryGetCompositeOfActionWithName(
      this InputAction action,
      string compositeName,
      out InputActionSetupExtensions.BindingSyntax iterator)
    {
      iterator = new InputActionSetupExtensions.BindingSyntax(action.actionMap, -1, action).NextCompositeBinding();
      while (iterator.valid && !iterator.binding.TriggersAction(action))
        iterator = iterator.NextCompositeBinding();
      while (iterator.valid && iterator.binding.TriggersAction(action) && iterator.binding.name != compositeName)
        iterator = iterator.NextCompositeBinding();
      return iterator.valid && iterator.binding.TriggersAction(action);
    }

    public static bool TryGetFirstCompositeOfAction(
      this InputAction action,
      out InputActionSetupExtensions.BindingSyntax iterator)
    {
      iterator = new InputActionSetupExtensions.BindingSyntax(action.actionMap, -1, action).NextCompositeBinding();
      while (iterator.valid && !iterator.binding.TriggersAction(action))
        iterator = iterator.NextCompositeBinding();
      return iterator.valid && iterator.binding.TriggersAction(action);
    }

    public static bool ForEachCompositeOfAction(
      this InputAction inputAction,
      InputActionSetupExtensions.BindingSyntax startIterator,
      Func<InputActionSetupExtensions.BindingSyntax, bool> action,
      out InputActionSetupExtensions.BindingSyntax endIterator)
    {
      endIterator = startIterator;
      if (action == null)
        return false;
      if (!startIterator.binding.isComposite)
        startIterator = startIterator.NextCompositeBinding();
      for (; startIterator.valid && startIterator.binding.TriggersAction(inputAction); startIterator = startIterator.NextCompositeBinding())
      {
        if (!action(startIterator))
          return false;
        endIterator = startIterator;
      }
      return true;
    }

    public static bool ForEachCompositeOfAction(
      this InputAction inputAction,
      Func<InputActionSetupExtensions.BindingSyntax, bool> action)
    {
      InputActionSetupExtensions.BindingSyntax iterator;
      return action != null && inputAction.TryGetFirstCompositeOfAction(out iterator) && inputAction.ForEachCompositeOfAction(iterator, action, out InputActionSetupExtensions.BindingSyntax _);
    }

    public static bool ForEachPartOfCompositeWithName(
      this InputAction inputAction,
      InputActionSetupExtensions.BindingSyntax startIterator,
      string partName,
      Func<InputActionSetupExtensions.BindingSyntax, bool> action,
      out InputActionSetupExtensions.BindingSyntax endIterator)
    {
      endIterator = startIterator;
      if (string.IsNullOrEmpty(partName) || action == null)
        return false;
      if (startIterator.binding.isComposite)
        startIterator = startIterator.NextPartBinding(partName);
      for (; startIterator.valid && startIterator.binding.isPartOfComposite && startIterator.binding.TriggersAction(inputAction); startIterator = startIterator.NextPartBinding(partName))
      {
        if (!action(startIterator))
          return false;
        endIterator = startIterator;
      }
      return true;
    }

    public static bool ForEachPartOfCompositeWithName(
      this InputAction inputAction,
      string partName,
      Func<InputActionSetupExtensions.BindingSyntax, bool> action)
    {
      InputActionSetupExtensions.BindingSyntax iterator;
      return action != null && inputAction.TryGetFirstCompositeOfAction(out iterator) && inputAction.ForEachPartOfCompositeWithName(iterator, partName, action, out InputActionSetupExtensions.BindingSyntax _);
    }
  }
}
