﻿namespace Gap
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using NetIrc2;
    using NetIrc2.Events;

    using log4net;

    public class IrcBot
    {
        internal static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IrcClient IrcClient { get; set; }
        public IdentServer IdentServer { get; set; }
        public IrcBot()
        {
            IrcClient = new IrcClient();
            IrcClient.Connected += IrcClientOnConnected;
            IrcClient.Closed += IrcClientOnClosed;
            IrcClient.GotChannelListBegin += IrcClientOnGotChannelListBegin;
            IrcClient.GotChannelListEnd += IrcClientOnGotChannelListEnd;
            IrcClient.GotChannelListEntry += IrcClientOnGotChannelListEntry;
            IrcClient.GotChannelTopicChange += IrcClientOnGotChannelTopicChange;
            IrcClient.GotChatAction += IrcClientOnGotChatAction;
            IrcClient.GotInvitation += IrcClientOnGotInvitation;
            IrcClient.GotIrcError += IrcClientOnGotIrcError;
            IrcClient.GotJoinChannel += IrcClientOnGotJoinChannel;
            IrcClient.GotLeaveChannel += IrcClientOnGotLeaveChannel;
            IrcClient.GotMessage += IrcClientOnGotMessage;
            IrcClient.GotMode += IrcClientOnGotMode;
            IrcClient.GotMotdBegin += IrcClientOnGotMotdBegin;
            IrcClient.GotMotdEnd += IrcClientOnGotMotdEnd;
            IrcClient.GotMotdText += IrcClientOnGotMotdText;
            IrcClient.GotNameChange += IrcClientOnGotNameChange;
            IrcClient.GotNameListEnd += IrcClientOnGotNameListEnd;
            IrcClient.GotNameListReply += IrcClientOnGotNameListReply;
            //IrcClient.GotNotice += IrcClientOnGotNotice;
            IrcClient.GotPingReply += IrcClientOnGotPingReply;
            IrcClient.GotUserKicked += IrcClientOnGotUserKicked;
            IrcClient.GotUserQuit += IrcClientOnGotUserQuit;
            IrcClient.GotWelcomeMessage += IrcClientOnGotWelcomeMessage;
        }
        public void Start()
        {
            IrcClient.Connect(IrcConfig.Server, IrcConfig.Port);
        }

        public void Stop()
        {
            if(IrcClient != null)
                IrcClient.Close();
            if(IdentServer != null)
                IdentServer.Stop();
        }

        private void IrcClientOnGotWelcomeMessage(object sender, SimpleMessageEventArgs simpleMessageEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
            IrcClient.Join(IrcConfig.Channel);
            IrcClient.Join("#octgn");
            IrcClient.Join("#octgn-dev");
        }

        private void IrcClientOnGotUserQuit(object sender, QuitEventArgs quitEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotUserKicked(object sender, KickEventArgs kickEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotPingReply(object sender, PingReplyEventArgs pingReplyEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotNameListReply(object sender, NameListReplyEventArgs nameListReplyEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotNameListEnd(object sender, NameListEndEventArgs nameListEndEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotNameChange(object sender, NameChangeEventArgs nameChangeEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotMotdText(object sender, SimpleMessageEventArgs simpleMessageEventArgs)
        {
            Log.Info(simpleMessageEventArgs.Message.ToString());
        }

        private void IrcClientOnGotMotdEnd(object sender, EventArgs eventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotMotdBegin(object sender, EventArgs eventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotMode(object sender, ModeEventArgs modeEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotMessage(object sender, ChatMessageEventArgs args)
        {
            Log.Info(args.Sender.Username.ToString() + ":" + args.Message.ToString());
            if (args.Message.StartsWith("@")) return;
            if (args.Recipient.StartsWith("#") && args.Recipient.Equals("#octgn-lobby") == false) return;
            MessageQueue.Get().Add(new MessageItem(args.Sender.Nickname, args.Message));
        }

        private void IrcClientOnGotLeaveChannel(object sender, JoinLeaveEventArgs joinLeaveEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotJoinChannel(object sender, JoinLeaveEventArgs joinLeaveEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotIrcError(object sender, IrcErrorEventArgs ircErrorEventArgs)
        {
            Log.Info("Error" + ircErrorEventArgs.Error.ToString());
        }

        private void IrcClientOnGotInvitation(object sender, InvitationEventArgs invitationEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotChatAction(object sender, ChatMessageEventArgs chatMessageEventArgs)
        {
            Log.Info(chatMessageEventArgs.Sender.ToString() + ":" + chatMessageEventArgs.Message.ToString());
        }

        private void IrcClientOnGotChannelTopicChange(object sender, ChannelTopicChangeEventArgs channelTopicChangeEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotChannelListEntry(object sender, ChannelListEntryEventArgs channelListEntryEventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotChannelListEnd(object sender, EventArgs eventArgs)
        {
            Log.Info(MethodBase.GetCurrentMethod().Name);
        }

        private void IrcClientOnGotChannelListBegin(object sender, EventArgs eventArgs)
        {
            Log.Info("IrcClientOnGotChannelListBegin");
        }

        private void IrcClientOnClosed(object sender, EventArgs eventArgs)
        {
            Log.Info("Connection Closed");
            Task.Factory.StartNew(() =>
                                      {
                                          Thread.Sleep(10000);
                                          IrcClient.Connect(IrcConfig.Server, IrcConfig.Port);
                                      });
        }

        private void IrcClientOnConnected(object sender, EventArgs eventArgs)
        {
            //IrcClient.ChangeName("Octgn-Gap");
            Log.Info("Connected");
            var un = IrcConfig.BotName;
            IrcClient.LogIn(un, un, un,null,null,IrcConfig.Password);

        }
    }
    public static class IrcConfig
    {
        public static string Channel
        {
            get
            {
                return ConfigurationManager.AppSettings["IrcChannel"];
            }
        }
        public static string BotName
        {
            get
            {
                return ConfigurationManager.AppSettings["IrcBotName"];
            }
        }
        public static string Server
        {
            get
            {
                return ConfigurationManager.AppSettings["IrcServer"];
            }
        }
        public static int Port
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["IrcPort"]);
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["IrcBotPassword"];
            }
        }
    }
}