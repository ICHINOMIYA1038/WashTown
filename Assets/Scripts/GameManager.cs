using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ShopRankの定数
    /// </summary>
    enum ShopRank
    {
        zero,
        one,
        two,
        three,
        four,
        five,
    }
    /// <summary>
    /// TownRankの定数
    /// </summary>
    enum TownRank
    {
        zero,
        one,
        two,
        three,
        four,
        five,
    }

    /// <summary>
    /// shopRankのしきい値
    /// </summary>
    enum ShopRateThreshHold
    {
        zeroToOne = 1000,
        oneToTwo = 2000,
        twoToThree = 3000,
        threeToFour = 4000,
        fourToFive = 5000,
    }

    /// <summary>
    /// TownRankのしきい値
    /// </summary>
    enum TownRateThreshHold
    {
        zeroToOne = 1000,
        oneToTwo = 2000,
        twoToThree = 3000,
        threeToFour = 4000,
        fourToFive = 5000,
    }

    /// <summary>
    /// townRankの値　初期値は0,最大5まで
    /// </summary>
    int townRank=0;
    /// <summary>
    /// shopRankの値　初期値は0,最大5まで
    /// </summary>
    int shopRank=0;
    /// <summary>
    /// townRateはtownRankを決めるためのレート値
    /// </summary>
    int townRate =0;
    /// <summary>
    /// shopRateはshopRankを決めるためのレート値
    /// </summary>
    int shopRate = 0;
    /// <summary>
    /// 店の持っているお金(4byteなので、2147483647が最大なので気を付ける)
    /// </summary>
    int money = 0;

    void addTownRate(int num)
    {
        townRate += num;
    }

    void addShopRate(int num)
    {
        shopRate += num;
    }
    void addShop(int num)
    {
        shopRate += num;
    }

    void checkShopRate()
    {
        
    }

}
