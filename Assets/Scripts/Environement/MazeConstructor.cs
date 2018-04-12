using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeConstructor : MonoBehaviour {

	[SerializeField] private Material floorMat;
	[SerializeField] private Material wallMat;
	[SerializeField] private Material pillarMat;
	[SerializeField] private Material ceilMat;

	public float roomWidth = 3.75f;
	public float wallHeight = 3.5f;
	public float wallWidth = 1.2f;

	public int rows;
	public int columns;
	private int sizeX {get {return rows * 2 + 1;}}
	private int sizeY {get {return columns * 2 + 1;}}
	public Vector2[] emptyPositions;
	public Vector2[] obstaclePositions;

	public int [,] data {get; private set;}

	private MazeDataGenerator dataGenerator;
	private MazeMeshGenerator meshGenerator;

	public GameObject generatedMaze {get; private set;}
	private List<NavMeshObstacle> generatedObstacles;

	void OnDrawGizmos() {
		Vector3 size = Vector3.zero;
		size.x = (roomWidth + wallWidth) * (sizeX / 2);
		size.y = wallHeight;
		size.z = (roomWidth + wallWidth) * (sizeY / 2);
		Gizmos.color = Color.yellow;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(size / 2, size);
	}

	void Awake() {
		data = new int[,] {
			{1, 1, 1},
			{1, 0, 1},
			{1, 1, 1}
		};
		dataGenerator = new MazeDataGenerator();
		meshGenerator = new MazeMeshGenerator();
		generatedMaze = null;
		generatedObstacles = new List<NavMeshObstacle>();
	}

	// Use this for initialization
	void Start() {
		GenerateMaze(sizeY, sizeX);
		GenerateObstacles();
	}

	// Update is called once per frame
	void Update() {

	}

	public Vector3[] SurrounedPositions() {
		return null;
	}

	public void GenerateMaze(int sizeRows, int sizeCols) {
		if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
			Debug.LogError("Odd numbers work better for dungeon size");
		
		dataGenerator.emptyPositions = emptyPositions;
		dataGenerator.obstaclePositions = obstaclePositions;
		data = dataGenerator.FromDimensions(sizeRows, sizeCols);
		DisplayMaze();
	}

	public void GenerateObstacles() {
		foreach(var obstacle in generatedObstacles) {
			Destroy(obstacle.gameObject);
		}
		generatedObstacles.Clear();
		if (generatedMaze == null)
			return ;
		int rMax = data.GetUpperBound(0);
		int cMax = data.GetUpperBound(1);
		for (int i = 0; i <= rMax; i++) {
			for (int j = 0; j <= cMax; j++) {
				float widthX = meshGenerator.roomWidth;
				float widthY = meshGenerator.roomWidth;
				float height = meshGenerator.wallHeight;
				if (i % 2 == 0)
					widthX = meshGenerator.wallWidth;
				if (j % 2 == 0)
					widthY = meshGenerator.wallWidth;
				float posX = (meshGenerator.roomWidth + meshGenerator.wallWidth) * (i / 2 + (i % 2) / 2.0f);
				float posY = (meshGenerator.roomWidth + meshGenerator.wallWidth) * (j / 2 + (j % 2) / 2.0f);

				Vector3 center = new Vector3(0, meshGenerator.wallHeight / 2, 0);
				if (data[i, j] == 1) {
					GameObject go = new GameObject();
					go.transform.SetParent(generatedMaze.transform);
					go.transform.localPosition = new Vector3(posY, 0, posX);
					go.transform.localRotation = Quaternion.identity;
					NavMeshObstacle obstacle = go.AddComponent<NavMeshObstacle>();
					obstacle.carving = true;
					obstacle.center = center;
					obstacle.size = new Vector3(widthY,
												height,
												widthX);
					generatedObstacles.Add(obstacle);
				}
			}
		}
	}

	void DisplayMaze() {
		GameObject go = new GameObject();
		go.transform.SetParent(transform);
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.name = "Procedural Maze";

		MeshFilter mf = go.AddComponent<MeshFilter>();
		meshGenerator.roomWidth = roomWidth;
		meshGenerator.wallHeight = wallHeight;
		meshGenerator.wallWidth = wallWidth;
		meshGenerator.drawFloor = floorMat != null;
		meshGenerator.drawPillars = pillarMat != null;
		mf.mesh = meshGenerator.FromData(data);

		MeshCollider mc = go.AddComponent<MeshCollider>();
		mc.sharedMesh = mf.mesh;

		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.materials = new Material[4] {floorMat, wallMat, pillarMat, ceilMat};

		generatedMaze = go;
	}

	public void DestroyMaze() {
		if (generatedMaze)
			Destroy(generatedMaze);
		generatedMaze = null;
	}

}

