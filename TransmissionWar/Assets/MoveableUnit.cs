using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableUnit : MonoBehaviour {

    Transform upArrow, downArrow, leftArrow, rightArrow, upArrowSelected, downArrowSelected, leftArrowSelected, rightArrowSelected;

    int command;

    float tileWidth, tileHeight;

    public Transform grid;
    GridCreator gridCreator;
    // Use this for initialization
    void Start () {

        grid = GameObject.FindGameObjectWithTag("Grid").transform;

        gridCreator = grid.GetComponent<GridCreator>();
        tileWidth = gridCreator.tileWidth;
        tileHeight = gridCreator.tileHeight;

        /*
        TransformUtils tu = new TransformUtils();
        
        upArrow = tu.GetChildByName("UpArrow", transform);
        downArrow = tu.GetChildByName("DownArrow", transform);
        leftArrow = tu.GetChildByName("LeftArrow", transform);
        rightArrow = tu.GetChildByName("RightArrow", transform);
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnSelect() {
        //upArrow.gameObject.SetActive(true);
        //downArrow.gameObject.SetActive(false);
        //leftArrow.gameObject.SetActive(true);
        //rightArrow.gameObject.SetActive(true);
    }

    public void SelectDirection(Direction direction)
    {
        /*upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        
        if (direction == Direction.UP)
        {
            upArrowSelected.gameObject.SetActive(true);
        }
        if (direction == Direction.DOWN)
        {
            downArrowSelected.gameObject.SetActive(true);
        }
        if (direction == Direction.LEFT)
        {
            leftArrowSelected.gameObject.SetActive(true);
        }
        if (direction == Direction.RIGHT)
        {
            rightArrowSelected.gameObject.SetActive(true);
        }*/
    }

    public void ExecuteCommand() {

        switch (command) {

            case Direction.UP:
                transform.position += new Vector3(0, 0, -tileHeight);
                break;

            case Direction.DOWN:
                transform.position += new Vector3(0, 0, tileHeight);
                break;

            case Direction.LEFT:
                transform.position += new Vector3(tileWidth, 0, 0);
                break;

            case Direction.RIGHT:
                transform.position += new Vector3(-tileWidth, 0, 0);
                break;
            
        }


    }
    
}
