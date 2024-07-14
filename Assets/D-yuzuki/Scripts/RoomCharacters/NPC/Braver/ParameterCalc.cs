using System.Collections.Generic;
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
        private Dictionary<int, List<int>> _roomToNpcs = new Dictionary<int, List<int>>();

        private void Start()
        {
            InitializeRoomToNpcs();
        }

        private void Update()
        {
            _timeAccumulator += Time.deltaTime;
            if (_timeAccumulator <= UPDATE_INTERVAL) return;
            _timeAccumulator = 0;
            
            UpdateParameter();
            UpdateFriendship();
        }

        private void InitializeRoomToNpcs()
        {
            for (var i = 0; i < _roomBunker.RoomDetails.Length; i++)
            {
                var room = _roomBunker.RoomDetails[i].RoomNum;
                if (!_roomToNpcs.ContainsKey(room))
                {
                    _roomToNpcs[room] = new List<int>();
                }
            }
        }

        private void UpdateParameter()
        {
            // Base Parameters.
            foreach (var braver in _braverGenerator.Braver)
            {
                if (braver.CurrentState != RoomAIState.STAY_ROOM) continue;
                var stayRoom = braver.StayRoomNum;
                var roomType = _roomBunker.RoomDetails[stayRoom].RoomType;
                
                for (var i = 0; i < _braverParameter.RoomEffects[(int)roomType]._upValue.Length; i++)
                {
                    var braverNum = braver.BraverNum;
                    if (_braverParameter.RoomEffects[(int)roomType]._upValue[i] == 0) continue;
                    var newValue = _braverParameter.Parameters[braverNum, i] +
                                   _braverParameter.RoomEffects[(int)roomType]._upValue[i];
                    _braverParameter.UpdateStatus(braverNum, (BraverParameter.Parameter)i, newValue);
                }
            }
        }

        // Update friendship Parameters.
        private void UpdateFriendship()
        {
            // 部屋ごとにNPCをグループ化（辞書をクリアして再利用）
            foreach (var key in _roomToNpcs.Keys)
            {
                _roomToNpcs[key].Clear();
            }
            
            // 部屋ごとにNPCをグループ化
            for (var i = 0; i < _braverGenerator.Braver.Count; i++)
            {
                if (_braverGenerator.Braver[i].CurrentState != RoomAIState.STAY_ROOM) continue;
                var room = _braverGenerator.Braver[i].StayRoomNum;
                if (!_roomToNpcs.ContainsKey(room))
                {
                    _roomToNpcs[room] = new List<int>();
                }
                _roomToNpcs[room].Add(i);
            }

            // 同じ部屋に2人以上のNPCがいる場合の処理
            foreach (var entry in _roomToNpcs)
            {
                var roomType = _roomBunker.RoomDetails[entry.Key].RoomType;
                var upValue = _braverParameter.RoomEffects[(int)roomType]._friendPoint;
                var npcsInRoom = entry.Value;
                if (npcsInRoom.Count <= 1) continue;
                
                for (var i = 0; i < npcsInRoom.Count; i++)
                {
                    for (var j = i + 1; j < npcsInRoom.Count; j++)
                    {
                        var npc1 = npcsInRoom[i];
                        var npc2 = npcsInRoom[j];
                        // 値を更新する処理
                        //var upValue = _braverParameter.RoomEffects[(int)roomType]._friendPoint;
                        var newValue1 = _braverParameter.Friendship[npc1][npc2] + upValue;
                        var newValue2 = _braverParameter.Friendship[npc2][npc1] + upValue;
                        _braverParameter.UpdateFriendship(npc1, npc2, newValue1);
                        _braverParameter.UpdateFriendship(npc2, npc1, newValue2);
                    }
                }
            }
        }
    }
}
