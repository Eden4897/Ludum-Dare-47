using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FinishLine : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Assert.IsTrue(_rigidbody.isKinematic);
        Assert.IsTrue(_collider.isTrigger);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.OnFinish();
        }
    }

    private void OnDrawGizmos()
    {
        if (!PlayerManager.Instance.debug)
        {
            return;
        }

        if (!_collider)
        {
            _collider = GetComponent<Collider2D>();
        }

        var bounds = _collider.bounds;
        Gizmos.color = new Color(1, 0, 0, 0.15f);
        Gizmos.DrawCube(bounds.center, bounds.size);
    }
}