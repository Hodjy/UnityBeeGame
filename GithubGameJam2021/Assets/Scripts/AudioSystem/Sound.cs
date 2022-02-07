using UnityEngine.Audio;
using UnityEngine;

namespace AudioSystem
{
    [System.Serializable]
    public class Sound 
    {

        [SerializeField] private AudioClip m_AudioClip;
        [Range(0f, 0.3f)] [SerializeField] private float m_Volume;
        [Range(.1f, 3f)] [SerializeField] private float m_Pitch;
        [SerializeField] private bool m_IsLooping;
        private AudioSource m_Source;

        public float Volume { get => m_Volume; set => m_Volume = value; }
        public float Pitch { get => m_Pitch; set => m_Pitch = value; }
        public bool IsLooping { get => m_IsLooping; set => m_IsLooping = value; }
        public AudioClip Clip { get => m_AudioClip; set => m_AudioClip = value; }
        public AudioSource Source { get => m_Source;  set => m_Source = value;  }

    }
}

