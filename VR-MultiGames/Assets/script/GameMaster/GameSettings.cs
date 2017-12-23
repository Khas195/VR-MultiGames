using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int standardScale;
    public int standardPaintSize;
	[SerializeField]
	private int numberOfSplash;
	[SerializeField]
	List<Texture2D> inksType;
	[SerializeField]
	List<Color> colors;
	[SerializeField]
	private int inkSheetSize;
	[SerializeField]
	private int numOfSmoothIteration;

	public int NumOfSmoothIteration{
		get { return numOfSmoothIteration; }
	}
	public int NumberOfSplash 
	{
		get{ return numberOfSplash; }
	}
	public int InkSheetSize 
	{
		get{ return inkSheetSize; }
	}

    private static GameSettings instance;

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
