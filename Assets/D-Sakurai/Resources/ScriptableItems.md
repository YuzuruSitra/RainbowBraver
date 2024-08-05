# ScriptableObject一覧

戦闘関連ScriptableObjectと、関連する要素の一覧。

以下、ScriptableObject = so

## Enemies (SO)

敵を管理するso

### Members

- `Name (string)`
  - 名前
- `MaxHp (int)`
  - 最大HP
- `PAtk (float)`
  - 物理攻撃力
- `MAtk (float)`
  - 魔法攻撃力
- `GenericAttackLabel (string)`
  - スキルでない、普通の攻撃をするときの技名
- `PDef (float)`
  - 物理防御力
- `MDef (float)`
  - 魔法防御力
- `Speed (int)`
  - 行動速度
- `EnemySkillIds(int[])`
  - 使用する`EnemySkills`のインデックス

## SkillBase (NameSpace)

敵味方、種別関係なくスキル情報を構成するクラスが置かれる名前空間。

一つのスキルは名前や解説、使用条件等を定義した`HogeSkillData`として定義し、攻撃・回復といった具体的な動作を`HogeSkillData.SkillProperties (HogeSkillProperty[])`に格納して管理する。

単一のスキルを複数のスキルプロパティで構成するため、攻撃後に回復することで吸血技を実装したり……が可能(だと思います)。

### 含まれるクラス
- `BraverSkillData`
  - 職業スキル、性格スキル
- `BraverSkillProperty`
- `EnemySkillData`
  - 敵が使用するスキル
- `EnemySkillProperty`

## JobSkills (SO)

職業スキルを管理するso

### Members

- `JobSkillArray (SkillBase.EnemySkillData[])`
  - 職業スキルの配列

## PersonalitySkills (SO)

性格スキルを管理するso

### Members

- `JobSkillArray (SkillBase.EnemySkillData[])`
  - 性格スキルの配列

## EnemySkills (SO)

敵が使用するスキルを管理するso

### Members

- `EnemySkillsData (SkillBase.EnemySkillData[])`
  - 敵が使用するスキルの配列

## StatusEffectBase (NameSpace)

状態効果の定義に使用するクラス群を含む名前空間。

構造は`SkillBase`と同様

### 含まれるクラス
- `StatusEffectData`
- `StatusEffectProperty`

## StatusEffects (SO)

状態効果を管理するso。

### Members

- `StatusEffectsData (StatusEffectBase.StatusEffectData[])` 