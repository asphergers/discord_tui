using System;
using Newtonsoft.Json;

namespace discord_cli.Discord {
    public class Client {
        public List<User_Config_Item>? client_config;
        public Dictionary<string, Guild> user_guilds = new Dictionary<string, Guild>();
        public HttpClient httpClient = new HttpClient();
        
        public Client(string filename) {
            this.init_vars(filename);
        }

        public Client() {
            this.init_vars("autoconfig");
        }

        private void init_vars(string filename) {
            this.client_config = ConfigHandler.get_config(filename);
            if (client_config != null) {
                //httpClient.DefaultRequestHeaders.Add("x-super-properties", client_config[0].x_super_properties);
                httpClient.DefaultRequestHeaders.Add("authorization", client_config[0].token);
                //httpClient.DefaultRequestHeaders.Add("user-agent", client_config[0].user_agent);
                return;
            } else {
                Console.WriteLine("config is null");
                return;
            }
        }

        private async Task<string> guilds_raw() {
            var response_raw = await httpClient.GetAsync("https://discord.com/api/v9/users/@me/guilds");
            return await response_raw.Content.ReadAsStringAsync();
        }

        private async Task init_guilds() {
            string raw_guilds = await this.guilds_raw();
            List<Guild_Item>? guilds_json = JsonConvert.DeserializeObject<List<Guild_Item>>(raw_guilds);

            foreach(Guild_Item guild in guilds_json ?? Enumerable.Empty<Guild_Item>()) {
                if (guild.id == null) {
                    guild.id = "no id";
                }
                this.user_guilds.Add(guild.id ,new Guild(guild, httpClient));
            }
        }

        public async Task init() {
            await init_guilds();
            // foreach (var guild in user_guilds.Values.ToList()) {
            //     await guild.init();
            // }
        }

        public List<Guild> guilds() {
            return this.user_guilds.Values.ToList();
        }

        public async Task update_guilds() {
            string response = await guilds_raw();
            user_guilds.Clear();
            List<Guild_Item>? guilds_json = JsonConvert.DeserializeObject<List<Guild_Item>>(response);
            foreach (Guild_Item guild in guilds_json ?? Enumerable.Empty<Guild_Item>()) {
                if (guild.id == null) {
                    guild.id = "no id";
                } 
                user_guilds.Add(guild.id, new Guild(guild, httpClient)); 
            }
        }
    }
}
