using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Dialogue;

public class DialogueManager : MonoBehaviour {

    [SerializeField] private DialogueController catDialogueController;
    [SerializeField] private DialogueController skeletonDialogueController;
    [SerializeField] private float defaultTalkSpeed;
    [SerializeField] private float defaultLingetTime;
    [SerializeField] private TextAsset dialogueFile;

    [HideInInspector] private Queue<Dialogue.DialogueLine> lineQueue;

    //List of all lines
    private List<Dialogue.DialogueLine> allLines = new List<Dialogue.DialogueLine>();


    // Start is called before the first frame update
    void Start()
    {
        loadDialogue(dialogueFile);
        lineQueue = new Queue<Dialogue.DialogueLine>();
    }

    void Update()
    {
        makeSpeak();
    }

    private void loadDialogue(TextAsset dialogueFile) {
        
        // Reading file from location

        StringReader strReader = new StringReader(dialogueFile.text);

        strReader.ReadLine();

        while (true) {
            string line = strReader.ReadLine();
            if (line == null) {
                break;
            }

            string[] lineComponents = line.Split("\t");

            string text = lineComponents[0];
            bool isSkeleton = lineComponents[1] == "Skeleton";
            float talkSpeed = float.Parse(lineComponents[2]) * defaultTalkSpeed;
            float lingerDuration = float.Parse(lineComponents[3]);
            int followingLine = int.Parse(lineComponents[4]);

            Dialogue.DialogueLine dialogueLine = new Dialogue.DialogueLine(text, isSkeleton, talkSpeed, lingerDuration, followingLine);

            allLines.Add(dialogueLine);
        }
    }

    // Adjusts a spreadsheet index to an array index
    private int sheetToIndex(int num) {
        int spreadsheetOffset = 2;
        return num - spreadsheetOffset;
    }

    // This gets the line with the given index. The index is adjusted so that the input can match the spreadsheet.
    private Dialogue.DialogueLine getLine(int lineNumber) {
        return allLines[sheetToIndex(lineNumber)];
    }

    public void triggerDialogue(int lineNumber) {
        // Get line
        Dialogue.DialogueLine dialogueLine = allLines[sheetToIndex(lineNumber)];
        
        lineQueue.Enqueue(dialogueLine);

        if (dialogueLine.followingLine != -1) {
            triggerDialogue(dialogueLine.followingLine);
        }
    }

    private void makeSpeak() {
        if (lineQueue.Count > 0 && !(catDialogueController.speaking || skeletonDialogueController.speaking)) {
            Dialogue.DialogueLine dialogueLine = lineQueue.Dequeue();

            if (dialogueLine.isSkeleton) {
                skeletonDialogueController.Speak(dialogueLine);
            } else {
                catDialogueController.Speak(dialogueLine);
            }
        }
    }
}
