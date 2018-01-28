using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedDirectionHelper {

	static Direction getDirection(Direction d) {
		switch(d) {
		case Direction.UP:
			return Direction.DOWN;

		case Direction.DOWN:
			return Direction.UP;

		case Direction.LEFT:
			return Direction.RIGHT;


		case Direction.RIGHT:
			return Direction.LEFT;
		}
		return Direction.NONE;
	}

}
