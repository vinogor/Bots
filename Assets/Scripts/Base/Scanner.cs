using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private ResourceSpawner _resourceSpawner;
    private int _maxCollidersForSearch = 30;

    private void Start()
    {
        _resourceSpawner = FindObjectOfType<ResourceSpawner>();
    }

    public List<Resource> FindResources()
    {
        List<Resource> resources = FindObjects<Resource>(_resourceSpawner.transform);
        return resources;
    }

    public List<BotCollector> FindBots()
    {
        List<BotCollector> bots = FindObjects<BotCollector>(transform);

        foreach (BotCollector bot in bots)
        {
            bot.SetBasePosition(transform.position);
        }

        return bots;
    }

    private List<T> FindObjects<T>(Transform scanArea)
    {
        List<T> result = new List<T>();

        Collider[] colliders = new Collider[_maxCollidersForSearch];
        var size = Physics.OverlapBoxNonAlloc(
            scanArea.position, scanArea.localScale / 2, colliders, scanArea.localRotation);

        for (var i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out T element))
            {
                result.Add(element);
            }
        }

        return result;
    }
}