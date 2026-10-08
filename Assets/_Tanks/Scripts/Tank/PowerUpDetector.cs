using System.Collections;
using UnityEngine;
using TMPro;

namespace Tanks.Complete
{
    public class PowerUpDetector : MonoBehaviour
    {
        // Variable that indicates if the tank has a PowerUp right now
        public bool m_HasActivePowerUp = false;
        // References to the tank's components
        private TankShooting m_TankShooting;
        private TankMovement m_TankMovement;
        private TankHealth m_TankHealth;
        private PowerUpHUD m_PowerUpHUD;
        private float m_ActiveSpeedBoost;
        private float m_ActiveTurnBoost;
        private float m_ActiveCooldownFactor = 1f;
        private bool m_ShieldApplied;
        private bool m_InvincibilityApplied;

        private void Awake()
        {
            // Get references to the tank's movement, shooting, and health components
            m_TankShooting = GetComponent<TankShooting>();
            m_TankMovement = GetComponent<TankMovement>();
            m_TankHealth = GetComponent<TankHealth>();
            m_PowerUpHUD = GetComponentInChildren<PowerUpHUD>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if (m_TankMovement != null)
            {
                m_TankMovement.m_Speed -= m_ActiveSpeedBoost;
                m_TankMovement.m_TurnSpeed -= m_ActiveTurnBoost;
            }
            if (m_TankShooting != null)
            {
                if (m_ActiveCooldownFactor > 0f)
                    m_TankShooting.m_ShotCooldown /= m_ActiveCooldownFactor;
                m_TankShooting.ClearSpecialShell();
            }
            if (m_TankHealth != null)
            {
                if (m_ShieldApplied) m_TankHealth.ToggleShield(0f);
                if (m_InvincibilityApplied) m_TankHealth.ToggleInvincibility();
            }
            m_ActiveSpeedBoost = m_ActiveTurnBoost = 0f;
            m_ActiveCooldownFactor = 1f;
            m_ShieldApplied = m_InvincibilityApplied = false;
            m_HasActivePowerUp = false;
            if (m_PowerUpHUD != null) m_PowerUpHUD.DisableActiveHUD();
            var oldTexts = GetComponentsInChildren<TextMeshPro>(true);
            foreach (var t in oldTexts)
            {
                if (t != null && t.gameObject.name == "FloatingText")
                    Destroy(t.gameObject);
            }
        }

        private void ShowFloatingText(string key)
        {
            string localizedText = TankDuelLocalization.Get(key);

            // Create floating text
            GameObject floatingText = new GameObject("FloatingText");
            floatingText.transform.position = transform.position + Vector3.up * 4f;
            floatingText.transform.SetParent(transform); // Follow tank

            TextMeshPro textMesh = floatingText.AddComponent<TextMeshPro>();
            TankDuelLocalization.EnsureTurkishSupport(textMesh.font);
            textMesh.text = localizedText;
            textMesh.fontSize = 6;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.color = new Color(1f, 0.8f, 0.2f, 1f);
            textMesh.fontStyle = FontStyles.Bold;
            textMesh.outlineWidth = 0.2f;
            textMesh.outlineColor = Color.black;

            // Simple animation
            StartCoroutine(AnimateFloatingText(floatingText));
        }

        private IEnumerator AnimateFloatingText(GameObject go)
        {
            float duration = 2f;
            float elapsed = 0f;
            var tmp = go.GetComponent<TextMeshPro>();
            Vector3 startPos = go.transform.localPosition;

            while (elapsed < duration)
            {
                if (go == null || tmp == null) yield break;
                elapsed += Time.deltaTime;
                float alpha = 1f - (elapsed / duration);
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
                go.transform.localPosition = startPos + Vector3.up * (elapsed * 2f);

                // Always face camera
                if (Camera.main != null && go != null)
                {
                    go.transform.LookAt(go.transform.position + Camera.main.transform.rotation * Vector3.forward,
                        Camera.main.transform.rotation * Vector3.up);
                }

                yield return null;
            }
            if (go != null) Destroy(go);
        }

        // Applies a temporary speed boost to the tank
        public void PowerUpSpeed(float speedBoost, float turnSpeedBoost, float duration)
        {
            ShowFloatingText("PU_SPEED");
            StartCoroutine(IncreaseSpeed(speedBoost, turnSpeedBoost, duration));
        }

