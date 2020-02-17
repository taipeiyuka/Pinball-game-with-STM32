using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.IO.Ports;


public class Globals : MonoBehaviour
{
    public SerialPort mySerialPort;
    public string com = "COM4";

    // Start is called before the first frame update
    void Start()
    {
        mySerialPort = new SerialPort(com);

        mySerialPort.BaudRate = 9600;
        mySerialPort.Parity = Parity.None;
        mySerialPort.StopBits = StopBits.One;
        mySerialPort.DataBits = 8;
        mySerialPort.Handshake = Handshake.None;
        mySerialPort.RtsEnable = true;
        mySerialPort.ReadTimeout = 1;

        mySerialPort.Open();

        //mySerialPort.Close();
    }

    // Update is called once per frame
    void Update()
    {
        string value;
        char[] val = new char[10];
        float v;
        mySerialPort.Read(val, 0, 7);
        value = val.ToString();
        Debug.Log(value);

    }
}
