using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public Transform shootPoint;
    public float shootSpeed = 10f;

    // Danh sách prefab bubble
    public GameObject[] bubblePrefabs;

    // SpriteRenderer dùng để vẽ bubble kế tiếp
    public SpriteRenderer nextPreview;

    private bool isBulletFlying = false;
    private int nextId; // index prefab kế tiếp

    void Start()
    {
        PrepareNext();  // Sinh lần đầu đồng thời cập nhật preview
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        Aim();
        if (!isBulletFlying && Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Aim()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouse - shootPoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, 10f, 170f);
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // Sinh chỉ số và cập nhật sprite vào nextPreview
    void PrepareNext()
    {
        // Chọn ngẫu nhiên prefab index
        nextId = Random.Range(0, bubblePrefabs.Length);

        // Lấy sprite từ prefab rồi gán cho nextPreview
        SpriteRenderer sr = bubblePrefabs[nextId]
            .GetComponent<SpriteRenderer>();
        if (sr != null && nextPreview != null)
            nextPreview.sprite = sr.sprite;
        else
            Debug.LogWarning("Thiếu SpriteRenderer hoặc nextPreview chưa gán");
    }

    void Shoot()
    {
        // 1. Spawn bubble dựa vào nextId
        GameObject prefab = bubblePrefabs[nextId];
        GameObject go = ObjectPool.Instance
            .Spawn(prefab, shootPoint.position, Quaternion.identity);

        // 2. Init Bubble
        Bubble b = go.GetComponent<Bubble>();
        b.colorId = nextId;               // Gán màu/ID phù hợp
        b.Init(GameManager.Instance.board);

        // 3. Thực hiện bắn
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 dir = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - (Vector2)shootPoint.position
        ).normalized;
        rb.linearVelocity = dir * shootSpeed;

        isBulletFlying = true;

        // 4. Chuẩn bị bubble kế tiếp ngay khi bắn xong
        PrepareNext();
    }

    // Gọi từ Bubble khi snap xong
    public void OnBulletSettled()
    {
        isBulletFlying = false;
    }
}