using UnityEngine;

public class CylinderController : MonoBehaviour
{
    private Rigidbody rbCylinder;

    private void Awake()
    {
        rbCylinder = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        GameManager.Instance.cylinderInScene.Add(rbCylinder);

        rbCylinder.useGravity = false;
        rbCylinder.isKinematic = true;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Cylinder") || other.collider.CompareTag("Cone"))
        {
            rbCylinder.useGravity = true;
            rbCylinder.isKinematic = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ocean"))
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.cylinderInScene.Remove(rbCylinder);

        if (gameObject.CompareTag("Cylinder"))
        {
            GameManager.Instance.cylinderPool.Add(this.gameObject);
        }
        else if (gameObject.CompareTag("Cone"))
        {
            GameManager.Instance.conePool.Add(this.gameObject);
        }

        GameManager.Instance.FillPercentImage();
    }
}
