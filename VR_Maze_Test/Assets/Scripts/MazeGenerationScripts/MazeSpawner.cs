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

	public InputManager im;
	public UIManager uim;
	public StartGoal sg;
	public Logger l;
	public int mazeCount;

	private List<GameObject> oldWalls = new List<GameObject>();
	public bool isCircle;
	public bool firstMazeCreated;
	private BasicMazeGenerator mMazeGenerator = null;
	public List<int> mazePicks = new List<int>();
	public int mazeId;
	public GameObject[] cirlceMazes;
	public GameObject circleMaze;
	public GameObject[] cirlceMessages;
	public GameObject circleMessage;
	private List<string[,]> mazes = new List<string[,]>();
	private List<UIManager.MovementType> movementList = new List<UIManager.MovementType>();

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
	{ "l", "b", "br", "ltb", "b", "brt" },
	{ "ltr", "lr", "lt", "br", "l", "br" },
	{ "lb", "tr", "lbr", "lr", "ltr", "lr" },
	{ "lr", "lbr", "l", "tr", "lb", "r" },
	{ "lr", "lt", "t", "t", "tr", "lr" }, 
	{ "lt", "tb", "trb", "ltb", "tb", "rt" }};

	private string[,] maze8 = new string[,] {
	{ "l", "tb", "tb", "tb", "br", "lbr" },
	{ "l", "tb", "br", "lb", "tr", "lr" },
	{ "l", "trb", "lr", "lr", "tlb", "r" },
	{ "ltr", "lb", "tr", "lt", "br", "lr" },
	{ "tlb", "tr", "lb", "trb", "lt", "r" },
	{ "ltb", "tb", "t", "tb", "tb", "rt" }};

	void Start () {
		for (int i = 0; i< 8; i++)
        {
			mazePicks.Add(i);
        }
		mazes.Add(maze5);
		mazes.Add(maze6);
		mazes.Add(maze7);
		mazes.Add(maze8);
		movementList.Add(UIManager.MovementType.Teleport);
		movementList.Add(UIManager.MovementType.Dash);
		movementList.Add(UIManager.MovementType.Walk);
		movementList.Add(UIManager.MovementType.Fog);

		movementList.Add(UIManager.MovementType.Teleport);
		movementList.Add(UIManager.MovementType.Dash);
		movementList.Add(UIManager.MovementType.Walk);
		movementList.Add(UIManager.MovementType.Fog);

		mazeCount = 0;
		PickRandomMaze();
	}

	public void ChangewireMove(int id)
    {
		uim.SetMovementType(movementList[id]);
		im.ChangeMovement();

		if (uim.GetMovementMode() == UIManager.MovementType.Walk)
		{
			uim.SetWireframeMode(UIManager.WireframeMode.Auto);
			im.ChangeWireframe();
		}
		else
		{
			uim.SetWireframeMode(UIManager.WireframeMode.Off);
			im.ChangeWireframe();
		}
	}

	public void ShowIntermission()
    {
		if (firstMazeCreated)
		{

			if (!isCircle)
			{
				oldWalls = new List<GameObject>();
				foreach (Transform child in this.transform)
				{


					oldWalls.Add(child.gameObject);
					child.GetComponent<ShaderChanger>().enabled = false;
					Destroy(child.gameObject);

				}
			}
			else
			{
				if (circleMaze != null)
				{
					Destroy(circleMaze);
					Destroy(circleMessage);
				}
			}
		}
		if (mazePicks.Count == 0)
        {
			PickRandomMaze();
        }
        else
        {
			im.canChangeMaze = true;
			im.ShowIntermissionMessage();
		}
		
    }

	public int GetMazeCount()
    {
		return mazeCount;
    }

	public void PickRandomMaze()
    {

		bool newMaze = true;
		mazeCount += 1;
		if (mazePicks.Count == 0)
		{

			newMaze = false;
		}
		
        if (newMaze)
        {
			mazeId = mazePicks[UnityEngine.Random.Range(0, mazePicks.Count)];
			mazePicks.Remove(mazeId);


			firstMazeCreated = true;

			if (mazeId < 4)
			{
				im.ChangeTutorialText(0);
				isCircle = false;
				ChangewireMove(mazeId);

				//Generate square maze
				GenerateMaze(newMaze);
				sg.Spawn(mazeId);

			}
			else
			{
				im.ChangeTutorialText(1);
				isCircle = true;
				ChangewireMove(mazeId);

				mazeId -= 4;
				GenerateCircleMaze(newMaze);
				
				sg.RequestEnding(StartGoal.ending.orientationCheck);
				
			}
        }
        else
        {
			// activate end message
			im.SetFog(false);
			im.activateEndMessage(3);
			l.UploadLogs();
        }
		
	}

	public void GenerateCircleMaze(bool newMaze)
    {

		if (newMaze)
        {
			circleMaze = Instantiate(cirlceMazes[mazeId], new Vector3(-2f, 0.5f, 15f), Quaternion.Euler(-90f, 180f, 0f));
			circleMessage = Instantiate(cirlceMessages[mazeId]);
			circleMessage.transform.parent = transform;
		}
		
	}

	public void GenerateMaze(bool newMaze)
    {
		
		GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
		int removeCounter = 0;
		
		if (newMaze)
        {
			
			string[,] maze = mazes[mazeId];
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
