using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public void InitBase(Flag flag)
    {
        Vector3 flagPosition = flag.transform.position;
        Vector3 basePosition = new Vector3(flagPosition.x, transform.position.y, flagPosition.z);
        Instantiate(_basePrefab, basePosition, Quaternion.identity);
        Destroy(flag.gameObject);
    }
}