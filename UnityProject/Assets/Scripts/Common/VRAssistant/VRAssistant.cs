using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAssistant : Singleton <VRAssistant> {
	public string Version = "0.0.1";

	private const float FindObjectDelayTime = 1f;

	/// <summary>
	/// SteamVR camera.
	/// </summary>
	public class SteamVRCamera {
		public GameObject gameObject = null;
		public bool Ready { get { return (gameObject != null); } }

		public Vector3 Position {
			get {
				if (gameObject != null) {
					return gameObject.transform.position;
				}
				return Vector3.zero;
			}
		}

		public Quaternion Rotation {
			get {
				if (gameObject != null) {
					return gameObject.transform.rotation;
				}
				return Quaternion.identity;
			}
		}
	}

	/// <summary>
	/// SteamVR HMD.
	/// </summary>
	public class SteamVRHMD {
		public SteamVRCamera Camera_Head;
		public SteamVRCamera Camera_Eye;
		public SteamVRCamera Camera_Ears;

		public SteamVRHMD () {
			Camera_Head = new SteamVRCamera();
			Camera_Eye = new SteamVRCamera();
			Camera_Ears = new SteamVRCamera();
		}
	}

	public delegate void ButtonEventCallback ();

	/// <summary>
	/// SteamVR controller.
	/// </summary>
	public class SteamVRController {
		public GameObject gameObject = null;
		public bool Ready { get { return (gameObject != null); } }

		public bool ApplicationMenuButton = false;
		public bool TrackpadButton = false;
		public bool SystemButton = false;
		public bool TriggerButton = false;
		public bool GripButton = false;

		public ButtonEventCallback ApplicationMenuButtonDown = () => {};
		public ButtonEventCallback ApplicationMenuButtonPress = () => {};
		public ButtonEventCallback ApplicationMenuButtonUp = () => {};

		public ButtonEventCallback TrackpadButtonDown = () => {};
		public ButtonEventCallback TrackpadButtonPress = () => {};
		public ButtonEventCallback TrackpadButtonUp = () => {};

		public ButtonEventCallback SystemButtonDown = () => {};
		public ButtonEventCallback SystemButtonPress = () => {};
		public ButtonEventCallback SystemButtonUp = () => {};

		public ButtonEventCallback TriggerButtonDown = () => {};
		public ButtonEventCallback TriggerButtonPress = () => {};
		public ButtonEventCallback TriggerButtonUp = () => {};

		public ButtonEventCallback GripButtonDown = () => {};
		public ButtonEventCallback GripButtonPress = () => {};
		public ButtonEventCallback GripButtonUp = () => {};

		public Vector2 Trackpad = Vector2.zero;
		public float HairTrigger = 0f;
	}

	public GameObject CameraRig = null;
	public SteamVRHMD Cameras = new SteamVRHMD ();
	public SteamVRController LeftController = new SteamVRController ();
	public SteamVRController RightController = new SteamVRController ();

	private bool PrivateIsCamerasReady = false;
	public bool IsCamerasReady { get { return PrivateIsCamerasReady; } }

	private bool PrivateIsControllerssReady = false;
	public bool IsControllerssReady { get { return PrivateIsControllerssReady; } }


	void Awake () {
		Debug.Log ("<color=blue>[VRAssistant] Starting VRAssistant v" + Version + "</color>");
	}

	void Start () {
		StartCoroutine ("FindHMD");
		StartCoroutine ("FindControllers");
	}
	
	void FixedUpdate () {
		UpdateControllers ();
	}

	private IEnumerator FindHMD () {
		PrivateIsCamerasReady = false;

		while (true) {
			if (Cameras.Camera_Head.gameObject == null) {
				Cameras.Camera_Head.gameObject = GameObject.Find ("Camera (head)");
				yield return new WaitForEndOfFrame ();
			}

			if (Cameras.Camera_Eye.gameObject == null) {
				Cameras.Camera_Eye.gameObject = GameObject.Find ("Camera (eye)");
				yield return new WaitForEndOfFrame ();
			}

			if (Cameras.Camera_Ears.gameObject == null) {
				Cameras.Camera_Ears.gameObject = GameObject.Find ("Camera (ears)");
				yield return new WaitForEndOfFrame ();
			}

			if ((Cameras.Camera_Head.gameObject != null) &&
			   (Cameras.Camera_Eye.gameObject != null) &&
			   (Cameras.Camera_Ears.gameObject == null)) {
				PrivateIsCamerasReady = true;
			} else {
				PrivateIsCamerasReady = false;
			}

			yield return new WaitForSeconds (FindObjectDelayTime);
		}
	}

	private IEnumerator FindControllers () {
		SteamVR_ControllerManager SteamVRCMInstance = null;

		PrivateIsControllerssReady = false;

		while (true) {
			if (CameraRig == null) {
				CameraRig = GameObject.Find ("[CameraRig]");
				yield return new WaitForEndOfFrame ();
			}

			if (CameraRig != null) {
				if (LeftController.gameObject == null) {
					if (SteamVRCMInstance == null) {
						SteamVRCMInstance = CameraRig.GetComponent<SteamVR_ControllerManager> ();
					}
					if (SteamVRCMInstance != null) {
						LeftController.gameObject = SteamVRCMInstance.left.activeSelf ? SteamVRCMInstance.left : null;
					}
					yield return new WaitForEndOfFrame ();
				}

				if (RightController.gameObject == null) {
					if (SteamVRCMInstance == null) {
						SteamVRCMInstance = CameraRig.GetComponent<SteamVR_ControllerManager> ();
					}
					if (SteamVRCMInstance != null) {
						RightController.gameObject = SteamVRCMInstance.right.activeSelf ? SteamVRCMInstance.right : null;
					}
					yield return new WaitForEndOfFrame ();
				}
			} else {
				if (LeftController.gameObject == null) {
					LeftController.gameObject = GameObject.Find ("Controller (left)");
					yield return new WaitForEndOfFrame ();
				}

				if (RightController.gameObject == null) {
					RightController.gameObject = GameObject.Find ("Controller (right)");
					yield return new WaitForEndOfFrame ();
				}
			}

			if ((LeftController.gameObject != null) &&
			   (RightController.gameObject != null)) {
				PrivateIsControllerssReady = true;
			} else {
				PrivateIsControllerssReady = false;
			}

			yield return new WaitForSeconds (FindObjectDelayTime);
		}
	}

	private void UpdateControllers () {

	}

	protected VRAssistant() {
	}
}