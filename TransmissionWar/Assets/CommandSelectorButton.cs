using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandSelectorButton : MonoBehaviour {

    public Transform upArrow, downArrow, leftArrow, rightArrow, label;

    GameObject unitSelectorObj;
    UnitSelector unitSelector;

    void Start()
    {
        unitSelectorObj = GameObject.FindGameObjectWithTag("UnitSelector");
        unitSelector = unitSelectorObj.GetComponent<UnitSelector>();

        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    public void ButtonCommandUp()
    {
        upArrow.gameObject.SetActive(true);
        Debug.Log("Up Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unit.command = Direction.UP;
        unit.ExecuteCommand();
    }

    public void ButtonCommandDown()
    {
        downArrow.gameObject.SetActive(true);
        Debug.Log("Down Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unit.command = Direction.DOWN;
        unit.ExecuteCommand();
    }

    public void ButtonCommandLeft()
    {
        leftArrow.gameObject.SetActive(true);
        Debug.Log("Left Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unit.command = Direction.LEFT;
        unit.ExecuteCommand();
    }

    public void ButtonCommandRight()
    {
        rightArrow.gameObject.SetActive(true);
        Debug.Log("Right Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unit.command = Direction.RIGHT;
        unit.ExecuteCommand();
    }

}
