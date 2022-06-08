using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Text ipText;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject playerListObject;
    private Dictionary<int, GameObject> playerList = new Dictionary<int, GameObject>();
    int disconnectedId = 0;
    public Text IpText { get { return ipText; } }
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if(disconnectedId != 0)
        {
            playerList[disconnectedId].SetActive(false);
            playerList.Remove(disconnectedId);
            disconnectedId = 0;
        }
    }
    public void Copy()
    {
        ClipboardExtension.CopyToClipboard(ipText.text);
    }
    public void AddPlayer(int id, string name)
    {
        GameObject obj = Instantiate(playerListObject, content);
        obj.GetComponent<PlayerUIInform>().SetInformation(name);
        obj.SetActive(true);
        playerList.Add(id, obj);
    }
    public void MinusPlayer(int id)
    {
        disconnectedId = id;
    }
}