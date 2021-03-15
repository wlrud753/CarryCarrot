using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{

    int SheepNumber;

    public float SpawnCooltime;
    float spawnTimer = 0;

    Bounds SpawnArea;
    Bounds WolfArea;

    GameObject sheep_parent;
    GameObject sheep;

    void Start()
    {
        SpawnArea = GameObject.Find("Map").GetComponent<BoxCollider2D>().bounds;
        WolfArea = GameObject.Find("Map").transform.Find("Wolf Gen Area").GetComponent<BoxCollider2D>().bounds;

        sheep_parent = GameObject.Find("Sheeps").gameObject;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= SpawnCooltime)
        {
            spawnTimer = 0;
            Spawn();
        }
    }

    #region Spawn
    Vector3 sheepPos = new Vector3();
    void Spawn()
    {
        sheep = Instantiate(Resources.Load("Prefabs/Rescue Sheeps/Sheep")) as GameObject;
        sheep.transform.SetParent(sheep_parent.transform);
        sheep.name = "Sheep" + SheepNumber++;

        // Sheep Position
        do
        {
            sheepPos.x = Random.Range(SpawnArea.min.x, SpawnArea.max.x);
            sheepPos.y = Random.Range(SpawnArea.min.y, SpawnArea.max.y);
        } while (sheepPos.x >= WolfArea.min.x && sheepPos.x <= WolfArea.max.x &&
                sheepPos.y >= WolfArea.min.y && sheepPos.y <= WolfArea.max.y);
        sheepPos.z = -1f;
        sheep.transform.position = sheepPos;
    }
    #endregion
}