public class MazeDataGenerator {

	public float placementThreshold;
	public Vector2[] emptyPositions;
	public Vector2[] obstaclePositions;

	public MazeDataGenerator() {
		placementThreshold = .1f;
	}

	public int[,] FromDimensions(int sizeRows, int sizeCols, bool emptyMaze = false) {
		int[,] maze = new int[sizeRows, sizeCols];

		int rMax = maze.GetUpperBound(0);
		int cMax = maze.GetUpperBound(1);

		for (int i = 0; i <= rMax; i++) {
			for (int j = 0; j <= cMax; j++) {
				if (i == 0 || j == 0 || i == rMax || j == cMax) {
					maze[i, j] = 1;
				} else if (!emptyMaze && (i % 2 == 0 && j % 2 == 0)) {
					maze[i, j] = 1;

					int a = Random.value < 0.5 ? 0 : (Random.value < 0.5 ? -1 : 1);
					int b = a != 0 ? 0 : (Random.value < 0.5 ? -1 : 1);
					maze[i + a, j + b] = 1;
				}
			}
		}
		foreach (var position in emptyPositions) {
			int py = (int)(position.x * 2);
			int px = (int)(position.y * 2);
			if (px <= rMax && px >= 0 && py <= cMax && py >= 0)
				maze[px, py] = 0;
		}
		foreach (var position in obstaclePositions) {
			int py = (int)(position.x * 2);
			int px = (int)(position.y * 2);
			if (px <= rMax && px >= 0 && py <= cMax && py >= 0)
				maze[px, py] = 1;
		}
		return maze;
	}

}

public class MazeMeshGenerator {

	public float roomWidth;
	public float wallHeight;
	public float wallWidth;

	public bool drawFloor;
	public bool drawPillars;

	private float widthX;
	private float widthY;
	private float height;
	private float posX;
	private float posY;
	private int rMax;
	private int cMax;
	private float halfH;
	private int[,] data;

	public MazeMeshGenerator() {
		roomWidth = 3.75f;
		wallHeight = 3.5f;
		wallWidth = 1.0f;
		drawFloor = true;
	}

	public Mesh FromData(int[,] tabData) {
		data = tabData;
		Mesh maze = new Mesh();

		List<Vector3> newVertices = new List<Vector3>();
		List<Vector2> newUVs = new List<Vector2>();

		maze.subMeshCount = 4;
		List<int> floorTriangles = new List<int>();
		List<int> wallTriangles = new List<int>();
		List<int> pillarTriangles = new List<int>();
		List<int> ceilTriangles = new List<int>();
		rMax = data.GetUpperBound(0);
		cMax = data.GetUpperBound(1);
		halfH = wallHeight * .5f;

		for (int i = 0; i <= rMax; i++) {
			for (int j = 0; j <= cMax; j++) {
				
				widthX = roomWidth;
				widthY = roomWidth;
				height = wallHeight;
				if (i % 2 == 0)
					widthX = wallWidth;
				if (j % 2 == 0)
					widthY = wallWidth;

				posX = (roomWidth + wallWidth) * (i / 2 + (i % 2) / 2.0f);
				posY = (roomWidth + wallWidth) * (j / 2 + (j % 2) / 2.0f);

				if (data[i, j] != 1) {

					if ((i % 2 == 0 || j % 2 == 0) && drawPillars)
						DrawFloorAt(i, j, ref newVertices, ref newUVs, ref floorTriangles, ref pillarTriangles);
					else
						DrawFloorAt(i, j, ref newVertices, ref newUVs, ref floorTriangles, ref wallTriangles);
					
				} else {

					if ((i % 2 == 0 && j % 2 == 0) && drawPillars)
						DrawCeilAt(i, j, ref newVertices, ref newUVs, ref ceilTriangles, ref pillarTriangles);
					else
						DrawCeilAt(i, j, ref newVertices, ref newUVs, ref ceilTriangles, ref wallTriangles);

				}
			}
		}

		maze.vertices = newVertices.ToArray();
		maze.uv = newUVs.ToArray();

		maze.SetTriangles(floorTriangles.ToArray(), 0);
		maze.SetTriangles(wallTriangles.ToArray(), 1);
		maze.SetTriangles(pillarTriangles.ToArray(), 2);
		maze.SetTriangles(ceilTriangles.ToArray(), 3);

		maze.RecalculateNormals();

		return maze;
	}

