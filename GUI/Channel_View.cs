using Terminal.Gui;
using discord_cli.Discord;

namespace discord_cli.GUI {
    public class Channel_View : GUI_Object {

        public Channel_View(string name) : base(name) {
            core_list.X = 1;
            core_list.Y = 1;

            core_list.Width = Dim.Fill();
            core_list.Height = Dim.Fill();

            this.core_list = new ListView() {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Add(core_list);
        }

        public override async Task update(Guild guild) {
           core_list.SetSource(await guild.get_channels());
        }
    }
}
