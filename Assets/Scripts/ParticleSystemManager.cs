using UnityEngine;

public class ParticleSystemManager : MonoBehaviour {

	public ParticleSystem[] ParticleSystems;

	private void Start() {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameOver();
	}

	private void GameStart() {
		foreach(var ps in ParticleSystems) {
			ps.Clear();
            var em = ps.emission;
            em.enabled = true;
        }
	}

	private void GameOver() {
        foreach (var ps in ParticleSystems) {
            var em = ps.emission;
            em.enabled = false;
		}
	}
}