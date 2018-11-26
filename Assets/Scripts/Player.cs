using Command;
using Memento;
using UnityEngine;
using System.Collections;

public class Player : Originator {

	public static float distanceTraveled;
	private static int boosts;

    public float speed;
	public float acceleration;
	
	public Vector3 boostVelocity, jumpVelocity, hoverVelocity;
	public float gameOverY;
	
	private bool touchingPlatform;
	private Vector3 startPosition;

    private bool thrustMode = true;

	ICommand _command;

	private Renderer _renderer;
    private Rigidbody _rigidbody;

	private void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
	    _command = new MoveCommand(transform, speed);
		
		_memento = new MementoState();
		StartCoroutine(CreateMemento());
    }

	private void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = transform.localPosition;
		_renderer.enabled = false;
		_rigidbody.isKinematic = true;
		enabled = false;
	}
	
	private void Update () {
		
		// TODO rotate model when steering left/right
		_command.Execute();
		
        if (Input.GetKey(KeyCode.Space)) {
            if (touchingPlatform) {
                _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
                touchingPlatform = false;
            }
				
            // TODO add trail particle system
			else if (thrustMode && _rigidbody.velocity.y < 0) {
				_rigidbody.velocity = new Vector3 (_rigidbody.velocity.x, _rigidbody.velocity.y * 0.8f, _rigidbody.velocity.z);
			}
        }

		// TODO add afterburner particle system (or trail renderer)
        if (Input.GetKey(KeyCode.LeftShift) && boosts > 0) {
            _rigidbody.AddForce(boostVelocity, ForceMode.VelocityChange);
            boosts -= 1;
            GUIManager.SetBoosts(boosts);
		}

		distanceTraveled = transform.localPosition.x;
		GUIManager.SetDistance(distanceTraveled);

		if(transform.localPosition.y < gameOverY) {
			GameEventManager.TriggerGameOver();
		}
	}

	private void FixedUpdate() {
		if(touchingPlatform) {
			_rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
		}
	}
	
	// TODO add checkpoints, regenerate platforms
	public override IEnumerator CreateMemento() {
		while(true) {
			_memento.SetState(transform.position, _rigidbody.velocity, boosts);
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public override void Restore(MementoParams wrappers) {
		transform.position = (Vector3)wrappers.parameters[0];
		_rigidbody.velocity = (Vector3)wrappers.parameters[1];
		boosts = (int)wrappers.parameters[2];
	}

	private void OnCollisionEnter() {
		touchingPlatform = true;
	}

	private void OnCollisionExit() {
		touchingPlatform = false;
	}

	private void GameStart() {
		boosts = 0;
		GUIManager.SetBoosts(boosts);
		distanceTraveled = 0f;
		GUIManager.SetDistance(distanceTraveled);
		transform.localPosition = startPosition;
		_renderer.enabled = true;
		_rigidbody.isKinematic = false;
		enabled = true;
	}
	
	private void GameOver() {
		_renderer.enabled = false;
		_rigidbody.isKinematic = true;
		enabled = false;
	}
	
	public static void AddBoost() {
		boosts += 1;
		GUIManager.SetBoosts(boosts);
	}
}