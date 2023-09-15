using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveControl : MonoBehaviour
{
    public Vector2 _input; //Input system, axis: X,Z. 
    private Vector3 _playerVelocity;
    private Vector3 _movement;

    private CharacterController _controller;
    private Animator _anim;

    //[SerializeField] LayerMask _groundMask;
    //[SerializeField] private float _raycastLenght = 1.15f; //Raio para identificar Ground, saindo do centro do gameObject

    private bool _inputPulo; //Input de pulo
    [SerializeField] bool _checkGround; //Verificador se o player est� encostando no ch�o
    [SerializeField] bool _checkTrepa; //Verificador se o player est� pr�ximo ao a�aizeiro

    private float _gravityValue = -9.81f;
    public float _gravityMultiplier;
    [SerializeField] float _speed = 5;
    [SerializeField] float _speedRotation = 15;
    [SerializeField] float _jump = 3;
    [SerializeField] float _timer; // Contador para input de pulo, �til se tiver problema de pulo duplo

    private float _timerValue;

    // ----------------- Tutorial de Escada -> UseAcaizeiro variaveis

    UseAcaizeiro _useAcaizeiro;

    public bool _acaizeiro;

    void Start()
    {
        _timer = _timerValue;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _useAcaizeiro = GetComponent<UseAcaizeiro>();
        AndarN();
    }

    void Update()
    {
        if (_acaizeiro == false)
        {
            Move();
            LookAtMovementDirection();
            if (_inputPulo && _checkGround)
            {
                Pulo();
            }
            Gravity();
            CheckPulo(); // Checa se est� encostando no ch�o e fun��o de timer para normalizar pulo
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if(_acaizeiro == false)
            {
                _useAcaizeiro.GetReferences();
                _acaizeiro = true;
            }
            else
            {
                _useAcaizeiro.ExitEscada();
                _acaizeiro = false;
            }
        }
        
        if (_useAcaizeiro == true)
        {
            _acaizeiro = _useAcaizeiro.UpdateEscada();
        }
    
    }
    void AndarN()// Sincronizar anima��es - Andar movimento de perna/Andar movimento de bra�o
    {
        _anim.SetLayerWeight(0, 1); //perna
        _anim.SetLayerWeight(1, 1); //braco
    }
    void Move()
    {
        _movement = new Vector3(_input.x, _controller.velocity.y, _input.y).normalized * _speed * Time.deltaTime;
        _controller.Move(_movement);

        // Linhas abaixo feitas para anima��o do personagem
        float _andar = Mathf.Abs(_input.x) + Mathf.Abs(_input.y);
        float _inputMagnitude = _movement.magnitude;
        _anim.SetFloat("Andar", _andar);
        _anim.SetFloat("VelocidadeY", _controller.velocity.y);
        _anim.SetBool("groundCheck", _checkGround);
    }
    void LookAtMovementDirection() //Script para virar a frente do personagem voltada a orienta��o do movimento
    {
        if (_input != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speedRotation * Time.deltaTime);
        }
    }
    void Pulo()
    {
        _inputPulo = false;
        _playerVelocity.y = Mathf.Sqrt(0);
        _playerVelocity.y = Mathf.Sqrt(_jump * -3.0f * _gravityValue);
        Debug.Log("Pular Acionado");
        _anim.SetFloat("VelocidadeY", _controller.velocity.y);
    }
    void CheckPulo() // Timer para negativar inputPulo corretamente
    {
        // ^^Raycast para idendificar terreno do tipo Ground
        //_checkGround = Physics.Raycast(transform.position, Vector3.down, _raycastLenght, _groundMask);
        _checkGround = _controller.isGrounded;
        if (_inputPulo==true)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _inputPulo = false;
                _timer = _timerValue;
            }
        }
    }
    void Gravity() // Se estiver encostando no ch�o zerar o vector.Down que no caso � o gravity multiplier
    {
        if (_checkGround == true)
        {
            _gravityMultiplier = 0;
        }
        else if (_checkGround == false)
        {
            _gravityMultiplier = 2;
        }
        _playerVelocity.y += _gravityValue * _gravityMultiplier * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    public void SetMove(InputAction.CallbackContext value) //Input direcional X e Z (Input System)
    {
        _input = value.ReadValue<Vector2>();
    }
    public void SetPular(InputAction.CallbackContext value) //Pulo: true ou false
    {
        _inputPulo = true;
    }

    /*
    void Update()
    {
    // Linhas abaixo pra fazer ele trepar
        bool _trepa = Input.GetKeyDown(KeyCode.E);

        if (_trepa && _checkTrepa && _checkGround)
        {
            Vector3 _trepada = new Vector3(0, _input.y, 0) * _speed * Time.deltaTime;
            _controller.Move(_trepada);
        }
        else
        {
            _controller.Move(_movement);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trepa")
        {
            _checkTrepa = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Trepa")
        {
            _checkTrepa = false;
        }
    }
    */
}
