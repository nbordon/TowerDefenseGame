using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wavephase
{
    PHASE_1 = 0,
    PHASE_2 = 1,
    PHASE_3 = 2,
    PHASE_4 = 3,
    PHASE_5 = 4
}

public class Spawner : MonoBehaviour
{
    // General Wave Attributes
    public static Transform[] SpawnPoints;
    public GameObject[] Enemies;
    public Transform target;
    public ProgressBar progressBar;

    // Wave Customization
    private int WaveIndex = 0;
    public int WaveLimit = 25;
    public float timer = 2.5f;
    public float timeOfWave = 6.5f;
    public int enemiesPerWave = 5;

    // Wave Internal Attributes
    public Wavephase wavephase;

    private void Awake()
    {
        // Initialize the array of points
        SpawnPoints = new Transform[transform.childCount];

        // Set all the childs point
        for(int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i] = transform.GetChild(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Init Wavephase
        wavephase = Wavephase.PHASE_1;

        // Init Wave Limit
        WaveLimit = 25;
    }

    // Update is called once per frame
    void Update()
    {
        if(WaveIndex != WaveLimit)
        {
            if(timer <= 0.0f)
            {
                // Generate the next wave
                StartCoroutine(WavePhaseState());

                timer = timeOfWave;
            }
        }

        timer -= Time.deltaTime;
        progressBar.current = timer;
    }

    IEnumerator WavePhaseState()
    {
        switch (wavephase)
        {
            case Wavephase.PHASE_1:
                WaveIndex++;
                wavephase = Wavephase.PHASE_2;
                break;
            case Wavephase.PHASE_2:
                enemiesPerWave += 2;
                WaveIndex++;
                wavephase = Wavephase.PHASE_3;
                break;
            case Wavephase.PHASE_3:
                enemiesPerWave += 2;
                WaveIndex++;
                wavephase = Wavephase.PHASE_4;
                break;
            case Wavephase.PHASE_4:
                enemiesPerWave += 2;
                WaveIndex++;
                wavephase = Wavephase.PHASE_5;
                break;
            case Wavephase.PHASE_5:
                enemiesPerWave += 2;
                WaveIndex++;
                break;
            default:
                break;
        }

        // Spawn a new wave of enemies
        for(int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, Enemies.Length);
        int randomPoints = Random.Range(0, SpawnPoints.Length);
        Enemies[randomEnemy].GetComponent<Unit>().target = target;
        var enemy = Instantiate(Enemies[randomEnemy], SpawnPoints[randomPoints].position, transform.rotation);
    }
}
