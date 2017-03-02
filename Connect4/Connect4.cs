using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    public static class Connect4
    {

        private static bool wrzucane_od_gory = true;          //  true - beda wrzucane od gory ; false - wybor wierz i kolumna
        private static int szer;                      // szerokosc
        private static int wys;                      // wysokosc
        private static int rzad;                    // ilosc w rzedzie by wygrac 
        private static int player = 1;                 // ktory player gra ? 1 - czlowiek ; 2 - komputer
        private static int ilosc_ruchow = 0;          // ile ruchow dokonano do tej pory w grze (licznik)
        private static int ilosc_pol;            // ilosc pol na planszy
        private static bool mozna_wygrac = true;       // czy mozliwe jest rozstrzygniecie gry

        private static List<int> kolejka = new List<int>();
        private static List<List<int>> plansza = new List<List<int>>();



        public static void Witaj()
        {

            Console.Write("\n\n   ----- >   Witaj w grze  Connect 4 !! < ----- \n\n");
            Console.Write(" Ile w rzedzie by wygrac ? ");
            rzad = Convert.ToInt32(Console.ReadLine());
            Console.Write(" Podaj wymiary planszy ? Najpierw szerokosc ? ");
            szer = Convert.ToInt32(Console.ReadLine());
            Console.Write(" Teraz wysokosc ? ");
            wys = Convert.ToInt32(Console.ReadLine());

            Console.Write("\n\n   ----- >   Witaj w grze  Connect 4 !! < ----- \n\n Uzyskaj " + rzad +
                          " takie same znaki w rzedzie by wygrac :) \n\n");
            Console.Write("Rozpoczynasz gre, jesli chcesz aby zaczal ja jednak komputer wcisnij 0 oraz ENTER"
                          + " Powodzenia :) \n");


            ilosc_pol = szer*wys; // ilosc miejsc na planszy 


            // Plansza w postaci wektora wektorow o odpowiednim rozmiarze, 
            // na poczatku wszystkie pola ustawimy na wartosc '0' 
            for (int x = 0; x < szer; x++)
            {
                plansza.Add(new List<int>());

                for (int y = 0; y < wys; y++)
                    plansza[x].Add(0);

            }
        }



        public static void Czyja_kolej()
        {
            if (player == 1)
                Console.WriteLine(" Twoj ruch czlowieku :   ");
            else
                Console.WriteLine(" Poczekaj, komputer mysli nad ruchem .... ");
        }

        public static void Ruch_czlowieka()
        {
            int x, y = 0;
            bool zm;

            do
            {
                Console.Write("\n Podaj numer kolumny do ktorej wrzucasz :  ");
                x = Convert.ToInt32(Console.ReadLine());

                // jesli wybrano 0, to zacznie komputer
                if (x == 0)
                    Ruch_komputera();
                else if (!wrzucane_od_gory)  // gdy wrzucamy od gory to nie wybieramy wiersza
                {
                    Console.WriteLine("\nPodaj numert wiersza: ");
                    y = Convert.ToInt32(Console.ReadLine());
                    --y;
                }
             
            }
            while (x!=0 && !Wrzuc_zeton(--x, y));
        }



        public static void Ruch_komputera()
        {
            Znajdz_wygranego(false);

        }

        public static void Ruch_Losowy()
        {

            int x, y;
            Random r = new Random();

            do
            {
                x =  r.Next() % szer;   // losuje pozycje do wrzucenia
                y =  r.Next() % wys;
            }
            while (!Wrzuc_zeton(x, y));


        }

        public static void Rysuj_plansze()
        {
           Console.WriteLine();


            for (int y = 0; y < wys + 1; y++)
            {
                for (int x = 0; x < szer + 1; x++)
                {
                    if (x == 0)
                    {
                        if (y < 10)
                            Console.Write(" " + y + " ");
                        else
                            Console.Write(" ");
                    }
                    else if (y == 0)
                    {
                        if (x < 10)
                            Console.Write(" " + x + " ");
                        else
                            Console.Write(" ");
                    }
                    else
                    {
                        // zamiast zer wyswietla puste pola          
                        if (plansza[x - 1][y - 1] == 0)
                            Console.Write("   ");
                        else
                            Console.Write(" " + plansza[x - 1][y - 1] + " ");
                    }


                    if (x < szer)
                        Console.Write("|");
                }

               Console.WriteLine();
               Console.WriteLine();
            }

        }


        public static bool Czy_plansza_pelna()
        {
            Przelacz_gracza();

            if (ilosc_ruchow >= ilosc_pol)
            {
                Console.Write("\n MAMY REMIS !!!\n\n");
                return true;
            }

            return false;
        }



        public static void Przelacz_gracza()
        {
            ilosc_ruchow++;

            if (++player > 2)
                player = 1;

        }

        public static bool Czy_mozna_wygrac()
        {
            //if (!mozna_wygrac)
            //{
            //    Console.Write("\n Ta gra nie bedzie miala rozstrzygniecia, koncze gre !\n\n");
            //    return false;
            //}
            return true;
        }


        public static bool Wrzuc_zeton(int x, int y)
        {

            if (x < 0 || x > szer - 1 || y < 0 || y > wys - 1 || plansza[x][y] != 0 )
                return false;    // wybor niemozliwy


            if (wrzucane_od_gory) // gdy wrzucamy od gory to ponizsza procedura to robi :
                for (y = 0; y < wys - 1 && plansza[x][y + 1]==0; y++) { }

            // wkladamy zeton na dane miejsce
            plansza[x][y] = player;

            return true;
        }


        public static void Kto_wygral(int player)
        {
            if (player == 1)
                Console.Write("WYGRAŁEŚ BRAWO !!");
            else
                Console.Write(" NIESTETY, KOMPUTER OKAZAL SIE LEPSZY -> PRZEGRALES !!");

        }

        public static bool Znajdz_wygranego(bool ZnajdzRuch)
        {

            int ZetonGracza;   // informacja jaki symbol ma zeton aktualnie obslugiwanego gracza
            int IleDoKonca;     //  rola flagi


            if (ZnajdzRuch)
                IleDoKonca = 0;               // szukamy tylko czy ktoś już wygrał grę
            else if (ilosc_ruchow >= 2)
                IleDoKonca = rzad;             // kazdy zrobil juz swoj ruch 
            else
                IleDoKonca = rzad - 1;        // to pierwszy ruch komputera, bedzie on losowy bo tak chcemy, wiec recznie ustawimy taka wartosc


            /*
            Tworzymy za pomocą prostej listy kolejność, którego gracza ruch symulujemy jako pierwszy.
            Oczywiście jest to bierzący, czyli komputer, bo teraz jego kolej.
            */

           // std::vector<int>::iterator it;
            kolejka.Clear();

            for (int i = 1; i <= 2; i++)
            {
                if (player == i) // bierzacy gracz na poczatku listy ( w symulacji AI to bedzie komputer)
                    kolejka.Insert(0, player);
                else
                    kolejka.Add(i); // drugi na koncu

            }


            //////////////////////////////////     Algorytm AI ////////////////////////////////////


            for (int IleBrakujeByWygrac = 0; IleBrakujeByWygrac <= IleDoKonca; IleBrakujeByWygrac++)
            {

                for (int i = 0; i < 2; i++)
                {

                    ZetonGracza = kolejka[i]; // ktorego gracza ruch symulujemy w danej chwili


                    for (int x = 0; x < szer; x++)
                    {
                        for (int y = 0; y < wys; y++)
                        {   // Szukamy w ktora strone mozna by 'szukac szczescia' 
                            if (SzukajLinii(x, y, +0, +1, IleBrakujeByWygrac, ZetonGracza) ||   // pion
                                SzukajLinii(x, y, +1, +0, IleBrakujeByWygrac, ZetonGracza) ||   // poziom
                                SzukajLinii(x, y, +1, +1, IleBrakujeByWygrac, ZetonGracza) ||   // skos gora-prawo
                                SzukajLinii(x, y, -1, +1, IleBrakujeByWygrac, ZetonGracza))     // skos lewo- gora
                            {

                                // Czy wygrał ktoś ? 
                                if (IleBrakujeByWygrac == 0)
                                {
                                    Kto_wygral(player);
                                    return true;
                                }
                                else
                                {    // info jaka taktyke wybrano, wynikajaca z tego komu blizej do wygranej
                                    if (ZetonGracza == player)    // szybciej wygra komputer to atakuje
                                        Console.Write("\nRuch ofensywny - by wygrac!\n\n");
                                    else                          // szybciej czlowiek , wiec trzeba obronic sie
                                        Console.Write("\nRuch defensywny - by nie przegrac!\n\n");

                                    return true;
                                }

                            }
                        }
                    }
                }
            }



            if (ZnajdzRuch == false)
            {    // gdy nie znaleziono ruchu umozliwiajacego wygrac

                // nie byl to pierwszy ruch, wiec juz nie dojdzie do czyjejs wygranej
                if (ilosc_ruchow >= 2)
                    mozna_wygrac = false;
                else
                {
                    Console.Write("\n Dokonuje losowego wyboru : ");
                    Ruch_Losowy();
                }
            }

            return false;

        }



        public static bool SzukajLinii(int x, int y, int x_kier, int y_kier, int IleBrakujeByWygrac, int ZetonGracza)
        {

            // czy nie przekroczymy zakresu planszy przy sprawdzaniu
            if (x + x_kier * (rzad - 1) < 0 || x + x_kier * (rzad - 1) >= szer
                || y + y_kier * (rzad - 1) < 0 || y + y_kier * (rzad - 1) >= wys)
            {

                return false;
            }


            int puste = 0;    // ilosc wolnych miejsc w rozpatrywanym rzedzie
            int nowe_x = 0;    // wspolrzedne pustego miejsca na ktore mozna wrzucic zeton
            int nowe_y = 0;

            for (int i = 0; i < rzad; i++)    // ta petla wykona sie cala poprawnie wtedy i tylko wtedy gdy w rzedzie bedzie wygrana 
            {                                 // lub brakowac bedzie (rzad-IleBrakujeByWygrac) zetonow


                /* W tej petli wyszukuje sie nastepny mozliwy ruch gracza poprzez szukanie 
                  pustego miejsca na planszy obok kilku jego zetonow.
                  Jesli zmienna IleBrakujeByWygrac=0, wowczas sprawdzane
                  jest czy ktos juz wygral rozgrywke. */



                if (plansza[x + i * x_kier][y + i * y_kier] == 0 && ++puste <= IleBrakujeByWygrac)
                {
                    // wspolrzedne pustego miejsca na planszy obok zetona/żetonów gracza
                    nowe_x = x + i * x_kier;
                    nowe_y = y + i * y_kier;
                }
                else if (plansza[x + i * x_kier][y + i * y_kier] != ZetonGracza)
                {
                    // Jezeli napotkamy w rozpatrywanym rzedzie żeton przeciwnika, 
                    // to juz wiemy na pewno ze tutaj nie odniesiemy sukcesu
                    // zwracamy false, musimy szukac gdzie indziej

                    return false;
                }

            }


            // Wrzucamy zeton gracza na znalezione miejsce w celu zwiekszenia rzędu jego żetonow, tj. przyblizenia go do wygranej
            Wrzuc_zeton(nowe_x, nowe_y);

            return true;

        }
    }
}
