using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SocketIO;
using System.Net.Sockets;

public class NetworkManager : MonoBehaviour
{

    private static NetworkManager instance;
    public static NetworkManager Instance { get { return instance; } }
    public Canvas FBcanvas;
    public Canvas Joincanvas;
    public Canvas ServerStatuscanvas;
    public Canvas DataCollectioncanvas;
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
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        CheckServerStatus();


    }
    private bool Load_Once = false;
    void Load_Core()
    {
        //subscribe to all the websocket events
        //socket.On("OnEnemies",_OnEnemies);
        socket.On("OnPlayerConnected", _OnPlayerConnected);
        //socket.On("OnPlay",_OnPlay);
        socket.On("OnPlayerMove", _OnPlayerMove);
        socket.On("OnObjectMove", _OnObjectMove);

        socket.On("OnPlayerShoot", _OnPlayerShoot);
        //socket.On("OnHealth",_OnHealth);
        socket.On("OnPlayerDisconnect", _OnPlayerDisconnect);
        Load_Once = true;


    }
    int check = 0;
    private void CheckServerStatus()
    {

        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect(GetIPAddress, GetPort);
            //Debug.Log("Port open");
            if (hasbeenDC == true && hasjoined == false && userIsConnected == false)
            {
                JoinGame();
            }
            ServerStatuscanvas.gameObject.SetActive(false);
        }
        catch (Exception)
        {
            // Debug.Log("Port closed");
            if (hasjoined == true)
            {
                hasbeenDC = true;
                ServerStatuscanvas.gameObject.SetActive(true);
            }
            hasjoined = false;
            userIsConnected = false;

        }
        //Debug.Log("Check Server: " + check);

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

        // Debug.Log("hasbeenDC ?: " + hasbeenDC);
        //Debug.Log("hasjoined ?: " + hasjoined);
        // Debug.Log("userIsConnected ?: " + userIsConnected);


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

    public void AutoSavePlayer()
    {
        if (Data_Manager.Instance != null && Data_Manager.Instance.UserId.text != "UserId" && hasjoined == true)
        {
            // Debug.Log("Lets Save The Players Details");
            if (MysqlManager.Instance != null)
            {
                MysqlManager.Instance.SaveUsersData(Data_Manager.Instance.GetUserId(),
                    Data_Manager.Instance.GetUserName(), Data_Manager.Instance.GetUserToken(),
                    Data_Manager.Instance.GetUserPosX(), Data_Manager.Instance.GetUserPosY(),
                    Data_Manager.Instance.GetUserPosZ(), Data_Manager.Instance.GetUserLevel(),
                    Data_Manager.Instance.GetUserCurrency(), Data_Manager.Instance.GetUserExpierance(),
                     Data_Manager.Instance.GetUserHealth(), Data_Manager.Instance.GetUserPower(),
                      Data_Manager.Instance.GetUserGpsX(), Data_Manager.Instance.GetUserGpsY(),
                      Data_Manager.Instance.GetUserGpsZ(), Data_Manager.Instance.GetUserRotX(),
                      Data_Manager.Instance.GetUserRotY(), Data_Manager.Instance.GetUserRotZ());
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
            // Debug.Log("Application ending after 3 " + Time.time + " seconds");
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
        yield return new WaitForSeconds(0.5f);

        //socket.Emit ("player connect");
        yield return new WaitForSeconds(1f);
        //string playerName = playerNameInput.text;
        List<SpawnPoints> playSpawnPoints = GetComponent<PlayersSpawnPoint>().playerSpawnPoints;
        List<SpawnPoints> enemySpawnPoints = GetComponent<EnemySpawnPoint>().enemySpawnPoionts;
        PlayerJSON playerJSON = new PlayerJSON(
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
            Data_Manager.Instance.GetUserRotX(),
            Data_Manager.Instance.GetUserRotY(),
            Data_Manager.Instance.GetUserRotZ(),
            playSpawnPoints,
            enemySpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("OnConnection", new JSONObject(data));

    }



    public void CommandMoveObject(string ObjectId, float posx, float posy, float posz, float rotx, float roty, float rotz)
    {

        POSObjectJSON posobjectJSON = new POSObjectJSON(ObjectId, posx, posy, posz, rotx, roty, rotz);
        string data = JsonUtility.ToJson(posobjectJSON);
        socket.Emit("OnObjectMove", new JSONObject(data));


    }


    public void CommandMove(bool isMoving, bool isBoost)
    {

        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
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

                    POSPlayerJSON posplayerJSON = new POSPlayerJSON(Data_Manager.Instance.GetUserId(), posx, posy, posz, rotx, roty, rotz, isMoving, isBoost);
                    string data = JsonUtility.ToJson(posplayerJSON);
                    socket.Emit("OnPlayerMove", new JSONObject(data));
                }
            }
        }

    }






    public void CommandShoot()
    {


        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {
                    ShootPlayerJSON shootplayerJSON = new ShootPlayerJSON(Data_Manager.Instance.GetUserId());
                    string data = JsonUtility.ToJson(shootplayerJSON);
                    socket.Emit("OnPlayerShoot", new JSONObject(data));
                }
            }
        }

    }

    public void ClientDissconnect()
    {

        if (Data_Manager.Instance != null)
        {
            if (NetworkManager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == NetworkManager.Instance.GetUserId())
                {
                    DCPlayerJSON dcplayerJSON = new DCPlayerJSON(
                    Data_Manager.Instance.GetUserId());
                    string data = JsonUtility.ToJson(dcplayerJSON);
                    socket.Emit("OnPlayerDisconnect", new JSONObject(data));
                    AutoSavePlayer();//if you want the user to lose the player prefs when they logout so they have to login again
                    //delete else let the device know who they are so they never have to login again they can just go directly to the game.
                    //PlayerPrefs.DeleteAll();
                }
            }
        }

        //  print("ClientDissconnect " + data);

    }

    public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange, bool isEnemy)
    {
        // print("health change cmd");
        HealthChangeJSON healthChangeJSON = new HealthChangeJSON(playerTo.name, healthChange, playerFrom.name, isEnemy);
        socket.Emit("health", new JSONObject(JsonUtility.ToJson(healthChangeJSON)));

    }
    #endregion

    #region Listening

    void _OnEnemies(SocketIOEvent socketIOEvent)
    {
        EnemiesJSON enemiesJSON = EnemiesJSON.CreateFromJSON(socketIOEvent.data.ToString());
        EnemySpawnPoint es = GetComponent<EnemySpawnPoint>();
        es.SpawnEnemys(enemiesJSON);
    }

    void _OnPlayerConnected(SocketIOEvent socketIOEvent)
    {

        //Debug.Log ("Method: _OnPlayerConnected");
        //Debug.Log ("Client Recieved:");
        string data = socketIOEvent.data.ToString();
        //Debug.Log ("Data_Manager:"+ data);

        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        //Debug.Log ("Data: Id: "+ userJSON.Id);

        GameObject o = GameObject.Find(userJSON.UserId) as GameObject;
        if (o != null)
        {
            hasjoined = true;
            userIsConnected = true;
            hasbeenDC = false;
            Debug.Log("Debug: _OnPlayerConnected Player Is Already Online:" + o.name);
            return;
        }
        float gpsx;
        float gpsy;
        float gpsz;
        float.TryParse(userJSON.UserGpsX.ToString(), out gpsx);
        float.TryParse(userJSON.UserGpsY.ToString(), out gpsy);
        float.TryParse(userJSON.UserGpsZ.ToString(), out gpsz);

        //ok so lets think about this a bit more..
        // we need to instantiate the players ID as the containor 
        //once we instanitate the containor we need to then instantiate the players ship ZeroDrone_ID
        // once we have the drone differnet from all the other drones we can then know what drone is what.
        //same for the players untiverse_ID that way each untivers will be different 

        //This is where we spawn the postion p0int for all players at 000 center point so all controllers are in position 
        GameObject p = Instantiate(player, new Vector3(0, 0, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
        p.name = userJSON.UserId;








        //search all the contents of the prefab to rename and copy as new players details
        Transform[] allChildren = p.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
             //Debug.Log("WHAT IS THE NAME OF THE CHILDREN?: " + child.name);

            if (child.name == "ZeroDrone")
            {
                // Debug.Log("YES THIS IS ZREO DRONE?: " + child.name);
                float shipposx;
                float shipposy;
                float shipposz;
                float.TryParse(userJSON.UserPosX.ToString(), out shipposx);
                float.TryParse(userJSON.UserPosY.ToString(), out shipposy);
                float.TryParse(userJSON.UserPosZ.ToString(), out shipposz);



                float shiprotx;
                float shiproty;
                float shiprotz;
                float.TryParse(userJSON.UserRotY.ToString(), out shiprotx);
                float.TryParse(userJSON.UserRotY.ToString(), out shiproty);
                float.TryParse(userJSON.UserRotZ.ToString(), out shiprotz);

                GameObject MyZreoDrone = Instantiate(child.gameObject, new Vector3(shipposx, shipposy, shipposz), Quaternion.Euler(shiprotx, shiproty, shiprotz)) as GameObject;
                MyZreoDrone.name = "ZeroDrone_" + userJSON.UserId;
                MyZreoDrone.SetActive(true);
                Destroy(child.gameObject);
                MyZreoDrone.transform.parent = p.transform;
                MyZreoDrone.transform.SetParent(p.transform, false);
                // child.name = "ZeroDrone_" + Data_Manager.Instance.GetUserId();
            }

            if (child.name == "Universe")
            {
                //Debug.Log("YES THIS IS MY UNTIVERS?: " + child.name);

                GameObject MyUntiverse = Instantiate(child.gameObject, new Vector3(gpsx, gpsy, gpsz), Quaternion.Euler(gpsx, gpsy, gpsz)) as GameObject;
                MyUntiverse.name = "Universe_" + userJSON.UserId;
                MyUntiverse.transform.localScale += new Vector3(child.transform.localScale.x, child.transform.localScale.y, child.transform.localScale.z);
                MyUntiverse.SetActive(true);

                Transform[] allUChildren = MyUntiverse.GetComponentsInChildren<Transform>();
                foreach (Transform Uchild in allUChildren)
                {
                    if(Uchild.name == "HomePoint")
                    {
                        GameObject HomePoint = Instantiate(Uchild.gameObject, new Vector3(Uchild.transform.position.x, Uchild.transform.position.y, Uchild.transform.position.z), Quaternion.Euler(Uchild.transform.rotation.eulerAngles.x, Uchild.transform.rotation.eulerAngles.y, Uchild.transform.rotation.eulerAngles.z)) as GameObject;
                        HomePoint.name = "HomePoint_" + userJSON.UserId;
                        HomePoint.transform.localScale += new Vector3(Uchild.transform.localScale.x, Uchild.transform.localScale.y, Uchild.transform.localScale.z);
                        HomePoint.SetActive(true);
                        Uchild.gameObject.SetActive(false);
                        HomePoint.transform.parent = MyUntiverse.transform;
                        HomePoint.transform.SetParent(MyUntiverse.transform, false);
                    }

                    if (Uchild.name == "Planets")
                    {
                        GameObject Planets = Instantiate(Uchild.gameObject, new Vector3(Uchild.transform.position.x, Uchild.transform.position.y, Uchild.transform.position.z), Quaternion.Euler(Uchild.transform.rotation.eulerAngles.x, Uchild.transform.rotation.eulerAngles.y, Uchild.transform.rotation.eulerAngles.z)) as GameObject;
                        Planets.name = "Planets_" + userJSON.UserId;
                        Planets.transform.localScale += new Vector3(Uchild.transform.localScale.x, Uchild.transform.localScale.y, Uchild.transform.localScale.z);
                        Planets.SetActive(true);

                        Transform[] allPChildren = Planets.GetComponentsInChildren<Transform>();
                        foreach (Transform Pchild in allPChildren)
                        {

                            //ok so we now know every users prants they buy from the store and IDs, they can spawn there unitverse
                            if (Pchild.name == "Sun")
                            {
                                GameObject Sun = Instantiate(Pchild.gameObject, new Vector3(Pchild.transform.position.x, Pchild.transform.position.y, Pchild.transform.position.z), Quaternion.Euler(Pchild.transform.rotation.eulerAngles.x, Pchild.transform.rotation.eulerAngles.y, Pchild.transform.rotation.eulerAngles.z)) as GameObject;
                                Sun.name = "Sun_" + userJSON.UserId;
                                Sun.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                                Sun.SetActive(true);
                                Pchild.gameObject.SetActive(false);
                                Sun.transform.parent = Planets.transform;
                                Sun.transform.SetParent(Planets.transform, false);
                            }

                            if (Pchild.name == "Alert_Detection")
                            {
                                GameObject Alert_Detection = Instantiate(Pchild.gameObject, new Vector3(Pchild.transform.position.x, Pchild.transform.position.y, Pchild.transform.position.z), Quaternion.Euler(Pchild.transform.rotation.eulerAngles.x, Pchild.transform.rotation.eulerAngles.y, Pchild.transform.rotation.eulerAngles.z)) as GameObject;
                                Alert_Detection.name = "Alert_Detection_" + userJSON.UserId;
                                Alert_Detection.transform.localScale += new Vector3(7, 7, 7);
                                Alert_Detection.SetActive(true);
                                Pchild.gameObject.SetActive(false);
                                Alert_Detection.transform.parent = Planets.transform;
                                Alert_Detection.transform.SetParent(Planets.transform, false);
                            }

                            
                        }



                           Uchild.gameObject.SetActive(false);
                        Planets.transform.parent = MyUntiverse.transform;
                        Planets.transform.SetParent(MyUntiverse.transform, false);
                    }


                    if (Uchild.name == "HomeWayPoints")
                    {
                        GameObject HomeWayPoints = Instantiate(Uchild.gameObject, new Vector3(Uchild.transform.position.x, Uchild.transform.position.y, Uchild.transform.position.z), Quaternion.Euler(Uchild.transform.rotation.eulerAngles.x, Uchild.transform.rotation.eulerAngles.y, Uchild.transform.rotation.eulerAngles.z)) as GameObject;
                        HomeWayPoints.name = "HomeWayPoints_" + userJSON.UserId;
                        HomeWayPoints.transform.localScale += new Vector3(Uchild.transform.localScale.x, Uchild.transform.localScale.y, Uchild.transform.localScale.z);
                        HomeWayPoints.SetActive(true);
                        Uchild.gameObject.SetActive(false);
                        HomeWayPoints.transform.parent = MyUntiverse.transform;
                        HomeWayPoints.transform.SetParent(MyUntiverse.transform, false);
                    }

                    if (Uchild.name == "AstoridWayPoints")
                    {
                        GameObject AstoridWayPoints = Instantiate(Uchild.gameObject, new Vector3(Uchild.transform.position.x, Uchild.transform.position.y, Uchild.transform.position.z), Quaternion.Euler(Uchild.transform.rotation.eulerAngles.x, Uchild.transform.rotation.eulerAngles.y, Uchild.transform.rotation.eulerAngles.z)) as GameObject;
                        AstoridWayPoints.name = "AstoridWayPoints_" + userJSON.UserId;
                        AstoridWayPoints.transform.localScale += new Vector3(Uchild.transform.localScale.x, Uchild.transform.localScale.y, Uchild.transform.localScale.z);
                        AstoridWayPoints.SetActive(true);
                        Uchild.gameObject.SetActive(false);
                        AstoridWayPoints.transform.parent = MyUntiverse.transform;
                        AstoridWayPoints.transform.SetParent(MyUntiverse.transform, false);
                    }
                }
                    child.gameObject.SetActive(false);
                MyUntiverse.transform.parent = p.transform;
                MyUntiverse.transform.SetParent(p.transform, false);
            }

           


           

        }


        if (Data_Manager.Instance != null)
        {

            if (Data_Manager.Instance.GetUserId() == userJSON.UserId.ToString())
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


    void _OnObjectMove(SocketIOEvent socketIOEvent)
    {
        //Debug.Log ("Method: _OnPlayerMove");
        //Debug.Log ("Client Recieved:");
        string data = socketIOEvent.data.ToString();
        //Debug.Log ("Data_Manager:"+ data);

        POSMoveObjectJSON moveposobjectJSON = POSMoveObjectJSON.CreateFromJSON(data);
        Debug.Log("Data: ObjectId: " + moveposobjectJSON.ObjectId);
        //Debug.Log("Data: PosX: " + moveposobjectJSON.posx);
        //Debug.Log("Data: PosY: " + moveposobjectJSON.posy);
        //Debug.Log("Data: PosZ: " + moveposobjectJSON.posz);
        //Debug.Log("Data: RotX: " + moveposobjectJSON.rotx);
        //Debug.Log("Data: RotY: " + moveposobjectJSON.roty);
        //Debug.Log("Data: RotZ: " + moveposobjectJSON.rotz);





        Vector3 position = new Vector3(moveposobjectJSON.posx, moveposobjectJSON.posy, moveposobjectJSON.posz);
        Quaternion rotation = Quaternion.Euler(moveposobjectJSON.rotx, moveposobjectJSON.roty, moveposobjectJSON.rotz);


        GameObject MovedObject = GameObject.Find(moveposobjectJSON.ObjectId) as GameObject;

        if (MovedObject != null)
        {
            MovedObject.GetComponent<Transform>().position = position;
            MovedObject.GetComponent<Transform>().rotation = rotation;
        }

    }



    void _OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        //Debug.Log ("Method: _OnPlayerMove");
        //Debug.Log ("Client Recieved:");
        string data = socketIOEvent.data.ToString();
        //Debug.Log ("Data_Manager:"+ data);

        POSUserJSON posuserJSON = POSUserJSON.CreateFromJSON(data);
        //Debug.Log("Data: UserId: " + posuserJSON.UserId);
        //Debug.Log("Data: PosX: " + posuserJSON.posx);
        //Debug.Log("Data: PosY: " + posuserJSON.posy);
        //Debug.Log("Data: PosZ: " + posuserJSON.posz);
        //Debug.Log("Data: RotX: " + posuserJSON.rotx);
        //Debug.Log("Data: RotY: " + posuserJSON.roty);
        //Debug.Log("Data: RotZ: " + posuserJSON.rotz);

       
        Vector3 position = new Vector3(posuserJSON.posx, posuserJSON.posy, posuserJSON.posz);
        Quaternion rotation = Quaternion.Euler(posuserJSON.rotx, posuserJSON.roty, posuserJSON.rotz);


        GameObject ZeroDrone = GameObject.Find("ZeroDrone_" + posuserJSON.UserId) as GameObject;

        if (ZeroDrone != null)
        {
            // Debug.Log("WE ARE SEEING SOMEONE ELSE DO SOMETHING: " + posuserJSON.UserId.ToString());




            ZeroDrone.GetComponent<Transform>().position = position;
            ZeroDrone.GetComponent<Transform>().rotation = rotation;
            GameObject ThrustLight2 = ZeroDrone.transform.GetChild(0).GetChild(0).gameObject;
            GameObject ThrustLight1 = ZeroDrone.transform.GetChild(1).GetChild(0).gameObject;

            // Debug.Log("WHAT ThrustLight2 " + ThrustLight2.name);
            //  Debug.Log("WHAT USER ID " + posuserJSON.UserId + " WHAT BOOST " + posuserJSON.isBoost);
            if (posuserJSON.isBoost == true)
            {
                ThrustLight1.gameObject.SetActive(true);
                ThrustLight2.gameObject.SetActive(true);
            }
            if (posuserJSON.isBoost == false)
            {
                ThrustLight1.gameObject.SetActive(false);
                ThrustLight2.gameObject.SetActive(false);
            }


        }

    }





    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Bullet;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float Bullet_Forward_Force;


    void _OnPlayerShoot(SocketIOEvent socketIOEvent)
    {
        // Debug.Log("Method: _OnPlayerShoot");
        // Debug.Log("Client Recieved:");
        string data = socketIOEvent.data.ToString();
        Debug.Log("Data_Manager:" + data);
        ShootUserJSON shootuserJSON = ShootUserJSON.CreateFromJSON(data);


        //fromme
        GameObject FromWhatWho = GameObject.Find("ZeroDrone_" + shootuserJSON.UserId) as GameObject;
        if (FromWhatWho != null)
        {
            //this is who shot the user we can now now where the hit came from
            //Debug.Log("WHO FIRED THE SHOT? :" + FromWhatWho.name);
            //Debug.Log("WHO FIRED AT WHAT POS? :" + FromWhatWho.transform.position);
           // Debug.Log("WHO FIRED AT WHAT ROT? :" + FromWhatWho.transform.rotation);



            GameObject Bullet_Emitter = FromWhatWho.transform.GetChild(5).GetChild(0).gameObject;
            Debug.Log("WHAT IS THIS 1 " + Bullet_Emitter.name);

            Debug.Log("WARP STATUS: " + FromWhatWho.GetComponent<MoveShip>().isBoost);


            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            Temporary_Bullet_Handler.transform.rotation = FromWhatWho.transform.rotation;

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();


            if (FromWhatWho.GetComponent<MoveShip>().isBoost == true)
            {
                Debug.Log("WHO FIRED IN WARP? :" + FromWhatWho.name);
                //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
                Temporary_RigidBody.AddForce(FromWhatWho.transform.forward * Bullet_Forward_Force * 10);
            }
            else
            {
                //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
                Temporary_RigidBody.AddForce(FromWhatWho.transform.forward * Bullet_Forward_Force);
            }

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_Handler, 10.0f);
        }

        Debug.Log("WE FIRED: " + shootuserJSON.UserId);








    }
    /*
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

        string data = socketIOEvent.data.ToString();
        Debug.Log("OnPlayerDisconnect " + data);
        DCUserJSON dcuserJSON = DCUserJSON.CreateFromJSON(data);

        GameObject p = GameObject.Find(dcuserJSON.UserId) as GameObject;
        if (p != null)
        {
           
            if (Data_Manager.Instance != null)
            {
                if (Data_Manager.Instance.GetUserId() == dcuserJSON.UserId.ToString())
                {
                    this.hasjoined = false;
                    this.userIsConnected = false;
                    this.hasbeenDC = false;
                    this.Joincanvas.gameObject.SetActive(true);
                    this.FBcanvas.gameObject.SetActive(false);
                    this.cam.SetActive(true);
                    this.DataCollectioncanvas.gameObject.SetActive(false);
                    Destroy(p);
                }
            }
        }

    }




    private void Get_Data_Container_Info(GameObject p, UserJSON userJSON)
    {
        Transform ZeroDrone = p.transform.Find("ZeroDrone_" + userJSON.UserId);

        GameObject _Data_Container_Canvas_Healthbar = ZeroDrone.transform.GetChild(6).GetChild(0).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_Healthbar: " + _Data_Container_Canvas_Healthbar.name);

        GameObject _Data_Container_Canvas_Powerbar = ZeroDrone.transform.GetChild(6).GetChild(1).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_Powerbar: " + _Data_Container_Canvas_Powerbar.name);

        GameObject _Data_Container_Canvas_UserId = ZeroDrone.transform.GetChild(6).GetChild(2).gameObject;
        //  Debug.Log("MyData: Data_Container_Canvas_UserId: " + _Data_Container_Canvas_UserId.name);
        Text UserId = _Data_Container_Canvas_UserId.GetComponent<Text>();
        UserId.text = "UserId: " + userJSON.UserId;

        GameObject _Data_Container_Canvas_Id = ZeroDrone.transform.GetChild(6).GetChild(3).gameObject;
        //  Debug.Log("MyData: Data_Container_Canvas_Id: " + _Data_Container_Canvas_Id.name);
        Text Id = _Data_Container_Canvas_Id.GetComponent<Text>();
        Id.text = "Id: " + userJSON.Id;

        GameObject _Data_Container_Canvas_UserName = ZeroDrone.transform.GetChild(6).GetChild(4).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserName: " + _Data_Container_Canvas_UserName.name);
        Text UserName = _Data_Container_Canvas_UserName.GetComponent<Text>();
        UserName.text = "UserName: " + userJSON.UserName;

        GameObject _Data_Container_Canvas_UserHealth = ZeroDrone.transform.GetChild(6).GetChild(5).gameObject;
        //  Debug.Log("MyData: Data_Container_Canvas_UserHealth: " + _Data_Container_Canvas_UserHealth.name);
        Text UserHealth = _Data_Container_Canvas_UserHealth.GetComponent<Text>();
        UserHealth.text = "UserHealth: " + userJSON.UserHealth;

        GameObject _Data_Container_Canvas_UserPower = ZeroDrone.transform.GetChild(6).GetChild(6).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserPower: " + _Data_Container_Canvas_UserPower.name);
        Text UserPower = _Data_Container_Canvas_UserPower.GetComponent<Text>();
        UserPower.text = "UserPower: " + userJSON.UserPower;

        GameObject _Data_Container_Canvas_UserExpierance = ZeroDrone.transform.GetChild(6).GetChild(7).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserExpierance: " + _Data_Container_Canvas_UserExpierance.name);
        Text UserExpierance = _Data_Container_Canvas_UserExpierance.GetComponent<Text>();
        UserExpierance.text = "UserExpierance: " + userJSON.UserExpierance;

        GameObject _Data_Container_Canvas_UserCurrency = ZeroDrone.transform.GetChild(6).GetChild(8).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserCurrency: " + _Data_Container_Canvas_UserCurrency.name);
        Text UserCurrency = _Data_Container_Canvas_UserCurrency.GetComponent<Text>();
        UserCurrency.text = "UserCurrency: " + userJSON.UserCurrency;

        GameObject _Data_Container_Canvas_UserLevel = ZeroDrone.transform.GetChild(6).GetChild(9).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserLevel: " + _Data_Container_Canvas_UserLevel.name);
        Text UserLevel = _Data_Container_Canvas_UserLevel.GetComponent<Text>();
        UserLevel.text = "UserLevel: " + userJSON.UserLevel;

        GameObject _Data_Container_Canvas_UserPosX = ZeroDrone.transform.GetChild(6).GetChild(10).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserPosX: " + _Data_Container_Canvas_UserPosX.name);
        Text UserPosX = _Data_Container_Canvas_UserPosX.GetComponent<Text>();
        UserPosX.text = "UserPosX: " + userJSON.UserPosX;

        GameObject _Data_Container_Canvas_UserPosY = ZeroDrone.transform.GetChild(6).GetChild(11).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserPosY: " + _Data_Container_Canvas_UserPosY.name);
        Text UserPosY = _Data_Container_Canvas_UserPosY.GetComponent<Text>();
        UserPosY.text = "UserPosY: " + userJSON.UserPosY;

        GameObject _Data_Container_Canvas_UserPosZ = ZeroDrone.transform.GetChild(6).GetChild(12).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserPosZ: " + _Data_Container_Canvas_UserPosZ.name);
        Text UserPosZ = _Data_Container_Canvas_UserPosZ.GetComponent<Text>();
        UserPosZ.text = "UserPosZ: " + userJSON.UserPosZ;

        GameObject _Data_Container_Canvas_UserGpsX = ZeroDrone.transform.GetChild(6).GetChild(13).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserGpsX: " + _Data_Container_Canvas_UserGpsX.name);
        Text UserGpsX = _Data_Container_Canvas_UserGpsX.GetComponent<Text>();
        UserGpsX.text = "UserGpsX: " + userJSON.UserGpsX;

        GameObject _Data_Container_Canvas_UserGpsY = ZeroDrone.transform.GetChild(6).GetChild(14).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserGpsY: " + _Data_Container_Canvas_UserGpsY.name);
        Text UserGpsY = _Data_Container_Canvas_UserGpsY.GetComponent<Text>();
        UserGpsY.text = "UserGpsY: " + userJSON.UserGpsY;

        GameObject _Data_Container_Canvas_UserGpsZ = ZeroDrone.transform.GetChild(6).GetChild(15).gameObject;
        //  Debug.Log("MyData: Data_Container_Canvas_UserGpsZ: " + _Data_Container_Canvas_UserGpsZ.name);
        Text UserGpsZ = _Data_Container_Canvas_UserGpsZ.GetComponent<Text>();
        UserGpsZ.text = "UserGpsZ: " + userJSON.UserGpsZ;

        GameObject _Data_Container_Canvas_UserVungleApi = ZeroDrone.transform.GetChild(6).GetChild(16).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserVungleApi: " + _Data_Container_Canvas_UserVungleApi.name);
        Text UserVungleApi = _Data_Container_Canvas_UserVungleApi.GetComponent<Text>();
        UserVungleApi.text = "UserVungleApi: " + userJSON.UserVungleApi;

        GameObject _Data_Container_Canvas_UserAdcolonyApi = ZeroDrone.transform.GetChild(6).GetChild(17).gameObject;
        //  Debug.Log("MyData: Data_Container_Canvas_UserAdcolonyApi: " + _Data_Container_Canvas_UserAdcolonyApi.name);
        Text UserAdcolonyApi = _Data_Container_Canvas_UserAdcolonyApi.GetComponent<Text>();
        UserAdcolonyApi.text = "UserAdcolonyApi: " + userJSON.UserAdcolonyApi;

        GameObject _Data_Container_Canvas_UserAdcolonyZone = ZeroDrone.transform.GetChild(6).GetChild(18).gameObject;
        // Debug.Log("MyData: Data_Container_Canvas_UserAdcolonyZone: " + _Data_Container_Canvas_UserAdcolonyZone.name);
        Text UserAdcolonyZone = _Data_Container_Canvas_UserAdcolonyZone.GetComponent<Text>();
        UserAdcolonyZone.text = "UserAdcolonyZone: " + userJSON.UserAdcolonyZone;

        //todo add the rotation text on the ship world space


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
    public class ShootPlayerJSON
    {
        public string UserId;


        public ShootPlayerJSON(string _UserId)
        {
            UserId = _UserId;
        }
    }

    [Serializable]
    public class ShootUserJSON
    {
        public string UserId;
        public static ShootUserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<ShootUserJSON>(data);
        }
    }


    [Serializable]
    public class POSObjectJSON
    {
        public string ObjectsId;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;

        public POSObjectJSON(string _ObjectsId, float _posx, float _posy, float _posz, float _rotx, float _roty, float _rotz)
        {
            ObjectsId = _ObjectsId;
            posx = _posx;
            posy = _posy;
            posz = _posz;
            rotx = _rotx;
            roty = _roty;
            rotz = _rotz;

        }
    }

    [Serializable]
    public class POSMoveObjectJSON
    {
        public string ObjectId;
        public float posx;
        public float posy;
        public float posz;
        public float rotx;
        public float roty;
        public float rotz;

        //public int health;
        public static POSMoveObjectJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<POSMoveObjectJSON>(data);
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
        public bool isMoving;
        public bool isBoost;
        public POSPlayerJSON(string _UserId, float _posx, float _posy, float _posz, float _rotx, float _roty, float _rotz, bool _isMoving, bool _isBoost)
        {
            UserId = _UserId;
            posx = _posx;
            posy = _posy;
            posz = _posz;
            rotx = _rotx;
            roty = _roty;
            rotz = _rotz;
            isMoving = _isMoving;
            isBoost = _isBoost;
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
        public bool isMoving;
        public bool isBoost;
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
        public string UserRotX;
        public string UserRotY;
        public string UserRotZ;


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
            string _UserRotX,
            string _UserRotY,
            string _UserRotZ,
            List<SpawnPoints> _playerSpawnPoints,
            List<SpawnPoints> _enemySpawnPoints)
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
            UserRotX = _UserRotX;
            UserRotY = _UserRotY;
            UserRotZ = _UserRotZ;

            foreach (SpawnPoints playerSpawnPoint in _playerSpawnPoints)
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }
            foreach (SpawnPoints enemySpawnPoint in _enemySpawnPoints)
            {
                PointJSON pointJSON = new PointJSON(enemySpawnPoint);
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
            position = new float[] { _position.x, _position.y, _position.z };

        }
    }

    [Serializable]
    public class RotationJSON
    {
        public float[] rotation;
        public RotationJSON(Quaternion _rotation)
        {
            rotation = new float[] { _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z };

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
        public string UserRotX;
        public string UserRotY;
        public string UserRotZ;
        //public int health;
        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
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
            return JsonUtility.FromJson<EnemiesJSON>(data);
        }
    }

    [Serializable]
    public class ShootJSON
    {
        public string name;
        public static ShootJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<ShootJSON>(data);
        }
    }

    [Serializable]
    public class UserHealthJSON
    {
        public string name;
        public int health;

        public static UserHealthJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserHealthJSON>(data);
        }

    }

    #endregion
}
