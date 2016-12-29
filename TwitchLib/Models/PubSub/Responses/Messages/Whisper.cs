﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwitchLib.Models.PubSub.Responses.Messages
{
    /// <summary>
    /// Class representing a whisper received via PubSub.
    /// </summary>
    public class Whisper : MessageData
    {
        /// <summary>Type of MessageData</summary>
        public string Type { get; protected set; }
        /// <summary>Data identifier in MessageData</summary>
        public string Data { get; protected set; }
        /// <summary>Object that houses the data accompanying the type.</summary>
        public DataObj DataObject { get; protected set; }

        /// <summary>Whisper object constructor.</summary>
        public Whisper(string jsonStr)
        {
            JObject json = JObject.Parse(jsonStr);
            Type = json.SelectToken("type").ToString();
            Data = json.SelectToken("data").ToString();
            DataObject = new DataObj(json.SelectToken("data_object"));
        }

        /// <summary>Class representing the data in the MessageData object.</summary>
        public class DataObj
        {
            /// <summary>DataObject identifier</summary>
            public int Id { get; protected set; }
            /// <summary>Twitch assigned thread id</summary>
            public string ThreadId { get; protected set; }
            /// <summary>Body of data received from Twitch</summary>
            public string Body { get; protected set; }
            /// <summary>Timestamp generated by Twitc</summary>
            public long SentTs { get; protected set; }
            /// <summary>Id of user that sent whisper.</summary>
            public long FromId { get; protected set; }
            /// <summary>Tags object housing associated tags.</summary>
            public TagsObj Tags { get; protected set; }
            /// <summary>Receipient object housing various properties about user who received whisper.</summary>
            public RecipientObj Recipient { get; protected set; }
            /// <summary>Uniquely generated string used to identify response from request.</summary>
            public string Nonce { get; protected set; }

            /// <summary>DataObj constructor.</summary>
            public DataObj(JToken json)
            {
                Id = int.Parse(json.SelectToken("id").ToString());
                ThreadId = json.SelectToken("thread_id")?.ToString();
                Body = json.SelectToken("body")?.ToString();
                SentTs = long.Parse(json.SelectToken("sent_ts").ToString());
                FromId = long.Parse(json.SelectToken("from_id").ToString());
                Tags = new TagsObj(json.SelectToken("tags"));
                Recipient = new RecipientObj(json.SelectToken("recipient"));
                Nonce = json.SelectToken("nonce")?.ToString();
            }

            /// <summary>Class representing the tags associated with the whisper.</summary>
            public class TagsObj
            {
                /// <summary>Login value associated.</summary>
                public string Login { get; protected set; }
                /// <summary>Display name found in chat.</summary>
                public string DisplayName { get; protected set; }
                /// <summary>Color of whispers</summary>
                public string Color { get; protected set; }
                /// <summary>User type of whisperer</summary>
                public string UserType { get; protected set; }
                /// <summary>True or false for whether whisperer is turbo</summary>
                public bool Turbo { get; protected set; }
                /// <summary>List of emotes found in whisper</summary>
                public List<EmoteObj> Emotes = new List<EmoteObj>();
                /// <summary>All badges associated with the whisperer</summary>
                public List<Badge> Badges = new List<Badge>();

                /// <summary></summary>
                public TagsObj(JToken json)
                {
                    Login = json.SelectToken("login")?.ToString();
                    DisplayName = json.SelectToken("login")?.ToString();
                    Color = json.SelectToken("color")?.ToString();
                    UserType = json.SelectToken("user_type")?.ToString();
                    Turbo = bool.Parse(json.SelectToken("turbo").ToString());
                    foreach(JToken emote in json.SelectToken("emotes"))
                        Emotes.Add(new EmoteObj(emote));
                    foreach (JToken badge in json.SelectToken("badges"))
                        Badges.Add(new Badge(badge));
                }

                /// <summary>Class representing a single emote found in a whisper</summary>
                public class EmoteObj
                {
                    /// <summary>Emote ID</summary>
                    public int Id { get; protected set; }
                    /// <summary>Starting character of emote</summary>
                    public int Start { get; protected set; }
                    /// <summary>Ending character of emote</summary>
                    public int End { get; protected set; }

                    /// <summary>EmoteObj construcotr.</summary>
                    public EmoteObj(JToken json)
                    {
                        Id = int.Parse(json.SelectToken("id").ToString());
                        Start = int.Parse(json.SelectToken("start").ToString());
                        End = int.Parse(json.SelectToken("end").ToString());
                    }
                }
            }

            /// <summary>Class representing the recipient of the whisper.</summary>
            public class RecipientObj
            {
                /// <summary>Receiver id</summary>
                public long Id { get; protected set; }
                /// <summary>Receiver username</summary>
                public string Username { get; protected set; }
                /// <summary>Receiver display name.</summary>
                public string DisplayName { get; protected set; }
                /// <summary>Receiver color.</summary>
                public string Color { get; protected set; }
                /// <summary>User type of receiver.</summary>
                public string UserType { get; protected set; }
                /// <summary>True or false on whther receiver has turbo or not.</summary>
                public bool Turbo { get; protected set; }
                /// <summary>List of badges that the receiver has.</summary>
                public List<Badge> Badges { get; protected set; }

                /// <summary>RecipientObj constructor.</summary>
                public RecipientObj(JToken json)
                {
                    Id = long.Parse(json.SelectToken("id").ToString());
                    Username = json.SelectToken("username")?.ToString();
                    DisplayName = json.SelectToken("display_name")?.ToString();
                    Color = json.SelectToken("color")?.ToString();
                    UserType = json.SelectToken("user_type")?.ToString();
                    Turbo = bool.Parse(json.SelectToken("turbo").ToString());
                    Badges = new List<Badge>();
                    foreach (JToken badge in json.SelectToken("badges"))
                        Badges.Add(new Badge(badge));

                }
            }

            /// <summary>Class representing a single badge.</summary>
            public class Badge
            {
                /// <summary>Id of the badge.</summary>
                public string Id { get; protected set; }
                /// <summary>Version of the badge.</summary>
                public string Version { get; protected set; }

                /// <summary></summary>
                public Badge(JToken json)
                {
                    Id = json.SelectToken("id")?.ToString();
                    Version = json.SelectToken("version")?.ToString();
                }
            }
        }
    }
}
