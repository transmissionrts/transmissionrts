using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour {
    public Transform gridPrefab, kingPrefab, pigeonPrefab, soldierPrefab;
    public int gridWidth, gridHeight;
    public int soldierCount;
    public float tileWidth, tileHeight;

    Transform mainCamera;
    List<Transform> teamA;
    Transform kingA;
	List<Transform> teamB;
    Transform kingB;

    void SpawnSoldiers(List<Transform> container, int team, int startX, int startY) {
        container = new List<Transform>();

        Quaternion orient;
        if (team == 0) {
            orient = Quaternion.Euler(0, 180, 0);
        } else {
            orient = Quaternion.Euler(0, 0, 0);
        }

		for (int i = 0; i < soldierCount; i++) {
            Transform soldier = Instantiate(soldierPrefab, position: GridPosToWorldPos(startX + i, startY), rotation: orient);
			container.Add(soldier);
            SoldierController controller = soldier.GetComponent<SoldierController>();
            controller.SetTeam(team);
        }

        Debug.Log(string.Format("Container LENGTH:: {0}", container.Count));
    }

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

        for (int ix = 0; ix < gridWidth; ix++) {
            for (int iy = 0; iy < gridHeight; iy++) {
				Transform tile = Instantiate(gridPrefab, new Vector3(ix * 10 + xoffset, 0 , iy * 10 + yoffset), Quaternion.identity);
				tile.parent = this.transform;
            }
        }

		LogicalGrid grid = this.gameObject.AddComponent<LogicalGrid> ();
		grid.Setup (this.gridWidth, this.gridHeight);

        // 0 for team A
        SpawnSoldiers(teamA, 0, 1, 0);
        kingA = Instantiate(kingPrefab, GridPosToWorldPos(gridWidth/2, -1), Quaternion.identity);

        // 1 for team B
        SpawnSoldiers(teamB, 1, Mathf.RoundToInt(gridWidth/2), gridHeight -1 );
        kingB = Instantiate(kingPrefab, GridPosToWorldPos(gridWidth/2, gridHeight), Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public Vector3 GridPosToWorldPos(int x, int y) {

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

	public LogicalGrid GetGrid(){
		return this.GetComponent<LogicalGrid> ();
	}    
}
