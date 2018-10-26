using UnityEngine;

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
    public GameObject gunPickup;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    LightController playerLight;
    PlayerShooting playerShooting;
    PlayerHealth playerHealth;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerLight = player.GetComponent<LightController>();
        playerShooting = player.GetComponentInChildren<PlayerShooting>();
        playerHealth = player.GetComponent<PlayerHealth>();
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

        print(Mathf.RoundToInt(startingHealth / 3).ToString());
        if (ScoreManager.score >= 150 && !playerShooting.shotgunUnlocked && !playerShooting.shotgunSpawned)
        {
            Instantiate(gunPickup, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
            playerShooting.shotgunSpawned = true;
        }
        else if (Random.Range(0, 100) <= Mathf.RoundToInt(startingHealth / 3))
        {
            if (playerShooting.rifleAmmo < 10 || playerShooting.shotgunAmmo < 4)
            {
                Instantiate(ammoPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
            }
            else if (playerHealth.currentHealth < 75)
            {
                Instantiate(healthPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
            }
            else if (playerLight.light.spotAngle < 15f)
            {
                Instantiate(lightPickup, new Vector3(transform.position.x, 0.25f, transform.position.z), Quaternion.identity);
            }
            else
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
