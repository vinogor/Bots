using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GroundClickReceiver : MonoBehaviour, IPointerDownHandler
{
    private Camera _camera;
    private UnityEvent<RaycastHit> _clickOnGround = new();

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = _camera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            _clickOnGround.Invoke(raycastHit);
        }
    }

    public void AddListener(UnityAction<RaycastHit> action)
    {
        _clickOnGround.AddListener(action);
    }
}