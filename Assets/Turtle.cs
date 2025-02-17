using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turtle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turtleHeadRotation;


    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cayan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
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
