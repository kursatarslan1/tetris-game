using System;
using System.Drawing;
using System.Windows.Forms;

namespace tetrisGame
{
    public partial class Form1 : Form
    {
        class Shape
        {
            public int x;
            public int y;
            public int[,] matrix;
            public int[,] sonrakiMatrix;
            public int sizeMatrix;
            public int sizeSonrakiMatrix;

            public int[,] tetromino1 = new int[4, 4]  // iki boyutlu dizileri kullanarak tetromino şekillerini oluşturuyoruz.
            {
                {0,1,0,0 },
                {0,1,0,0 },
                {0,1,0,0 },
                {0,1,0,0 },
            };

            public int[,] tetromino2 = new int[3, 3] 
            {
                {0,2,0 },
                {0,2,2 },
                {0,0,2 },
            };

            public int[,] tetromino3 = new int[3, 3] 
            {
                {3,3,3 },
                {0,3,0 },
                {0,0,0 },
            };

            public int[,] tetromino4 = new int[3, 3] 
            {
                {4,0,0 },
                {4,0,0 },
                {4,4,0 },
            };

            public int[,] tetromino5 = new int[2, 3] 
            {
                {5,5,0 },
                {5,5,0 },
            };

            public int[,] tetromino6 = new int[3, 3]
            {
                {0,6,0 },
                {0,6,0 },
                {6,6,0 },
            };

            public int[,] tetromino7 = new int[3, 3]
            {
                {0,7,0 },
                {7,7,0 },
                {7,0,0 },
            };

            public Shape(int _x, int _y)
            {
                x = _x;
                y = _y;
                matrix = MatrixOlustur();
                for(int i = 0; i <= matrix.Length; i++) // for döngüsünde serbest
                {
                    sizeMatrix = (int)Math.Sqrt(i);
                }
                //sizeMatrix = (int)Math.Sqrt(matrix.Length);
                sonrakiMatrix = MatrixOlustur();
                for (int i = 0; i <= sonrakiMatrix.Length; i++) 
                {
                    sizeSonrakiMatrix = (int)Math.Sqrt(i);
                }
                //sizeSonrakiMatrix = (int)Math.Sqrt(sonrakiMatrix.Length);
            }

            public void SekliResetle(int _x, int _y)  
            {
                x = _x;
                y = _y;
                matrix = sonrakiMatrix;
                for (int i = 0; i <= matrix.Length; i++)
                {
                    sizeMatrix = (int)Math.Sqrt(i);
                }
                //sizeMatrix = (int)Math.Sqrt(matrix.Length);
                sonrakiMatrix = MatrixOlustur();
                for (int i = 0; i <= sonrakiMatrix.Length; i++)
                {
                    sizeSonrakiMatrix = (int)Math.Sqrt(i);
                }
                //sizeSonrakiMatrix = (int)Math.Sqrt(sonrakiMatrix.Length);
            }

            public int[,] MatrixOlustur() 
            {
                
                int[,] _matrix = tetromino1; // ilk başta varsayılan olarak tetromino1 i seçtim
                Random rassal;
                rassal = new Random(); // rastgele bir sayı oluşturdum.
                switch (rassal.Next(1, 8)) // gelen sayılara göre tetromino şekillerim oluşacak.
                {
                    case 1:
                        _matrix = tetromino1;
                        break;
                    case 2:
                        _matrix = tetromino2;

                        break;
                    case 3:
                        _matrix = tetromino3;

                        break;
                    case 4:
                        _matrix = tetromino4;

                        break;
                    case 5:
                        _matrix = tetromino5;

                        break;
                    case 6:
                        _matrix = tetromino6;

                        break;
                    case 7:
                        _matrix = tetromino7;

                        break;
                }
                return _matrix;
            }

