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
    [SerializeField] private new Camera camera;
    [SerializeField] private float vertOffset;
    [SerializeField] private float horiOffset;
    [SerializeField] FMODUnity.StudioEventEmitter startSpeakingSound;
    [SerializeField] FMODUnity.StudioEventEmitter speakingSound;
    [SerializeField] PauseMenu levelManager;

    private static float postSpeakBuffer = .5f;
    [HideInInspector] public bool speaking;
    [HideInInspector] private Queue<Dialogue.DialogueLine> lineQueue;

    // Start is called before the first frame update
    void Start()
    {
        // Set Postion and Rotation relative to speaker
        setPosition();
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
        setPosition();

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
            
            if (dialogueLine.text != "EndLevel") {
                text.text = dialogueLine.text;
                startSpeakingSound.Play();
                speakingSound.Play();
            } else {
                levelManager.EndLevel();
            }

            /*
            foreach (char letter in dialogueLine.text.ToCharArray())
            {
                text.text += letter;
                yield return new WaitForSeconds(dialogueLine.talkSpeed);
            }
            */

            yield return new WaitForSeconds(dialogueLine.lingerDuration);
            speakingSound.Stop();
            yield return new WaitForSeconds(postSpeakBuffer);
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

    private void setPosition() {
        Vector3 targetPos = speakerTransform.position;
        targetPos = targetPos + new Vector3(horiOffset,vertOffset, 0f);
        targetPos = camera.WorldToScreenPoint(targetPos);

        // Get's the components of where the bubble wants to be in screen space
        Vector3 targetPosX = new Vector3(targetPos.x, 0f, 0f);
        Vector3 targetPosY = new Vector3(0f, targetPos.y, 0f);



        // Clamps the magnitude of the components so that the bubble is on the screen
        Vector3 bubbleSize = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)) + speechBubble.bounds.size);
        float xMax = (Screen.width - bubbleSize.x) / 2;
        float yMax = (Screen.height - bubbleSize.y) / 2;

        Vector3 xCenter = new Vector3(Screen.width / 2, 0f, 0f);
        Vector3 yCenter = new Vector3(0f, Screen.height / 2, 0f);
        targetPosX = Vector3.MoveTowards(xCenter, targetPosX, xMax);
        targetPosY = Vector3.MoveTowards(yCenter, targetPosY, yMax);

        // Construct new position
        targetPos = camera.ScreenToWorldPoint(targetPosX + targetPosY);
        transform.position = new Vector3(targetPos.x, targetPos.y, 0f);
    }
}