using System.Collections.Generic;
using UnityEngine;

// BraverDataContainerの値を変更するクラス
public class BraverDataChanger
{
    private BraverDataContainer _braverDataContainer;

    public BraverDataChanger()
    {
        _braverDataContainer = BraverDataContainer.Instance;
    }

    public void ChangeMaxHP(int id, int newMaxHP)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.hp = newMaxHP;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeHP(int id, int newHP)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.hp = newHP;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeMaxMP(int id, int newMaxMP)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.mp = newMaxMP;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeMP(int id, int newMP)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.mp = newMP;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangePAtk(int id, int newPAtk)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.pAtk = newPAtk;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangePDef(int id, int newPDef)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.pDef = newPDef;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeMAtk(int id, int newMAtk)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.mAtk = newMAtk;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeMDef(int id, int newMDef)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.mDef = newMDef;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeSpeed(int id, int newSpeed)
    {
        if (ErrorCheck(id)) return;
        var tmpData = _braverDataContainer.BraversData[id];
        tmpData.speed = newSpeed;
        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    public void ChangeFriendShip(int id, int targetID, int newFriendShip)
    {
        if (ErrorCheck(id)) return;

        var tmpData = _braverDataContainer.BraversData[id];

        var friendshipLevels = new List<BraverDataContainer.FriendShipLevel>(tmpData.friendShipLevel);
        var targetFriendship = friendshipLevels[targetID];

        // 経験値とレベルを更新(仮置き)
        float newExp = targetFriendship.exp + newFriendShip;
        while (newExp >= 100)
        {
            newExp -= 100;
            targetFriendship.level += 1;
        }
        targetFriendship.exp = newExp;
        friendshipLevels[targetID] = targetFriendship;
        tmpData.friendShipLevel = friendshipLevels;

        _braverDataContainer.ChangeBraverData(this, id, tmpData);
    }

    private bool ErrorCheck(int id)
    {
        if (id < 0 || id >= _braverDataContainer.BraversData.Count)
        {
            Debug.LogError($"List out of bounds. The given id {id} does not exist in the list.");
            return true;
        }
        return false;
    }

}
