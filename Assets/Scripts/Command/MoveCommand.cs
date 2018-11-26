using UnityEngine;

namespace Command {
	public class MoveCommand : ICommand {

		private Transform _transform;
		private float _speed;

		public MoveCommand(Transform transform, float speed) {
			_transform = transform;
			_speed = speed;
		}

		public void Execute() {
			_transform.position += Input.GetAxis("Horizontal") * _transform.right * _speed * Time.deltaTime;
		}
	}
}