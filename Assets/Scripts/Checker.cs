using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checker : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEndGame;
    private Dictionary<string, bool> _modelTable;

    private void Start()
    {
        _modelTable = new Dictionary<string, bool>();
        FindAllModels();
    }

    private void FindAllModels()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("ModelTarget"))
            {
                _modelTable[obj.name] = false;                
            }
        }
    }

    public void Find(string name)
    {
        if (_modelTable.ContainsKey(name))
        {
            _modelTable[name] = true;            
        }
        foreach(var m in _modelTable.Values)
        {
            if(!m)
            {
                return;
            }
        }
        OnEndGame?.Invoke();
    }

    public bool IsFound(string name)
    {
        if (_modelTable.ContainsKey(name) && _modelTable[name])
        {
            return true;
        }
        return false;
    }
}
