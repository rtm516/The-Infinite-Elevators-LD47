using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    #endregion

    [Header("Locations")]
    [SerializeField]
    private Transform puzzleLocation1;

    [SerializeField]
    private Transform puzzleLocation2;

    [SerializeField]
    private Transform endingLocation1;

    [SerializeField]
    private Transform endingLocation2;

    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> puzzles;

    [SerializeField]
    private GameObject doorEnding;

    [SerializeField]
    private GameObject elevatorEnding;

    [Header("Other")]
    [SerializeField]
    private ElevatorController elevator;

    public GameObject elevatorMusic;

    private List<GameObject> floorObjects;
    public int floors = -1;

    // Start is called before the first frame update
    void Start()
    {
        floorObjects = new List<GameObject>();

        StartCoroutine(GenerateFloor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GenerateFloor(GameObject ignore = null)
	{
        Debug.Log("Generating new floor!");
        floors++;
        UIManager.Instance.setFloorCount(floors);

        // Remove the current floor objects
        foreach (GameObject go in floorObjects)
		{
            if (go != ignore)
            {
                GameObject.Destroy(go);
            }
		}
        floorObjects.Clear();

        SpawnHallways();
        SpawnEndings();

        yield return new WaitForSeconds(5f);

        yield return elevator.OpenDoors();

        if (ignore != null)
        {
            GameObject.Destroy(ignore);
        }
    }

    void SpawnEndings()
    {
        if (Random.value <= 0.1f)
        {
            GameObject ending1 = GameObject.Instantiate(elevatorEnding);
            GameObject ending2 = GameObject.Instantiate(doorEnding);

            floorObjects.Add(ending1);
            floorObjects.Add(ending2);

            if (Random.value > 0.5f)
            {
                ending1.transform.position = endingLocation1.position;
                ending1.transform.rotation = endingLocation1.rotation;
                ending2.transform.position = endingLocation2.position;
                ending2.transform.rotation = endingLocation2.rotation;
            }
            else
            {
                ending1.transform.position = endingLocation2.position;
                ending1.transform.rotation = endingLocation2.rotation;
                ending2.transform.position = endingLocation1.position;
                ending2.transform.rotation = endingLocation1.rotation;
            }
        }
        else
        {
            GameObject ending1 = GameObject.Instantiate(elevatorEnding);
            ending1.transform.position = endingLocation1.position;
            ending1.transform.rotation = endingLocation1.rotation;
            floorObjects.Add(ending1);

            GameObject ending2 = GameObject.Instantiate(elevatorEnding);
            ending2.transform.position = endingLocation2.position;
            ending2.transform.rotation = endingLocation2.rotation;
            floorObjects.Add(ending2);
        }
    }

    void SpawnHallways()
	{
        GameObject puzzle1 = GameObject.Instantiate(puzzles[Random.Range(0, puzzles.Count)]);
        puzzle1.transform.position = puzzleLocation1.position;
        puzzle1.transform.rotation = puzzleLocation1.rotation;
        floorObjects.Add(puzzle1);

        GameObject puzzle2 = GameObject.Instantiate(puzzles[Random.Range(0, puzzles.Count)]);
        puzzle2.transform.position = puzzleLocation2.position;
        puzzle2.transform.rotation = puzzleLocation2.rotation;
        floorObjects.Add(puzzle2);
    }
}
