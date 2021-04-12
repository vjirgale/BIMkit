## DBMS

The `DBMS` solution is split into two parts, `DBMS` and `DbmsApi`.
`DBMS` is whats running on the server and `DbmsApi` provides classes for interfacing with `DBMS`.

`DbmsApi` provides two components. 

1. `MongoModels`: Models that define how documents are stored in MongoDB. For example, below is the `BaseModel` model.

```CSharp

namespace DbmsApi.MongoModels.SubModels
{
    public abstract class BaseModel : MongoDocument
    {

        public string Name;
        public Properties Properties = new Properties();
        public List<ModelObject> ModelObjects = new List<ModelObject>();
        public List<Relation> Relations = new List<Relation>();

        public abstract class BaseObject
        {
            public string Id;
            public Tuple<double, double, double> Location;
            public Tuple<double, double, double, double> Orientation;
            public List<KeyValuePair<string, string>> Tags = new List<KeyValuePair<string, string>>();
        }

        public class ModelObject : BaseObject
        {
            public string Name;
            public ObjectTypes TypeId;

            public Properties Properties = new Properties();
            public List<Component> Components = new List<Component>();
        }

        public class Relation
        {
            public string ObjectId1;
            public string ObjectId2;
            public Properties Properties = new Properties();
        }

    }
}


```

2. `DBMSAPIController`: A class that handles stuff like routing requests and automatically refreshing session tokens. 
Below are some of the method signatures in `DBMSAPIController`.

```CSharp
public DBMSAPIController(string serverAddress);

// Performs login and sets session and refresh tokens
public async Task<APIResponse> LoginAsync(string username, string password);

// session token can be set directly in place of logging in
public string sessionToken { get; set; } 

// CRUD Methods automatically use the session token, and refresh it with the refresh token if it is expired
public async Task<APIResponse<string>> CreateModel(Model model);

public async Task<APIResponse<CatalogObject>> RetrieveCatalogObject(string Id);
```

Small notes:
* The methods are async to avoid freezing the application while waiting for the webserver
* `APIResponse<T>` is just a wrapper for the type (`T`) of `MongoModel` being operated on, it contains the `MongoModel` and some data about the web request

## BIMPlatform

`BIMPlatform` will need methods for the applications (Unity & MCAD) to switch between `MongoModels` and `BIMPlatform.Model`.
For example, the method signatures would look as such:

```CSharp
using DbmsApi;

namespace BIMPlactform
{
  public class Model
  {
    public Model(MongoModels.Model);

    public MongoModels.Model ToMongoModel();
  }
}
```

Or, inside a static helper class:

```CSharp
using Dbms.Api;

namespace BIMPlactform
{
  public static class MongoModelHelper
  {
    public static MongoModels.Model ToMongoModel(this BIMPlatform.Model);
    public static BIMPlatform.Model ToBPModel(this MongoModels.Model);
    
    public static MongoModels.CatalogObject ToMongoModel(this BIMPlatfom.Obj);
    public static BIMPlatform.Obj ToBPModel(this MongoModels.CatalogObject);
  }
}
```

The implementation is not important but I think the conversion should be inside the `BIMPlatform` project.


## Unity & MCAD

I'm sure you see where this is going, but allow me to demonstrate anyways. With this implementation, 
the final applications could use both libraries as they see fit.

```CSharp
public async void Init()
{
  this.DBMSAPIController = new DBMSAPIController(serverAddress);
  await this.DBMSAPIController.LoginAsync("username", "password");
}

// In MCAD using constructor converter
public Task<BIMPlatform.Model> LoadModel(string Id)
{
  return new BIMPlatform.Model(await this.DBMSAPIController.RetrieveModel(Id));
}

// In Unity using static converter
public Task SaveObject(MeshObject mo)
{
  return await this.DBMSAPIController.SaveObject(mo.BIMPlatformObject.ToMongoModel());
}
```





