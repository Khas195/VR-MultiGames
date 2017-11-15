using UnityEngine;
using System.Collections.Generic;

public class InputReceiver : MonoBehaviour {
	[SerializeField]
	List<CustomHotkey> hotkeys = new List<CustomHotkey> ();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < hotkeys.Count; i++) {
			hotkeys [i].HandleEvent ();

		}
	}

	public List<CustomHotkey> GetHotkeysList ()
	{
		return hotkeys;
	}
}
