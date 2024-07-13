using UnityEngine;

namespace D_yuzuki.Scripts.RoomCharacters.NPC.Braver
{
    public class ParameterCalc
    {
        private readonly RoomBunker _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        private readonly BraverParameter _braverParameter = GameObject.FindWithTag("BraverParameter").GetComponent<BraverParameter>();
        private float _timeAccumulator = 0f;
        private const float UPDATE_INTERVAL = 1f;
        public RoomType FindRoomType(int roomNum)
        {
            var target = _roomBunker.RoomDetails[roomNum];
            return target.RoomType;
        }

        public void UpdateParameter(int braverNum, RoomType roomType)
        {
            if (roomType == RoomType.None) return;
            
            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator <= UPDATE_INTERVAL) return;
            _timeAccumulator = 0;
            
            for (var i = 0; i < _braverParameter.RoomEffects[(int)roomType]._upValue.Length; i++)
            {
                if (_braverParameter.RoomEffects[(int)roomType]._upValue[i] == 0) continue;
                var newValue = _braverParameter.Parameters[braverNum, i] +
                          _braverParameter.RoomEffects[(int)roomType]._upValue[i];
                _braverParameter.UpdateStatus(braverNum, (BraverParameter.Parameter)i, newValue);
            }
            
        }
        
    }
}
