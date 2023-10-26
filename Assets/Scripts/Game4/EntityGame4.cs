using UnityEngine;
using UnityEngine.Rendering;

public abstract class EntityGame4 : MonoBehaviour
{
    protected SortingGroup _row;
    protected Animator _anim;

    public Transform center;
    public EnemyesGame4 type { get; protected set; }

    public int RowPos
    {
        get { return _row.sortingOrder; }
        set { _row.sortingOrder = value; }
    }


    protected virtual void Awake()
    {
        _row = GetComponent<SortingGroup>();
        _anim = GetComponent<Animator>();
    }

    public virtual void MoveTo(Vector2 target)
    {
        Vector2 offsetPos = transform.position - center.position;
        transform.position = target + offsetPos;
    }

    public virtual void TPto(Vector2 target)
    {
        Vector2 offsetPos = transform.position - center.position;
        transform.position = target + offsetPos;
    }

    public abstract void Death();

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}

public enum EnemyesGame4
{
    Horse = 0,
    Demon1 = 1,
    Demon2 = 2,
}