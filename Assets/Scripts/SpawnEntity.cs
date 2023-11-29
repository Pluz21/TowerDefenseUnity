using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntity : MonoBehaviour
{
    [SerializeField] MovementComponent movementComponent = null;
    [SerializeField] Player playerRef = null;
    [SerializeField] Recorder recorder = null;

    [SerializeField] int recorderCurrentIndex = 0;
    [SerializeField] Vector3 heightOffset = Vector3.zero;
    [SerializeField] Vector3 recorderCurrentIndexPosition = Vector3.zero;
    [SerializeField] float minDistanceAllowed = 1;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef == null) return;
        DistanceChecker();

    }
    void Init()
    {
        movementComponent = GetComponent<MovementComponent>();
        playerRef = FindObjectOfType<Player>();
        recorder = playerRef.GetComponent<Recorder>();
        heightOffset = new Vector3(0, 1, 0);
        //recorderCurrentIndex = playerRef.Recorder.Index;
        //recorderCurrentIndexPosition = playerRef.Recorder.AllPoints[recorderCurrentIndex].Position;
    }
    void DistanceChecker()
    { 
        if (recorderCurrentIndex > playerRef.Recorder.AllPoints.Count - 1) return;
        //if (playerRef.Recorder.Index > 0) return;
        float _distance = Vector3.Distance(transform.position, playerRef.Recorder.AllPoints[recorderCurrentIndex].Position);
            Debug.LogWarning($"Distance to point {_distance}");
        //Vector3 _targetPos = playerRef.Recorder.AllPoints[recorderCurrentIndex].Position + 
            movementComponent.MoveTo(Time.deltaTime, (playerRef.Recorder.AllPoints[recorderCurrentIndex].Position));
        if (_distance < minDistanceAllowed)
        {

            recorderCurrentIndex++;
            Debug.LogWarning($"Distance to point {_distance}");

        }

    }
    
}
