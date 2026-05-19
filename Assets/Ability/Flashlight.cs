using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : Ability
{
    [Header("Flashlight")]
    public float battery;
    public bool isBroken;
    private Light targetLight;

    protected override void Start() 
    {
           base.Start();
        targetLight = GetComponent<Light>();
    }
    
  public  override void Input(InputAction.CallbackContext context)
    {
      targetLight.enabled = !targetLight.enabled;
    }

}
