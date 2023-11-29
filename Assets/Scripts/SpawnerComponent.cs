using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


[Serializable]
public struct FWaveElement
{
    [SerializeField] public SpawnEntity entityToSpawn;
    [SerializeField] public int numberToSpawn;

}


[Serializable]
public struct FWave
{
    [SerializeField] public List<FWaveElement> allWaveElements;  
}

[Serializable]
public class WaveSpawner
{
    public event Action OnElementReceived = null;
    public event Action OnElementEnded = null;
    public event Action OnSpawn = null;
    public event Action OnDelayEnded = null;
    [SerializeField] FWaveElement waveElement = new FWaveElement();
    [SerializeField] float currentTime = 0;
    [SerializeField] float maxTime = 5;

    public WaveSpawner(FWaveElement _waveElement)
    {
        waveElement = _waveElement;
    }

    float IncreaseTime(float _current, float _max)
    {
        _current += Time.deltaTime;
        if (_current >= _max)
        {
            _current = 0;
        }
        return _current;
    }

    public void Init()
    {
        OnElementReceived?.Invoke();
    }

}


public class SpawnerComponent : MonoBehaviour
{
    public event Action OnWaveReceived = null;
    public event Action OnSpawnDelayEnded = null;
    public event Action<SpawnEntity> OnEntityPicked = null;
   


    [SerializeField] Recorder recorder = null;
    [SerializeField] Player playerRef = null;

    [SerializeField] List<FWave> allWaves = new List<FWave>();
    [SerializeField] List<SpawnEntity> allSpawnedEntities = new List<SpawnEntity>();
    [SerializeField] Vector3 spawnLocation = Vector3.zero;

    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float waveDelay = 10;
    [SerializeField] float currentTime = 0;
    [SerializeField] float maxTime = 5;

    [SerializeField] int currentEntityCount = 0;
    [SerializeField] int maxEntityCount = 20;


    [SerializeField] FWave currentWave = new FWave();
    [SerializeField] int elementCount = 0;
    [SerializeField] int waveElementCount = 0;
    [SerializeField] int waveElementIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Init()
    {
        recorder = playerRef.GetComponent<Recorder>();
        spawnLocation = transform.position;
        OnWaveReceived += ManageWaveContent;
        if (allWaves.Count < 1) return;
        ReceiveWaveContent(allWaves[0]);
        OnEntityPicked += Spawn;
    }

    void ReceiveWaveContent(FWave _newWave)
    {
        currentWave = _newWave;
        InvokeRepeating(nameof(PickEntityToSpawnFromWaveElement), spawnDelay, spawnDelay);
    }

    void ManageWaveContent()
    {
            //foreach (FWaveElement _waveElement in currentWave.allEntitiesToSpawn)
            //{
            //    int _size = _waveElement.numberToSpawn;
            //    for (int i = 0; i < _size; i++)
            //    {
            //        Spawn(_waveElement.entityToSpawn);
            //    }
            //}
        
        //if (currentWave.allEntitiesToSpawn.Count < 1) return;
        //WaveSpawner _waveSpawner = new WaveSpawner(currentWave.allEntitiesToSpawn[0]);
        //_waveSpawner.Init();
    }

    void PickEntityToSpawnFromWaveElement()
    {
        if (recorder.RecordingState != Recorder.RecordState.HAS_RECORDED)
        {
            
           Debug.Log($"{recorder.RecordingState}");
            return;
        }

        if (currentWave.allWaveElements.Count < 1 || waveElementIndex > currentWave.allWaveElements.Count-1)
        {
            //OnWaveEnded?.Invoke();
            CancelInvoke(nameof(PickEntityToSpawnFromWaveElement));
            return;
        }
        FWaveElement _element = currentWave.allWaveElements[waveElementIndex];
        if (_element.entityToSpawn == null) return;
        OnEntityPicked?.Invoke(_element.entityToSpawn);
        elementCount++;
        if (elementCount >= _element.numberToSpawn)
        {
            waveElementIndex++;
            elementCount = 0;
        }

    }

    void Spawn(SpawnEntity _entityToSpawn)
    {
        //
        SpawnEntity _spawned = Instantiate(_entityToSpawn, spawnLocation, Quaternion.identity);
        if (_spawned == null) return;
        allSpawnedEntities.Add(_spawned);
        
        //foreach (SpawnEntity _spawnedEnt in allSpawnedEntities)   // might as well do it inside the SpawnEntity class 
        //{
            
        //}
    }

    void IncreaseTime(float _current, float _maxTime)
    {
        _current += Time.deltaTime;
        if(_current >=  _maxTime)
        {
            _current = 0;
            OnSpawnDelayEnded?.Invoke();
        }
    }



}
