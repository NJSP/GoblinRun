using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public string playerTag = "Player"; // Ensure the player is tagged correctly
    [Serialize] public GameObject enemyWeapon;
    public AudioClip hitSound;
    private Collider attackCollider;
    public GameObject hitEffectPrefab;
    public Transform hitPoint;


    private void Start()
    {
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public void EnableAttack()
    {
        attackCollider.enabled = true;
        Debug.Log("Enemy attack enabled");
    }

    public void DisableAttack() 
    {
        attackCollider.enabled = false;
        Debug.Log("Enemy attack disabled");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            HealthComponent playerHealth = other.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
                Debug.Log("Player hit by enemy attack!");
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
                DisableAttack();
            }
        }
    }
}
