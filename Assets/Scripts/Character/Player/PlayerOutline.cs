using UnityEngine;

public class PlayerOutline : MonoBehaviour
{
    [Header("中心からのオフセット")]
    [SerializeField]
    private Vector3 _offset;

    [Header("レイの長さ")]
    [SerializeField]
    private float _rayLength = 10f;

    Vector3 _direction;
    private Vector3[] _rayPositions;
    private const string LAYER_OUTLINE = "Outline";
    private const string LAYER_DEFAULT = "Default";

    private VisibilityHandler _visibilityHandler;

    void Start()
    {
        _visibilityHandler = VisibilityHandler.Instance;
        _direction = Camera.main.transform.position - transform.position;
        _direction.x = 0;
        _direction.y = 0;
        _direction.Normalize();

        _rayPositions = new Vector3[]{
            _offset,
            new Vector3(-_offset.x, -_offset.y, _offset.z),
            new Vector3(_offset.x, -_offset.y, _offset.z),
            new Vector3(-_offset.x, _offset.y, _offset.z),
        };
    }

    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.x = 0;
        direction.y = 0;
        direction.Normalize();
        // 各方向にレイを飛ばす
        foreach (Vector3 origin in _rayPositions)
        {
            Debug.DrawRay(gameObject.transform.position + origin, direction * _rayLength, Color.red);
            if (Physics.Raycast(gameObject.transform.position + origin, direction, out RaycastHit hit, _rayLength))
            {
                if (hit.collider.CompareTag("Wall") && _visibilityHandler.IsClearRoomNum == -1)
                    gameObject.layer = LayerMask.NameToLayer(LAYER_OUTLINE);
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer(LAYER_DEFAULT);
            }
        }
    }
}
