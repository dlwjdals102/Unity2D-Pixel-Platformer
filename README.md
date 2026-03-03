## 개요
Unity 2D로 만든 포트폴리오용 프로젝트입니다.

## 프로젝트 소개
플랫폼, 액션 RPG 장르를 참고해서 제작했습니다.  
여러 맵을 탐험하고 실시간으로 적들과 전투를 치르는 게임입니다. 

프로젝트명: 
개발 기간: 2025.02 ~  
개발 인원: 1인 개발  
엔진: Unity 6.3  
언어: C#  

## 핵심 기능
### FSM 기반 캐릭터 제어
* 추상 클래스 기반의 상태패턴을 도입하여 각 동작 (Idle, Move, Attack 등)을 독립된 클래스로 캡슐화.<br>
* 상태 전이 시 Exit/Enter 로직을 명확히 분리해 상태 충돌을 차단.<br>
* 새로운 동작 추가 시 기존 코드를 수정하지 않고 클래스 추가만으로 확장 가능한 구조.<br>
<details>
<summary>코드</summary>
<div markdown="1">

## State
```c#
public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public virtual void UpdateAnimationParameters()
    {
    
    }
}

```

## StateMachine
```c#
public class StateMachine 
{
    public EntityState currentState { get; private set; }
    public bool canChangeState;

    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        if (!canChangeState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();        
    }

    public void SwitchOffStateMachine()
    {
        canChangeState = false;
    }
}

```

</div>
</details>

<br>

### 전투 시스템
* IDamageable 인터페이스를 정의하여 플레이어, 몬스터, 파괴 가능한 오브젝트에 일괄 적용.<br>
* 공격 주체는 타겟의 세부 타입을 알 필요 없이 TakeDamage() 메서드만 호출하면 되므로 객체 간 결합도를 낮춤.<br>
* 히트박스 OverlapCircleAll 사용 시 특정 애니메이션 프레임에서만 실행, LayerMask를 지정하여 불필요한 연산을 배제하고 성능 오버헤드 최소화.<br>
* 전투 로직을 별도 컴포넌트로 분리하여 캐릭터 컨트롤러의 비대화 방지.<br>

<details>
<summary>코드</summary>
<div markdown="1">
  
## 공격
```c#
public void PerformAttack()
{
    bool targetGotHit = false;

    foreach (var target in GetDetectedColliders())
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null) continue;
        
        targetGotHit = damageable.TakeDamage(stats.GetDamage(), transform);

        if (targetGotHit)
        {
            vfx?.CreateOnHitVFX(target.transform);
            sfx?.PlayAttackHit();
        }
    }

    if (!targetGotHit)
        sfx?.PlayAttackMiss();
}

private Collider2D[] GetDetectedColliders()
{
    return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
}

```

## 히트
```c#

public virtual bool TakeDamage(float damage, Transform damageDealer)
{
    if (isDead) return false;

    if (entity != null)
    {
        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);
        entity.ReciveKnockback(knockback, duration);
    }

    ReduceHealth(damage);

    return true;
}

```

</div>
</details>

<br>

### ScriptableObject 기반의 데이터 시스템
* 캐릭터 기본 스탯(HP, Speed 등)과 오디오 리소스 데이터를 ScriptableObject(SO)로 자산화.
* 데이터와 로직의 분리를 통해 기획 수치 변경 시 코드 수정 없이 인스펙터에서 즉시 수정 가능하도록 설계.
* 동일한 프리팹이 여러 개 생성되어도 데이터(SO)는 하나만 참조하므로 메모리 사용 효율성 증대.

<details>
<summary>코드</summary>
<div markdown="1">

## Stat
```c#
[System.Serializable]
public class Stat
{
    [SerializeField] private float value;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>(); 


    public float GetValue()
    {
        return GetFinalValue();
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifiers => modifiers.source == source);
    }

    private float GetFinalValue()
    {
        float finalValue = value;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }

        return finalValue;
    }
}

[System.Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}

```
## Entity Stats
```c#
public class EntityStats : MonoBehaviour
{
    public StatSetupSO defaultStatSetup;

    public Stat maxHealth;
    public Stat damage;
    public Stat armor;
    public Stat attackSpeed;

    public float GetMaxHealth()
    {
        return maxHealth.GetValue();
    }

    public float GetDamage()
    {
        return damage.GetValue();
    }

    public float GetArmor()
    {
        return armor.GetValue();
    }

    public float GetAttackSpeed()
    {
        return attackSpeed.GetValue();
    }


    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        maxHealth.SetValue(defaultStatSetup.maxHealth);
        damage.SetValue(defaultStatSetup.damage);
        armor.SetValue(defaultStatSetup.armor);
        attackSpeed.SetValue(defaultStatSetup.attackSpeed);
    }
}


```

## StatSetupSO (ScriptableObject)
```c#
[CreateAssetMenu(menuName = "RPG Setup/Defalut Stat Setup", fileName = "Default Stat Setup")]
public class StatSetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100;

    [Header("Offense")]
    public float damage = 10;
    public float magicDamage = 12;
    public float attackSpeed = 1;

    [Header("Defense")]
    public float armor;

    // 필요한 거 있으면 추가
}

```

</div>
</details>

<br>

### Save System
* GameData(데이터), DataHandler(I/O), SaveManager(컨트롤러)의 3레이어 구조.<br>
* 객체 데이터를 JSON 형식으로 변환하여 로컬 저장 기능을 구현하고, Application.persistentDataPath를 활용해 플랫폼 독립적 저장 경로 확보.<br>
* ISaveable 인터페이스를 구현한 객체들이 SaveManager에 자신을 등록하는 방식을 사용하여 씬 전체 검색 없이 효율적으로 데이터 수집.<br>
* 슬롯 번호에 따른 파일 네이밍 규칙을 적용하여 다중 세이브 데이터 관리 기능 구현.<br>

<br>

### 시스템 매니저 및 UI 컨트롤
### Game Manager : 
* 씬 전환 시 발생할 수 있는 데이터 손실을 방지하고, 사용자 경험(UX)을 위해 시각적 효과(Fade)와 로직을 동기화.<br>
* 씬 전환 시점의 경쟁 상태을 방지하고, 로딩 중 멈춤 현상 없는 부드러운 화면 전환.<br>

<details>
<summary>코드</summary>
<div markdown="1">

코드1

</div>
</details>

<br>

### Audio System :
* 갑작스러운 사운드 변화로 인한 이질감을 줄이고, 사운드의 거리감을 계산하여 몰입감을 높임.<br>
* 코루틴을 이용해 배경음(BGM) 전환 시 볼륨을 선형 보간(Mathf.Lerp)하여 부드러운 사운드 교체를 구현.<br>
* 오디오 리소스를 DB화하여 관리하고, Audio Mixer와 PlayerPrefs를 연동하여 실시간 음량 조절 및 설정 자동 로드 기능을 구현.<br>

<details>
<summary>코드</summary>
<div markdown="1">

코드1

</div>
</details>

<br>

### UI Control :
* PlayerInput 시스템을 UI 매니저에서 참조하여, UI 열릴 때 캐릭터 컨트롤을 비활성화하는 로직을 구현, 메뉴 조작 중 캐릭터가 움직이거나 공격하는 버그 차단.<br>
* GetComponentInChildren을 활용한 자동 참조로 UI 요소 관리의 편의성을 높임.<br>

<details>
<summary>코드</summary>
<div markdown="1">

코드1

</div>
</details>

<br>




