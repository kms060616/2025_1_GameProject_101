using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeNPC : MonoBehaviour
{

    public DialogueDataSo myDialogue;
    public DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null )
        {
            Debug.LogError("다이얼로그 매니저가 없습니다");

        }
    }

    void OnMouseDown()
    {
        if (dialogueManager == null) return;
        if (dialogueManager.IsDialogueActive()) return;
        if (myDialogue == null) return;

        dialogueManager.StartDialogue(myDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
