using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(BotSpawner), typeof(BaseSpawner))]
public class Base : MonoBehaviour
{
    private Scanner _scanner;
    private BotSpawner _botSpawner;
    private List<BotCollector> _bots;
    private List<Resource> _resources;
    private BasePlatform _basePlatform;
    private BaseSpawner _baseSpawner;
    private Flag _flag;
    private GoalOfResourceAccumulation _goal;

    private int _waitTimeForSendingBot = 1;
    private int _collectedResources = 0;
    private int _newBotCost = 3;
    private int _newBaseCost = 5;

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _botSpawner = GetComponent<BotSpawner>();
        _basePlatform = GetComponentInChildren<BasePlatform>();
        _baseSpawner = GetComponent<BaseSpawner>();
        _goal = GoalOfResourceAccumulation.NewBot;
        StartCoroutine(Work());
    }

    private IEnumerator Work()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_waitTimeForSendingBot);

        while (true)
        {
            yield return waitTime;
            _bots = _scanner.FindBots();
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

    public void IncreaseCollectedResourceCounter(BotCollector bot)
    {
        _collectedResources++;
        TrySpendResources(bot);
    }

    private void TrySpendResources(BotCollector bot)
    {
        switch (_goal)
        {
            case GoalOfResourceAccumulation.NewBot:
                if (_collectedResources >= _newBotCost)
                {
                    _collectedResources -= _newBotCost;
                    _botSpawner.InitBot();
                }

                break;

            case GoalOfResourceAccumulation.NewBase:
                if (_collectedResources >= _newBaseCost)
                {
                    _collectedResources -= _newBaseCost;
                    bot.SetTargetToMove(_flag.transform.position);
                    _baseSpawner.InitBase(_flag);
                    _goal = GoalOfResourceAccumulation.NewBot;
                }

                break;
        }
    }

    public void ChangeColorToGreen()
    {
        _basePlatform.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void ChangeColorToDefault()
    {
        _basePlatform.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void ChangeGoalToNewBase(Flag flag)
    {
        _flag = flag;
        _goal = GoalOfResourceAccumulation.NewBase;
    }

    enum GoalOfResourceAccumulation
    {
        NewBot,
        NewBase
    }
}