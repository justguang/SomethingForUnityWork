#	UObjectPool 对象池基类
namespace RGuang.Utils

注：要求Unity版本2021.1以上

    【BaseClassIPool<T>】
    【BaseClassPool<T>】
    【BaseGamePool<T>】
    【BaseComponentPool<T>】

    public function:
        
    InitPool(bool colloectionCheck = true);
    InitPool(int size, int maxSize, bool colloectionCheck = true);
    T Get();
    void Release(T obj);
    void Clear();


    protected virtual function:
        T OnCreatePoolItem();
        void OnGetPoolItem(T obj);
        void OnReleasePoolItem(T obj);
        void OnDestroyPoolItem(T obj)


