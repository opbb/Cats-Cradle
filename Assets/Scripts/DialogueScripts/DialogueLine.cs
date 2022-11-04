using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

    // A class to contain all of the information needed to properly render a sentence.
    public class DialogueLine {

        public string text;
        public bool isSkeleton;
        public float talkSpeed;
        public float lingerDuration;
        public int followingLine;

        public DialogueLine(string text, bool isSkeleton, float talkSpeed, float lingerDuration, int followingLine) {
            this.text = text;
            this.isSkeleton = isSkeleton;
            this. talkSpeed = talkSpeed;
            this.lingerDuration = lingerDuration;
            this.followingLine = followingLine;
        }
    }
}
