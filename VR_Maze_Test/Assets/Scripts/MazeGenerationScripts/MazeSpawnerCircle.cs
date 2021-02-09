using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawnerCircle : MonoBehaviour {
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
	public GameObject[] CircleCell;
	public GameObject Pillar = null;
	public int Rows = 3;
	public int Columns = 5;
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
					MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
					GameObject tmp;
					tmp = Instantiate(CircleCell[row], new Vector3(0, 0, 0), Quaternion.Euler(0, 365 / 5 * column, 0));
					foreach (Transform child in tmp.transform)
					{
						if (child.name == "Right" && !cell.WallRight)
						{
							Destroy(child.gameObject);
						} else if (child.name == "Left" && !cell.WallLeft)
						{
							Destroy(child.gameObject);
						} else if ((child.name == "Front" && !cell.WallFront) || (child.name == "Front" && row == Rows-1 && column == Columns-1))
						{
							Destroy(child.gameObject);
						} else if ((child.name == "Back" && !cell.WallBack) || (child.name == "Back" && row == 0 && column == 0))
                        {
							Destroy(child.gameObject);
                        }

					}
				}
			}

		}
		
	}
}
