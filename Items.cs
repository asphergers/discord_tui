using System.Collections.Generic;

namespace discord_cli
{
    public class User_Config_Item {
        public string? x_super_properties;
        public string? token;
        public string? user_agent;
    }

    public class Guild_Item {
        public string? name;
        public string? id;
        public string? permissions;
        public string? icon;
        public string[]? features;
    }

    public class Guild_Channel_Item {
        public string? id;
        public string? name;
        public string? type;
        public int? position;
        public string? parent_id;
        public string? topic;

        public override string ToString() {
            return name;
        }
    }

    public class Message_Item {
        public string? id;
        public string? content;
        public Author_Item? author;
        public string? timestamp;
        public IList<Mentions_Item>? mentions;
        public string? guild_id;

        public override string ToString()
        {
            return ($"{author.username}: {content}\n");
        }
    }

    public class Author_Item {
        public string? id;
        public string? username;
        public string? discriminator;
    }

    public class Mentions_Item {
        public string? id;
        public string? username;
        public string? discriminator;
    }


    public class Message_Create_Item {
        public string? content;
        public Author_Item? author;
        public string? guild_id;
        public string? channel_id;
        public string? t;
    }

    public class Handshake_Item {
        public int op;
        public Data? d;

    }

    public class Data {
        public int heartbeat_interval;
    }
    
    public class Login_Request_Item {
        public string? login;
        public string? password;
    }

    public class Login_Respone_Item {
        public string? token;
    }
}
