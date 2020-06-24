using UnityEngine;

public class TouchController : MonoBehaviour
{
    //Refers to the bullet prefab
    [SerializeField] private Rigidbody bullet;
    //Refers to the placeHolder for bullet start point
    [SerializeField] private Transform placeHolder;
    
    //For touches start and end points
    private Vector2 _touchStartPos,_touchEndPos;
    
    private Vector2 _direction;
    
    private Vector3 _rotation;
    
    private Transform _transform;
    
    private Touch _touch;
    
    private Rigidbody _rb;

    private bool _hasMove = false;

    private float _directionX, _directionY;
    
    //For player moving speed
    private const float Speed = .1f;
    private const float BulletPower = 1000f;
    private const float BulletDestroyTime = 3f;
    private void Awake()
    {    
        //gameObject => Rigidbody
        _rb = GetComponent<Rigidbody>();
        //gameObject => Transform
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        TouchDetect();
        ChooseDirection();
        ChooseRotation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Touch detection for player touches
    private void TouchDetect()
    {
        // Track a single touch as a direction control
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase
            switch (_touch.phase)
            {
                // Record initial touch position
                case TouchPhase.Began:
                    _touchStartPos = _touch.position;
                    _hasMove = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    _touchEndPos = _touch.position;
                    _direction = _touchEndPos - _touchStartPos;
                    _hasMove = true;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    _direction = Vector2.zero;
                    Fire();
                    _hasMove = false;
                    break;
            }
        }
    }
    
    // Decade which direction to go
    private void ChooseDirection()
    {
        _directionX = Mathf.Clamp(_direction.x, -100, 100);
        _directionY = Mathf.Clamp(_direction.y, -100, 100);
    }

    // Decade which rotation to rotate
    private void ChooseRotation()
    {
        _rotation = new Vector3(_directionX,transform.position.y,_directionY);
    }

    //Provides the movement of the player
    private void Move()
    {
        if (_hasMove)
        {
            _rb.velocity = new Vector3(-_directionX*Speed,0,-_directionY*Speed);
            _transform.LookAt(_rotation);
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    //Allows the player to shoot 
    private void Fire()
    {
        if (!_hasMove) return;
        
        var bulletInstance = Instantiate(bullet,placeHolder.position,placeHolder.rotation);
        bulletInstance.AddForce(placeHolder.up* BulletPower);
        Destroy(bulletInstance.gameObject,BulletDestroyTime);
    }
}
