﻿using DbmsApi;
using DbmsApi.API;
using GenerativeDesignAPI;
using MathPackage;
using ModelCheckAPI;
using ModelCheckPackage;
using RuleAPI;
using RuleAPI.Methods;
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
    public Dropdown LevelOfDetailDropdown;

    public GameObject RuleSelectCanvas;
    public GameObject CurrentModelGameObj;
    public GameObject ModelObjectPrefab;
    public GameObject ModelComponentPrefab;

    public InputField RuleUsernameInput;
    public Button RuleButtonPrefab;
    public GameObject RuleListViewContent;
    public Text RuleDescriptionText;
    private List<ButtonData> RuleButtonData;
    public Button ModelCheckButton;
    public Button GenDesignButton;

    public GameObject CheckResultCanvas;
    public Button CheckResultButtonPrefab;
    public GameObject ResultListViewContent;
    public Button CheckInstanceButtonPrefab;
    public GameObject InstanceListViewContent;
    public Text InstanceValueText;

    public GameObject AddObjectCanvas;
    public GameObject CatalogObjectListViewContent;
    public Button CatalogObjectButtonPrefab;

    private RuleAPIController RuleAPIController;
    private DBMSAPIController DBMSController;
    private MCAPIController MCAPIController;
    private GDAPIController GDAPIController;
    private string ruleServiceURL = "https://localhost:44370/api/";
    private string dbmsURL = "https://localhost:44322//api/";
    private string mcURL = "https://localhost:44346//api/";
    private string gdURL = "https://localhost:44328///api/";

    private Model CurrentModel;
    private List<ModelObjectScript> ModelObjects = new List<ModelObjectScript>();

    // Start is called before the first frame update
    void Start()
    {
        DBMSController = new DBMSAPIController(dbmsURL);
        RuleAPIController = new RuleAPIController(ruleServiceURL);
        MCAPIController = new MCAPIController(mcURL);
        GDAPIController = new GDAPIController(gdURL);

        ResetCanvas();
        this.ModelSelectCanvas.SetActive(true);

        List<ObjectTypes> types = ObjectTypeTree.ObjectDict.Keys.ToList();
        List<ObjectType> leafTypes = types.Select(t => ObjectTypeTree.GetNode(t)).Where(t => t.Children.Count == 0).ToList();
        this.ObjectTypeChangeDropdown.options.Clear();
        this.ObjectTypeChangeDropdown.options.AddRange(leafTypes.Select(t => new OptionData(t.ID.ToString())));

        this.LevelOfDetailDropdown.options.Clear();
        this.LevelOfDetailDropdown.options.AddRange(Enum.GetValues(typeof(LevelOfDetail)).Cast<LevelOfDetail>().Select(l => new OptionData(l.ToString())));
        int index = LevelOfDetailDropdown.options.FindIndex((i) => { return i.text.Equals(LevelOfDetail.LOD500.ToString()); });
        this.LevelOfDetailDropdown.value = index;
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
        if (roatatingObject)
        {
            RotatingObject();
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

        if (!roatatingObject)
        {
            MainCamera.transform.position += MainCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        }

        //float fov = MainCamera.fieldOfView;
        //fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        //fov = Mathf.Clamp(fov, minFov, maxFov);
        //MainCamera.fieldOfView = fov;
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

        string selectedLOD = LevelOfDetailDropdown.options[LevelOfDetailDropdown.value].text;
        LevelOfDetail lod = (LevelOfDetail)Enum.Parse(typeof(LevelOfDetail), selectedLOD);
        APIResponse <Model> response = await DBMSController.GetModel(new ItemRequest(modelId, lod));
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
        o.Id = o.Id ?? Guid.NewGuid().ToString();
        o.Orientation = o.Orientation ?? Utils.GetQuaterion(new Vector3D(0, 0, 1), 0.0 * Math.PI / 180.0);
        o.Location = o.Location ?? new Vector3D(0, 0, 0);
        return Instantiate(ModelObjectPrefab, parentObj.transform);
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
    public static Vector3D VectorConvert(Vector3 v)
    {
        return new Vector3D((float)v.x, (float)v.z, (float)v.y);
    }
    public static Vector4D VectorConvert(Quaternion v)
    {
        return new Vector4D((float)v.x, (float)v.z, (float)v.y, (float)v.w);
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

        if (mos.ModelObject.GetType() == typeof(ModelCatalogObject))
        {
            ObjectDataText.text += "Catalog Id: " + ((ModelCatalogObject)mos.ModelObject).CatalogId + "\n";
        }

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
        modelCheckMode = true;
        GenDesignButton.gameObject.SetActive(false);
        ModelCheckButton.gameObject.SetActive(true);
    }

    public void EditModelClicked()
    {
        ResetCanvas();
        ModelEditCanvas.SetActive(true);
    }

    public void AddObjectClicked()
    {
        RefreshCatalogClicked();
        ResetCanvas();
        AddObjectCanvas.SetActive(true);
    }

    public void GenDesignClicked()
    {
        RefreshCatalogClicked();
        ResetCanvas();
        AddObjectCanvas.SetActive(true);
        genDesignMode = true;
        GenDesignButton.gameObject.SetActive(true);
        ModelCheckButton.gameObject.SetActive(false);
    }

    public async void SaveModelClicked()
    {
        DBMSReadWrite.WriteModel(CurrentModel, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\temp.bpm");

        LoadingCanvas.SetActive(true);
        APIResponse<string> response = await this.DBMSController.UpdateModel(CurrentModel);
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        Debug.LogWarning("Saved");
        LoadingCanvas.SetActive(false);
    }

    public void RevertToPreviousVersion()
    {
        string modelId = CurrentModel.Id;
        ExitClicked();
        LoadDBMSModel(modelId);
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

    public void DeleteObject()
    {
        if (EditingGameObject != null)
        {
            ModelObjectScript mos = EditingGameObject.GetComponent<ModelObjectScript>();
            ModelObjects.Remove(mos);
            CurrentModel.ModelObjects.Remove(mos.ModelObject);
            Destroy(EditingGameObject);
            EditingGameObject = null;
        }
    }

    public void MoveObjectClicked()
    {
        if (EditingGameObject == null)
        {
            return;
        }
        PlaceOject();
    }

    public void RotateObjectClicked()
    {
        roatatingObject = true;
        if (EditingGameObject == null)
        {
            roatatingObject = false;
            return;
        }
    }

    bool roatatingObject;
    private void RotatingObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeAllChidrenTags(EditingGameObject, "Untagged");
            ModelObject mo = EditingGameObject.GetComponent<ModelObjectScript>().ModelObject;
            mo.Location = VectorConvert(EditingGameObject.transform.position);
            mo.Orientation = VectorConvert(EditingGameObject.transform.rotation);
            roatatingObject = false;
            return;
        }

        float rotationAmount = Input.mouseScrollDelta.y * 90.0f;
        //Vector4D quaternion = Utils.GetQuaterion(new Vector3D(0, 0, 1), rotationAmount * Math.PI / 180.0);
        EditingGameObject.transform.Rotate(Vector3.up, rotationAmount);
    }

    public void AddPropertyClicked()
    {
        if (EditingGameObject != null)
        {
            ModelObjectScript mos = EditingGameObject.GetComponent<ModelObjectScript>();
            foreach (var method in MethodFinder.GetAllPropertyInfos())
            {
                object result = method.Value.Invoke(null, new object[] { new RuleCheckObject(mos.ModelObject) });
                if (result.GetType() == typeof(string))
                {
                    mos.ModelObject.Properties.Add(method.Key, (string)result);
                }
                if (result.GetType() == typeof(double))
                {
                    mos.ModelObject.Properties.Add(method.Key, (double)result);
                }
                if (result.GetType() == typeof(bool))
                {
                    mos.ModelObject.Properties.Add(method.Key, (bool)result);
                }
            }
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

    private Vector3 worldPosition = new Vector3();
    private float heightOfset = 0;
    private float ERROR = 0.0001f;

    private async void PlaceCatalogObject(string catalogId)
    {
        EditingGameObject = await LoadCatalogObject(catalogId);
        PlaceOject();
    }

    private void PlaceOject()
    {
        ChangeAllChidrenTags(EditingGameObject, "Temp");

        ModelObject mo = EditingGameObject.GetComponent<ModelObjectScript>().ModelObject;
        float minZ = (float)mo.Components.Min(c => c.Vertices.Min(v => v.z));
        float maxZ = (float)mo.Components.Max(c => c.Vertices.Max(v => v.z));
        heightOfset = (maxZ - minZ) / 2.0f + ERROR;

        placingObject = true;
        if (EditingGameObject == null)
        {
            placingObject = false;
            return;
        }
    }

    #endregion

    #region Moving Objects

    private GameObject EditingGameObject;
    private bool placingObject;
    private void MoveObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeAllChidrenTags(EditingGameObject, "Untagged");
            ModelObject mo = EditingGameObject.GetComponent<ModelObjectScript>().ModelObject;
            mo.Location = VectorConvert(EditingGameObject.transform.position);
            mo.Orientation = VectorConvert(EditingGameObject.transform.rotation);
            placingObject = false;

            if (genDesignMode)
            {
                GeneratingObject = EditingGameObject;
                SelectRulesClicked();
            }

            return;
        }

        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.collider.transform.tag != "Temp")
        {
            worldPosition = hitData.point + new Vector3(0, heightOfset, 0);
        }
        EditingGameObject.transform.position = worldPosition;
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
            TypeId = o.TypeId == 0 ? ObjectTypes.BuildingElement : o.TypeId
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
        RuleButtonData = new List<ButtonData>();
        foreach (RuleSet rs in ruleSets)
        {
            foreach (Rule rule in rs.Rules)
            {
                Button newButton = GameObject.Instantiate(this.RuleButtonPrefab, this.RuleListViewContent.transform);
                newButton.GetComponent<ButtonData>().Item = rule;
                newButton.GetComponentInChildren<Text>().text = rs.Name + " - " + rule.Name;
                UnityAction action = new UnityAction(() => RuleButtonClicked(newButton));
                newButton.onClick.AddListener(action);

                RuleButtonData.Add(newButton.GetComponent<ButtonData>());
            }
        }

        LoadingCanvas.SetActive(false);
    }

    public void RuleButtonClicked(Button ruleButton)
    {
        ButtonData data = ruleButton.GetComponent<ButtonData>();
        data.Clicked = !data.Clicked;
        ruleButton.image.color = data.Clicked ? new Color(0, 250, 0) : new Color(255, 255, 255);
        RuleDescriptionText.text = ((Rule)data.Item).Description;
    }

    public async void CheckModelClicked()
    {
        LoadingCanvas.SetActive(true);

        List<string> rules = RuleButtonData.Where(rbd => rbd.Clicked).Select(r => ((Rule)r.Item).Id).ToList();
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
        modelCheckMode = false;
    }

    private GameObject GeneratingObject;
    public async void PerfromGenDesignClicked()
    {
        LoadingCanvas.SetActive(true);

        List<string> rules = RuleButtonData.Where(rbd => rbd.Clicked).Select(r => ((Rule)r.Item).Id).ToList();

        ModelCatalogObject mo = (ModelCatalogObject)GeneratingObject.GetComponent<ModelObjectScript>().ModelObject;
        GenerativeRequest request = new GenerativeRequest(DBMSController.Token,
                                                          RuleAPIController.CurrentUser.Username,
                                                          CurrentModel.Id,
                                                          mo.CatalogId,
                                                          rules,
                                                          LevelOfDetail.LOD100,
                                                          VectorConvert(GeneratingObject.transform.position),
                                                          new GenSettings(
                                                                Convert.ToInt32(20),
                                                                Convert.ToDouble(20),
                                                                Convert.ToDouble(0.5),
                                                                Convert.ToInt32(10),
                                                                false
                                                                )
                                                          );

        APIResponse<string> response = await GDAPIController.PerformGenDesign(request);
        if (response.Code != System.Net.HttpStatusCode.OK)
        {
            Debug.LogWarning(response.ReasonPhrase);
            LoadingCanvas.SetActive(false);
            return;
        }

        GeneratingObject = null;
        genDesignMode = false;
        ExitClicked();
        SignInClicked();
        LoadDBMSModel(response.Data);
    }

    public void CancelRuleSelectClicked()
    {
        this.ResetCanvas();
        this.ModelViewCanvas.SetActive(true);
        modelCheckMode = false;
        genDesignMode = false;
    }

    #endregion

    #region Generative Design Mode

    bool genDesignMode;

    public void SelectRulesClicked()
    {
        ResetCanvas();
        this.RuleSelectCanvas.SetActive(true);
        this.genDesignMode = true;
    }

    #endregion

    #region Model Check Mode

    bool modelCheckMode;

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

        string displayStr = "";
        foreach (RuleCheckObject ruleCheckObject in instance.Objs)
        {
            displayStr += ruleCheckObject.Name + "\n";
            foreach (Property property in ruleCheckObject.Properties)
            {
                displayStr += property.String() + "\n";
            }
            displayStr += "\n";
        }
        displayStr += "=====================================\n";
        foreach (RuleCheckRelation ruleCheckRelation in instance.Rels)
        {
            displayStr += ruleCheckRelation.FirstObj.Name + " => "+ ruleCheckRelation.SecondObj.Name + "\n";
            foreach (Property property in ruleCheckRelation.Properties)
            {
                displayStr += property.String() + "\n";
            }
        }
        InstanceValueText.text = displayStr;
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
        EditingGameObject = null;
        placingObject = false;
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