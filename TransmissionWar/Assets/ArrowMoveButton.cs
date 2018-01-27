using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMoveButton : ClickButton {

    MoveableUnit moveableUnit;

	// Use this for initialization
	void Start () {
		
	}

    public override void OnClick()
    {
        base.OnClick();
        //moveableUnit.SelectDirection();
    }

}
