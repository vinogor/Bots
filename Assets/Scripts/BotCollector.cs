using UnityEngine;

public class BotCollector : MonoBehaviour, IBusyness
{
    private Resource _resource;
    private Vector3 _basePosition;
    private BotMover _botMover;
    private bool _isBusy;

    private void Start()
    {
        _basePosition = transform.position;
        _botMover = GetComponent<BotMover>();
        _isBusy = false;
    }

    public void SetResourceToCollect(Resource resource)
    {
        _resource = resource;
        _botMover.SetTarget(_resource.transform.position);
        _isBusy = true;
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
                botBase.IncreaseCollectedResourceCounter();
            }
        }
    }

    public bool IsBusy()
    {
        return _isBusy;
    }
}