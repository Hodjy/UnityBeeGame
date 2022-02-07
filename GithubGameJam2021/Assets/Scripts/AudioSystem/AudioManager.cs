using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager m_Instance;
        [SerializeField] private KeySoundPair[] m_Music;
        [SerializeField] private KeySoundPair[] m_Ambience;
        [SerializeField] private KeySoundPair[] m_SFX;
        private Dictionary<string, Sound> m_MusicMap;
        private Dictionary<string, Sound> m_SFXMap;
        private Dictionary<string, Sound> m_AmbienceMap;

        [Range(0f, 0.7f)] [SerializeField] private float m_MusicVolume;
        [Range(0f, 0.7f)] [SerializeField] private float m_SFXVolume;


        void Awake()
        {
            if(m_Instance == null)
            {
                m_Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            setData();
        }

        void Start()
        {
            PlayMusic("Main");
            PlayAmbience("Main");
        }

        private void setData()
        {
            m_MusicMap = new Dictionary<string, Sound>();
            m_AmbienceMap = new Dictionary<string, Sound>();
            m_SFXMap = new Dictionary<string, Sound>();

            setSounds(m_Music, m_MusicMap);
            setSounds(m_Ambience, m_AmbienceMap);
            setSounds(m_SFX, m_SFXMap);
        }

        private void setSounds(KeySoundPair[] i_Pairs, Dictionary<string, Sound> i_Dictionary)
        {
            foreach (KeySoundPair pair in i_Pairs)
            {
                i_Dictionary.Add(pair.key, pair.value);
                setSound(pair.value);
            }
        }

        private void setSound(Sound sound)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume + m_MusicVolume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.IsLooping;
        }

        public void PlaySfx(string i_SoundName)
        {
            play(i_SoundName, m_SFXMap);
        }

        public void PlayMusic(string i_SoundName)
        {
            play(i_SoundName, m_MusicMap);
        }

        public void PlayAmbience(string i_SoundName)
        {
            play(i_SoundName, m_AmbienceMap);
        }

        private void play(string i_SoundName, Dictionary<string,Sound> i_Dictionary)
        {
            if (i_Dictionary.ContainsKey(i_SoundName))
            {
                i_Dictionary[i_SoundName].Source.Play();
            }
        }
    }



    [System.Serializable]
    public class KeySoundPair
    {
        public string key;
        public Sound value;
    }
}