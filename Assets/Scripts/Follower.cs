using UnityEngine;

public class Follower : MonoBehaviour
{
    //Follows the selected target
    
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    
    private void FixedUpdate()
    {
        _transform.position = target.position + offset;
    }
}
