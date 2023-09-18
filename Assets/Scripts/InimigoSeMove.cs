using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InimigoSeMove : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _animator;
    [SerializeField] Transform _alvo;
    [SerializeField] Transform _player;
    [SerializeField] Transform[] _pos;

    [SerializeField] GameController _gameController; // Sc que está como componente da câmera

    //---------------------------------------------

    float _distancia;
    float _distanciaPlayer;
    [SerializeField] float _distanciaPatrulhar;
    [SerializeField] float _speedPatrulha;
    [SerializeField] float _speedPerseguicao;

    [SerializeField] float _speedAnim;

    Vector3 _speedAgent;

    int _numberM;

    bool _seguirPlayer;

    [SerializeField] bool _hitCheck;

    //---------------------------------------------

    float _checkTime;
    [SerializeField] float _timeLimit;

    Hit _hit;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        _checkTime = _timeLimit;
        // ---------------------------------------
        _hit = GetComponent<Hit>();
        _gameController = Camera.main.GetComponent<GameController>();
        _player = _gameController._player;
    }


    void Update()
    {
        if (!_hitCheck)
        {
            Movimento();
        }
        else
        {
            Hit(true);
        }
        _speedAgent = _agent.velocity;
        Animacoes();

        if (_hitCheck)
        {
            _checkTime -= Time.deltaTime;
            if (_checkTime < 0)
            {
                Hit(false);
                _hitCheck = false;
                _checkTime = _timeLimit;
            }
        }
    }

    void Patrulhar()
    {
        if (_distancia < _distanciaPatrulhar && _numberM == 0)
        {
            //_alvo = _pos[1];
            _numberM = 1;
        }
        else if (_distancia < _distanciaPatrulhar && _numberM == 1)
        {
            //_alvo = _pos[0];
            _numberM = 0;
        }
        _alvo = _pos[_numberM];
    }

    void Movimento()
    {
        //_agent.destination = _alvo.position;
        _agent.SetDestination(_alvo.position);
        _distancia = _agent.remainingDistance;
        _distanciaPlayer = Vector3.Distance(transform.position, _player.position);
        if (_distanciaPlayer < 12)
        {
            _seguirPlayer = true;
            _agent.speed = _speedPerseguicao;
        }
        else
        {
            _seguirPlayer = false;
            _agent.speed = _speedPatrulha;
        }

        if (_distanciaPlayer < 4 && _seguirPlayer == true)
        {
            _agent.speed = 0;
        }

        if (!_seguirPlayer)
        {
            Patrulhar();
            _agent.SetDestination(_alvo.position);
        }
        else
        {
            _agent.SetDestination(_player.position);
        }
    }

    void Animacoes()
    {
        _speedAgent = _agent.velocity;
        _speedAnim = Mathf.Abs(_speedAgent.x) + Mathf.Abs(_speedAgent.z);
        _animator.SetFloat("Speed", _speedAnim);
        _animator.SetBool("hitmorte", _hitCheck);
    }

    void Morte()
    {

    }
    void Hit(bool on)
    {
        if (on)
        {
            _agent.velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic= false;
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AtaqueMili"))
        {
            _hitCheck = true;
        }
    }
}