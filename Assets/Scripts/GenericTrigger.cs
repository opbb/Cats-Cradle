using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenericTrigger : MonoBehaviour {

    [SerializeField] private TriggerType triggerType;
    [SerializeField] private LayerMask activators;
    [SerializeField] private int index;
    [SerializeField] private bool onlyOnce;
    [SerializeField] private DialogueManager dialogueManager;

    private void OnTriggerEnter2D(Collider2D col) {

        int layer = col.gameObject.layer; // The layer this collision is with.

        if ((activators == (activators | (1 << layer)))) {
            switch (triggerType)
            {
                case TriggerType.Dialogue:
                    dialogueTrigger();
                    break;
                case TriggerType.BonePickup:
                    boneTrigger();
                    break;            
                default:
                    throw new Exception("This trigger has no recognized type assigned.");
            }

            if(onlyOnce) {
                Destroy(this);
            }
        }
    }

    private void dialogueTrigger() {
        dialogueManager.triggerDialogue(index);
    }

    private void boneTrigger() {
        Debug.Log("Bone trigger activated with index of " + index);
    }
}

enum TriggerType
{
    Dialogue,
    BonePickup
}