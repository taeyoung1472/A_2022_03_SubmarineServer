using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class PlayerUIInform : MonoBehaviour
{
    [SerializeField] Text nameIdText;
    public void SetInformation(string name)
    {
        nameIdText.text = name;
    }
}
