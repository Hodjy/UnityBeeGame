using AudioSystem;
using UnityEngine;

namespace ColorSystem
{
    public class PollenColorReactor : MonoBehaviour, IColorReactor
    {
        private const string rm_PollenHitSFXName = "PollenHit";

        private void Start()
        {
            ColorSpreader colorSpreader = GetComponent<ColorSpreader>();
            if(colorSpreader != null)
            {
                colorSpreader.SpreadToMain(GetComponent<ColorHolder>().CurrentColorType);
            }
        }

        public void React(GameObject i_CollidedObject, ColorHolder i_CollidedObjColor, ColorHolder i_SelfColor)
        {
            FindObjectOfType<AudioManager>().PlaySfx(rm_PollenHitSFXName);
            i_CollidedObjColor.setColor(i_SelfColor.CurrentColorType);
            knockbackPlayer(i_CollidedObject);
        }

        private void knockbackPlayer(GameObject i_Player)
        {
            Vector3 knockbackDirection = i_Player.transform.position - transform.position;
            i_Player.GetComponent<PlayerController>().Knockbacked(knockbackDirection);
        }
    }
}

