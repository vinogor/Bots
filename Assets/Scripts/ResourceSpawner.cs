using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private int _quantity = 10;
    [SerializeField] private Resource _resourcePrefab;

    private List<Resource> _resources;

    private void Start()
    {
        _resources = new List<Resource>();
        Vector3 center = transform.position;
        Vector3 scale = transform.lossyScale;

        for (int i = 0; i < _quantity; i++)
        {
            Resource resource = Instantiate(_resourcePrefab,
                new Vector3(
                    Random.Range(center.x - scale.x / 2, center.x + scale.x / 2),
                    _resourcePrefab.transform.localScale.y / 2,
                    Random.Range(center.z - scale.z / 2, center.z + scale.z / 2)),
                Quaternion.identity);
            _resources.Add(resource);
        }
    }
}