            public void SekliDönder() // şekli döndürüyoruz.
            {
                int[,] geciciM = new int[sizeMatrix, sizeMatrix]; // iki boyutlu boş bir dizi oluşturdum matriximin boyutunda.
                for (int i = 0; i < sizeMatrix; i++)
                {
                    for (int j = 0; j < sizeMatrix; j++) // şeklimi dolaşacak bir iç içe for döngüsü kurdum.
                    {
                        geciciM[i, j] = matrix[j, (sizeMatrix - 1) - i]; // ve boş matriximi yönünü burada çevirdim. yerlerini değiştirdim.
                    }
                }
                matrix = geciciM; // elde ettiğim geçici olan matriximi, asıl matrixime atadım.
                int o = (9 - (x + sizeMatrix));
                if (o < 0)
                {
                    for (int i = 0; i < Math.Abs(o); i++) 
                    {
                        MoveLeft();
                    }
                }
                if (x < 0)
                {
                    for (int i = 0; i < Math.Abs(x) + 1; i++) 
                    {
                        MoveRight();
                    }
                }
            }

            public void MoveDown()
            {
                y++; // Şeklim devamlı olarak aşağı iner
            }
            public void MoveRight()
            {
                x++; // sağa hareket         
            }
            public void MoveLeft()
            {   
                x--;// sola hareket                   
            }
        }
        
        Shape kullanilanSekil; // Bu şeklimiz oyunumuzda yukarıdan başlayıp tabana veya herhangi bir şekle değene kadar aşağı hareket eden şeklimizdir ve işlemlerimizi bu şekile göre yaparız.
        int boyut;
        readonly int[,] map = new int[20, 9]; // Oyun haritamız
        readonly int[,] nextShapeMap = new int[4, 4]; // Sonraki şeklimizin haritası
        int skor;
        int butonClickKontrol = 0; // bu değişkenim sadece 1 veya 0 tutuyor, bool olarak da kullanılabilir.
        readonly int Interval = 225; // timer1 , yani oyun içi hızımız.
        int sekilOlusturmaSayisi  = 0;
        int kaybetmeSayisi = 0;
        bool degerTut = false;

        public Form1()
        {
            InitializeComponent();
            kullanilanSekil = new Shape(3, 0);  // Şeklimizi oluşturduk.
            this.KeyUp += new KeyEventHandler(TusFonksiyonlari);
            this.Text = "Tetris";
        }
        
        private void TusFonksiyonlari(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Down:
                    timer1.Interval = 300;
                    break;
            }
        }
        protected override bool ProcessDialogKey(Keys KeyData) // Yeniden başlat butonu aktif olduğundan keyDown yön tuşlarını yakalayamıyor. Ama bu yöntem klavye tuşlarına öncelik veriyor 
        {                                                     // Bu sayede yön tuşlarını kullanabiliyoruz.
            if (KeyData == Keys.Down)
            {
                timer1.Interval = 15;
            }
            else if (KeyData == Keys.Left)
            {
                if (!YanlariKontrolEt(-1))
                {
                    HaritayiSifirla();
                    kullanilanSekil.MoveLeft();
                    Sekil();
                    Invalidate();
                }
            }
            else if(KeyData == Keys.Right)
            {
                if (!YanlariKontrolEt(1))
                {
                    HaritayiSifirla(); // her harekette arenamızı sıfırlıyoruz ki bir sonraki hareketmizde bir önceki hareketimiz arenamızda kalmasın. iz bırakmamalıyız!!
                    kullanilanSekil.MoveRight();
                    Sekil(); // bir önceki hareketimizi sildikten sonra yeni şeklimize bakmalıyız.
                    Invalidate();
                }
            }
            else if (KeyData == Keys.Up)
            {
                if (!KesisiyorMu()) // eğer etrafındaki bir şekille kesişmiyor ise up tuşuna bastığımız zaman döndürür. fakat herhangi bir şekil ile kesişiyorsa hiçbir şey yapmaz.
                {
                    HaritayiSifirla();
                    kullanilanSekil.SekliDönder();
                    Sekil();
                    Invalidate(); //Denetimin bu bölgesini geçersiz kılar ve Paint iletisinin denetime gönderilmesine neden olur.
                }
            }
            else if (KeyData == Keys.P)
            {
                if(degerTut == false)
                {
                    timer1.Stop();
                    timer2.Stop();
                    degerTut = true;
                }
                else if (degerTut == true)
                {
                    timer1.Start();
                    timer2.Start();
                    degerTut = false;
                }
                
            }
            else if (KeyData == Keys.Space)
            {
                button2.PerformClick();
            }
            return base.ProcessDialogKey(KeyData);
        }
        public void Basla()
        {  
            boyut = 25;
            skor = 0;

            if(sekilOlusturmaSayisi == 0) // Bunu yapma sebebim eğer bu olmazsa oyun bittiği zaman başka şekiller oluşturuyor ve oyuna başla dediğimiz zaman yeniden şekil oluşturuyor
            {                            // Bu durumu önlemek amacıyla böyle bir yapıya gerek duydum.
                kullanilanSekil = new Shape(3, 0);
                sekilOlusturmaSayisi = 1;
            }
            
            label1.Text = skor.ToString();

            timer1.Interval = Interval;
            timer1.Tick += new EventHandler(Update); // timer.Tick Bu fonksiyon timer'ın intervalde belirtilen zamanda yapılmasını istediğimiz komutları yazdığımız yerdir.
            timer1.Start();

            timer2.Interval = 1000; // Süremiz arttıkça puanımız da artar.
            timer2.Start();

            Invalidate();
        }

        

