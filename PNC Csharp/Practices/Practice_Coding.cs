using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Serialization.Formatters.Binary;//Binary Serialization
using System.IO;//Stream
using PNC_Csharp.Practices;

namespace PNC_Csharp
{
    public partial class Practice_Coding : Form
    {
        private static Practice_Coding Instance;
        public static Practice_Coding getInstance()
        {
            if (Instance == null)
                Instance = new Practice_Coding();

            return Instance;
        }
        public static bool IsIstanceNull()
        {
            if (Instance == null)
                return true;
            else
                return false;
        }

        public static void DeleteInstance()
        {
            Instance = null;
        }
        private Practice_Coding()
        {
            InitializeComponent();
        }
        private void Practice_Coding_Load(object sender, EventArgs e)
        {
            //(Directory.GetCurrentDirectory() + "\\Practice\\SJH.txt");
        }
        private void button_Hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            f1.GB_Status_AppendText_Nextline("---Normal (No Casting)---", Color.Purple);
            Tallguy tallguy = new Tallguy("Jonghyun Shin",176);
            f1.GB_Status_AppendText_Nextline(tallguy.Talkaboutyourself(), Color.Red);
            f1.GB_Status_AppendText_Nextline(tallguy.FunnyThingIHave, Color.Red);
            f1.GB_Status_AppendText_Nextline(tallguy.Honk(), Color.Red);
            
            Scarayguy scaryguy = new Scarayguy();
            f1.GB_Status_AppendText_Nextline(scaryguy.FunnyThingIHave, Color.Blue);
            f1.GB_Status_AppendText_Nextline(scaryguy.ScraryThingIHave, Color.Blue);
            f1.GB_Status_AppendText_Nextline(scaryguy.Honk(), Color.Blue);

            Scary_Tallguy scary_tallguy = new Scary_Tallguy(tallguy.name, tallguy.height);
            f1.GB_Status_AppendText_Nextline(scary_tallguy.Talkaboutyourself(), Color.Green);
            f1.GB_Status_AppendText_Nextline(scary_tallguy.FunnyThingIHave, Color.Green);
            f1.GB_Status_AppendText_Nextline(scary_tallguy.ScraryThingIHave, Color.Green);
            f1.GB_Status_AppendText_Nextline(scary_tallguy.Honk(), Color.Green);
            f1.GB_Status_AppendText_Nextline("", Color.Purple);

            f1.GB_Status_AppendText_Nextline("---Up casting---", Color.Purple);
            ICrown[] Temp = new ICrown[3];
            Temp[0] = tallguy; //Up casting
            Temp[1] = scaryguy;//Up casting
            Temp[2] = scary_tallguy;//Up casting

            f1.GB_Status_AppendText_Nextline(Temp[0].FunnyThingIHave, Color.Red);
            f1.GB_Status_AppendText_Nextline(Temp[0].Honk(), Color.Red);
            f1.GB_Status_AppendText_Nextline(Temp[1].FunnyThingIHave, Color.Blue);
            f1.GB_Status_AppendText_Nextline(Temp[1].Honk(), Color.Blue);
            f1.GB_Status_AppendText_Nextline(Temp[2].FunnyThingIHave, Color.Green);
            f1.GB_Status_AppendText_Nextline(Temp[2].Honk(), Color.Green);
            f1.GB_Status_AppendText_Nextline("", Color.Purple);

