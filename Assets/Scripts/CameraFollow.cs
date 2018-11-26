using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Transform target;

    private Vector3 _offset;
    private Vector3 _moveVector;

	private void Start () {
        _offset = transform.position - target.position;
	}

	private void Update () {
        _moveVector = target.position + _offset;

        transform.position = new Vector3(_moveVector.x, _offset.y, 0f);
    }
}