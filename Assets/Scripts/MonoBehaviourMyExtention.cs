using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourMyExtention : MonoBehaviour
{
    /// <Summary>
    /// 指定されたコンポーネントへの参照を取得します。
    /// コンポーネントがない場合はアタッチします。
    /// </Summary>
    protected T CheckParentComponent<T>(GameObject obj) where T : Component
    {
        // 型パラメータで指定したコンポーネントへの参照を取得します。
        var targetComp = obj.GetComponent<T>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<T>();
        }
        return targetComp;
    }
}



public class MyExtention
{

    /// <Summary>
    /// 指定されたコンポーネントへの参照を取得します。
    /// コンポーネントがない場合はアタッチします。
    /// </Summary>
    protected T CheckParentComponent<T>(GameObject obj) where T : Component
    {
        // 型パラメータで指定したコンポーネントへの参照を取得します。
        var targetComp = obj.GetComponent<T>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<T>();
        }
        return targetComp;
    }
}





/// <summary>
/// MonoBehaviourを継承してはいるが、ゲームオブジェクトを使わずにシングルトンを実現しているので、
/// ゲームオブシェクトにアタッチしたいスクリプトにも、アタッチしないスクリプトにも継承できる
/// </summary>
public abstract class MonoBehaviourMyExtentionSingleton<SingletonType> : MonoBehaviourMyExtention,
    System.IDisposable
    where SingletonType : MonoBehaviourMyExtentionSingleton<SingletonType>, new()
{
    private static SingletonType ins;
    public static SingletonType Ins
    {
        get
        {
            return GetOrCreateInstance<SingletonType>();
        }
    }


    protected static InheritSingletonType GetOrCreateInstance<InheritSingletonType>()
        where InheritSingletonType : class, SingletonType, new()
    {
        if (IsCreated)
        {
            // 基底クラスから呼ばれた後に継承先から呼ばれるとエラーになる。先に継承先から呼ぶ
            if (!typeof(InheritSingletonType).IsAssignableFrom(ins.GetType()))
            {
                Debug.LogErrorFormat("{1}が{0}を継承していません", typeof(InheritSingletonType), ins.GetType());
            }
        }
        else
        {
            ins = new InheritSingletonType();
        }
        return ins as InheritSingletonType;
    }

    public static bool IsCreated
    {
        get { return ins != null; }
    }

    public virtual void Dispose()
    {
        ins = default;
    }

    /// <summary>
    /// 【使い方】
    /// https://www.notion.so/7d0966e1c52e48ec84e976af050a84aa#f7d0608026a048eebb61c5a949c4f379
    /// </summary>
}



