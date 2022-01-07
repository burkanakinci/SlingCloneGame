using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    public enum ManState
    {
        Wait,
        Climb,
        ReadyForShoot,
        Shoot
    }
    public ManState manState = ManState.Wait;
    private Rigidbody manRB;
    private CapsuleCollider manCollider;
    [SerializeField] private float radius = 30f;
    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private Collider[] bombColliders;
    private Rigidbody explosingRB;
    private Animator manAnimator;
    private SlingController slingController;
    private Transform rubberTarget;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;
    [SerializeField] private Collider[] ragdollColliders;
    private void Awake()
    {
        manRB = GetComponent<Rigidbody>();
        manCollider = GetComponent<CapsuleCollider>();
        manAnimator = GetComponent<Animator>();

        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }
    private void Start()
    {
        slingController = FindObjectOfType<SlingController>();

        rubberTarget = GameObject.FindGameObjectWithTag("ManTarget").transform;

    }
    private void OnEnable()
    {
        manState = ManState.Wait;
        // manAnimator.SetBool("IsIdle", true);

        this.gameObject.tag = "Man";
        this.transform.SetParent(null);

        manRB.isKinematic = true;
        manRB.useGravity = false;

        RagdollActive(false);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        GameManager.Instance.manInScene.Add(this.gameObject);

    }
    private void Update()
    {
        if ((GameManager.Instance.manInScene.Count > 0) &&
            (GameManager.Instance.manInScene[GameManager.Instance.manInScene.Count - 1].gameObject == this.gameObject))
        {
            //slingController.throwingManRB = manRB;
            manState = ManState.Climb;
        }

        switch (manState)
        {
            case ManState.Wait:
                break;

            case ManState.Climb:
                transform.position = Vector3.MoveTowards(transform.position, rubberTarget.position, 3f);

                if (transform.position == rubberTarget.position)
                {
                    this.transform.SetParent(rubberTarget);
                    slingController.ShowPath();
                    slingController.throwingManRB = manRB;
                    manState = ManState.ReadyForShoot;
                }
                break;

            case ManState.ReadyForShoot:

                if (GameManager.Instance.manInScene.Count > 0)
                {
                    if ((GameManager.Instance.manInScene[GameManager.Instance.manInScene.Count - 1].gameObject != this.gameObject))
                    {
                        manState = ManState.Shoot;
                    }
                }
                else
                {
                    manState = ManState.Shoot;
                }
                break;

            case ManState.Shoot:
                //RagdollActive(false);
                manState = default;
                break;

            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.CompareTag("Untagged"))
        {
            RagdollActive(true);
        }

        if (this.gameObject.tag == "Man" && (other.collider.CompareTag("Cylinder") || other.collider.CompareTag("Cone")))
        {
            this.gameObject.tag = "ManTnt";
            Explosion();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ocean"))
        {
            this.gameObject.SetActive(false);
        }
    }
    private void Explosion()
    {
        bombColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = bombColliders.Length - 1; i >= 0; i--)
        {
            for (int j = GameManager.Instance.cylinderInScene.Count - 1; j >= 0; j--)
            {
                if (bombColliders[i].gameObject == GameManager.Instance.cylinderInScene[j].gameObject)
                {
                    explosingRB = GameManager.Instance.cylinderInScene[j];
                    break;
                }
            }

            if (explosingRB != null)
            {
                explosingRB.useGravity = true;
                explosingRB.isKinematic = false;
                explosingRB.AddExplosionForce(explosionForce, new Vector3(transform.position.x, explosingRB.transform.position.y, transform.position.z), radius);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
    private void OnDisable()
    {
        GameManager.Instance.manPool.Add(this.gameObject);
    }

    private void RagdollActive(bool _isRagdoll)
    {
        manAnimator.enabled = !_isRagdoll;

        for (int _colCount = ragdollColliders.Length - 1; _colCount > 0; _colCount--)
        {
            ragdollColliders[_colCount].enabled = _isRagdoll;
        }

        for (int _rbCount = ragdollRigidbodies.Length - 1; _rbCount > 0; _rbCount--)
        {
            ragdollRigidbodies[_rbCount].isKinematic = !_isRagdoll;
        }

        manCollider.enabled = !_isRagdoll;
    }
}

