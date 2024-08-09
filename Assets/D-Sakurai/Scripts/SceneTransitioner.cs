using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine.SceneManagement;

namespace D_Sakurai.Scripts
{
    public class SceneTransitioner
    {
        public static void Transition(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}