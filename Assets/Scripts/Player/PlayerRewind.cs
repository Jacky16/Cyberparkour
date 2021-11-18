using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerRewind : MonoBehaviour
{
    [SerializeField] List<PlayerTransforms> listOfPositions = new List<PlayerTransforms>();
    [SerializeField] Transform cameraFps;
    [SerializeField] Transform orientation;
    [SerializeField] float recordLenght = 5;
    public static bool isRewinding;
    PlayerGlobalVolume playerGlobalVolume;

    Rigidbody rb;

    struct PlayerTransforms
    {
        public Vector3 position;
        public Vector3 rotation;
    };
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerGlobalVolume = GetComponentInChildren<PlayerGlobalVolume>();

    }
    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    
    void Record()
    {
        if(listOfPositions.Count > Mathf.Round(recordLenght / Time.fixedDeltaTime))
        {
            listOfPositions.RemoveAt(listOfPositions.Count - 1);
        }
        PlayerTransforms currentTransform;
        currentTransform.position = rb.position;
        currentTransform.rotation = cameraFps.transform.localRotation.eulerAngles;
        listOfPositions.Insert(0, currentTransform);
    }

    

    void Rewind()
    {
        if (listOfPositions.Count > 0)
        {
            Vector3 rotCam = listOfPositions[0].rotation;

            rb.MovePosition (listOfPositions[0].position);
            cameraFps.transform.localEulerAngles = rotCam;
            orientation.transform.localRotation = Quaternion.Euler(0, rotCam.y, 0);

            listOfPositions.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }
    public void StartRewind()
    {
        rb.isKinematic = true;
        isRewinding = true;
        playerGlobalVolume.SetVolumeRewind(isRewinding);
    }
    void StopRewind()
    {
        rb.isKinematic = false;
        isRewinding = false;
        playerGlobalVolume.SetVolumeRewind(isRewinding);
    }
}
