using Unity.Collections;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;

  





    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
      
        EventManager.holding += GagaHold;
        EventManager.throwing += GagaThrow;
        EventManager.drop += DropItem;
        EventManager.Die += Dead;
        
    }
    private void OnDisable()
    {
        EventManager.holding -= GagaHold;
        EventManager.throwing -= GagaThrow;
        EventManager.drop -= DropItem;
        EventManager.Die -= Dead;
    }

    public void MoveAnim(bool hasInput)
    {
        
        anim.SetBool("isMoving", hasInput);
    }
    void GagaHold(GameObject target)
    {   
        if (transform.name != target.name) return;
        Debug.Log("toss" + transform.name);
        anim.SetBool("isHolding", true);
    }
    void GagaThrow(GameObject target)
    { 
        if (transform.name != target.name) return;
        Debug.Log("h" + transform.name);
        anim.SetBool("isHolding", false);
        anim.SetTrigger("Throw");
    }
    void DropItem(GameObject target)
    {
        if (transform.name != target.name) return;
        anim.SetBool("isHolding", false);
    }

    void Dead(GameObject target)
    {
        if (transform.name != target.name) return;
        anim.SetBool("isDead", true);
    }
    public void Launch()
    {
        anim.SetTrigger("Launch");
    }
}


