using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Windows.Speech;

#if !UNITY_EDITOR
using Windows.Networking;
using Windows.Networking.Sockets;
using System.IO;
#endif

public class UDPClient : MonoBehaviour {

    [Header("Local")]
    [SerializeField]
    private int localPort;

    [Header("Remote")]
    [SerializeField]
    private string remoteIP;
    [SerializeField]
    public string remotePort;

    IPEndPoint remoteEndPoint;
    //UdpClient client;  

    public void Start() {
        //Init();
    }

    /*
    public void Init() {
        print("UDPSend.init()");

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
        //client = new UdpClient();

        print("Sending to " + remoteIP + " : " + remotePort);
        print("Testing: nc -lu " + remoteIP + " : " + remotePort);
    }

    public void SendByteData(byte[] data) {
        try {
            //client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err) {
            print(err.ToString());
        }
    }

    public void SendString(string message) {
        byte[] data = Encoding.UTF8.GetBytes(message);
        SendByteData(data);
    }
    */

#if !UNITY_EDITOR
    public async void SendByteData(byte[] data) {
        StreamSocket socket = new StreamSocket();

        HostName host = new HostName(remoteIP); //Replace with coorect hostname when running on RPi

        try {
            try {
                await socket.ConnectAsync(host, remotePort);
            }
            catch(Exception ex) {
                Debug.Log("HIBA");
                //txb_Events.Text += ex.Message;
            }

            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            StreamWriter writer = new StreamWriter(streamOut);
            //BinaryWriter writer = new BinaryWriter(streamOut);
            string request = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
            //string request = BitConverter.ToString(data);
            Debug.Log(data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "," + data[4] + "," + data[5]);
            Debug.Log(request);
            await writer.WriteLineAsync(request);
            //await writer.WriteAsync((char) data);
            //await writer.Write(data);
            //writer.Write(data);
            await writer.FlushAsync();

            socket.Dispose();

        }
        catch (Exception ex) {
            //txb_Events.Text += ex.Message;
            //Logs.Add(ex.Message)
        }
    }
#endif
}