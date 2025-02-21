// Decompiled with JetBrains decompiler
// Type: Game.UICursorCollection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using cohtml.Net;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game
{
  [CreateAssetMenu(menuName = "Colossal/UI/UICursorCollection", order = 1)]
  public class UICursorCollection : ScriptableObject
  {
    public UICursorCollection.CursorInfo m_Pointer;
    public UICursorCollection.CursorInfo m_Text;
    public UICursorCollection.CursorInfo m_Move;
    public UICursorCollection.NamedCursorInfo[] m_NamedCursors;
    private Dictionary<string, UICursorCollection.CursorInfo> m_NamedCursorsDict;

    private void OnEnable()
    {
      if (this.m_NamedCursors == null)
        this.m_NamedCursors = new UICursorCollection.NamedCursorInfo[0];
      this.m_NamedCursorsDict = new Dictionary<string, UICursorCollection.CursorInfo>();
      this.RefreshNamedCursorsDict();
    }

    public void SetCursor(Cursors cursor)
    {
      switch (cursor)
      {
        case Cursors.Move:
          this.m_Move.Apply();
          break;
        case Cursors.Pointer:
          this.m_Pointer.Apply();
          break;
        case Cursors.Text:
          this.m_Text.Apply();
          break;
        default:
          UICursorCollection.ResetCursor();
          break;
      }
    }

    public void SetCursor(string cursorName)
    {
      UICursorCollection.CursorInfo cursorInfo;
      if (this.m_NamedCursorsDict.TryGetValue(cursorName, out cursorInfo))
        cursorInfo.Apply();
      else
        UICursorCollection.ResetCursor();
    }

    public static void ResetCursor()
    {
      Cursor.SetCursor((Texture2D) null, Vector2.zero, CursorMode.Auto);
    }

    private void RefreshNamedCursorsDict()
    {
      this.m_NamedCursorsDict.Clear();
      foreach (UICursorCollection.NamedCursorInfo namedCursor in this.m_NamedCursors)
        this.m_NamedCursorsDict["cursor://" + namedCursor.m_Name] = (UICursorCollection.CursorInfo) namedCursor;
    }

    [Serializable]
    public class CursorInfo
    {
      public Texture2D m_Texture;
      public Vector2 m_Hotspot;

      public void Apply() => Cursor.SetCursor(this.m_Texture, this.m_Hotspot, CursorMode.Auto);
    }

    [Serializable]
    public class NamedCursorInfo : UICursorCollection.CursorInfo
    {
      public string m_Name;
    }
  }
}
