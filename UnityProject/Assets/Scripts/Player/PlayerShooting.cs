using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 1.2f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    public LineRenderer[] gunLines = new LineRenderer[8];
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    public int ammo = 100;
    public Text ammoText;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            ShootShotgun();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        foreach (LineRenderer line in gunLines)
        {
            line.enabled = false;
        }
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        if (ammo > 0)
        {
            ammo--;
            ammoText.text = ammo.ToString();
            timer = 0f;

            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            gunLines[0].enabled = true;
            gunLines[0].SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
                gunLines[0].SetPosition(1, shootHit.point);
            }
            else
            {
                gunLines[0].SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }
    }

    void ShootShotgun()
    {
        if (ammo > 0)
        {
            ammo--;
            ammoText.text = ammo.ToString();
            timer = 0f;

            gunAudio.Play();            

            for (int i = 0; i < 8; i++)
            {
                gunLight.enabled = true;

                gunParticles.Stop();
                gunParticles.Play();

                gunLines[i].enabled = true;
                gunLines[i].SetPosition(0, transform.position);

                shootRay.origin = transform.position;

                float x = Random.Range(-0.25f, 0.25f);
                float y = Random.Range(-0.1f, 0.1f);
                float z = Random.Range(-0.25f, 0.25f);
                shootRay.direction = transform.forward + new Vector3(x, y, z);

                if (Physics.Raycast(shootRay, out shootHit, 7f, shootableMask))
                {
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                    }
                    gunLines[i].SetPosition(1, shootHit.point);
                }
                else
                {
                    gunLines[i].SetPosition(1, shootRay.origin + shootRay.direction * 7f);
                }
            }           
        }
    }
}
