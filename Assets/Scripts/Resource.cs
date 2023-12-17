using UnityEngine;

public class Resource : MonoBehaviour, IBusyness
{
    private bool _isBusy = false;

    public void SetBusy()
    {
        _isBusy = true;
    }

    public bool IsBusy()
    {
        return _isBusy;
    }
}