using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourMyExtention : MonoBehaviour
{
    /// <Summary>
    /// �w�肳�ꂽ�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
    /// �R���|�[�l���g���Ȃ��ꍇ�̓A�^�b�`���܂��B
    /// </Summary>
    protected T CheckParentComponent<T>(GameObject obj) where T : Component
    {
        // �^�p�����[�^�Ŏw�肵���R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
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
    /// �w�肳�ꂽ�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
    /// �R���|�[�l���g���Ȃ��ꍇ�̓A�^�b�`���܂��B
    /// </Summary>
    protected T CheckParentComponent<T>(GameObject obj) where T : Component
    {
        // �^�p�����[�^�Ŏw�肵���R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
        var targetComp = obj.GetComponent<T>();
        if (targetComp == null)
        {
            targetComp = obj.AddComponent<T>();
        }
        return targetComp;
    }
}





/// <summary>
/// MonoBehaviour���p�����Ă͂��邪�A�Q�[���I�u�W�F�N�g���g�킸�ɃV���O���g�����������Ă���̂ŁA
/// �Q�[���I�u�V�F�N�g�ɃA�^�b�`�������X�N���v�g�ɂ��A�A�^�b�`���Ȃ��X�N���v�g�ɂ��p���ł���
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
            // ���N���X����Ă΂ꂽ��Ɍp���悩��Ă΂��ƃG���[�ɂȂ�B��Ɍp���悩��Ă�
            if (!typeof(InheritSingletonType).IsAssignableFrom(ins.GetType()))
            {
                Debug.LogErrorFormat("{1}��{0}���p�����Ă��܂���", typeof(InheritSingletonType), ins.GetType());
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
    /// �y�g�����z
    /// https://www.notion.so/7d0966e1c52e48ec84e976af050a84aa#f7d0608026a048eebb61c5a949c4f379
    /// </summary>
}



