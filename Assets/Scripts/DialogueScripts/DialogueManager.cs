using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Dialogue;

public class DialogueManager : MonoBehaviour {

    [SerializeField] private DialogueController catDialogueController;
    [SerializeField] private DialogueController skeletonDialogueController;
    [SerializeField] private float defaultTalkSpeed;

    //List of all lines
    private List<Dialogue.DialogueLine> allLines = new List<Dialogue.DialogueLine>();


    // Start is called before the first frame update
    void Start()
    {
        loadDialogue("Assets/Dialogue/Dialogue.tsv");
    }

    private void loadDialogue(string path) {
        
        // Reading file from location

        StreamReader strReader = new StreamReader(path);

        strReader.ReadLine();

        while (true) {
            string line = strReader.ReadLine();
            if (line == null) {
                break;
            }

            string[] lineComponents = line.Split("\t");

            string text = lineComponents[0];
            bool isSkeleton = lineComponents[1] == "1";
            float talkSpeed = float.Parse(lineComponents[2]) * defaultTalkSpeed;
            float lingerDuration = float.Parse(lineComponents[3]);


            Dialogue.DialogueLine dialogueLine = new Dialogue.DialogueLine(text, isSkeleton, talkSpeed, lingerDuration);

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

        if (dialogueLine.isSkeleton) {
            skeletonDialogueController.Speak(dialogueLine);
        } else {
            catDialogueController.Speak(dialogueLine);
        }
    }
}
