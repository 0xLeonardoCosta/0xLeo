using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAcaizeiro : MonoBehaviour
{
    // ScriptAssinalado ao Buri

    Animator animator;

    MoveControl moveControl;

    public Transform top, baseE, esc;
    public bool isInTop;

    public bool usingEscada;
    public float sphereRadius = 5f;
    public string escadaTag;

    [Header("Movimento no Açaizeiro")]
    public float moveSpeed = 4f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetReferences()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, sphereRadius);

        foreach (var c in colls) // Loop - 'Paracada' colisão vai receber c
        {
            if (c.tag == escadaTag) // Se c colidir com a tag "Escada" vai receber o objeto
            {
                baseE = c.transform.GetChild(0);
                print("Tocou na base");
                esc = c.transform.GetChild(1);
                top = c.transform.GetChild(2);

                usingEscada = true;
                InitEscada();

                break;
            }
        }
    }

    public void InitEscada()
    {
        isInTop = false;

        if(top == null || baseE == null || esc == null) 
        {
            //Rigidbody rigidbody = GetComponent<Rigidbody>();
            //rigidbody.useGravity = false;

            MoveControl moveControl = FindObjectOfType<MoveControl>();

            if (usingEscada == true)
            {
                // Acessando a variável gravityMultiplier.
                float gravityMultiplier = moveControl._gravityMultiplier;

                gravityMultiplier = 0;
            }

            Vector3 escRef = new Vector3(esc.position.x, transform.position.y, esc.position.z);
            escRef.y = Mathf.Clamp(escRef.y, baseE.position.y, top.position.y);
            esc.position = escRef;

            transform.position = esc.position;
            transform.rotation = esc.rotation;

            //animacao
            animator.applyRootMotion = false;
            animator.SetBool("useAcai", true);
        }
    }

    public void ExitEscada()
    {
        isInTop = false;
        usingEscada = false;

        //Rigidbody rigidbody = GetComponent<Rigidbody> ();
        //rigidbody.useGravity = true;


        MoveControl moveControl = FindObjectOfType<MoveControl>();

        if (usingEscada == false)
        {
            // Acessando a variável gravityMultiplier.
            float gravityMultiplier = moveControl._gravityMultiplier;

            gravityMultiplier = 2;
        }

        //animacao
        animator.SetBool ("useAcai", false);
    }
    public bool UpdateEscada()
    {
        if (top == null || baseE == null || esc == null)
        {
            ExitEscada();
            return false; // Importante retornar false aqui para indicar que não está usando a escada.
        }
        else
        {
            //GameObject playerObject = GameObject.Find("Player");
            GameObject playerObject = GameObject.FindObjectOfType<MoveControl>().gameObject;
            Vector2 inputValue = playerObject.GetComponent<MoveControl>()._input;

            
            float v = inputValue.y;
            animator.SetFloat("Altura", v);

            //movimentar o esc
            esc.position += Vector3.up * v * moveSpeed * Time.deltaTime;

            //limitar o esc
            Vector3 escRef = esc.position;
            escRef.y = Mathf.Clamp(escRef.y, baseE.position.y, top.position.y);
            esc.position = escRef;

            //verificar se está no topo ou na base do açaizeiro
            float distMin = (baseE.position - esc.position).magnitude;
            float distMax = (top.position - esc.position).magnitude;

            if (v > 0 && distMax < 0.2f) // Subir no açaizeiro
            {
                animator.applyRootMotion = true;
                isInTop = true;
                animator.Play("subir");
            }
            if (v < 0 && distMin < 0.2f) // Descer do açaizeiro
            {
                ExitEscada();
                return false; // Importante retornar false aqui para indicar que não está usando a escada.
            }
            if (!isInTop)
            {
                transform.position = esc.position;
            }
            return usingEscada;
        }
    }
}