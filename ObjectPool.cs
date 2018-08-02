using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Stack<GameObject> _pool; //오브젝트 풀은 기본적으로 스택 구조를 가진다.
    private int _overAllocateCount;
    private GameObject _originObject;
    private Transform _parent;
    private string _objname;
    private List<GameObject> _list;

    public static ObjectPool makeInstance(GameObject gm) => (gm.AddComponent<ObjectPool>()) as ObjectPool;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="originObject"></param>
    /// <param name="objname"></param>
    /// <param name="parent"></param>
    /// <param name="count"></param>
    /// <param name="overAllocateCount"></param>
    public void setObjectPool(GameObject originObject, string objname, Transform parent, int count, int overAllocateCount) {
        _overAllocateCount = overAllocateCount;
        _originObject = originObject;
        _parent = parent;
        _objname = objname;
        _pool = new Stack<GameObject>(); //새로운 스택을 만든다.
        _list = new List<GameObject>();
        allocate(count);
    }

    public void allocate(int alloCount) { //할당하기 (인자 : 할당할 개수)
        for(int i=0; i < alloCount; i++) //할당할 개수까지
        {
            GameObject obj = Instantiate(_originObject); //오브젝트를 만든다.
            obj.name = obj.name + i.ToString(); //오브젝트의 이름에 넘버링을 한다.
            obj.SetActive(false); //오브젝트를 비활성화한다.
            obj.transform.SetParent(_parent, false); //오브젝트의 부모를 설정한다.
            _pool.Push(obj); //obj풀에 만든 오브젝트를 넣어둔다.
        }
    }

    public GameObject pop(bool setActive = true) { //오브젝트 꺼내기
        if(_pool.Count <= 0)  //만약 오브젝트 풀이 부족하면
            allocate(_overAllocateCount); //부족할 때 필요한 오브젝트 풀만큼 더 만든다. 
        GameObject retobj = _pool.Pop(); //오브젝트 풀에서 하나 꺼내온다.
        retobj.gameObject.SetActive(setActive); //활성화한다.
        _list.Add(retobj);
        return retobj;
    }

    public void push(GameObject obj) {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }

    public void returnAllObjects() {
        for (int i = 0; i < _list.Count; i++) {
            _list[i].SetActive(false);
            _pool.Push(_list[i]);
        }
        _list.Clear();
    }

    public int length {
        get {
            return _pool.Count;
        }
    }

}
