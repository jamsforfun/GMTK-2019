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
    private List<bool> _inputsCheck = new List<bool>();
    private bool _allInputsCheck;

    private bool _hasChanged;
    private bool _onFire;

    [SerializeField]
    private float _transformationTime;

    [SerializeField]
    private ParticleSystem _engineFire;
    [SerializeField]
    private ParticleSystem _engineExtinguish;
    private bool _hasStartFire;

    // Start is called before the first frame update
    void Start()
    {
        if (_engineFire == null)
        {
            return;
        }
        if (_engineExtinguish == null)
        {
            return;
        }

        _engineFire.Stop();
        _engineExtinguish.Stop();
        //ajoute un nouveau boolean pour chaque valeur object demandé en input
        for (int i = 0; i < _inputsWanded.Length; i++)
        {
            _inputsCheck.Add(new bool());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputsCheck == null)
        {
            return;
        }
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

        if ((_onFire) && (!_hasStartFire))
        {
            _engineFire.Play();
            _hasStartFire = true;
        } 

        if ((!_onFire) && (_hasStartFire))
        {
            _hasStartFire = false;
            _engineFire.Stop();
            StartCoroutine("WaitForSmoke");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<Pickable>())
        {
            //cherche si le type de l'object corespond à celui voulu en input
            for (int i = 0; i < _inputsWanded.Length; i++)
            {
                if ((collision.gameObject.GetComponentInChildren<Pickable>().PickableType == _inputsWanded[i]) && (!_inputsCheck[i]) && (!_onFire))
                {
                    _inputsCheck[i] = true;
                    _hasChanged = true;
                }
                if (_hasChanged)
                {
                    _hasChanged = false;
                }
                else
                {
                    if (collision.gameObject.GetComponentInChildren<Pickable>().PickableType != pickableinput.extincteur)
                        _onFire = true;
                }
            }
            if ((collision.gameObject.GetComponentInChildren<Pickable>().PickableType == pickableinput.extincteur) && (_onFire))
            {
                _onFire = false;     
            }
            else
            { 
                _onFire = true;
            }
            DropItem(collision);
        }
    }

    private void DropItem(Collision p_collision)
    {
        //p_collision.gameObject.GetComponentInParent<Pickable>().DropItem();
        Destroy(p_collision.gameObject.GetComponentInChildren<Pickable>().gameObject);                   
    }

    IEnumerator CreateOutput()
    {
        yield return new WaitForSeconds(_transformationTime);
        Instantiate(_output, spawner.position, Quaternion.identity);
    }
    IEnumerator WaitForSmoke()
    {
        yield return new WaitForSeconds(3f);
        _engineExtinguish.Play();
    }
}
