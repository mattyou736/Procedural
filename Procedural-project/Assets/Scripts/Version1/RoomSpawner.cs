using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    /// 1 == need bottom door , 2 == need top door, 3 == need left door, 4 == need right door

    RoomTemplates templates;
    int rand;
    bool spawned = false;

    public float waitTime = 4f;

    private void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn",0.1f);
    }

    void Spawn()
    {
        if(spawned == false)
        {
            if (openingDirection == 1)
            {
                //spawn bottom door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                //spawn top door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                //spawn left door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                //spawn right door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spawnpoint"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //instantiate wall
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
