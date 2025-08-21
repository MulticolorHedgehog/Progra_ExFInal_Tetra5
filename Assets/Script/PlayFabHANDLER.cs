using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using PlayFab.Internal;

public class PlayFabHANDLER : MonoBehaviour
{
    private string titleID = "1B1D51"; //El titulo de playfab al que esta vinculado el proyecto
    private string devKey = "ZKMWNUQZZOEKS56JT9XJDBF7PMCCZFWJ7J7MBKD4OGKO51BQUR";


    [Header("Register UI Elements")]
    [SerializeField] private TMP_InputField register_usernameInputField;
    [SerializeField] private TMP_InputField register_emailInpputFIeld;
    [SerializeField] private TMP_InputField register_PasswordInputField;
    [SerializeField] private TMP_InputField register_ConfirmPasswordInputField;
    [SerializeField] private TMP_InputField register_displaynameInputField;
    [SerializeField] private UnityEvent onRegisterSuccess;

    [Header("Login UI Elements")]
    [SerializeField] private TMP_InputField login_PasswordInputField;
    [SerializeField] private TMP_InputField login_UsernameInputField;
    [SerializeField] private UnityEvent onLoginSuccess;

    [SerializeField] private Image userAvatarImage;
    [SerializeField] private TMP_Text userDisplayNameText;
    [SerializeField] private string userDisplayName;
    [SerializeField] private string userAvatarUrl;
    

    public GameObject userScoreAreaPrefab;

    public Transform leaderboardParent;


    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(titleID))
        {
            PlayFabSettings.TitleId = titleID;
        }
        if(string.IsNullOrEmpty(devKey))
        {
            PlayFabSettings.DeveloperSecretKey = devKey;
            //Por alguna extraña razon esta linea de codigo si la dejo activa provoca este error
            //'PlayFabSettings' does not contain a definition for 'DeveloperSecretKey'

        }


    }

    /// <summary>
    /// Username / DisplayName / Email / Password
    /// 
    /// Username: Nombre de usuario, debe ser unico y no puede contener espacios o caracteres especiales
    /// DisplayName: Nombre que se mostrara en el juego, no es necesario que sea unico
    /// Email: Correo electronico del usuario, debe ser valido
    /// Password: Contraseña del usuario, debe tener al menos 8 caracteres
    /// 
    /// 
    /// </summary>
    public void CreatePlayfabAccount()
    {
        
        if(register_PasswordInputField.text != register_ConfirmPasswordInputField.text)
        {
            Debug.Log("Error");
            return;
        }
        
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            DisplayName = register_usernameInputField.text,
            Username = register_usernameInputField.text,
            Email = register_emailInpputFIeld.text,
            Password = register_PasswordInputField.text,
            RequireBothUsernameAndEmail = true
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, ErrorMessage);
    }

    [ContextMenu("Get Player Profile")]
    public void GetPlayerProfile()
    {
        GetPlayerProfileRequest request = new GetPlayerProfileRequest // La solicitud es decir que datos quieres conseguir
        {
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true, // Mostrar el nombre de usuario             
                ShowAvatarUrl = true // Mostrar la URL del avatar del jugador
            },
        };

        PlayFabClientAPI.GetPlayerProfile(request, OnGetPlayerProfileSuccess, ErrorMessage); // Aqui se ejecuta la solicitud
     }

    private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        userDisplayName = result.PlayerProfile.DisplayName;
        userAvatarUrl = result.PlayerProfile.AvatarUrl;

        userDisplayNameText.text = userDisplayName;
        StartCoroutine(RetrievePlayerAvatar()); // Inicia la corrutina para descargar la imagen del avatar del jugador

    }

    private Sprite userAvatarSprite; // Variable para guardar la imagen del avatar del jugador

    IEnumerator RetrievePlayerAvatar()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(userAvatarUrl); // Estoy creando una solicitud a la web, esta solicitud es especificamente para conseguir una imagen

        yield return request.SendWebRequest(); // Esta linea envia la solicitud a la web y espera a que se complete

        if (request.result != UnityWebRequest.Result.Success) // Si la solicitud no se pudo hacer
        {
            Debug.Log(request.error);
            StopAllCoroutines(); // Esta linea detiene todas las corrutinas si la solicitud fallo
        }
        else // Si la solicitud fue exitosa, ya conseguir la textura, es decir la imagen
        {
            // Tengo un componente que maneja la descarga de texturas
            DownloadHandlerTexture downloadHandler = request.downloadHandler as DownloadHandlerTexture; // Esta linea me guarda la imagen que consegui en una variable
            Sprite playerImage = Sprite.Create(downloadHandler.texture, new Rect(0.0f, 0.0f, downloadHandler.texture.width, downloadHandler.texture.height), Vector2.zero);
            userAvatarImage.sprite = playerImage; // Asigno la imagen que consegui al componente de la UI
        }
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Usuario registrado");
    }

    public void LogInUser()
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest
        {
            Username = login_UsernameInputField.text,
            Password = login_PasswordInputField.text,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, ErrorMessage);
    }

    [ContextMenu("Update Score")]
    private void UpdateScore(int score)
    {
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }

        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateStatisticsSuccess, ErrorMessage);

    }

    [ContextMenu("Get Leaderboard")]
    public void GetLeaderboard()
    {
        GetLeaderboardRequest request = new GetLeaderboardRequest
        {
            StatisticName = "High Score",
            StartPosition = 0,
            MaxResultsCount = 15,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderBoardSuccess, ErrorMessage);
    }

    private void OnGetLeaderBoardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Leaderboard retrieved successfully");
        foreach (PlayerLeaderboardEntry user in result.Leaderboard)
        {
            Debug.Log("$Player: {user.DisplayName}, Score: {user: StatValue}");
        }
    }


    public void UpdateLeaderBoard(string leaderboard, int value)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboard,
                    Value = value
                }
            }
        }, OnUpdateLeaderboardSuccess, ErrorMessage);
    }

    private void OnUpdateLeaderboardSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard updated successfully");
    }

    public void DisplayLeaderboard(string leaderboard)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = leaderboard,
            StartPosition = 0,
            MaxResultsCount = 100
        }, OnDisplayLeaderboardSuccess, ErrorMessage);
    }

    private void OnDisplayLeaderboardSuccess(GetLeaderboardResult result)
    {
        foreach (Transform item in leaderboardParent)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
        foreach (PlayerLeaderboardEntry item2 in result.Leaderboard.OrderByDescending((PlayerLeaderboardEntry item) => item.StatValue).ToList())
        {
            GameObject obj = UnityEngine.Object.Instantiate(userScoreAreaPrefab, leaderboardParent);
            TextMeshProUGUI component = obj.transform.Find("Username").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI component2 = obj.transform.Find("Score").GetComponent<TextMeshProUGUI>();
            Image component3 = obj.transform.Find("Image").GetComponent<Image>();
            if (component != null)
            {
                component.text = item2.DisplayName;
            }
            if (component2 != null)
            {
                component2.text = item2.StatValue.ToString();
            }
            
        }
    }


    private void OnUpdateStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Estadisticas actualizadas de forma correcta");
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logeado");
        onLoginSuccess.Invoke();
    }

    private void ErrorMessage(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    
}
