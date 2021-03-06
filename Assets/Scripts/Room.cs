using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public DoorObj north;
    public DoorObj east;
    public DoorObj south;
    public DoorObj west;

    public eAgeGroup ageGroup;

    public Vector2 coord;

    [SerializeField] GameObject doorPref;

    public List<GameObject> enemies;

    public Cinemachine.CinemachineVirtualCamera cam;

    private Transform[] enemySpawnLocations = new Transform[0];

    [SerializeField] Vector3 northSpawn, eastSpawn, southSpawn, westSpawn;
    public GameObject chestPref;

    private void Start()
    {
        enemies = new List<GameObject>();
        SetEnemySpawnLocations();
    }

    private void SetEnemySpawnLocations()
    {
        enemySpawnLocations = GetComponentsInChildren<Transform>().Where(x => x.CompareTag("EnemySpawner")).ToArray();
    }

    /// <summary>
    /// Spawn in doors at the correct locations
    /// </summary>
    public void Setup(bool n, bool e, bool s, bool w)
    {
        //east.door.coord = new Vector2(7f, 0);
        //south.door.coord = new Vector2(0, -4);
        //west.door.coord = new Vector2(-7f, 0);

        cam.name = $"Cam [{coord.x}, {coord.y}]";

        if(n)
        {
            var dr = Instantiate(doorPref, transform.position + (Vector3)northSpawn, Quaternion.identity, transform);
            dr.name = "North Door";
            
            if(dr.TryGetComponent(out DoorObj dobj))
            {
                north = dobj;
                dobj.owner = this;
            }

            north.door.coord = northSpawn;
        }

        if (e)
        {
            var dr = Instantiate(doorPref, transform.position + (Vector3)eastSpawn, Quaternion.identity, transform);
            dr.transform.Rotate(Vector3.forward, 90);
            dr.name = "East Door";

            if (dr.TryGetComponent(out DoorObj dobj))
            {
                east = dobj;
                dobj.owner = this;
            }

            east.door.coord = eastSpawn;
        }

        if (s)
        {
            var dr = Instantiate(doorPref, transform.position + (Vector3)southSpawn, Quaternion.identity, transform);
            dr.transform.Rotate(Vector3.forward, 180);
            dr.name = "South Door";

            if (dr.TryGetComponent(out DoorObj dobj))
            {
                south = dobj;
                dobj.owner = this;
            }

            south.door.coord = southSpawn;
        }

        if (w)
        {
            var dr = Instantiate(doorPref, transform.position + (Vector3)westSpawn, Quaternion.identity, transform);
            dr.transform.Rotate(Vector3.forward, -90);
            dr.name = "West Door";

            if (dr.TryGetComponent(out DoorObj dobj))
            {
                west = dobj;
                dobj.owner = this;
            }

            west.door.coord = westSpawn;
        }
    }

    public void EnterRoom()
    {
        if (enemySpawnLocations.Length == 0)
            SetEnemySpawnLocations();

        float enemyPercentage = GameManager.Instance.GetAgePercentage();
        int numEnemiesToSpawn = (int)Mathf.Clamp(enemyPercentage * enemySpawnLocations.Length, 1, enemySpawnLocations.Length);
        StartCoroutine(SpawnEnemies(numEnemiesToSpawn));

        cam.gameObject.SetActive(true);

        GameManager.Instance.currentRoom = this;
        GameManager.Instance.player.transform.position = transform.position;
        GameManager.Instance.AgePlayer();

        if (GameManager.Instance.player == null) return;
        if(GameManager.Instance.player.TryGetComponent(out Health health))
        {
            health.DoDamage(-GameManager.Instance.amountToHealOnRoomEnter);
        }
    }

    private IEnumerator SpawnEnemies(int numEnemiesToSpawn)
    {
        yield return new WaitForSeconds(1f);

        //Spawn the correct number of enemies
        for (int i = 0; i < numEnemiesToSpawn; i++)
        {
            GameObject obj = null;

            switch (GameManager.Instance.currentAgeGroup)
            {
                case eAgeGroup.YOUNG:
                    obj = GameManager.Instance.youngEnemies[Random.Range(0, GameManager.Instance.youngEnemies.Length)];
                    break;
                case eAgeGroup.STUDENT:
                    obj = GameManager.Instance.studentEnemies[Random.Range(0, GameManager.Instance.studentEnemies.Length)];
                    break;
                case eAgeGroup.ADULT:
                    obj = GameManager.Instance.adultEnemies[Random.Range(0, GameManager.Instance.adultEnemies.Length)];
                    break;
                case eAgeGroup.OLD:
                    obj = GameManager.Instance.oldEnemies[Random.Range(0, GameManager.Instance.oldEnemies.Length)];
                    break;
            }

            var nmy = Instantiate(obj, enemySpawnLocations[Random.Range(0, enemySpawnLocations.Length)].position, Quaternion.identity, transform);
            nmy.transform.position = new Vector3(nmy.transform.position.x, nmy.transform.position.y, -1);
            enemies.Add(nmy);
        }
    }

    public void ExitRoom()
    {
        cam.gameObject.SetActive(false);

        if(north)
        {
            north.door.doorVal = eDoorVal.LOCKED;
        }

        if (east)
        {
            east.door.doorVal = eDoorVal.LOCKED;
        }

        if (south)
        {
            south.door.doorVal = eDoorVal.LOCKED;
        }

        if (west)
        {
            west.door.doorVal = eDoorVal.LOCKED;
        }

        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("NoDestroy") || transform.GetChild(i).gameObject.CompareTag("EnemySpawner")) continue;

            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void RemoveEnemy(GameObject nmy)
    {
        enemies.Remove(nmy);

        if(enemies.Count == 0)
        {
            //Maybe an unlock event

            if(north != null && north.door.doorVal != 0)
               north.door.doorVal = eDoorVal.UNLOCKED;

            if (east != null && east.door.doorVal != 0)
                east.door.doorVal = eDoorVal.UNLOCKED;

            if (south != null && south.door.doorVal != 0)
                south.door.doorVal = eDoorVal.UNLOCKED;

            if (west != null && west.door.doorVal != 0)
                west.door.doorVal = eDoorVal.UNLOCKED;

            Vector3 offset = new Vector3(0, 0, -1);
            Instantiate(chestPref, transform.position + offset, Quaternion.identity, transform);
        }
    }
}

[System.Serializable]
public class door
{
    public GameObject neighbor;
    public eDoorVal doorVal;
    public Vector2 coord;

    public door(GameObject neigh = null, eDoorVal val = eDoorVal.NONE)
    {
        neighbor = neigh;
        doorVal = val;
    }
}

public enum eDoorVal
{
    NONE,
    LOCKED,
    UNLOCKED
}
