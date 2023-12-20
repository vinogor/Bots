using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Base))]
public class BaseClickReceiver : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Flag _flagPrefab;

    private Camera _camera;
    private Base _base;
    private GroundClickReceiver _groundClickReceiver;

    // TODO: сделать чтобы только одна база могла быть выбрана в один момент времени
    private bool _isBaseSelected;
    private Flag flag;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _groundClickReceiver = FindObjectOfType<GroundClickReceiver>();
        _groundClickReceiver.AddListener(ClickOnGround);
        _base = GetComponent<Base>();
    }

    private void ClickOnGround(RaycastHit raycastHit)
    {
        if (raycastHit.collider.gameObject.CompareTag("Ground") && _isBaseSelected)
        {
            if (flag == null)
            {
                flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }
            else
            {
                Destroy(flag.gameObject);
                flag = Instantiate(_flagPrefab, raycastHit.point, Quaternion.identity);
            }

            _base.ChangeColorToDefault();
            _isBaseSelected = false;
            _base.ChangeGoalToNewBase(flag);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = _camera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.gameObject.CompareTag("Base"))
            {
                if (_isBaseSelected)
                {
                    _isBaseSelected = false;
                    _base.ChangeColorToDefault();
                }
                else
                {
                    _isBaseSelected = true;
                    _base.ChangeColorToGreen();
                }
            }
        }
    }
}