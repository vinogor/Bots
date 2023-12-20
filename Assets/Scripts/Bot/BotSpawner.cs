using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private BotCollector _botPrefab;

    public void InitBot()
    {
        Vector3 position =
            new Vector3(transform.position.x, _botPrefab.transform.localScale.y / 2, transform.position.z);
        Instantiate(_botPrefab, position, Quaternion.identity);
    }
}