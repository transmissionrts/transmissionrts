using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableUnit : MonoBehaviour {

    Transform upArrow, downArrow, leftArrow, rightArrow, upArrowSelected, downArrowSelected, leftArrowSelected, rightArrowSelected;
	// Use this for initialization
	void Start () {
        TransformUtils tu = new TransformUtils();
        upArrow = tu.GetChildByName("UpArrow", transform);
        downArrow = tu.GetChildByName("DownArrow", transform);
        leftArrow = tu.GetChildByName("LeftArrow", transform);
        rightArrow = tu.GetChildByName("RightArrow", transform);
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect() {
        upArrow.gameObject.SetActive(true);
        //downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);
    }

    public void SelectDirection(Direction direction)
    {
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        /*
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
    
}
