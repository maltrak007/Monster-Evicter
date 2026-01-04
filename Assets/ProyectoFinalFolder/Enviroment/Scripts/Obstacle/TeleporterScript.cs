using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Common.Manager;
using ProyectoFinalFolder.Enviroment.Scripts.Obstacle;
using UnityEngine;

public class TeleporterScript : InteractableComponent
{
    private ParticleSystem teleportParticleSystem;
    public SphereCollider teleportCollider;
    public Transform teleportDestination;
    
    public void OnEnable()
    {
        interactionCanvas.enabled = false;
    }
    
    private void Start()
    {
        teleportParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        CheckTeleportRequisites();
    }
    
    public override void Interact()
    {
        if (!canInteract) return;
        HandleTeleportInteraction();
        base.Interact();
    }

    private void HandleTeleportInteraction()
    {
        //GameManagerScript.Instance.player.transform.position = teleportDestination.position;
        var player = GameManagerScript.Instance.player;
    
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;
        
        player.transform.position = teleportDestination.position + Vector3.up * 0.1f;

        if (controller != null) controller.enabled = true;
    }
    
    public override bool CheckConditionToActivateCanvas()
    {
        return HasCanvas() && canInteract;
    }
    
    private void CheckTeleportRequisites()
    {
        if (!GameManagerScript.Instance.currentLevel.GetIfAllEnemiesAreDefeated()) return;
        SetCanInteract(true);
        teleportParticleSystem.Stop();
        teleportCollider.enabled = false;
    }
}
