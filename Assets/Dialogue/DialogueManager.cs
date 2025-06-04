using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("UI 요소 - Inspector 에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본설정")]
    public Sprite defaultCharacterImage;

    [Header("타이핑 효과설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypeingOnClick = true;


    //내부변수들
    private DialogueDataSo currentDialogue;     //현재 진행중인 대화 데이터
    private int currentLineIndex = 0;           //현재 몇 번째 대화 중인지 
    private bool isDialogueActive = false;      //대화 진행중인지 확인 하는 플래그
    private bool isTyping = false;              //현재 타이핑 효과가 진행중인지 확인
    private Coroutine typingCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyUp(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textType)                  //타이핑 할 전체 텍스트
    {
        isTyping = true;                                    //타이핑시작
        dialogueText.text = "";                             //텍스트 초기화

        for (int i = 0; i < textType.Length; i++)          //텍스트를 한 글자씩 추가
        {
            dialogueText.text += textType[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }    
    
    void ShowCurrentLine()
    {
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }

        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypeingOnClick)
        {
            CompleteTyping();
        }
        else
        {
            ShowNextLine();
        }
    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSo dialogue)
    {
        if (dialogue != null || dialogue.dialogueLines.Count == 0) return;
       currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if(dialogue.characterName != null)
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;
            }
        }

        ShowCurrentLine();
    }
}
