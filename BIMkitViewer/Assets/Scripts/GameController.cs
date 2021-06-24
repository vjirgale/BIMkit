using DbmsApi;
using DbmsApi.API;
using MathPackage;
using ModelCheckAPI;
using ModelCheckPackage;
using RuleAPI;
using RuleAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Component = DbmsApi.API.Component;
using Debug = UnityEngine.Debug;
using Material = UnityEngine.Material;
using Mesh = UnityEngine.Mesh;

public class GameController : MonoBehaviour
{
    public Camera MainCamera;

    public Material HighlightMatRed;
    public Material HighlightMatYellow;
    public Material HighlightMatGreen;
    public Material DefaultMat;

    public GameObject LoadingCanvas;
    public GameObject ModelViewCanvas;

    public GameObject ModelSelectCanvas;
    public Button ModelButtonPrefab;
    public GameObject ModelListViewContent;
    public InputField UsernameInput;
    public InputField PasswordInput;

    public GameObject RuleSelectCanvas;
    private Model CurrentModel;
    public GameObject CurrentModelGameObj;
    public GameObject ModelObjectPrefab;
    public GameObject ModelComponentPrefab;
    private List<ModelObjectScript> ModelObjects;

    public InputField RuleUsernameInput;
    public Button RuleButtonPrefab;
    public GameObject RuleListViewContent;
    public Text RuleDescriptionText;
    private List<RuleButtonData> RuleButtonData;

    public GameObject CheckResultCanvas;
    public Button CheckResultButtonPrefab;
    public GameObject ResultListViewContent;
    public Button CheckInstanceButtonPrefab;
    public GameObject InstanceListViewContent;

    public GameObject EditModelCanvas;
    public GameObject CatalogObjectListViewContent;
    public Button CatalogObjectButtonPrefab;

    private RuleAPIController RuleAPIController;
    private DBMSAPIController DBMSController;
    private MCAPIController MCAPIController;
    private string ruleServiceURL = "https://localhost:44370/api/";
    private string dbmsURL = "https://localhost:44322//api/";
    private string mcURL = "https://localhost:44346//api/";

    // Start is called before the first frame update
    void Start()
    {
        DBMSController = new DBMSAPIController(dbmsURL);
        RuleAPIController = new RuleAPIController(ruleServiceURL);
        MCAPIController = new MCAPIController(mcURL);

        ResetCanvas();
        this.ModelSelectCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    #region Camera Controls

    public float sensitivityRotate = 1.0f;
    public float sensitivityMove = 1.0f;
    public bool caermaLock = true;
    public void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            caermaLock = !caermaLock;
        }

