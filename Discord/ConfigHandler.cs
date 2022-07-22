using Newtonsoft.Json;

namespace discord_cli.Discord {
    public static class ConfigHandler {

        public static List<User_Config_Item>? get_config(string filename) {
            return ConfigHandler.get_file_config(filename);
        }

        public static List<User_Config_Item>? get_config() {
            return ConfigHandler.get_file_config("autoconfig");
        }

        public static List<User_Config_Item>? get_file_config(string filename) {
            using (StreamReader r = new StreamReader($"./configs/{filename}.json")) {
                string json = r.ReadToEnd();
                List<User_Config_Item>? items = JsonConvert.DeserializeObject<List<User_Config_Item>>(json);
                return items;
            }
        } 

        public async static Task make_autoconfig(string json_raw) {
            await File.WriteAllTextAsync("./configs/autoconfig.json", json_raw);
        }
    }
}
