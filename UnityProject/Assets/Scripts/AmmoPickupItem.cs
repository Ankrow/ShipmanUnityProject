using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponentInChildren<PlayerShooting>();
            if (playerShooting.activeGun == 1)
            {
                playerShooting.rifleAmmo += 25;
            }
            else if (playerShooting.activeGun == 2)
            {
                playerShooting.shotgunAmmo += 6;
            }
            
            Destroy(gameObject);
        }
    }
}
