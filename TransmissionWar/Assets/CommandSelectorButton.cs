using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public void ButtonCommandUp()
    {
		UnselectAll();
		if(this.localPlayer.nextCommand == Direction.UP) {
			this.localPlayer.nextCommand = Direction.NONE;
		} else {
			this.localPlayer.SelectedCommand (Direction.UP);
			upArrow.gameObject.SetActive(true);
			Debug.Log("Up Command Given to Pigeon");
			label.GetComponent<Text>().text = "Choose unit to send command to";
		}
    }

    public void ButtonCommandDown()
    {
		UnselectAll();
		if(this.localPlayer.nextCommand == Direction.DOWN) {
			this.localPlayer.nextCommand = Direction.NONE;
		} else {
			this.localPlayer.SelectedCommand (Direction.DOWN);
			downArrow.gameObject.SetActive(true);
			Debug.Log("Down Command Given to Pigeon");
			label.GetComponent<Text>().text = "Choose unit to send command to";
		}
    }

    public void ButtonCommandLeft()
    {
       
		UnselectAll();
		if(this.localPlayer.nextCommand == Direction.LEFT) {
			this.localPlayer.nextCommand = Direction.NONE;
		} else {
			this.localPlayer.SelectedCommand (Direction.LEFT);
			leftArrow.gameObject.SetActive(true);
			Debug.Log("Left Command Given to Pigeon");
			label.GetComponent<Text>().text = "Choose unit to send command to";
		}
    }

    public void ButtonCommandRight()
    {
		UnselectAll();
		if(this.localPlayer.nextCommand == Direction.RIGHT) {
			this.localPlayer.nextCommand = Direction.NONE;
		} else {
			this.localPlayer.SelectedCommand (Direction.RIGHT);
			rightArrow.gameObject.SetActive(true);
			Debug.Log("Right Command Given to Pigeon");
			label.GetComponent<Text>().text = "Choose unit to send command to";
		}
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
