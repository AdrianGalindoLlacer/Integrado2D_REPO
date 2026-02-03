using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Inicio del singleton basico
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance = null)
            {
                Debug.Log("Game Manager is null");
            }

            return _instance;
        }
    }
    //Fin del singleton basico

    //Variables


    private void Awake()
    {
        _instance = this;
    }
}
