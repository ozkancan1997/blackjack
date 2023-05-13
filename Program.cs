// See https://aka.ms/new-console-template for more information

MainMenu();

static void MainMenu(){
    int input;
    Console.WriteLine("1. New Game");
    Console.WriteLine("2. Change Player");
    Console.WriteLine("3. New Player");
    Console.WriteLine("4. Exit");
    while(!GetRange(4).Contains(input = GetInput("Your Choice: "))){
        Console.WriteLine(input);
        MainMenu();
    }
    switch(input){
        case 1:
            NewGame(); break;
        default:
            break;
    }
}


static int[] GetRange(int range) {
    int[] _range = new int[range+1];
    for(int i = 1 ; i<range+1 ; i++){
        _range[i] = i;
    }
    foreach(int i in _range){
        Console.WriteLine(i);
    }
    return _range;
}

static int GetInput(string message){
    Console.Write(message);
    string? input=Console.ReadLine();
    return input==null?0:Int32.Parse(input);
}

static void NewGame(){
    Console.WriteLine("hello");
}

public class Game{
    int state;
    List<Card> player_hand;
    List<Card> pc_hand;
    Deck deck;

    string[] players;

    string main_menu = "1. New Game\n2. Change Player\n3. Create Player\n4. Exit";


    private class Deck{
        List<int> cards = new List<int>();

        public Deck(){
            for(int i=0 ; i<52; i++){
                cards.Add(i);
            }
        }

        public Card Draw(){
            Random rd = new Random();
            int index = rd.Next(52);
            while(!cards.Contains(index)){
                index = rd.Next(52);
            }
            Card card = new Card(index);
            cards.Remove(index);
            return card;
        }

        
    }

    private class Card{
            String sing;
            int number;

            public Card(int number){
                int signum = (number+1)/13;
                this.sing = GetSign(signum);
                this.number = number+1;
            }

            private string GetSign(int signum) {
                switch(signum){
                    case 0:
                        return "♠";
                    case 1:
                        return "♥";
                    case 2:
                        return "♦";
                    default:
                        return "♣";
                }
            }

            public override string ToString(){
                return sing+number;
            }

            public static int operator+ (int a, Card b){
                return a + b.number;
            }


        }

    private void Print(){
        switch(state){
            case 1:
                PrintTable();
                break;
            default:
                PrintMainMenu();
                break;
        }
    }

    private void GameplayLoop(){
            switch(state){
            case 1:
                deck = new Deck();
                player_hand = new List<Card>();
                pc_hand = new List<Card>();
                players = new string[]{"Player", "Computer"};
                Play();
                break;
            default:
                state = Choice(5);
                break;
        }    

    }

    private void PrintMainMenu(){
        Console.WriteLine(main_menu);
    }

    private void PrintTable(){
        string pc="";
        string pl="";
        foreach(Card c in pc_hand){
            pc+=c;
        }

        foreach(Card c in player_hand){
            pl+=c;
        }
        
        Console.WriteLine("Opponen Hand: " + pc);
        Console.WriteLine("Your Hand: " + pc);
        Console.Write("1. Draw   2. Hold");

    }

    private int Choice(int range){
        int input = Int32.Parse(Console.ReadLine());
        while( input<1 || input>=range){
            input = Int32.Parse(Console.ReadLine());
        }
        return input;
    }

    private void Play(){
        pc_hand.Add(deck.Draw());
        pc_hand.Add(deck.Draw());
        player_hand.Add(deck.Draw());
        player_hand.Add(deck.Draw());
        PlayerTurn();
        if(state==0){
            return;
        }
        ComputerTurn();
        VersusCheck();
    }

    private void PlayerTurn(){
        Print();
        while(true){
            int ch = Choice(3);
            if(ch==1){
                player_hand.Add(deck.Draw());
            }
            if(ch==2){
                break;
            }
            if(HandCheck(player_hand, 0)){
                break;
            }
        }

    }

    private void ComputerTurn(){

    }

    private bool HandCheck(List<Card> hand, int player){
        int sum = HandSum(hand);

        if(sum==21){
            Console.WriteLine("It is 21");
            GameOver(1, player);
            return true;
        }else if(sum>21){
            Console.WriteLine("Bust!");
            GameOver(2, player);
            return true;
        }
        return false;
        
    }

    private void VersusCheck(){
        if(HandSum(player_hand)==HandSum(pc_hand)){
            Console.WriteLine("Push");
            GameOver(3, 0);
        }else if(HandSum(player_hand)>HandSum(pc_hand)){
            GameOver(1, 0);
        }else{
            GameOver(1, 1);
        }
    }

    private int HandSum(List<Card> hand){
        int sum=0;
        foreach(Card c in hand){
            sum=sum+c;
        }
        return sum;
    }

    private void GameOver(int hand_state, int player){   
        if(hand_state==1){
            Console.WriteLine(players[player]+" Wins!");
        }else if(hand_state==2){
            Console.WriteLine(players[(player+1)%2]+" Wins");
        }else if(hand_state==3){
            Console.WriteLine("Draw Game");
        }
        state=0;
    }

}