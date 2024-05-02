using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
//using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    
    private Camera _mainCamera;
    //private NavMeshAgent _playerNavMeshAgent;
    public Transform playerTransform;
    public float speed = 2.5f;
    //Vector2 lastClickedPos;
    bool moving;
    Collider2D clickedNPCCollider;
    public float stoppingDistance = 30f;
    Vector2 targetPos;

    private Animator characterAnimator;
    
    private static InputHandler _instance;

    private void Awake() {
        _mainCamera = Camera.main;
        _instance = this;
        GameObject player = GameObject.Find("Player");
        if(player != null){
            characterAnimator = player.GetComponent<Animator>();
        }
    }
    public static InputHandler GetInstance(){
        return _instance;
    }

    public void OnClick(InputAction.CallbackContext context) {
        if (!context.started) return;

         
        RaycastHit2D rayHit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        
        
         if (rayHit.collider != null){
            if(rayHit.collider.CompareTag("NPC")){
                moving = true;
                Debug.Log("clicked");
                clickedNPCCollider = rayHit.collider;
                targetPos = clickedNPCCollider.transform.position;
            }
            
            //float minY = 2.00f;
            //float maxY = 3.22f;

            /* if(clickedNPCCollider != null) {
                if (rayHit.collider != clickedNPCCollider){
                    clickedNPCCollider = null;
                } else {
                    return;
                }
            }*/
             //lastClickedPos = rayHit.point;
             //if(rayHit.point.y >= minY && rayHit.point.y <= maxY){
                
             //}
             

             
        }
    } 



    private void Update(){
        if(moving && !DialogueManager.GetInstance().dialogueIsPlaying){

            float step = speed * Time.deltaTime;
            Vector2 currentPosition = playerTransform.position;
            
            float distance = (currentPosition - targetPos).sqrMagnitude;
            Debug.Log("Distance to target: " + distance);
            
            // Check if player is close enough to the clicked position
            if(distance > stoppingDistance) {
                /* Vector2 direction = (targetPos - currentPosition).normalized;

                if(direction.x <0){
                    playerTransform.localScale = new Vector3(-1,1,1);
                } else if(direction.x >0){
                    playerTransform.localScale = new Vector3(1,1,1);
                } */
                playerTransform.position = Vector2.MoveTowards(currentPosition, targetPos,step);
                characterAnimator.SetBool("isMoving",true);
            } else {
                Debug.Log("here");
                moving = false;
                characterAnimator.SetBool("isMoving", false);
            }
                /* Collider2D npcCollider = Physics2D.OverlapPoint(targetPos);
                
                if (npcCollider != null && npcCollider.isTrigger && npcCollider == clickedNPCCollider){
                    
                } */
               
            
        }
    }

    /* private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "NPC") {
            Debug.Log("NPC detected: " + other.gameObject.name);
            moving = false;
        }
    }*/

    public void checkIconClicked(RaycastHit2D bruh, TextAsset inkJSON){
        if(bruh.collider.CompareTag("DialogueInteractionIcon")){
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);   
        }
    }
    
}
