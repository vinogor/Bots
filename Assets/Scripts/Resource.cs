using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isMarkedForHarvest = false;

    public void SetIsMarkedForHarvest()
    {
        _isMarkedForHarvest = true;
    }

    public bool GetIsMarkedForHarvest()
    {
        return _isMarkedForHarvest;
    }
}