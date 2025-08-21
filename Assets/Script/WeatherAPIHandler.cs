using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using SimpleJSON; //Esta libreria la necesitamos para poder leer el formato json que nos manda la API
using static System.Net.WebRequestMethods;
using UnityEngine.Networking;
using System;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;




public class WeatherAPIHandler : MonoBehaviour
{
    public List<Coodernadas> Cordenadas;
    [SerializeField] WeatherData weatherData;
    

    [SerializeField] public string latitude = "-38.416097"; //Replace with your latitude;
    [SerializeField] public string longitude = "-63.616672";//Replace with your longitude;
    [SerializeField] public string url;
    
    
    [SerializeField] private Light2D directonalLight;
    [SerializeField] private float lightColorTransitionSpeed;

    public int cityIndex;

    public int lastIndex = -1;

    private string jsonRaw;




    private void Awake()
    {
        
        
    }

    private void OnValidate()
    {
        url = $"https://api.openweathermap.org/data/3.0/onecall?lat={latitude}&lon={longitude}&appid=7fe45acb4f5a69f83c45312aad97613a&units=metric";
    }

    

    private void Start()
    {
        
        StartCoroutine(WeatherUpdate()); //Esta linea inicia la corrutina que se encargara de hacer la solicitud a la web
        StartCoroutine(SelectCity(50.5f));
    }

    IEnumerator WeatherUpdate()
    {
        UnityWebRequest request = new UnityWebRequest(url); //Esta linea nos guarda la solicitud que queremos realizar a la web
        request.downloadHandler = new DownloadHandlerBuffer(); //Esta linea nos dice que queremos descargar el contenido de la web en un buffer

        yield return request.SendWebRequest(); //Esta linea envia la solicitud a la web y espera a que se complete

        if(request.result != UnityWebRequest.Result.Success) // Si la solicitud no se pudo hacer
        {
            Debug.Log(request.error);
            StopCoroutine(WeatherUpdate()); //Esta linea detiene el WeahterUpdate
        }
        else
        {
            Debug.Log("Weather Data recibido ");
            jsonRaw = request.downloadHandler.text; //Esta linea guarada el contenido de la web en una variante
            Debug.Log(url);
            Debug.Log(jsonRaw); //Esta linea imprime el contenido de la web en la consola
            DecodeJson();
        }
    }

    private IEnumerator RetrieveWeatherData()
    {
        UnityWebRequest request = new UnityWebRequest(url)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            yield break;
        }
        Debug.Log(request.downloadHandler.text);
        jsonRaw = request.downloadHandler.text;
        DecodeJson();
        UpdateDirectionalLight();
        
        
        DynamicGI.UpdateEnvironment();
        
       
    }

    private void UpdateDirectionalLight()
    {
        Color color;
        switch (weatherData.actualTemp)
        {
            case float temp when weatherData.actualTemp >= 50:
                {
                    
                    color = new Color(55, 92, 100, 100);
                    break;
                }
            case float temp when weatherData.actualTemp >= 30:
                {
                    
                    color = new Color(121, 53, 100, 100);
                    break;
                }
            case float temp when weatherData.actualTemp <= 5:
                {
                    
                    color = new Color(179, 16, 100, 100);
                    break;
                }
            default:
                {
                    
                    color = directonalLight.color;
                    break;
                }
           

        }
        StartCoroutine(ChangeColor(color));
    }

    private void DecodeJson()
    {
        //La variable JSONNode es una clase que nos permite leer el formato json que nos manda la API
        JSONNode json = JSON.Parse(jsonRaw); //JSON.Parse me transforma el string jsonRaw en un JOSN

        string timezone = json["timezone"];
        weatherData.continent = timezone.Split('/')[0];
        weatherData.city = timezone.Split("/")[1];
        weatherData.actualTemp = json["current"]["temp"];
        weatherData.description = json["current"]["weather"][0]["description"];
        weatherData.windSpeed = json["current"]["wind_speed"];
        
        Debug.LogWarning("TIMEZONE: " + timezone);

        UpdateDirectionalLight();

    }

    IEnumerator ChangeColor(Color colorToTransition)
    {
        Debug.Log("Iniciando transicion de color");
        yield return new WaitUntil(() => ActualDirectionalLightColor(colorToTransition) == colorToTransition);
        // La condicion que quiero que se cumpla, es que el color de la luz sea el elegido, es decir que termine de transicionar
        Debug.Log("Se hizo");
    }

    float t = 0;
    private Color ActualDirectionalLightColor(Color colorToTransition)
    {
        t += Time.deltaTime * lightColorTransitionSpeed;
        directonalLight.color = Color.Lerp(directonalLight.color, colorToTransition, t);
        return directonalLight.color;
    }

    private IEnumerator SelectCity(float duration)
    {
        while (true)
        {
            int num = UnityEngine.Random.Range(0, Cordenadas.Count);
            if (num != lastIndex)
            {
                cityIndex = num;
                lastIndex = cityIndex;
                float latitud = Cordenadas[cityIndex].latitud;
                float longitud = Cordenadas[cityIndex].longitud;
                url = $"https://api.openweathermap.org/data/3.0/onecall?lat={latitud}&lon={longitud}&appid=7fe45acb4f5a69f83c45312aad97613a&units=metric";
                StartCoroutine(RetrieveWeatherData());
                yield return new WaitForSecondsRealtime(90f);
            }
        }
    }

    

    

}

    



[Serializable]
public struct WeatherData
{
    public string continent;
    public string city;
    public float actualTemp;
    public string description;
    public float windSpeed;
}

[Serializable]
public struct Coodernadas
{
    
    public float latitud;
    public float longitud;
    


}


