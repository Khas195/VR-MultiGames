using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionInGame
{
    PlayerShootLaser,
    DroneShootLaser,
    LaserHitSolidObject,
    PlayJump,
    PlayerLandOnTheGround
}
[Serializable]
public class ActionToClip
{
    public AudioClip clip;
    public ActionInGame actionName;
}

public class SoundsManager : MonoBehaviour
{
    [SerializeField]
    private List<ActionToClip> sounds;

    private static SoundsManager instance;

    public static SoundsManager GetInstance()
    {
        return instance;
    }
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip GetClip(ActionInGame action)
    {
        foreach (var atc in sounds)
        {
            if (atc.actionName.Equals(action))
            {
                return atc.clip;
            }
        }

        return null;
    }
}