        public void SonrakiSekliGöster(Graphics e) // Sonraki gelecek olan şekli ekrana gösterir.
        {
            for (int i = 0; i < kullanilanSekil.sizeSonrakiMatrix; i++)
            {
                for (int j = 0; j < kullanilanSekil.sizeSonrakiMatrix; j++)
                {
                    if (kullanilanSekil.sonrakiMatrix[i,j] == 1)
                    {
                        e.FillRectangle(Brushes.Brown, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Orange, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Gray, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.DarkBlue, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                    if (kullanilanSekil.sonrakiMatrix[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(300 + j * (boyut) + 1, 330 + i * (boyut) + 1, boyut - 1, boyut - 1)); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler
                    }
                }
            }
        }

        public void YeniOyun()
        {
            boyut = 25;
            skor = 0;

            kullanilanSekil = new Shape(3, 0);     
            label1.Text = skor.ToString();

            timer1.Interval = Interval;
            timer1.Start();

            timer2.Interval = 1000;
            timer2.Start();
 
            /*       Harita aslında aşağıdaki 2 for döngüsü ile temizlenir.      */
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    map[i, j] = 0; // iki boyutlu dizim ile şekilleri oluşturdum  1 olan yerleri belirttiğim renge, 0 olan yerleri ise boş bıraktım. Bu for döngüsü tüm haritayı boş bırakıyor.
                }
            }

            Sekil();

            button1.Enabled = false;
            button2.Enabled = false; 

            Invalidate(); 
        }

