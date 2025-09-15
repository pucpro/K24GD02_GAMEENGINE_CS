using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public BoardManager board;
    public UIManager uiManager;

    [Header("Match-3 Settings")]
    public int matchThreshold = 3;

   // [Header("Lose Settings")]
    //public float loseY = -4.5f;      // world-space Y below which you lose

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        board      = board      ?? Object.FindFirstObjectByType<BoardManager>();
        uiManager  = uiManager  ?? Object.FindFirstObjectByType<UIManager>();
    }

    void Update()
    {
        //CheckLoseByY();
    }

    /// <summary>
    /// Called by BoardManager after a bubble snaps into place.
    /// </summary>
    public void OnBubblePlaced(int r, int c, int colorId)
    {
        HandleMatches(r, c, colorId);
        RemoveFloating();
        // We no longer check row-based lose here.
    }

    private void HandleMatches(int r, int c, int color)
    {
        var matched = FloodFill(r, c, color);
        if (matched.Count >= matchThreshold)
        {
            foreach (var p in matched)
            {
                var b = board.grid[p.x, p.y];
                ObjectPool.Instance.Despawn(b.gameObject);
                board.grid[p.x, p.y] = null;
            }
            uiManager.AddScore(matched.Count * 10);
        }
    }

    private List<Vector2Int> FloodFill(int r, int c, int color)
    {
        var result = new List<Vector2Int>();
        bool[,] visited = new bool[board.rows, board.cols];
        DFS(r, c, color, visited, result);
        return result;
    }

    private void DFS(int r, int c, int color, bool[,] vis, List<Vector2Int> res)
    {
        if (r < 0 || r >= board.rows || c < 0 || c >= board.cols) return;
        if (vis[r, c] || board.grid[r, c] == null) return;
        if (board.grid[r, c].colorId != color) return;

        vis[r, c] = true;
        res.Add(new Vector2Int(r, c));

        foreach (var n in board.GetNeighbors(r, c))
            DFS(n.x, n.y, color, vis, res);
    }

    private void RemoveFloating()
    {
        int R = board.rows, C = board.cols;
        bool[,] connected = new bool[R, C];
        var queue = new Queue<Vector2Int>();

        // Start BFS from all bubbles in the topmost logical row (row = 0)
        for (int c = 0; c < C; c++)
        {
            if (board.grid[0, c] != null)
            {
                connected[0, c] = true;
                queue.Enqueue(new Vector2Int(0, c));
            }
        }

        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            foreach (var n in board.GetNeighbors(p.x, p.y))
            {
                if (!connected[n.x, n.y] && board.grid[n.x, n.y] != null)
                {
                    connected[n.x, n.y] = true;
                    queue.Enqueue(n);
                }
            }
        }

        // Any bubble not marked connected is floating → remove it
        for (int r = 0; r < R; r++)
        for (int c = 0; c < C; c++)
        {
            if (board.grid[r, c] != null && !connected[r, c])
            {
                ObjectPool.Instance.Despawn(board.grid[r, c].gameObject);
                board.grid[r, c] = null;
                uiManager.AddScore(5);
            }
        }
    }

    /// <summary>
    /// Every frame we check if any bubble's world-space Y has dropped below loseY.
    /// </summary>
    //private void CheckLoseByY()
    //{
    //    foreach (var bubble in board.bubbleParent.GetComponentsInChildren<Bubble>())
    //    {
    //        if (bubble.transform.position.y <= loseY)
    //        {
    //            uiManager.ShowLose();
    //            // Stop further updates
    //            enabled = false;
    //            return;
    //        }
    //    }
    //}
}