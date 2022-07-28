using Newtonsoft.Json;
using Websocket.Client;

namespace discord_cli.Discord {
    public class GateWayListener {
        private Client client;
        private ManualResetEvent exit_event = new ManualResetEvent(false);
        private static Uri url = new Uri("wss://gateway.discord.gg/v=6&encoding=json");
        public GateWayListener(Client client) {
            this.client = client;
        }

        public void init() {
            Handshake_Item? handshake_response = handshake();
            int interval;
            string? token = string.Empty;
            
            if (handshake_response != null) {
                if (handshake_response.d != null) {
                    interval = handshake_response.d.heartbeat_interval;
                } else { throw new NullReferenceException(); }
            } else { throw new NullReferenceException(); }

            if (client.client_config != null) {
                if (client.client_config[0].token != null) {
                    token = client.client_config[0].token;
                } else { throw new NullReferenceException(); }
            } else { throw new NullReferenceException(); }

            using (var web_client = new WebsocketClient(url)) {
                web_client.ReconnectTimeout = null;

                web_client.MessageReceived.Subscribe(msg => {
                    //Console.WriteLine(msg);
                    dynamic? server_message = JsonConvert.DeserializeObject(msg.ToString());
                    if (server_message != null) {
                        if (server_message.t == "MESSAGE_CREATE") {
                            if (client.user_guilds.ContainsKey(server_message.d.guild_id.ToString())) {
                                if (client.user_guilds[server_message.d.guild_id.ToString()].channels.ContainsKey(server_message.d.channel_id.ToString())){
                                    client.
                                    user_guilds[server_message.d.guild_id.ToString()].
                                    channels[server_message.d.channel_id.ToString()].
                                    messages.Insert(0,
                                        new Message_Item(){
                                            id = server_message.d.id.ToString(),
                                            content = server_message.d.content.ToString(),
                                            author = new Author_Item(){
                                            username = server_message.d.author.username.ToString()
                                        },
                                            timestamp = server_message.d.timestamp,
                                            guild_id = server_message.d.guild_id
                                    });
                                }
                            }
                        }
                    }
                });

                web_client.Start();

                Thread heartbeat = new Thread(() => heartbeat_loop(interval, web_client));
                heartbeat.IsBackground = true;
                heartbeat.Start();

                Task.Run(() => web_client.Send(@"{
                    ""op"" : 2,
                    ""d"" : {
                        ""token"" : " + "\"" + token + "\"" + @",
                        ""intents"" : 33280,
                        ""properties"": {
                            ""os"": ""linux"",
                            ""browser"": ""chrome"",
                            ""device"": ""pc""
                        }
                    }
                }"));

                exit_event.WaitOne();
            }
        }
        private static Handshake_Item? handshake() {
            Handshake_Item? response = null;
            using (var client = new WebsocketClient(url)) {

                client.MessageReceived.Subscribe(msg => {
                    response = JsonConvert.DeserializeObject<Handshake_Item>(msg.ToString());
                });

                client.Start().Wait();

                client.Dispose();
            }

            return response;
        }

        private static void heartbeat_loop(int interval, WebsocketClient web_client) {
            using (web_client) {
                //client.Start();

                while (true) {
                    Thread.Sleep(interval);
                    Task.Run(() => web_client.Send(@"{
                        ""op"" : 1,
                        ""d"" : ""null""
                    }")).Wait();
                }
            }
        }
    }
}
