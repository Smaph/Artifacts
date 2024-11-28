using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Checker))]
public class AnimObject : MonoBehaviour
{
    [System.Serializable]
    public class MaterialEntry
    {
        public string key;
        public GameObject gameObject;                
    }

    public List<MaterialEntry> materialEntries;

    public Dictionary<string, GameObject> _materials = new Dictionary<string, GameObject>();

    void Awake()
    {        
        foreach (var entry in materialEntries)
        {
            _materials.Add(entry.key, entry.gameObject);
        }
    }
    
    private Checker _checker;
    private MeshFilter _defaultMeshFilter;
    private Renderer _defaultRenderer;
    private Animator _animator;
    private GameObject _model;
    public string _modelName;

    private void Start()
    {
        _checker = GetComponent<Checker>();
        GameObject defaultObj = GameObject.Find("defaultObject");
        _animator = GetComponent<Animator>();
        if (defaultObj != null)
        {
            _defaultRenderer = defaultObj.GetComponent<Renderer>();
            _defaultMeshFilter = defaultObj.GetComponent<MeshFilter>();
            _defaultRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("Default object not found");
        }
    }

    public void FoundEvent(string name)
    {
        if(name == string.Empty)
        {
            return;
        }
        _modelName = name;
        GameObject model = GameObject.Find(_modelName).transform.GetChild(0).gameObject;

        if (_defaultRenderer.enabled)
        {
            return;
        }
        if (!_materials.ContainsKey(_modelName))
        {
            Debug.LogError($"Material for model with modelName {_modelName} is not set");
        }
        if (!_checker.IsFound(_modelName))
        {
            _defaultMeshFilter.mesh = _materials[_modelName].GetComponent<MeshFilter>().sharedMesh;
            _defaultRenderer.material = _materials[_modelName].GetComponent<Renderer>().sharedMaterial;
            _defaultRenderer.enabled = true;
            model.GetComponent<Renderer>().enabled = true;
            model.GetComponent<Renderer>().material.color = Color.blue;
            _animator.Play("Spin");

        }
        _model = model; //save model
    }

    public void EndEvent()
    {
        _defaultRenderer.enabled = false;
        _model.GetComponent<Renderer>().material.color = Color.green;
        _checker.Find(_modelName);                
    }
}
