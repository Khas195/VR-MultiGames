using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    [SerializeField]
    [Range(0, 100)]
    private float _paintFilPercent = 50;

    [SerializeField]
    private int paintWidth;
    [SerializeField]
    private int paintHeight;

    private static GameSettings instance;

    public float PaintFilPercent
    {
        get { return _paintFilPercent; }
    }

    public int PaintWidth
    {
        get { return paintWidth; }
    }

    public int PaintHeight
    {
        get { return paintHeight; }
    }

    public static GameSettings GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
}
