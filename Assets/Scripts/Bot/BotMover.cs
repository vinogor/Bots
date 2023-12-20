using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2;

    private Vector3 _targetPosition;
    private bool _canMoving;

    private void Start()
    {
        _canMoving = false;
    }

    private void Update()
    {
        if (_canMoving && transform.position != _targetPosition)
        {
            transform.LookAt(_targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }

        if (transform.position == _targetPosition)
        {
            _canMoving = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _targetPosition);
    }

    public void SetTarget(Vector3 position)
    {
        _canMoving = true;
        _targetPosition = new Vector3(position.x, transform.localScale.y / 2, position.z);
    }
}