using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{

    [SerializeField] private Transform thisTransform;
    [SerializeField] private Transform speakerTransform;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private SpriteRenderer speechBubble;
    [SerializeField] private float vertOffset;
    [SerializeField] private float horiOffset;
    [SerializeField] private float defaultTalkSpeed;
    [SerializeField] private float defaultLingerDuration;

    [HideInInspector] public bool speaking;
    [HideInInspector] private Queue<Sentence> sentenceQueue;

    // Start is called before the first frame update
    void Start()
    {
        // Set Postion and Rotation relative to speaker
        Vector3 parentPos = speakerTransform.position;
        Vector3 thisPos = new Vector3(parentPos.x + horiOffset, parentPos.y + vertOffset, 0f);
        thisTransform.position = thisPos;
        thisTransform.rotation.Set(0f,0f,0f,0f);

        //Set up sentence data
        endSpeaking();
        sentenceQueue = new Queue<Sentence>();
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Set Postion relative to speaker
        Vector3 parentPos = speakerTransform.position;
        Vector3 thisPos = new Vector3(parentPos.x + horiOffset, parentPos.y + vertOffset, 0f);
        thisTransform.position = thisPos;

        // Speak the current text, and hold it for a little after.
        if(!speaking && sentenceQueue.Count > 0) {
            StartCoroutine(makeSpeak());
        }
    }

    // Methods for making this speaker say something

    // Make the speaker say one sentence. If a new sentence is given before the old one is finished being spoken then it will be queued.
    public void Speak(string sentence) {
        sentenceQueue.Enqueue(new Sentence(sentence, defaultTalkSpeed, defaultLingerDuration));
    }

    // Make the speaker say one sentence. If a new sentence is given before the old one is finished being spoken then it will be queued.
    public void Speak(string sentence, float talkSpeed, float lingerDuration) {
        sentenceQueue.Enqueue(new Sentence(sentence, talkSpeed, lingerDuration));
    }

    // Speaks all of the sentences currently queued. 
    private IEnumerator makeSpeak() {
        prepareSpeaking();
        while(sentenceQueue.Count > 0) {
            Sentence sentence = sentenceQueue.Dequeue();
            
            text.text = sentence.text;

            /*
            foreach (char letter in sentence.text.ToCharArray())
            {
                text.text += letter;
                yield return new WaitForSeconds(sentence.talkSpeed);
            }
            */

            yield return new WaitForSeconds(sentence.lingerDuration);
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

    // A class to contain all of the information needed to properly render a sentence.
    private class Sentence {

        public string text;
        public float talkSpeed;
        public float lingerDuration;

        public Sentence(string text, float talkSpeed, float lingerDuration) {
            this.text = text;
            this. talkSpeed = talkSpeed;
            this.lingerDuration = lingerDuration;
        }
    }

}