using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

	public Transform prefab;
	public int numberOfObjects;
	public float recycleOffset;
	public Vector3 startPosition;
	public Vector3 minSize, maxSize, minGap, maxGap;
	public float minY, maxY;
	public Material[] materials;
	public PhysicMaterial[] physicMaterials;

	// TODO refactor as list of abstract items
	public Booster booster;
    public Thruster Thruster;

	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;

	private void Start() {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		objectQueue = new Queue<Transform>(numberOfObjects);
		for (var i = 0; i < numberOfObjects; i++) {
			objectQueue.Enqueue((Transform)Instantiate(
				prefab, new Vector3(0f, 0f, -100f), Quaternion.identity));
		}
		enabled = false;
	}

	private void Update() {
		if(objectQueue.Peek().localPosition.x + recycleOffset < Player.distanceTraveled) {
			Recycle();
		}
	}

	private void Recycle() {
		var scale = new Vector3(
			Random.Range(minSize.x, maxSize.x),
			Random.Range(minSize.y, maxSize.y),
			Random.Range(minSize.z, maxSize.z));

		var position = nextPosition;
		position.x += scale.x * 0.5f;
		position.y += scale.y * 0.5f;
        
		// TODO implement roulette wheel selection
		var itemSpawnChance = Random.Range(0, 10);
        if (itemSpawnChance > 5) {
            Thruster.SpawnIfAvailable(position);
        } 
        else booster.SpawnIfAvailable(position);

		var o = objectQueue.Dequeue();
		o.localScale = scale;
		o.localPosition = position;
		var materialIndex = Random.Range(0, materials.Length);
		o.GetComponent<Renderer>().material = materials[materialIndex];
		// TODO instead of physic materials, assign friction values with Strategy pattern
		o.GetComponent<Collider>().material = physicMaterials[materialIndex];
		objectQueue.Enqueue(o);

		nextPosition += new Vector3(
			Random.Range(minGap.x, maxGap.x) + scale.x,
			Random.Range(minGap.y, maxGap.y),
			Random.Range(minGap.z, maxGap.z));

		if (nextPosition.y < minY) {
			nextPosition.y = minY + maxGap.y;
		}
		else if (nextPosition.y > maxY) {
			nextPosition.y = maxY - maxGap.y;
		}
	}
	
	private void GameStart() {
		nextPosition = startPosition;
		for (var i = 0; i < numberOfObjects; i++) {
			Recycle();
		}
		enabled = true;
	}

	private void GameOver() {
		enabled = false;
	}
}