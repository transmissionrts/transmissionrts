using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {

    public Transform gridPrefab;

    public int gridWidth, gridHeight;

	// Use this for initialization
	void Start ()
    {
        for (int ix = 0; ix < gridWidth; ix++)
            for (int iy = 0; iy < gridHeight; iy++) {
                Instantiate(gridPrefab, new Vector3(ix * 10 - 10 * gridWidth / 2, 0 , iy * 10 - 10 * gridHeight / 2), Quaternion.identity);
            }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
