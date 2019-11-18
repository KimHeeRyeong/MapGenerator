using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MapManager))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPref;
    [SerializeField] Material mat;
    [Range(3, 10)]
    [SerializeField] int mapSize = 5;
    [SerializeField] int chunkSize = 100;
    [SerializeField, HideInInspector] Chunk[] chunk;
    
    void Awake()
    {
        Destroy(this);
    }
    //map tile
    public void InitializeGround()
    {
        //create chunks
        int cnt = transform.childCount;
        if (cnt == 0)
        {
            CreateChunk();
            SetPositionChunks();
        }
        else
        {
            RemoveChunks();
            CreateChunk();
            SetPositionChunks();
        }
        //init map manager serialize value
        GetComponent<MapManager>().SetSize(mapSize, chunkSize);

    }
    void CreateChunk()
    {
        int cnt = mapSize * mapSize;
        chunk = new Chunk[cnt];
        for (int i = 0; i < cnt; i++)
        {
            CreateChunk(i);
        }
    }
    void CreateChunk(int i)
    {
        GameObject obj = Instantiate(chunkPref, transform);
        obj.name = "Chunk" + i.ToString();
        obj.transform.localScale = new Vector3(chunkSize, 1, chunkSize);
        chunk[i] = obj.AddComponent<Chunk>();
        chunk[i].InitializeChunk(mat,i);
    }
    void SetPositionChunks()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                int index = FindIndexFromXZ(x, z);
                chunk[index].transform.position = Vector3.forward* x * chunkSize+ Vector3.right*z * chunkSize;
            }
        }
    }
    int FindIndexFromXZ(int x, int z)
    {
        return z + x * mapSize;
    }
    void RemoveChunks()
    {
        chunk = null;
        int cnt = transform.childCount;
        for (int i = cnt - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    public void UpdateMaterial()
    {
        int cnt = chunk.Length;
        for (int i = 0; i < cnt; i++)
        {
            chunk[i].UpdateMat(mat);
        }
    }

    int FindIndexRealXZ(int x , int z)
    {
        return 0;

    }
}