            f1.GB_Status_AppendText_Nextline("---Down Casting---", Color.Purple);
            Scary_Tallguy Temp_Scary = Temp[2] as Scary_Tallguy; //Down Casting
            f1.GB_Status_AppendText_Nextline(Temp_Scary.Talkaboutyourself(), Color.Black);
            f1.GB_Status_AppendText_Nextline(Temp_Scary.FunnyThingIHave, Color.Black);
            f1.GB_Status_AppendText_Nextline(Temp_Scary.ScraryThingIHave, Color.Black);
            f1.GB_Status_AppendText_Nextline(Temp_Scary.Honk(), Color.Black);
            f1.GB_Status_AppendText_Nextline("", Color.Purple);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("A", "B", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (result == DialogResult.OK)
            {
                MessageBox.Show("Ok is Clicked");
            }
            else
            {
                MessageBox.Show("????");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("A", "B", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
            if (result == DialogResult.OK)
            {
                MessageBox.Show("Ok is Clicked");
            }
            else if (result == DialogResult.Cancel)
            {
                MessageBox.Show("Cancel is Clicked");
            }
            else
            {
                MessageBox.Show("????");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("A", "B", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            if(result == DialogResult.Retry)
            {
                MessageBox.Show("Retry is Clicked");
                button3.PerformClick();
            }
            else if (result == DialogResult.Cancel)
            {
                MessageBox.Show("Cancel is Clicked");
            }
            else
            {
                MessageBox.Show("????");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string A = "ABC DEF GHI JKL";
            f1.GB_Status_AppendText_Nextline("Main String : " + A, Color.Blue);

            string[] B = A.Split(new char[] { ' ' });
            foreach (string temp in B)
            {
                f1.GB_Status_AppendText_Nextline("Sub String : " + temp, Color.Blue);
            }

            string C = "ABC,DEF,GHI,JKL";
            f1.GB_Status_AppendText_Nextline("Main String : " + C, Color.Green);

            string[] D = C.Split(new char[] {','});
            foreach (string temp in D)
            {
                f1.GB_Status_AppendText_Nextline("Sub String : " + temp, Color.Green);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Tallguy tallguy = new Tallguy("Jonghyun Shin",176);   
            using (Stream output = File.Create(Directory.GetCurrentDirectory() + "\\Practice\\Serialized_tallguy.SJH"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, tallguy);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            using (Stream input = File.OpenRead(Directory.GetCurrentDirectory() + "\\Practice\\Serialized_tallguy.SJH"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Tallguy tallguy = (Tallguy)formatter.Deserialize(input);
                f1.GB_Status_AppendText_Nextline(tallguy.Talkaboutyourself(), Color.Green);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (FileStream output = File.Create(Directory.GetCurrentDirectory() + "\\Practice\\Binary_Write_Read.dat"))
            {
                using (BinaryWriter writer = new BinaryWriter(output))
                {
                    writer.Write(123);
                    writer.Write("ABC");
                    writer.Write(new byte[] { 47, 127, 0, 116 });
                    writer.Write(456.789);
                    writer.Write(new char[] { '1', '2', '3', '4' });
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            byte[] datawritten = File.ReadAllBytes(Directory.GetCurrentDirectory() + "\\Practice\\Binary_Write_Read.dat");
            foreach (byte element in datawritten)
            {
                f1.GB_Status_AppendText_Nextline(element.ToString("X2"), Color.Green);
            }

            using (FileStream input = File.OpenRead(Directory.GetCurrentDirectory() + "\\Practice\\Binary_Write_Read.dat"))
            {
                using (BinaryReader reader = new BinaryReader(input))
                {
                    int A = reader.ReadInt32();
                    string B = reader.ReadString();
                    byte[] C = reader.ReadBytes(4);
                    double D = reader.ReadDouble();
                    char[] E = reader.ReadChars(4);

                    f1.GB_Status_AppendText_Nextline("A : " + A.ToString(), Color.Blue);
                    f1.GB_Status_AppendText_Nextline("B : " + B, Color.Blue);
                    f1.GB_Status_AppendText_Nextline("C : " + C[0] + " " + C[1] + " " + C[2] + " " + C[3], Color.Blue);
                    f1.GB_Status_AppendText_Nextline("D : " + D, Color.Blue);
                    f1.GB_Status_AppendText_Nextline("E : " + E[0] + " " + E[1] + " " + E[2] + " " + E[3], Color.Blue);
                }
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int A = 255;
            string B = String.Format("Dec string : {0} / Hex string : {1}", A.ToString(), String.Format("{0:x2}", A));
            f1.GB_Status_AppendText_Nextline(B, Color.Blue);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string Hex_A = "FF";
            string Hex_B = "100";
            int A = Hex_A.Hex_string_To_Dec_int();
            int B = Hex_B.Dec_string_To_Dec_int();

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("A:" + A.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("B:" + B.ToString(), Color.Blue);
            f1.GB_Status_AppendText_Nextline("(A+B):" + (A + B).ToString(), Color.Blue);

            f1.GB_Status_AppendText_Nextline("B_Dec:" + B.Dec_int_To_Dec_String(), Color.Red);
            f1.GB_Status_AppendText_Nextline("B_Hex:" + B.Dec_int_To_Hex_String(), Color.Red);
            
            
        }

        class Comic
        {
            public int Issue;
            public double Price;
        }

        enum PriceRange { Cheap, Midrange, Expensive };

        PriceRange EvaulatePrice(double Price)
        {
            if (Price < 1000) return PriceRange.Cheap;
            else if (Price < 5000) return PriceRange.Midrange;
            else return PriceRange.Expensive;
        }


        IEnumerable<Comic> BuildCatalog()
        {
            return new List<Comic> 
            {
                new Comic{Issue = 1,Price = 5162.51},
                new Comic{Issue = 8,Price = 82462.51},
                new Comic{Issue = 9,Price = 51145.51},
                new Comic{Issue = 7,Price = 52342.51},
                new Comic{Issue = 10,Price = 51542.51},
                new Comic{Issue = 6,Price = 512132.51},
                new Comic{Issue = 11,Price = 12162.51},
                new Comic{Issue = 5,Price = 52362.51},
                new Comic{Issue = 12,Price = 45162.51},
                new Comic{Issue = 4,Price = 6762.51},
                new Comic{Issue = 3,Price = 1262.51},
                new Comic{Issue = 2,Price = 5262.51},
                new Comic{Issue = 15,Price = 362.51},
                new Comic{Issue = 14,Price = 4162.51},
                new Comic{Issue = 13,Price = 1162.51},
            };
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            int[] values = new int[] { 0, 12, 44, 92, 36, 54, 8, 13 ,7,59,157,523,456,17};

            var result = from v in values 
                         where v < 100 
                         orderby v 
                         select v;

            IEnumerable<int> result_2 = from v in values
                         where v < 50
                         orderby v
                         select v;
            
            int count = 0;
            foreach (int i in result) f1.GB_Status_AppendText_Nextline((count++).ToString() + ") " + i.ToString(), Color.Blue);
            count = 0;
            foreach (int i in result_2) f1.GB_Status_AppendText_Nextline((count++).ToString() + ") " + i.ToString(), Color.Red);

            IEnumerable<Comic> catalogs = BuildCatalog();

            var result_3 = from catalog in catalogs
                           where catalog.Issue < 10
                           orderby catalog.Price descending
                           select catalog;

            count = 0;
            foreach (Comic i in result_3) f1.GB_Status_AppendText_Nextline((count++).ToString() + ") (Isuue,Price) : (" +  i.Issue.ToString() + "," + i.Price.ToString() + ")", Color.Green);

            var PriceGroups = from catalog in catalogs
                              group catalog.Price by EvaulatePrice(catalog.Price) into pricegroup    
                              orderby pricegroup.Key descending
                              select pricegroup;

            foreach (var pricegroup in PriceGroups)
            {
                f1.GB_Status_AppendText_Nextline("pricegroup.Key / pricegroup.Count() : " + pricegroup.Key + " / " + pricegroup.Count(), Color.Blue);

                int num = 0;
                foreach (var price in pricegroup)
                {
                    f1.GB_Status_AppendText_Nextline(price.ToString(), Color.Blue);
                }
            }




        }

        private void Event_1(object sender,EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Add Event 1", Color.Red);
        }

        private void Event_2(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Add Event 2", Color.Blue);
        }

        private void Clear(object sender, EventArgs e)
        {
            //Do nothing
        }

        private void button13_Click(object sender, EventArgs e)
        {
            button15.Click += new EventHandler(Event_1);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button15.Click += new EventHandler(Event_2);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("button15 is clicked", Color.Green);
        }


        delegate string Int_To_String(int A);
        private string SJH_A(int A)
        {
            return A.ToString() + " Oh";
        }
        private string SJH_B(int A)
        {
            return A.ToString() + " Yes!";
        }
        private string SJH_C(int A)
        {
            return A.ToString() + " Haha";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Int_To_String A = new Int_To_String(SJH_A);
            Int_To_String B= new Int_To_String(SJH_B);
            Int_To_String C = new Int_To_String(SJH_C);

            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline(A(1), Color.Red);
            f1.GB_Status_AppendText_Nextline(B(3), Color.Green);
            f1.GB_Status_AppendText_Nextline(C(7), Color.Blue);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
            Duck mallard_duck = new MallardDuck();
            f1.GB_Status_AppendText_Nextline(mallard_duck.perform_fly(), Color.Red);
            f1.GB_Status_AppendText_Nextline(mallard_duck.perform_quack(), Color.Red);
            f1.GB_Status_AppendText_Nextline(mallard_duck.swim(), Color.Red);
            f1.GB_Status_AppendText_Nextline(mallard_duck.display(), Color.Red);

            mallard_duck.setFlyBehavior(new FlyWithWings_Fast());
            f1.GB_Status_AppendText_Nextline(mallard_duck.perform_fly(), Color.Red);
            
            Duck toy_duck = new ToyDuck();
            f1.GB_Status_AppendText_Nextline(toy_duck.perform_fly(), Color.Blue);
            f1.GB_Status_AppendText_Nextline(toy_duck.perform_quack(), Color.Blue);
            f1.GB_Status_AppendText_Nextline(toy_duck.swim(), Color.Blue);
            f1.GB_Status_AppendText_Nextline(toy_duck.display(), Color.Blue);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            WeatherData weatherData = new WeatherData();

            CurrentConditionDisplay currentDisplay = new CurrentConditionDisplay(weatherData);//++
            Statistics statistics = new Statistics(weatherData);//++

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 15.4f);

            weatherData.removeObserver(currentDisplay);//--

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 15.4f);

            weatherData.removeObserver(statistics);//--

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 15.4f);

            weatherData.registerObserver(currentDisplay);//++

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 15.4f);
        }

     
        private void button19_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            Beverage beverage1 = new Espresso(Beverage.Beverage_Size.TALL);
            f1.GB_Status_AppendText_Nextline(beverage1.getDescription() + " $" + beverage1.cost(), Color.Blue);

            beverage1 = null;
            beverage1 = new Espresso(Beverage.Beverage_Size.GRANDE);
            f1.GB_Status_AppendText_Nextline(beverage1.getDescription() + " $" + beverage1.cost(), Color.Blue);

            beverage1 = null;
            beverage1 = new Espresso(Beverage.Beverage_Size.VENTI);
            f1.GB_Status_AppendText_Nextline(beverage1.getDescription() + " $" + beverage1.cost(), Color.Blue);


            Beverage beverage2 = new HouseBlend(Beverage.Beverage_Size.TALL); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Mocha(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Mocha(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Whip(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);

            beverage2 = null;
            beverage2 = new HouseBlend(Beverage.Beverage_Size.GRANDE); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Mocha(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Mocha(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Whip(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);

            beverage2 = null;
            beverage2 = new HouseBlend(Beverage.Beverage_Size.VENTI); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Mocha(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Soy(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Whip(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);
            beverage2 = new Whip(beverage2, beverage2.beverage_size); f1.GB_Status_AppendText_Nextline(beverage2.getDescription() + " $" + beverage2.cost(), Color.Red);

            Beverage beverage3 = new DarkRoast(Beverage.Beverage_Size.TALL); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Mocha(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Mocha(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Whip(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);

            beverage3 = null;
            beverage3 = new DarkRoast(Beverage.Beverage_Size.GRANDE); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Mocha(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Mocha(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Whip(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);

            beverage3 = null;
            beverage3 = new DarkRoast(Beverage.Beverage_Size.VENTI); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Mocha(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Soy(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Whip(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);
            beverage3 = new Whip(beverage3, beverage3.beverage_size); f1.GB_Status_AppendText_Nextline(beverage3.getDescription() + " $" + beverage3.cost(), Color.Green);

            f1.GB_Status_AppendText_Nextline("-----------------------------------------------------", Color.Black);

            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\Practice\\SJH.txt");
            int count = 0;
            while (!reader.EndOfStream)
            {
                string Read_Line = reader.ReadLine();
                f1.GB_Status_AppendText_Nextline((count++).ToString() + ")Read_Line : " + Read_Line, Color.Blue);
            }

            reader = new LowerCase_StreamReader(Directory.GetCurrentDirectory() + "\\Practice\\SJH.txt");
            count = 0;
            while (!reader.EndOfStream)
            {
                string Read_Line = reader.ReadLine();
                f1.GB_Status_AppendText_Nextline((count++).ToString() + ")ReadLine_As_Lowercases : " + Read_Line, Color.Red);

            }
        }

     

        private void button20_Click(object sender, EventArgs e)
        {
             Form1 f1 = (Form1)Application.OpenForms["Form1"];
            
             //------------------Simple Factory-----------------
             f1.GB_Status_AppendText_Nextline("------------------Simple Factory(encapsulating object creation by composition)-----------------", Color.Black); 
             SimplePizzaFactory spPizzaFactory = new SimplePizzaFactory();
             Simple_PizzaStore pzstore = new Simple_PizzaStore(spPizzaFactory);
             Pizza pizza = pzstore.orderPizza("cheese");
             f1.GB_Status_AppendText_Nextline("mr.A ordered a " + pizza.name, Color.Red);

             pizza = pzstore.orderPizza("peperoni");
             f1.GB_Status_AppendText_Nextline("mr.B ordered a " + pizza.name, Color.Red);

             pizza = pzstore.orderPizza("veggie");
             f1.GB_Status_AppendText_Nextline("mr.C ordered a " + pizza.name, Color.Red);

            //------------------Factory Method-----------------
             f1.GB_Status_AppendText_Nextline("------------------Factory Method(encapsulating object creation by inheritance)-----------------", Color.Black);
             PizzaStore korean_pizza_store = new Korean_PizzaStore();
             pizza = korean_pizza_store.orderPizza("cheese");
             f1.GB_Status_AppendText_Nextline("mr.A ordered a " + pizza.name, Color.Red);
             pizza = korean_pizza_store.orderPizza("peperoni");
             f1.GB_Status_AppendText_Nextline("mr.B ordered a " + pizza.name, Color.Red);
             
             PizzaStore NY_pizza_store = new NY_PizzaStore();
             pizza = NY_pizza_store.orderPizza("cheese");
             f1.GB_Status_AppendText_Nextline("mr.A ordered a " + pizza.name, Color.Red);
             pizza = NY_pizza_store.orderPizza("peperoni");
             f1.GB_Status_AppendText_Nextline("mr.B ordered a " + pizza.name, Color.Red);


             //------------------Abstract Factory-----------------
             Pizza2 pizza2 = null;

             f1.GB_Status_AppendText_Nextline("------------------Abstract Factory(encapsulating object creation by composition)-----------------", Color.Black); 
             PizzaStore2 NY_pizza_store2 = new NY_PizzaStore2();
             pizza2 = NY_pizza_store2.orderPizza("cheese");
             f1.GB_Status_AppendText_Nextline("mr.A ordered a " + pizza2.name, Color.Red);
             pizza2 = NY_pizza_store2.orderPizza("peperoni");
             f1.GB_Status_AppendText_Nextline("mr.B ordered a " + pizza2.name, Color.Red);

             PizzaStore2 Korean_pizza_store2 = new Korean_PizzaStore2();
             pizza2 = Korean_pizza_store2.orderPizza("cheese");
             f1.GB_Status_AppendText_Nextline("mr.A ordered a " + pizza2.name, Color.Red);
             pizza2 = Korean_pizza_store2.orderPizza("peperoni");
             f1.GB_Status_AppendText_Nextline("mr.B ordered a " + pizza2.name, Color.Red);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            List<Menu> menus = new List<Menu>();
            menus.Add(new PancakeHouseMenu());
            menus.Add(new DinerMenu());
            menus.Add(new CafeMenu());

            Waitress waitress = new Waitress(menus);
            waitress.printMenu();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            DuckSimulator duck_simulator = new DuckSimulator();
            duck_simulator.simulate();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("--------------------------------", Color.Black);

            //MinHeap mh = new MinHeap(15);
            MinHeap<int> mh = new MinHeap<int>(15);

            mh.Insert(77);
            mh.Insert(7);
            mh.Insert(64);
            mh.Insert(21);
            mh.Insert(89);
            mh.Insert(1);
            mh.Insert(92);
            mh.Insert(17);
            mh.Insert(3);
            mh.Insert(30);
            mh.Insert(42);
            mh.Insert(50);
            mh.Insert(2);
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMin() : " + mh.getMin(), Color.Red);
            mh.RemoveMin();
            mh.displayHeap();



            f1.GB_Status_AppendText_Nextline("--------------------------------", Color.Black);

            int[] arr = { 77, 7, 64, 21, 89,1, 92, 17, 3,30, 42, 50, 2 };
            mh.BuildMinHeap(arr);
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMin() : " + mh.getMin(), Color.Green);
            mh.RemoveMin();
            mh.displayHeap();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("--------------------------------", Color.Black);

            //MinHeap mh = new MinHeap(15);
            MaxHeap<int> mh = new MaxHeap<int>(15);

            mh.Insert(77);
            mh.Insert(7);
            mh.Insert(64);
            mh.Insert(21);
            mh.Insert(89);
            mh.Insert(1);
            mh.Insert(92);
            mh.Insert(17);
            mh.Insert(3);
            mh.Insert(30);
            mh.Insert(42);
            mh.Insert(50);
            mh.Insert(2);
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("1) mh.getMax() : " + mh.getMax(), Color.Red);
            mh.RemoveMax();
            mh.displayHeap();




            f1.GB_Status_AppendText_Nextline("--------------------------------", Color.Black);

            int[] arr = { 77, 7, 64, 21, 89, 1, 92, 17, 3, 30, 42, 50, 2 };
            mh.BuildMaxHeap(arr);
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();

            f1.GB_Status_AppendText_Nextline("2) mh.getMax() : " + mh.getMax(), Color.Green);
            mh.RemoveMax();
            mh.displayHeap();


        }

        

        

    }
}
