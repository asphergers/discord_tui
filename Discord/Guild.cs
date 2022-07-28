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

                if (channel.parent_id == null) {
                    channel.name = $"#{channel.name}#";
                }

                channel.parent_name = this.info.name;
                this.channels.Add(channel.id ,new Guild_Channel(channel, httpClient));
            }

            this.sorted_channels = this.channels.Values.ToList();
            this.sort_channels();
            return;
        }

        private void sort_channels() {
            List<Guild_Channel> parent_ids = new List<Guild_Channel>();
            List<Guild_Channel> final_sorted = new List<Guild_Channel>();

            
            foreach (Guild_Channel channel in this.sorted_channels) {
                if (channel.channel_info.parent_id == null) {
                    parent_ids.Add(channel);
                }
            }

            for (int i = 0; i < parent_ids.Count; i++) {
                for (int j = 0; j < parent_ids.Count - 1 - i; j++) {
                    if (parent_ids[j].channel_info.position > parent_ids[j+1].channel_info.position) {
                        Guild_Channel temp = parent_ids[j];
                        parent_ids[j] = parent_ids[j + 1];
                        parent_ids[j + 1] = temp;
                    }
                }
            }

            for (int i = 0; i < parent_ids.Count; i++) {
                final_sorted.Add(parent_ids[i]);
                List<Guild_Channel> temp = new List<Guild_Channel>();

                for (int j = 0; j < this.sorted_channels.Count; j++) {
                    if (this.sorted_channels[j].channel_info.parent_id != null) {
                        if (this.sorted_channels[j].channel_info.parent_id == parent_ids[i].channel_info.id) {
                            temp.Add(this.sorted_channels[j]);
                            this.sorted_channels.RemoveAt(j);
                            j--;
                        }
                    }
                }

                for (int k = 0; k < temp.Count; k++) {
                    for (int l = 0; l < temp.Count - 1 - k; l++) {
                        if (temp[l].channel_info.position > temp[l + 1].channel_info.position) {
                            Guild_Channel t = temp[l];
                            temp[l] = temp[l + 1];
                            temp[l + 1] = t;
                        }
                    }
                }

                final_sorted.AddRange(temp);
            }

            this.sorted_channels = final_sorted;
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
