using UnityEngine;
using System.Collections;

public class Maps : MonoBehaviour {
	public GameObject player;
	public int test;
	public GameObject[] mapImages;
	private int numPlayers = 4;
	private int width;
	private int height;
	public float disX = 4f;
	public float disY = 3f;
	// Use this for initialization
	void Start () {
		width = mapImages.Length/2;
		height = 2;
		int j = -1;
		Debug.Log (numPlayers);
		for (int i = 0; i < numPlayers; i++) {
			Debug.Log ("spawn");
			(Instantiate (player) as GameObject).GetComponent<MapSelector> ().setPlayerNum (i);
		}
		for (int i = 0; i < mapImages.Length; i++) {
			if (i % width == 0)
				j++;
			Instantiate(mapImages[i],new Vector3(((i%width) * disX)-9, (j * -disY) +3, 0),Quaternion.identity);
		}
	}
	public int getWidth()
	{
		return width;
	}

	public int getHeight()
	{
		return height;
	}
	public float getMoveX()
	{
		return disX;
	}
	public float getMoveY()
	{
		return disY;
	}
}
