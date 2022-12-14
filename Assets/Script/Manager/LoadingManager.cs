using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("[Loading UI]")]
    [SerializeField] public Image FadeInPannel;
    [SerializeField] public Image FadeOutPannel;
    [SerializeField] public Image GameLogo;
    [SerializeField] public TextMeshProUGUI GameLogoText;
    [SerializeField] public TextMeshProUGUI GameLogoText_change;

    float fadeNum = 0f;
    float ColorNum = 0f;

    float time = 0;
    float Size = 8;
    float SizeTime = 0.1f;
    void Start()
    {
        StartCoroutine("LoadingFadeInPannel");

    }

    //
    // Coroutine
    #region Coroutine
    public enum STATE
    {
        FadeInPannel, FadeOutPannel, FadeLogoImage, FadeLogoText
    }

    Coroutine curCoroutine = null;
    STATE curState = STATE.FadeInPannel;

    void nextState(STATE newState)
    {
        //받아온 STATE를 현재 STATE로 
        if (newState == curState)
            return;

        //현재 코루틴이 있을때 현재 코루틴 중지
        if (curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        // 새로운 STATE를 현재 STATE에 할당받기 
        curState = newState;
        curCoroutine = StartCoroutine("Loading" + newState.ToString());
    }
    #endregion

    bool IsNext = false;

    //
    // FadeInPannel
    #region FadeInPannel
    IEnumerator LoadingFadeInPannel()
    {
        while (true)
        {
            if (IsNext == false)
            {
                fadeNum += 0.01f;
                yield return new WaitForSeconds(0.01f);
                FadeInPannel.color = new Color(0, 0, 0, fadeNum);

                if (fadeNum >= 1.0f)
                {
                    IsNext = true;
                    if (IsNext == true)
                    {
                        IsNext = false;
                        yield return new WaitForSeconds(1f);
                        nextState(STATE.FadeLogoImage);
                        break;
                    }

                }
            }
        }

    }
    #endregion


    // FadeOutPannel
    #region FadeOutPannel
    /*IEnumerator LoadingFadeOutPannel()
    {
        while (true)
        {
            if (IsNext == false)
            {
                fadeNum += 0.01f;
                yield return new WaitForSeconds(0.01f);
                FadeOutPannel.color = new Color(0, 0, 0, fadeNum);

                if (fadeNum >= 1.0f)
                {
                    IsNext = true;
                    if (IsNext == true)
                    {
                        yield return null;
                        break;
                    }

                }
            }
        }

    }*/
    #endregion


    int count = 0;

    //
    // FadeLogoImage
    #region FadeLogoImage
    IEnumerator LoadingFadeLogoImage()
    {
        GameLogo.color = new Color(255, 255, 255, 1);
        while (time <= 1f)
        {
            if (time <= SizeTime)
            {
                GameLogo.gameObject.transform.localScale = Vector3.one * (1.2f * Size * time);
                GameLogo.color = new Color(255, 255, 255, 1);
            }
            else if (time <= SizeTime * 1.5f)
            {
                GameLogo.gameObject.transform.localScale = Vector3.one * (1.5f * Size * SizeTime + 1 - time * Size);
                GameLogo.color = new Color(255, 255, 255, 1);
            }
            else
            {
                GameLogo.gameObject.transform.localScale = Vector3.one;
                GameLogo.color = new Color(255, 255, 255, 1);
                time = 0f;
                count++;
                if (count == 1)
                {
                    yield return new WaitForSeconds(0.1f);
                    nextState(STATE.FadeLogoText);
                    break;
                }
            }
            time += Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }
    }
    #endregion


    //
    // FadeLogoText
    #region FadeLogoText
    IEnumerator LoadingFadeLogoText()
    {
        IsNext = false;
        
        while (true)
        {
            if (IsNext == false)
            {
                fadeNum += 0.01f;
                yield return new WaitForSeconds(0.01f);
                GameLogoText.color = new Color(1, 1, 1, fadeNum);

                if (fadeNum >= 1.0f)
                {
                    yield return new WaitForSeconds(1f);
                    break;
                }
            }
        }        
    }
    #endregion
}
