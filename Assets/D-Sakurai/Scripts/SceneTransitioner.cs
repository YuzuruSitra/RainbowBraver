using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace D_Sakurai.Scripts
{
    public static class SceneTransitioner
    {
        public static void TransitionToCombat(int id, UnitAlly[] allies)
        {
            SceneManager.sceneLoaded += (next, mode) =>
            {
                
            };

            SceneManager.LoadScene("CombatSystem");
        }
    }
}