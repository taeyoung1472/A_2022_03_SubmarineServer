using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    #region 총 정보
    [Header("총 정보")]
    public Sprite gunSprite;
    public string gunName;
    public string desc;
    public float reloadDelay;
    #endregion

    #region 발사 관련
    [Header("발사관련")]
    public Transform firePos;
    public Vector2 recoil;
    public float fireDelay;
    #endregion

    #region Manager
    [Header("Manager")]
    public GameManager gameManager;
    public UIManager uiManager;
    #endregion

    #region 풀링
    [Header("풀링")]
    public Queue<GameObject> poolQueue;
    public GameObject poolObj;
    public float poolTime;
    #endregion

    #region 사운드
    [Header("사운드")]
    public AudioClip reloadClip;
    public AudioClip gunFireClip;
    #endregion

    #region 프로퍼티
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
    /// 총발사 함수
    /// </summary>
    /// <param name="_dmg">데미지</param>
    /// <param name="_firePos">발사 위치</param>
    public void Shoot(int _dmg, Vector3 _firePos)
    {

    }
    /// <summary>
    /// 총 장전 함수
    /// </summary>
    /// <param name="_reloadStartClip">장전 시작 클립</param>
    /// <param name="_reloadEndClip">장전 완료 클립</param>
    public void Reload(AudioClip _reloadStartClip, AudioClip _reloadEndClip)
    {

    }
    /// <summary>
    /// 커스텀 사운드 재생 함수
    /// </summary>
    /// <param name="_clip">재생 함수</param>
    /// <param name="_playPos">재생 위치</param>
    public void PlaySound(AudioClip _clip, Vector3 _playPos)
    {

    }
    /// <summary>
    /// 예비함수(지워질 예정)
    /// </summary>
    /// <param name="_temp">임시 변수</param>
    public void Temp(int _temp)
    {

    }
    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="_callBackMsg">초기화 콜백 문자열</param>
    public void Init(string _callBackMsg)
    {

    }
}
