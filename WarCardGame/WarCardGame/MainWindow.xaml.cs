using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarCardGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Card> allCards { get; set; }
        List<Card> shuffled { get; set; }
        List<Card>[] playerDecks { get; set; }
        List<Card> battleCards { get; set; }
        List<Card> tableCards { get; set; }
        List<Card>[] playerDecksDown { get; set; }
        
        int numPlayers { get; set; }
        int numWarCards { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Start Game pressed!");
            StartButton.Visibility = Visibility.Hidden;
            BattleButton.IsEnabled = true;
            ManyBattlesLater.IsEnabled = true;
            
            //UI: "LET THE GAMES BEGIN! Click Go! to begin the war."

            //UI is built for 2-player. Code is scalable.
            numPlayers = 2;
            numWarCards = 3;

            allCards = InitCards();

            shuffled = Shuffle(allCards);

            playerDecks = new List<Card>[numPlayers];
            playerDecksDown = new List<Card>[numPlayers];

            playerDecks = DealCards(shuffled, numPlayers);
            
            //TESTING CODE//
            for (int i = 0; i < playerDecks.Length; i++)
            {
                Console.WriteLine("Deck " + i + ":");
                for (int j = 0; j < playerDecks[i].Count; j++)
                {
                    Console.WriteLine(playerDecks[i][j].number + " " + playerDecks[i][j].suit);
                }

                Console.WriteLine("Count for Deck " + i + ": " + playerDecks[i].Count);
            }
            //TESTING CODE//
        }

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            Player0Card.Visibility = Visibility.Visible;
            Player1Card.Visibility = Visibility.Visible;

            battleCards = new List<Card>();
            tableCards = new List<Card>();
            int victor = new int();

            battleCards = CollectBattleCards();
            victor = CompareBattleCards(battleCards); //victor = 0, 1, or (-1 = tie)

            int totalBattleCards = battleCards.Count;
            for (int i=0; i<totalBattleCards; i++)
            {
                MoveCard(battleCards, tableCards, 0);
            }

            while (victor == -1) //war
            {
                for (int i=0; i<numPlayers; i++)
                {
                    for (int j=0; j<numWarCards; j++)
                    {
                        MovePlayerCard(playerDecks[i], tableCards, 0, i);
                    }
                }
                battleCards = CollectBattleCards();
                victor = CompareBattleCards(battleCards);

                totalBattleCards = battleCards.Count;
                for (int i = 0; i < totalBattleCards; i++)
                {
                    MoveCard(battleCards, tableCards, 0);
                }
            }
            GiveCards(victor);

            for (int i=0; i<numPlayers; i++)
            {
                Console.WriteLine("Player " + i + " cards left: " + playerDecks[i].Count);
                Console.WriteLine("Player " + i + " cards down: " + playerDecksDown[i].Count);
            }

            UpdateCounter();
        }

        private List<Card> InitCards()
        {
            //A=14 K=13 Q=12 J=11 10=10... 2=2
            int[] numbers = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }; 
            char[] suits = { 'D', 'H', 'S', 'C' };

            List<Card> allCards = new List<Card>();

            for (int i = 0; i < numbers.Length ; i++)
            {
                for (int j = 0; j < suits.Length; j++)
                {
                    Card card = new Card();
                    card.number = numbers[i];
                    card.suit = suits[j];
                    allCards.Add(card);
                }
            }
            return (allCards);
        }

        //Shuffles cards using Fischer-Yates method
        //https://www.coursera.org/learn/algorithms-part1/lecture/12vcF/shuffling
        //https://stackoverflow.com/questions/273313/randomize-a-listt
        private List<Card> Shuffle(List<Card> cards)
        {
            Random rand = new Random();
            
            List<Card> shuffledCards = new List<Card>(cards); 

            int numCards = shuffledCards.Count;
            for (int i = 0; i < numCards; i++)
            {
                int r = rand.Next(0, i + 1);
                Card temp = shuffledCards[r];
                shuffledCards[r] = shuffledCards[i]; //replaces rth card with 0th card
                shuffledCards[i] = temp; //replaces 0th card with former rth card
            }
            return shuffledCards;
        }

        private void MoveCard(List<Card> from, List<Card> to, int index)
        {
            to.Add(from[index]);
            from.RemoveAt(index);
        }

        private void MovePlayerCard(List<Card> from, List<Card> to, int index, int player)
        {
            if (from.Count == 0)
            {
                ReshufflePlayerDeck(player);
            }
            if (from.Count != 0)
            {
                to.Add(from[index]);
                from.RemoveAt(index);
            }
        }

        private List<Card>[] DealCards(List<Card> cards, int numDecks)
        {
            List<Card> undealtCards = new List<Card>(cards);
            List<Card>[] playerDecks = new List<Card>[numDecks];
            for (int i=0; i<numDecks; i++)
            {
                playerDecks[i] = new List<Card>();
                playerDecksDown[i] = new List<Card>();
            }

            int d = 0;
            int totalCards = undealtCards.Count;
            
            for (int u=0; u<totalCards; u++)
            {
                MoveCard(undealtCards, playerDecks[d], 0);
                
                //Change deck that next card is dealt to
                d = (d < numDecks - 1) ? d+1 : 0;
            }

            return playerDecks;
        }
        
        //Collect cards for battle and keep them in order
        private List<Card> CollectBattleCards()
        {
            List<Card> bcards = new List<Card>();
            
            for (int i = 0; i < numPlayers; i++)
            {
                MovePlayerCard(playerDecks[i], bcards, 0, i);
            }

            if (bcards.Count > 0)
            {
                Player0Card.Content = bcards[0].number.ToString() + " " + bcards[0].suit.ToString();
                Player1Card.Content = bcards[1].number.ToString() + " " + bcards[1].suit.ToString();
            }

            return bcards;
        }

        //Determines victor by comparing the first and last occurrences of the max of a list of cards
        private int CompareBattleCards(List<Card> bcards)
        {
            int[] numbers = new int[bcards.Count];
            int firstMax, lastMax;
            int victor;

            for (int i=0; i<bcards.Count; i++)
            {
                numbers[i] = bcards[i].number;
            }

            int max = numbers.Max();
            firstMax = Array.IndexOf(numbers, max);
            lastMax = Array.LastIndexOf(numbers, max);

            //Find victor or declare war
            if (firstMax != lastMax) //there is a tie
            {
                victor = -1;

            }
            else //there is only one highest card
            {
                victor = firstMax;
                Console.WriteLine("Victor is Player " + victor);
            }

            ShowBattleElements(victor);

            return victor;
        }
        
        private void GiveCards(int victor)
        {
            Console.WriteLine("Number of cards on table: " + tableCards.Count);

            Console.WriteLine("Cards won: ");
            for (int i = 0; i < tableCards.Count; i++)
            {
                Console.WriteLine(tableCards[i].number + " " + tableCards[i].suit);
            }

            int totalTableCards = tableCards.Count;
            
            for (int i=0; i< totalTableCards; i++)
            {
                MoveCard(tableCards, playerDecksDown[victor], 0);
            }
        }

        private void ReshufflePlayerDeck(int player)
        {
            List<Card> shuffleCards = Shuffle(playerDecksDown[player]);
            playerDecksDown[player].Clear();
            playerDecks[player].Clear(); //extra check

            int totalShuffleCards = shuffleCards.Count;
            for (int i = 0; i < totalShuffleCards; i++)
            {
                MoveCard(shuffleCards, playerDecks[player], 0);
            }
            if (totalShuffleCards == 0)
            {
                EndGame();
            }
        }

        private void ShowBattleElements(int victor)
        {
            if (victor == 0)
            {
                Star0.Visibility = Visibility.Visible;
                Star1.Visibility = Visibility.Hidden;
                WarLabel.Visibility = Visibility.Hidden;
            }
            else if (victor == 1)
            {
                Star0.Visibility = Visibility.Hidden;
                Star1.Visibility = Visibility.Visible;
                WarLabel.Visibility = Visibility.Hidden;
            }
            else if (victor == -1)
            {
                Star0.Visibility = Visibility.Hidden;
                Star1.Visibility = Visibility.Hidden;
                WarLabel.Visibility = Visibility.Visible;
            }
            else
            {
                Star0.Visibility = Visibility.Hidden;
                Star1.Visibility = Visibility.Hidden;
                WarLabel.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateCounter()
        {
            int cardCount0 = playerDecks[0].Count + playerDecksDown[0].Count;
            int cardCount1 = playerDecks[1].Count + playerDecksDown[1].Count;

            CardCount0.Content = cardCount0.ToString();
            CardCount1.Content = cardCount1.ToString();
        }
        
        private void EndGame()
        {
            int winner = -1;
            int totalPlayerCards;

            for (int i=0; i<numPlayers; i++)
            {
                totalPlayerCards = playerDecks[i].Count + playerDecksDown[i].Count;
                if (totalPlayerCards == allCards.Count)
                {
                    winner = i;
                }
            }
            Announcement.Content = "Player " + winner + " won!";
            Announcement.Visibility = Visibility.Visible;

            BattleButton.IsEnabled = false;
            StartButton.Visibility = Visibility.Visible;
        }

        private void ManyBattlesLater_Click(object sender, RoutedEventArgs e)
        {
            for (int i=0; i<100; i++)
            {
                BattleButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
