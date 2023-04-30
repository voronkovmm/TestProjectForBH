using Mirror;
using System;
using TMPro;

public class DebugText : NetworkBehaviour
{
    public static DebugText Instance { get; private set; }

    private TMP_Text _tmp;

    private void Start()
    {
        _tmp = transform.GetComponentInChildren<TMP_Text>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    [Command(requiresAuthority = false)]
    public void Print(string message, string from)
    {        
        SuperPrint(message, from);
    }

    [ClientRpc]
    private void SuperPrint(string message, string from)
    {
        _tmp.text += $"\n{from}{message}";
    }

    //DebugText.Instance.Print("hello", isClientOnly? "клиент: " : "сервер: ");
}
