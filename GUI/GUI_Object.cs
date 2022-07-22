using discord_cli.Discord;
using Terminal.Gui;

namespace discord_cli.GUI {
    public class GUI_Object : FrameView {
        public GUI_Object(string name) {
            base.X = 0;
            base.Y = 0;

            base.Title = name;

            base.Width = Dim.Fill();
            base.Height = Dim.Fill();
        }

        public virtual void update(Client client) {
            return;
        }

        public virtual async Task update(Guild guild) {
            return;
        }

        public virtual async Task<bool> update(Guild_Channel channel) {
            return false;
        }

        public ListView core_list = new ListView();
    }
}
