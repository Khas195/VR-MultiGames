using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Paintable))]
public class ControlBox : MonoBehaviour
{
    private Paintable cachePaint;
    private AudioSource audio;
    [SerializeField]
    private List<GameCube> slaveCubes = new List<GameCube>();
	// Use this for initialization
	void Start ()
	{
	    audio  = this.GetComponent<AudioSource>();
	    cachePaint = GetComponent<Paintable>();
	    cachePaint.newPaintEvent.AddListener(TriggerBox);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    bool finished = true;
	    foreach (var c in slaveCubes)
	    {
	        if (!c.IsMoving()) continue;
	        finished = false;
	        break;
	    }

	    if (finished)
	    {
	        GetComponent<Glowable>().StopGlow();
	    }
	}

    public void TriggerBox(Color color)
    {

        SoundsManager.GetInstance().PlayClip(audio, ActionInGame.ControlBoxTrigger);
        GetComponent<Glowable>().GlowColor = color;
        GetComponent<Glowable>().Glow();
        foreach (var c in slaveCubes)
        {
            c.MoveAccordingToColor(color);
        }
    }
}
