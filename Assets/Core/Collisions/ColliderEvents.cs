using System;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvents : MonoBehaviour
{
    public Action OnTriggerEnter,OnTriggerStay,OnTriggerExit;
    public Action OnCollisionEnter, OnCollisionStay,OnCollisionExit;
    
    public Action<Collider2D> OnTriggerEnterValue,OnTriggerStayValue,OnTriggerExitValue;
    public Action<Collision2D> OnCollisionEnterValue, OnCollisionStayValue,OnCollisionExitValue;
    
    public UnityEvent UnityOnTriggerEnter,UnityOnTriggerStay,UnityOnTriggerExit;
    public UnityEvent UnityOnCollisionEnter, UnityOnCollisionStay,UnityOnCollisionExit;
    
    public UnityEvent<Collider2D> UnityOnTriggerEnterValue,UnityOnTriggerStayValue,UnityOnTriggerExitValue;
    public UnityEvent<Collision2D> UnityOnCollisionEnterValue, UnityOnCollisionStayValue,UnityOnCollisionExitValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter?.Invoke();
        OnTriggerEnterValue?.Invoke(other);
        
        UnityOnTriggerEnter?.Invoke();
        UnityOnTriggerEnterValue?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStay?.Invoke();
        OnTriggerStayValue?.Invoke(other);
        
        UnityOnTriggerStay?.Invoke();
        UnityOnTriggerStayValue?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit?.Invoke();
        OnTriggerExitValue?.Invoke(other);
        
        UnityOnTriggerExit?.Invoke();
        UnityOnTriggerExitValue?.Invoke(other);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEnter?.Invoke();
        OnCollisionEnterValue?.Invoke(other);
        
        UnityOnCollisionEnter?.Invoke();
        UnityOnCollisionEnterValue?.Invoke(other);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionStay?.Invoke();
        OnCollisionStayValue?.Invoke(other);
        
        UnityOnCollisionStay?.Invoke();
        UnityOnCollisionStayValue?.Invoke(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        OnCollisionExit?.Invoke();
        OnCollisionExitValue?.Invoke(other);
        
        UnityOnCollisionExit?.Invoke();
        UnityOnCollisionExitValue?.Invoke(other);
    }
}