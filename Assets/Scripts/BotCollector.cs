using UnityEngine;

public class BotCollector : MonoBehaviour
{
    private Resource _resource;
    private Vector3 _basePosition;
    private BotMover _botMover;
    private bool _isFree;

    public bool IsFree => _isFree;

    private void Start()
    {
        _basePosition = transform.position;
        _botMover = GetComponent<BotMover>();
        _isFree = true;
    }

    public void SetResourceToCollect(Resource resource)
    {
        _resource = resource;
        _botMover.SetTarget(_resource.transform.position);
        _isFree = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            if (_resource == resource)
            {
                other.transform.parent = transform;
                _botMover.SetTarget(_basePosition);
            }
        }
        else if (other.TryGetComponent(out Base botBase))
        {
            _isFree = true;
            Resource childrenResource = GetComponentInChildren<Resource>();

            if (childrenResource != null)
            {
                Destroy(childrenResource.gameObject);
                botBase.IncreaseCollectedResourceCounter();
            }
        }
    }
}