using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
    {

        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        // Use this for initialization
        void Start()
        {
            print("Power Attack behavior attach to " + gameObject.name);   // TODO implement the Power Attack input (e.g. btn pressed)
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Use(AbilityUseParams useParams)
        {
			print ("Power attack used by " + gameObject.name);
			float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
			useParams.target.TakeDamage(damageToDeal);
        }
    }
}