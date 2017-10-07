using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemPickupEvent : UnityEngine.Events.UnityEvent<GameObject>{
}

[System.Serializable]
public class ItemDropEvent : UnityEngine.Events.UnityEvent<GameObject>{
}