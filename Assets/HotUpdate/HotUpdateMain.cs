using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using YooAsset;

public class HotUpdateMain : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Entry.Start();

        UnityEngine.Debug.Log("安卓热更新完成");

        //Run("函数未变化，运行AOT版本", RunNotChangedMethod);
        //Run("函数有变化，运行解释器版本", RunChangedMethod);

        //var package = YooAssets.GetPackage("DefaultPackage");
        //AssetOperationHandle handle = package.LoadAssetAsync<GameObject>("Cube");
        //handle.Completed += Handle_Completed;
    }

    private void Handle_Completed(AssetOperationHandle obj)
    {
        //GameObject go = obj.InstantiateSync();
        //UnityEngine.Debug.Log($"Prefab name is {go.name}");
    }

    public void Run(string name, Action<int[]> method)
    {
        var arr = new int[1000000];
        for (int i = 0; i < 5; i++)
        {
            var sw = new Stopwatch();
            sw.Start();
            method(arr);
            sw.Stop();
            long costTime = sw.ElapsedMilliseconds;
            UnityEngine.Debug.Log($"{name} round:[{i}] cost time:{costTime} ms");
        }
    }


    void RunNotChangedMethod(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = arr[i] ^ i;
            arr[i] += i;
        }
    }

    void RunChangedMethod(int[] arr)
    {
#if DHE_HOT_UPDATE
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = arr[i] | i;
            arr[i] += i;
        }
#else
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = arr[i] ^ i;
            arr[i] += i;
        }
#endif
    }
}