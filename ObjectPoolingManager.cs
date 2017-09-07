using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This Script was written by huseong Lee.
/// with MIT License
/// You Need Singleton<T>
/// </summary>
/// This is enum of Pooling Object
public enum PoolingObject {

}

public class ObjectPoolingManager : Singleton<ObjectPoolingManager> {
    public Dictionary<PoolingObject, Queue<GameObject>> _pool;
    private Dictionary<PoolingObject, GameObject> _gameObjectDic;

    protected void Awake () {
        if (!isSingleton()) {
            return;
        }
        _pool = new Dictionary<PoolingObject, Queue<GameObject>>();
        _gameObjectDic = new Dictionary<PoolingObject, GameObject>();
    }

    public void initObjects(PoolingObject type, GameObject gm, Transform parents, int poolCount) {
        if(_pool.ContainsKey(type)) {
            Debug.LogError(type.ToString() + 'is already Inited');
            return;
        }
        _gameObjectDic.Add(type, gm);
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i=0; i<poolCount; i++) {
            GameObject gm = Instantiate(gm);
            gm.transform.parent = parents;
            gm.SetActive(false);
            gm.name = type.ToString() + '_' + i.ToString();
            queue.Enqueue(gm);
        }
        _pool.Add(type, queue);
    }

    public GameObject getObject(PoolingObject type, bool isActive = false) {   
        GameObject gm = _pool[type].Dequeue();
        gm.SetActive(isActive);
        return gm;
    }

    public void setObject(PoolObject poolObject, GameObject gm) {
        if(!_pool.ContainsKey(type)) {
            Debug.LogError(type.ToString() + 'is not Inited');
            return;
        }
        gm.SetActive(false);
        _pool[type].Enqueue(gm);
    }
}
