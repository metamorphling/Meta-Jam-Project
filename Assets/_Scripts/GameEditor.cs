using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameEditor : MonoBehaviour {

	public Transform GridPrefab;
	public Vector2 mapSiz;
	public GameObject objectToSpawn;
	Transform mapGrid;
	bool CreateGame = true;
	public Camera cam;
	void Start()
	{
		GenerateMap ();
	}


	void Update()
	{
		
		if(Input.GetKeyDown(KeyCode.P))
			{
				PlayGame();
			}
	}

	public void PlaceTile()
	{
		if (CreateGame) {
			Vector2 mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint(mousePos);
			if (hit != null) {
				GameObject tile = Instantiate (objectToSpawn, hit.gameObject.transform.position,Quaternion.identity);
			}
		}



	}

	void ColorTile() // this is a test fuction
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Collider2D hit = Physics2D.OverlapPoint(mousePos);
		if (hit != null) {
			Debug.Log ("Hit");
			hit.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}
	public void GenerateMap()
	{

		string ParentName = "MapGrid"; // the name of the empty transform to hold our empty tiles

		if (transform.Find (ParentName)) {
			 DestroyImmediate (transform.Find (ParentName).gameObject,true);
			 
		}

		 mapGrid = new GameObject (ParentName).transform;

		//mapGrid.parent = transform;
			
		for (int x = 0; x < mapSiz.x; x++) {
			for (int y = 0; y < mapSiz.y; y++) {
				Vector3 tilePosition = new Vector3(-mapSiz.x+ (x *1.5f) + 2, -mapSiz.y+(y ) +9,0); // the .64 is the size of the tile as a float below 1f
				Transform newtile = Instantiate (GridPrefab, tilePosition, Quaternion.identity) as Transform;
				newtile.parent = mapGrid;

			}
		}

	}


	public void ChangeCurrentTile(GameObject obj)
	{
		objectToSpawn = obj;
	}

    public void PlayGame()
    {
        if (CreateGame == true)
        {
            CreateGame = false;
            mapGrid.gameObject.SetActive(false);
        }
        else
        {
            CreateGame = true;
            mapGrid.gameObject.SetActive(true);
        }
    }

}
