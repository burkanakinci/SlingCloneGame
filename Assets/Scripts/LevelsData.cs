using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Levels Data")]
public class LevelsData : ScriptableObject
{
    private GameObject[] cylinderPoses;
    private GameObject[] conePoses;
    private GameObject[] tntPoses;
    private Transform manStartPos;
    public int manCount = 3;
    [SerializeField] private GameObject levelObject;
    public void SpawnLevel()
    {
        GameManager.Instance.tempLevelObject =
            Instantiate(levelObject, Vector3.zero, Quaternion.identity);

        cylinderPoses = GameObject.FindGameObjectsWithTag("CylinderPose");
        conePoses = GameObject.FindGameObjectsWithTag("ConePose");
        tntPoses = GameObject.FindGameObjectsWithTag("TntPose");
        manStartPos = GameObject.FindGameObjectWithTag("ManPose").transform;

        GameManager.Instance.totalThrowedCount = 0;
        GameManager.Instance.totalThrowedCount += cylinderPoses.Length;
        GameManager.Instance.totalThrowedCount += conePoses.Length;

        SpawnCylinder();
        SpawnCone();
        SpawnTnt();
        SpawnMan();
    }
    private void SpawnCylinder()
    {
        for (int i = cylinderPoses.Length - 1; i >= 0; i--)
        {
            if (GameManager.Instance.cylinderPool.Count > 0)
            {
                GameManager.Instance.cylinderPool[0].transform.position = cylinderPoses[i].transform.position;
                GameManager.Instance.cylinderPool[0].SetActive(true);

                GameManager.Instance.cylinderPool.Remove(GameManager.Instance.cylinderPool[0]);
            }
            else
            {
                Instantiate(GameManager.Instance.cylinder, cylinderPoses[i].transform.position, Quaternion.identity, GameManager.Instance.cylinderParent);
            }
        }
    }
    private void SpawnCone()
    {
        for (int i = conePoses.Length - 1; i >= 0; i--)
        {
            if (GameManager.Instance.conePool.Count > 0)
            {
                GameManager.Instance.conePool[0].transform.position = conePoses[i].transform.position;
                GameManager.Instance.conePool[0].SetActive(true);
                GameManager.Instance.conePool.Remove(GameManager.Instance.conePool[0]);
            }
            else
            {
                Instantiate(GameManager.Instance.cone, conePoses[i].transform.position, Quaternion.identity, GameManager.Instance.coneParent);
            }
        }
    }
    private void SpawnTnt()
    {
        for (int i = tntPoses.Length - 1; i >= 0; i--)
        {
            if (GameManager.Instance.tntPool.Count > 0)
            {
                GameManager.Instance.tntPool[0].transform.position = tntPoses[i].transform.position;
                GameManager.Instance.tntPool[0].SetActive(true);
                GameManager.Instance.tntPool.Remove(GameManager.Instance.tntPool[0]);
            }
            else
            {
                Instantiate(GameManager.Instance.tnt, tntPoses[i].transform.position, Quaternion.identity, GameManager.Instance.tntParent);
            }
        }
    }
    private void SpawnMan()
    {
        for (int i = manCount - 1; i >= 0; i--)
        {
            if (GameManager.Instance.manPool.Count > 0)
            {
                GameManager.Instance.manPool[0].transform.position = manStartPos.position;
                GameManager.Instance.manPool[0].SetActive(true);
                GameManager.Instance.manPool.Remove(GameManager.Instance.manPool[0]);
            }
            else
            {
                Instantiate(GameManager.Instance.man, manStartPos.position, Quaternion.identity);
            }

            if ((manCount - i) % 3 == 0)
            {
                manStartPos.position += new Vector3(11f, 0f, -5.5f);
            }
            else
            {
                manStartPos.position += new Vector3(-5.5f, 0f, 0f);
            }
        }
    }
}
