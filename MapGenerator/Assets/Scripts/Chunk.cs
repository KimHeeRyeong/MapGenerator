using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField, HideInInspector]int num = -1;

    MapManager mapManager;
    private void Awake()
    {
        mapManager = GetComponentInParent<MapManager>();
    }
    public void InitializeChunk(Material mat, int num) {
        GetComponent<MeshRenderer>().sharedMaterial = mat;
        this.num = num;
    }
    public void UpdateMat(Material mat) {
        GetComponent<MeshRenderer>().sharedMaterial = mat;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            mapManager.SetNowMap(num);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            mapManager.SetNowMap(num);
        }
    }
}
