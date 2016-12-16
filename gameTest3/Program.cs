using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Round
            int rd = 1;
            //-----**------
            //Alt alta yazdirmak icin sayac
            int stepByStep = 0;
            //-----**------
            //Players' homes
            int[] homes = new int[16];
            //-----**------
            //Player
            int whichPlayer = 0;
            //-----**------
            //Hareket alanları
            int[] path = new int[56];
            //-----**------
            //Bitis yerleri
            int[] endOfPath = new int[16];
            //-----**------
            //Zar
            int dice = 0;
            //-----**------
            //Random
            Random rnd = new Random();
            int randomNumber = 0;//rastgele gelen sayıları tutmak icin kullanıldı
            //-----**------
            //oyuncunun girdigi degeri tutacak degisken
            int pawn = 0;
            //-----**------
            //Hata icin kullanildi
            bool error = false;
            //-----**------
            //Playerlara round hakkı ver
            bool[] isAddOneRound = new bool[16];//tüm oyuncular icin tanımladık cunku yer degistirirken silinmis olan büyüktür veya kücüktür harflerini tekrar yerine yazmak gerek
            bool playOneRound = false;
            //-----**------
            //Playerleri bir round beklet
            bool[] waitOneRound = new bool[16];//tüm oyuncular icin tanımladık cunku yer degistirirken silinmis olan büyüktür veya kücüktür harflerini tekrar yerine yazmak gerek
            bool waitRound = false;//bunu sadece 1 round bekletmek icin kullandık
            //-----**------
            //ekrana yazdirma stilli
            int lastOfX = 3;//setCursorPosition'nun son kaldığı yeri tutacaktır baslangic degerleri verildi
            int lastOfY = 8;//setCursorPosition'nun son kaldığı yeri tutacaktır baslangic degerleri verildi
            //-----**------
            //yazdırma kısmında kullanılan bazı degiskenler
            int negativePath = 1;
            int addNumber = 0;
            //-----**------
            //oyunun bitisini sorguluyoruz
            bool isGo = true;//oyun biterse false olacak
            //-----**------
            //YAPAY ZEKA
            int homeAIno = 4;//D,E,F... gibi harflerin sayılarını tutuyoruz
            int startPoint = 14;//her oyuncu icin 14 artacak
            bool[] waitOneRoundPC = { false, false, false };//eğer bir round bekleme cezası var ise true olacak ve diğer oyuncu oynayacaktır(player 2,player 3,player 4)
            //------*---------------
            string stringControl;//yanlis girilen degerleri kontrol amacıyla tanımlandı
            //---------------------------------------DEGİSKENLER BURADA BİTİYOR---------------------------------------------------------
            for (int i = 0; i < 16; i++)//Playerların baslangicda duracagi yerlerin dizisi oluşturuldu(65 ascii'de A'ya esittir)
            {
                homes[i] = 65 + i;//Baslangic harfi A oldugu icin 65'den baslanarak 81'e kadar gidiyor
            }
            for (int i = 0; i < path.Length; i++)//Oyuncuların ilerleyecegi yol dizisine noktaları atıyoruz(46 ascii'de noktaya esittir)
            {
                path[i] = 46;//tüm yollar noktalarla dosenecek
            }
            for (int i = 0; i < 16; i++)//oyuncuların sonunda ulasacagi bitis noktlarının dizisi olusturuldu(111 ascii'de o'ya esittir)
            {
                endOfPath[i] = 111;//bitis noktalarının dizisi o'lar ile dosenecek
            }
            for (int i = 0; i < isAddOneRound.Length; i++)//Bunu kullanmamızın sebebi < veya > karakterlerinin üzerine harfler gelebiliyor ve bu harfler yer degistirdiginde <,> karakterleri yerine konmalıdır
            {
                //true olan degerleri kontrol ettirip ona gore islemleri yaptıracagız
                isAddOneRound[i] = false;//baslangic degerlerini false olarak atadık
            }
            for (int i = 0; i < waitOneRound.Length; i++)//Bunu kullanmamızın sebebi < veya > karakterlerinin üzerine harfler gelebiliyor ve bu harfler yer degistirdiginde <,> karakterleri yerine konmalıdır
            {
                //true olan degerleri kontrol ettirip ona gore islemleri yaptıracagız
                waitOneRound[i] = false;//baslangic degerlerini false olarak atadık
            }
            for (int i = 0; i < 3; i++)//3 adım geriye gitme '(' karakteri icin kullanıldı(3 tane olacak)
            {
                do
                {
                    randomNumber = rnd.Next(0, 56);
                    if (randomNumber < 53 && randomNumber > 3)//Alttaki kontrol ile 3 adım ileri gidildiğinde, 3adım geri dönme simgesi varsa yeniden random oluşturmaya sokuyoruz. Yoksa buga giriyor
                    {
                        if (path[randomNumber + 3] != 46 || path[randomNumber - 3] != 46)//noktadan farklı ise yeniden random atanacaktır
                        {
                            randomNumber = 0;//Tekrar random üretmek için 0 değerini verdim(buradaki 0 degeri asagida sorguladımız icin 0'dır 55,54 gibi degerlerde olurdu)
                        }
                    }

                } while (randomNumber == 0 || randomNumber == 1 || randomNumber == 2 ||
                    randomNumber == 14 || randomNumber == 15 || randomNumber == 16 ||
                    randomNumber == 28 || randomNumber == 29 || randomNumber == 30 ||
                    randomNumber == 42 || randomNumber == 43 || randomNumber == 44
                    || path[randomNumber] != 46);//Bu kontrol sayesinde başlangıç noktalarına veya aynı yere tekrar koyma gibi kontrolleri yapıyoruz
                path[randomNumber] = 40;//Burada olusan rastgele sayıyı yol dizisinde bulup oraya geriye '(' karakterini yani 40'ı yazıyoruz
            }
            for (int i = 0; i < 3; i++)//3 adım ileriye gitme ')' karakteri icin kullanıldı(3 tane olacak)
            {
                do
                {
                    randomNumber = rnd.Next(0, 56);
                    if (randomNumber < 53)//Alttaki kontrol ile 3 adım ileri gidildiğinde, 3adım geri dönme simgesi varsa yeniden random oluşturmaya sokuyoruz. Yoksa buga giriyor
                    {
                        if (path[randomNumber + 3] != 46)//noktadan farklı ise yeniden random atanacaktır
                        {
                            randomNumber = 0;//Tekrar random üretmek için 0 değerini verdim(buradaki 0 degeri asagida sorguladımız icin 0'dır 55,54 gibi degerlerde olurdu)
                        }
                    }
                } while (randomNumber == 0 || randomNumber == 55 || randomNumber == 54 || randomNumber == 53 ||
                   randomNumber == 14 || randomNumber == 13 || randomNumber == 12 || randomNumber == 11 || randomNumber == 28 ||
                   randomNumber == 27 || randomNumber == 26 || randomNumber == 26 || randomNumber == 42 || randomNumber == 41 ||
                   randomNumber == 40 || randomNumber == 39 || path[randomNumber] != 46);//Bu kontrol sayesinde başlangıç noktalarına veya aynı yere tekrar koyma gibi kontrolleri yapıyoruz
                path[randomNumber] = 41;//Burada olusan rastgele sayıyı yol dizisinde bulup oraya ileri ')' karakterini yani 41'ı yazıyoruz
            }
            for (int i = 0; i < 2; i++)//Bir round bekleme '<' 
            {
                do
                {
                    randomNumber = rnd.Next(0, 56);
                    if (randomNumber < 53 && randomNumber > 3)//Alttaki kontrol ile 3 adım ileri gidildiğinde, 3adım geri dönme simgesi varsa yeniden random oluşturmaya sokuyoruz. Yoksa buga giriyor
                    {
                        if (path[randomNumber + 3] != 46 || path[randomNumber - 3] != 46)//noktadan farklı ise yeniden random atanacaktır
                        {
                            randomNumber = 0;//Tekrar random üretmek için 0 değerini verdim(buradaki 0 degeri asagida sorguladımız icin 0'dır 55,54 gibi degerlerde olurdu)
                        }
                    }
                } while (randomNumber == 0 || randomNumber == 14 || randomNumber == 28 || randomNumber == 42 || path[randomNumber] != 46);//Bu kontrol sayesinde başlangıç noktalarına veya aynı yere tekrar koyma gibi kontrolleri yapıyoruz
                path[randomNumber] = 60;//Burada olusan rastgele sayıyı yol dizisinde bulup oraya bir round bekle '<' karakterini yani 60'ı yazıyoruz
            }
            for (int i = 0; i < 2; i++)
            {
                do
                {
                    randomNumber = rnd.Next(0, 56);
                    if (randomNumber < 53 && randomNumber > 3)//Alttaki kontrol ile 3 adım ileri gidildiğinde, 3adım geri dönme simgesi varsa yeniden random oluşturmaya sokuyoruz. Yoksa buga giriyor
                    {
                        if (path[randomNumber + 3] != 46 || path[randomNumber - 3] != 46)//noktadan farklı ise yeniden random atanacaktır
                        {
                            randomNumber = 0;//Tekrar random üretmek için 0 değerini verdim(buradaki 0 degeri asagida sorguladımız icin 0'dır 55,54 gibi degerlerde olurdu)
                        }
                    }
                } while (randomNumber == 0 || randomNumber == 14 || randomNumber == 28 || randomNumber == 42 || path[randomNumber] != 46);//Bu kontrol sayesinde başlangıç noktalarına veya aynı yere tekrar koyma gibi kontrolleri yapıyoruz
                path[randomNumber] = 62;//Burada olusan rastgele sayıyı yol dizisinde bulup oraya bir round daha oyna '>' karakterini yani 62'ı yazıyoruz
            }
            do//bir tane X koyacagimiz icin for'a sokmadık
            {
                randomNumber = rnd.Next(0, 56);
                if (randomNumber < 53 && randomNumber > 3)//Alttaki kontrol ile 3 adım ileri gidildiğinde, 3adım geri dönme simgesi varsa yeniden random oluşturmaya sokuyoruz. Yoksa buga giriyor
                {
                    if (path[randomNumber + 3] != 46 || path[randomNumber - 3] != 46)//noktadan farklı ise yeniden random atanacaktır
                    {
                        randomNumber = 0;//Tekrar random üretmek için 0 değerini verdim(buradaki 0 degeri asagida sorguladımız icin 0'dır 55,54 gibi degerlerde olurdu)
                    }
                }
            } while (randomNumber == 0 || randomNumber == 14 || randomNumber == 28 || randomNumber == 42 || path[randomNumber] != 46);//Bu kontrol sayesinde başlangıç noktalarına veya aynı yere tekrar koyma gibi kontrolleri yapıyoruz
            path[randomNumber] = 88;//Burada olusan rastgele sayıyı yol dizisinde bulup oraya baslangic noktasına dön 'X' karakterini yani 88'ı yazıyoruz
                                    //---------------------------------------Rastgele Oluşumların Atanması Burada Bitiyor---------------------------------------------------------

            do
            {
                for (int i = 0; i < endOfPath.Length; i++)//bitip bitmediğini kontolünü yapıyoruz ama döngüden çıkma işini player 1'in üzerinde gerçekleştiriyoruz
                {
                    if (endOfPath[i] != 111 && endOfPath[i + 1] != 111 && endOfPath[i + 2] != 111 && endOfPath[i + 3] != 111)
                    {
                        isGo = false;
                        break;
                    }
                    else
                    {
                        i += 3;
                    }
                }
                if (isGo == false && whichPlayer == 0)
                {
                    whichPlayer = 4;
                }
                else if (isGo == true)
                {
                    whichPlayer++;
                }
                if (whichPlayer == 2 && waitOneRoundPC[0] == true && dice != 6)//0 player 2yi temsil eder herhangi bir pc oyuncusunun bir round bekleme durumu varsa diğer oyuncuya geçilir
                {//2.oyuncunun beklemesi var ise 3.'ye geç
                    waitOneRoundPC[0] = false;
                    whichPlayer++;
                    homeAIno += 4;//oynayan playerin yuva numarasını tutar
                    startPoint += 14;//bir sonraki oyuncunun başlangıç noktasını belirliyoruz
                }
                if (whichPlayer == 3 && waitOneRoundPC[1] == true && dice != 6)//burada else if yapmamamızın sebebi eğer player 2nin beklemesi ve player 3ünde beklemesi varsa player 4'e geçmesini sağlamak
                {//3. oyuncunun beklemesi var ise 4.'ye geç
                    waitOneRoundPC[1] = false;
                    whichPlayer++;
                    homeAIno += 4;//oynayan playerin yuva numarasını tutar
                    startPoint += 14;//bir sonraki oyuncunun başlangıç noktasını belirliyoruz
                }
                if (whichPlayer == 4 && waitOneRoundPC[2] == true && dice != 6)//Eğer player 4'ün beklemesi varsa sıra player 1'e geçer
                {
                    waitOneRoundPC[2] = false;
                    whichPlayer = 1;
                    homeAIno = 4;
                    startPoint = 14;
                    rd++;
                }
                do//bir round oynama hakkı(player 1'in round hakkı varsa)
                {

                    playOneRound = false;
                    lastOfX = 3;//setCursorPosition'nun son kaldığı yeri tutacaktır baslangic degerleri verildi
                    lastOfY = 8;//setCursorPosition'nun son kaldığı yeri tutacaktır baslangic degerleri verildi
                    negativePath = 1;//ters yönde haraketler icin
                    addNumber = 0;
                    dice = rnd.Next(1, 7);//random bir zar attık
                    Console.SetCursorPosition(0, 0 + stepByStep);
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Player 1");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("                  ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("Player 2");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\tRound: " + rd);
                    Console.WriteLine(" A B C D     + - - - +     E F G H\tTurn: Player " + whichPlayer);
                    Console.WriteLine(" + - - +     |       |     + - - +\tDice: " + dice);
                    Console.WriteLine(" |     |     |       |     |     |");
                    Console.WriteLine(" |     |     |       |     |     |");
                    Console.WriteLine(" + - - +     |       |     + - - +");
                    Console.WriteLine("             |       |");
                    Console.WriteLine(" + - - - - - +   #   + - - - - - +");
                    Console.WriteLine(" |               #               |");
                    Console.WriteLine(" |           # # # # #           |");
                    Console.WriteLine(" |               #               |");
                    Console.WriteLine(" + - - - - - +   #   + - - - - - +");
                    Console.WriteLine("             |       |");
                    Console.WriteLine(" + - - +     |       |     + - - +");
                    Console.WriteLine(" |     |     |       |     |     |");
                    Console.WriteLine(" |     |     |       |     |     |");
                    Console.WriteLine(" + - - +     |       |     + - - +");
                    Console.WriteLine(" M N O P     + - - - +     I J K L");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Player 4");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("                   ");
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Player 3");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < path.Length; i++)//yolumuzu cizdiriyoruz
                    {
                        if (i < 6 + addNumber)//oncelike 0-6 arası sonra addNumber ile path[29] üzerinden devam ediyoruz
                        {
                            if (i == 0)//başlama noktaları renklendirildi
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            lastOfX += 2 * negativePath;
                            Console.Write(Convert.ToChar(path[i]));
                        }
                        else if (i < 13 + addNumber)
                        {
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            lastOfY -= 1 * negativePath;
                            Console.Write(Convert.ToChar(path[i]));
                        }
                        else if (i == 13 + addNumber)
                        {
                            lastOfY += 1 * negativePath;
                            lastOfX += 2 * negativePath;
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            Console.Write(Convert.ToChar(path[i]));
                            lastOfX += 2 * negativePath;
                        }
                        else if (i < 20 + addNumber)
                        {
                            if (i == 14)//başlama noktaları renklendirildi
                            {
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else if (i == 42)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;

                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            lastOfY += 1 * negativePath;
                            Console.Write(Convert.ToChar(path[i]));
                        }
                        else if (i < 27 + addNumber)
                        {
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            lastOfX += 2 * negativePath;
                            Console.Write(Convert.ToChar(path[i]));
                        }
                        else if (i == 27 + addNumber)
                        {
                            lastOfY += 1 * negativePath;
                            lastOfX -= 2 * negativePath;
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            Console.Write(Convert.ToChar(path[i]));
                        }
                        else
                        {
                            lastOfY += 1;
                            Console.BackgroundColor = ConsoleColor.Green;//player 3'ün baslangic noktasina rengini veriyoruz
                            Console.ForegroundColor = ConsoleColor.Black;//player 3'ün baslangic noktasina rengini veriyoruz
                            Console.SetCursorPosition(lastOfX, lastOfY + stepByStep);
                            Console.Write(Convert.ToChar(path[i]));
                            lastOfX -= 2 * negativePath;
                            negativePath = -1;
                            addNumber = 28;
                        }

                    }
                    //----------------------------------------Yol yazdırma islemleri bitmistir---------------------
                    lastOfX = 3;//setCursorPosition degerini burdan alacağız
                    lastOfY = 3;//setCursorPosition degerini burdan alacağız
                    for (int i = 0; i < homes.Length; i++)//Harfleri yuvalarına koyacagiz
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                Console.SetCursorPosition(lastOfX + (k * 2), lastOfY + j + stepByStep);
                                Console.Write(Convert.ToChar(homes[i]));
                                i++;
                            }
                        }
                        if (i > 3 && i < 7)
                        {
                            lastOfX = 29;
                            i--;
                        }
                        else if (i > 7 && i < 11)
                        {
                            lastOfX = 29;
                            lastOfY = 14;
                            i--;
                        }
                        else if (i > 11)
                        {
                            lastOfX = 3;
                            i--;
                        }
                    }
                    //------------------------------------------Harfler çizdirildi----------------------------------------
                    lastOfX = 5;//bitis noktları icin kullanıldi
                    lastOfY = 9;//bitis noktları icin kullanıldi
                    negativePath = 1;//player 3 ve player 4lerin bitis noktalarını tersten yazdırırsak isler kolaylasacak
                    for (int i = 0; i < endOfPath.Length; i++)//Bitis noktalarını yazdiriyoruz
                    {
                        if (i > 7)
                        {
                            lastOfX = 29;
                            lastOfY = 9;
                            negativePath = -1;
                            i--;
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            Console.SetCursorPosition(lastOfX + ((j * 2) * negativePath), lastOfY + stepByStep);
                            Console.Write(Convert.ToChar(endOfPath[i]));
                            i++;
                        }
                        if (i > 7)
                        {
                            lastOfX = 17;
                            lastOfY = 15;
                            negativePath = -1;
                        }
                        else
                        {
                            lastOfX = 17;
                            lastOfY = 3;
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            Console.SetCursorPosition(lastOfX, lastOfY + (j * negativePath) + stepByStep);
                            Console.Write(Convert.ToChar(endOfPath[i]));
                            i++;
                        }
                    }
                    //--------------------------------------------Bitiş noklarının yazdırma islemi bitti
                    //-----------------------------------------------------------Bütün yazdırma islemleri bitmiştir.
                    //Bitiş kontrolünü burada yapıyoruz amacımız son halini oyuncuya göstermek

                    if (isGo == false)//birinci do while döngüsünden çıkıyoruz(oyun bitti)
                    {
                        break;
                    }
                    //Bitiş kontolünün bitimi
                    //-----Player oynatma kismi

                    if (whichPlayer == 1)
                    {
                        if (waitRound == true)
                        {
                            waitRound = false;
                            Console.SetCursorPosition(40, 4 + stepByStep);
                            Console.Write("Please wait one round");
                        }
                        else
                        {
                            if (dice == 6)
                            {
                                playOneRound = true;//6 attıysa bir round daha oynama hakkı verilir
                            }
                            else
                            {
                                playOneRound = false;
                            }
                            if (homes[0] == 65 && homes[1] == 66 && homes[2] == 67 && homes[3] == 68 && dice == 6 || homes[0] == 46 || homes[1] == 46 || homes[2] == 46 || homes[3] == 46)//zar 6 ve tüm harfler yuvadaysa veya min bir harf disarda ise
                            {
                                int homeNumber = 0;
                                do//doğru değeri alana kadar döner
                                {
                                    Console.SetCursorPosition(34, 3 + stepByStep);//enter pawn yazılacak yere gidiyor
                                    Console.Write("\tEnter Pawn: ");
                                    stringControl = Console.ReadLine().ToUpper();
                                } while (stringControl != "A" && stringControl != "B" && stringControl != "C" && stringControl != "D");
                                pawn = Convert.ToChar(stringControl);//alınan değer doğru olduğu zaman pawn'a aktarıyoruz
                                switch (pawn)//girilen değere göre islem yaptıracağız
                                {
                                    case 65:
                                        homeNumber = 0;
                                        break;
                                    case 66:
                                        homeNumber = 1;
                                        break;
                                    case 67:
                                        homeNumber = 2;
                                        break;
                                    case 68:
                                        homeNumber = 3;
                                        break;
                                }
                                if (homes[homeNumber] == pawn && dice == 6)//seçilen harf içerde ve zarda 6 geldiyse eğer bu kısma girer
                                {
                                    if (path[0] == 46)//Nokta ise buraya girer
                                    {
                                        path[0] = pawn;
                                        homes[homeNumber] = 46;
                                    }
                                    else if (path[0] > 68 && path[0] < 81)//A,B,C,D disinda bir harf var ise ona göre islem yaptiracagiz
                                    {
                                        homes[path[0] - 65] = path[0];//Burada A,B,C,D nin bir tanesi diger harflerin üzerine gelince diger harfi yuvasına gönderiyoruz
                                        path[0] = pawn;//oynadığımız harfide ilerletiyoruz
                                        homes[homeNumber] = 46;
                                    }
                                    else//no legal move
                                    {
                                        error = true;
                                    }
                                }
                                else if (homes[homeNumber] != pawn)//Eğer girilen harf disari cikmis ise buraya girer
                                {
                                    //-------------------------------------alt kısımda bitiş noktasına giren harflerin hareketleri hakkındadır
                                    for (int i = 0; i < 4; i++)//eğer oynanacak harf bitiş noktalarında ise ona göre hareket ettireceğiz
                                    {
                                        if (endOfPath[i] == pawn)
                                        {
                                            if (endOfPath[i] + dice > 3)
                                            {
                                                if (endOfPath[((i + dice) % 4)] == 111)//Eger oyunun bitis noktasi bos ise yerlestir yoksa hata!!
                                                {
                                                    endOfPath[i] = 111;//harfin olduğu yeri o'ya ceviriyoruz
                                                    endOfPath[((i + dice) % 4)] = pawn;
                                                    break;
                                                }
                                                else
                                                {
                                                    error = true;
                                                    endOfPath[i] = pawn;//harf yerinde kaliyor
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (endOfPath[i + dice] == 111)//Eger oyunun bitis noktasi bos ise yerlestir yoksa hata!!
                                                {
                                                    endOfPath[i] = 111;//harfin olduğu yeri o'ya ceviriyoruz
                                                    endOfPath[i + dice] = pawn;
                                                    break;
                                                }
                                                else
                                                {
                                                    error = true;
                                                    endOfPath[i] = pawn;//harf yerinde kaliyor
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //-----------------------------------------------------------------
                                    for (int i = 0; i < path.Length; i++)//tüm yolu tarıyoruz istenilen harfin konumunu belirlemek icin
                                    {
                                        if (path[i] == pawn)//istenilen harfi buluduğunda buraya girecek
                                        {
                                            if (isAddOneRound[homeNumber] == false && waitOneRound[homeNumber] == false)//Eğer bir round oyna veya bekle komutu yoksa
                                            {
                                                path[i] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                            }
                                            else//round bekleme ve oynama true ise buraya düser
                                            {
                                                if (isAddOneRound[homeNumber] == true)
                                                {
                                                    isAddOneRound[homeNumber] = false;
                                                    path[i] = 62;//bir daha oyna karakterini atadık
                                                }
                                                else if (waitOneRound[homeNumber] == true)
                                                {
                                                    waitOneRound[homeNumber] = false;
                                                    path[i] = 60;//bir round bekle karakterini atadık
                                                }
                                            }
                                            if (i + dice < 56)//Dizinin disina tasmayi engelliyoruz
                                            {
                                                if (path[i + dice] == 46)//tasiyacagimiz yerde nokta varsa direk tasiyoruz
                                                {
                                                    path[i + dice] = pawn;
                                                    break;
                                                }
                                                else if (path[i + dice] == 41)//Tasiyacagimiz yerde ')' uc adim ileri gitme varsa
                                                {
                                                    if (path[i + dice + 3] == 46)//3 adım ileride nokta varsa
                                                    {
                                                        path[i + dice + 3] = pawn;
                                                    }
                                                    else//3 adım ileride kendi harfimiz veya adamın harfi varsa
                                                    {
                                                        if (path[i + dice + 3] == 65 || path[i + dice + 3] == 66 || path[i + dice + 3] == 67 || path[i + dice + 3] == 68)//tasinacagi yerde A,B,C,D varsa hata verecektir.
                                                        {
                                                            error = true;
                                                            path[i] = pawn;//pawn yerinde kaliyor
                                                        }
                                                        else if (path[i + dice + 3] > 68 && path[i + dice + 3] < 81)//A,B,C,D disinda bir harf var ise ona göre islem yaptiracagiz
                                                        {
                                                            homes[path[i + dice + 3] - 65] = path[i + dice + 3];//Burada A,B,C,D nin bir tanesi diger harflerin üzerine gelince diger harfi yuvasına gönderiyoruz
                                                            path[i + dice + 3] = pawn;//oynadığımız harfide ilerletiyoruz
                                                        }
                                                        else
                                                        {
                                                            error = true;
                                                            path[i] = pawn;//pawn yerinde kaliyor
                                                        }
                                                    }
                                                    Console.SetCursorPosition(40, 4 + stepByStep);
                                                    Console.Write("Extra 3 steps");
                                                    break;
                                                }
                                                else if (path[i + dice] == 40)//Tasiyacagimiz yerde '(' uc adim geri gitme varsa
                                                {
                                                    if (path[i + dice - 3] == 46)//3 adım geride nokta varsa
                                                    {
                                                        path[i + dice - 3] = pawn;
                                                    }
                                                    else//3 adım geride kendi harfimiz veya adamın harfi varsa
                                                    {
                                                        if (path[i + dice - 3] == 65 || path[i + dice - 3] == 66 || path[i + dice - 3] == 67 || path[i + dice - 3] == 68)//tasinacagi yerde A,B,C,D varsa hata verecektir.
                                                        {
                                                            error = true;
                                                            path[i] = pawn;//pawn yerinde kaliyor
                                                        }
                                                        else if (path[i + dice - 3] > 68 && path[i + dice - 3] < 81)//A,B,C,D disinda bir harf var ise ona göre islem yaptiracagiz
                                                        {
                                                            homes[path[i + dice - 3] - 65] = path[i + dice - 3];//Burada A,B,C,D nin bir tanesi diger harflerin üzerine gelince diger harfi yuvasına gönderiyoruz
                                                            path[i + dice - 3] = pawn;//oynadığımız harfide ilerletiyoruz
                                                        }
                                                    }
                                                    Console.SetCursorPosition(40, 4 + stepByStep);
                                                    Console.Write("Extra -3 steps");
                                                    break;
                                                }
                                                else if (path[i + dice] == 62)//1 defa daha oyna'>'
                                                {
                                                    isAddOneRound[homeNumber] = true;//Buna göre sorgumuzu yapacağız(sonra > işaretini yerine koymamız lazım onun için)
                                                    playOneRound = true;//bir round daha dönmesini bunu true yaparak sağlıyoruz
                                                    path[i + dice] = pawn;//> üzerine gelir ve onu yok eder bunu sonra hareket ettirdiğinde yerine koymak gerek
                                                    Console.SetCursorPosition(40, 4 + stepByStep);
                                                    Console.Write("Play one more time");
                                                    break;
                                                }
                                                else if (path[i + dice] == 60)//1 round bekle '<'
                                                {
                                                    waitOneRound[homeNumber] = true;//Buna göre sorgumuzu yapacağız
                                                    path[i + dice] = pawn;//< üzerine gelir ve onu yok eder bunu sonra hareket ettirdiğinde yerine koymak gerek
                                                    waitRound = true;
                                                    break;
                                                }
                                                else if (path[i + dice] == 88)
                                                {
                                                    //Baslangic noktasina geri don
                                                    if (path[0] != 46)//Eger baslangic noktasında baska bir tas varsa ondan sonra gelen ilk bos yere yerlestir
                                                    {
                                                        for (int j = 0; j < path.Length; j++)
                                                        {
                                                            if (path[j] == 46)
                                                            {
                                                                path[j] = pawn;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else//baslangic noktası bos ise yerlestir
                                                    {
                                                        path[0] = pawn;
                                                        break;
                                                    }
                                                }
                                                else if (path[i + dice] == 65 || path[i + dice] == 66 || path[i + dice] == 67 || path[i + dice] == 68)//tasinacagi yerde A,B,C,D varsa hata verecektir.
                                                {
                                                    error = true;
                                                    path[i] = pawn;//pawn yerinde kaliyor
                                                    break;
                                                }
                                                else if (path[i + dice] > 68 && path[i + dice] < 81)//A,B,C,D disinda bir harf var ise ona göre islem yaptiracagiz--Adam yeme
                                                {
                                                    if (isAddOneRound[path[i + dice] - 65] == true)//eğer adam <,> harflerinin üzerindeyken yediyse, yenen harfin bekleme ve oynama harfleri ile ilgili kısmı false yaptık
                                                    {
                                                        isAddOneRound[path[i + dice] - 65] = false;
                                                        isAddOneRound[homeNumber] = true;//yiyen harfin bekleme ve oynama harfleri ile ilgili kısmı true yaptık
                                                    }
                                                    else if (waitOneRound[path[i + dice] - 65] == true)//eğer adam <,> harflerinin üzerindeyken yediyse, yenen harfin bekleme ve oynama harfleri ile ilgili kısmı false yaptık
                                                    {
                                                        waitOneRound[path[i + dice] - 65] = false;
                                                        waitOneRound[homeNumber] = true;//yiyen harfin bekleme ve oynama harfleri ile ilgili kısmı true yaptık
                                                    }
                                                    homes[path[i + dice] - 65] = path[i + dice];//Burada A,B,C,D nin bir tanesi diger harflerin üzerine gelince diger harfi yuvasına gönderiyoruz
                                                    path[i + dice] = pawn;//oynadığımız harfide ilerletiyoruz
                                                    break;
                                                }
                                            }
                                            else//Yolun sonuna yerleştirme işlemleri yapılacaktır
                                            {
                                                if (dice - ((path.Length - 1) - i) > 4)//4'den büyükse
                                                {
                                                    if (endOfPath[4 - dice % 4] == 111)//Eger oyunun bitis noktasi bos ise yerlestir yoksa hata!!
                                                    {
                                                        endOfPath[4 - dice % 4] = pawn;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        error = true;
                                                        path[i] = pawn;//harf yerinde kaliyor
                                                        break;
                                                    }
                                                }
                                                else//4'den küçükse direk yerlestiriyoruz
                                                {
                                                    if (endOfPath[(dice - (path.Length - i)) % 4] == 111)//Eger oyunun bitis noktasi bos ise yerlestir yoksa hata!!
                                                    {
                                                        endOfPath[(dice - (path.Length - i)) % 4] = pawn;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        error = true;
                                                        path[i] = pawn;//harf yerinde kaliyor
                                                        break;
                                                    }
                                                }
                                            }
                                        }//<==path[i]==pawn'ın parantezi
                                    }//<==for'un parantezi
                                }
                                else//6 atmadan dışarı çıkarmak isterse buraya girer
                                {
                                    error = true;
                                }
                            }//<==zar ve oyuncu kontrol if'nin parantezi
                        }//<==bir round beklemenin else'nin
                    }//<==whichPlayer'ın IF'inin
                    //---------------------------------------------------------------------Error verdirme kısmı
                    if (error)//Error varsa eğer hata mesajımızı yazdırıyoruz
                    {
                        Console.SetCursorPosition(40, 4+stepByStep);
                        Console.Write("No legal move!");
                        error = false;
                    }
                    stepByStep += 22;
                } while (playOneRound);
                //-----------------------------------------------Player 1 için işlemler bitti
                //-----------YAPAY ZEKA
                //startpoint oyuncuların baslangic noktalarını tutar
                //homeAIno playerların harflerinin index degerlerini tutar orn. E harfi 4'dur ve bu deger ilk baslangicdaki degeridir, her oyuncuda bu deger 4 arttırılır.
                int lastPoint = 0;
                bool isPlay = false;//oyuncunun oynayıp oynamadığını kontrol eder

                if (whichPlayer != 1 && whichPlayer != 0)//sıra player 2,3,4'de ise buraya girer
                {

                    System.Threading.Thread.Sleep(1000);
                    if (homes[homeAIno] != 46 && homes[homeAIno + 1] != 46 && homes[homeAIno + 2] != 46 && homes[homeAIno + 3] != 46 && dice == 6)//harf yuvaları noktadan farklıysa ve zar 6 geldiyse
                    {
                        if (path[startPoint] == 46)//başlangıç noktası nokta(boş) ise buraya girer
                        {
                            path[startPoint] = 65 + homeAIno;//ilk karakteri baslangic noktasına atadık
                            homes[homeAIno] = 46;//karakterin yuvasındaki yere nokta atadık
                            isPlay = true;
                        }
                        else if (path[startPoint] > 64 && path[startPoint] < 82 && path[startPoint] < 65 + homeAIno || path[startPoint] > 64 && path[startPoint] < 82 && path[startPoint] > 65 + (homeAIno + 4))
                        {//Başlangıç noktasında diğer adamın harfleri varsa onları yiyecektir
                            homes[homeAIno] = 46;//karakterin yuvasındaki yere nokta atadık
                            homes[path[startPoint] - 65] = path[startPoint];//üstüne gelinen harfi yuvasına gönderiyoruz
                            path[startPoint] = 65 + homeAIno;//asıl harfi baslangic noktasına yerlestiriyoruz
                            isPlay = true;
                        }
                    }
                    else if (homes[homeAIno] == 46 || homes[homeAIno + 1] == 46 || homes[homeAIno + 2] == 46 || homes[homeAIno + 3] == 46)
                    {//yuvadaki harflerin bir tanesi bile dışarda ise
                        if (dice == 6)
                        {//zar 6 geldiyse eğer bir tane harfi dışarı çıkaracağız
                            for (int i = 0; i < 4; i++)//yuvadaki 4 harfede bakılır
                            {
                                if (homes[homeAIno + i] != 46)//eğer harfin yerinde nokta varsa işlem yapmaz
                                {
                                    //Bunun içine girdiyse harfi başlangıç noktasına atamaya hazırdır
                                    if (path[startPoint] == 46)//başlangic noktası boş ise harfi başlangic noktasına koyar
                                    {
                                        path[startPoint] = 65 + homeAIno + i;//karakteri baslangic noktasına atadık
                                        homes[homeAIno + i] = 46;//yuvadaki harfin bulunduğu yeri noktaya çevirir
                                        isPlay = true;//6 attığı için bir hak daha verilir fakat bu en alt satırda kontrol ediliyor bundan sonraki adımlar es geçmesi için isPlay=true'dur.
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    //--------------------------------------------Asıl yapay zeka
                    int layer = 0;//aşağıdaki işlemlerin sırasıyla yapılması için kullanılacak
                                  /*Hangi harfi oynayacağını belirlemeliyiz(
                                  oncelikler
                                      0-Bitiş noktasına girebilecek harfler
                                      1-Adam Yeme
                                      2-Bir defa daha oynama
                                      3-Üç adım ileri
                                      4-Nokta ise git
                                      5-Üç adım geri git
                                      6-Bir round bekle
                                      7-Başlangıç noktasına dön*/
                    bool overComeSP = false;//eğer harf son noktalardan birinde ve bitis noktalarına giremiyorsa true olacak
                    int forJ = 0;//bunu hangi harfin oynandığını döngüden sonra görmek için kullandık.
                    if (isPlay == false)//yukarda bir işlem yapmadıysa buraya girecektir.--------Burası harflerden biri bitiş noktasına yerleşebilecekse onu yapması için kullanıldı.
                    {
                        do
                        {
                            for (int i = 0; i < path.Length; i++)//oynayacak oyuncunun harflerinin yol üzerinde arayacağız bu yüzden yolun uzunluğu kadar for kullandık
                            {
                                for (int j = 0; j < 4; j++)//her oyuncunun 4 harfi olduğu için for'u 4 kere döndürdük
                                {
                                    if (path[i] == 65 + homeAIno + j)//harf bulununca yapılacaklar
                                    {
                                        //bazı yerlerde lastPoint bazı yerlerde i kullanılacaktır(zar eklenen yerlerde i, eklenmeyen yerlerde lastPoint kullanıldı)
                                        if (i + dice > 55)//eğer zar ile i'nin toplamı 55'den büyükse hata verir bunu engellemek için bu kısmı yazdık
                                        {
                                            lastPoint = i;//harfin ayrıldığı son nokta
                                            i = (i - path.Length);//negatif bir deger cikar ve zarı eklediğimizde tam istediğimiz yere gider
                                        }
                                        else if (i + dice < 55 && i > 0)//eğer zar ile i'nin toplamı 55'den küçükse ve i>0'dan lastPoint=i
                                        {
                                            lastPoint = i;
                                        }
                                        else
                                        {
                                            lastPoint = i;//yukardaki şartlar uymuyorsa
                                        }
                                        //---------bitiş noktasına yerleştirebiliyorsa yerleştir.
                                        /*burada path[i] yol üzerindeki noktaları, harfleri,diğer simgeleri gösteriyor.
                                    65 Ascii karakterlerde A'ya denk gelir bunun üzerine homeAIno'yu eklediğimiz 
                                    biz o an ki oyuncunun ilk harf numarasını verir +j ile de sonraki harflerini buluruz*/
                                        if (lastPoint < startPoint && lastPoint + dice >= startPoint)//zar ile bitiş noktasına geldiyse yerleştiriyoruz
                                        {
                                            if ((dice - ((startPoint - 1) - lastPoint)) > 4)//4'den büyükse (-1 vermemizin sebebi homeAIno 4,8,12 olabiliyor, pathOfEnd'lerde 4,8,12 ile başlıyor zar 1 gelirse 4,8,12'ye yerleşmesi gerekiyor o yüzden)
                                            {
                                                if (endOfPath[(homeAIno - 1) + ((dice - ((startPoint - 1) - lastPoint)) % 4)] == 111)//Eger oyunun bitis noktasi bos ise
                                                {
                                                    endOfPath[(homeAIno - 1) + ((dice - ((startPoint - 1) - lastPoint)) % 4)] = 65 + homeAIno + j;
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    isPlay = true;
                                                    forJ = j;
                                                }
                                                else
                                                {
                                                    overComeSP = true;//eğer harf son noktalardan birinde ve bitis noktalarına giremiyorsa true olacak
                                                }
                                            }
                                            else//4'den küçükse direk yerlestiriyoruz
                                            {
                                                if (endOfPath[(homeAIno - 1) + (dice - ((startPoint - 1) - lastPoint))] == 111)//Eger oyunun bitis noktasi bos ise yerlestir
                                                {
                                                    endOfPath[(homeAIno - 1) + (dice - ((startPoint - 1) - lastPoint))] = 65 + homeAIno + j;
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    isPlay = true;
                                                    forJ = j;
                                                }
                                                else
                                                {
                                                    overComeSP = true;//eğer harf son noktalardan birinde ve bitis noktalarına giremiyorsa true olacak
                                                }
                                            }
                                        }
                                        //------------------------------------------------------------------------------------------------------------------------------------------------------
                                        if (overComeSP == false)
                                        {
                                            if (layer == 1)//diğer adamları yeme kısmı
                                            {
                                                if (path[i + dice] > 64 && path[i + dice] < 82 && path[i + dice] < 65 + homeAIno || path[i + dice] > 64 && path[i + dice] < 82 && path[i + dice] > 65 + (homeAIno + 4))//diger adamların harflerini ye
                                                {
                                                    if (isAddOneRound[path[i + dice] - 65] == true)//eğer adam <,> harflerinin üzerindeyken yediyse, yenen harfin bekleme ve oynama harfleri ile ilgili kısmı false yaptık
                                                    {
                                                        isAddOneRound[path[i + dice] - 65] = false;
                                                        isAddOneRound[homeAIno + j] = true;//yiyen harfin bekleme ve oynama harfleri ile ilgili kısmı true yaptık
                                                    }
                                                    else if (waitOneRound[path[i + dice] - 65] == true)//eğer adam <,> harflerinin üzerindeyken yediyse, yenen harfin bekleme ve oynama harfleri ile ilgili kısmı false yaptık
                                                    {
                                                        waitOneRound[path[i + dice] - 65] = false;
                                                        waitOneRound[homeAIno + j] = true;//yiyen harfin bekleme ve oynama harfleri ile ilgili kısmı true yaptık
                                                    }
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    homes[path[i + dice] - 65] = path[i + dice];//üstüne gelinen harfi yuvasına gönderiyoruz
                                                    path[i + dice] = 65 + homeAIno + j;//biglisayar uygun bulduğu harfi ilerletiyor
                                                    isPlay = true;//oyuncu oyunu oynadıysa true olur
                                                    forJ = j;
                                                }

                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 2) //bir round daha oyna kısmı
                                            {
                                                if (path[i + dice] == 62)//1 defa daha oyna'>'
                                                {
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    isAddOneRound[homeAIno + j] = true;//Buna göre sorgumuzu yapacağız(sonra > işaretini yerine koymamız lazım onun için)
                                                    path[i + dice] = 65 + homeAIno + j;//> üzerine gelir ve onu yok eder bunu sonra hareket ettirdiğinde yerine koymak gerek
                                                    Console.SetCursorPosition(40, 4 + stepByStep - 22);
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    playOneRound = true;//Bununla tekrar oynamasını sağlayacağız en alt kısımda kontrol ettireceğiz
                                                    Console.Write("Play one more time");
                                                    isPlay = true;//oynadıysa
                                                    forJ = j;
                                                }

                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 3)//uc adim ileri gitme
                                            {
                                                if (path[i + dice] == 41)//Tasiyacagimiz yerde ')' uc adim ileri gitme varsa
                                                {
                                                    if (path[i + dice + 3] == 46)//noktaysa 3 ileri git(burada taşma durumu olabilir bir kontrol et sonra)
                                                    {
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        path[i + dice + 3] = 65 + homeAIno + j;
                                                        Console.SetCursorPosition(40, 4 + stepByStep - 22);
                                                        Console.Write("Extra 3 steps");
                                                        isPlay = true;
                                                        forJ = j;
                                                    }
                                                    //Eğer 3 adım ileri gittiğinde adamın taşı varsa üzerine atla
                                                    else if (path[i + dice + 3] > 64 && path[i + dice + 3] < 82 && path[i + dice + 3] < 65 + homeAIno || path[i + dice + 3] > 64 && path[i + dice + 3] < 82 && path[i + dice + 3] > 65 + (homeAIno + 4))//diger adamların harflerini ye
                                                    {
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        homes[path[i + dice + 3] - 65] = path[i + dice + 3];//üstüne gelinen harfi yuvasına gönderiyoruz
                                                        path[i + dice + 3] = 65 + homeAIno + j;//biglisayar uygun bulduğu harfi ilerletiyor
                                                        isPlay = true;//oyuncu oyunu oynadıysa true olur
                                                        forJ = j;
                                                    }
                                                }
                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 4)//nokta varsa
                                            {
                                                if (path[i + dice] == 46)//Tasiyacagimiz yerde nokta var ise oraya yerleştir
                                                {
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    path[i + dice] = 65 + homeAIno + j;
                                                    isPlay = true;
                                                    forJ = j;
                                                }
                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 5)//üc adım geriye gitme
                                            {
                                                if (path[i + dice] == 40)//Tasiyacagimiz yerde '(' uc adim geri gitme varsa
                                                {
                                                    if (path[i + dice - 3] == 46)//noktaysa 3 geriye git
                                                    {
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        path[i + dice - 3] = 65 + homeAIno + j;
                                                        Console.SetCursorPosition(40, 4 + stepByStep - 22);
                                                        Console.Write("Extra -3 steps");
                                                        isPlay = true;
                                                        forJ = j;
                                                    }
                                                    //Eğer 3 adım geri gittiğinde adamın taşı varsa üzerine atla
                                                    else if (path[i + dice - 3] > 64 && path[i + dice - 3] < 82 && path[i + dice - 3] < 65 + homeAIno || path[i + dice - 3] > 64 && path[i + dice - 3] < 82 && path[i + dice - 3] > 65 + (homeAIno + 4))//diger adamların harflerini ye
                                                    {
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        homes[path[i + dice - 3] - 65] = path[i + dice - 3];//üstüne gelinen harfi yuvasına gönderiyoruz
                                                        path[i + dice - 3] = 65 + homeAIno + j;//biglisayar uygun bulduğu harfi ilerletiyor
                                                        isPlay = true;//oyuncu oyunu oynadıysa true olur
                                                        forJ = j;
                                                    }
                                                }
                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 6)//1 round bekle
                                            {
                                                if (path[i + dice] == 60)//1 round bekle '<'
                                                {
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    waitOneRound[homeAIno + j] = true;//Buna göre sorgumuzu yapacağız
                                                    path[i + dice] = 65 + homeAIno + j;//< üzerine gelir ve onu yok eder bunu sonra hareket ettirdiğinde yerine koymak gerek
                                                    path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                    Console.SetCursorPosition(40, 4 + stepByStep - 22);
                                                    waitOneRoundPC[whichPlayer - 2] = true;//-2 yapmamız sebebi which player 2,3,4 olabilir dizimiz 0,1,2'ye kadar tutabiliyor yani dizinin sıfırncı indexi player 2'yi temsil eder.
                                                    Console.Write("Wait one round");
                                                    isPlay = true;
                                                    forJ = j;
                                                }
                                            }
                                            //------------------------------------------------------------------------------------------------------------------------------------------------------
                                            if (layer == 7)//başlangıç noktasına geri dön
                                            {
                                                if (path[i + dice] == 88)//Baslangic noktasina geri don
                                                {
                                                    if (path[startPoint] > 64 && path[startPoint] < 82 && path[startPoint] < 65 + homeAIno || path[startPoint] > 64 && path[startPoint] < 82 && path[startPoint] > 65 + (homeAIno + 4))
                                                    {//adamın taşı varsa ise adamın taşını ye
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        homes[path[startPoint] - 65] = path[startPoint];//üstüne gelinen harfi yuvasına gönderiyoruz
                                                        path[startPoint] = 65 + homeAIno + j;//kendi taşımızı yerleştiriyoruz
                                                        isPlay = true;
                                                        forJ = j;
                                                    }
                                                    else if (path[startPoint] == 46)//baslangic noktası bos ise yerlestir
                                                    {
                                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                                        path[startPoint] = 65 + homeAIno + j;
                                                        isPlay = true;
                                                        forJ = j;
                                                    }
                                                }
                                            }
                                        }

                                        //------------------------------------------------------------------------------------------------------------------------------------------------------

                                        if (isPlay == true || i < 0)//Eğer oynaydıysa boşuna döngüyü çalıştırmıyoruz ve çıkıyoruz,i negatif ise döngüden çıkıyoruz
                                        {
                                            break;
                                        }
                                    }//path[i] == 65 + homeAIno + j'nin
                                }//<==j'nin
                                if (isPlay == true && layer != 2 && layer != 6)
                                {
                                    //ayrılacağı yerde büyüktür veya küçüktür harfi varsa bunu yerine koyacağız

                                    if (isAddOneRound[homeAIno + forJ] == true && layer != 1)//&&layer!=1 yazmamızın sebebi adamı yediğinde, ayrıldığı noktaya <,> işareti koymaması için.
                                    {
                                        isAddOneRound[homeAIno + forJ] = false;
                                        path[lastPoint] = 62;//bir daha oyna karakterini atadık
                                    }
                                    else if (waitOneRound[homeAIno + forJ] == true && layer != 1)
                                    {
                                        waitOneRound[homeAIno + forJ] = false;
                                        path[lastPoint] = 60;//bir round bekle karakterini atadık
                                    }
                                    else
                                    {
                                        path[lastPoint] = 46;//harfin'nin bulunduğu yeri noktaya cevir
                                    }
                                }
                                overComeSP = false;
                                if (isPlay == true)//Eğer oynaydıysa boşuna döngüyü çalıştırmıyoruz ve çıkıyoruz,i negatif ise döngüden çıkıyoruz
                                {
                                    break;
                                }
                                if (i < 0)
                                {
                                    break;
                                }
                            }//<==path.Lenght'in
                            layer++;
                            if (layer > 7)//eğer 7 durumu kontrol edip oynamadıysa while'dan çıkmak için isPlay=true yapıyoruz
                            {
                                isPlay = true;
                            }
                        } while (isPlay == false);//eğer oynamadıysa tekrar tekrar dönecektir.
                    }
                    if (dice == 6 || playOneRound == true)//zar 6 ise tekrar oyna veya bir round hakkı varsa bir daha oyna
                    {
                        playOneRound = false;
                        whichPlayer--;
                    }
                    else
                    {
                        homeAIno += 4;//oynayan playerin yuva numarasını tutar
                        startPoint += 14;//bir sonraki oyuncunun başlangıç noktasını belirliyoruz
                    }

                    if (whichPlayer == 4)//Birinci oyuncuya geri donuyoruz
                    {
                        whichPlayer = 0;
                        homeAIno = 4;
                        startPoint = 14;
                        rd++;//4 oyuncu oynadıktan sonra roundu bir arttırıyoruz
                    }
                    if (stepByStep > 8000)//9000lerde sınırı doluyor bu yüzden 8000'in üzerine çıkınca ekranı sıfırlayacak böylelikle baştan yazmaya devam edecek
                    {
                        Console.Clear();
                        stepByStep = 0;
                    }

                }//<==whichPlayer!=0,whichPlayer!=1'in parantezi
            } while (isGo);
            if (whichPlayer == 0)
            {
                whichPlayer = 4;
            }
            //Tebrik mesajı
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < i; j += 2)
                {
                    Console.SetCursorPosition(50 - j, 11 + i + stepByStep);
                    Console.Write(" ");
                    Console.SetCursorPosition(50 + j, 11 + i + stepByStep);
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(51, 12 + stepByStep);
            Console.Write("   Congratulations! Player " + whichPlayer + "   ");
            //--------------------------------------------------------------
            Console.Read();
        }
    }
}
