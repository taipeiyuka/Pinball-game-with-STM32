using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.IO.Ports;


public class Globals : MonoBehaviour
{
    public enum OP {
        WAIT_4_BUTTON,
        WAIT_4_VELOCITY,
        WAIT_4_END,
        GAME_OVER
    }
    public SerialPort mySerialPort;
    public string com = "COM4";
    public Rigidbody2D Go_ball;

    public OP now;
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

        now = OP.WAIT_4_BUTTON;
    }

    // Update is called once per frame
    void Update()
    {
        string value;
        char[] val = new char[10];
        float v;
        if (now == OP.WAIT_4_BUTTON)
        {
            //Debug.Log("button");
            //value = mySerialPort.ReadExisting();
            mySerialPort.Read(val, 0, 7);
            value = new string(val);
            if (string.Compare(value, "GOOOOO!") == 0)
            {
                now = OP.WAIT_4_VELOCITY;
                Debug.Log("Let's go!");
                GameObject.FindGameObjectWithTag("ball").transform.position = new Vector3(4.23f, -4.7f, 0f);
            }
        }
        else if (now == OP.WAIT_4_VELOCITY)
        {
            //Debug.Log("velocity");
            if (GameObject.FindGameObjectWithTag("ball").transform.position.y > -4.5)
            {
                now = OP.WAIT_4_END;
            }
            //value = mySerialPort.ReadExisting();
            mySerialPort.Read(val, 0, 7);
            value = new string(val);
            Debug.Log(value);
            if (string.Compare(value, "GOOOOO!") != 0)
            {
                //Debug.Log(value);
                v = float.Parse(value);
                v /= 1000;
                //Go_ball.AddForce(Vector3.up * v, ForceMode2D.Force);
                if (v < 300)
                {
                    Go_ball.AddForce(Vector3.up * v, ForceMode2D.Impulse);
                }
                now = OP.WAIT_4_END;
                Debug.Log(v);
            }
        }
        else if (now == OP.WAIT_4_END)
        {
            mySerialPort.Read(val, 0, 7);
            value = new string(val);
        }
        else if (now == OP.GAME_OVER)
        {
            mySerialPort.Read(val, 0, 7);
            value = new string(val);
            if (string.Compare(value, "GOOOOO!") == 0)
            {
                now = OP.WAIT_4_VELOCITY;
                Debug.Log("Let's go!");
                GameObject.FindGameObjectWithTag("ball").transform.position = new Vector3(4.23f, -4.7f, 0f);
            }
        }
    }
}
