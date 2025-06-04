using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("UI ��� - Inspector ���� ����")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("�⺻����")]
    public Sprite defaultCharacterImage;

    [Header("Ÿ���� ȿ������")]
    public float typingSpeed = 0.05f;
    public bool skipTypeingOnClick = true;


    //���κ�����
    private DialogueDataSo currentDialogue;     //���� �������� ��ȭ ������
    private int currentLineIndex = 0;           //���� �� ��° ��ȭ ������ 
    private bool isDialogueActive = false;      //��ȭ ���������� Ȯ�� �ϴ� �÷���
    private bool isTyping = false;              //���� Ÿ���� ȿ���� ���������� Ȯ��
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

    IEnumerator TypeText(string textType)                  //Ÿ���� �� ��ü �ؽ�Ʈ
    {
        isTyping = true;                                    //Ÿ���ν���
        dialogueText.text = "";                             //�ؽ�Ʈ �ʱ�ȭ

        for (int i = 0; i < textType.Length; i++)          //�ؽ�Ʈ�� �� ���ھ� �߰�
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
