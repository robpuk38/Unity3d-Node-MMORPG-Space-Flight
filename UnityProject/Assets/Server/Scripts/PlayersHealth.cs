using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHealth : MonoBehaviour {

	public const int maxHealth = 100;
	public bool destroyOnDeath;

	public int currentHealth = maxHealth;
	public bool isEmeny = false;
	public RectTransform healthBar;

	private bool isLocalPlayer;

	void Start () {
		PlayerController pc = GetComponent<PlayerController> ();
		isLocalPlayer = pc.isLocalPlayer;
	}

	public void OnChangeHealth()
	{
		Debug.Log ("ON CHANGE HEALTH ");
		healthBar.sizeDelta = new Vector2 (currentHealth, healthBar.sizeDelta.y);
		if (currentHealth <= 0) 
		{
			if (destroyOnDeath) {
				Destroy (gameObject);
			} 
			else 
			{
				currentHealth = maxHealth;
				healthBar.sizeDelta = new Vector2 (currentHealth, healthBar.sizeDelta.y);
				Respawn ();
			}
		}
	}


	public void TakeDamage(GameObject playerFrom, int amount)
	{
		currentHealth -= amount;
		//OnChangeHealth ();
		NetworkManager.Instance.CommandHealthChange (playerFrom, this.gameObject, amount, isEmeny);
	}


	private void Respawn()
	{
		if(isLocalPlayer)
		{
			Vector3 spawnPoint = Vector3.zero;
			Quaternion spawnRoation = Quaternion.Euler (0,180,0);
			transform.position = spawnPoint;
			transform.rotation = spawnRoation;
		}
	}
}
