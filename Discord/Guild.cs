using System;
using Newtonsoft.Json;

namespace discord_cli.Discord {
    public class Guild {
        public Guild_Item info;
        public Dictionary<string, Guild_Channel> channels = new Dictionary<string, Guild_Channel>();
        private List<Guild_Channel> sorted_channels = new List<Guild_Channel>();
        HttpClient httpClient;
        public Guild(Guild_Item info, HttpClient httpClient) {
            this.httpClient = httpClient;
            this.info = info;
        }

        public async Task init_channels() {
            var response_raw = await httpClient.GetAsync($"https://discord.com/api/v9/guilds/{info.id}/channels");
            string raw_channels = await response_raw.Content.ReadAsStringAsync();
            List<Guild_Channel_Item> channels = JsonConvert.DeserializeObject<List<Guild_Channel_Item>>(raw_channels);
            foreach (Guild_Channel_Item channel in channels ?? Enumerable.Empty<Guild_Channel_Item>()) {
                if (channel.id == null) {
                    channel.id = "no id";
                }
                this.channels.Add(channel.id ,new Guild_Channel(channel, httpClient));
            }

            this.sorted_channels = this.channels.Values.ToList();
            this.sort_channels();
            return;
        }

        private void sort_channels() {
            for (int i = 0; i<this.sorted_channels.Count; i++) {
                for (int j = 0; j<this.sorted_channels.Count-1-i; j++) {
                    if (sorted_channels[j].channel_info.position > sorted_channels[j+1].channel_info.position) {
                        Guild_Channel temp = this.sorted_channels[j];
                        this.sorted_channels[j] = this.sorted_channels[j+1];
                        this.sorted_channels[j+1] = temp;
                    }
                }
            }
        }

        public async Task init() {
            await init_channels();
        }

        public string? name() {
            return info.name;
        }

        public override string? ToString() {
            return info.name;
        }

        public async Task<List<Guild_Channel>> get_channels() {
            if (!channels.Any()) {
                await this.init();
                return this.sorted_channels;
            } else {
                return this.sorted_channels;
            }
        }
    }
}
