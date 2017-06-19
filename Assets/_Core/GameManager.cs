using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;

        [SerializeField] GameObject player;
        [SerializeField] GameObject[] spawnPoints;
        [SerializeField] Zerg zerg_rhino;

        private bool gameOver = false;

        // TODO track the level of player to modify the enemy spawn speed and so on...
        private float generatedSpawnTime = 7f;
        private float currentSpawnTime = 0f;
        private Zerg newEnemy;
        private List<Zerg> enemies = new List<Zerg>();
        private List<Zerg> killedEnemies = new List<Zerg>();

        public void RegisterEnemy(Zerg enemy)
        {
            enemies.Add(enemy);
        }

        public void KilledEnemy(Zerg enemy)
        {
            killedEnemies.Add(enemy);
        }

        public bool GameOver
        {
            get { return gameOver; }
        }

        public GameObject Player
        {
            get { return player; }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            currentSpawnTime += Time.deltaTime;
            StartCoroutine(Spawn());

        }

        IEnumerator Spawn()
        {
            if (currentSpawnTime > generatedSpawnTime)
            {
                currentSpawnTime = 0;

                // TODO tune the dynamic performance of spawned enemies (enemies amount and classification)

                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                newEnemy = Instantiate(zerg_rhino) as Zerg;
                newEnemy.transform.position = spawnLocation.transform.position;

                //yield return new WaitForSeconds(2f);  

                yield return null;
                StartCoroutine(Spawn());

            }
        }


    }
}