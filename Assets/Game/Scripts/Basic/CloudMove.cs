using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public Transform ResetPosition;
    public float speed = 10f;
    public Transform StartPosition;
    private Transform _tr;
    private Vector2 startPosition;
    private Vector2 resetPosition;

    private void Start()
    {
        _tr = transform;
        startPosition = _tr.localPosition;
        resetPosition = new Vector2(startPosition.x - (ResetPosition.localPosition.x - StartPosition.localPosition.x), startPosition.y);
        
    }

    void Update()
    {
        _tr.localPosition = Vector3.MoveTowards((Vector2)_tr.localPosition, resetPosition, Time.deltaTime * speed);
        var rect = GetComponent<RectTransform>();
        if ((Vector2)_tr.localPosition == resetPosition)
            _tr.localPosition = startPosition;
    }
}
