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
		public SoldierController OccupiedBy;

		public Tile (Vector2 pos) {
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

	public Transform KingATransform;
	public Transform KingBTransform;

	public int Width {
		get{ return this._width; }
		private set{ this._width = value; }
	}

	public int Height {
		get{ return this._height; }
		private set{ this._height = value; }
	}

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

	public bool IsValidPos (Vector2 pos)
	{
		if (pos.x < 0 || pos.x > this.Width -1)
			return false;
		if (pos.y < 0 || pos.y > this.Height - 1)
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
		if (tile == null) {
			Debug.LogWarningFormat ("{0}.IsTileFree({1}) is Null", this.name, pos);
			return false;
		}
		var isFree = tile.IsFree();
		string usedBy = "";
		if (!isFree) {
			usedBy = string.Format (" {0}[{1}]", tile.OccupiedBy.name, tile.OccupiedBy.Team);
		}
		Debug.LogFormat ("{0}.IsTileFree({1}): {2}{3}", this.name, pos, isFree, usedBy);
		return isFree;
	}

	public Vector2 GetTargetPos(Vector2 targetPos, Direction movementDirection) {
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
		return targetPos;
	}

	public bool CanMakeMove (SoldierController soldier, Direction movementDirection)
	{
		Vector2 soldierPos = soldier.Position;
		Vector2 targetPos = GetTargetPos(soldierPos, movementDirection);
		return this.IsTileFree (targetPos);
	}
		
	public void RegisterSoldier(SoldierController soldier, Vector2 pos) {
		Debug.LogFormat("THERE:: {0}", soldier.Position);
		Tile tile = this.GetTile(soldier.Position);
		tile.OccupiedBy = null;

		Debug.LogFormat("HERE:: {0}", pos);
		tile = this.GetTile(pos);
		tile.OccupiedBy = soldier;
		soldier.Position = pos;
	}

	public void SetUpSoliders(IEnumerable<SoldierController> soldiers){
		foreach (var soldier in soldiers) {
			var tile = this.GetTile (soldier.Position);
			if (tile == null)
				Debug.LogErrorFormat (this, "{0}.SetUp() {1} has no tile at {2}!", this.name, soldier.name, soldier.Position);
			tile.OccupiedBy = soldier;
		}
	}
}
