using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapParticleSetting
{
    public float radius = 1;
    public int count = 10;
    public Vector3 centre = Vector3.zero;
    public Vector2 regionSize = Vector2.zero;
    public Vector2 rotY = Vector2.zero;
    public Vector2 rotX = Vector2.zero;
    public Vector2 scale = new Vector2(1,1);
    public int rejectionSamples = 30;

    public GameObject[] particle;
    public Material[] mat;
    public GameObject[] chunks;
    [HideInInspector] public GameObject[] chunksParticle;
    [HideInInspector] public bool fold;
}
public class ParticleGenerator : MonoBehaviour
{
    [SerializeField] public MapParticleSetting[] mapParticleSetting;
    // Start is called before the first frame update
    public void InitializeParticle(int index)
    {
        MapParticleSetting setting = mapParticleSetting[index];
        if (setting.chunks == null)
            return;

        int cnt = setting.chunks.Length;
        
        //remove chunksParticles
        if (setting.chunksParticle != null && setting.chunksParticle.Length > 0)
        {
            int partCnt = setting.chunksParticle.Length;
            for (int i = 0; i < partCnt; i++)
            {
                if(setting.chunksParticle[i]!=null)
                    DestroyImmediate(setting.chunksParticle[i]);
            }
        }
        //create chunksParticle
        setting.chunksParticle = new GameObject[cnt];
        for (int i = 0; i < cnt; i++)
        {   if (setting.chunks[i] == null)
                continue;
            GameObject obj = new GameObject("particle" + index);
            obj.transform.parent = setting.chunks[i].transform;
            obj.transform.localPosition = Vector3.zero;
            setting.chunksParticle[i] = obj;

            List<Vector2> points = GeneratePoints(setting.radius
                , setting.regionSize
                , setting.rejectionSamples
                , setting.count);

            int pointCnt = points.Count;
            
            int matCnt = setting.mat.Length;
            Material[] mat = new Material[matCnt];
            for (int m = 0; m < matCnt; m++)
            {
                if (setting.mat[m] == null)
                    mat[m] = new Material(Shader.Find("Standard"));
                else
                    mat[m] = setting.mat[m];
            }

            for(int p = 0; p < pointCnt; p++)
            {
                if (setting.particle == null||setting.particle.Length==0)
                    break;
                //random particle
                int partKindCnt = setting.particle.Length;
                int randomPart = Random.Range(0, partKindCnt);

                GameObject part = Instantiate(setting.particle[randomPart], obj.transform);
                part.transform.localPosition = new Vector3(points[p].x,0,points[p].y)
                    -new Vector3(setting.regionSize.x,0,setting.regionSize.y)*0.5f
                    +setting.centre;
                
                //random scale
                float randomScale = Random.Range(setting.scale.x, setting.scale.y);
                part.transform.localScale = new Vector3(1, 1, 1) * randomScale;
                
                //random rotation
                float randomRotY = Random.Range(setting.rotY.x, setting.rotY.y);
                float randomRotX = 0;
                if(setting.rotX!=Vector2.zero)
                    randomRotX = Random.Range(setting.rotX.x, setting.rotX.y);
                part.transform.localRotation = Quaternion.Euler(randomRotX, randomRotY, 0);
                
                //material
                part.GetComponent<MeshRenderer>().sharedMaterials = mat;
            }
        }
        
    }

    List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30, int pointCnt = 10)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0 && points.Count < pointCnt)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }

        }

        return points;
    }

    bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
