using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour {

	public GameObject enemy;
	public GameObject spawnPoint;
	public int numberOfEnemies;
	[HideInInspector]
	public List<SpawnPoints> enemySpawnPoionts;

	private void Start()
	{
		for(int i =0; i < numberOfEnemies; i++)
		{
			var spawnPosition = new Vector3 (Random.Range (-8f, 8f), 0f, Random.Range (-8f, 8f));
			var spawnRoation = Quaternion.Euler (0f, Random.Range(0f,180f), 0f);
			SpawnPoints enemySpawnPoiont = (Instantiate(spawnPoint,spawnPosition,spawnRoation) as GameObject).GetComponent<SpawnPoints>();
			enemySpawnPoionts.Add (enemySpawnPoiont);
		}

	}


	public void SpawnEnemys(NetworkManager.EnemiesJSON enemiesJSON)
	{
		
		foreach(NetworkManager.UserJSON enemyJSON in enemiesJSON.enemies)
		{

			if(int.Parse(enemyJSON.UserHealth) <= 0)
			{
			 continue;
			}
			Vector3 position = new Vector3 (enemyJSON.position[0],0,enemyJSON.position[2]);
			Quaternion roation = Quaternion.Euler (enemyJSON.rotation[0],enemyJSON.rotation[1],enemyJSON.rotation[2]);
			GameObject newEnemy = Instantiate (enemy, position, roation) as GameObject;
			newEnemy.name = enemyJSON.Id;
			//PlayerController pc = newEnemy.GetComponent<PlayerController> ();
			//pc.isLocalPlayer = false;
			PlayersHealth h = newEnemy.GetComponent<PlayersHealth> ();
			h.currentHealth = int.Parse(enemyJSON.UserHealth);
			h.OnChangeHealth ();
			h.isEmeny = true;
			h.destroyOnDeath = true;

		}
	}
}
