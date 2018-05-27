using UnityEngine;

/// <summary>
/// https://unity3d.college/2017/06/17/steamvr-laser-pointer-menus/
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class VRUIItem : MonoBehaviour {
  private BoxCollider boxCollider;
  private RectTransform rectTransform;

  private void OnEnable() {
    ValidateCollider();
  }

  private void OnValidate() {
    ValidateCollider();
  }

  private void ValidateCollider() {
    rectTransform = GetComponent<RectTransform>();

    boxCollider = GetComponent<BoxCollider>();
    if (boxCollider == null) {
      boxCollider = gameObject.AddComponent<BoxCollider>();
    }

    boxCollider.size = rectTransform.sizeDelta;
  }
}