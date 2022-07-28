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

            TextField message_input = new TextField() {
                X = 0,
                Y = 0,

                Height = 1,
                Width = Dim.Fill()
            };

            Button enter_message = new Button("Send", true) {
                X = Pos.Right(message_input),
                Y = 0,

                Height = 1,
                Width = 1
            };

            enter_message.Clicked += async () => {
                if (message_input.Text != null) {
                    await this.current_channel.send_message(message_input.Text.ToString()); 
                    message_input.Text = "";
               }
            };

            Add(core_list);
            Add(message_input);
            Add(enter_message);

        }

        public override async Task<bool> update(Guild_Channel channel) {
            this.current_channel = channel;
            bool can_view = await channel.update_messages();
            if (can_view) {
                base.Title = $"{channel.channel_info.name} Messages";
                core_list.SetSource(channel.messages);
                return true;
            } else {
                MessageBox.ErrorQuery(0, 0, "Error", "Cannot Acess Channel", "OK");  
                return false;
            }
        }
    }
}
