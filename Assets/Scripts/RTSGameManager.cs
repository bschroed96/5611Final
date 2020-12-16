using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RTSGameManager : MonoBehaviour
{
    public static void UnitTakeDamage(UnitController attackingController, UnitController attackedController)
    {
        var damage = attackingController.attack;

        attackedController.TakeDamage(attackingController, damage);
    }
    
    public static void UnitGetPowerUp(UnitController myUnit, GameObject powerup)
    {
        myUnit.IncreaseDamage(powerup, 5);
    }

    public static void EndGame(GameObject endui) {
        Debug.Log("EndGame");
        //GameObject endui = GameObject.FindGameObjectWithTag("EndGame");
        endui.SetActive(true);
        GameObject man = GameObject.Find("RTSManager");
        RTSGameManager m = man.GetComponent<RTSGameManager>();
        m.wait();
    }

     IEnumerator wait(){
      yield return new WaitForSeconds(3f);
      //my code here after 3 seconds
      SceneManager.LoadScene("SampleScene");
 }

    public static void ResetGame() {
        SceneManager.LoadScene("SampleScene");
    }
}
