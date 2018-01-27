using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {

    public Transform gridPrefab, kingPrefab, pigeonPrefab, soldierPrefab;

    public int gridWidth, gridHeight;

    public float tileWidth, tileHeight;

    Transform mainCamera;

    // Use this for initialization
    void Start ()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        mainCamera.position = new Vector3(gridWidth / 2 * tileWidth, Mathf.Max(gridWidth, gridHeight) * 10, gridWidth / 2 * tileWidth);

        float xoffset = 0, yoffset = 0;

        if (gridWidth % 2 == 0)
        {
            xoffset = 5;
        }
        if (gridHeight % 2 == 0)
        {
            yoffset = 5;
        }

        for (int ix = 0; ix < gridWidth; ix++)
            for (int iy = 0; iy < gridHeight; iy++) {
                Instantiate(gridPrefab, new Vector3(ix * 10 + xoffset, 0 , iy * 10 + yoffset), Quaternion.identity);
            }

        Instantiate(soldierPrefab, GridPosToWorldPos(1, 1), Quaternion.identity);

        Instantiate(kingPrefab, GridPosToWorldPos(gridWidth/2, -1), Quaternion.identity);

    }

    // Update is called once per frame
    void Update () {
		
	}

    Vector3 GridPosToWorldPos(int x, int y) {

        float xoffset = 0, yoffset = 0;

        if (gridWidth % 2 == 0)
        {
            xoffset = 5;
        }
        if (gridHeight % 2 == 0)
        {
            yoffset = 5;
        }

        return new Vector3(x * tileWidth + xoffset, 1, y * tileHeight + yoffset);
    }
    
}
