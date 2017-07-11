using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text content;
    public InputField input;

    string _contentText = string.Empty;
    bool _contentUpdate = false;
    string contentText
    {
        get { return _contentText; }
        set { _contentText = value; _contentUpdate = true; }
    }

    TcpClient client;
    BinaryWriter bw;
    BinaryReader br;

    void Start()
    {
        contentText = "Type \"host port\" to connect";

        input.onEndEdit.AddListener(cmd =>
        {
            if (!Input.GetButtonDown("Submit"))
                return;
            if (client == null)
            {
                string[] arr = cmd.Split(' ');
                int port;
                if (arr.Length == 2 && int.TryParse(arr[1], out port))
                {
                    try
                    {
                        client = new TcpClient();
                        client.Connect(arr[0], port);
                        bw = new BinaryWriter(client.GetStream());
                        br = new BinaryReader(client.GetStream());

                        var receiveThread = new Thread(receive);
                        receiveThread.IsBackground = true;
                        receiveThread.Start();
                    }
                    catch (System.Exception ex)
                    {
                        contentText += ex.ToString();
                        client = null;
                    }
                }
            }

            if (client != null && client.Client.Connected)
                bw.Write(cmd);
            else
            {
                client = null;
                contentText += "\nType \"host port\" to connect";
            }

            input.text = string.Empty;
        });
    }

    void Update()
    {
        if (_contentUpdate)
            content.text = contentText;
        try
        {
            if (client != null && client.Connected)
                bw.Write(string.Empty);
        }
        catch { }
    }

    void receive()
    {
        while (client != null && client.Connected)
        {
            string content = br.ReadString();
            if (string.IsNullOrEmpty(content))
                continue;
            char cmd = content[0];
            content = content.Substring(1);
            switch (cmd)
            {
                case 'R': contentText = string.Empty; break;
                case 'E': contentText += content; break;
                case 'S': contentText = content + contentText; break;
            }
        }
    }
}
