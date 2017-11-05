﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public static class Bluetooth
{
#if UNITY_ANDROID
    const string JAVA_CLASS_NAME = "com.kisaragi.inaba.lib.BluetoothTask";
    const string FUNC_NAME_INIT = "init";
    const string FUNC_NAME_CONNECT = "connect";
    const string FUNC_NAME_WRITE = "write";
    const string FUNC_NAME_CLOSE = "close";
#endif

#if UNITY_IOS
    // ネイティブプラグイン
    [DllImport("__Internal")]
    private static extern string _init();

#endif

    /// <summary>
    /// Bluetoothの初期化。
    /// </summary>
    /// <param name="deviceName"></param>
    public static string Init()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass javaClass = new AndroidJavaClass(JAVA_CLASS_NAME))
        {
            return javaClass.CallStatic<string>(FUNC_NAME_INIT);
        }
#elif UNITY_IOS
        return _init();
#else
        return string.Empty;
#endif
    }

    /// <summary>
    /// Bluetooth接続の開始。
    /// </summary>
    public static bool Connect(string deviceName)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass javaClass = new AndroidJavaClass(JAVA_CLASS_NAME))
        {
           return javaClass.CallStatic<bool>(FUNC_NAME_CONNECT, deviceName);
        }
#elif UNITY_IOS
        return false;
#else
        return false;  
#endif
    }

    /// <summary>
    /// 送信したいデータをbyte[]で送る
    /// </summary>
    /// <param name="buffer"></param>
    public static bool Write(byte[] buffer)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass javaClass = new AndroidJavaClass(JAVA_CLASS_NAME))
        {
            return javaClass.CallStatic<bool>(FUNC_NAME_WRITE, buffer);
        }
#elif UNITY_IOS
        return false;
#else
        return false;
#endif
    }

    public static void Close()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass javaClass = new AndroidJavaClass(JAVA_CLASS_NAME))
        {
            javaClass.CallStatic(FUNC_NAME_CLOSE);
        }
#elif UNITY_IOS

#endif
    }
}
