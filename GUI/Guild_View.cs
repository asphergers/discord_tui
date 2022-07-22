using Terminal.Gui;
using discord_cli.Discord;

namespace discord_cli.GUI {
    public class Guild_View : GUI_Object { 
        public List<Guild> _guilds;
        private Client client;

        public Guild_View(Client client, string name) : base(name) {
            this.client = client;
            this._guilds = client.guilds();
            core_list.X = 0;
            core_list.Y = 0;

            core_list.Width = Dim.Fill();
            core_list.Height = Dim.Fill();

            core_list.SetSource(client.guilds());
            Add(core_list);

        }

        public void update() {
            core_list.SetSource(_guilds);  
        }
    }
}
