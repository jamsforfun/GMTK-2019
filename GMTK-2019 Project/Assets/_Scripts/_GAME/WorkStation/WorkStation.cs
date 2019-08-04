using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    [SerializeField]
    private Transform spawner;

    [SerializeField]
    private pickableinput[] _inputsWanded;

    [SerializeField]
    private GameObject _output;

    [SerializeField]
    private List<bool> _inputsCheck;
    private bool _allInputsCheck;

    [SerializeField]
    private float _transformationTime;

    [SerializeField]
    private ParticleSystem _engineFire;
    [SerializeField]
    private ParticleSystem _engineExtinguish;
    private bool _hasStartFire;
    private bool _hasExtinguish;

    // Start is called before the first frame update
    void Start()
    {
        //ajoute un nouveau boolean pour chaque valeur object demandé en input
        for (int i = 0; i < _inputsWanded.Length; i++)
        {
            _inputsCheck.Add(new bool());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _inputsWanded.Length; i++)
        {
            if (_inputsCheck[i])
            {
                _allInputsCheck = true;
                
            }
            else
            {
                _allInputsCheck = false;
                break;
            }
        }

        if (_allInputsCheck)
        {
            _allInputsCheck = false;
            for (int i = 0; i < _inputsWanded.Length; i++)
            {
                _inputsCheck[i] = false;
            }
            StartCoroutine("CreateOutput");
        }   
    }

    private void OnCollisionEnter(Collision collision)
    {
        //cherche si le type de l'object corespond à celui voulu en input
        for (int i = 0; i < _inputsWanded.Length; i++)
        {
            if (collision.gameObject.GetComponentInChildren<Pickable>().PickableType == _inputsWanded[0])
            {
                //collision.gameObject.GetComponentInParent<Pickable>().DropItem();
                _inputsCheck[i] = true;
                Destroy(collision.gameObject.GetComponentInChildren<Pickable>().gameObject);
            }
        }
    }

    IEnumerator CreateOutput()
    {
        yield return new WaitForSeconds(_transformationTime);
        Instantiate(_output, spawner.position, Quaternion.identity);
    }
}
