using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Maps : MonoBehaviour {


	public GameObject player;
	private int[] mapVotes;
	public int rows = 2;
	public GameObject[] mapImages;
	public int numPlayers = 4;
	public int votes = 0;
	private int width;
	private int height;
	public float disX = 4f;
	public float disY = 3f;
	// Use this for initialization
	void Start () 
	{
		//sets the width and height of the map grid off of the number of rows and number of maps
		mapVotes = new int[mapImages.Length];
		for (int i = 0; i < mapVotes.Length; i++)
			mapVotes[i] = 0;
		width = mapImages.Length/rows;
		height = rows;
		int j = -1;

		//displays every map
		for (int i = 0; i < mapImages.Length; i++) {
			if (i % width == 0)
				j++;
			Instantiate(mapImages[i],new Vector3(((i%width) * disX)-6, (j * -disY) +3, 0),Quaternion.identity);
		}
	}
	public void addPlayers(int a){
		numPlayers = a;
		for (int i = 0; i < numPlayers; i++) {
			(Instantiate (player) as GameObject).GetComponent<MapSelector> ().setPlayerNum (i);
		}
	}
	//collects map voats and then picks the next level
	public void selectMap(int selection)
	{
		mapVotes [selection] ++;
		votes++;
		//the voting thing
		if (votes == numPlayers)
		{
			List<int> mapToPlay = new List<int>();
			int level = 0;

			//check the votes for each level
			for (int i = 0; i < mapVotes.Length; i++) {
				if (mapVotes [i] == mapVotes [level]) {
					mapToPlay.Add (i);
					level = i;
				} else if (mapVotes [i] > mapVotes [level]) {
					mapToPlay.Clear ();
					level = i;
					mapToPlay.Add (i);
				}
			}
			for(int i = 0; i < mapToPlay.Count; i ++)
			Debug.Log (mapToPlay[i]);
			//pick level
			Random.seed = System.DateTime.Now.Millisecond;
			int temp = mapToPlay [(int)(Random.value * 100) % mapToPlay.Count] + SceneManager.GetActiveScene ().buildIndex +1;
			Debug.Log (temp);
			SceneManager.LoadScene(temp);

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
