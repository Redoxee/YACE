using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YACE
{
    class GameManager
    {
        public GameContext Context;

        public void PlayCard(int cardId);
        public void PlayerEndTurn();
        public void PlayerBuyCard(int cardId);
    }
}
