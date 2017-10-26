using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    [SerializeField]
    private int paintWidth;
    [SerializeField]
    private int paintHeight;
	[SerializeField]
	List<Texture2D> inksType;
	[SerializeField]
	List<Color> colors;
	[SerializeField]
	private int inkSplatSize;

	public int InkSplatSize 
	{
		get{ return inkSplatSize; }
	}

    private static GameSettings instance;

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

	public Color GetRandomColor ()
	{
		return colors[Random.Range (0, colors.Count)];
	}
	public Color GetColorAt(int index){
		return colors [index];
	}
	public Texture2D GetRandomInk(){
		return inksType[Random.Range (0, inksType.Count)];
	}
    void Awake()
    {
        instance = this;
    }
}
