using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{

    private GameObject towerObj = null;

    public TowerInfo nowTowerInfo = null;

    public List<int> chooseIDs;

    public void CreateTower(int id)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];

        if (info.money > GameLevelMgr.Instance.player.money)
            return;


        GameLevelMgr.Instance.player.AddMoney(-info.money);

        if(towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }

        towerObj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);
        towerObj.GetComponent<TowerObject>().InitInfo(info);
        
        nowTowerInfo = info;

        if(nowTowerInfo.nextLev != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0)
            return;
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {

        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
