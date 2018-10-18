using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

	public Light light;
    private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
        coroutine = LightFade();
        StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update () {

		//light.spotAngle--;
	}

    IEnumerator LightFade()
    {
        while (true)
        {
            light.spotAngle -= .1f;
            yield return new WaitForSeconds(.1f);
        }
    }
}
