using UnityEngine;

public abstract class SingletonGUI<T> : Singleton<T> where T : class {

    private Transform _parent;

    protected new bool isSingleton(bool isDestoryOnLoad = false) {
        if (instance != null) {
            DestroyImmediate(gameObject);
            return false;
        }
        _parent = new GameObject().transform;
        transform.SetParent(_parent); // 부모로 정한다.
        _instance = this as T;
        if (!isDestoryOnLoad) {
            DontDestroyOnLoad(_parent);
        }
        return true;
    }

    // 씬이 종료될 때 부모의 자식으로 설정한다.
    public void onSceneQuit() {
        transform.SetParent(_parent);
    }

    // 씬이 로드될 때 해당 캔버스의 자식으로 설정한다.
    public void onSceneLoaded(Transform canvas) {
        transform.SetParent(canvas);
        transform.SetSiblingIndex(canvas.childCount - 2);
    }
}
