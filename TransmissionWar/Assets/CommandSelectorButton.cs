﻿using System.Collections;
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

        //MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unitSelector.nextCommand = Direction.UP;
        //unit.command = Direction.UP;
        //unit.ExecuteCommand();
    }

    public void ButtonCommandDown()
    {
        downArrow.gameObject.SetActive(true);
        Debug.Log("Down Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        //MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unitSelector.nextCommand = Direction.DOWN;
        //unit.command = Direction.DOWN;
        //unit.ExecuteCommand();
    }

    public void ButtonCommandLeft()
    {
        leftArrow.gameObject.SetActive(true);
        Debug.Log("Left Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        // MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unitSelector.nextCommand = Direction.LEFT;
        //unit.command = Direction.LEFT;
        //unit.ExecuteCommand();
    }

    public void ButtonCommandRight()
    {
        rightArrow.gameObject.SetActive(true);
        Debug.Log("Right Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";

        //MoveableUnit unit = unitSelector.selectedUnit.GetComponent<MoveableUnit>();
        unitSelector.nextCommand = Direction.RIGHT;
        //unit.command = Direction.RIGHT;
        //unit.ExecuteCommand();
    }

    public void UnselectAll()
    {
        rightArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);

        label.GetComponent<Text>().text = "Pigeons delivered messages...";
    }

}
