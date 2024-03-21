using System;
using System.Collections.Generic;

namespace Card_game
{
    public class Karta
    {
        public string Mast { get; private set; }
        public string Tip { get; private set; }

        public Karta(string mast, string tip)
        {
            Mast = mast;
            Tip = tip;
        }
    }

    public class Player
    {
        public List<Karta> Cards { get; private set; }

        public Player()
        {
            Cards = new List<Karta>();
        }

        public void AddCard(Karta card)
        {
            Cards.Add(card);
        }

        public void PrintCards()
        {
            foreach (var card in Cards)
            {
                Console.WriteLine($"Mast: {card.Mast}, Tip: {card.Tip}");
            }
        }

        internal void AddCardRange(List<Karta> cards)
        {
            throw new NotImplementedException();
        }
    }

    public class Game
    {
        private List<Player> players;
        private List<Karta> deck;

        public Game()
        {
            players = new List<Player>();
            deck = new List<Karta>();

            // Создание колоды карт
            string[] masts = { "черви", "бубны", "пики", "трефы" };
            string[] tips = { "6", "7", "8", "9", "10", "валет", "дама", "король", "туз" };

            foreach (var mast in masts)
            {
                foreach (var tip in tips)
                {
                    deck.Add(new Karta(mast, tip));
                }
            }

            // Перетасовка карт
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Karta value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void DealCards()
        {
            int cardsPerPlayer = deck.Count / players.Count;

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < cardsPerPlayer; j++)
                {
                    Karta card = deck[i * cardsPerPlayer + j];
                    players[i].AddCard(card);
                }
            }
        }

        public void PlayGame()
        {
            int currentPlayerIndex = 0;
            while (true)
            {
                Player currentPlayer = players[currentPlayerIndex];
                Karta playedCard = currentPlayer.Cards[0];

                foreach (var player in players)
                {
                    player.PrintCards();
                }

                Console.WriteLine($"Current player: {currentPlayerIndex}");

                if (playedCard.Tip != "туз")
                {
                    for (int i = 1; i < players.Count; i++)
                    {
                        Karta card = players[i].Cards[0];

                        if (tipsValue(card.Tip) > tipsValue(playedCard.Tip))
                        {
                            currentPlayer.AddCardRange(players[i].Cards); // Забираем все карты
                            players[i].Cards.Clear();
                        }
                    }
                }

                currentPlayer.Cards.RemoveAt(0); // Удаляем сыгранную карту

                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

                if (currentPlayer.Cards.Count == 36)
                {
                    Console.WriteLine($"Player {currentPlayerIndex} wins!");
                    break;
                }
            }
        }

        private int tipsValue(string tip)
        {
            if (int.TryParse(tip, out int result))
            {
                return result;
            }

            if (tip == "валет")
            {
                return 11;
            }

            if (tip == "дама")
            {
                return 12;
            }

            if (tip == "король")
            {
                return 13;
            }

            return 0;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();

            Player player1 = new Player();
            Player player2 = new Player();

            game.AddPlayer(player1);
            game.AddPlayer(player2);

            game.DealCards();

            game.PlayGame();
        }
    }
}