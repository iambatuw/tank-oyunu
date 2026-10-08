using UnityEngine;

namespace Tanks.Complete
{
    public class PowerUp : MonoBehaviour
    {
        public enum PowerUpType { Speed, DamageReduction, ShootingBonus, Healing, Invincibility, DamageMultiplier }
        [Tooltip("Select the kind of Power Up that you want.")]
        [SerializeField] private PowerUpType m_PowerUpType = PowerUpType.DamageReduction;

        [Tooltip("Particle to emit when this Power Up is collected.")]
        [SerializeField] private ParticleSystem m_CollectFX;
        [Tooltip("Time in seconds that this Power Up will be active.")]
        [SerializeField] private float m_DurationTime = 5f;

        [Header("Damage Reduction")]
        [Tooltip("Percentage of damage reduction [0 , 1].")]
        [SerializeField] private float m_DamageReduction = 0.5f;

        [Header("Speed Bonus")]
        [Tooltip("Extra speed value of the tank.")]
        [SerializeField] private float m_SpeedBonus = 5f;
        [Tooltip("Extra turn speed value of the tank.")]
        [SerializeField] private float m_TurnSpeedBonus = 0f;

        [Header("Shooting Bonus")]
        [Tooltip("Percentage of reduction in the cooldown shooting time (0 , 1].")]
        [SerializeField] private float m_CooldownReduction = 0.5f;

        [Header("Healing")]
        [Tooltip("Life that will recover the tank.")]
        [SerializeField] private float m_HealingAmount = 20f;

        [Header("Extra Damage")]
        [Tooltip("Amount by which the damage will be multiplied.")]
        [SerializeField] private float m_DamageMultiplier = 2f;

        private PowerUpSpawner m_Spawner;               // Reference to the spawner that instantiated this PowerUp
        private bool m_Collected;
        private static readonly System.Collections.Generic.List<PowerUp> s_ActivePowerUps =
            new System.Collections.Generic.List<PowerUp>();

        public static System.Collections.Generic.IReadOnlyList<PowerUp> ActivePowerUps => s_ActivePowerUps;
        public PowerUpType Type => m_PowerUpType;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetActivePowerUps() => s_ActivePowerUps.Clear();

        private void OnEnable()
        {
            m_Collected = false;
            if (!s_ActivePowerUps.Contains(this)) s_ActivePowerUps.Add(this);
        }

        private void OnDisable() => s_ActivePowerUps.Remove(this);

        public bool CanBeCollectedBy(PowerUpDetector detector) =>
            isActiveAndEnabled && !m_Collected && detector != null && detector.CanCollectPowerUp;

        private void Update()
        {
            // Rotates the power up game object
            transform.rotation = Quaternion.Euler(0, 50f * Time.time, 0);
        }


        private void OnTriggerEnter(Collider other)
        {
            // Tank colliders can live on children; resolve the actual tank through its rigidbody.
            var detector = other.attachedRigidbody != null
                ? other.attachedRigidbody.GetComponent<PowerUpDetector>()
                : other.GetComponentInParent<PowerUpDetector>();
            if (detector != null && detector.gameObject.layer == LayerMask.NameToLayer("Players"))
            {
                // Reference to the PowerUpDetector component of the tank.
                PowerUpDetector m_PowerUpDetector = detector;

                // Checks that the tank has not picked up other power up
                if (CanBeCollectedBy(m_PowerUpDetector))
                {
                    m_Collected = true;
                    s_ActivePowerUps.Remove(this);
                    // The power up reduces is a shield
                    if (m_PowerUpType == PowerUpType.DamageReduction)
                        m_PowerUpDetector.PickUpShield(m_DamageReduction, m_DurationTime);
                    // The power up enhances any speed stat
                    else if (m_PowerUpType == PowerUpType.Speed)
                        m_PowerUpDetector.PowerUpSpeed(m_SpeedBonus, m_TurnSpeedBonus, m_DurationTime);
                    // The power up enhances any shooting stat
                    else if (m_PowerUpType == PowerUpType.ShootingBonus)
                        m_PowerUpDetector.PowerUpShoootingRate(m_CooldownReduction, m_DurationTime);
                    // The power up heals the tank
                    else if (m_PowerUpType == PowerUpType.Healing)
                        m_PowerUpDetector.PowerUpHealing(m_HealingAmount);
                    // The power up makes the tank invincible
                    else if (m_PowerUpType == PowerUpType.Invincibility)
                        m_PowerUpDetector.PowerUpInvincibility(m_DurationTime);
                    // The power up increases the damage of the shell
                    else if (m_PowerUpType == PowerUpType.DamageMultiplier)
                        m_PowerUpDetector.PowerUpSpecialShell(m_DamageMultiplier);

                    // Tells the spawner that the power up has been collected
                    if (m_Spawner != null)
                        m_Spawner.CollectPowerUp();

                    // Instantiates the PowerUp effects
                    if (m_CollectFX != null)
                    {
                        var fx = Instantiate(m_CollectFX, transform.position, Quaternion.identity);
                        if (fx != null && fx.GetComponent<PowerUpFX>() == null)
                        {
                            Destroy(fx.gameObject, fx.main.duration + 0.5f);
                        }
                    }

                    // Destroys the Power Up
                    Destroy(gameObject);

                    TankMovement movement = detector.GetComponent<TankMovement>();
                    string playerLabel = (movement != null && movement.m_PlayerNumber == 1) ?
                        (TankDuelLocalization.IsTurkish ? "1. OYUNCU" : "P1") :
                        (TankDuelData.AIModeEnabled ? (TankDuelLocalization.IsTurkish ? "BİLGİSAYAR" : "BOT") : (TankDuelLocalization.IsTurkish ? "2. OYUNCU" : "P2"));
                    string puName = m_PowerUpType switch
                    {
                        PowerUpType.DamageReduction => TankDuelLocalization.Get("PU_SHIELD"),
                        PowerUpType.Speed => TankDuelLocalization.Get("PU_SPEED"),
                        PowerUpType.ShootingBonus => TankDuelLocalization.Get("PU_FIRE"),
                        PowerUpType.Healing => TankDuelLocalization.Get("PU_HEAL"),
                        PowerUpType.Invincibility => TankDuelLocalization.Get("PU_INVINCIBLE"),
                        PowerUpType.DamageMultiplier => TankDuelLocalization.Get("PU_DAMAGE"),
                        _ => (TankDuelLocalization.IsTurkish ? "GÜÇLENDİRME" : "POWER-UP")
                    };
                    TankDuel.ShowToast($"{playerLabel}: {puName}");
                }
            }
        }

        private void OnTriggerStay(Collider other) => OnTriggerEnter(other);

        // Sets m_Spawner
        public void SetSpawner(PowerUpSpawner spawner)
        {
            m_Spawner = spawner;
        }
    }
}
