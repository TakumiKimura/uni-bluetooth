using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BluetoothTask
{
#if UNITY_ANDROID
    const string JAVA_CLASS_NAME = "com.kisaragi.inaba.lib.BluetoothTask";
#endif

    public static void Init()
    {
        using (AndroidJavaClass javaClass = new AndroidJavaClass(JAVA_CLASS_NAME))
        {
            javaClass.CallStatic("init");
        }
    }

}
