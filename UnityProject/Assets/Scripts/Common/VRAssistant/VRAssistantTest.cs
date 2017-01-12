using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAssistantTest : MonoBehaviour {
	void Start () {
		Debug.Log ("[VRAssistantTest] Start");
		string CameraHeadPosition = VRAssistant.Instance.Version;
		Invoke ("TestVive", 1f);
	}

	private void TestVive () {
		Debug.Log (
			"[VRAssistantTest] TestVive\n" +
			"- CameraRig: " + (VRAssistant.Instance.CameraRig != null) + "\n" +
			"- Camera_Head: " + VRAssistant.Instance.Cameras.Camera_Head.Ready + "\n" +
			"- Camera_Eye: " + VRAssistant.Instance.Cameras.Camera_Eye.Ready + "\n" +
			"- Camera_Ears: " + VRAssistant.Instance.Cameras.Camera_Ears.Ready + "\n" +
			"- LeftController: " + VRAssistant.Instance.LeftController.Ready + "\n" +
			"- RightController: " + VRAssistant.Instance.RightController.Ready
		);
	}
}
