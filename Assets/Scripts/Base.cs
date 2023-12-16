using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private ResourceSpawner _resourceSpawner;
    private List<BotCollector> _bots;
    private List<Resource> _resources;

    int _maxCollidersForSearch = 10;
    int _waitTimeForSendingBot = 3;
    private int _collectedResources = 0;

    private void Start()
    {
        // нужен чтобы искать ресурсы только в его области 
        _resourceSpawner = FindObjectOfType<ResourceSpawner>();

        FindBots();
        StartCoroutine(Work());
    }

    private IEnumerator Work()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_waitTimeForSendingBot);

        while (true)
        {
            yield return waitTime;
            FindFreeResources();
            SendBotToCollect();
        }
    }

    private void SendBotToCollect()
    {
        if (TryGetFreeBot(out BotCollector freeBot) && TryGetFreeResource(out Resource freeResource))
        {
            freeResource.SetIsMarkedForHarvest();
            freeBot.SetResourceToCollect(freeResource);
        }
    }

    private bool TryGetFreeResource(out Resource resource)
    {
        if (_resources.Count == 0)
        {
            resource = null;
            return false;
        }

        resource = _resources[0];
        return true;
    }

    private bool TryGetFreeBot(out BotCollector freeBot)
    {
        foreach (BotCollector bot in _bots)
        {
            if (bot.IsFree)
            {
                freeBot = bot;
                return true;
            }
        }

        freeBot = null;
        return false;
    }

    private void FindBots()
    {
        _bots = new List<BotCollector>();
        Collider[] colliders = new Collider[_maxCollidersForSearch];

        // а есть ли способ указать в OverlapBoxNonAlloc слой не который игнорить, а наоборот, чтобы только его и учитывать? 
        var size = Physics.OverlapBoxNonAlloc(
            transform.position, transform.localScale / 2, colliders, transform.localRotation);

        for (var i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out BotCollector bot))
            {
                _bots.Add(bot);
            }
        }

        Debug.Log("bots.Count = " + _bots.Count);
    }

    private void FindFreeResources()
    {
        _resources = new List<Resource>();
        Transform resourceArea = _resourceSpawner.transform;
        Collider[] colliders = new Collider[_maxCollidersForSearch];
        var size = Physics.OverlapBoxNonAlloc(
            resourceArea.position, resourceArea.localScale / 2, colliders, resourceArea.localRotation);

        for (var i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out Resource resource) && resource.GetIsMarkedForHarvest() == false)
            {
                _resources.Add(resource);
            }
        }

        Debug.Log("FREE _resources.Count = " + _resources.Count);
    }

    public void IncreaseCollectedResourceCounter()
    {
        _collectedResources++;
        Debug.Log("_collectedResources = " + _collectedResources);
    }
}