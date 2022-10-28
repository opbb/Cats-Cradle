using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dialogue;

public class DialogueController : MonoBehaviour
{

    [SerializeField] private Transform speakerTransform;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private SpriteRenderer speechBubble;
    [SerializeField] private float vertOffset;
    [SerializeField] private float horiOffset;

    [HideInInspector] public bool speaking;
    [HideInInspector] private Queue<Dialogue.DialogueLine> lineQueue;

    // Start is called before the first frame update
    void Start()
    {
        // Set Postion and Rotation relative to speaker
        Vector3 parentPos = speakerTransform.position;
        Vector3 thisPos = new Vector3(parentPos.x + horiOffset, parentPos.y + vertOffset, 0f);
        transform.position = thisPos;
        transform.rotation.Set(0f,0f,0f,0f);

        //Set up dialogueLine data
        endSpeaking();
        lineQueue = new Queue<Dialogue.DialogueLine>();
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Set Postion relative to speaker
        Vector3 parentPos = speakerTransform.position;
        Vector3 thisPos = new Vector3(parentPos.x + horiOffset, parentPos.y + vertOffset, 0f);
        transform.position = thisPos;

        // Speak the current text, and hold it for a little after.
        if(!speaking && lineQueue.Count > 0) {
            StartCoroutine(makeSpeak());
        }
    }

    // Methods for making this speaker say something

    // Make the speaker say one dialogueLine. If a new dialogueLine is given before the old one is finished being spoken then it will be queued.
    public void Speak(Dialogue.DialogueLine dialogueLine) {
        lineQueue.Enqueue(dialogueLine);
    }

    // Speaks all of the dialogueLines currently queued. 
    private IEnumerator makeSpeak() {
        prepareSpeaking();
        while(lineQueue.Count > 0) {
            Dialogue.DialogueLine dialogueLine = lineQueue.Dequeue();
            
            text.text = dialogueLine.text;

            /*
            foreach (char letter in dialogueLine.text.ToCharArray())
            {
                text.text += letter;
                yield return new WaitForSeconds(dialogueLine.talkSpeed);
            }
            */

            yield return new WaitForSeconds(dialogueLine.lingerDuration);
        }
        endSpeaking();
        //yield return new WaitForSeconds(0); // This is some bullshit to ensure we always return something.
    }

    private void prepareSpeaking() {
        speaking = true;
        speechBubble.color = Color.white;
        text.color = Color.black;
    }

    private void endSpeaking() {
        speaking = false;
        speechBubble.color = Color.clear;
        text.color = Color.clear;
    }

}