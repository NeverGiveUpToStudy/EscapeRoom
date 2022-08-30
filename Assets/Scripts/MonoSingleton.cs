using System;
using UnityEngine;

[AutoSingleton(true)]
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    private static bool _destroyed;

    public static T Instance
    {
        get
        {
            var _type = typeof(T);
            if (_destroyed)
            {
                Debug.LogWarningFormat("[Singleton]【{0}】已被标记为销毁，返 Null！", _type.Name);
                return (T)null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(_type);
                    if (FindObjectsOfType(_type).Length > 1)
                    {
                        Debug.LogErrorFormat("[Singleton]类型【{0}】存在多个实例.", _type.Name);
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        var customAttributes = _type.GetCustomAttributes(typeof(AutoSingletonAttribute), true);
                        var autoAttribute = customAttributes.Length > 0
                            ? (AutoSingletonAttribute)customAttributes[0]
                            : null;
                        if (null == autoAttribute || !autoAttribute.autoCreate)
                        {
                            Debug.LogWarningFormat("[Singleton]欲访问单例【{0}】不存在且设置了非自动创建~", _type.Name);
                            return (T)null;
                        }

                        GameObject go = null;
                        if (string.IsNullOrEmpty(autoAttribute.resPath))
                        {
                            go = new GameObject(_type.Name);
                            _instance = go.AddComponent<T>();
                        }
                        else
                        {
                            go = Resources.Load<GameObject>(autoAttribute.resPath);
                            if (null != go)
                            {
                                go = Instantiate(go);
                            }
                            else
                            {
                                Debug.LogErrorFormat("[Singleton]类型【{0}】ResPath设置了错误的路径【{1}】", _type.Name,
                                    autoAttribute.resPath);
                                return (T)null;
                            }

                            _instance = go.GetComponent<T>();
                            if (null == _instance)
                                Debug.LogErrorFormat("[Singleton]指定预制体未挂载该脚本【{0}】，ResPath【{1}】", _type.Name,
                                    autoAttribute.resPath);
                        }
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            Debug.Log("创造了新的克隆体！");
            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }
        else
        {
            _instance = GetComponent<T>();
            if (!transform.parent) //Unity 只允许最最根节点的 游戏对象不销毁加载。
                DontDestroyOnLoad(gameObject);
            OnInit();
        }
    }

    /// <summary>
    ///     当播放停止时，Unity 会以随机顺序销毁对象
    ///     若单例 gameObject 先于其他对象销毁，不排除这个单例再次被调用的可能性。
    ///     故而在编辑器模式下，即便播放停止了，也可能会生成一个 gameObject 对象残留在编辑器场景中。
    ///     所以，此方法中加把锁，避免不必要的单例调用
    /// </summary>
    public void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == gameObject)
        {
            _instance = (T)null;
            _destroyed = true;
        }
    }

    public static void DestroyInstance()
    {
        if (_instance != null) Destroy(_instance.gameObject);
        _destroyed = true;
        _instance = (T)null;
    }

    /// <summary>
    ///     清除 _destroyed 锁
    /// </summary>
    public static void ClearDestroy()
    {
        DestroyInstance();
        _destroyed = false;
    }

    /// <summary>Awake 初始化完成之后 </summary>
    public virtual void OnInit()
    {
        Debug.Log("OnInit");
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class AutoSingletonAttribute : Attribute
{
    public bool autoCreate; //是否自动创建单例
    public string resPath; //从指定的预制体路径生成单例

    public AutoSingletonAttribute(bool _autoCreate, string _resPath = "")
    {
        autoCreate = _autoCreate;
        resPath = _resPath;
    }
}