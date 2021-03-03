using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm {
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
	public bool quick;

	private BasicMazeGenerator mMazeGenerator = null;
	private List<string[,]> mazes = new List<string[,]>();
    private string[,] maze1 = new string[,] {
	{ "l", "br", "lb", "b", "br", "lbr" },
	{ "lr", "lt", "tr", "lr", "lt", "tr" },
	{ "lt", "tb", "br", "lt", "tb", "br" },
	{ "lb", "tb", "tr", "lb", "br", "lr" },
	{ "lr", "lbr", "lbr", "lr", "lt", "r" },
	{ "lt", "tr", "lt", "tr", "tlb", "r" }};

	private string[,] maze2 = new string[,] {
	{ "l", "tb", "br", "lb", "tb", "br" },
	{ "l", "br", "tl", "tr", "tlb", "tr" },
	{ "lr", "lt", "tb", "br", "lb", "br" },
	{ "lt", "tb", "trb", "lt", "tr", "lr" },
	{ "tlb", "tb", "rb", "lb", "tb", "r" },
	{ "tlb", "tb", "t", "tr", "tlb", "r" }};

	private string[,] maze3 = new string[,] {
	{ "l", "br", "lb", "tb", "br", "lbr" },
	{ "lr", "lt", "tr", "lb", "tr", "lr" },
	{ "lr", "tlb", "br", "lr", "lb", "r" },
	{ "l", "br", "lt", "t", "tr", "lr" },
	{ "lr", "lt", "tb", "br", "lbr", "lr" },
	{ "lt", "tb", "trb", "ltr", "lt", "r" }};

	private string[,] maze4 = new string[,] {
	{ "lt", "b", "br", "tlb", "b", "br" },
	{ "lb", "tr", "lt", "br", "ltr", "lr" },
	{ "lt", "br", "tlb", "t", "tb", "r" },
	{ "lb", "tr", "lb", "br", "tlb", "tr" },
	{ "l", "tb", "tr", "lt", "br", "lbr" },
	{ "lt", "tb", "tb", "trb", "lt", "r" }};

	private string[,] maze5 = new string[,] {
	{ "l", "tb", "tb", "tb", "tb", "br" },
	{ "l", "br", "lb", "br", "tlb", "tr" },
	{ "lr", "lt", "tr", "lr", "lb", "br" },
	{ "ltr", "lb", "br", "lt", "tr", "lr" },
	{ "lb", "tr", "lt", "tb", "tb", "r" },
	{ "lt", "tb", "tb", "trb", "tlb", "rt" }};

	private string[,] maze6 = new string[,] {
	{ "l", "tb", "br", "ltb", "b", "trb" },
	{ "lt", "br", "lt", "br", "lt", "br" },
	{ "lb", "tr", "lbr", "lt", "br", "lr" },
	{ "lr", "lb", "", "br", "lt", "r" },
	{ "lr", "ltr", "lr", "ltr", "lb", "rt" },
	{ "lt", "trb", "lt", "tb", "t", "brt" }};

	private string[,] maze7 = new string[,] {
	{ "lr", "lbr", "lb", "b", "tb", "br" },
	{ "l", "tr", "ltr", "lr", "lb", "tr" },
	{ "lt", "tb", "br", "lr", "lt", "br" },
	{ "lb", "br", "lt", "tr", "lbr", "lr" },
	{ "lr", "l", "tb", "trb", "l", "tr" }, 
	{ "ltr", "lt", "tb", "tb", "t", "brt" }};

	private string[,] maze8 = new string[,] {
	{ "l", "tb", "tb", "tb", "br", "lbr" },
	{ "l", "tb", "br", "lb", "tr", "lr" },
	{ "l", "trb", "lr", "lr", "tlb", "r" },
	{ "ltr", "lb", "tr", "lt", "br", "lr" },
	{ "tlb", "tr", "lb", "trb", "lt", "r" },
	{ "ltb", "tb", "t", "tb", "tb", "rt" }};
	private int curr;

	void Start () {
		if (quick)
        {
			mazes.Add(maze1);
			mazes.Add(maze2);
			mazes.Add(maze3);
			mazes.Add(maze4);
        }
        else
        {
			mazes.Add(maze5);
			mazes.Add(maze6);
			mazes.Add(maze7);
			mazes.Add(maze8);
		}
		
		curr = 0;
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
			/*switch (Algorithm)
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
			}*/
			string[,] maze = mazes[curr];
			curr++;
			for (int i = 0; i < maze.GetLength(0); i++)
            {
				for (int j = 0; j < maze.GetLength(1); j++)
                {
					float x = j * (CellWidth + (AddGaps ? .2f : 0));
					float z = i * (CellHeight + (AddGaps ? .2f : 0));
					float y = 1f;
					
					GameObject tmp;
					for (int k = 0; k < maze[i,j].Length; k++)
					{
						switch (maze[i, j][k])
						{
							case 'l':
								tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, y, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
								tmp.transform.parent = transform;
								break;
							case 't':
								tmp = Instantiate(Wall, new Vector3(x, y, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
								tmp.transform.parent = transform;
								break;
							case 'r':
								tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, y, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
								tmp.transform.parent = transform;
								break;
							case 'b':
								tmp = Instantiate(Wall, new Vector3(x, y, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
								tmp.transform.parent = transform;
								break;

						}

					}
				}
            }

			wo.RemoveDuplicateWalls(oldWalls);
		}
		
	}
}
