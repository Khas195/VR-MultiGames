using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ColorToPosition
{
    public Color color;
    public List<Transform> points = new List<Transform>();

    public void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i+1].position);
        }
    }
}
public class GameCube : MonoBehaviour
{
    [SerializeField]
    private List<ColorToPosition> colorPosList = new List<ColorToPosition>() ;

    [SerializeField] private float moveSpeed;
    private ColorToPosition curTarget;

    private Queue<Vector3> movePoint = new Queue<Vector3>();
    private Vector3 originPos;
    private AudioSource audio;
    private Glowable glow;
    void OnDrawGizmos()
    {
        foreach (var cop in colorPosList)
        {
            Gizmos.color = cop.color;
            Gizmos.DrawLine(transform.position, cop.points[0].position);
            cop.OnDrawGizmos();
        }
    }
    
    // Use this for initialization
	void Start ()
	{
	    audio = GetComponent<AudioSource>();
	    glow = GetComponent<Glowable>();
	    originPos = transform.position;

	    audio.loop = true;
    }
	
	// Update is called once per frame
	void Update () {
	    if (movePoint.Count > 0)
        {
            MoveToNextPoint();
        }
        else
        {
            TurnOffMiscellaneous();
        }
    }

    private void TurnOffMiscellaneous()
    {
        glow.StopGlow();
        if (audio.isPlaying)
        {
            audio.Stop();
        }
    }

    private void MoveToNextPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.Peek(), moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.Peek()) <= 0.1f)
        {
            movePoint.Dequeue();
        }
    }

    public void MoveAccordingToColor(Color targetColor)
    {
        foreach (var cop in colorPosList)
        {
            if (!(Ultil.CalColorDifference(cop.color, targetColor) < 0.5f)) continue;
            TurnOnMiscellaneous            (cop);
            movePoint.Clear();
            movePoint.Enqueue(originPos);

            foreach (var p in cop.points)
                movePoint.Enqueue(p.position);

            curTarget = cop;
            return;
        }
    }

    private void TurnOnMiscellaneous(ColorToPosition cop)
    {
        glow.GlowColor = cop.color;
        glow.Glow();
        SoundsManager.GetInstance().PlayClip(audio, ActionInGame.CubeMoving);
    }

    public bool IsMoving()
    {
       return movePoint.Count > 0;
    }
}
