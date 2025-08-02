using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScenes";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScene fadeScreen;
    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)  //û����������޷����������ť
        {
            continueButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        StartCoroutine(loadSceneWithFadeEffect(1.5f));
    }
    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();//ɾ�����������
        StartCoroutine(loadSceneWithFadeEffect(1.5f));

    }
    public void ExitGame()
    {
        Debug.Log("�˳���Ϸ");
        Application.Quit();
    }
    IEnumerator loadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
