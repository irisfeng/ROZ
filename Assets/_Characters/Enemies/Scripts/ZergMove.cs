using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using RPG.Core;

namespace RPG.Characters
{
    public class ZergMove : MonoBehaviour
    {

        [SerializeField] float notChaseRadius = 2f;
        private GameObject player;
        private NavMeshAgent nav;
        private Animator anim;

        private void Awake()
        {
            Assert.IsNotNull(player);
        }

        // Use this for initialization
        void Start()
        {

            player = GameManager.instance.Player;
            anim = GetComponent<Animator>();
            nav = GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (nav.enabled == true)
            {
                nav.SetDestination(player.transform.position);
            }
            return;

        }

    }
}