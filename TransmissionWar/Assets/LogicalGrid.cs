using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicalGrid : MonoBehaviour
{
	[System.Serializable]
	public class Tile
	{
		[SerializeField]
		private Vector2 position;

		public Vector2 Position { get { return this.position; } }

		[SerializeField]
		public Soldier OccupiedBy;

		public Tile (Vector2 pos)
		{
			this.position = pos;
		}

		public bool IsFree(){
			return this.OccupiedBy == null;
		}
	}

	//[SerializeField]
	private int _width;
	//[SerializeField]
	private int _height;

	[SerializeField]
	private Tile[,] tiles;

	private GridCreator gridCreator;

	void Awake(){
		this.gridCreator = this.GetComponent<GridCreator> ();
	}

	public void Setup (int width, int height)
	{
		this._width = width;
		this._height = height;

		this.tiles = new Tile[this._width, this._height];
		for (int x = 0; x < this._width; x++) {
			for (int y = 0; y < this._height; y++) {
				this.tiles [x, y] = new Tile (new Vector2 (x, y));
			}
		}
	}

	public int Width {
		get{ return this._width; }
		private set{ this._width = value; }
	}

	public int Height {
		get{ return this._height; }
		private set{ this._height = value; }
	}

	public bool IsValidPos (Vector2 pos)
	{
		if (pos.x < 0 || pos.x > this.Width)
			return false;
		if (pos.y < 0 || pos.y > this.Height)
			return false;
		return true;
	}

	public Tile GetTile(Vector2 pos){
		if (!this.IsValidPos (pos))
			return null;
		return this.tiles [(int)pos.x, (int)pos.y];
	}

	public bool IsTileFree (Vector2 pos)
	{
		Tile tile = this.GetTile (pos);
		if (tile == null)
			return false;
		return tile.IsFree();
	}

	public bool CanMakeMove (Soldier soldier, int movementDirection)
	{
		Vector2 soldierPos = soldier.Position;
		Vector2 targetPos = soldierPos;
		switch (movementDirection) {
		case Direction.UP:
			targetPos.y += 1;
			break;
		case Direction.DOWN:
			targetPos.y -= 1;
			break;
		case Direction.LEFT:
			targetPos.x -= 1;
			break;
		case Direction.RIGHT:
			targetPos.x += 1;
			break;
		}
		return this.IsTileFree (targetPos);
	}
}
