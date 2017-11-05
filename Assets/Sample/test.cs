using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    [SerializeField]
    Text m_deviceText;
    [SerializeField]
    Text m_deviceListText;

    bool m_isConnect;
    byte[] m_buffer = new byte[1024];

    private void Start()
    {
        //StartCoroutine(WriteUpdate());
    }

    IEnumerator WriteUpdate()
    {
        var wait = new WaitForSeconds(0.5f);
        for(;;)
        {
            yield return wait;
            if (m_isConnect)
            {
                m_buffer = JsonBinaryUtility.ToByteArray(Input.acceleration);
                if(!Bluetooth.Write(m_buffer))
                {
                    Bluetooth.Close();
                    m_isConnect = false;
                }
                // bufferのクリア
                for(int i = 0; i < m_buffer.Length; ++i)
                {
                    m_buffer[i] = 0;
                }
            }
        }
    }

    private void OnDisable()
    {
        Bluetooth.Close();
    }

    public void BluetoothInit()
    {
        string deviceList = Bluetooth.Init();

        m_deviceListText.text = deviceList;

        m_deviceText.GetComponentInParent<InputField>().text = deviceList.Replace("\r\n", "\n").Split('\n')[0];
    }

    public void BluetoothConnect()
    {
        if (Bluetooth.Connect(m_deviceText.text))
        {
            m_isConnect = true;
            m_deviceListText.text = "接続成功。";
        }
    }

    public void OnWrite()
    {
        byte[] data = JsonBinaryUtility.ToByteArray(Input.acceleration);
        Bluetooth.Write(data);
    }
}
