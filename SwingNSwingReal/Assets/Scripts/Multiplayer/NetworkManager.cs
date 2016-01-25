using UnityEngine;
using System.Collections;
public class NetworkManager : MonoBehaviour {

	//Name of game on unity server
	private const string typeName = "SwingNSwing";
	//name of first room created by host
	private const string gameName = "Room 1";

	//create a conncetion to server on unity
	private void StartServer()
	{
		Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	//when server is created send a message to us saying we did it
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}
	//create a hostdata type to hold room information
	private HostData[] hostList;
	//request a host
	private void RefreshHostList()
	{
		MasterServer.RequestHostList (typeName);
	}
	//if the host list is recieved from the unity server this function automatically runs
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		//if the event is a hostlist received then we will store it in our host file
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}
	//join the hosted server
	private void JoinServer(HostData hostData)
	{
		Debug.Log("you hit the button for room 1");
		Network.Connect(hostData);
	}
	//send message to us if our connection worked
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}

	//gui that includes buttons for server, refresh host, and room1
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();

			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	
	



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
