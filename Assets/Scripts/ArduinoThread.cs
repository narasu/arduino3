using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoThread : MonoBehaviour
{
    private Thread thread;
    private Queue outputQueue;    // From Unity to Arduino
    private Queue inputQueue;    // From Arduino to Unity

    private SerialPort stream;
    private bool connectionOpened;

    [SerializeField] private bool debugMode;
    [SerializeField] private string port = "COM3";
    [SerializeField] private int baudRate;
    [SerializeField] private int readTimeout;

    public bool looping = true;

    public void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());
        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    public void ThreadLoop()
    {
        // Opens the connection on the serial port
        stream = new SerialPort(port, baudRate);
        stream.ReadTimeout = readTimeout;
        stream.Open();

        while (IsLooping())
        {
            // Send to Arduino
            if (outputQueue.Count != 0)
            {
                string command = (string)outputQueue.Dequeue();
                WriteToArduino(command);
            }
            // Read from Arduino
            string result = ReadFromArduino();
            if (result != null)
                inputQueue.Enqueue(result);
        }

        stream.Close();
    }
    public bool IsLooping()
    {
        lock(this)
        {
            return looping;
        }
    }
    public void StopThread()
    {
        lock(this)
        {
            looping = false;
        }
    }

    public void WriteToArduino(string _command)
    {
        outputQueue.Enqueue(_command);
    }

    public string ReadFromArduino()
    {

        if (inputQueue.Count == 0)
            return null;
        return (string)inputQueue.Dequeue();
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