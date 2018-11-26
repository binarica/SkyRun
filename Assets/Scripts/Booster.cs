using UnityEngine;

public class Booster : PowerUp {

	protected override void OnTriggerEnter() {
		base.OnTriggerEnter();
		Player.AddBoost();
	}
}