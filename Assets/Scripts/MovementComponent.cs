using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotateSpeed = 50;
   
    [SerializeField] SpawnEntity spawnEntity = null;
    // Start is called before the first frame update
    void Start()
    {
        spawnEntity = GetComponent<SpawnEntity>();

    }

    // Update is called once per frame
    void Update()
    {
        //MoveTo();
        //RotateTo();
    }

 

    public void MoveTo(float _deltaTime, Vector3 _pos)
    {
        if (spawnEntity == null) return;
        transform.position = Vector3.MoveTowards(transform.position, _pos, _deltaTime * moveSpeed);
    }
    public void RotateTo(float _deltaTime, Vector3 _pos)
    {
        if (spawnEntity == null) return;
        Vector3 _lookAtDirection = _pos - transform.position;
        if (_lookAtDirection == Vector3.zero) return;
        Quaternion _lookAt = Quaternion.LookRotation(_lookAtDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookAt, _deltaTime * rotateSpeed);
        //transform.eulerAngles += _deltaTime *
    }
    public void RotateTo(float _deltaTime, Quaternion _rot)
    {
        if (spawnEntity = null) return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _rot, _deltaTime * rotateSpeed);

    }


}
