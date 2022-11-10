using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private float _speed = 30f;
    private float _maxBounceAngle = 75f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetPaddle();
    }

    public void ResetPaddle()
    {
        _rigidbody.velocity = Vector2.zero;
        transform.position = new Vector2(0f, transform.position.y);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            _direction = Vector2.left;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            _direction = Vector2.right;
        } else {
            _direction = Vector2.zero;
        }
#else
        Vector3 acceleration = Input.acceleration;
        if (acceleration.x > 0.10f)
            _direction = Vector2.right;
        else if (acceleration.x < -0.10f)
            _direction = Vector2.left;
        else
            _direction = Vector2.zero;
#endif
    }

    private void FixedUpdate()
    {
        if (_direction != Vector2.zero) {
            _rigidbody.AddForce(_direction * _speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Vector2 paddlePosition = transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float maxOffset = collision.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rigidbody.velocity);
            float bounceAngle = (offset / maxOffset) * _maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -_maxBounceAngle, _maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            ball.rigidbody.velocity = rotation * Vector2.up * ball.rigidbody.velocity.magnitude;
        }
    }

}
