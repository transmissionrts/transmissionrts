using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(order: -1000)]
public class GridCreator : MonoBehaviour
{
    public Transform gridPrefab, kingPrefab, pigeonPrefab, catSoldierPrefab, dogSoldierPrefab;
    public int gridWidth, gridHeight;
    public int soldierCount;
    public float tileWidth, tileHeight;

    public Transform lightTile, darkTile;

    Transform mainCamera;
    public List<Transform> teamA;
    Transform catKing;
    public List<Transform> teamB;
    Transform dogKing;

    public Transform dogKingPrefab, catKingPrefab;

    LogicalGrid logicalGrid;

    void SpawnSoldiers(List<Transform> container, int team, int startX, int startY) {
		container.Clear ();

        Quaternion orient;
        if (team == 0)
        {
            orient = Quaternion.Euler(0, 180, 0);
        }
        else {
            orient = Quaternion.Euler(0, 0, 0);
        }

        Transform[] soldierPrefab = new Transform[2];

        soldierPrefab[0] = catSoldierPrefab;
        soldierPrefab[1] = dogSoldierPrefab;

        for (int i = 0; i < soldierCount; i++)
        {
            Transform soldier = Instantiate(soldierPrefab[team], position: GridPosToWorldPos(startX + i, startY), rotation: Quaternion.Euler(0, 180, 0)); //rotation: orient
            soldier.name = string.Format("{0}_{1:00}", soldierPrefab[team].name, i);
            container.Add(soldier);
            SoldierController controller = soldier.GetComponent<SoldierController>();

            controller.Team = (PlayerId) team;
            controller.Position = new Vector2(startX + i, startY);

            // register soldier last after proper state
            logicalGrid.RegisterSoldier(soldier.GetComponent<SoldierController>(), new Vector2(startX + i, startY));
        }

        Debug.Log(string.Format("Container LENGTH:: {0}", container.Count));
    }

    // Use this for initialization
    void Awake()
    {
        soldierCount = gridWidth;
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
        {
            for (int iy = 0; iy < gridHeight; iy++)
            {
                if ((ix+ iy) % 2 == 0)
                    gridPrefab = lightTile;
                else
                    gridPrefab = darkTile;
                Transform tile = Instantiate(gridPrefab, new Vector3(ix * 10 + xoffset, 0, iy * 10 + yoffset), Quaternion.identity);
                tile.parent = this.transform;
            }
        }

		logicalGrid = this.gameObject.AddComponent<LogicalGrid> ();
		logicalGrid.Setup (this.gridWidth, this.gridHeight);

        // 0 for team A
        this.teamA = new List<Transform>();
        SpawnSoldiers(teamA, 0, 0, 0);
        catKing = Instantiate(catKingPrefab, GridPosToWorldPos(-1, 0), Quaternion.Euler(0, 180, 0));
        logicalGrid.KingATransform = catKing;

        // 1 for team B
        this.teamB = new List<Transform>();
        SpawnSoldiers(teamB, 1, 0, gridHeight - 1); //Mathf.RoundToInt(gridWidth),
        dogKing = Instantiate(dogKingPrefab, GridPosToWorldPos(-1, gridHeight-1), Quaternion.Euler(0,180,0));
        logicalGrid.KingBTransform = dogKing;
    }

    public Vector3 GridPosToWorldPos(int x, int y)
    {

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

    public LogicalGrid GetGrid()
    {
        return this.GetComponent<LogicalGrid>();
    }
}