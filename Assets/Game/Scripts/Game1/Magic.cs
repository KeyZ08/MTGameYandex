using System.Linq;
using UnityEngine;

public class Magic : MonoBehaviour
{
    private Transform _tr;
    private Rigidbody2D _rb;
    private Transform _target;

    public float MovementSpeed = 3;

    private void Start()
    {
        var objs = FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID).Where(o => o.isTarget == false).ToArray();
        _target = objs.Length == 0 ? null : objs[objs.Length - 1].Center;
        objs[objs.Length - 1].isTarget = true;
        _tr = transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_rb.position.x > 20) Destroy(gameObject);
        _rb.MovePosition(Vector2.MoveTowards(_tr.position, _target == null ? (Vector2)_tr.position + Vector2.right * 5 : _target.position, Time.fixedDeltaTime * MovementSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Death();
            Destroy(gameObject);
        }
    }
}