        if (caermaLock)
        {
            float rotateHorizontal = Input.GetAxis("Mouse X");
            float rotateVertical = Input.GetAxis("Mouse Y");
            MainCamera.transform.RotateAround(MainCamera.transform.position, Vector3.up, rotateHorizontal * sensitivityRotate);
            MainCamera.transform.RotateAround(MainCamera.transform.position, MainCamera.transform.right, -rotateVertical * sensitivityRotate);

            if (Input.GetKey(KeyCode.W))
            {
                MainCamera.transform.position += (MainCamera.transform.forward * sensitivityMove * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                MainCamera.transform.position -= (MainCamera.transform.forward * sensitivityMove * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                MainCamera.transform.position -= (MainCamera.transform.right * sensitivityMove * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                MainCamera.transform.position += (MainCamera.transform.right * sensitivityMove * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Model Select Mode

    public void ModelViewClicked()
    {
        ResetCanvas();
        this.ModelViewCanvas.SetActive(true);
    }

    public async void SignInClicked()
    {
        LoadingCanvas.SetActive(true);
        APIResponse<TokenData> response = await DBMSController.LoginAsync(this.UsernameInput.text, this.PasswordInput.text);
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        APIResponse<List<ModelMetadata>> response2 = await DBMSController.GetAvailableModels();
        if (response2.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response2.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        RemoveAllChidren(this.ModelListViewContent);
        List<ModelMetadata> models = response2.Data;
        foreach (ModelMetadata mm in models)
        {
            Button newButton = GameObject.Instantiate(this.ModelButtonPrefab, this.ModelListViewContent.transform);
            newButton.GetComponentInChildren<Text>().text = mm.ToString();
            UnityAction action = new UnityAction(() => LoadDBMSModel(mm.ModelId));
            newButton.onClick.AddListener(action);
        }

        LoadingCanvas.SetActive(false);
    }

    public async void LoadDBMSModel(string modelId)
    {
        LoadingCanvas.SetActive(true);

        APIResponse<Model> response = await DBMSController.GetModel(new ItemRequest(modelId, LevelOfDetail.LOD500));
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        CurrentModel = response.Data;

        RemoveAllChidren(CurrentModelGameObj);

        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        ModelObjects = new List<ModelObjectScript>();
        foreach (ModelObject obj in CurrentModel.ModelObjects)
        {
            GameObject modelObject = CreateModelObject(obj, CurrentModelGameObj);
            Bounds objBound = CreateComponents(obj.Components, modelObject);
            b.Encapsulate(objBound);

            modelObject.name = obj.Name;
            modelObject.transform.SetPositionAndRotation(VectorConvert(obj.Location), VectorConvert(obj.Orientation));
            ModelObjectScript script = modelObject.GetComponent<ModelObjectScript>();
            script.SetMainMaterial(DefaultMat);
            script.ModelObject = obj;

            ModelObjects.Add(script);
        }

        LoadingCanvas.SetActive(false);
        ModelSelectCanvas.SetActive(false);
        ModelViewCanvas.SetActive(true);
    }

    #endregion

    #region Model Load Methods

    public GameObject CreateModelObject(ModelObject o, GameObject parentObj)
    {
        float minZ = (float)o.Components.Min(c => c.Vertices.Min(v => v.z));
        float maxZ = (float)o.Components.Max(c => c.Vertices.Max(v => v.z));
        float height = maxZ - minZ;

        o.Id = o.Id ?? Guid.NewGuid().ToString();
        o.Orientation = o.Orientation ?? new Vector4D(1, 0, 0, 0);
        o.Location = o.Location ?? new Vector3D(0, 0, height);
        return Instantiate(ModelObjectPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), parentObj.transform);
    }

    private Bounds CreateComponents(List<Component> components, GameObject parentObj)
    {
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        foreach (Component c in components)
        {
            GameObject meshObject = Instantiate(ModelComponentPrefab, parentObj.transform);
            Mesh mesh = new Mesh();
            MeshFilter meshFilter = meshObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshCollider meshCollider = meshObject.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            mesh.vertices = c.Vertices.Select(v => VectorConvert(v)).ToArray();
            //mesh.uv = mesh.vertices.Select(v => new Vector2(v.x, v.y)).ToArray();
            mesh.uv = CalculateUVs(mesh, mesh.vertices.ToList());
            mesh.triangles = c.Triangles.SelectMany(t => new List<int>() { t[0], t[1], t[2] }).Reverse().ToArray();
            mesh.RecalculateNormals();
            b.Encapsulate(mesh.bounds);
        }

        return b;
    }

    public static Vector2[] CalculateUVs(Mesh mesh, List<Vector3> newVerticesFinal)
    {
        // calculate UVs ============================================
        float scaleFactor = 0.5f;
        Vector2[] uvs = new Vector2[newVerticesFinal.Count];
        int len = mesh.GetIndices(0).Length;
        int[] idxs = mesh.GetIndices(0);
        for (int i = 0; i < len; i = i + 3)
        {
            Vector3 v1 = newVerticesFinal[idxs[i + 0]];
            Vector3 v2 = newVerticesFinal[idxs[i + 1]];
            Vector3 v3 = newVerticesFinal[idxs[i + 2]];
            Vector3 normal = Vector3.Cross(v3 - v1, v2 - v1);
            Quaternion rotation;
            if (normal == Vector3.zero)
                rotation = new Quaternion();
            else
                rotation = Quaternion.Inverse(Quaternion.LookRotation(normal));
            uvs[idxs[i + 0]] = (Vector2)(rotation * v1) * scaleFactor;
            uvs[idxs[i + 1]] = (Vector2)(rotation * v2) * scaleFactor;
            uvs[idxs[i + 2]] = (Vector2)(rotation * v3) * scaleFactor;
        }
        //==========================================================
        return uvs;
    }

    public static Vector3 VectorConvert(Vector3D v)
    {
        return new Vector3((float)v.x, (float)v.z, (float)v.y);
    }
    public static Quaternion VectorConvert(Vector4D v)
    {
        return new Quaternion((float)v.x, (float)v.z, (float)v.y, (float)v.w);
    }

    #endregion

    #region Model View Mode

    public void ModelCheckClicked()
    {
        ResetCanvas();
        this.RuleSelectCanvas.SetActive(true);
    }

    public void EditClicked()
    {
        PopulateCatalog();
        ResetCanvas();
        EditModelCanvas.SetActive(true);
    }

    public void GenDesignClicked()
    {

    }

    public void ExitClicked()
    {
        RemoveAllChidren(CurrentModelGameObj);
        CurrentModel = null;
        ResetCanvas();
        this.ModelSelectCanvas.SetActive(true);
    }

    #endregion

    #region Model Edit Mode

    public async void PopulateCatalog()
    {
        // Access all object from the catalog:
        LoadingCanvas.SetActive(true);
        APIResponse<List<CatalogObjectMetadata>> response = await DBMSController.GetAvailableCatalogObjects();
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        // Create a button for each catalog item and place in the list
        foreach (CatalogObjectMetadata com in response.Data)
        {
            Button newButton = GameObject.Instantiate(this.CatalogObjectButtonPrefab, this.CatalogObjectListViewContent.transform);
            newButton.GetComponentInChildren<Text>().text = com.ToString();
            UnityAction action = new UnityAction(() => LoadCatalogObject(com.CatalogObjectId));
            newButton.onClick.AddListener(action);
        }

        LoadingCanvas.SetActive(false);
    }

    public void RefreshCatalogClicked()
    {
        RemoveAllChidren(this.CatalogObjectListViewContent);
        PopulateCatalog();
    }

    public void DoneEditClicked()
    {
        RemoveAllChidren(this.CatalogObjectListViewContent);
        ResetCanvas();
        ModelViewCanvas.SetActive(true);
    }

    #endregion

    #region Catalog Object Load Methods

    public async void LoadCatalogObject(string catalogId)
    {
        LoadingCanvas.SetActive(true);

        APIResponse<CatalogObject> response = await DBMSController.GetCatalogObject(new ItemRequest(catalogId, LevelOfDetail.LOD500));
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        ModelCatalogObject modelCatalogObject = CreateModelCatalogObject(response.Data);

        GameObject modelObject = CreateModelObject(modelCatalogObject, CurrentModelGameObj);
        Bounds objBound = CreateComponents(modelCatalogObject.Components, modelObject);

        modelObject.name = modelCatalogObject.Name;
        modelObject.transform.SetPositionAndRotation(VectorConvert(modelCatalogObject.Location), VectorConvert(modelCatalogObject.Orientation));
        ModelObjectScript script = modelObject.GetComponent<ModelObjectScript>();
        script.SetMainMaterial(DefaultMat);
        script.ModelObject = modelCatalogObject;

        CurrentModel.ModelObjects.Add(modelCatalogObject);
        ModelObjects.Add(script);

        LoadingCanvas.SetActive(false);
    }

    public ModelCatalogObject CreateModelCatalogObject(CatalogObject o)
    {
        return new ModelCatalogObject()
        {
            CatalogId = o.CatalogID,
            Components = o.Components,
            Name = o.Name,
            Properties = o.Properties,
            TypeId = o.TypeId
        };
    }

    #endregion

    #region Rule Select Mode

    public async void RuleSignInClicked()
    {
        LoadingCanvas.SetActive(true);

        APIResponse<RuleUser> response = await RuleAPIController.LoginAsync(this.RuleUsernameInput.text);
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        APIResponse<List<RuleSet>> response2 = await RuleAPIController.GetAllRuleSetsAsync();
        if (response2.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response2.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        List<RuleSet> ruleSets = response2.Data;
        RemoveAllChidren(this.RuleListViewContent);
        RuleButtonData = new List<RuleButtonData>();
        foreach (RuleSet rs in ruleSets)
        {
            foreach (Rule rule in rs.Rules)
            {
                Button newButton = GameObject.Instantiate(this.RuleButtonPrefab, this.RuleListViewContent.transform);
                newButton.GetComponent<RuleButtonData>().Rule = rule;
                newButton.GetComponentInChildren<Text>().text = rs.Name + " - " + rule.Name;
                UnityAction action = new UnityAction(() => RuleButtonClicked(newButton));
                newButton.onClick.AddListener(action);

                RuleButtonData.Add(newButton.GetComponent<RuleButtonData>());
            }
        }

        LoadingCanvas.SetActive(false);
    }

    public void RuleButtonClicked(Button ruleButton)
    {
        RuleButtonData data = ruleButton.GetComponent<RuleButtonData>();
        data.Clicked = !data.Clicked;
        ruleButton.image.color = data.Clicked ? new Color(0, 250, 0) : new Color(255, 255, 255);
        RuleDescriptionText.text = data.Rule.Description;
    }

    public async void CheckModelClicked()
    {
        LoadingCanvas.SetActive(true);

        List<string> rules = RuleButtonData.Select(r => r.Rule.Id).ToList();
        CheckRequest request = new CheckRequest(DBMSController.Token, RuleAPIController.CurrentUser.Username, CurrentModel.Id, rules, LevelOfDetail.LOD500);
        APIResponse<List<RuleResult>> response = await MCAPIController.PerformModelCheck(request);
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        List<RuleResult> results = response.Data;
        RemoveAllChidren(this.ResultListViewContent);
        foreach (RuleResult result in results)
        {
            Button newButton = GameObject.Instantiate(this.CheckResultButtonPrefab, this.ResultListViewContent.transform);
            newButton.GetComponentInChildren<Text>().text = result.ToString();
            UnityAction action = new UnityAction(() => ResultButtonClicked(result));
            newButton.onClick.AddListener(action);
        }

        ResetCanvas();
        CheckResultCanvas.SetActive(true);
    }

    public void CancelRuleSelectClicked()
    {
        this.ResetCanvas();
        this.ModelViewCanvas.SetActive(true);
    }

    #endregion

    #region Model Check Mode

    private void ResultButtonClicked(RuleResult result)
    {
        RemoveAllChidren(this.InstanceListViewContent);
        foreach (RuleInstance instance in result.RuleInstances)
        {
            Button newButton = GameObject.Instantiate(this.CheckInstanceButtonPrefab, this.InstanceListViewContent.transform);
            newButton.GetComponentInChildren<Text>().text = instance.ToString();
            UnityAction action = new UnityAction(() => InstanceButtonClicked(instance, result));
            newButton.onClick.AddListener(action);
        }

        UnHighlightAllObjects();
        foreach (var objId in result.RuleInstances.SelectMany(i => i.Objs.Select(o => o.ID)))
        {
            ModelObjects.First(o => o.ModelObject.Id == objId).Highlight(HighlightMatYellow);
        }
    }

    private void InstanceButtonClicked(RuleInstance instance, RuleResult result)
    {
        UnHighlightAllObjects();
        foreach (var objId in result.RuleInstances.SelectMany(i => i.Objs.Select(o => o.ID)))
        {
            ModelObjects.First(o => o.ModelObject.Id == objId).Highlight(HighlightMatYellow);
        }
        foreach (var objId in instance.Objs.Select(i => i.ID))
        {
            ModelObjects.First(o => o.ModelObject.Id == objId).Highlight(instance.PassVal == 1 ? HighlightMatGreen : HighlightMatRed);
        }
    }

    public void DoneCheckClicked()
    {
        UnHighlightAllObjects();
        this.ResetCanvas();
        this.ModelViewCanvas.SetActive(true);
    }

    #endregion

    #region Random Methods

    public static void RemoveAllChidren(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void ResetCanvas()
    {
        this.ModelViewCanvas.SetActive(false);
        this.RuleSelectCanvas.SetActive(false);
        this.CheckResultCanvas.SetActive(false);
        this.ModelViewCanvas.SetActive(false);
        this.LoadingCanvas.SetActive(false);
        this.EditModelCanvas.SetActive(false);
    }

    private void UnHighlightAllObjects()
    {
        foreach (ModelObjectScript mo in ModelObjects)
        {
            if (mo.IsHighlighted)
            {
                mo.UnHighlight();
            }
        }
    }

    #endregion
}