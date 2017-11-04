package com.kisaragi.inaba.lib;

/**
 * Created by Takumi on 2017/11/04.
 */

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.widget.Toast;

import java.io.InputStream;
import java.io.OutputStream;
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

    public  static void init()
    {
        getInstance()._init();
    }

    private BluetoothAdapter m_bAdapter;
    private BluetoothDevice m_bDevice;
    private BluetoothSocket m_bSocket;
    private InputStream m_btIn;
    private OutputStream m_btOut;

    private void _init()
    {
        m_bAdapter = BluetoothAdapter.getDefaultAdapter();
        if(m_bAdapter == null)
        {
            showToast("この端末にはBluetoothが搭載されていません。");
            return;
        }
        if(!m_bAdapter.isEnabled())
        {
            showToast("Bluetoothが有効になっていません。");
            return;
        }
    }
}
