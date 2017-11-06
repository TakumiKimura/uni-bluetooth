package com.kisaragi.inaba.lib;

/**
 * Created by Takumi on 2017/11/04.
 */

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.util.Set;
import java.util.UUID;

import com.unity3d.player.UnityPlayer;

public class BluetoothTask
{
    private static  final String TAG = "BluetoothTask";

    private static  final UUID APP_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");

    private static BluetoothTask instance = new BluetoothTask();

    public static BluetoothTask getInstance()
    {
        if(instance == null)
        {
            instance = new BluetoothTask();
        }
        return instance;
    }

    public static void showToast(final String msg)
    {
        final Activity activity = UnityPlayer.currentActivity;
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(activity, msg, Toast.LENGTH_SHORT).show();
            }
        });
    }

    public  static String init()
    {
        return getInstance()._init();
    }

    public static boolean connect(String pcName)
    {
        return getInstance()._connect(pcName);
    }

    public static boolean write(byte[] buf)
    {
        return getInstance()._write(buf);
    }

    public static byte[] read(int bufferSize)
    {
        return getInstance()._read(bufferSize);
    }

    public static void close()
    {
        getInstance()._close();
    }

    private BluetoothAdapter m_bAdapter;
    private BluetoothDevice m_bDevice;
    private BluetoothSocket m_bSocket;
    private InputStream m_btIn;
    private OutputStream m_btOut;

    private String _init()
    {
        m_bAdapter = BluetoothAdapter.getDefaultAdapter();
        if(m_bAdapter == null)
        {
            showToast("この端末にはBluetoothが搭載されていません。");
            return "";
        }
        if(!m_bAdapter.isEnabled())
        {
            showToast("Bluetoothが有効になっていません。");
            return "";
        }

        String deviceNames = "";

        for(BluetoothDevice device : pairedDevices())
        {
            deviceNames += device.getName();
            deviceNames += "\n";
        }

        return deviceNames;
    }

    private Set<BluetoothDevice> pairedDevices()
    {
        return m_bAdapter.getBondedDevices();
    }

    private boolean _connect(String pcName)
    {
        if(m_bSocket != null && m_bSocket.isConnected())
        {
            _close();
        }

        for(BluetoothDevice device : pairedDevices())
        {
            if(device.getName().equals(pcName))
            {
                m_bDevice = device;
                break;
            }
        }
        if(m_bDevice == null)
        {
            StringBuilder msg = new StringBuilder();
            msg.append("指定された名前のデバイスが見つかりません。\n");
            msg.append(pcName);
            showToast(msg.toString());
            return false;
        }

        BluetoothSocket socket = null;
        try
        {
            socket = m_bDevice.createRfcommSocketToServiceRecord(APP_UUID);
        }
        catch (IOException e)
        {
            showToast("socketの生成に失敗しました。");
            e.printStackTrace();
        }
        m_bSocket = socket;

        if(m_bSocket == null)
        {
            showToast("socketの生成に失敗しました。");
            return false;
        }

        try
        {
            m_bSocket.connect();
            m_btIn = m_bSocket.getInputStream();
            m_btOut = m_bSocket.getOutputStream();
            showToast("接続に成功しました。");
        }
        catch (IOException e)
        {
            try
            {
                showToast("接続に失敗しました。接続先が受信可能か確認して下さい。");
                m_bSocket.close();
            }
            catch (IOException closeException)
            {
                showToast("なんらかのエラーが発生しました。");
                e.printStackTrace();
            }
            return false;
        }

        return true;
    }

    private boolean _write(byte[] buf)
    {
        try
        {
            m_btOut.write(buf);
        }
        catch (IOException e)
        {
            e.printStackTrace();
            showToast("送信エラー");
            return false;
        }

        return true;
    }

    private void _close()
    {
        try
        {
            m_bSocket.close();
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
    }

    private byte[] _read(int bufferSize)
    {
        byte[] buffer = new byte[bufferSize];
        try
        {
            m_btIn.read(buffer, 0, bufferSize);
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }

        return buffer;
    }
}
