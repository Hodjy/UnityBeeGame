using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public class PlayerColorHolder : ColorHolder
    {
        [SerializeField] private ParticleSystem m_ColorChangedParticle;
        private bool m_FirstTime = true;


        public override void setColor(eColors i_ColorType)
        {
            base.setColor(i_ColorType);

            if (m_FirstTime)
            {
                m_FirstTime = false;
                m_ColorChangedParticle.transform.localScale *= 0.1f;
                return;
            }

            playParticleByColor(i_ColorType);
        }

        private void playParticleByColor(eColors i_ColorType)
        {
            ParticleSystem.MainModule a = m_ColorChangedParticle.main;
            a.startColor = Colors.GetInstance().getColor(i_ColorType);
            m_ColorChangedParticle.Play();
        }
    }
}

