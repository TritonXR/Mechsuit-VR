using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUIShader : MonoBehaviour {

  public Shader newShader;

	// Use this for initialization
	void Start () {
    Canvas.GetDefaultCanvasMaterial().shader = newShader;
  }
}
