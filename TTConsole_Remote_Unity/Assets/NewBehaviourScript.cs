using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    class History
    {
        List<string> history = new List<string>();
        int index = 0;

        public void Add(string item)
        {
            history.Add(item);
            index = 0;
            current = item;
        }

        string current = string.Empty;

        public string Fetch(string cur, bool next)
        {
            if (index == 0) this.current = cur;

            if (history.Count == 0) return current;

            index += next ? -1 : 1;

            if (history.Count + index < 0 || history.Count + index > history.Count - 1)
            {
                index = 0;
                return this.current;
            }

            current = history[history.Count + index];

            return current;
        }
    }

    public Text content;
    public InputField input;

    History _history = new History();
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

            _history.Add(cmd);
            input.text = string.Empty;
            input.ActivateInputField();
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

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            input.text = _history.Fetch(input.text, true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            input.text = _history.Fetch(input.text, false);
        }
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
