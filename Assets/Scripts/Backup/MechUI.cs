using UnityEngine;
using UnityEngine.EventSystems;

//https://gamedev.stackexchange.com/questions/140334/how-to-make-curved-gui-in-unity

public class MechUI : StandaloneInputModule {

  new public Camera camera;
  public RenderTexture uiTexture;

  Vector2 m_cursorPos;
  private readonly MouseState m_MouseState = new MouseState();
  protected override MouseState GetMousePointerEventData(int id = 0) {
    MouseState m = new MouseState();

    // Populate the left button...
    PointerEventData leftData;
    var created = GetPointerData(kMouseLeftId, out leftData, true);

    leftData.Reset();

    if (created)
      leftData.position = m_cursorPos;

    // Ordinarily we'd just pass the screen coordinates of the cursor through.
    //Vector2 pos = Input.mousePosition;

    // Instead, I'm going to translate that position into the latitude longitude
    // texture space used by my UI canvas:
    Vector2 trueMousePosition = Input.mousePosition;
    Vector3 ray = camera.ScreenPointToRay(trueMousePosition).direction;

    Vector2 pos;
    pos.x = uiTexture.width * (0.5f - Mathf.Atan2(ray.z, ray.x) / (2f * Mathf.PI));
    pos.y = uiTexture.height * (Mathf.Asin(ray.y) / Mathf.PI + 0.5f);
    m_cursorPos = pos;

    // For UV-mapped meshes, you could fire a ray against its MeshCollider 
    // and determine the UV coordinates of the struck point.

    leftData.delta = pos - leftData.position;
    leftData.position = pos;
    leftData.scrollDelta = Input.mouseScrollDelta;
    leftData.button = PointerEventData.InputButton.Left;
    eventSystem.RaycastAll(leftData, m_RaycastResultCache);
    var raycast = FindFirstRaycast(m_RaycastResultCache);
    leftData.pointerCurrentRaycast = raycast;
    m_RaycastResultCache.Clear();

    // copy the apropriate data into right and middle slots
    PointerEventData rightData;
    GetPointerData(kMouseRightId, out rightData, true);
    CopyFromTo(leftData, rightData);
    rightData.button = PointerEventData.InputButton.Right;

    PointerEventData middleData;
    GetPointerData(kMouseMiddleId, out middleData, true);
    CopyFromTo(leftData, middleData);
    middleData.button = PointerEventData.InputButton.Middle;

    m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
    m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
    m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

    return m_MouseState;
  }
}