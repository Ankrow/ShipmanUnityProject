using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.2f;
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
    public int rifleAmmo = 50;
    public int shotgunAmmo = 12;
    public Text ammoText;
    public bool shotgunUnlocked = false;
    public int activeGun = 1;
    public bool shotgunSpawned = false;
    public AudioSource reload;
    private bool reloading;

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
            if (activeGun == 1)
            {
                Shoot();
            }
            else if (activeGun == 2) 
            {
                ShootShotgun();
            }
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }

        if (Input.GetKeyDown("1") && shotgunUnlocked)
        {
            activeGun = 1;
            timeBetweenBullets = 0.15f;
            damagePerShot = 20;
            ammoText.text = rifleAmmo.ToString();
        }
        if (Input.GetKeyDown("2") && shotgunUnlocked)
        {
            activeGun = 2;
            timeBetweenBullets = 0.75f;
            damagePerShot = 50;
            ammoText.text = shotgunAmmo.ToString();
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
        if (rifleAmmo > 0)
        {
            rifleAmmo--;
            ammoText.text = rifleAmmo.ToString();
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
        else if (!reloading)
        {
            StartCoroutine("Reload");
        }
    }

    void ShootShotgun()
    {
        if (shotgunAmmo > 0)
        {
            shotgunAmmo--;
            ammoText.text = shotgunAmmo.ToString();
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

    IEnumerator Reload()
    {
        reloading = true;
        reload.Play();
        yield return new WaitForSeconds(1f);
        rifleAmmo = 25;
        ammoText.text = rifleAmmo.ToString();
        reloading = false;
        yield break;
    }
}
