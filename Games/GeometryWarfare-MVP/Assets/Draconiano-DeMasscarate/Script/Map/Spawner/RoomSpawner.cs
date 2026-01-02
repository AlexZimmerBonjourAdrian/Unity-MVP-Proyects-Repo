using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public int OpeningDirection;
    //1 ---> need buttom door
    //2 ---> need top door
    //3 ---> need left door
    //4 ---> need right door

    private RoomTemplate templates;
    private int rand;
    private bool spawned = false;
  //  private bool collision = true;
    public BoxCollider2D Collider;


    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("room").GetComponent<RoomTemplate>();
        Collider = GetComponent<BoxCollider2D>();
        Invoke("Spawn", 0.5f);

    }

    public void Spawn()
    {
        if (spawned == false)
        {
            switch (OpeningDirection)
            {
                case 1:
                    //Need to spawn a room whith a bottom doors
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    var room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, templates.GetLayer().transform.parent);
                    room.transform.parent = templates.GetLayer().transform;
                    break;
                //Need to spawn a room whith a Top door
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    room = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, templates.GetLayer().transform.parent);
                    room.transform.parent = templates.GetLayer().transform;
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    room = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, templates.GetLayer().transform.parent);
                    //Need to spawn a room whith a Left doors
                    room.transform.parent = templates.GetLayer().transform;
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    room = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, templates.GetLayer().transform.parent);
                    room.transform.parent = templates.GetLayer().transform;
                    //Need to spawn a room whith a bottom
                    break;
                default:
                    break;
            }
            spawned = true;
        }
       
    }

    //public void Update()
    //{
    //    if()
       
    //        Destroy(gameObject);
        
    //}
    public void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("SpawnPointer"))
        //{
        //    templates.StartAdnEnd();
        //    spawned = true;
        //    Destroy(other.gameObject);
        //    Destroy(gameObject);
        //}
        //else if (other.CompareTag("Rooms"))
        //{
        //    templates.StartAdnEnd();
        //    spawned = true;
        //    Destroy(other.gameObject);
        //    Destroy(gameObject);
        //}

        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }

    //public void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.CompareTag("SpawnPointer"))
    //    {
    //        templates.StartAdnEnd();
    //        spawned = true;
    //        Destroy(other.gameObject);
    //        Destroy(gameObject);
    //    }
    //    if (other.CompareTag("Rooms"))
    //    {
    //        templates.StartAdnEnd();
    //        spawned = true;
    //        Destroy(other.gameObject);
    //        Destroy(gameObject);
    //    }
    //}

    //public void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("SpawnPointer"))
    //    {
    //        templates.StartAdnEnd();
    //        spawned = true;
    //        Destroy(other.gameObject);
    //        Destroy(gameObject);
    //    }
    //    if (other.CompareTag("Rooms"))
    //    {
    //        templates.StartAdnEnd();
    //        spawned = true;
    //        Destroy(other.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
}
