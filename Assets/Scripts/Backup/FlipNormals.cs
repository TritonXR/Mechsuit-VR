using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlipNormals : MonoBehaviour {

  public GameObject UIGuide;

	// Use this for initialization
	void Start () {
    Mesh mesh = UIGuide.GetComponent<MeshFilter>().mesh;
    mesh.triangles = mesh.triangles.Reverse().ToArray();
  }
}
