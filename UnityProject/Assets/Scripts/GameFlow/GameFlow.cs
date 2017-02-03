using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum GameStateEnum {
	Default,
	Initialize,
}

[System.Serializable]
public class GameState {
	public GameStateEnum State = GameStateEnum.Initialize;
	public UnityEvent OnEnterState = new UnityEvent();
}

public class GameFlow : MonoBehaviour {
	[SerializeField]
	private GameStateEnum PrivateCurrentState = GameStateEnum.Default;
	private GameStateEnum PrivateNextState = GameStateEnum.Initialize;
	public GameStateEnum CurrentState {
		get {
			return PrivateCurrentState;
		}
		set {
			PrivateNextState = value;
		}
	}

	[SerializeField]
	public UnityEvent OnAwakeEvent = new UnityEvent ();
	[SerializeField]
	public UnityEvent OnStartEvent = new UnityEvent ();
	[SerializeField]
	public UnityEvent OnDestroyEvent = new UnityEvent ();

	[SerializeField]
	public GameState[] GameStates = new GameState[2];

	static private GameFlow instance;
	public static GameFlow Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType(typeof(GameFlow)) as GameFlow;
				if (instance == null) {
					Debug.LogError("There need to be one active GameFlow script on a GameObject in your scene.");
				}
			}
			return instance;
		}
	}

	void Awake () {
		if (OnAwakeEvent != null) {
			OnAwakeEvent.Invoke ();
		}
	}

	void Start () {
		if (OnStartEvent != null) {
			OnStartEvent.Invoke ();
		}
	}

	void OnDestroy () {
		if (OnDestroyEvent != null) {
			OnDestroyEvent.Invoke ();
		}
	}

	void Update () {
		GameFlowStateMachine ();
	}

	private void GameFlowStateMachine () {
		if (PrivateNextState != PrivateCurrentState) {
			PrivateCurrentState = PrivateNextState;
			for (int i = 0; i < GameStates.Length; i++) {
				if (GameStates [i].State == PrivateNextState) {
					if (GameStates [i].OnEnterState != null) {
						GameStates [i].OnEnterState.Invoke ();
						break;
					}
				}
			}
		}
	}
}