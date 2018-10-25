using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShooting playerShooting;


    Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
        }
        
        if (playerShooting.shotgunUnlocked)
        {
            anim.SetTrigger("GunPickup");
        }
    }
}
