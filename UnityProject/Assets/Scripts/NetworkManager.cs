using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SocketIO;
using System.Net.Sockets;

public class NetworkManager : MonoBehaviour {

	private static NetworkManager instance;
	public static NetworkManager Instance{get{return instance; }}
    public Canvas FBcanvas;
    public Canvas Joincanvas;
    public Canvas ServerStatuscanvas;
    public SocketIOComponent socket;
	public GameObject player;
    public string GetIPAddress = "73.146.71.203";
    public int GetPort = 3000;
    private bool hasjoined = false;
    private bool hasbeenDC = false;
    private bool userIsConnected = false;



    public GameObject cam;

    public string _UserId = "";

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy (gameObject);
		}
        CheckServerStatus();


    }
    private bool Load_Once = false;
	void Load_Core () {
		//subscribe to all the websocket events
		//socket.On("OnEnemies",_OnEnemies);
		socket.On("OnPlayerConnected",_OnPlayerConnected);
		//socket.On("OnPlay",_OnPlay);
		socket.On("OnPlayerMove",_OnPlayerMove);
		
		//socket.On("OnPlayerShoot",_OnPlayerShoot);
		//socket.On("OnHealth",_OnHealth);
		socket.On("OnPlayerDisconnect",_OnPlayerDisconnect);
        Load_Once = true;


    }
    int check = 0;
    private void CheckServerStatus()
    {

        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect(GetIPAddress, GetPort);
            Debug.Log("Port open");
            if(hasbeenDC == true && hasjoined == false && userIsConnected == false)
            {
                JoinGame();
            }
            ServerStatuscanvas.gameObject.SetActive(false);
        }
        catch (Exception)
        {
            Debug.Log("Port closed");
            if(hasjoined == true)
            {
                hasbeenDC = true;
                ServerStatuscanvas.gameObject.SetActive(true);
            }
            hasjoined = false;
            userIsConnected = false;
    
        }
        Debug.Log("Check Server: "+ check);

    }


    public string SetUserId(string UserId)
    {
        _UserId = UserId;
        return _UserId;
    }

    public string GetUserId()
    {
        return _UserId;
    }
    
    private void Update()
    {
        check++;

        Debug.Log("hasbeenDC ?: " + hasbeenDC);
        Debug.Log("hasjoined ?: " + hasjoined);
        Debug.Log("userIsConnected ?: " + userIsConnected);
      

            if (check > 500)
        {
            //Ok We now know if the server is online or offline this would also be a good place to auto save the players 
            CheckServerStatus();
            AutoSavePlayer();
            check = 0;
        }
       
        if (Data_Manager.Instance != null && Data_Manager.Instance.UserId.text == "UserId")
        {
           // Debug.Log("Lets Not Send Or Get Any Data From Server Until User Is Login!");
            return;
        }

        if (IsDisconnecteding == true)
        {
            ClientDissconnect();
        }
        if (Load_Once == false)
        {
            Load_Core();
        }
    }

    private void AutoSavePlayer()
    {
        if (Data_Manager.Instance != null && Data_Manager.Instance.UserId.text != "UserId" && hasjoined == true)
        {
            Debug.Log("Lets Save The Players Details");
            if (MysqlManager.Instance != null)
            {
                MysqlManager.Instance.SaveUsersData();
            }
            else
            {
                Debug.Log("MySql Manager Is Null AutoSavePlayer");
            }
            return;
        }
    }


    private bool IsDisconnecteding = false;
    public void OnApplicationQuit() 
	{
        if (Data_Manager.Instance != null && Data_Manager.Instance.UserId.text != "UserId")
        {
            ClientDissconnect();
            Debug.Log("Application ending after 3 " + Time.time + " seconds");
            IsDisconnecteding = true;
        }

    }
   
	public void JoinGame()
	{

        if (hasjoined == false)
        {
            StartCoroutine(_ConnectToServer());
        }

	}
	#region Commands
	IEnumerator _ConnectToServer()
	{
		yield return new WaitForSeconds (0.5f);

		//socket.Emit ("player connect");
		yield return new WaitForSeconds (1f);
		//string playerName = playerNameInput.text;
		List<SpawnPoints> playSpawnPoints = GetComponent<PlayersSpawnPoint> ().playerSpawnPoints;
		List<SpawnPoints> enemySpawnPoints = GetComponent<EnemySpawnPoint> ().enemySpawnPoionts;
		PlayerJSON playerJSON = new PlayerJSON (
			Data_Manager.Instance.GetId(),
			Data_Manager.Instance.GetUserId(),
			Data_Manager.Instance.GetUserName(),
			Data_Manager.Instance.GetUserPic(),
			Data_Manager.Instance.GetUserToken(),
			Data_Manager.Instance.GetUserPosX(),
			Data_Manager.Instance.GetUserPosY(),
			Data_Manager.Instance.GetUserPosZ(),
			Data_Manager.Instance.GetUserLevel(),
			Data_Manager.Instance.GetUserCurrency(),
			Data_Manager.Instance.GetUserExpierance(),
			Data_Manager.Instance.GetUserHealth(),
			Data_Manager.Instance.GetUserPower(),
			Data_Manager.Instance.GetUserGpsX(),
			Data_Manager.Instance.GetUserGpsY(),
			Data_Manager.Instance.GetUserGpsZ(),
			Data_Manager.Instance.GetUserVungleApi(),
			Data_Manager.Instance.GetUserAdcolonyApi(),
			Data_Manager.Instance.GetUserAdcolonyZone(),
			playSpawnPoints, 
			enemySpawnPoints);
		string data = JsonUtility.ToJson (playerJSON);
		socket.Emit ("OnConnection", new JSONObject (data));
		
	}

	public void CommandMove()
	{
        float posx;
        float posy;
        float posz;
        float.TryParse(Data_Manager.Instance.GetUserPosX(), out posx);
        float.TryParse(Data_Manager.Instance.GetUserPosY(), out posy);
        float.TryParse(Data_Manager.Instance.GetUserPosZ(), out posz);

        float rotx;
        float roty;
        float rotz;
        float.TryParse(Data_Manager.Instance.GetUserRotX(), out rotx);
        float.TryParse(Data_Manager.Instance.GetUserRotY(), out roty);
        float.TryParse(Data_Manager.Instance.GetUserRotZ(), out rotz);

        POSPlayerJSON posplayerJSON = new POSPlayerJSON(
            Data_Manager.Instance.GetUserId(), 
            posx,posy,posz,rotx,roty,rotz);
        string data = JsonUtility.ToJson(posplayerJSON);
        socket.Emit ("OnPlayerMove", new JSONObject (data));

	}
	

	public void CommandShoot()
	{
		print ("Shoot");
		socket.Emit ("OnPlayerShoot");

	}

	public void ClientDissconnect()
	{

        DCPlayerJSON dcplayerJSON = new DCPlayerJSON(
            Data_Manager.Instance.GetUserId());
        string data = JsonUtility.ToJson(dcplayerJSON);
        socket.Emit("OnPlayerDisconnect", new JSONObject(data));
        print("ClientDissconnect " + data);

    }

	public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange, bool isEnemy)
	{
		print ("health change cmd");
		HealthChangeJSON healthChangeJSON = new HealthChangeJSON (playerTo.name, healthChange, playerFrom.name, isEnemy);
		socket.Emit ("health", new JSONObject (JsonUtility.ToJson(healthChangeJSON)));

	}
	#endregion

	#region Listening

	void _OnEnemies(SocketIOEvent socketIOEvent)
	{
		EnemiesJSON enemiesJSON = EnemiesJSON.CreateFromJSON (socketIOEvent.data.ToString());
		EnemySpawnPoint es = GetComponent<EnemySpawnPoint> ();
		es.SpawnEnemys (enemiesJSON);
	}

	void _OnPlayerConnected(SocketIOEvent socketIOEvent)
	{

		//Debug.Log ("Method: _OnPlayerConnected");
		//Debug.Log ("Client Recieved:");
		string data = socketIOEvent.data.ToString ();
		//Debug.Log ("Data_Manager:"+ data);

		UserJSON userJSON = UserJSON.CreateFromJSON (data);
		//Debug.Log ("Data: Id: "+ userJSON.Id);
		
		GameObject o = GameObject.Find (userJSON.UserId) as GameObject;
			if(o != null)
			{
            hasjoined = true;
            userIsConnected = true;
            hasbeenDC = false;
            Debug.Log ("Debug: _OnPlayerConnected Player Is Already Online:"+ o.name);
				return;
			}
        float x;
        float y;
        float z;
        float.TryParse(userJSON.UserPosX.ToString(), out x);
        float.TryParse(userJSON.UserPosY.ToString(), out y);
        float.TryParse(userJSON.UserPosZ.ToString(), out z);
        GameObject p = Instantiate (player, new Vector3(x,y,z),Quaternion.Euler(Vector3.zero)) as GameObject;
		p.name = userJSON.UserId;


        if (Data_Manager.Instance != null)
        {

            if(Data_Manager.Instance.GetUserId() == userJSON.UserId.ToString())
            {
                 Transform _Player_Camera = p.transform.Find("Player_Camera");
                 _Player_Camera.gameObject.SetActive(true);
                 Transform _UI_Controls = p.transform.Find("UI_Controls");
                 _UI_Controls.gameObject.SetActive(true);
                hasjoined = true;
                userIsConnected = true;
                hasbeenDC = false;
                cam.SetActive(false);
                SetUserId(userJSON.UserId.ToString());
                Joincanvas.gameObject.SetActive(false);
                FacebookManager.Instance.Facebook_Canvas.SetActive(true);
                FacebookManager.Instance.Collected_Data_Canvas.SetActive(true);
                FacebookManager.Instance.Join_Game_Canvas.SetActive(false);

            }
           // Debug.Log("WHO AM I" + Data_Manager.Instance.GetUserId());
        }
        
      



        Get_Data_Container_Info(p, userJSON);


		
	}





	void _OnPlayerMove(SocketIOEvent socketIOEvent)
	{
		//Debug.Log ("Method: _OnPlayerMove");
		//Debug.Log ("Client Recieved:");
		string data = socketIOEvent.data.ToString ();
		//Debug.Log ("Data_Manager:"+ data);

        POSUserJSON posuserJSON = POSUserJSON.CreateFromJSON (data);
       // Debug.Log("Data: UserId: " + posuserJSON.UserId);
      //  Debug.Log("Data: PosX: " + posuserJSON.posx);
       // Debug.Log("Data: PosY: " + posuserJSON.posy);
       // Debug.Log("Data: PosZ: " + posuserJSON.posz);
      //  Debug.Log("Data: RotX: " + posuserJSON.rotx);
      //  Debug.Log("Data: RotY: " + posuserJSON.roty);
      //  Debug.Log("Data: RotZ: " + posuserJSON.rotz);

        


        Vector3 position = new Vector3(posuserJSON.posx, posuserJSON.posy, posuserJSON.posz);
           Quaternion rotation = Quaternion.Euler(posuserJSON.rotx, posuserJSON.roty, posuserJSON.rotz);

        GameObject p = GameObject.Find(posuserJSON.UserId) as GameObject;
        
            if (p != null)
            {
           // Debug.Log("WE ARE SEEING SOMEONE ELSE DO SOMETHING: " + posuserJSON.UserId.ToString());
            Transform ZeroDrone = p.transform.Find("ZeroDrone");
           // Debug.Log("WHAT " + ZeroDrone.name);
            ZeroDrone.GetComponent<Transform>().position = position;
            ZeroDrone.GetComponent<Transform>().rotation = rotation;
        }
      


                



            }
    /*
	

	void _OnPlayerShoot(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString ();
		ShootJSON shootJSON = ShootJSON.CreateFromJSON (data);
		GameObject p = GameObject.Find (userJSON.UserId);
		PlayerController pc = p.GetComponent<PlayerController>();
		pc.CmdFire ();
	}

	void _OnHealth(SocketIOEvent socketIOEvent)
	{
		print ("changeing the health");
		string data = socketIOEvent.data.ToString ();

		UserHealthJSON	userHealthJSON = UserHealthJSON.CreateFromJSON (data);
		GameObject p = GameObject.Find (userHealthJSON.name);
		PlayersHealth h = p.GetComponent<PlayersHealth> ();
		h.currentHealth = userHealthJSON.health;
		h.OnChangeHealth ();


	}*/

	void _OnPlayerDisconnect(SocketIOEvent socketIOEvent)
	{
		
		string data = socketIOEvent.data.ToString ();
        Debug.Log("OnPlayerDisconnect " + data);
        DCUserJSON dcuserJSON = DCUserJSON.CreateFromJSON (data);
        

            
                GameObject p = GameObject.Find(dcuserJSON.UserId) as GameObject;
                if (p != null)
                {
                    Destroy(GameObject.Find(dcuserJSON.UserId));
                  if (Data_Manager.Instance != null)
                  {
                    if (Data_Manager.Instance.GetUserId() == dcuserJSON.UserId.ToString())
                    {
                    this.Joincanvas.gameObject.SetActive(true);
                    this.FBcanvas.gameObject.SetActive(false);
                    this.cam.SetActive(true);
                    }
                  }
                }

	}

	private void Get_Data_Container_Info(GameObject p,UserJSON userJSON)
	{
        Transform ZeroDrone = p.transform.Find("ZeroDrone");
        Transform _Data_Container_Canvas = ZeroDrone.Find("Data_Container_Canvas");
		Debug.Log ("Data: Data_Container_Canvas: "+ _Data_Container_Canvas.name);

		Transform _Id = _Data_Container_Canvas.transform.Find ("Id");
		Debug.Log ("Data: Data_Container_Canvas: Id: "+ _Id.name);
		Text Id = _Id.GetComponent<Text> ();
		Id.text = "Id: "+userJSON.Id;

		Transform _UserId = _Data_Container_Canvas.transform.Find ("UserId");
		Debug.Log ("Data: Data_Container_Canvas: UserId: "+ _UserId.name);
		Text UserId = _UserId.GetComponent<Text> ();
		UserId.text = "UserId: "+userJSON.UserId;

		Transform _UserName = _Data_Container_Canvas.transform.Find ("UserName");
		Debug.Log ("Data: Data_Container_Canvas: UserName: "+ _UserName.name);
		Text UserName = _UserName.GetComponent<Text> ();
		UserName.text = "UserName: "+userJSON.UserName;

		Transform _UserHealth = _Data_Container_Canvas.transform.Find ("UserHealth");
		Debug.Log ("Data: Data_Container_Canvas: UserHealth: "+ _UserHealth.name);
		Text UserHealth = _UserHealth.GetComponent<Text> ();
		UserHealth.text = "UserHealth: "+userJSON.UserHealth;

		Transform _UserPower = _Data_Container_Canvas.transform.Find ("UserPower");
		Debug.Log ("Data: Data_Container_Canvas: UserPower: "+ _UserPower.name);
		Text UserPower = _UserPower.GetComponent<Text> ();
		UserPower.text = "UserPower: "+userJSON.UserPower;

		Transform _UserExpierance = _Data_Container_Canvas.transform.Find ("UserExpierance");
		Debug.Log ("Data: Data_Container_Canvas: UserExpierance: "+ _UserExpierance.name);
		Text UserExpierance = _UserExpierance.GetComponent<Text> ();
		UserExpierance.text = "UserExpierance: "+userJSON.UserExpierance;

		Transform _UserCurrency = _Data_Container_Canvas.transform.Find ("UserCurrency");
		Debug.Log ("Data: Data_Container_Canvas: UserCurrency: "+ _UserCurrency.name);
		Text UserCurrency = _UserCurrency.GetComponent<Text> ();
		UserCurrency.text = "UserCurrency: "+userJSON.UserCurrency;

		Transform _UserLevel = _Data_Container_Canvas.transform.Find ("UserLevel");
		Debug.Log ("Data: Data_Container_Canvas: UserLevel: "+ _UserLevel.name);
		Text UserLevel = _UserLevel.GetComponent<Text> ();
		UserLevel.text = "UserLevel: "+userJSON.UserLevel;

		Transform _UserPosX = _Data_Container_Canvas.transform.Find ("UserPosX");
		Debug.Log ("Data: Data_Container_Canvas: UserPosX: "+ _UserPosX.name);
		Text UserPosX = _UserPosX.GetComponent<Text> ();
		UserPosX.text = "UserPosX: "+userJSON.UserPosX;

		Transform _UserPosY = _Data_Container_Canvas.transform.Find ("UserPosY");
		Debug.Log ("Data: Data_Container_Canvas: UserPosY: "+ _UserPosY.name);
		Text UserPosY = _UserPosY.GetComponent<Text> ();
		UserPosY.text = "UserPosY: "+userJSON.UserPosY;

		Transform _UserPosZ = _Data_Container_Canvas.transform.Find ("UserPosZ");
		Debug.Log ("Data: Data_Container_Canvas: UserPosZ: "+ _UserPosZ.name);
		Text UserPosZ = _UserPosZ.GetComponent<Text> ();
		UserPosZ.text = "UserPosZ: "+userJSON.UserPosZ;

		Transform _UserGpsX = _Data_Container_Canvas.transform.Find ("UserGpsX");
		Debug.Log ("Data: Data_Container_Canvas: UserGpsX: "+ _UserGpsX.name);
		Text UserGpsX = _UserGpsX.GetComponent<Text> ();
		UserGpsX.text = "UserGpsX: "+userJSON.UserGpsX;

		Transform _UserGpsY = _Data_Container_Canvas.transform.Find ("UserGpsY");
		Debug.Log ("Data: Data_Container_Canvas: UserGpsY: "+ _UserGpsY.name);
		Text UserGpsY = _UserGpsY.GetComponent<Text> ();
		UserGpsY.text = "UserGpsY: "+userJSON.UserGpsY;

		Transform _UserGpsZ = _Data_Container_Canvas.transform.Find ("UserGpsZ");
		Debug.Log ("Data: Data_Container_Canvas: UserGpsZ: "+ _UserGpsZ.name);
		Text UserGpsZ = _UserGpsZ.GetComponent<Text> ();
		UserGpsZ.text = "UserGpsZ: "+userJSON.UserGpsZ;

		Transform _UserVungleApi = _Data_Container_Canvas.transform.Find ("UserVungleApi");
		Debug.Log ("Data: Data_Container_Canvas: UserVungleApi: "+ _UserVungleApi.name);
		Text UserVungleApi = _UserVungleApi.GetComponent<Text> ();
		UserVungleApi.text = "UserVungleApi: "+userJSON.UserVungleApi;

		Transform _UserAdcolonyApi = _Data_Container_Canvas.transform.Find ("UserAdcolonyApi");
		Debug.Log ("Data: Data_Container_Canvas: UserAdcolonyApi: "+ _UserAdcolonyApi.name);
		Text UserAdcolonyApi = _UserAdcolonyApi.GetComponent<Text> ();
		UserAdcolonyApi.text = "UserAdcolonyApi: "+userJSON.UserAdcolonyApi;

		Transform _UserAdcolonyZone = _Data_Container_Canvas.transform.Find ("UserAdcolonyZone");
		Debug.Log ("Data: Data_Container_Canvas: UserAdcolonyZone: "+ _UserAdcolonyZone.name);
		Text UserAdcolonyZone = _UserAdcolonyZone.GetComponent<Text> ();
		UserAdcolonyZone.text = "UserAdcolonyZone: "+userJSON.UserAdcolonyZone;



	}

    #endregion

    #region JSONMessageClasses

    [Serializable]
    public class DCPlayerJSON
    {
      public string UserId;
       public DCPlayerJSON(string _UserId)
        {
           UserId = _UserId;
        }
    }

    [Serializable]
    public class DCUserJSON
    {
        public string UserId;
        public static DCUserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<DCUserJSON>(data);
        }
    }


    [Serializable]
    public class POSPlayerJSON
    {
        public string UserId;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;
        public POSPlayerJSON(string _UserId,float _posx, float _posy, float _posz, float _rotx, float _roty, float _rotz)
        {
            UserId = _UserId;
            posx = _posx;
            posy = _posy;
            posz = _posz;
            rotx = _rotx;
            roty = _roty;
            rotz = _rotz;
        }
    }


    [Serializable]
    public class POSUserJSON
    {
        public string UserId;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;
        //public int health;
        public static POSUserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<POSUserJSON>(data);
        }
    }



    [Serializable]
	public class PlayerJSON
	{
		//public string name;
		public string Id;
		public string UserId;
		public string UserName;
		public string UserPic;
		public string UserToken;
		public string UserPosX;
		public string UserPosY;
		public string UserPosZ;
		public string UserLevel;
		public string UserCurrency;
		public string UserExpierance;
		public string UserHealth;
		public string UserPower;
		public string UserGpsX;
		public string UserGpsY;
		public string UserGpsZ;
		public string UserVungleApi;
		public string UserAdcolonyApi;
		public string UserAdcolonyZone;


		public List<PointJSON> playerSpawnPoints;
		public List<PointJSON> enemySpawnPoints;

		public PlayerJSON(
			string _Id,
			string _UserId,
			string _UserName,
			string _UserPic,
			string _UserToken,
			string _UserPosX,
			string _UserPosY,
			string _UserPosZ,
			string _UserLevel,
			string _UserCurrency,
			string _UserExpierance,
			string _UserHealth,
			string _UserPower,
			string _UserGpsX,
			string _UserGpsY,
			string _UserGpsZ,
			string _UserVungleApi,
			string _UserAdcolonyApi,
			string _UserAdcolonyZone,
			List<SpawnPoints> _playerSpawnPoints,
			List<SpawnPoints> _enemySpawnPoints )
		{
			playerSpawnPoints = new List<PointJSON>();
			enemySpawnPoints = new List<PointJSON>();

			Id = _Id;
			UserId = _UserId;
			UserName = _UserName;
			UserPic = _UserPic;
			UserToken = _UserToken;
			UserPosX = _UserPosX;
			UserPosY = _UserPosY;
			UserPosZ = _UserPosZ;
			UserLevel = _UserLevel;
			UserCurrency = _UserCurrency;
			UserExpierance = _UserExpierance;
			UserHealth = _UserHealth;
			UserPower = _UserPower;
			UserGpsX = _UserGpsX;
			UserGpsY = _UserGpsY;
			UserGpsZ = _UserGpsZ;
			UserVungleApi = _UserVungleApi;
			UserAdcolonyApi = _UserAdcolonyApi;
			UserAdcolonyZone = _UserAdcolonyZone;

			foreach(SpawnPoints playerSpawnPoint in _playerSpawnPoints)
			{
				PointJSON pointJSON = new PointJSON (playerSpawnPoint);
				playerSpawnPoints.Add(pointJSON);
			}
			foreach(SpawnPoints enemySpawnPoint in _enemySpawnPoints)
			{
				PointJSON pointJSON = new PointJSON (enemySpawnPoint);
				enemySpawnPoints.Add(pointJSON);
			}
		}

	}

	[Serializable]
	public class PointJSON
	{
		public float[] position;
		public float[] rotation;
		public PointJSON(SpawnPoints spawnPoint)
		{
			position = new float[]{
				spawnPoint.transform.position.x,
				spawnPoint.transform.position.z,
				spawnPoint.transform.position.z
			};
			rotation = new float[]{
				spawnPoint.transform.eulerAngles.x,
				spawnPoint.transform.eulerAngles.z,
				spawnPoint.transform.eulerAngles.z
			};
		}
	}

	[Serializable]
	public class PositionJSON
	{
		public float[] position;
		public PositionJSON(Vector3 _position)
		{
			position = new float[]{_position.x,_position.y,_position.z};

		}
	}

	[Serializable]
	public class RotationJSON
	{
		public float[] rotation;
		public RotationJSON(Quaternion _rotation)
		{
			rotation = new float[]{_rotation.eulerAngles.x,_rotation.eulerAngles.y,_rotation.eulerAngles.z};

		}
	}

	[Serializable]
	public class UserJSON
	{
		public string Id;
		public string UserId;
		public string UserName;
		public string UserPic;
		public string UserToken;
		public string UserPosX;
		public string UserPosY;
		public string UserPosZ;
		public string UserLevel;
		public string UserCurrency;
		public string UserExpierance;
		public string UserHealth;
		public string UserPower;
		public string UserGpsX;
		public string UserGpsY;
		public string UserGpsZ;
		public string UserVungleApi;
		public string UserAdcolonyApi;
		public string UserAdcolonyZone;
		public float[] position;
		public float[] rotation;
		//public int health;
		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON> (data);
		}
	}

	[Serializable]
	public class HealthChangeJSON
	{
		public string name;
		public int healthChange;
		public string from;
		public bool isEnemy;
		public HealthChangeJSON(string _name, int _healthChange, string _from, bool _isEnemy)
		{
			name = _name;
			healthChange = _healthChange;
			from = _from;
			isEnemy = _isEnemy;
		}
	}

	[Serializable]
	public class EnemiesJSON
	{
		public List<UserJSON> enemies;
		public static EnemiesJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<EnemiesJSON> (data);
		}
	}

	[Serializable]
	public class ShootJSON
	{
		public string name;
		public static ShootJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<ShootJSON> (data);
		}
	}

	[Serializable]
	public class UserHealthJSON
	{
		public string name;
		public int health;

		public static UserHealthJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserHealthJSON> (data);
		}

	}

	#endregion
}
