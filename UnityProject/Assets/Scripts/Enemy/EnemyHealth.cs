﻿using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    public GameObject lightPickup;
    public GameObject ammoPickup;
    public GameObject healthPickup;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    LightController playerLight;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
        playerLight = GameObject.FindGameObjectWithTag("Player").GetComponent<LightController>();
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Death");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

        playerLight.light.spotAngle += startingHealth / 100;

        if (Random.Range(0, 100) <= Mathf.RoundToInt(startingHealth / 3))
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 1:
                    Instantiate(lightPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(ammoPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(healthPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
                    break;
            }
            
        }
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
