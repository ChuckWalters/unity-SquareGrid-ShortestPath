using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour {

    [Header("Grid Size")]
    public int Width = 10;
    public int Height = 12;

    [Header("Cell")]
    public GameObject CellPrefab;
    public Vector2 CellOffsets = new Vector2(0.15f, 0.15f);

    [System.Serializable]
    public class Agent
    {
        public int PathDepth = 6;
        public Vector2 Position = Vector2.zero;
    }
    [Header("Character Agent")]
    public Agent Character = new Agent();

    [Header("Walls")]
    public List<Vector2> Walls = new List<Vector2>(20);

    private Cell[][] CellList;

    // Use this for initialization
    void Start () {
        CreateBoard();
        StartCoroutine("PositionAgent");
        StartCoroutine("AddWalls");
        StartCoroutine("FindPaths");
    }

    // Update is called once per frame
    void Update () {
		
	}

    void CreateBoard()
    {
        float startY = (Height / 2f) * CellOffsets.y;
        float startX = (-Width / 2f) * CellOffsets.x;

        CellList = new Cell[Height][];
        // Generate board
        for (int row = 0; row < Height; row++)
        {
            CellList[row] = new Cell[Width];
            for (int col = 0; col < Width; col++)
            {
                GameObject cellClone = Instantiate<GameObject>(CellPrefab);
                cellClone.name = "cell_" + row + "_" + col;
                cellClone.transform.SetParent(gameObject.transform, false);
                float x = startX + col * CellOffsets.x;
                float y = startY - row * CellOffsets.y;
                cellClone.transform.localPosition = new Vector3(x,y,1);
                CellList[row][col] = cellClone.GetComponent<Cell>();
            }
        }
    }

    IEnumerator PositionAgent()
    {
        yield return null;
        int row = Mathf.FloorToInt( Character.Position.y );
        int col = Mathf.FloorToInt( Character.Position.x );

        CellList[row][col].SetAgent();
    }
    IEnumerator AddWalls()
    {
        yield return null;
        for(int i=0; i< Walls.Count; i++)
        {
            int row = Mathf.FloorToInt(Walls[i].y);
            int col = Mathf.FloorToInt(Walls[i].x);

            CellList[row][col].SetWall();
        }
    }

    IEnumerator FindPaths()
    {
        yield return null;
        int row = Mathf.FloorToInt(Character.Position.y);
        int col = Mathf.FloorToInt(Character.Position.x);

        addDepth(1 ,row, col);

    }

    void addDepth(int currentDepth, int row, int col)
    {
        Cell N,E,S,W;

        N = addDepthMark(currentDepth, row - 1, col);
        E = addDepthMark(currentDepth, row, col + 1);
        S = addDepthMark(currentDepth, row + 1, col);
        W = addDepthMark(currentDepth, row, col - 1);

        if (currentDepth == Character.PathDepth)
        {
            return;
        }

        if (null != N)
        {
            addDepth(currentDepth + 1, row - 1, col);
        }
        if (null != E)
        {
            addDepth(currentDepth + 1, row, col + 1);
        }
        if (null != S)
        {
            addDepth(currentDepth + 1, row + 1, col);
        }
        if (null != W)
        {
            addDepth(currentDepth + 1, row, col - 1);
        }
    }

    Cell addDepthMark(int depth, int row, int col)
    {
        if(row<0 || row >= Height || col<0 || col >= Width)
        {
            return null;
        }

        Cell cell = CellList[row][col];
        if (null == cell || cell.State != Cell.CellState.Empty)
        {
            return null;
        }

        cell.SetVisited(depth.ToString());
        return cell;
    }
}
