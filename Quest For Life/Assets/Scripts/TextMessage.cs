using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour
{
    public enum MessageStatus
    {
        WAITING,
        READING,
        READ,
        DONE
    }
    [HideInInspector]
    public MessageStatus messageStatus;

    [HideInInspector]
    public string Text;
    [HideInInspector]
    public Vector2 SpawnPos;

    string CurrentText;

    [SerializeField]
    float TimeToDestroy = 0.5f;
    float DestroyTimer;

    float TimeForNextLetter = 0.01f;
    float NextLetterTimer;

    Text t;
    RectTransform r;

    [HideInInspector]
    public MessageDisplayer manager;

    int letterNum;

    public void Init(string text, Vector2 SpawnPos, MessageDisplayer manager, MessageStatus status)
    {
        this.Text = text;
        this.SpawnPos = SpawnPos;
        letterNum = 1;
        CurrentText = text.Substring(0, letterNum);
        this.manager = manager;
        this.messageStatus = status;

        t = GetComponent<Text>();
        r = GetComponent<RectTransform>();

        t.text = CurrentText;
        r.anchoredPosition = SpawnPos;

        DestroyTimer = 0;
        NextLetterTimer = 0;
    }

    void OnEnable()
    {
        messageStatus = MessageStatus.READING;
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        switch (messageStatus)
        {
            case MessageStatus.WAITING:

                break;
            case MessageStatus.READING:
                ReadMessage();
                break;
            case MessageStatus.READ:
                FinishUp();
                break;
            case MessageStatus.DONE:
                destroyObject();
                break;
            default:
                break;
        }


    }


    void ReadMessage()
    {
        if (CurrentText.Length < Text.Length)
        {
            if (NextLetterTimer < TimeForNextLetter) NextLetterTimer += Time.deltaTime;
            else
            {
                letterNum++;
                CurrentText = Text.Substring(0, letterNum);

                NextLetterTimer = 0;
            }
        } else
        {
            CurrentText = Text;
            messageStatus = MessageStatus.READ;

            if (manager.Messages.Count == 0) TimeToDestroy = 1.5f;
        }

        t.text = CurrentText;
    }

    void FinishUp()
    {
        if (DestroyTimer < TimeToDestroy) DestroyTimer += Time.deltaTime;
        else
        {
            messageStatus = MessageStatus.DONE;
        }
    }

    void destroyObject()
    {
        manager.loadNextMessage(this);


        Destroy(gameObject);
    }



}
