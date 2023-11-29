using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] MyInputs controls;
    [SerializeField] InputAction record;
    [SerializeField] Recorder recorder = null;

    public InputAction Record => record;
    public Recorder Recorder => recorder;
    // Start
    // is called before the first frame update
    private void Awake()
    {
        controls = new MyInputs();
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        recorder = GetComponent<Recorder>();
        Record.performed += Recorder.SetRecording;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        record = controls.Recorder.Record;
        record.Enable();
    }
    private void OnDisable()
    {
        record.Disable();  
    }
}
