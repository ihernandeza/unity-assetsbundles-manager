using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;


public class LoadAssets : MonoBehaviour
{
	public const string AssetBundlesOutputPath = "/AssetBundles/";
	public string assetBundleName;
	public string assetName;
    public string[] assetNames;

    /// <summary>
    /// Aqui voy a guardar los bunldes descargados para su posterior referencia
    /// </summary>
    private List<GameObject> bundlesLoaded = new List<GameObject>();

    // Use this for initialization
    IEnumerator Start ()
	{
		yield return StartCoroutine(Initialize() );

        // Load asset.
        if (assetName != "")
        {
            yield return StartCoroutine(InstantiateGameObjectAsync(assetBundleName, assetName));
        }

        // Load multiple Assets if Exist
        foreach (string asset in assetNames)
        {
            yield return StartCoroutine(InstantiateGameObjectAsync(assetBundleName, asset));
        }

        //Ya se cargaron Todos, enviar GamObjects al Manager y activar UI
        //@AnimalAnimatorManager
        SendMessage("Init", bundlesLoaded, SendMessageOptions.DontRequireReceiver);
	}

	// Initialize the downloading url and AssetBundleManifest object.
	protected IEnumerator Initialize()
	{
		// Don't destroy this gameObject as we depend on it to run the loading script.
		DontDestroyOnLoad(gameObject);

        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project. 
        // 	Another approach would be to make this configurable in the standalone player.)
        /*
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
		AssetBundleManager.SetDevelopmentAssetBundleServer ();
		#else
		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
		AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
		// Or customize the URL based on your deployment or configuration
		//AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
		#endif
        */

        AssetBundleManager.SetSourceAssetBundleURL(Constants.urlEndpointAB);

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();
		if (request != null)
			yield return StartCoroutine(request);
	}

	protected IEnumerator InstantiateGameObjectAsync (string assetBundleName, string assetName)
	{
		// This is simply to get the elapsed time for this phase of AssetLoading.
		float startTime = Time.realtimeSinceStartup;

		// Load asset from assetBundle.
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();

        if (prefab != null)
        {
            GameObject currentAsset = Instantiate(prefab);

            //Agregar al listado
            bundlesLoaded.Add(currentAsset);
        }



        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log(assetName + (prefab == null ? " was not" : " was")+ " loaded successfully in " + elapsedTime + " seconds" );
	}
}
