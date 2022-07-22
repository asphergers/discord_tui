using discord_cli.Discord;
using Terminal.Gui;

namespace discord_cli.GUI {
    public class Message_View : GUI_Object {

        Guild_Channel current_channel;
        
        public Message_View(string name) : base(name) {
            core_list.X = 0;
            core_list.Y = 1;

            core_list.Width = Dim.Fill();
            core_list.Height = Dim.Fill();

            Add(core_list);

        }

        public override async Task<bool> update(Guild_Channel channel) {
            this.current_channel = channel;
            bool can_view = await channel.update_messages();
            if (can_view) {
                core_list.SetSource(channel.messages);
                return true;
            } else {
                MessageBox.ErrorQuery(0, 0, "Error", "Cannot Acess Channel", "OK");  
                return false;
            }
        }
    }
}
