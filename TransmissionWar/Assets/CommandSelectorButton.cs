using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TransmissionNetworking;

public class CommandSelectorButton : MonoBehaviour {

    public Transform upArrow, downArrow, leftArrow, rightArrow, label;

    GameObject unitSelectorObj;
    UnitSelector unitSelector;

	public LocalPlayer localPlayer;

    void Start()
    {
		this.localPlayer = GameObject.FindObjectOfType<LocalPlayer> ();

        unitSelectorObj = GameObject.FindGameObjectWithTag("UnitSelector");
        unitSelector = unitSelectorObj.GetComponent<UnitSelector>();

        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    //When up button is pressed
    public void ButtonCommandUp()
    {
        AnyButtonCommand(upArrow, Direction.UP);
    }

    //When down button is pressed
    public void ButtonCommandDown()
    {
        AnyButtonCommand(downArrow, Direction.DOWN);
    }

    //When left button is pressed
    public void ButtonCommandLeft()
    {
        AnyButtonCommand(leftArrow, Direction.LEFT);
    }

    //When right button is pressed
    public void ButtonCommandRight()
    {
        AnyButtonCommand(rightArrow, Direction.RIGHT);
    }

    //When any button is pressed
    void AnyButtonCommand(Transform arrowToEnable, int direction)
    {
        UnselectAll();
        if (this.localPlayer.nextCommand == direction)
        {
            this.localPlayer.nextCommand = Direction.NONE;
        }
        else {
            this.localPlayer.SelectedCommand(direction);
            arrowToEnable.gameObject.SetActive(true);
        }
        
        unitSelector.gameStep = GameState.SELECT_RECIPIENT;
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
