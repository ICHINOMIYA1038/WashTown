using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ShopRank�̒萔
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
    /// TownRank�̒萔
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
    /// shopRank�̂������l
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
    /// TownRank�̂������l
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
    /// townRank�̒l�@�����l��0,�ő�5�܂�
    /// </summary>
    int townRank=0;
    /// <summary>
    /// shopRank�̒l�@�����l��0,�ő�5�܂�
    /// </summary>
    int shopRank=0;
    /// <summary>
    /// townRate��townRank�����߂邽�߂̃��[�g�l
    /// </summary>
    int townRate =0;
    /// <summary>
    /// shopRate��shopRank�����߂邽�߂̃��[�g�l
    /// </summary>
    int shopRate = 0;
    /// <summary>
    /// �X�̎����Ă��邨��(4byte�Ȃ̂ŁA2147483647���ő�Ȃ̂ŋC��t����)
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
