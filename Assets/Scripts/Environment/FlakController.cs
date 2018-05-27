using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakController : MonoBehaviour {

  Animator anim;
  //int centerBlades = Animator.StringToHash("FlakCenter");
  //int middleBlades = Animator.StringToHash("FlakMiddle");
  //int outerBlades = Animator.StringToHash("FlakOuter");

  Animation centerBlades;
  Animation middleBlades;
  Animation outerBlades;

  // Use this for initialization
  void Start () {
    //anim = GetComponent<Animator>();
  }
	
	// Update is called once per frame
	void Update () {
    /*Animation.Play(centerBlades);
    Animation.Play(middleBlades);
    Animation.Play(outerBlades);
    anim.SetTrigger(centerBlades);
    anim.SetTrigger(middleBlades);
    anim.SetTrigger(outerBlades);*/
  }

  void ActivateBlades() {
    //this.animation();
  }
}