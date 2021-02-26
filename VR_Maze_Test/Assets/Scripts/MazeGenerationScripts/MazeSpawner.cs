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
	{ "l","tb", "tb", "tb", "rb", "ltb", "b", "tb", "rb", "lrb" },
	{ "lr", "lb", "tb", "tb", "t", "tb", "tr", "lb", "tr", "lr" },
	{ "lr", "ltr", "lb", "tb", "tb", "rb", "lb", "tr", "lb", "r" },
	{ "lt", "rb", "lr", "ltb", "rb", "lr", "lt", "tb", "tr", "ltr" },
	{ "lb", "r", "lt", "rb", "lr", "lt", "tb", "tb", "tb", "rb" },
	{ "lr", "l", "trb", "lr", "lr", "lb", "tb", "tb", "tb", "r" },
	{ "lr", "ltr", "lb", "tr", "lr", "lr", "lb", "rb", "lb", "tr" },
	{ "l", "tb", "tr", "lb", "tr", "lt", "tr", "lr", "lt", "rb" },
	{ "lr","ltb", "tb", "t", "b", "br", "lb", "tl", "br", "lr" },
	{ "lt","tb","tb","tb","tr","ltr","lt","tb","tr","lr" }};

	private string[,] maze2 = new string[,] {
	{ "l", "rb", "lb", "rb", "lb", "b", "tb", "tb", "tb", "trb" },
	{ "lr", "lr", "lr", "lt", "tr", "lr", "lb", "tb", "tb", "rb" },
	{ "lr", "lt", "tr", "ltb", "rb", "lt", "tr", "lb", "tb", "r" },
	{ "lr", "lrb", "lb", "b", "t", "b", "trb", "lr", "lrb", "lr" },
	{ "lr", "l", "tr", "lt", "rb", "lr", "lb", "tr", "lr", "lr" },
	{ "lr", "lt", "trb", "lb", "tr", "ltr", "lt", "rb", "lt", "tr" },
	{ "lr", "lb", "tb", "r", "lb", "tb", "trb", "l", "tb", "rb" },
	{ "lr", "lr", "lb", "tr", "lr", "lb", "tb", "t", "trb", "lr" },
	{ "lt", "tr", "lt", "rb", "lr", "ltr", "lb", "tb", "br", "lr" },
	{ "ltb", "tb", "tb", "tr", "lt", "tb", "tr", "ltb", "t", "r" }};

	private string[,] maze3 = new string[,] {
	{ "lr", "lb", "b", "tb", "rb", "lrb", "lb", "rb", "lb", "rb" },
	{ "lt", "tr", "lr", "lb", "t", "tr", "lr", "lr", "ltr", "lr" },
	{ "lb", "tb", "tr", "ltr", "lb", "tb", "tr", "ltr", "lb", "r" },
	{ "l", "tb", "tb", "tb", "tr", "lb", "rb", "lb", "tr", "lr" },
	{ "lt", "rb", "ltb", "tb", "b", "r", "lt", "tr", "lb", "tr" },
	{ "lb", "tr", "lb", "rb", "ltr", "lt", "b", "rb", "lt", "rb" },
	{ "lr", "ltb", "r", "lt", "tb", "rb", "lr", "lr", "lb", "tr" },
	{ "lt", "rb", "lt", "rb", "lb", "tr", "lr", "lr", "lr", "lrb" },
	{ "lrb", "lt", "rb", "lr", "lr", "ltb", "tr", "lr", "lr", "lr" },
	{ "lt", "tb", "t", "tr", "lt", "tb", "tb", "tr", "lt", "r" }};

	private string[,] maze4 = new string[,] {
	{ "lt", "tb", "b", "tb", "tb", "rb", "ltb", "b", "tb", "rb" },
	{ "lb", "tb", "tr", "lb", "br", "lt", "rb", "lt", "rb", "lr" },
	{ "lt", "tb", "rb", "ltr", "lt", "tb", "t", "rb", "lr", "lr" },
	{ "lb", "tb", "r", "lb", "tb", "tb", "rb", "lt", "tr", "lr" },
	{ "lt", "rb", "lr", "lr", "lb", "trb", "lr", "lb", "tb", "tr" },
	{ "lb", "tr", "ltr", "lr", "lr", "lb", "tr", "lr", "ltb", "rb" },
	{ "lr", "lb", "tb", "tr", "lr", "lr", "lb", "t", "tb", "tr" },
	{ "lr", "lt", "tb", "rb", "lr", "lr", "lt", "trb", "lb", "rb" },
	{ "l", "tb", "rb", "lt", "r", "lt", "tb", "rb", "ltr", "lr" },
	{ "lt", "trb", "lt", "tb", "t", "tb", "trb", "lt", "tb", "r" }};

	private string[,] maze5 = new string[,] {
	{ "lt", "tb", "tb", "b", "tb", "br", "tlb", "b", "tb", "trb" },
	{ "lb", "tb", "tb", "tr", "tlb", "t", "rb", "l", "tb", "rb" },
	{ "lr", "lb", "br", "lb", "br", "tlb", "r", "lr", "lbr", "lr" },
	{ "lr", "lr", "lt", "tr", "lt", "br", "lr", "lt", "tr", "lr" },
	{ "l", "tr", "tlb", "br", "lb", "rt", "lt", "tb", "tb", "r" },
	{ "lr", "lb", "tb", "r", "lt", "rb", "ltb", "tb", "rb", "ltr" },
	{ "l", "tr", "tlb", "tr", "lb", "t", "tb", "rb", "lt", "rb" },
	{ "lr", "lb", "tb", "tb", "tr", "lbr", "lb", "tr", "lb", "tr" },
	{ "lr", "lt", "tb", "rb", "ltb", "t", "t", "trb", "lr", "lbr" },
	{ "lt", "tb", "tbr", "lt", "tb", "tb", "tb", "tb", "t", "rt" }};

	private string[,] maze6 = new string[,] {
	{ "l", "br", "lbr", "lb", "b", "trb", "lb", "b", "tb", "rb" },
	{ "lr", "lr", "lr", "lr", "lt", "tb", "tr", "lt", "br", "lr" },
	{ "lr", "lt", "tr", "lr", "lb", "tb", "br", "lbr", "lr", "lr" },
	{ "l", "tb", "br", "lr", "lr", "lb", "tr", "lr", "lr", "ltr" },
	{ "ltr", "lb", "r", "lt", "tr", "lt", "b", "tr", "lt", "br" },
	{ "lb", "tr", "lrt", "lb", "tb", "tb", "tr", "lbr", "lb", "tr" },
	{ "lr", "lb", "tb", "tr", "tlb", "tb", "rb", "lr", "lt", "rb" },
	{ "lr", "lr", "tlb", "b", "tb", "tb", "r", "tl", "tb", "tr" },
	{ "l", "tr", "lb", "tr", "lb", "trb", "lr", "lt", "b", "br" },
	{ "lt", "tb", "tr", "tlb", "t", "tb", "t", "tb", "tr", "ltr" }};

	private string[,] maze7 = new string[,] {
	{ "l", "b", "br", "ltb", "b", "tb", "tb", "br", "ltb", "br" },
	{ "lr", "lr", "lt", "b", "tr", "lb", "br", "lr", "lb", "r" },
	{ "ltr", "lr", "tlb", "tr", "lb", "tr", "lr", "lt", "tr", "lr" },
	{ "lb", "r", "lb", "tb", "tr", "lb", "tr", "tlb", "tb", "tr" },
	{ "lr", "lr", "lr", "ltb", "br", "lt", "tb", "tb", "tb", "rb" },
	{ "lr", "ltr", "lt", "tb", "r", "lb", "trb", "lb", "tb", "r" },
	{ "lr", "lb", "b", "trb", "tl", "r", "lb", "tr", "lbr", "lr" },
	{ "l", "tr", "lt", "tb", "tb", "tr", "lr", "lbr", "l", "tr" },
	{ "lr", "lb", "br", "lb", "tb", "br", "lr", "l", "tr", "lbr" },
	{ "lt", "tr", "ltr", "ltr", "blt", "t", "tr", "lt", "tb", "tr" }};

	private string[,] maze8 = new string[,] {
	{ "l", "trb", "lb", "b", "tb", "br", "lb", "tb", "br", "lbr" },
	{ "l", "br", "lr", "lr", "lbr", "lr", "lt", "br", "lr", "lr" },
	{ "lr", "lt", "tr", "lr", "lr", "lt", "tb", "tr", "lr", "lr" },
	{ "lt", "br", "lbr", "lr", "lt", "b", "tb", "tb", "t", "r" },
	{ "lb", "tr", "lr", "lt", "br", "lt", "trb", "lb", "br", "lr" },
	{ "lt", "tb", "r", "lb", "r", "lb", "br", "lr", "lr", "ltr" },
	{ "lbr", "lb", "tr", "lr", "ltr", "ltr", "lt", "tr", "lt", "br" },
	{ "lr", "lr", "lbr", "lt", "tb", "tb", "b", "tb", "rb", "lr" },
	{ "lr", "lt", "r", "lb", "br", "lbr", "lr", "ltb", "tr", "lr" },
	{ "tl", "tb", "t", "tr", "lt", "tr", "lt", "tb", "tb", "tr" }};
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
