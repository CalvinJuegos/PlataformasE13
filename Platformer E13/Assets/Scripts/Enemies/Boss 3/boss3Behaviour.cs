using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3Behaviour : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void ChooseAttack()
    {
        Debug.Log("Elegir ataque");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
