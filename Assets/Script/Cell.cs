using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour {
    public enum CellState
    {
        Empty,
        Agent,
        Wall,
        Visited
    }
    public CellState State { get { return _state; } }

    #region Private Data
    private CellState _state = CellState.Empty;
    private TextMeshPro tmPro;
    #endregion

    #region Lifecycle
    // Use this for initialization
    void Start () {
        tmPro = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    #endregion

    public void SetVisited(string s)
    {
        _state = CellState.Visited;
        tmPro.SetText(s);
    }

    public void SetAgent()
    {
        _state = CellState.Agent;
        tmPro.SetText("X");
        tmPro.color = Color.black;
    }
    public void SetWall()
    {
        _state = CellState.Wall;
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
}
