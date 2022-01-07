using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public List<Rigidbody> cylinderInScene = new List<Rigidbody>();
    public List<GameObject> cylinderPool = new List<GameObject>();
    public GameObject cylinder;
    public Transform cylinderParent;
    public List<GameObject> conePool = new List<GameObject>();
    public GameObject cone;
    public Transform coneParent;
    public List<GameObject> tntPool = new List<GameObject>();
    public GameObject tnt;
    public Transform tntParent;
    private GameObject[] tnties;
    public List<GameObject> manPool = new List<GameObject>();
    public GameObject man;
    public List<GameObject> manInScene = new List<GameObject>();
    public GameObject tempLevelObject;
    public int level;
    [SerializeField] LevelsData[] levelsScriptable;
    public event Action nextLevelAction;
    public event Action showFillImage;
    public event Action levelCompleting;
    public int totalThrowedCount;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        level = 1;

        levelsScriptable = Resources.LoadAll<LevelsData>("LevelsDatas");

        nextLevelAction += LevelUp;
        levelCompleting += SceneClean;

        LevelUp();

    }

    private void LevelUp()
    {

        try
        {
            levelsScriptable[level - 1].SpawnLevel();
        }
        catch
        {
            levelsScriptable[0].SpawnLevel();
        }

        level++;
        showFillImage?.Invoke();
    }
    private void SceneClean()
    {
        Destroy(tempLevelObject);
        tnties = GameObject.FindGameObjectsWithTag("Tnt");
        for (int i = tnties.Length - 1; i >= 0; i--)
        {
            tnties[i].SetActive(false);
        }

        for (int j = manInScene.Count - 1; j >= 0; j--)
        {
            manInScene[j].SetActive(false);
        }

        for (int k = cylinderInScene.Count - 1; k >= 0; k--)
        {
            cylinderInScene[k].gameObject.SetActive(false);
        }

    }

    public void FillPercentImage()
    {
        showFillImage?.Invoke();
    }

    public void NextLevelGameManager()
    {
        nextLevelAction?.Invoke();
    }

    public void LevelComleted()
    {
        levelCompleting?.Invoke();
    }
}
