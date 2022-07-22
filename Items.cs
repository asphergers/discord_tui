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
        public string x_super_properties = "eyJvcyI6IkxpbnV4IiwiYnJvd3NlciI6IkRpc2NvcmQgQ2xpZW50IiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X3ZlcnNpb24iOiIwLjAuMTgiLCJvc192ZXJzaW9uIjoiNS4xOC41LXplbjEtMS16ZW4iLCJvc19hcmNoIjoieDY0Iiwic3lzdGVtX2xvY2FsZSI6ImVuLVVTIiwid2luZG93X21hbmFnZXIiOiJ1bmtub3duLHVua25vd24iLCJkaXN0cm8iOiJcIkdhcnVkYSBMaW51eFwiIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTM3MDk1LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==";
    }

    public class Login_Respone_Item {
        public string? token;
    }
}
