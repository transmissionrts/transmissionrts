using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandSelectorButton : MonoBehaviour {

    public Transform upArrow, downArrow, leftArrow, rightArrow, label;

    void Start()
    {
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
    }

    public void ButtonCommandDown()
    {
        downArrow.gameObject.SetActive(true);
        Debug.Log("Down Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";
    }

    public void ButtonCommandLeft()
    {
        leftArrow.gameObject.SetActive(true);
        Debug.Log("Left Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";
    }

    public void ButtonCommandRight()
    {
        rightArrow.gameObject.SetActive(true);
        Debug.Log("Right Command Given to Pigeon");
        label.GetComponent<Text>().text = "Choose unit to send command to";
    }

}