	private void DrawFloorAt(int i, int j,
		ref List<Vector3> newVertices, ref List<Vector2> newUVs,
		ref List<int> floorTriangles, ref List<int> wallTriangles) {

		if (drawFloor) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY, 0, posX),
				Quaternion.LookRotation(Vector3.up),
				new Vector3(widthY, widthX, 1)
			), ref newVertices, ref newUVs, ref floorTriangles);
		}

		if (!(i - 1 < 0) && data[i-1, j] == 1) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY, halfH, posX - .5f * widthX),
				Quaternion.LookRotation(Vector3.forward),
				new Vector3(widthY, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (!(j + 1 > cMax) && data[i, j+1] == 1) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY + .5f * widthY, halfH, posX),
				Quaternion.LookRotation(Vector3.left),
				new Vector3(widthX, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (!(j - 1 < 0) && data[i, j-1] == 1) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY - .5f * widthY, halfH, posX),
				Quaternion.LookRotation(Vector3.right),
				new Vector3(widthX, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (!(i + 1 > rMax) && data[i+1, j] == 1) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY, halfH, posX + .5f * widthX),
				Quaternion.LookRotation(Vector3.back),
				new Vector3(widthY, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

	}

	private void DrawCeilAt(int i, int j,
		ref List<Vector3> newVertices, ref List<Vector2> newUVs,
		ref List<int> floorTriangles, ref List<int> wallTriangles) {

		AddQuad(Matrix4x4.TRS(
			new Vector3(posY, height, posX),
			Quaternion.LookRotation(Vector3.up),
			new Vector3(widthY, widthX, 1)
		), ref newVertices, ref newUVs, ref floorTriangles);

		if (i == 0) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY, halfH, posX - .5f * widthX),
				Quaternion.LookRotation(Vector3.back),
				new Vector3(widthY, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (i == rMax) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY, halfH, posX + .5f * widthX),
				Quaternion.LookRotation(Vector3.forward),
				new Vector3(widthY, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (j == 0) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY - .5f * widthY, halfH, posX),
				Quaternion.LookRotation(Vector3.left),
				new Vector3(widthX, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

		if (j == cMax) {
			AddQuad(Matrix4x4.TRS(
				new Vector3(posY + .5f * widthY, halfH, posX),
				Quaternion.LookRotation(Vector3.right),
				new Vector3(widthX, height, 1)
			), ref newVertices, ref newUVs, ref wallTriangles, true);
		}

	}

	private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
		ref List<Vector2> newUVs, ref List<int> newTriangles, bool wall = false)
	{
		int index = newVertices.Count;

		// corners before transforming
		Vector3 vert1 = matrix.MultiplyPoint3x4(new Vector3(-.5f, -.5f, 0));
		Vector3 vert2 = matrix.MultiplyPoint3x4(new Vector3(-.5f, .5f, 0));
		Vector3 vert3 = matrix.MultiplyPoint3x4(new Vector3(.5f, .5f, 0));
		Vector3 vert4 = matrix.MultiplyPoint3x4(new Vector3(.5f, -.5f, 0));

		newVertices.Add(vert1);
		newVertices.Add(vert2);
		newVertices.Add(vert3);
		newVertices.Add(vert4);

		if (wall) {
			newUVs.Add(new Vector2(1, 0));
			newUVs.Add(new Vector2(1, 1));
			newUVs.Add(new Vector2(0, 1));
			newUVs.Add(new Vector2(0, 0));
		} else {
			float tw = roomWidth + wallWidth;
			newUVs.Add(new Vector2(vert4.x / tw, vert4.z / tw));
			newUVs.Add(new Vector2(vert3.x / tw, vert3.z / tw));
			newUVs.Add(new Vector2(vert2.x / tw, vert2.z / tw));
			newUVs.Add(new Vector2(vert1.x / tw, vert1.z / tw));
		}

		newTriangles.Add(index+2);
		newTriangles.Add(index+1);
		newTriangles.Add(index);

		newTriangles.Add(index+3);
		newTriangles.Add(index+2);
		newTriangles.Add(index);
	}
}