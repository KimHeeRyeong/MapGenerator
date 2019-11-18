using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField, HideInInspector]int mapSize = 0;//if 3, map size is 3x3
    [SerializeField, HideInInspector]int chunkSize = 0;
    Transform[] maps;
    int nowMap = -1;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeMap();
    }
    void InitializeMap() {
        int cnt = transform.childCount;
        if (cnt == 0)
            return;

        maps = new Transform[cnt];
        for (int i = 0; i <cnt; i++)
        {
            maps[i] = transform.GetChild(i);
        }
    }

    #region Get close Index 
    int GetFoward(int now) {
        if (now >= mapSize * (mapSize - 1))
            return now % mapSize;
        else
            return now + mapSize;
    }
    int GetBack(int now)
    {
        if (now < mapSize)
            return (mapSize - 1) * mapSize + now;
        else
            return now - mapSize;
    }
    int GetRight(int now)
    {
        if (now % mapSize == mapSize - 1)
            return now-mapSize+1;
        else
            return now + 1;
    }
    int GetLeft(int now)
    {
        if (now % mapSize == 0)
            return now + mapSize - 1;
        else
            return now - 1;
    }
    int GetForwardLeft(int now)
    {
        int num = GetFoward(now);
        num = GetLeft(num);
        return num;
    }
    int GetForwardRight(int now) {
        int num = GetFoward(now);
        num = GetRight(num);
        return num;
    }
    int GetBackLeft(int now)
    {
        int num = GetBack(now);
        num = GetLeft(num);
        return num;
    }
    int GetBackRight(int now)
    {
        int num = GetBack(now);
        num = GetRight(num);
        return num;
    }
    #endregion

    public void SetNowMap(int num) {
        if (num == nowMap || num == -1)
            return;
        nowMap = num;
        LoadCloseMap();
    }
    void LoadCloseMap()
    {
        Vector3 pos = maps[nowMap].position;
        //forward
        int closeMap = GetFoward(nowMap);
        maps[closeMap].position = pos + Vector3.forward * chunkSize;

        //forward right
        closeMap = GetForwardRight(nowMap);
        maps[closeMap].position = pos + (Vector3.right + Vector3.forward) * chunkSize;

        //forward left
        closeMap = GetForwardLeft(nowMap);
        maps[closeMap].position = pos + (Vector3.left + Vector3.forward) * chunkSize;

        //right
        closeMap = GetRight(nowMap);
        maps[closeMap].position = pos + Vector3.right * chunkSize;

        //left
        closeMap = GetLeft(nowMap);
        maps[closeMap].position = pos + Vector3.left * chunkSize;

        //back
        closeMap = GetBack(nowMap);
        maps[closeMap].position = pos + Vector3.back * chunkSize;

        //back right
        closeMap = GetBackRight(nowMap);
        maps[closeMap].position = pos + (Vector3.right + Vector3.back) * chunkSize;

        //back left
        closeMap = GetBackLeft(nowMap);
        maps[closeMap].position = pos + (Vector3.left + Vector3.back) * chunkSize;

    }
    public void SetSize(int mapSize, int chunkSize) {
        this.mapSize = mapSize;
        this.chunkSize = chunkSize;
    }
}
