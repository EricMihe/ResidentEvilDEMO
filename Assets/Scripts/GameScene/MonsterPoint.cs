using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;
    //每波怪物有多少只
    public int monsterNumOneWave;
    //用于记录 当前波的怪物还有多少只没有创建
    private int nowNum;

    //怪物ID 允许有多个 这样就可以随机创建不同的怪物 更具多样性
    public List<int> monsterIDs;
    //用于记录 当前波 要创建什么ID的怪物
    private int nowID;

    //单只怪物创建间隔时间
    public float createOffsetTime;

    //波与波之间的间隔时间
    public float delayTime;

    //第一波怪物创建的间隔时间
    public float firstDelayTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);

        //记录出怪点
        GameLevelMgr.Instance.AddMonsterPoint(this);
        //更新最大波数
        GameLevelMgr.Instance.UpdatgeMaxNum(maxWave);
    }

    /// <summary>
    /// 开始创建一波的怪物
    /// </summary>
    private void CreateWave()
    {

        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];

        nowNum = monsterNumOneWave;

        CreateMonster();

        --maxWave;
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }

    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateMonster()
    {
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];


        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position, Quaternion.identity);

        MonsterObject monsterObj = obj.AddComponent<MonsterObject>();
        monsterObj.InitInfo(info);
        GameLevelMgr.Instance.AddMonster(monsterObj);

        --nowNum;
        if( nowNum == 0 )
        {
            if (maxWave > 0)
                Invoke("CreateWave", delayTime);
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
