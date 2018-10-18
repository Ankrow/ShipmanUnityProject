using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            if (light.spotAngle >=30 )
            {
                NavMeshAgent[] enemies = FindObjectsOfType<NavMeshAgent>();
                foreach(NavMeshAgent enemy in enemies)
                {
                    enemy.GetComponent<NavMeshAgent>().speed = 2.5f;
                }
            }
            else if (light.spotAngle < 30 && light.spotAngle > 10)
            {
                NavMeshAgent[] enemies = FindObjectsOfType<NavMeshAgent>();
                foreach (NavMeshAgent enemy in enemies)
                {
                    enemy.GetComponent<NavMeshAgent>().speed = 3.5f;
                }
            }
            else if (light.spotAngle < 10)
            {
                NavMeshAgent[] enemies = FindObjectsOfType<NavMeshAgent>();
                foreach (NavMeshAgent enemy in enemies)
                {
                    enemy.GetComponent<NavMeshAgent>().speed = 5f;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
