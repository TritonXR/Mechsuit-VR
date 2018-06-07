using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {
  public GameObject enemyShieldBar, enemyHealthBar;
  public GameObject selfShieldFill, selfHealthFill, enemyShieldFill, enemyHealthFill;
  public UnityEngine.UI.Text enemyName;

  public void UpdateHealth(IHealth health, bool isEnemy, string name) {
    if (!isEnemy) { // update self's health/shield bar on the left
      // The health itself is always ShieldedHealth
      ShieldedHealth shieldedHealth = health as ShieldedHealth;
      selfShieldFill.transform.localPosition = new Vector3(-100 + 100 * (shieldedHealth.CurrShield / shieldedHealth.maxShield), selfShieldFill.transform.localPosition.y, selfShieldFill.transform.localPosition.z);
      selfHealthFill.transform.localPosition = new Vector3(-100 + 100 * (shieldedHealth.CurrHealth / shieldedHealth.maxHealth), selfHealthFill.transform.localPosition.y, selfHealthFill.transform.localPosition.z);
    } else {
      enemyName.gameObject.SetActive(true);
      enemyHealthBar.SetActive(true);

      enemyName.text = name;
      enemyHealthFill.transform.localPosition = new Vector3(-100 + 100 * (health.CurrHealth / health.MaxHealth), enemyHealthFill.transform.localPosition.y, enemyHealthFill.transform.localPosition.z);
      if (health is ShieldedHealth) {
        ShieldedHealth shieldedHealth = health as ShieldedHealth;
        enemyShieldBar.SetActive(true);
        enemyShieldFill.transform.localPosition = new Vector3(-100 + 100 * (shieldedHealth.CurrShield / shieldedHealth.maxShield), enemyShieldFill.transform.localPosition.y, enemyShieldFill.transform.localPosition.z);
      } else {
        enemyShieldBar.SetActive(false);
      }

      if (health.CurrHealth <= 0) {
        StartCoroutine(RemoveEnemyBar());
      }
    }
  }

  IEnumerator RemoveEnemyBar() {
    yield return new WaitForSeconds(5);
    enemyName.gameObject.SetActive(false);
    enemyHealthBar.SetActive(false);
    enemyShieldBar.SetActive(false);
  }
}
