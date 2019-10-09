using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    public float minimumSpawnRange = 10f;

    public GameObject[] spawnPoints;
    public int ticker = 0;
    public int nextSpawnTime = 600;
    public int previousSpawnTime = 600;
    public int initialSpawnTime = 600;
    public int minimumSpawnTime = 60;
    public int spawnTimeDecrement = 30;

    public GameObject zombiePrefab;
    public DroppedItem drop;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = initialSpawnTime;
        previousSpawnTime = initialSpawnTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ticker++;
        if(ticker > nextSpawnTime)
        {
            ticker = 0;
            Spawn();

            if (nextSpawnTime > minimumSpawnTime)
            {
                previousSpawnTime = nextSpawnTime;
                nextSpawnTime = previousSpawnTime - spawnTimeDecrement;
            }
        }
    }


    void Spawn()
    {

        float rand = Random.Range(0f, 100f);
        int dropID = -1;
        if (rand > 40f)
        {
            dropID = (int)Random.Range(0, drop.sprites.Length);
        }

        List<GameObject> availableSpawns = new List<GameObject>();
        foreach(GameObject g in spawnPoints)
        {
            if (Vector3.Distance(g.transform.position, player.transform.position) > minimumSpawnRange) availableSpawns.Add(g);
        }

        int spawnerToUse = (int)(Random.Range(0, availableSpawns.Count));

        GameObject z = Instantiate(zombiePrefab, availableSpawns[spawnerToUse].transform.position, availableSpawns[spawnerToUse].transform.rotation);
        z.GetComponent<ZombieAttack>().dropID = dropID;

    }

}
