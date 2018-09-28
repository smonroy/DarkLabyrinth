using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    public Material pressKeyMaterial;
    public Material holdDownKeyMaterial;
    public float tapDuration;

    private float pressedSince;
    private Renderer rnd;
    private Material originalMaterial;
    private bool holdDown;
    private bool tap;

	// Use this for initialization
	void Awake () {
        rnd = GetComponent<Renderer>();
        originalMaterial = rnd.material;
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time - pressedSince >= tapDuration) {
            if(holdDown) {
                rnd.material = holdDownKeyMaterial;
                holdDown = false;
            }
            if(tap) {
                rnd.material = originalMaterial;
                tap = false;
            }
        }
	}

    public void PressDown(bool hold = false)
    {
        if (hold) {
            holdDown = true;
        } else {
            tap = true;
        }
        pressedSince = Time.time;
        rnd.material = pressKeyMaterial;
    }

    public void Release(){
        if(!tap) {
            rnd.material = originalMaterial;
            holdDown = false;
        }
    }
}
