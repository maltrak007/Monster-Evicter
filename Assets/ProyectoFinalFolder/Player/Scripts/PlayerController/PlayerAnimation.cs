using System;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float locomotionBlend = 0.02f;
    
    private PlayerLocomotionInput playerInput;
    
    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");
    
    private static int isElementalBallHash = Animator.StringToHash("ElementalBall");
    private static int isElementalShieldHash = Animator.StringToHash("ElementalShield");
    private static int isElementalAreaHash = Animator.StringToHash("ElementalArea");
    private static int isHealing = Animator.StringToHash("Healing");
    
    private Vector3 currentBlendInput = Vector3.zero;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = playerInput.MoveInput;

        currentBlendInput = Vector3.Lerp(currentBlendInput, inputTarget, locomotionBlend * Time.deltaTime);
        
        animator.SetFloat(inputXHash, inputTarget.x);
        animator.SetFloat(inputYHash, inputTarget.y);
        
        animator.SetBool(isElementalBallHash, playerInput.ElementalBallAttackPressed);
        animator.SetBool(isElementalShieldHash, playerInput.ElementalShieldPressed);
        animator.SetBool(isElementalAreaHash, playerInput.ElementalAreaAttackPressed);
        animator.SetBool(isHealing, playerInput.HealthAction);
    }
}
