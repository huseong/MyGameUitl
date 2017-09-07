using UnityEngine;
/// <summary>
/// This Script was written by huseong Lee.
/// with MIT License
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : class {
    private static T _instance;
    public static T instance {
        get {
            return _instance;
        }
    }

    /// This Function have to be Called in (void Awake)
    /// ex 
    /// private void Awake() {
    ///    if(!isSingleton()) {
    ///         return;
    ///    } 
    /// }
    protected bool isSingleton() {
        if(_instance != null) {
            DestroyImmediate(gameObject);
            return false;
        }
        _instance = this as T;
        DontDestoryOnLoad(gameObject);
        return true;
    }
}