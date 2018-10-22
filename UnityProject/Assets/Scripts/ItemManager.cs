using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public Transform[] lightSpawnPoints = new Transform[0];
    public Transform[] ammoSpawnPoints = new Transform[0];
    public GameObject lightPickup;
    public GameObject ammoPickup;
    private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
        coroutine = SpawnPickups();
        StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnPickups()
    {
        while(true)
        {
            int randInt = Random.Range(0, 2);
            if (randInt == 0)
            {
                Instantiate(lightPickup, lightSpawnPoints[Random.Range(0, lightSpawnPoints.Length - 1)].position, Quaternion.identity);
                yield return new WaitForSeconds(5f);
            }
            else if (randInt == 1)
            {
                Instantiate(ammoPickup, ammoSpawnPoints[Random.Range(0, ammoSpawnPoints.Length - 1)].position, Quaternion.identity);
                yield return new WaitForSeconds(5f);
            }
        }
    }
}
