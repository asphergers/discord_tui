using System.Text;
using Newtonsoft.Json;

namespace discord_cli.Discord {
    public static class Login_Manager {
        public static async Task<string> login(string usr, string pass) {
            string login_url = "https://discord.com/api/v9/auth/login"; 
            HttpClient httpClient = new HttpClient();
            Login_Request_Item request = new Login_Request_Item() {
                  login = usr,
                  password = pass
            };

            string json = JsonConvert.SerializeObject(request);
            var json_raw = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(login_url, json_raw);

            string response_json = await response.Content.ReadAsStringAsync();

            return "[" + response_json + "]";
        }
    }
}
