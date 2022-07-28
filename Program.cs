using Terminal.Gui;
using discord_cli.Discord;
using discord_cli.GUI;

namespace discord_cli {
    public class Program {
        public static async Task Main(string[] args) {

            if (!File.Exists("./configs/autoconfig.json")) {
                Application.Init();
                Login_Window login = new Login_Window(new Window());
                Application.Top.Add(login);
                Application.Run();
                Application.Shutdown();
            }
             
            Client client = new Client();

            Console.WriteLine("initalizing");
            await client.init();
            Console.WriteLine("done");

            GateWayListener listener = new GateWayListener(client);
            
            Thread gateway_thread = new Thread(() => listener.init());
            gateway_thread.Start();

            Application.Init();
            var top = Application.Top;

                      
            Window main_window = new Window("Discord CLI") {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            Guild_View guild_frame = new Guild_View(client, "Guilds");
            main_window.Add(guild_frame);

            Channel_View channel_frame = new Channel_View("Channels");

            Message_View message_frame = new Message_View("Messages");

            GUI_Object[] frame_path = new GUI_Object[] {
                guild_frame,
                channel_frame,
                message_frame,
            };

            int current_position = 0;

            //EventArgs argss = new ListViewItemEventArgs(guild_list.SelectedItem, guild_list);
            guild_frame.core_list.OpenSelectedItem += async (a) => {
                int index = frame_path[current_position].core_list.SelectedItem;
                Guild? guild = (Guild?)frame_path[current_position].core_list.Source.ToList()[index]; 
                main_window.Remove(frame_path[current_position]);
                current_position++;
                await frame_path[current_position].update(guild);
                //frame_path[current_position].core_list.SetSource(await guild.get_channels());
                //channel_frame.core_list.SetSource(await guild.get_channels());
                //main_window.Remove(guild_frame.core_list_frame);
                main_window.Add(frame_path[current_position]);
            };

            channel_frame.core_list.OpenSelectedItem += async (a) => {
                var index = channel_frame.core_list.SelectedItem;
                Guild_Channel? channel = (Guild_Channel?)channel_frame.core_list.Source.ToList()[index];
                bool viewable = await frame_path[current_position+1].update(channel);
                if (viewable) {
                    main_window.Remove(frame_path[current_position]);
                    current_position++;
                    main_window.Add(frame_path[current_position]);
                }
            };

             MenuBar menu_bar = new MenuBar(new MenuBarItem [] {
                new MenuBarItem ("", new MenuItem[] {
                    new MenuItem ("", "", () => {
                        if (current_position != 0) {
                            main_window.Remove(frame_path[current_position]);
                            current_position--;
                            main_window.Add(frame_path[current_position]);
                        }
                    }, null, null, Key.CtrlMask | Key.X)
                }),
            });
            
            top.Add(menu_bar);
            //channel_list.SetSource(_channels);
            top.Add(main_window);
            Application.Run(top);
            Application.Shutdown();

            Environment.Exit(0);
        }
    }
}

