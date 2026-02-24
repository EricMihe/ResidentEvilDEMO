using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;

    public PlayerObject player;

    private List<MonsterPoint> points = new List<MonsterPoint>();

    private int nowWaveNum = 0;

    private int maxWaveNum = 0;

    private List<MonsterObject> monsterList = new List<MonsterObject>();

    private GameLevelMgr()
    {

    }

    public void InitInfo(SceneInfo info)
    {
        UIManager.Instance.ShowPanel<GamePanel>();
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, info.money);
        
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);
    }

    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }
    public void UpdatgeMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())
                return false;
        }

        if (monsterList.Count > 0)
            return false;

        Debug.Log("游戏胜利");
        return true;
    }

    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }

    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }

    public MonsterObject FindMonster(Vector3 pos, int range)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            if( !monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                return monsterList[i];
            }
        }
        return null;
    }

    public List<MonsterObject> FindMonsters(Vector3 pos, int range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
