using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
	public WallOverlap wo;

	private BasicMazeGenerator mMazeGenerator = null;

	void Start () {
		
		GenerateMaze(true);


	}

	public void GenerateMaze(bool newMaze)
    {
		int removeCounter = 0;
		List<GameObject> oldWalls = new List<GameObject>();
		foreach (Transform child in this.transform)
		{
			removeCounter++;
			oldWalls.Add(child.gameObject);
			child.GetComponent<ShaderChanger>().enabled = false;
			Destroy(child.gameObject);
			
		}
		GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
		//Debug.Log("walls " + walls.Length);
		//Debug.Log("Removed " + removeCounter + " walls");
        if (newMaze)
        {
			switch (Algorithm)
			{
				case MazeGenerationAlgorithm.PureRecursive:
					mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RecursiveTree:
					mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RandomTree:
					mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.OldestTree:
					mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
					break;
				case MazeGenerationAlgorithm.RecursiveDivision:
					mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
					break;
			}
			mMazeGenerator.GenerateMaze();
			//Debug.Log("The maze is being Generated");
			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));
					float y = 1f;
					MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
					GameObject tmp;
					//tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
					//tmp.transform.parent = transform;
					if (cell.WallRight)
					{
						tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, y, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
						tmp.transform.parent = transform;
					}
					if (cell.WallFront)
					{
						if (!(row == Rows - 1 && column == Columns - 1))
						{
							tmp = Instantiate(Wall, new Vector3(x, y, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
							tmp.transform.parent = transform;
						}

					}
					if (cell.WallLeft)
					{
						tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, y, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
						tmp.transform.parent = transform;
					}
					if (cell.WallBack)
					{
						if (!(row == 0 && column == 0))
						{
							tmp = Instantiate(Wall, new Vector3(x, y, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
							tmp.transform.parent = transform;
						}


					}
					if (cell.IsGoal && GoalPrefab != null)
					{
						tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
						tmp.transform.parent = transform;
					}
				}
			}
			if (Pillar != null)
			{
				for (int row = 0; row < Rows + 1; row++)
				{
					for (int column = 0; column < Columns + 1; column++)
					{
						float x = column * (CellWidth + (AddGaps ? .2f : 0));
						float z = row * (CellHeight + (AddGaps ? .2f : 0));
						GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
						tmp.transform.parent = transform;
					}
				}
			}

			wo.RemoveDuplicateWalls(oldWalls);
		}
		
	}
}
