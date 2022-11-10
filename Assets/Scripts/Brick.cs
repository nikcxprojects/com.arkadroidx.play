using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Brick : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _states = new Sprite[0];
    private int _health;
    private int _points = 100;

    [SerializeField] private bool _isBreakable;
    [SerializeField] private AudioClip _hitClip;

    public int points => _points;
    public bool isBreakable => _isBreakable;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetBrick();
    }

    public void ResetBrick()
    {
        gameObject.SetActive(true);

        if (!_isBreakable)
        {
            _health = _states.Length;
            _spriteRenderer.sprite = _states[_health - 1];
        }
    }

    private void Hit()
    {
        if (_isBreakable) {
            return;
        }

        _health--;
        AudioManager.instance.PlayAudio(_hitClip);

        if (_health <= 0) gameObject.SetActive(false);
        else  _spriteRenderer.sprite = _states[_health - 1];
        

        FindObjectOfType<GameManager>().Hit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball") {
            Hit();
        }
    }

}
