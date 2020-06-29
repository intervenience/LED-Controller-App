
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;
using System.Net;

namespace LEDController {

    public class ConnectionManager : MonoBehaviour {

        [SerializeField] TMP_InputField ip, port;
        [SerializeField] Button connectButton;
        [SerializeField] Persistence persistence;
        [SerializeField] ErrorManager errorManager;
        [SerializeField] ViewManager view;

        TcpClient _tcp;
        NetworkStream _ns;

        void Start () {
            if (persistence == null)
                persistence = GetComponent<Persistence> ();

            port.text = persistence.settings.port.ToString ();
            ip.text = persistence.settings.ip;
        }

        void OnEnable () {
            connectButton.onClick.AddListener (delegate {
                Task.Run (QueryConnection);
                //persistence.SaveSettings ();
            });

            ip.onValueChanged.AddListener (delegate { 
                IpValueChanged (); 
            });

            port.onValueChanged.AddListener (delegate {
                PortValueChanged ();
            });
        }

        void OnDisable () {
            connectButton.onClick.RemoveAllListeners ();
            ip.onValueChanged.RemoveAllListeners ();
            port.onValueChanged.RemoveAllListeners ();
        }

        void OnDestroy () {
            if (_ns != null) {
                _ns.Close ();
            }
            if (_tcp != null) {
                _tcp.Close ();
            }
        }

        async Task<int> QueryConnection () {
            try {
                TcpClient tcp = new TcpClient (persistence.settings.ip, persistence.settings.port);
                NetworkStream ns = tcp.GetStream ();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes ("ping");
                //Debug.Log (bytesToSend.Length);

                ns.Write (bytesToSend, 0, bytesToSend.Length);

                byte[] bytesToRead = new byte[tcp.ReceiveBufferSize];
                int bytesRead = await ns.ReadAsync (bytesToRead, 0, tcp.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString (bytesToRead, 0, bytesRead);

                Debug.Log (data);
                if (data == "pong") {
                    //correct response so connect to this
                    //Debug.Log ("Successfully connected");
                    view.OnSuccessfulConnection ();
                } else {
                    ns.Close ();
                    tcp.Close ();
                }
            } catch (Exception e) {
                errorManager.RaiseError (e);
            }

            return 0;
        }

        public void UpdateLighting (string s) {
            //Debug.Log ("Updating lighting " + s);
            Task.Run (() => AttemptUpdateLighting (s));
        }

        async Task AttemptUpdateLighting (string s) {
            try {
                //Debug.Log ("trying " + s);
                TcpClient tcp = new TcpClient (persistence.settings.ip, persistence.settings.port);
                NetworkStream ns = tcp.GetStream ();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes (s);

                ns.Write (bytesToSend, 0, bytesToSend.Length);

                byte[] bytesToRead = new byte[tcp.ReceiveBufferSize];
                int bytesRead = await ns.ReadAsync (bytesToRead, 0, tcp.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString (bytesToRead, 0, bytesRead);

                //Debug.Log (data);
                ns.Close ();
                tcp.Close ();
                //Debug.Log ("Written");
            } catch (Exception e) {
                errorManager.RaiseError (e);
            }
        }

        public void UpdateFrequency (float f) {
            Task.Run (() => AttemptUpdateFrequency (f));
        }

        async Task AttemptUpdateFrequency (float f) {
            try {
                TcpClient tcp = new TcpClient (persistence.settings.ip, persistence.settings.port);
                NetworkStream ns = tcp.GetStream ();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes ("frequency " + f);

                ns.Write (bytesToSend, 0, bytesToSend.Length);

                byte[] bytesToRead = new byte[tcp.ReceiveBufferSize];
                int bytesRead = await ns.ReadAsync (bytesToRead, 0, tcp.ReceiveBufferSize);
                string data = Encoding.ASCII.GetString (bytesToRead, 0, bytesRead);

                //Debug.Log (data);
                ns.Close ();
                tcp.Close ();
            } catch (Exception e) {
                errorManager.RaiseError (e);
            }
        }

        void PortValueChanged () {
            persistence.settings.port = Convert.ToInt32 (port.text);
        }

        void IpValueChanged () {
            persistence.settings.ip = ip.text;
        }

    }

}