        private void Update(object sender, EventArgs e)
        {
            HaritayiSifirla();

            if (!YereVeyaTavanaDegdiMi())
            {
                kullanilanSekil.MoveDown(); // Yere veya tavana değmediyse şeklimiz aşağı inmeye devam eder.
            }
            else
            {              
                Sekil();
                TetrisYap();
                timer1.Interval = Interval;
                kullanilanSekil.SekliResetle(3, 0);
                if (YereVeyaTavanaDegdiMi())
                {
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            map[i, j] = 0;
                        }
                    }
                    
                    if(kaybetmeSayisi == 0) // Oyuna her ilk başladığımızda ve yeni oyun dediğimizde oluşan ilk 2 şekil aynı oluyordu fakat oyunu kaybettikten sonra böyle bir sorun olmuyor.
                    {                      // Ben de oyuna başla dediğimizde oyunu kaybettiriyorum ve sonrasında button1.PerformClick(); komutu ile butona tekrar bastırıp oyunu başlatıyorum.
                        timer1.Tick -= new EventHandler(Update); // Eğer kaybetme sayım 0 değilse o zaman ekrana MessageBox ile kaybettiniz yazdırıyorum. Bunu yapma sebebim ise butona ilk bastığımda
                        timer1.Stop();                          // ekrana kaybettiniz yazdırmaması.
                        timer2.Stop();
                        butonClickKontrol = 0;
                        skor = 0;
                        label1.Text = skor.ToString();
                        button1.Enabled = true;
                        button1.Visible = true;
                        button1.PerformClick(); // bunun yerine button1 içerisindeki işlemleri buraya yazabiliriz. 
                        kaybetmeSayisi = 1;
                    }
                    else
                    {
                        timer1.Tick -= new EventHandler(Update);
                        timer1.Stop();
                        timer2.Stop();                   
                        MessageBox.Show("Kaybettiniz!!", "Bilgilendirme Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        butonClickKontrol = 0;
                        skor = 0;
                        label1.Text = skor.ToString();
                        button1.Enabled = true;
                        button1.Visible = true;
                    } 
                }
            }
            Sekil();
            Invalidate();
        }

        public bool KesisiyorMu() //Etrafındaki herhangi bir şekille kesişip kesişmediğini kontrol eder ve kesişmiyor ise şekli dönderir.
        {                        
            for (int i = kullanilanSekil.y; i < kullanilanSekil.y + kullanilanSekil.sizeMatrix; i++)
            {
                for (int j = kullanilanSekil.x; j < kullanilanSekil.x + kullanilanSekil.sizeMatrix; j++)
                {
                    if (j >= 0 && j <= 7)
                    {
                        if (map[i, j] != 0 && kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x] == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void TetrisYap()  // Burada full olan satırları silme görevini gerçekleştiriyoruz.
        {
            int count = 0; // bir tane sayacımız var
            for (int i = 0; i < 20; i++)  // 17 satırımız var ama şekillerimiz yukarıdan geliyor.
            {
                count = 0; 
                for (int j = 0; j < 9; j++)
                {
                    if (map[i, j] != 0) // tek tek karelere bakıyor eğer aynı satırda count sayısı 9 olursa o satır full doludur demektir. 
                    {
                        count++;
                    }                 
                }
                if (count == 9) // eğer burada tetris olacaksa
                {
                    for (int k = i; k >= 1; k--)
                    {
                        for (int e = 0; e < 9; e++)
                        {
                            map[k, e] = map[k - 1, e]; // bütün satırları bir aşağı indiriyoruz.
                        }
                    }
                    skor += 100; // puanımıza 100 puan ekleniyor .
                }
            }
        }

        public void Sekil()  
        {
            for (int i = kullanilanSekil.y; i < kullanilanSekil.y + kullanilanSekil.sizeMatrix; i++)
            {
                for (int j = kullanilanSekil.x; j < kullanilanSekil.x + kullanilanSekil.sizeMatrix; j++)
                {
                    if(kullanilanSekil.matrix[i-kullanilanSekil.y, j - kullanilanSekil.x]!=0)
                    {
                        map[i, j] = kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x];
                    }                  
                }
            }
        }


        public bool YereVeyaTavanaDegdiMi() 
        {
            if(kaybetmeSayisi == 0) // Burada oyuna ilk başladığımda ilk 2 şekil aynı geliyordu fakat kaybedip tekrar başladığımda böyle bir sorunum yoktu.
            {                      // Bende oyuna başlar başlamaz oyunu kaybettiriyorum ve "oyuna başla" butonuna kod ile basarak normal bir şekilde oyuna başlıyorum.
                return true;
            }
            else
            {
                for (int i = kullanilanSekil.y + kullanilanSekil.sizeMatrix - 1; i >= kullanilanSekil.y; i--)
                {
                    for (int j = kullanilanSekil.x ; j < kullanilanSekil.x + kullanilanSekil.sizeMatrix ; j++)
                    {
                        if (kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x] != 0)
                        {
                            if (i  == 17)
                            {
                                return true;
                            }
                            if (map[i + 1, j] != 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }
            


        public bool YanlariKontrolEt(int ade) // Yanlara gelip gelmediğini kontrol ediyor. Bu fonksiyonum çok önemli çünkü eğer yanlardaki duvara yakınsa dönmeyecek. Bu durumu kontrol ettiriyorum.
        {
            for (int i = kullanilanSekil.y; i < kullanilanSekil.y + kullanilanSekil.sizeMatrix; i++)
            {
                for (int j = kullanilanSekil.x; j < kullanilanSekil.x + kullanilanSekil.sizeMatrix; j++)
                {
                    if (kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x] != 0) 
                    {
                        if (j + 1 * ade > 8 || j + 1 * ade < 0) 
                        {
                            return true;
                        }

                        if (map[i, j + 1 * ade] != 0) 
                        {
                            if (j - kullanilanSekil.x + 1 * ade >= kullanilanSekil.sizeMatrix || j - kullanilanSekil.x + 1 * ade < 0)
                            {
                                return true;
                            }
                            if (kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x + 1 * ade] == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void HaritayiSifirla() // her hareketimizde bir önceki hareketimizden kalan yeri siler. // Hemen fonksiyonun bitiminde örnek vereceğim. // (yılan oyununda yaptığım gibi)
        {
            for (int i = kullanilanSekil.y; i < kullanilanSekil.y + kullanilanSekil.sizeMatrix; i++)
            {
                for (int j = kullanilanSekil.x; j < kullanilanSekil.x + kullanilanSekil.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < 20 && j < 9)
                    {
                        if (kullanilanSekil.matrix[i - kullanilanSekil.y, j - kullanilanSekil.x] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }                   
                }
            }
        }

        /* HaritayiSifirla Fonksiyonu Örnek

            1 1 1 1    bu benim tetromino 1 şeklimin yan hali düz olduğu zaman  1 1 1 1  olacak ve biz yanda kalan kısımları ve her aşağı indiğinde arkasında bıraktığı şekilleri siliyoruz.
                                                                                      1 
                                                                                      1
                                                                                      1  
                                                                                 
        */
        public void HaritayaSekilleriCiz(Graphics e) // Harita üzerinde şekillerimizi çiziyoruz. Burada aslında kare (dikdörtgen ama kare) dolduruyoruz.
        {
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (map[i, j] == 0)
                    {
                        e.FillRectangle(Brushes.Black, new Rectangle(0 + j * (boyut), 0 + i * (boyut), boyut, boyut));
                    }
                    if (map[i, j] == 1)  // FillRectangle kullandığımızda 5 parametre vermeliyiz : Renk, ve 4 köşe
                    {
                        e.FillRectangle(Brushes.Brown, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut )); // + 1 ve - 1 ler aradaki kareler arasındaki düz çizgiler // tetromino şekil 1
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut ));  // tetromino şekil 2
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut ));  // tetromino şekil 3
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Orange, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut )); // tetromino şekil 4
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Gray, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut )); // tetromino şekil 5
                    }
                    if (map[i, j] == 6)
                    {
                        e.FillRectangle(Brushes.DarkBlue, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut )); // tetromino şekil 6
                    }
                    if (map[i, j] == 7)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(0 + j * (boyut) , 0 + i * (boyut) , boyut , boyut ));  // tetromino şekil 7
                    }
                }
            }
        }

        public void HaritayiCiz(Graphics g) // haritayı çiziyoruz, form 1 üzerine OnPaint fonksiyonunda(640. satır) aktarıcaz.
        {
            for (int i = 0; i <= 18 ; i++) // Haritamızı çiziyoruz 17, 9 luk bir harita olacağından 18 tane yan çizgi 9 tane de düz çizgi çizeceğiz.
            {
                if(i == 0 || i == 18)
                {
                    g.DrawLine(Pens.Black, new Point(0, 0 + i * boyut), new Point(0 + 9 * boyut, 0 + i * boyut));
                }
                else
                {
                    continue;
                    //g.DrawLine(Pens.Black, new Point(50, 50 + i * boyut), new Point(50 + 9 * boyut, 50 + i * boyut)); 
                }   
            }
            for (int i = 0; i <= 9; i++)
            {
                if(i == 0 || i == 9)
                {
                    g.DrawLine(Pens.Black, new Point(0 + i * boyut, 0), new Point(0 + i * boyut, 0 + 18 * boyut));
                }
                else
                {
                    continue;
                    //g.DrawLine(Pens.Black, new Point(50 + i * boyut, 50), new Point(50 + i * boyut, 50 + 17 * boyut)); 
                }  
            }
        }

        public void SonrakiSekliHaritadaGoster(Graphics g) // Haritayı Çizdik sadece form üzerine aktarmak kaldı onu da OnPaint adını verdiğimiz fonksiyonda gerçekleştiricez.
        {
            for (int i = 0; i <= 4; i++) // sonraki şekli göstereceğim parça 4 e 4 lük olacak bu yüzden 2 boyutlu bir dizi daha oluşturdum ve aşağıdaki fonksiyonla çizimimi sağladım
            {
                if (i == 0 || i == 4)
                {
                    g.DrawLine(Pens.Blue, new Point(275, 330 + i * boyut), new Point(275 + 4 * boyut, 330 + i * boyut));
                    g.DrawLine(Pens.Blue, new Point(276, 331 + i * boyut), new Point(276 + 4 * boyut, 331 + i * boyut));
                    g.DrawLine(Pens.Blue, new Point(277, 332 + i * boyut), new Point(277 + 4 * boyut, 332 + i * boyut));
                    g.DrawLine(Pens.Blue, new Point(278, 333 + i * boyut), new Point(278 + 4 * boyut, 333 + i * boyut));
                }
                //g.DrawLine(Pens.Black, new Point(305, 330 + i * boyut), new Point(305 + 4 * boyut, 330 + i * boyut));  // dik çizgilerin çizilmesi ve konumu,
            }
            for (int i = 0; i <= 4; i++)  
            {
                if (i == 0 || i == 4)
                {
                    g.DrawLine(Pens.Blue, new Point(275 + i * boyut, 330), new Point(275 + i * boyut, 329 + 4 * boyut));
                    g.DrawLine(Pens.Blue, new Point(276 + i * boyut, 331), new Point(276 + i * boyut, 331 + 4 * boyut));
                    g.DrawLine(Pens.Blue, new Point(277 + i * boyut, 332), new Point(277 + i * boyut, 332 + 4 * boyut));
                    g.DrawLine(Pens.Blue, new Point(278 + i * boyut, 333), new Point(278 + i * boyut, 333 + 4 * boyut));
                }
                //g.DrawLine(Pens.Black, new Point(305 + i * boyut, 330), new Point(305 + i * boyut, 330 + 4 * boyut)); // Yan çizgilerin çizilmesi ve konumu.
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)  // Çizdiklerimizi form1 üzerine aktarıyoruz.
        {
            HaritayiCiz(e.Graphics); // form1 üzerine aktardık, oyuna başladığımız zaman harita form1 üzerinde gözüekcez.
            HaritayaSekilleriCiz(e.Graphics); // form1 üzerine aktardık, oyuna başladığımız zaman harita form1 üzerinde gözüekcez.
            SonrakiSekliGöster(e.Graphics); // form1 üzerine aktardık, oyuna başladığımız zaman harita form1 üzerinde gözüekcez.
            SonrakiSekliHaritadaGoster(e.Graphics); // form1 üzerine aktardık, oyuna başladığımız zaman harita form1 üzerinde gözüekcez.
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            butonClickKontrol = 1; // Oyunu hiç oynamadan yeniden başlat butonuna basıldığında oyunu başlatmaması ve ekrana bir mesaj vermesi için oluşturduğum bir değişken.
            HaritayiSifirla();    // Eğer "Oyunu Başlat" butonuna hiç basılmadıysa "Yeniden Başlat" butonuna basıldığı zaman içerisinde bulunan yazıyı ekrana çıktı olarak verecek.
            Basla();

            button1.Enabled = false;  
            button2.Enabled = true;
            button1.Visible = false;
            
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (butonClickKontrol == 0) // buton 1 e basma sayımı kontrol ediyorum, eğer 0 ise aşağıdaki mesajı ekrana çıktı olarak veriyor.
            {
                MessageBox.Show("Oyunu yeniden başlatabilmeniz için ilk önce oyuna başlamanız gerekmektedir.", "Bilgilendirme Penceresi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                kaybetmeSayisi = 0; // kaybetme sayımı 0 yapıyorum ve her yeni oyun dediğimde ilk 2 şeklin aynı gelmesini önlüyorum.
                YeniOyun(); // eğer basma sayım 0 değilse de YeniOyun fonksiyonumu çalıştırıyor yani oyunum yeniden başlıyor.  
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            skor++; // her saniye skorumu arttırıyorum.
            label1.Text = skor.ToString(); // ekrana yazdırıyorum.            
        }
    }
}
