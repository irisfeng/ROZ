using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        public float projectileSpeed; // Note other classed can set

        float damageCaused;
        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        void OnCollisionEnter(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, 0.05f);
        }
    }
}
