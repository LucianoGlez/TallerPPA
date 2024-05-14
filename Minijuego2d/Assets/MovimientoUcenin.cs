using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovimientoUcenin : MonoBehaviour
{


    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private float movimentoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [SerializeField] private float suavizadoDeMovimiento;
    private Vector3 velocidad= Vector3.zero;
    private bool MirandoDerecha = true;

    [Header("Salto")]

    [SerializeField] private float fuerzaDeSalto;

    [SerializeField] private LayerMask queEsSuelo;

    [SerializeField] private Transform controladorSuelo;

    [SerializeField] private Vector3 dimensionesCaja;

    [SerializeField] private bool enSuelo;

    private bool Salto = false;

    [Header("Animacion")]

    private Animator animator;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        movimentoHorizontal = Input.GetAxisRaw("Horizontal")*velocidadDeMovimiento;

        animator.SetFloat("Horizontal",Mathf.Abs(movimentoHorizontal));

        if (Input.GetButtonDown("Jump"))
        {
            Salto = true;
        }
    }

    private void FixedUpdate()
    {

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);
        animator.SetBool("enSuelo",enSuelo);
        
        Mover(movimentoHorizontal* Time.fixedDeltaTime, Salto);

        Salto=false;
    }



    private void Mover (float mover, bool saltar) {
    
        Vector3 velocidadObjetivo=new Vector3(mover,rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);



        if(mover >0 && !MirandoDerecha)
        {
            Girar();

        }else if(mover <0 && !MirandoDerecha)
        {

            Girar();

        }

        if(enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }


    private void Girar()
    {

        MirandoDerecha=!MirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }


}
    