        // Coroutine to temporarily increase the tank's movement speed and turn speed
        private IEnumerator IncreaseSpeed(float speedBoost, float TurnSpeedBoost, float duration)
        {
            // Apply the speed boost
            m_HasActivePowerUp = true;
            if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.Speed, duration);
            if (m_TankMovement != null)
            {
                m_TankMovement.m_Speed += speedBoost;
                m_TankMovement.m_TurnSpeed += TurnSpeedBoost;
                m_ActiveSpeedBoost = speedBoost;
                m_ActiveTurnBoost = TurnSpeedBoost;
            }
            // Wait for the duration of the power up
            yield return new WaitForSeconds(duration);
            // Revert the speed boost
            if (m_TankMovement != null)
            {
                m_TankMovement.m_Speed -= speedBoost;
                m_TankMovement.m_TurnSpeed -= TurnSpeedBoost;
                m_ActiveSpeedBoost = m_ActiveTurnBoost = 0f;
            }
            m_HasActivePowerUp = false;
        }

        // Applies a temporary shooting rate boost to the tank
        public void PowerUpShoootingRate(float cooldownReduction, float duration)
        {
            ShowFloatingText("PU_FIRE");
            StartCoroutine(IncreaseShootingRate(cooldownReduction, duration));
        }

        // Coroutine to temporarily enhance the tank's shooting rate
        private IEnumerator IncreaseShootingRate(float cooldownReduction, float duration)
        {
            // Apply the shooting cooldown reduction if it is greater than zero
            if(cooldownReduction > 0)
            {
                m_HasActivePowerUp = true;
                if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.ShootingBonus, duration);
                if (m_TankShooting != null)
                {
                    m_TankShooting.m_ShotCooldown *= cooldownReduction;
                    m_ActiveCooldownFactor = cooldownReduction;
                }
                // Wait for the duration of the power up
                yield return new WaitForSeconds(duration);
                // Revert the shooting boost after the duration ends
                if (m_TankShooting != null)
                    m_TankShooting.m_ShotCooldown /= cooldownReduction;
                m_ActiveCooldownFactor = 1f;
                m_HasActivePowerUp = false;
            }
        }

        // Grants the tank a temporary shield if it does not already have one
        public void PickUpShield(float shieldAmount, float duration)
        {
            if (m_TankHealth != null && !m_TankHealth.m_HasShield)
            {
                ShowFloatingText("PU_SHIELD");
                StartCoroutine(ActivateShield(shieldAmount, duration));
            }
        }

        // Grants the tank a temporary shield if it does not already have one
        private IEnumerator ActivateShield(float shieldAmount, float duration)
        {
            // Activate the shield
            m_HasActivePowerUp = true;
            if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.DamageReduction, duration);
            if (m_TankHealth != null)
            {
                m_TankHealth.ToggleShield(shieldAmount);
                m_ShieldApplied = true;
            }
            // Wait for the duration of the power up
            yield return new WaitForSeconds(duration);
            // Deactivate the shield
            if (m_TankHealth != null)
                m_TankHealth.ToggleShield(shieldAmount);
            m_ShieldApplied = false;
            m_HasActivePowerUp = false;
        }

        // Increases the health of the tank
        public void PowerUpHealing(float healAmount)
        {
            ShowFloatingText("PU_HEAL");
            if (m_TankHealth != null)
                m_TankHealth.IncreaseHealth(healAmount);
            if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.Healing, 1.0f);
        }

        // Makes the tank invulnerable for an amount of time
        public void PowerUpInvincibility(float duration)
        {
            ShowFloatingText("PU_INVINCIBLE");
            StartCoroutine(ActivateInvincibility(duration));
        }

        private IEnumerator ActivateInvincibility(float duration)
        {
            m_HasActivePowerUp = true;
            if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.Invincibility, duration);
            if (m_TankHealth != null)
            {
                m_TankHealth.ToggleInvincibility();
                m_InvincibilityApplied = true;
            }
            yield return new WaitForSeconds(duration);
            m_HasActivePowerUp = false;
            if (m_TankHealth != null)
                m_TankHealth.ToggleInvincibility();
            m_InvincibilityApplied = false;
        }

        // Equips the tank with a special shell that increases damage
        public void PowerUpSpecialShell(float damageMultiplier)
        {
            ShowFloatingText("PU_DAMAGE");
            m_HasActivePowerUp = true;
            if (m_PowerUpHUD != null) m_PowerUpHUD.SetActivePowerUp(PowerUp.PowerUpType.DamageMultiplier, 0f);
            if (m_TankShooting != null)
                m_TankShooting.EquipSpecialShell(damageMultiplier);
        }
    }
}
