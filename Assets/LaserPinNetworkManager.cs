using UnityEngine;
using System.Collections;

public class LaserPinNetworkManager : MonoBehaviour {

	HostData[] hostDatas;

	void OnGUI () 
	{
		if (Network.isClient || Network.isServer)
			return;
		if (GUI.Button(new Rect(20f, 20f, 200f, 20f), "Start a server"))
		{
			StartServer();
		}

		if (GUI.Button(new Rect(20f, 50f, 200f, 20f), "Refresh"))
		{
			MasterServer.RequestHostList("LaserPin");
		}

		if (hostDatas == null)
			return;
		for(int i = 0 ; i < hostDatas.Length ; i++)
		{
			HostData hd = hostDatas[i];
			if (GUI.Button(new Rect(20f, 80f + 30f * i, 200f, 20f), hd.gameName))
			{
				Network.Connect(hd);
			}

		}
	}

	void StartServer()
	{
		Network.InitializeServer(32, 4242, !Network.HavePublicAddress());
		MasterServer.RegisterHost("LaserPin", "Game");
	}

	void OnServerInitialized () {
		Debug.Log("Server Initialized");
	}

	void OnMasterServerEvent(MasterServerEvent e)
	{
		if (e == MasterServerEvent.RegistrationSucceeded)
			Debug.Log("RegistrationSucceeded");
		else if (e == MasterServerEvent.HostListReceived)
			hostDatas = MasterServer.PollHostList();
	}
}
