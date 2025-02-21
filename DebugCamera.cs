// Decompiled with JetBrains decompiler
// Type: DebugCamera
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine;

#nullable disable
public class DebugCamera : MonoBehaviour
{
  private float mainSpeed = 100f;
  private float shiftAdd = 250f;
  private float maxShift = 1000f;
  private float camSens = 0.25f;
  private Vector3 lastMouse = new Vector3((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue);
  private float totalRun = 1f;

  private void Update()
  {
    this.lastMouse = Input.mousePosition - this.lastMouse;
    this.lastMouse = new Vector3(-this.lastMouse.y * this.camSens, this.lastMouse.x * this.camSens, 0.0f);
    this.lastMouse = new Vector3(this.transform.eulerAngles.x + this.lastMouse.x, this.transform.eulerAngles.y + this.lastMouse.y, 0.0f);
    this.transform.eulerAngles = this.lastMouse;
    this.lastMouse = Input.mousePosition;
    Vector3 baseInput = this.GetBaseInput();
    Vector3 vector3;
    if (Input.GetKey(KeyCode.LeftShift))
    {
      this.totalRun += Time.deltaTime;
      vector3 = baseInput * this.totalRun * this.shiftAdd;
      vector3.x = Mathf.Clamp(vector3.x, -this.maxShift, this.maxShift);
      vector3.y = Mathf.Clamp(vector3.y, -this.maxShift, this.maxShift);
      vector3.z = Mathf.Clamp(vector3.z, -this.maxShift, this.maxShift);
    }
    else
    {
      this.totalRun = Mathf.Clamp(this.totalRun * 0.5f, 1f, 1000f);
      vector3 = baseInput * this.mainSpeed;
    }
    Vector3 translation = vector3 * Time.deltaTime;
    Vector3 position = this.transform.position;
    if (Input.GetKey(KeyCode.Space))
    {
      this.transform.Translate(translation);
      position.x = this.transform.position.x;
      position.z = this.transform.position.z;
      this.transform.position = position;
    }
    else
      this.transform.Translate(translation);
  }

  private Vector3 GetBaseInput()
  {
    Vector3 baseInput = new Vector3();
    if (Input.GetKey(KeyCode.W))
      baseInput += new Vector3(0.0f, 0.0f, 1f);
    if (Input.GetKey(KeyCode.S))
      baseInput += new Vector3(0.0f, 0.0f, -1f);
    if (Input.GetKey(KeyCode.A))
      baseInput += new Vector3(-1f, 0.0f, 0.0f);
    if (Input.GetKey(KeyCode.D))
      baseInput += new Vector3(1f, 0.0f, 0.0f);
    if (Input.GetKey(KeyCode.Q))
      baseInput += new Vector3(0.0f, -1f, 0.0f);
    if (Input.GetKey(KeyCode.E))
      baseInput += new Vector3(0.0f, 1f, 0.0f);
    return baseInput;
  }
}
