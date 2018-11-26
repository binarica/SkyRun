using UnityEngine;

public class PowerUp : MonoBehaviour {

	public Vector3 offset, rotationVelocity;
	public float recycleOffset, spawnChance;
	
	protected void Start() {
		GameEventManager.GameOver += GameOver;
		gameObject.SetActive(false);
	}
	
	protected void Update() {
		if(transform.localPosition.x + recycleOffset < Player.distanceTraveled) {
			gameObject.SetActive(false);
			return;
		}
		transform.Rotate(rotationVelocity * Time.deltaTime);
	}
	
	protected virtual void OnTriggerEnter() {
		gameObject.SetActive(false);
	}

	public void SpawnIfAvailable (Vector3 position) {
		if(gameObject.activeSelf || spawnChance <= Random.Range(0f, 100f)) {
			return;
		}
		transform.localPosition = position + offset;
		gameObject.SetActive(true);
	}

	private void GameOver () {
		gameObject.SetActive(false);
	}
}