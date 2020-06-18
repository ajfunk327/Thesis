﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PositionData : MonoBehaviour
{

    public OVRCameraRig OVRCameraRig;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float clock;
    public int trutrial;
    public int frame = 0;
    public int Istrial;
    public int IsWait;

    Quaternion rotation;
    Vector3 position;
    private Variables Variables;
    private ResponseTime ResponseTime;
    private Level2 Level2;
    private Order Order;
    public int Error = 1;
    public bool lean;
    public double forward = 0.01;


    // Use this for initialization
    void Start()
    {
        Order = GameObject.FindGameObjectWithTag("UI").GetComponent<Order>();
        Level2 = GameObject.FindGameObjectWithTag("Player").GetComponent<Level2>();
        Variables = GameObject.FindGameObjectWithTag("UI").GetComponent<Variables>();
       
        StreamWriter sw = File.AppendText(Variables.folder + "PositionDataHead" + Variables.participant + ".csv");
        sw.WriteLine("RotationX" + "," + "RotationY" + "," + "RotationZ" + "," + "PositionX" + "," + "PositionY" + "," + "PositionZ" + "," + "Frame" + "," + "TimeFromStart" + "," + "TimeInTrial" + "," + "Block" + "," + "Trial" + "," + "Scene" + "," + "Condition" + "," + "IsTrial" + "," + "IsWait");
        lean = false;
        transform.GetChild(0).gameObject.SetActive(false);
        sw.Close();
        StreamWriter sw1 = File.AppendText(Variables.folder + "Error_Count_Participant" + Variables.participant + ".csv"); //Edit this to change where the file is writen.
        sw1.WriteLine("Error" + "," + "Trial" + "," + "Scene" + "," + "Frame" + "," + "SceneClock" + "," + "Clock");//Names the headers
        sw1.Close();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        ResponseTime = GameObject.FindGameObjectWithTag("ResponseTime").GetComponent<ResponseTime>();

        rotation = transform.rotation; rotationX = rotation.x; rotationY = rotation.y; rotationZ = rotation.z;// These grab the rotation and position data from the object it is attached to
        position = transform.position; positionX = position.x; positionY = position.y; positionZ = position.z;

        frame = Time.frameCount; clock += Time.deltaTime * 1; trutrial = ResponseTime.trialminusprac+1;


        if (ResponseTime.ParticipantReady == false)
        {
            IsWait = 1;
        }
        else
        {
            IsWait = 0;
        }


        if (ResponseTime.posrecording == true)
        {
            Istrial = 1;
        }
        else
        {
            Istrial = 0;
        }

        
        
        StreamWriter sw = File.AppendText(Variables.folder + "PositionDataHead" + Variables.participant + ".csv");// This is where the data will be written
        sw.WriteLine(rotationX + "," + rotationY + "," + rotationZ + "," + positionX + "," + positionY + "," + positionZ + "," + frame + "," + clock + "," + ResponseTime.levelTimer + "," + ResponseTime.trueblock + "," + trutrial + "," + ResponseTime.Trial + "," + Order.Condition + "," + Istrial + "," + IsWait);
        sw.Close();

        if (positionZ >= forward)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (!lean)
            {
                StreamWriter sw1 = File.AppendText(Variables.folder + "Error_Count_Participant" + Variables.participant + ".csv");
                sw1.WriteLine(Error + "," + Level2.Trial + "," + ResponseTime.Trial + "," + frame + "," + ResponseTime.levelTimer + "," + clock);
                sw1.Close();
                lean = true;
                Error++;
            }
        }
        else
        {
            lean = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}