
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmCharacter : MonoBehaviour
{
    public GameObject[] Characters;
    public int Number = 0;

    public static int SelectedCharacter = 0;

    void Start()
    {
        ShowCharacter();
    }

    public void ChangeCharacter(int Num)
    {
        if (Characters == null || Characters.Length == 0)
            return;

        Number += Num;

        if (Number >= Characters.Length)
            Number = 0;

        if (Number < 0)
            Number = Characters.Length - 1;

        ShowCharacter();
    }

    void ShowCharacter()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] != null)
                Characters[i].SetActive(i == Number);
        }
    }

    public void SelectAndContinue()
    {
        SelectedCharacter = Number;
        SceneManager.LoadScene("GameScene");
    }
}
