using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class BotCollector : MonoBehaviour, IBusyness
{
    private Resource _resource;
    private Vector3 _basePosition;
    private BotMover _botMover;
    private bool _isBusy;

    private void Start()
    {
        _botMover = GetComponent<BotMover>();
        _isBusy = false;
    }

    public void SetResourceToCollect(Resource resource)
    {
        _resource = resource;
        _botMover.SetTarget(_resource.transform.position);
        _isBusy = true;
    }

    public void SetTargetToMove(Vector3 target)
    {
        _botMover.SetTarget(target);
        _isBusy = true;
    }

    public void SetBasePosition(Vector3 basePosition)
    {
        _basePosition = basePosition;
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
            _isBusy = false;
            Resource childrenResource = GetComponentInChildren<Resource>();

            if (childrenResource != null)
            {
                Destroy(childrenResource.gameObject);
                botBase.IncreaseCollectedResourceCounter(this);
            }
        }
    }

    public bool IsBusy()
    {
        return _isBusy;
    }
}