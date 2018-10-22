using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    public Transform[] lightSpawnPoints = new Transform[0];
    public GameObject lightPickup;
    private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
        coroutine = SpawnLights();
        StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnLights()
    {
        while(true)
        {
            Instantiate(lightPickup, lightSpawnPoints[Random.Range(0, 7)].position, Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }
}
