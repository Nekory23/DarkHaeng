using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats palyerStats = other.GetComponent<PlayerStats>();

            if (palyerStats != null)
            {
                palyerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            AI.EnemyStats enemyStats = other.GetComponentInParent<AI.EnemyStats>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }
    }
}
