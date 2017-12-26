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
    PlayerLandOnTheGround, 
    PlayBounce,
    ControlBoxTrigger,
    CubeMoving,
    PlayerPassThroughColorSphere,
    PlayerPassThroughColorWall
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
    [Range(0, 1)]
    private float soundVolumne = 1;
    [SerializeField]
    private List<ActionToClip> sounds;
    
    [SerializeField] private AudioSource ambientSource;

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

    public void PlayClip(AudioSource src, ActionInGame action)
    {
        src.volume = soundVolumne;
        src.clip = GetClip(action);
        src.Play();
    }
    public void PlayClip( ActionInGame action, Vector3 positon)
    {
        var clip = GetClip(action);
        AudioSource.PlayClipAtPoint(clip, positon, soundVolumne);
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
