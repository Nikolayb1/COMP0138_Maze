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
	{ "lr", "lb", "b", "tb", "tb", "b", "trb" },
	{ "l", "tr", "lt", "br", "tlb", "r", "lbr" },
	{ "lt", "tb", "rb", "lt", "br", "ltr", "lr" },
	{ "lb", "tb", "tr", "lbr", "lt", "tb", "tr" },
	{ "lt", "br", "lb", "r", "tlb", "tb", "rb" },
	{ "lbr", "lr", "lr", "lr", "lb", "tb", "r" },
	{ "lt", "t", "tr", "lt", "tr", "tlb", "r" }};

	private string[,] maze2 = new string[,] {
	{ "l", "tb", "tb", "rb", "lb", "b", "trb" },
	{ "l", "rb", "ltb", "r", "lr", "lt", "rb" },
	{ "lr", "lt", "rb", "lr", "ltr", "lb", "r" },
	{ "lt", "trb", "lr", "lt", "rb", "lr", "lr" },
	{ "ltb", "rb", "tl", "rb", "lt", "tr", "lr" },
	{ "lbr", "lr", "lb", "tr", "lb", "br", "lr" },
	{ "lt", "t", "tr", "tlb", "tr", "lt", "r" }};

	private string[,] maze3 = new string[,] {
	{ "l", "rb", "lb", "tb", "tb", "tb", "rb" },
	{ "lr", "lt", "tr", "lb", "br", "lb", "tr" },
	{ "lr", "lb", "tb", "tr", "lr", "lt", "trb" },
	{ "l", "tr", "lb", "br", "lt", "tb", "rb" },
	{ "lr", "tlb", "tr", "lt", "b", "trb", "lr" },
	{ "lr", "lb", "tb", "rb", "lr", "lbr", "lr" },
	{ "lt", "tr", "tlb", "t", "tr", "lt", "r" }};

	private string[,] maze4 = new string[,] {
	{ "lr", "lb", "tb", "rb", "lb", "tb", "rb" },
	{ "lr", "lt", "rb", "lt", "tr", "lb", "tr" },
	{ "l", "rb", "lt", "rb", "lbr", "lt", "rb" },
	{ "l", "tb", "tb", "tr", "l", "br", "ltr" },
	{ "lt", "tb", "rb", "lb", "tr", "lt", "rb" },
	{ "lb", "trb", "lt", "tr", "lb", "tb", "tr" },
	{ "lt", "tb", "tb", "tb", "t", "tb", "rb" }};

	private string[,] maze5 = new string[,] {
	{ "lr", "lb", "br", "lb", "tb", "tb", "rb" },
	{ "l", "tr", "lt", "tr", "lbr", "lb", "tr" },
	{ "tl", "br", "ltb", "b", "r", "lt", "rb" },
	{ "lbr", "lt", "tb", "r", "lt", "trb", "lr" },
	{ "l", "trb", "lb", "tr", "ltb", "tb", "tr" },
	{ "lr", "lb", "tr", "tlb", "b", "tb", "rb" },
	{ "lt", "t", "tb", "tb", "rt", "ltb", "rt" }};

	private string[,] maze6 = new string[,] {
	{ "l", "b", "tb", "rb", "lb", "tb", "rb" },
	{ "ltr", "lr", "lbr", "lt", "tr", "lb", "tr" },
	{ "lb", "tr", "lt", "b", "trb", "lt", "rb" },
	{ "tl", "tb", "rb", "l", "rb", "lb", "tr" },
	{ "lb", "tb", "tr", "lr", "lt", "t", "rb" },
	{ "lr", "lb", "br", "ltr", "lb", "trb", "lr" },
	{ "lt", "tr", "lt", "tbb", "tr", "tlb", "rt" }};

	private string[,] maze7 = new string[,] {
	{ "l", "tb", "tb", "tb", "rb", "lb", "rb" },
	{ "lt", "rb", "lb", "rb", "lt", "tr", "lr" },
	{ "ltb", "tr", "lr", "lt", "tb", "rb", "lr" },
	{ "lb", "tb", "tr", "ltb", "rb", "lr", "lr" },
	{ "lrt", "lb", "b", "br", "lr", "lt", "r" },
	{ "lbr", "lr", "lr", "lt", "tr", "lb", "tr" },
	{ "lt", "tr", "lt", "tb", "tb", "t", "rbt" }};

	private string[,] maze8 = new string[,] {
	{ "lr", "lb", "tb", "tb", "tb", "tb", "rb" },
	{ "l", "tr", "lb", "b", "br", "lb", "tr" },
	{ "lr", "lbr", "lr", "lr", "ltr", "lt", "br" },
	{ "lr", "tl", "tr", "lr", "ltb", "br", "lr" },
	{ "lt", "tb", "rb", "l", "trb", "lt", "tr" },
	{ "lb", "tbr", "lr", "lt", "b", "tb", "rb" },
	{ "lt", "tb", "t", "tb", "tr", "ltb", "tr" }};
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
