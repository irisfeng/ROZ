using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

// TODO consider rewiring
using RPG.CameraUI; 
using RPG.Weapons;
using RPG.Core;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        float currentHealthPoints = 100f;
        public float CurrentHP
        {
            get { return currentHealthPoints; }
        }

		[SerializeField] float baseDamage = 10f;
        [SerializeField] float minTimebetweenAttacks = .5f;
        [SerializeField] float maxAttackRange = 2f;

        [SerializeField] ArmedWeapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        // Temporarily serializing for debugging
        [SerializeField] SpecialAbility[] abilities;

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
            abilities[0].AttachComponentTo(gameObject);
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
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }


		void OnMouseOverEnemy(Zerg zerg){
			if (Input.GetMouseButton (0) && IsTargetInRange (zerg.gameObject)) {
				AttackTarget (zerg);
			} else if(Input.GetMouseButtonDown(1)){
				AttemptSpecialAbility (0, zerg);
			}	
		}

        // TODO refactor to simple
//        public void OnMouseClick(RaycastHit raycastHit, int layerHit)
//        {
//            if (layerHit == enemyLayer)
//            {
//                Zerg enemy = (Zerg)raycastHit.collider.gameObject;
//
//                if (IsTargetInRange(enemy))
//                {
//                    //AttackTarget(enemy);
//                    AttemptSpecialAbility(enemy);
//                }
//                //currentTarget = enemy;
//            }
//        }

        private void AttemptSpecialAbility(int abilityIndex, Zerg zerg)
        {
            
			var abilityParams = new AbilityUseParams(zerg, baseDamage);
			abilities[abilityIndex].Use(abilityParams);
        }

        private void AttackTarget(Zerg zerg)
        {
      																 // TODO suit zerg and human-enemy ???
            if (Time.time - lastHitTime > minTimebetweenAttacks)
            {
                animator.SetTrigger("Attack");  // TODO make const
				zerg.TakeDamage(baseDamage);
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