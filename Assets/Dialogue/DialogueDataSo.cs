using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogue" , menuName = "Dialogue/dialogueData")]
public class DialogueDataSo : ScriptableObject
{

    [Header("캐릭터 정보")]
    public string characterName = "캐릭터";                 //대화 창에 표시될 캐릭터 이름
    public Sprite characterImage;                           //

    [Header("대화내용")]
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
