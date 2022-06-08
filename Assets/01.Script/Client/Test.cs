using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    #region �� ����
    [Header("�� ����")]
    public Sprite gunSprite;
    public string gunName;
    public string desc;
    public float reloadDelay;
    #endregion

    #region �߻� ����
    [Header("�߻����")]
    public Transform firePos;
    public Vector2 recoil;
    public float fireDelay;
    #endregion

    #region Manager
    [Header("Manager")]
    public GameManager gameManager;
    public UIManager uiManager;
    #endregion

    #region Ǯ��
    [Header("Ǯ��")]
    public Queue<GameObject> poolQueue;
    public GameObject poolObj;
    public float poolTime;
    #endregion

    #region ����
    [Header("����")]
    public AudioClip reloadClip;
    public AudioClip gunFireClip;
    #endregion

    #region ������Ƽ
    public string Desc { get { return desc; } }
    public string GunName { get { return $"Gun : {gunName}"; } set { gunName = value.Trim(); } }
    public Sprite GunSprite { get { return gunSprite; } }
    #endregion

    private void Start()
    {
    }
    private void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        
    }
    public void OnDestroy()
    {
    }
    /// <summary>
    /// �ѹ߻� �Լ�
    /// </summary>
    /// <param name="_dmg">������</param>
    /// <param name="_firePos">�߻� ��ġ</param>
    public void Shoot(int _dmg, Vector3 _firePos)
    {

    }
    /// <summary>
    /// �� ���� �Լ�
    /// </summary>
    /// <param name="_reloadStartClip">���� ���� Ŭ��</param>
    /// <param name="_reloadEndClip">���� �Ϸ� Ŭ��</param>
    public void Reload(AudioClip _reloadStartClip, AudioClip _reloadEndClip)
    {

    }
    /// <summary>
    /// Ŀ���� ���� ��� �Լ�
    /// </summary>
    /// <param name="_clip">��� �Լ�</param>
    /// <param name="_playPos">��� ��ġ</param>
    public void PlaySound(AudioClip _clip, Vector3 _playPos)
    {

    }
    /// <summary>
    /// �����Լ�(������ ����)
    /// </summary>
    /// <param name="_temp">�ӽ� ����</param>
    public void Temp(int _temp)
    {

    }
    /// <summary>
    /// �ʱ�ȭ �Լ�
    /// </summary>
    /// <param name="_callBackMsg">�ʱ�ȭ �ݹ� ���ڿ�</param>
    public void Init(string _callBackMsg)
    {

    }
}
