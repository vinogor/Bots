using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Base : MonoBehaviour
{
    private Scanner _scanner;
    private List<BotCollector> _bots;
    private List<Resource> _resources;

    private int _waitTimeForSendingBot = 1;
    private int _collectedResources = 0;

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _bots = _scanner.FindBots();
        StartCoroutine(Work());
    }

    private IEnumerator Work()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_waitTimeForSendingBot);

        while (true)
        {
            yield return waitTime;
            _resources = _scanner.FindResources();

            if (TryGetFreeObjects(out BotCollector freeBot, _bots) &&
                TryGetFreeObjects(out Resource freeResource, _resources))
            {
                yield return StartCoroutine(SendBotToCollect(freeBot, freeResource));
            }
        }
    }

    private IEnumerator SendBotToCollect(BotCollector bot, Resource resource)
    {
        resource.SetBusy();
        bot.SetResourceToCollect(resource);
        yield return null;
    }

    private bool TryGetFreeObjects<T>(out T freeObject, List<T> objects) where T : IBusyness
    {
        foreach (T currentObject in objects)
        {
            if (currentObject.IsBusy() == false)
            {
                freeObject = currentObject;
                return true;
            }
        }

        freeObject = default;
        return false;
    }

    public void IncreaseCollectedResourceCounter()
    {
        _collectedResources++;
        Debug.Log("_collectedResources = " + _collectedResources);
    }
}