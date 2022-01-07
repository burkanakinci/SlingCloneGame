using UnityEngine;

public class TntController : MonoBehaviour
{
    [SerializeField] private float radius = 125f;
    [SerializeField] private float explosionForce = 410f;
    private Rigidbody explosingRB;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Man") || other.collider.CompareTag("ManTnt"))
        {
            ExplosionTnt();
        }
    }
    private void ExplosionTnt()
    {
        for (int i = GameManager.Instance.cylinderInScene.Count - 1; i >= 0; i--)
        {
            explosingRB = GameManager.Instance.cylinderInScene[i];
            if (explosingRB != null && (explosingRB.CompareTag("Cylinder") || explosingRB.CompareTag("Cone")))
            {
                explosingRB.useGravity = true;
                explosingRB.isKinematic = false;
                explosingRB.AddExplosionForce((explosingRB.transform.position - transform.position).normalized.magnitude * explosionForce, Vector3.zero, radius);
            }
        }

    }
    private void OnDisable()
    {

    }
}
