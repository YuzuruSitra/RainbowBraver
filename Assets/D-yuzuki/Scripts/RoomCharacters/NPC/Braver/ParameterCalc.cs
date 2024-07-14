using System;
using UnityEngine;

namespace D_yuzuki.Scripts.RoomCharacters.NPC.Braver
{
    public class ParameterCalc : MonoBehaviour
    {
        [SerializeField] private RoomBunker _roomBunker;
        [SerializeField] private BraverParameter _braverParameter;
        [SerializeField] private BraverGenerator _braverGenerator;
        private float _timeAccumulator;
        private const float UPDATE_INTERVAL = 1f;
        private BraverController[] _braverController;
        
        public RoomType FindRoomType(int roomNum)
        {
            var target = _roomBunker.RoomDetails[roomNum];
            return target.RoomType;
        }

        private void Update()
        {
            UpdateParameter();
        }

        public void UpdateParameter()
        {
            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator <= UPDATE_INTERVAL) return;
            _timeAccumulator = 0;
            
            // Base Parameters.
            foreach (var braver in _braverGenerator.Braver)
            {
                if (braver.CurrentState != RoomAIState.STAY_ROOM) continue;
                var stayRoom = braver.StayRoomNum;
                var roomType = _roomBunker.RoomDetails[stayRoom].RoomType;
                
                for (var n = 0; n < _braverParameter.RoomEffects[(int)roomType]._upValue.Length; n++)
                {
                    var braverNum = braver.BraverNum;
                    if (_braverParameter.RoomEffects[(int)roomType]._upValue[n] == 0) continue;
                    var newValue = _braverParameter.Parameters[braverNum, n] +
                                   _braverParameter.RoomEffects[(int)roomType]._upValue[n];
                    _braverParameter.UpdateStatus(braverNum, (BraverParameter.Parameter)n, newValue);
                }
            }
            
        }
        
    }
}
