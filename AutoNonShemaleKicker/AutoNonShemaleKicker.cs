using FChatSharpLib.Entities.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AutoNonShemaleKicker
{
    public class AutoNonShemaleKicker : BasePlugin
    {
        private Timer _kickMonitor;
        private List<string> subscribedCharacters = new List<string>();
        private bool isUsingDefaultStatus = false;

        public AutoNonShemaleKicker(string channel) : base(nameof(AutoNonShemaleKicker), "1.0.0", channel)
        {
            _kickMonitor = new Timer(PeriodicallyCheckThings, null, 1000, 5000);
            base.Run();
        }

        private void PeriodicallyCheckThings(object state)
        {
            if (!FChatClient.IsUserAdmin(FChatClient.State.BotCharacterName, Channel))
            {
                return;
            }
            var characters = FChatClient.State.GetAllCharactersInChannel(Channel);
            var newListSubscribedCharacters = SDSWQueryAPI.GetAllCharacters();
            var newCharacters = new List<string>();
            if(subscribedCharacters.Count != 0)
            {
                newCharacters = newListSubscribedCharacters.Except(subscribedCharacters).ToList();
            }

            subscribedCharacters = newListSubscribedCharacters;

            foreach (var character in characters)
            {
                if ((character.Gender != FChatSharpLib.Entities.Events.Helpers.GenderEnum.Shemale || (subscribedCharacters.Count > 0 && !subscribedCharacters.Contains(character.Character))) && !FChatClient.IsSelf(character.Character) && !FChatClient.IsUserAdmin(character.Character, Channel))
                {
                    FChatClient.KickUser(character.Character, Channel);
                    FChatClient.SendPrivateMessage("Hello! You've been kicked out from the Shemale DS World ([session=Shemale DS World]adh-ae6c36cf75c40ec6a52a[/session]) channel because you're not an active player of that game.\nIf you'd like to register, please join the OOC room ([session=Shemale DS Game]adh-3c6c8a69c06c14bbf523[/session]) and read the description.\nThank you!", character.Character);
                }
            }

            if (newCharacters.Count == 0 && !isUsingDefaultStatus)
            {
                FChatClient.SetStatus(FChatSharpLib.Entities.Events.Helpers.StatusEnum.Looking, $"[color=pink]Come join other bitch-breakers @ [session=Shemale DS Game]adh-3c6c8a69c06c14bbf523[/session]. Read the room's description for instructions on how to join the game.[/color]");
                isUsingDefaultStatus = true;
            }

            foreach (var character in newCharacters)
            {
                FChatClient.SetStatus(FChatSharpLib.Entities.Events.Helpers.StatusEnum.Looking, $"[icon]{character}[/icon] [user]{character}[/user] [color=pink]just joined the game![/color]");
                isUsingDefaultStatus = false;
            }
        }
    }
}
