using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右键
    public Button btnLeft;
    public Button btnRight;

    //购买按钮
    public Button btnUnLock;
    public Text txtUnLock;

    //开始和返回
    public Button btnStart;
    public Button btnBack;

    //左上角拥有的钱
    public Text txtMoney;

    //角色信息
    public Text txtName;

    //英雄预设体需要创建在的位置
    private Transform heroPos;

    //当前场景中显示的对象
    private GameObject heroObj;
    //当前使用的数据
    private RoleInfo nowRoleData;

    private int nowIndex;

    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;


        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            ChangeHero();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfoList.Count)
                nowIndex = 0;

            ChangeHero();
        });

        btnUnLock.onClick.AddListener(() =>
        {

            PlayerData data = GameDataMgr.Instance.playerData;

            if(data.haveMoney >= nowRoleData.lockMoney)
            {

                data.haveMoney -= nowRoleData.lockMoney;

                txtMoney.text = data.haveMoney.ToString();

                data.buyHero.Add(nowRoleData.id);
 
                GameDataMgr.Instance.SavePlayerData();


                UpdateLockBtn();


                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功");
            }
            else
            {

                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("金钱不足");
            }
        });

        btnStart.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.nowSelRole = nowRoleData;


            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRgiht(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        ChangeHero();
    }

    /// <summary>
    /// 更新场景上要显示的模型的
    /// </summary>
    private void ChangeHero()
    {
        if(heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        Destroy(heroObj.GetComponent<PlayerObject>());

        txtName.text = nowRoleData.tips;

        UpdateLockBtn();
    }

    /// <summary>
    /// 更新解锁按钮显示情况
    /// </summary>
    private void UpdateLockBtn()
    {

        if( nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id) )
        {

            btnUnLock.gameObject.SetActive(true);
            txtUnLock.text = "￥" + nowRoleData.lockMoney;

            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        if(heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
