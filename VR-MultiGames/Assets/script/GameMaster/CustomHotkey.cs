using UnityEngine;

[System.Serializable]
public class CustomHotkey {
	[SerializeField]
	private string actionName;
	[SerializeField]
	private KeyCode key;
	[SerializeField]
	private UnityEngine.Events.UnityEvent keyDown;
	[SerializeField]
	private UnityEngine.Events.UnityEvent keyUp;
	[SerializeField]
	private UnityEngine.Events.UnityEvent keyHold;

	public void HandleEvent(){
		if (Input.GetKey(key)) {
			keyHold.Invoke ();
		}
		if (Input.GetKeyDown(key)) {
			keyDown.Invoke ();
		}
		if (Input.GetKeyUp(key)) {
			keyUp.Invoke ();
		}
	}
}
