using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;
using RPG.CameraUI; // TODO consider rewiring
using RPG.Weapons;
using RPG.Core;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] int enemyLayer = 9;
        [SerializeField] float maxHealthPoints = 100f;
        float currentHealthPoints = 100f;
        public float CurrentHP
        {
            get { return currentHealthPoints; }
        }

        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimebetweenAttacks = .5f;
        [SerializeField] float maxAttackRange = 2f;

        [SerializeField] ArmedWeapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // Temporarily serializing for debugging
        [SerializeField] SpecialAbilityConfig ability1;

        Animator animator;
        //GameObject currentTarget;          
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;
        private ParticleSystem blood;

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start()
        {
            RegisiterForMouseClick();
            SetCurrentMaxHP();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            blood = GetComponentInChildren<ParticleSystem>();
            ability1.AddComponent(gameObject);
        }

        public void TakeDamage(float damage)
        {

            // TODO reply to enemy's dizzle skill

            //animator.SetTrigger("Hurt");           
            blood.Play();
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            //if (currentHealthPoints <= 0)
            //{
            //    Destroy(gameObject);
            //}
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO remove const
        }

        private void SetCurrentMaxHP()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No DominantHand found on Player, please add one.");
            Assert.IsFalse(numberOfDominantHands > 1, "Mutiple DominantHand script on Player, please remove to one.");
            return dominantHands[0].gameObject;
        }

        private void RegisiterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }


        // TODO refactor to simple
        public void OnMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                if (IsTargetInRange(enemy))
                {
                    //AttackTarget(enemy);
                    AttemptSpecialAbility1();
                }
                //currentTarget = enemy;
            }
        }

        private void AttemptSpecialAbility1()
        {
            throw new NotImplementedException();
        }

        private void AttackTarget(GameObject target)
        {
            var enemyComponent = target.GetComponent<Zerg>();       // TODO suit zerg and human-enemy ???
            if (Time.time - lastHitTime > minTimebetweenAttacks)
            {
                animator.SetTrigger("Attack");  // TODO make const
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= maxAttackRange;
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (Time.time - lastHitTime > minTimebetweenAttacks && currentHealthPoints > 0) {
        //        if (other.tag == "weapon") {

        //            lastHitTime = Time.time;
        //        }
        //    }
        //}


    }
}