using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memento {
	public class Caretaker : MonoBehaviour {
		List<Originator> savedStates;

		private void Start () {
			savedStates = new List<Originator>();
			var listTemp = FindObjectsOfType<Originator>();

			foreach (var x in listTemp) {
				savedStates.Add(x);
			}
		}

		private void Update () {
			if (!Input.GetKey(KeyCode.R)) return;

			foreach (var obj in savedStates) {
				obj.Action();
			}
		}
	}
}