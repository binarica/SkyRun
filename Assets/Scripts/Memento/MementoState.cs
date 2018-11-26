using System.Collections.Generic;

namespace Memento {
	public class MementoState {
		List<MementoParams> RememberPosition;
		private bool _inUndo;

		public MementoState() {
			RememberPosition = new List<MementoParams>();
		}

		public bool Available() {
			return RememberPosition.Count > 0;
		}

		public MementoParams GetState() {
			_inUndo = true;

			var currentPos = RememberPosition[RememberPosition.Count - 1];
			RememberPosition.RemoveAt(RememberPosition.Count - 1);
			_inUndo = false;
			return currentPos;
		}

		public void SetState(params object[] parametersWrapper) {
			if (_inUndo) return;
			RememberPosition.Add(new MementoParams(parametersWrapper));
		}

	}

	public class MementoParams {
		public object[] parameters;
	
		public MementoParams(params object[] parametersWrapper) {
			parameters = new object[parametersWrapper.Length];
			for (var i = 0; i < parametersWrapper.Length; i++) {
				parameters[i] = parametersWrapper[i];
			}
		}
	}
}