using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 20;
    public int cols = 8;
    public float radius = 0.32f;

    [Header("References")]
    public GameObject[] bubblePrefabs;
    public Transform ballPool;

    [HideInInspector]
    public Bubble[,] grid;

    void Awake()
    {
        if (ballPool == null)
        {
            GameObject pool = new GameObject("BallPool");
            ballPool = pool.transform;
        }
    }

    void Start()
    {
        grid = new Bubble[rows, cols];
        SpawnInitialGrid();
    }

    void SpawnInitialGrid()
    {
        int initRows = rows / 2;
        for (int r = 0; r < initRows; r++)
        {
            SpawnRow(r);
        }
    }

    void SpawnRow(int r)
    {
        int ballsInRow = (r % 2 == 0) ? cols : cols - 1;

        for (int c = 0; c < ballsInRow; c++)
        {
            Vector2 pos = GetWorldPos(r, c);
            int id = Random.Range(0, bubblePrefabs.Length);
            var bubbleGO = Instantiate(bubblePrefabs[id], pos, Quaternion.identity, ballPool);
            var bubble = bubbleGO.GetComponent<Bubble>();

            bubble.colorId = id;
            bubble.row = r;
            bubble.col = c;
            grid[r, c] = bubble;
            bubble.Init(this);

            bubbleGO.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public Vector2 GetWorldPos(int r, int c)
    {
        float rowHeight = radius * 1.75f;
        float offsetX = (r % 2 == 0) ? 0f : radius;
        return new Vector2(
            transform.position.x + c * radius * 2f + offsetX,
            transform.position.y - r * rowHeight
        );
    }

    public void SnapBubble(Bubble b, Bubble hitBubble, Vector2 contactNormal)
    {
        Vector2 worldPos = b.transform.position;
        float rowHeight = radius * 1.75f;
        Vector2 local = worldPos - (Vector2)transform.position;

        int r = Mathf.Clamp(Mathf.RoundToInt(-local.y / rowHeight), 0, rows - 1);
        float offsetX = (r % 2 == 0) ? 0f : radius;
        int c = Mathf.Clamp(Mathf.RoundToInt((local.x - offsetX) / (radius * 2f)), 0, cols - 1);

        var candidates = new List<Vector2Int> { new Vector2Int(r, c) };
        candidates.AddRange(GetNeighbors(r, c));

        Vector2Int bestCell = new Vector2Int(-1, -1);
        Vector2 bestPos = Vector2.zero;
        float bestDist = float.MaxValue;

        foreach (var cell in candidates)
        {
            int rr = cell.x, cc = cell.y;
            if (grid[rr, cc] != null) continue;

            Vector2 nodePos = GetWorldPos(rr, cc);
            float d = (nodePos - worldPos).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                bestCell = cell;
                bestPos = nodePos;
            }
        }

        if (bestCell.x >= 0)
        {
            grid[bestCell.x, bestCell.y] = b;
            b.row = bestCell.x;
            b.col = bestCell.y;
            b.transform.position = bestPos;
            GameManager.Instance.OnBubblePlaced(b.row, b.col, b.colorId);
        }
        else
        {
            ObjectPool.Instance.Despawn(b.gameObject);
        }
    }

    public List<Vector2Int> GetNeighbors(int r, int c)
    {
        var list = new List<Vector2Int>();
        bool even = (r % 2 == 0);

        list.Add(new Vector2Int(r - 1, c));
        list.Add(new Vector2Int(r + 1, c));
        list.Add(new Vector2Int(r, c - 1));
        list.Add(new Vector2Int(r, c + 1));

        if (even)
        {
            list.Add(new Vector2Int(r - 1, c - 1));
            list.Add(new Vector2Int(r + 1, c - 1));
        }
        else
        {
            list.Add(new Vector2Int(r - 1, c + 1));
            list.Add(new Vector2Int(r + 1, c + 1));
        }

        list.RemoveAll(p =>
            p.x < 0 || p.x >= rows ||
            p.y < 0 || p.y >= cols
        );
        return list;
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            float rowHeight = radius * 1.75f;

            for (int r = 0; r < rows; r++)
            {
                int ballsInRow = (r % 2 == 0) ? cols : cols - 1;
                float offsetX = (r % 2 == 0) ? 0f : radius;

                for (int c = 0; c < ballsInRow; c++)
                {
                    float x = c * radius * 2f + offsetX;
                    float y = -r * rowHeight;
                    Vector2 pos = (Vector2)transform.position + new Vector2(x, y);

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(pos, radius * 0.95f);
                }
            }
        }
    }
}