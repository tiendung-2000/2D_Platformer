using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    #region Variables

    [field: BoxGroup, SerializeField] public bool DontDestroy { get; set; }

    private static T _ins;

    public static T Ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType<T>();

                if (_ins == null)
                {
                    var singletonObject = new GameObject($"[Singleton - {typeof(T).Name}]");
                    _ins = singletonObject.AddComponent<T>();
                    _ins.DontDestroy = true;
                }
            }
            return _ins;
        }
    }

    #endregion

    #region Unity callback functions

    private void Awake()
    {
        if (_ins == null)
        {
            _ins = this as T;
            LoadInAwake();
            if (DontDestroy) DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Another instance of " + GetType().Name + " is already exist! Destroying self...");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadInStart();
    }

    public virtual void LoadInAwake()
    {
    }

    public virtual void LoadInStart()
    {
    }

    #endregion
}