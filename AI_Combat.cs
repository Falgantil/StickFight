using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StickFight
{
    public class AiCombat
    {
        private Random rnd;

        private bool hasHit;

        public Player PlayerObject { get; set; }

        public AiCombat(Player player)
        {
            this.PlayerObject = player;
            rnd = new Random();
        }

        public void Update(Player opponent)
        {
            bool isNearby = false;

            bool breakout = false;
            for (int i = PlayerObject.Collision.X - 1; i < PlayerObject.Collision.X + PlayerObject.Collision.Width + 1; i++)
            {
                for (int u = PlayerObject.Collision.Y - 1; u < PlayerObject.Collision.Y + PlayerObject.Collision.Height + 1; u++)
                {
                    if (PlayerObject.Nearby(new Point(i, u), opponent.Collision))
                    {
                        isNearby = true;

                        breakout = true;
                    }
                    if (breakout)
                        break;
                }
                if (breakout)
                    break;
            }

            if (isNearby)
            {
                Nearby(opponent);
            }
            else
            {
                NotNearby(opponent);
            }
        }

        private void Nearby(Player opponent)
        {
            int min = (PlayerObject.State == Standing.Stand ? 0 : 1);

            switch (rnd.Next(min, 6))
            {
                case 0:
                    hasHit = true;
                    Attack();
                    break;
                case 1:
                    MoveAway(opponent);
                    break;
                case 2:
                    MoveTowards(opponent);
                    break;
                default:

                    break;
            }
        }

        private void NotNearby(Player opponent)
        {
            int max = (hasHit ? 3 : 2);

            switch (rnd.Next(0, max))
            {
                case 0:
                    break;
                case 1:
                    MoveTowards(opponent);
                    break;
                case 2:
                    MoveAway(opponent);
                    break;
                default:

                    break;
            }

            if (hasHit && (GetPlayerCenter(PlayerObject).X - 3 > GetPlayerCenter(opponent).X || GetPlayerCenter(PlayerObject).X + 3 < GetPlayerCenter(opponent).X))
            {
                hasHit = false;
            }
        }

        private void MoveAway(Player opponent)
        {
            if (GetPlayerCenter(PlayerObject).X > GetPlayerCenter(opponent).X)
            {
                if (PlayerObject.Collision.X > 72)
                    PlayerObject.Jump();
                else
                    PlayerObject.Move(Direction.Right);
            }
            else if (GetPlayerCenter(PlayerObject).X < GetPlayerCenter(opponent).X)
            {
                if (PlayerObject.Collision.X < 3)
                    PlayerObject.Jump();
                else
                    PlayerObject.Move(Direction.Left);
            }

            if (GetPlayerCenter(PlayerObject).Y > GetPlayerCenter(opponent).Y && rnd.Next(0, 2) == 1)
            {
                PlayerObject.Jump();
            }
        }

        private void MoveTowards(Player opponent)
        {
            if (GetPlayerCenter(PlayerObject).X > GetPlayerCenter(opponent).X)
            {
                PlayerObject.Move(Direction.Left);
            }
            else if (GetPlayerCenter(PlayerObject).X < GetPlayerCenter(opponent).X)
            {
                PlayerObject.Move(Direction.Right);
            }

            if (GetPlayerCenter(PlayerObject).Y > GetPlayerCenter(opponent).Y && rnd.Next(0, 2) == 1)
            {
                PlayerObject.Jump();
            }
        }

        private void Attack()
        {
            switch (rnd.Next(0, 3))
            {
                case 0:
                    PlayerObject.Block();
                    break;
                case 1:
                    PlayerObject.Hit();
                    break;
                case 2:
                    PlayerObject.Kick();
                    break;
            }
        }

        private Point GetPlayerCenter(Player plr)
        {
            return new Point(plr.Collision.Width / 2 + plr.Collision.X, plr.Collision.Height / 2 + plr.Collision.Y); ;
        }
    }
}
