using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    #region Singleton
    private static RoomManager instance;

    public static RoomManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public Room roomPref;
    public int numReqRooms;

    [SerializeField] int roomWidth;
    [SerializeField] int roomHeight;
    [SerializeField] Vector3[] offsets;

    Room[] rooms;

    // Start is called before the first frame update
    void Start()
    {
        SetupGameMap();
    }

    public void SetupGameMap()
    {
        SetOffsets();

        rooms = new Room[numReqRooms];

        for (int i = 0; i < numReqRooms; i++)
        {
            Vector3 off = offsets[i];
            off.x *= roomWidth;
            off.y *= roomHeight;

            var rm = Instantiate(roomPref.gameObject, transform.position + off, Quaternion.identity, transform);
            rm.name = $"Room [{offsets[i].x}, {offsets[i].y}]";
            rm.GetComponent<Room>().coord = offsets[i];

            rooms[i] = rm.GetComponent<Room>();
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            Room neighborE = rooms.SingleOrDefault(rm => rm.coord.x == rooms[i].coord.x + 1 && rm.coord.y == rooms[i].coord.y);
            Room neighborW = rooms.SingleOrDefault(rm => rm.coord.x == rooms[i].coord.x - 1 && rm.coord.y == rooms[i].coord.y);
            Room neighborN = rooms.SingleOrDefault(rm => rm.coord.y == rooms[i].coord.y + 1 && rm.coord.x == rooms[i].coord.x);
            Room neighborS = rooms.SingleOrDefault(rm => rm.coord.y == rooms[i].coord.y - 1 && rm.coord.x == rooms[i].coord.x);

            rooms[i].Setup(neighborN != null, neighborE != null, neighborS != null, neighborW != null);

            //East
            if (neighborE != null)
            {
                rooms[i].east.door.doorVal = eDoorVal.LOCKED;
                rooms[i].east.door.neighbor = neighborE.gameObject;
            }

            //West
            if (neighborW != null)
            {
                rooms[i].west.door.doorVal = eDoorVal.LOCKED;
                rooms[i].west.door.neighbor = neighborW.gameObject;
            }

            //North
            if (neighborN != null)
            {
                rooms[i].north.door.doorVal = eDoorVal.LOCKED;
                rooms[i].north.door.neighbor = neighborN.gameObject;
            }

            //South
            if (neighborS != null)
            {
                rooms[i].south.door.doorVal = eDoorVal.LOCKED;
                rooms[i].south.door.neighbor = neighborS.gameObject;
            }
        }

        GameManager.Instance.StartGame();
    }

    public Room[] GetRooms() { return rooms; }

    private void SetOffsets()
    {
        offsets = new Vector3[numReqRooms];
        offsets[0] = Vector2.zero;

        for (int i = 1; i < numReqRooms; i++)
        {
            Vector3 newOff = offsets[i - 1];

            bool changeX = Random.Range(0, 2) == 1;

            if(changeX)
            {
                int val = Random.Range(0, 2) == 1 ? -1 : 1;
                newOff.x += val;

                while (!CheckIfValid(newOff))
                {
                    newOff.x += val;
                }
            }
            else
            {
                int val = Random.Range(0, 2) == 1 ? -1 : 1;
                newOff.y += val;

                while (!CheckIfValid(newOff))
                {
                    newOff.y += val;
                }
            }

            offsets[i] = newOff;
        }
    }

    private bool CheckIfValid(Vector3 newOff)
    {
        for (int i = 0; i < offsets.Length; i++)
        {
            if (offsets[i].x == newOff.x && offsets[i].y == newOff.y)
            {
                return false;
            }
        }

        return true;
    }
}
