using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip dayBaseAudio;
    public AudioClip[] dayMotivesAudio;
    public AudioClip nightBaseAudio;
    public AudioClip[] nightMotivesAudio;
    public AudioClip[] windAudio;
    public AudioClip[] rainAudio;

    AudioSource[] dayNight;
    AudioSource[] rain;
    AudioSource[] wind;

    const float BASE_DURATION = 3.42857f;
    float currentTimeToNextBar = 0;

    int onlyRainPhase = 0;

    void Start()
    {
        dayNight = transform.Find("day_night").GetComponents<AudioSource>();
        rain = transform.Find("rain").GetComponents<AudioSource>();
        wind = transform.Find("wind").GetComponents<AudioSource>();
    }

    void Update()
    {
        currentTimeToNextBar -= Time.deltaTime;
        if (currentTimeToNextBar <= 0)
        {
            currentTimeToNextBar = BASE_DURATION;

            //day-night base
            bool isDay = SeedStateManager.Instance.isDay;
            if (isDay) dayNight[0].PlayOneShot(dayBaseAudio);
            else dayNight[0].PlayOneShot(nightBaseAudio);

            //motives
            bool isRaining = (WaterManager.Instance.rainFactor > 0);
            bool isWinding = (WindManager.Instance.airFactor > 0);

            if (!isRaining || isWinding) onlyRainPhase = 0;

            if (isRaining && isWinding)
            {
                wind[0].PlayOneShot(windAudio[Random.Range(0, windAudio.Length)]);
                rain[0].PlayOneShot(rainAudio[0]);
            }
            else if (isRaining)
            {
                rain[0].PlayOneShot(rainAudio[0]);

                if (onlyRainPhase > 0)
                {
                    rain[1].PlayOneShot(rainAudio[1]);
                }
                if (onlyRainPhase > 1)
                {
                    int rand = Random.Range(0, 3);

                    switch (rand)
                    {
                        case 0: //only medium
                            rain[2].PlayOneShot(rainAudio[Random.Range(2, 4)]);
                            break;
                        case 1: //only high
                            rain[3].PlayOneShot(rainAudio[4]);
                            break;
                        default: //medium and high
                            rain[2].PlayOneShot(rainAudio[3]);
                            rain[3].PlayOneShot(rainAudio[4]);
                            break;
                    }
                }
                ++onlyRainPhase;
            }
            else if (isWinding)
            {
                int rand = Random.Range(0, 3);
                if (rand > 0) wind[0].PlayOneShot(windAudio[0]);
                if (rand < 2) wind[1].PlayOneShot(windAudio[1]);
            }
            else if (!dayNight[1].isPlaying)
            {
                AudioClip[] motives = (isDay) ? dayMotivesAudio : nightMotivesAudio;
                int rand = Random.Range(0, motives.Length);
                if (rand < motives.Length)
                {
                    dayNight[1].PlayOneShot(motives[rand]);
                }
            }
        }
    }
}
