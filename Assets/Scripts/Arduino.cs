using UnityEngine;
using System.IO.Ports;
using System.Collections;
using System;

public class Arduino : MonoBehaviour
{
    private SerialPort stream;

    [SerializeField]
    [Tooltip("All serial information will be dumped to Debug.Log with this on")]
    private bool debugMode;


    private bool connectionOpened;

    [SerializeField]
    [Tooltip("Port to which the Arduino is connected - check Device Manager if unsure")]
    private string port = "COM3";

    //baudRate should match Serial rate in arduino
    [SerializeField]
    private int baudRate;

    //readTimeout should match delay in arduino
    [SerializeField]
    private int readTimeout;

    //how long to wait until next iteration in coroutine
    private float readYieldTimeout = 0.033f;
    private float writeYieldTimeout = 0.066f;
    [Space]

    [SerializeField]
    private GameObject playerGameObject;
    private PlayerMovement playerMovement;

    [Space]
    //	---------------------- INPUT VARIABLES ---------------------- \\
    [SerializeField] private Vector3 position;
    private int interact;

    //	---------------------- PUBLIC VARIABLES ---------------------- \\
    public Vector3 Position { get => position; }
    public int Interact { get => interact; set => interact = value;  }


    public void Start()
    {
        playerMovement = playerGameObject.GetComponent<PlayerMovement>();

        if (debugMode)
        {
            Debug.Log("Start");
        }
        OpenConnection();
        if (debugMode)
        {
            Debug.Log("Finished initializing");
        }

        StartCoroutine(ReadDataFromStream());
        StartCoroutine(WriteDataToStream());
    }

    private void Update()
    {
        
    }
    private IEnumerator ReadDataFromStream()
    {
        while (true)
        {
            try
            {
                if (connectionOpened)
                {
                    
                    while (stream.BytesToRead > 0)
                    {
                        float[] str = Array.ConvertAll(stream.ReadLine().Split(';'), float.Parse);
                        position.x = str[0];
                        position.y = str[1];
                        position.z = 0;
                    }

                    stream.BaseStream.Flush();

                }
            }
            catch (System.Exception)
            {
                if (debugMode)
                {
                    Debug.Log("Data stream interrupted or incorrectly read.");
                }
            }
            yield return new WaitForSeconds(readYieldTimeout);
        }
    }

    private IEnumerator WriteDataToStream()
    {
        while(connectionOpened)
        {
            byte[] directions = new byte[]
            {

                Convert.ToByte(playerMovement.upDistance),
                Convert.ToByte(playerMovement.downDistance),
                Convert.ToByte(playerMovement.leftDistance),
                Convert.ToByte(playerMovement.rightDistance),
                Convert.ToByte(0),
                Convert.ToByte(255)
            };

            //Debug.Log(directions[0]);
            //System.Buffer.BlockCopy(up, 0, b, 0, 4);
            //System.Buffer.BlockCopy(down, 0, b, 4, 4);
            //System.Buffer.BlockCopy(left, 0, b, 8, 4);
            //System.Buffer.BlockCopy(right, 0, b, 12, 4);

            stream.Write(directions, 0, 6);

            yield return new WaitForSeconds(writeYieldTimeout);
        }
    }
    private void OpenConnection()
    {
        stream = new SerialPort(port, baudRate);
        stream.ReadTimeout = readTimeout;

        if (debugMode)
        {
            Debug.Log("Started OpenConnection");
        }
        if (stream != null)
        {
            if (stream.IsOpen)
            {
                stream.Close();
                connectionOpened = false;
                if (debugMode)
                {
                    Debug.Log("Port was already open. Closing...");
                }
            }
            else
            {
                stream.Open();
                connectionOpened = true;
                if (debugMode)
                {
                    Debug.Log("Port opened");
                }
            }
        }
        else
        {
            if (stream.IsOpen)
            {
                connectionOpened = true;
                if (debugMode)
                {
                    Debug.Log("Port is already open");
                }
            }
            else
            {
                connectionOpened = false;
                if (debugMode)
                {
                    Debug.Log("Port == null");
                }
            }
        }
        if (debugMode)
        {
            Debug.Log("OpenConnection finished");
        }
    }

    
    private void OnApplicationQuit()
    {
        if (stream != null)
        {
            stream.Close();
        }
    }

}
