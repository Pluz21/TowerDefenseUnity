using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Recorder : MonoBehaviour
{
    [SerializeField] float time = 0;
    [SerializeField] float rewindTime = 0;
    [SerializeField] int index = 0;
    [SerializeField] float minTimeAllowed = 1;
    [SerializeField] float minDistanceAllowed = 0.1f;
    [SerializeField] float distanceBetweenPoints = 1f;

    [SerializeField] MovementComponent movementRef;
    [SerializeField] Player player;
    [SerializeField] RecordState recordState = RecordState.NONE;   // Will allow us to check if we are recording or not. 
    [SerializeField] List<Point> allPoints = new List<Point>();
    public RecordState RecordingState => recordState;
    public List<Point> AllPoints => allPoints;
    public int Index
    { get { return index; }
        set { index = value; }
    }

    //public bool IsValid => truckRef != null && movementRef != null;
    public enum RecordState
    { 
        NONE,                       // NONE will act as default.
        IS_RECORDING,
        HAS_RECORDED,
        
    }
    [Serializable]
    public struct Point
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float PointTime { get; set; }

    public Point(Vector3 _position, Quaternion _rotation, float _time)
    {
        Position = _position;
        Rotation = _rotation;
        PointTime = _time;
    }
        
    }
    void Start()
    {
        
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Record();
        Rewind();
    }

    void Init()
    {
        movementRef = GetComponent<MovementComponent>(); 
        player = GetComponent<Player>();
    }
    void Record()
    {
        if (recordState != RecordState.IS_RECORDING)
        {
            if (recordState != RecordState.HAS_RECORDED && allPoints.Count > 0)
            { 
                recordState = RecordState.HAS_RECORDED;
                allPoints.Add(new Point(transform.position,transform.rotation,time));
                time = 0;
                //Might need to reset object position to first position 
            }
            return;  // We are not recording anymore
        }
        if(recordState == RecordState.IS_RECORDING && allPoints.Count > 0)
            //reset points;
        time = IncreaseTime(time);
        if (allPoints.Count < 1 || Vector3.Distance(transform.position,
            allPoints[^1].Position) > distanceBetweenPoints) // replace 1 with mindist value // DIFFERENT WRITING STYLE
           // allPoints[allPoints.Count - 1].Position) > distanceBetweenPoints) // replace 1 with mindist value
                                                                                                                        // .Position directly calls 
        {
            allPoints.Add(new Point(transform.position, transform.rotation, time));
            time = 0;
            }
    }
    float IncreaseTime(float _time)                 
    {   
        _time += Time.deltaTime;
        return _time;
    }
    public void SetRecording(InputAction.CallbackContext _context)
    {
        if (player == null) return;
        if (recordState == RecordState.HAS_RECORDED)
            ResetRecordPoints();
        recordState = recordState != RecordState.IS_RECORDING ? RecordState.IS_RECORDING : RecordState.NONE;
        Debug.Log("Called SetRecording");
    }
    private void ResetRecordPoints()
    {
        allPoints.Clear();
        index = 0;
    }

    void Rewind()
    {
        if (allPoints.Count < 1) return;
        if (player == null ||  movementRef == null) return;
        rewindTime = IncreaseTime(rewindTime);
        Debug.Log(allPoints.Count);
        if (rewindTime >= allPoints[index].PointTime || allPoints[index].PointTime <= minTimeAllowed)
        {
            movementRef.MoveTo(Time.deltaTime, allPoints[index].Position);
            movementRef.RotateTo(Time.deltaTime, allPoints[index].Rotation);
        if (Vector3.Distance(transform.position, allPoints[index].Position) < minDistanceAllowed)
        {
            index++;
            rewindTime = 0;
            if (index + 1 > allPoints.Count - 1)
            {
                index = 0;
                return;
            }
        }
        }

    }

    private void OnDrawGizmos()
    {
        if (recordState == RecordState.IS_RECORDING)
        {
            AnmaGizmos.DrawSphere(transform.position, 1, Color.red, AnmaGizmos.DrawMode.Wire);
        }
        int _size = allPoints.Count;
        for (int i = 0; i < _size; i++)
        {
            AnmaGizmos.DrawSphere(allPoints[i].Position, 0.3f, Color.green,AnmaGizmos.DrawMode.Wire);
            if (i + 1 < allPoints.Count)
                AnmaGizmos.DrawLine(allPoints[i].Position, allPoints[i + 1].Position, Color.blue);

        }
        Gizmos.color = Color.white;
    }


}
