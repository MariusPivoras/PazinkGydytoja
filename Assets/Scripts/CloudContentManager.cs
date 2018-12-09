using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloudContentManager : MonoBehaviour
{
    #region PRIVATE_CONSTANTS
    const string JSON_URL = "https://piworks.lt/api/json/";
    // JSON File Key Strings
    const string DOC1_FOTO_KEY = "doc1_foto";
    const string DOC1_NAME_KEY = "doc1_name";
    const string DOC1_KAVL_KEY = "doc1_kvalifikacija";
    const string DOC1_PAT_KEY = "doc1_patirtis";
    const string DOC1_APD_KEY = "doc1_apdovanojimai";
    const string DOC1_HOBIS_KEY = "doc1_hobis";
    const string DOC1_NARYSTE_KEY = "doc1_naryste";
    const string DOC2_FOTO_KEY = "doc2_foto";
    const string DOC2_NAME_KEY = "doc2_name";
    const string DOC2_KAVL_KEY = "doc2_kvalifikacija";
    const string DOC2_PAT_KEY = "doc2_patirtis";
    const string DOC2_APD_KEY = "doc2_apdovanojimai";
    const string DOC2_HOBIS_KEY = "doc2_hobis";
    const string DOC2_NARYSTE_KEY = "doc2_naryste";

    #endregion // PRIVATE_CONSTANTS

    #region PROPERTIES
    string Doc1_foto { get; set; }
    string Doc1_name { get; set; }
    string Doc1_kval { get; set; }
    string Doc1_pat { get; set; }
    string Doc1_apd { get; set; }
    string Doc1_hobis { get; set; }
    string Doc1_naryste { get; set; }
    string Doc2_foto { get; set; }
    string Doc2_name { get; set; }
    string Doc2_kval { get; set; }
    string Doc2_pat { get; set; }
    string Doc2_apd { get; set; }
    string Doc2_hobis { get; set; }
    string Doc2_naryste { get; set; }
    #endregion // PROPERTIES


    #region PUBLIC_MEMBERS
    public Image m_LoadingIndicator1;
    public Image m_Cover1;
    public Text m_Doc1_name;
    public Text m_Doc1_kval;
    public Text m_Doc1_pat;
    public Text m_Doc1_apd;
    public Text m_Doc1_hobis;
    public Text m_Doc1_naryste;

    // public Image m_LoadingIndicator2;
    public Image m_LoadingIndicator2;
    public Image m_Cover2;
    public Text m_Doc2_name;
    public Text m_Doc2_kval;
    public Text m_Doc2_pat;
    public Text m_Doc2_apd;
    public Text m_Doc2_hobis;
    public Text m_Doc2_naryste;
    #endregion PUBLIC_MEMBERS


    #region PRIVATE_MEMBERS
   
    bool wwwRequestInProgress;
    #endregion PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        if (wwwRequestInProgress && m_LoadingIndicator1 && m_LoadingIndicator2)
        {
            m_LoadingIndicator1.rectTransform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
            m_LoadingIndicator2.rectTransform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void HandleMetadata(string metadata)
    {
        // metadata string will be in the following format: samplebook[1-3].json
        // concatenate the metadata string filename to the base JSON URL:
        // https://developer.vuforia.com/samples/cloudreco/json/samplebook[#].json
        string fullURL = JSON_URL + metadata;

        StartCoroutine(WebRequest(fullURL));
    }

    public void ClearDocData()
    {
        m_Cover1.sprite = null;
        m_Cover1.color = Color.black;
        m_Doc1_name.text = "none";
        m_Doc1_kval.text = "none";
        m_Doc1_pat.text = "none";
        m_Doc1_apd.text = "none";
        m_Doc1_hobis.text = "none";
        m_Doc1_naryste.text = "none";
        m_Cover2.sprite = null;
        m_Cover2.color = Color.black;
        m_Doc2_name.text = "none";
        m_Doc2_kval.text = "none";
        m_Doc2_pat.text = "none";
        m_Doc2_apd.text = "none";
        m_Doc2_hobis.text = "none";
        m_Doc2_naryste.text = "none";
    }
    #endregion // PUBLIC_METHODS


    #region BUTTON_METHODS
    /*  public void MoreInfoButton()
      {
          Application.OpenURL(BrowserURL);
      }*/
    #endregion // BUTTON_METHODS


    #region PRIVATE_METHODS
    void UpdateDocData()
    {
        Debug.Log("UpdateDocData() called.");


        m_Doc1_name.text = Doc1_name;
        m_Doc1_kval.text = Doc1_kval;
        m_Doc1_pat.text = Doc1_pat;
        m_Doc1_apd.text = Doc1_apd;
        m_Doc1_hobis.text = Doc1_hobis;
        m_Doc1_naryste.text = Doc1_naryste;

        m_Doc2_name.text = Doc2_name;
        m_Doc2_kval.text = Doc2_kval;
        m_Doc2_pat.text = Doc2_pat;
        m_Doc2_apd.text = Doc2_apd;
        m_Doc2_hobis.text = Doc2_hobis;
        m_Doc2_naryste.text = Doc2_naryste;





    }

    void ProcessWebRequest(WWW www, WWW www1, WWW www2)
    {
        Debug.Log("ProcessWebRequest() called: \n" + www.url);

        if (www.url.Contains(".json"))
        {
            ParseJSON(www.text);
            www.Dispose();
            UpdateDocData();

            StartCoroutine(WebRequest(Doc1_foto));
            StartCoroutine(WebRequest(Doc2_foto));
        }
        else if (www.url.Contains(".jpg") && www.texture != null)
        {
            if (m_Cover1)
            {
                m_Cover1.sprite = Sprite.Create(www1.texture,
                                               new Rect(0, 0, www1.texture.width, www1.texture.height),
                                               new Vector2(0.5f, 0.5f));
                m_Cover1.color = Color.white;

               m_Cover2.sprite = Sprite.Create(www2.texture,
                                            new Rect(0, 0, www2.texture.width, www2.texture.height),
                                            new Vector2(0.5f, 0.5f));
                m_Cover2.color = Color.white;
             
                www.Dispose();
            }

        }
    }

    IEnumerator WebRequest(string url)
    {
        Debug.Log("WebRequest() called: \n" + url);

        wwwRequestInProgress = true;
        m_LoadingIndicator1.enabled = true;
        m_LoadingIndicator2.enabled = true;

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("WebRequest() failed. Your URL is null or empty.");
            yield return null;
        }

        WWW www = new WWW(url);
        WWW www1 = new WWW(Doc1_foto);
        WWW www2 = new WWW(Doc2_foto);

        yield return www;
        yield return www1;
        yield return www2;


        if (www.isDone)
        {
            Debug.Log("Done Loading: \n" + www.url);
            wwwRequestInProgress = false;
            m_LoadingIndicator1.enabled = false;
            m_LoadingIndicator2.enabled = false;
        }

        if (string.IsNullOrEmpty(www.error))
        {
            // If error string is null or empty, then request was successful
            ProcessWebRequest(www, www1, www2);
        }
        else
        {
            Debug.LogError("Error With WWW Request: " + www.error);

            string error = "<color=red>" + www.error + "</color>" + "\nURL Requested: " + www.url;

            MessageBox.DisplayMessageBox("WWW Request Error", error, true, null);
        }
    }

    /// <summary>
    /// Parses a JSON string and returns a book data struct from that
    /// </summary>
    void ParseJSON(string jsonText)
    {
        Debug.Log("ParseJSON() called: \n" + jsonText);

        // Remove opening and closing braces and any spaces
        char[] trimChars = { '{', '}', ' ' };

        // Remove double quote and new line chars from the JSON text
        jsonText = jsonText.Trim(trimChars).Replace("\"", "").Replace("\n", "");

        string[] jsonPairs = jsonText.Split(',');

        Debug.Log("# of JSON pairs: " + jsonPairs.Length);

        foreach (string pair in jsonPairs)
        {
            // Split pair into a max of two strings using first colon
            string[] keyValuePair = pair.Split(new char[] { ':' }, 2);

            if (keyValuePair.Length == 2)
            {
                switch (keyValuePair[0])
                {
                    case DOC1_FOTO_KEY:
                        Doc1_foto = keyValuePair[1];
                        break;
                    case DOC1_NAME_KEY:
                        Doc1_name = keyValuePair[1];
                        break;
                    case DOC1_KAVL_KEY:
                        Doc1_kval = keyValuePair[1];
                        break;
                    case DOC1_PAT_KEY:
                        Doc1_pat = keyValuePair[1];
                        break;
                    case DOC1_APD_KEY:
                        Doc1_apd = keyValuePair[1];
                        break;
                    case DOC1_HOBIS_KEY:
                        Doc1_hobis = keyValuePair[1];
                        break;
                    case DOC1_NARYSTE_KEY:
                        Doc1_naryste = keyValuePair[1];
                        break;

                    case DOC2_FOTO_KEY:
                        Doc2_foto = keyValuePair[1];
                        break;
                    case DOC2_NAME_KEY:
                        Doc2_name = keyValuePair[1];
                        break;
                    case DOC2_KAVL_KEY:
                        Doc2_kval = keyValuePair[1];
                        break;
                    case DOC2_PAT_KEY:
                        Doc2_pat = keyValuePair[1];
                        break;
                    case DOC2_APD_KEY:
                        Doc2_apd = keyValuePair[1];
                        break;
                    case DOC2_HOBIS_KEY:
                        Doc2_hobis = keyValuePair[1];
                        break;
                    case DOC2_NARYSTE_KEY:
                        Doc2_naryste = keyValuePair[1];
                        break;
                }
            }
        }
    }

    #endregion //PRIVATE_METHODS
}
