using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogue" , menuName = "Dialogue/dialogueData")]
public class DialogueDataSo : ScriptableObject
{

    [Header("ĳ���� ����")]
    public string characterName = "ĳ����";                 //��ȭ â�� ǥ�õ� ĳ���� �̸�
    public Sprite characterImage;                           //

    [Header("��ȭ����")]
    [TextArea(3,10)]
    public List<string> dialogueLines = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
