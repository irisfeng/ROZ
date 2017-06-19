using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] float attackRadius = 30f;
	float currentHealthPoints = 100f;

	AICharacterControl aiCharacterControl = null;
	GameObject player = null;

	public float healthAsPercentage {
		get { 
			return currentHealthPoints / maxHealthPoints;
		}
	}

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		aiCharacterControl = GetComponent<AICharacterControl> ();
	}

	void Update () {
		float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);
		if (distanceToPlayer <= attackRadius) {
			aiCharacterControl.SetTarget (player.transform);
		} else {
			aiCharacterControl.SetTarget (transform);
		}
	}
}
