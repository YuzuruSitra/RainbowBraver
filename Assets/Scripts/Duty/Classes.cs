using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classes : MonoBehaviour
{
    // <summary>
    // Duty
    // 1つのクエストの流れを定義するクラス
    // クエスト情報やフェーズの内容など
    // 「フェーズ」の定義は下記Phaseクラスのsummaryで確認お願いします
    // </summary>
    public class Duty{
        // クエストのタイトル
        public string title { get; private set; }
        // クエストの説明文
        public string description { get; private set; }

        // フェーズのデータ
        public List<Phase> phases { get; private set; }

        // 味方キャラクターのデータ
        public List<Unit> allies { get; private set; }
    }


    // <summary>
    // Phase
    // 戦闘の1フェーズの進行を定義するクラス
    // 「1フェーズ」の定義は、任意の敵のグループに遭遇してから、敵か味方が全滅するか、味方が退却するまでの期間
    // </summary>
    public class Phase{
        // 敵が出現する数
        public int HostileAmount;

        // 出現する敵を選択する方法
        // 0: random (hostilesの中からランダム), 1: fixed (hostilesの上から順にhostileAmount体が出現)
        public int HostileSpawnMode;

        public List<Unit> Hostiles;

        public Phase(int hostileAmount, int hostileSpawnMode, List<Unit> hostiles){
            HostileAmount = hostileAmount;
            HostileSpawnMode = hostileSpawnMode;

            Hostiles = hostiles;
        }
    }


    // <summary>
    // Unit
    // 敵味方問わず、戦闘の主体となるオブジェクト1つを定義するクラス
    // とりあえず全てpublicにしていますが、仕様が固まり次第最小限にしていく予定です
    // </summary>
    public class Unit{
        // 敵味方の別
        // 0: 味方(ally), 1: 敵(hostile)
        // 特殊なユニットを作成できるようintで実装。分かりにくくてまずそうだったら変えます。
        public int Affiliation { get; set; }

        public int JobId { get; set; }

        public int Hp { get; set; }
        public int Mp { get; set; }

        // 物理攻撃力
        public float PAtk { get; set; }
        // 敵の基本的な物理攻撃の技名
        public string GenericPAtkLabel { get; set; }
        // 物理防御力
        public float PDef { get; set; }

        // 魔法攻撃力
        public float MAtk { get; set; }
        // 敵の基本的な魔法攻撃の技名
        public string GenericMAtkLabel { get; set; }
        // 魔法防御力
        public float MDef { get; set; }

        // すばやさ
        public float Speed { get; set; }
        // 技値
        public float Tech { get; set; }
        // 運
        public float Luck { get; set; }

        // 出現可能性
        public float Probability { get; set; }

        public Unit(int affiliation, int jobId, int hp, int mp, float pAtk, string genericPAtkLabel, float pDef, float mAtk, string genericMAtkLabel, float mDef, float speed, float tech, float luck, float probability){
            Affiliation = affiliation;
            JobId = jobId;

            Hp = hp;
            Mp = mp;

            PAtk = pAtk;
            GenericPAtkLabel = genericPAtkLabel;
            PDef = pDef;

            MAtk = mAtk;
            GenericMAtkLabel = genericMAtkLabel;
            MDef = mDef;

            Speed = speed;
            Luck = luck;

            Probability = probability;
        }
    }
}
