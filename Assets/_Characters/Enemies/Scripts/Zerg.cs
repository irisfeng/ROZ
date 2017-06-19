using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using RPG.Core;

namespace RPG.Characters
{
    public class Zerg : MonoBehaviour, IDamageable
    {


        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float attackRadius = 3f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damagePerHit = 50f;
        [SerializeField] float disappearSpeed = 0.5f;

        private float timer = 0f;
        private Animator anim;
        private NavMeshAgent nav;
        private GameObject player;
        private bool playerInRange;
        private BoxCollider hornCollider;
        private Rigidbody rigidBody;
        private CapsuleCollider capsuleCollider;
        private bool disappearEnemy = false;
        private bool isAlive;
        private ParticleSystem blood;

        bool isAttacking = false;

        float currentHealthPoints = 100f;

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
        }


        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }


        // Use this for initialization
        void Start()
        {

            GameManager.instance.RegisterEnemy(this);
            rigidBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            hornCollider = GetComponentInChildren<BoxCollider>();
            nav = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");
            currentHealthPoints = maxHealthPoints;
            anim = GetComponent<Animator>();
            isAlive = true;
            blood = GetComponentInChildren<ParticleSystem>();
            //StartCoroutine (attack ());
        }

        // Update is called once per frame
        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                playerInRange = true;
                StartCoroutine(attack(player));
            }
            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                playerInRange = false;
            }

            timer += Time.deltaTime;
            if (disappearEnemy)
            {
                transform.Translate(-Vector3.up * disappearSpeed * Time.deltaTime);
            }
        }

        public void TakeDamage(float damage)
        {

            if (currentHealthPoints > 0)
            {
                anim.Play("Get_Hit");
                blood.Play();
                currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            }
            if (currentHealthPoints <= 0)
            {
                isAlive = false;
                KillEnemy();
            }
        }

        private void KillEnemy()
        {
            GameManager.instance.KilledEnemy(this);
            capsuleCollider.enabled = false;
            nav.enabled = false;
            anim.SetTrigger("ZergDie");
            rigidBody.isKinematic = true;
            StartCoroutine(RemoveEnemy());
        }

        IEnumerator RemoveEnemy()
        {
            yield return new WaitForSeconds(4f);
            disappearEnemy = true;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);

        }

        IEnumerator attack(GameObject target)
        {   // TODO  add the player is dead or alive logic
            var playerComponent = target.GetComponent<Player>();
            if (playerInRange && playerComponent.CurrentHP > 0 && isAlive == true)
            {
                anim.Play("Attack");
                playerComponent.TakeDamage(damagePerHit);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            if (playerComponent.CurrentHP == 0)
            {
                anim.Play("Idle");
            }
            yield return null;
            StartCoroutine(attack(player));
        }
    }
}