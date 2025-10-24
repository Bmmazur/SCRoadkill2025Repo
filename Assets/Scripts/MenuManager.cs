using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject nextLevelMenu;
    public GameObject gameUI;
    public void CloseMenu()
    {
        gameObject.SetActive(false);
        gameUI.SetActive(true);
    }
    public void OpenGameOver()
    {
        gameOverMenu.SetActive(true);
        gameUI.SetActive(false);
    }
    public void OpenNextLevel()
    {
        nextLevelMenu.SetActive(true);
        gameUI.SetActive(false);
    }
}
