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
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Dropdown;
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
    public Text ObjectDataText;

    public GameObject ModelEditCanvas;
    public Dropdown ObjectTypeChangeDropdown;

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
    private List<ModelObjectScript> ModelObjects = new List<ModelObjectScript>();

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

    public GameObject AddObjectCanvas;
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

        List<ObjectTypes> types = ObjectTypeTree.ObjectDict.Keys.ToList();
        List<ObjectType> leafTypes = types.Select(t => ObjectTypeTree.GetNode(t)).Where(t => t.Children.Count == 0).ToList();
        this.ObjectTypeChangeDropdown.options.Clear();
        this.ObjectTypeChangeDropdown.options.AddRange(leafTypes.Select(t => new OptionData(t.ID.ToString())));
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        if (this.ModelViewCanvas.activeInHierarchy)
        {
            ViewingMode();
        }
        if (placingObject)
        {
            MoveObject();
        }
        if (this.ModelEditCanvas.activeInHierarchy)
        {
            EditingMode();
        }
    }

    #region Camera Controls

    public float cameraSpeed = 200f;
    float minFov = 10f;
    float maxFov = 90f;
    float sensitivity = 30f;

    public void MoveCamera()
    {
        if (Input.GetMouseButton(1))
        {
            MainCamera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0));
            float X = MainCamera.transform.rotation.eulerAngles.x;
            float Y = MainCamera.transform.rotation.eulerAngles.y;
            MainCamera.transform.rotation = Quaternion.Euler(X, Y, 0);
        }

        if (Input.GetMouseButton(2))
        {
            var newPosition = new Vector3();
            newPosition.x = Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime;
            newPosition.y = Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime;
            MainCamera.transform.Translate(-newPosition);
        }

        float fov = MainCamera.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        MainCamera.fieldOfView = fov;
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

        this.ResetCanvas();
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
        o.Orientation = o.Orientation ?? new Vector4D(0, 0, 0, 1);
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
            mesh.uv = mesh.vertices.Select(v => new Vector2(v.x, v.y)).ToArray();
            //mesh.uv = CalculateUVs(mesh, mesh.vertices.ToList());
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

    private GameObject ViewingGameObject;
    public void ViewingMode()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            ModelObjectScript mos;
            if (ViewingGameObject != null)
            {
                mos = ViewingGameObject.GetComponent<ModelObjectScript>();
                mos.UnHighlight();
            }

            ViewingGameObject = hitData.collider.gameObject.transform.parent.gameObject;
            DisplayObjectInfo();

            mos = ViewingGameObject.GetComponent<ModelObjectScript>();
            mos.Highlight(HighlightMatYellow);
        }
    }

    private void DisplayObjectInfo()
    {
        ModelObjectScript mos = ViewingGameObject.GetComponent<ModelObjectScript>();
        ObjectDataText.text = "Name: " + mos.ModelObject.Name + "\n";
        ObjectDataText.text += "Id: " + mos.ModelObject.Id + "\n";
        ObjectDataText.text += "TypeId: " + mos.ModelObject.TypeId + "\n\n";
        foreach (Property p in mos.ModelObject.Properties)
        {
            ObjectDataText.text += p.Name + ": " + p.GetValueString() + "\n";
        }
    }

    public void ModelCheckClicked()
    {
        ResetCanvas();
        this.RuleSelectCanvas.SetActive(true);
    }

    public void EditModelClicked()
    {
        ResetCanvas();
        ModelEditCanvas.SetActive(true);
    }

    public void AddObjectClicked()
    {
        PopulateCatalog();
        ResetCanvas();
        AddObjectCanvas.SetActive(true);
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

    private GameObject EditingGameObject;
    public void EditingMode()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                ModelObjectScript mos;
                if (EditingGameObject != null)
                {
                    mos = EditingGameObject.GetComponent<ModelObjectScript>();
                    mos.UnHighlight();
                }

                EditingGameObject = hitData.collider.gameObject.transform.parent.gameObject;
                mos = EditingGameObject.GetComponent<ModelObjectScript>();
                mos.Highlight(HighlightMatRed);

                int index = ObjectTypeChangeDropdown.options.FindIndex((i) => { return i.text.Equals(mos.ModelObject.TypeId.ToString()); });
                this.ObjectTypeChangeDropdown.value = index;
            }
        }
    }

    public void TypeDropdownChange()
    {
        if (EditingGameObject != null)
        {
            string selectedType = ObjectTypeChangeDropdown.options[ObjectTypeChangeDropdown.value].text;
            ObjectTypes newType = (ObjectTypes)Enum.Parse(typeof(ObjectTypes), selectedType);
            ModelObjectScript mos = EditingGameObject.GetComponent<ModelObjectScript>();
            mos.ModelObject.TypeId = newType;
        }
    }

    #endregion

    #region Model Select Catalog Object Mode:

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
            UnityAction action = new UnityAction(() => PlaceCatalogObject(com.CatalogObjectId));
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

    #region Place Catalog Object Mode:

    private bool placingObject;
    private GameObject MovingGameObject;
    private Vector3 worldPosition = new Vector3();
    private float heightOfset = 0;

    private async void PlaceCatalogObject(string catalogId)
    {
        MovingGameObject = await LoadCatalogObject(catalogId);
        ChangeAllChidrenTags(MovingGameObject, "Temp");

        ModelObject mo = MovingGameObject.GetComponent<ModelObjectScript>().ModelObject;
        float minZ = (float)mo.Components.Min(c => c.Vertices.Min(v => v.z));
        float maxZ = (float)mo.Components.Max(c => c.Vertices.Max(v => v.z));
        heightOfset = maxZ - minZ;

        placingObject = true;
        if (MovingGameObject == null)
        {
            placingObject = false;
            return;
        }
    }

    private void MoveObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeAllChidrenTags(MovingGameObject, "Untagged");
            MovingGameObject = null;
            placingObject = false;
            return;
        }

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.collider.transform.tag != "Temp")
        {
            worldPosition = hitData.point + new Vector3(0, heightOfset, 0);
        }
        MovingGameObject.transform.position = worldPosition;
    }

    #endregion

    #region Catalog Object Load Methods

    public async Task<GameObject> LoadCatalogObject(string catalogId)
    {
        LoadingCanvas.SetActive(true);

        APIResponse<CatalogObject> response = await DBMSController.GetCatalogObject(new ItemRequest(catalogId, LevelOfDetail.LOD500));
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return null;
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

        return modelObject;
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

    public static void ChangeAllChidrenTags(GameObject obj, string newTag)
    {
        obj.transform.tag = newTag;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            var child = obj.transform.GetChild(i);
            ChangeAllChidrenTags(child.gameObject, newTag);
        }
    }

    private void ResetCanvas()
    {
        this.ModelEditCanvas.SetActive(false);
        this.RuleSelectCanvas.SetActive(false);
        this.CheckResultCanvas.SetActive(false);
        this.ModelViewCanvas.SetActive(false);
        this.LoadingCanvas.SetActive(false);
        this.AddObjectCanvas.SetActive(false);
        this.ModelSelectCanvas.SetActive(false);

        UnHighlightAllObjects();

        ViewingGameObject = null;

        placingObject = false;
        MovingGameObject = null;

        EditingGameObject = null;
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