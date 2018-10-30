using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour {

    List<GameObject> enemiesInRadius = new List<GameObject>();
    bool detonated = false;

	// Use this for initialization
	void Start () {
        StartCoroutine("Timer");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            this.GetComponent<AudioSource>().Play();
            if (!detonated)
            {
                Detonate();
            }
            detonated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            enemiesInRadius.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyHealth>() != null)
        {
            enemiesInRadius.Remove(other.gameObject);
        }
    }

    void Detonate()
    {
        StopAllCoroutines();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        GameObject[] grenades = GameObject.FindGameObjectsWithTag("Grenade");
        foreach (GameObject enemy in enemiesInRadius)
        {            
            foreach (GameObject grenade in grenades)
            {
                grenade.GetComponent<GrenadeController>().enemiesInRadius.Remove(enemy);
            }
            enemy.GetComponent<EnemyHealth>().TakeDamage(150, enemy.transform.position);
        }
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 3f);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        if (!detonated)
        {

            Detonate();
        }
        detonated = true;
    }
}
