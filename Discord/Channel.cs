using Newtonsoft.Json;
using System.Text;

namespace discord_cli.Discord {
    public class Guild_Channel {
        public HttpClient httpClient;
        public List<Message_Item> messages = new List<Message_Item>();
        public Guild_Channel_Item channel_info;
        public Guild_Channel(Guild_Channel_Item info, HttpClient httpClient) { 
            this.httpClient = httpClient;
            this.channel_info = info;
            return;
        }

        private async Task<string> messages_raw() {
            var response_raw = await httpClient.GetAsync($"https://discord.com/api/v9/channels/{channel_info.id}/messages?limit=50");
            return await response_raw.Content.ReadAsStringAsync();
        }

        public async Task<bool> update_messages() {
            string raw_messages = await messages_raw();
            if (!raw_messages.Contains("channel_id")) {
                return false;
            } else {
                messages = JsonConvert.DeserializeObject<List<Message_Item>>(raw_messages); 
                return true;
            }    
        }
        
        public string? name() {
            return channel_info.name;
        }

        public string? id() {
            return channel_info.id;
        }

        public override string? ToString()
        {
            return channel_info.name;
        }

        public async Task send_message(string message) {
            string post_url = $"https://discord.com/api/v9/channels/{this.channel_info.id}/messages";
            Send_Message_Item request = new Send_Message_Item() {
                content = message,
                tts = false
            };

            string raw_json = JsonConvert.SerializeObject(request);
            var json = new StringContent(raw_json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(post_url, json);

            string response_json = await response.Content.ReadAsStringAsync();
        }
    }
}
