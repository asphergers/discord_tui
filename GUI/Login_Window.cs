using Terminal.Gui;
using discord_cli.Discord;

namespace discord_cli.GUI {
    public class Login_Window : Window {
        private View parent;

        public Login_Window(Window parent) : base("Login") {
            this.parent = parent; 
            initalize();
        } 

        private void initalize() {
            Label email_label = new Label("Email") {
                X = Pos.Center(),
                Y = Pos.Center(),
            };

            TextField email_input = new TextField("") {
                X = Pos.Center(),
                Y = Pos.Bottom(email_label),

                Width = 30
            };

            Add(email_input);
            Add(email_label);

            Label password_label = new Label("Password") {
                X = Pos.Center(),
                Y = Pos.Bottom(email_input) + 2,
            };

            TextField password_input = new TextField("") {
                X = Pos.Center(),
                Y = Pos.Top(password_label),

                Width = 30,
            };

            Add(password_label);
            Add(password_input);

            Button login_button = new Button("Login", true) {
                X = Pos.Center(),
                Y = Pos.Bottom(password_input),

                Width = 10
            };

            Add(login_button);

            login_button.Clicked += async () => {
                if (email_input.Text.ToString() == null || password_input.Text.ToString() == null) {
                    MessageBox.ErrorQuery(0, 0, "Error", "Fields cannot be empty", "OK");
                    return;
                }

                string discord_response = await Login_Manager.login(email_input.Text.ToString(),
                                                                    password_input.Text.ToString());

                if (discord_response == null || !discord_response.Contains("token")) {
                    MessageBox.ErrorQuery(0, 0, "Error", "Incorrect username or password", "OK");       
                    return;
                } 

                await ConfigHandler.make_autoconfig(discord_response);
                Application.Top.Remove(this);
                Application.Top.Running = false;
            };
        }
    }
}
