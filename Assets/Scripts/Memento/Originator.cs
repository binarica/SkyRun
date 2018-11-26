using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memento {
	public abstract class Originator : MonoBehaviour {
		
		protected MementoState _memento;

		public abstract IEnumerator CreateMemento();
		public abstract void Restore(MementoParams wrappers);

		public void Action() {
			if (!_memento.Available()) return;

			Restore(_memento.GetState());
		}
	}
}