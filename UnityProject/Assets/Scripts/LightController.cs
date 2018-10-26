using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightController : MonoBehaviour {

	public Light light;
    private IEnumerator coroutine;
    private float timer = 0;
    private float darknessSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        coroutine = LightFade();
        StartCoroutine(coroutine);
	}
	
	// Update is called once per frame
	void Update () {

        //light.spotAngle--;
        timer += Time.deltaTime;
    }

    IEnumerator LightFade()
    {
        while (true)
        {
            if(light.spotAngle < 1.5f)
            {
                //print("boop");
                GetComponent<PlayerHealth>().TakeDamage(1000);
                break;
            }
            light.spotAngle -= 0.1f;

            if (timer > 30f && timer < 60f)
            {
                darknessSpeed = 0.075f;
            }
            else if (timer > 60f && timer < 120f)
            {
                darknessSpeed = 0.05f;
            }
            else if (timer > 120f)
            {
                darknessSpeed = 0.025f;
            }

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
            yield return new WaitForSeconds(darknessSpeed);
        }
    }
}
