using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort m_serialPort;
    private Thread m_thread;
    private bool m_isRunning = false;

    private string m_message;
    private bool m_isNewMessageReceived = false;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        if (m_isNewMessageReceived)
        {
            m_isNewMessageReceived = false;
            OnDataReceived(m_message);
        }
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        m_serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        m_serialPort.Open();

        m_isRunning = true;

        m_thread = new Thread(Read);
        m_thread.Start();
    }

    private void Close()
    {
        m_isRunning = false;

        if (m_thread != null && m_thread.IsAlive)
        {
            m_thread.Join();
        }

        if (m_serialPort != null && m_serialPort.IsOpen)
        {
            m_serialPort.Close();
            m_serialPort.Dispose();
        }
    }

    private void Read()
    {
        const int bufferSize = 1024;
        byte[] buffer = new byte[bufferSize];

        int readBuffer = 0;

        while (m_isRunning && (m_serialPort != null && m_serialPort.IsOpen))
        {
            try
            {
                readBuffer = m_serialPort.Read(buffer, 0, bufferSize);
                if (readBuffer > 0)
                {
                    // 送信側の仕様に合わせて変えて下さい。
                    m_message = System.Text.Encoding.UTF8.GetString(buffer);
                    m_isNewMessageReceived = true;
                }
                for (int i = 0; i < bufferSize; ++i)
                {
                    buffer[i] = 0;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void StrWrite(string message)
    {
        try
        {
            m_serialPort.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void BinWrite(byte[] buffer)
    {
        try
        {
            m_serialPort.Write(buffer, 0, buffer.Length);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}