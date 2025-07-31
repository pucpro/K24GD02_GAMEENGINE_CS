using UnityEngine;
using UnityEngine.InputSystem;

public class fly : MonoBehaviour
{
    [SerializeField] private float _velocity = 1.5f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Âm thanh")]
    [SerializeField] private AudioClip flapSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private Rigidbody2D _rb;
    private AudioSource _audioSource;
    private bool _isDead = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (_isDead) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.linearVelocity = Vector2.up * _velocity;
            _audioSource.PlayOneShot(flapSound);
        }
    }

    private void FixedUpdate()
    {
        float rotationZ = _rb.linearVelocity.y * _rotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) return;

        _isDead = true;

        _audioSource.PlayOneShot(hitSound);

        Invoke(nameof(PlayDeathAndGameOver), 0.2f);
    }

    private void PlayDeathAndGameOver()
    {
        _audioSource.PlayOneShot(deathSound);
        GameManager.instance.GameOver();
    }
}