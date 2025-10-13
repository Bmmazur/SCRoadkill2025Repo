using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject nextLevelMenu;
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    public void OpenGameOver()
    {
        gameOverMenu.SetActive(true);
    }
    public void OpenNextLevel()
    {
        nextLevelMenu.SetActive(true);
    }
}
