using System.Collections;
using UnityEngine;

public class SlingController : MonoBehaviour
{
    public Rigidbody throwingManRB;
    [SerializeField] private float h = 2f, minH = 1f, maxH = 30f;

    public float gravity = -18;
    public bool debugPath;
    public Transform[] pathObjects = new Transform[30];
    [SerializeField] private Transform target;
    private Vector3 bandBoneStart;
    [SerializeField] private Vector3 firstTargetPos;
    private Vector3 firstTouchPos, distTouchPos;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private float platformRotateSpeed = 1f;

    float displacementY;
    Vector3 displacementXZ;
    float time;
    Vector3 velocityY;
    Vector3 velocityXZ;

    LaunchData launchData;
    Vector3 previousDrawPoint;

    int resolution;
    float simulationTime;
    Vector3 displacement;
    Vector3 drawPoint;



    private void Start()
    {
        firstTargetPos = target.position;

    }

    private void Update()
    {
        platformTransform.Rotate(Vector3.down * platformRotateSpeed);
        if (throwingManRB != null)
        {
            DrawPath();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                h = 2;
                debugPath = true;
                firstTouchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                TargetMovement();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                RigidbodyMovement();

            }
        }
        else
        {
            CleanPath();
        }
    }
    private void TargetMovement()
    {
        distTouchPos = (Camera.main.ScreenToViewportPoint(Input.mousePosition) - firstTouchPos) * 300f;

        target.position =
            new Vector3(Mathf.Clamp(target.position.x - distTouchPos.x * 0.5f, -55f, 55f),
                target.position.y,
                Mathf.Clamp(target.position.z - distTouchPos.y, -80f, 120f));

        h = Mathf.Clamp(h - (distTouchPos.y * 0.25f), minH, maxH);

        distTouchPos = Vector3.zero;

        firstTouchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    private void RigidbodyMovement()
    {
        debugPath = false;

        Launch();

        GameManager.Instance.manInScene.Remove(throwingManRB.gameObject);

        if (GameManager.Instance.manInScene.Count <= 0 || GameManager.Instance.cylinderInScene.Count <= 0)
        {
            StartCoroutine(SceneComplete());
        }

        target.position = firstTargetPos;

        throwingManRB = null;
    }

    private void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        throwingManRB.isKinematic = false;
        throwingManRB.useGravity = true;
        throwingManRB.velocity = CalculateLaunchData().initialVelocity;
    }

    LaunchData CalculateLaunchData()
    {
        displacementY = target.position.y - throwingManRB.position.y;
        displacementXZ = new Vector3(target.position.x - throwingManRB.position.x, 0, target.position.z - throwingManRB.position.z);
        time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    private void DrawPath()
    {
        launchData = CalculateLaunchData();
        previousDrawPoint = throwingManRB.position;

        resolution = pathObjects.Length;
        for (int i = 1; i <= resolution; i++)
        {
            simulationTime = i / (float)resolution * launchData.timeToTarget;
            displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            drawPoint = throwingManRB.position + displacement;
            pathObjects[i - 1].transform.position = previousDrawPoint;
            previousDrawPoint = drawPoint;
        }
    }
    private void CleanPath()
    {
        resolution = pathObjects.Length;

        for (int j = resolution - 1; j >= 0; j--)
        {
            pathObjects[j].gameObject.SetActive(false);
        }
    }
    public void ShowPath()
    {
        resolution = pathObjects.Length;

        for (int k = resolution - 1; k >= 0; k--)
        {
            pathObjects[k].gameObject.SetActive(true);
        }
    }
    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }

    IEnumerator SceneComplete()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.LevelComleted();
    }
}
