using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserMovement : Movement
{
	public List<GhostMovement> ghosts;
	private int ghostLayer, pelletLayer, ghostPelletLayer, cherryLayer;
	public float frightenDuration = 20f;
	private float frightenTimer;
	public AudioClip pelletSound, ghostPelletSound, loseSound, catchGhostSound,
		hitWallSound, cherrySound;
	public float powerUpDuration = 20f;
	private float powerUpTimer;
	public bool canUsePowerUp;

	private void Awake()
	{
		ghostLayer = LayerMask.NameToLayer("Ghost");
		pelletLayer = LayerMask.NameToLayer("Pellet");
		ghostPelletLayer = LayerMask.NameToLayer("GhostPellet");
		cherryLayer = LayerMask.NameToLayer("Cherry");
	}

	protected override void Update()
	{
		base.Update();
		if (frightenTimer > 0f)
		{
			frightenTimer -= Time.deltaTime;
			if (frightenTimer <= 0f)
			{
				for (int i = 0; i < ghosts.Count; i++)
				{
					ghosts[i].Frighten(false);
				}
			}
		}

		if (powerUpTimer > 0f)
		{
			powerUpTimer -= Time.deltaTime;
			if (powerUpTimer <= 0f)
			{
				speed *= 0.5f;
				canTravelThroughWalls = false;
			}
		}
	}

	protected override Vector3 GetDirection()
	{
		return new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"));
	}

	protected override void Stop()
	{
		base.Stop();
		// AudioManager.PlaySound(hitWallSound);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		int otherLayer = collider.gameObject.layer;
		if (otherLayer == ghostLayer)
		{
			GhostMovement ghost = collider.GetComponent<GhostMovement>();
			//catch ghost
			if (ghost.frightened)
			{
				ghost.Catch();
				AudioManager.PlaySound(catchGhostSound);
			}
			//lose life
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				AudioManager.PlaySound(loseSound);
			}
			return;
		}
		if (otherLayer == pelletLayer)
		{
			//gain points
			Destroy(collider.gameObject);
			AudioManager.PlaySound(pelletSound);
			return;
		}
		if (otherLayer == ghostPelletLayer)
		{
			//put all ghosts in frightened state
			for (int i = 0; i < ghosts.Count; i++)
			{
				ghosts[i].Frighten(true);
				frightenTimer = frightenDuration;
			}
			Destroy(collider.gameObject);
			AudioManager.PlaySound(ghostPelletSound);
			return;
		}
		if (otherLayer == cherryLayer)
		{
			//activate special ability
			Destroy(collider.gameObject);
			AudioManager.PlaySound(cherrySound);
			if(canUsePowerUp){
				powerUpTimer = powerUpDuration;
				speed *= 2f;
				canTravelThroughWalls = true;
			}
			return;
		}
	}
}
