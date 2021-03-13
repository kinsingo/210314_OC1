using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using System.Collections;
using System.Collections.ObjectModel;

namespace PNC_Csharp 
{
    //--------- Design Pattern 1 (Implement(interface) / Has-A(Class) / Is-A(Class)) ---------
    public class FlyWithWings : FlyBehavior
    {
        public string fly()
        {
            return "i'm flying with wings";
        }
    }

    public class FlyWithWings_Fast : FlyBehavior
    {
        public string fly()
        {
            return "i'm flying with wings in a very fast way!";
        }
    }

    public class FlyNoWay : FlyBehavior
    {
        public string fly()
        {
            return "i cannot fly";
        }
    }



    public class Quack : QuackBehavior
    {
        public string quack()
        {
            return "Quack !";
        }
    }

    public class MuteQuack : QuackBehavior
    {
        public string quack()
        {
            return "...(Silence)";
        }
    }

    public abstract class Duck
    {
        public FlyBehavior flybehavior;
        public QuackBehavior quackbehavior;
        public Duck()
        {

        }
        public abstract string display();

        public void setFlyBehavior(FlyBehavior fb)
        {
            flybehavior = fb;
        }

        public string perform_quack()
        {
            return quackbehavior.quack();
        }

        public string perform_fly()
        {
            return flybehavior.fly();
        }

        public string swim()
        {
            return "All ducks can swim"; 
        }
    }

    public class MallardDuck : Duck
    {
        public MallardDuck()
        {
            flybehavior = new FlyWithWings();
            quackbehavior = new Quack();
        }

        public override string display()
        {
            return "I'm MallardDuck";
        }
    }

    public class ToyDuck : Duck
    {
        public ToyDuck()
        {
            flybehavior = new FlyNoWay();
            quackbehavior = new MuteQuack();
        }

        public void setFlyBehavior(FlyBehavior fb)
        {
            flybehavior = fb;
        }

        public override string display()
        {
            return "It's ToyDuck";
        }
    }
    
    //----------------------C# Serialize--------------------
    [Serializable]
    class Tallguy : ICrown
    {
        public string name;
        public int height;
        public Tallguy(string name, int height)
        {
            this.name = name;
            this.height = height;
        }

        public string Talkaboutyourself()
        {
            return "My name is " + name + " and my height is " + height + "cm";
        }

        public string FunnyThingIHave
        {
            get { return "Big shoes"; } 
        }

        public string Honk()
        {
            return "Honk honk!";
        }
    }

    class Scary_Tallguy : Tallguy, IScaryClown
    {
        public Scary_Tallguy(string name, int height) : base(name, height) { }

        public string ScraryThingIHave
        {
            get { return "Ghost!!"; }
        }

        public string Honk()
        {
            return "Honkkkkkkkkkkk!!!";
        }
    }

    class Scarayguy : IScaryClown
    {
        public string ScraryThingIHave
        {
            get { return "Ghost!!"; }
        }

        public string FunnyThingIHave
        {
            get { return "Funny rabbit !"; }
        }

        public string Honk()
        {
            return "Honk Honk Baby~";
        }
    }

    //--------- Design Pattern 2(Observer Pattern) ---------
    public class WeatherData : Subject
    {
        private List<Observer> observers;
        private float temperature;
        private float humidity;
        private float pressure;

        public WeatherData()
        {
            observers = new List<Observer>();
        }

        public void registerObserver(Observer o)
        {
            observers.Add(o);
        }

        public void removeObserver(Observer o)
        {
            if (observers.IndexOf(o) >= 0) observers.Remove(o);
        }

        public void notifyObservers()
        {
            foreach (Observer o in observers)
            {
                o.update(this.temperature, this.humidity, this.pressure);
            }
        }

        public void MeasurementChanged()
        {
            this.notifyObservers();
        }

        public void SetMeasurements(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.MeasurementChanged();
        }
    }

    public class CurrentConditionDisplay : Observer, DisplayElement
    {
        private float temperature;
        private float humidity;
        private Subject weatherData;

        public CurrentConditionDisplay(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temp, float humidity, float pressure)
        {
            //In this class, we don't use the parameter "pressure"
            this.temperature = temp;
            this.humidity = humidity;
            display();
        }

        public void display()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Current Condition : " + temperature.ToString() + "F degrees and " + humidity.ToString() + "% humidity", Color.Blue);
        }
    }

    public class Statistics : Observer, DisplayElement
    {
        private float temperature;
        private float humidity;
        private float pressure;
        private Subject weatherData;

        public Statistics(Subject weatherData)
        {
            this.weatherData = weatherData;
            weatherData.registerObserver(this);
        }

        public void update(float temp, float humidity, float pressure)
        {
            this.temperature = temp;
            this.humidity = humidity;
            this.pressure = pressure;
            display();
        }

        public void display()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Statistics : " + temperature.ToString() + "F degrees and " + humidity.ToString() + "% humidity " + pressure.ToString() + " pressure", Color.Red);
        }
    }

    //--------- Design Pattern 3(Decorator Pattern) ---------
    
    public abstract class Beverage
    {
        public string description = "Unknown Beverage";
        public virtual string getDescription() { return description; }
        public abstract double cost();

        public Beverage(Beverage_Size beverage_size) 
        {
            this.beverage_size = beverage_size;
            description = "Unknown Beverage"; 
        }
        //Size Related
        public enum Beverage_Size { TALL, GRANDE, VENTI };
        public Beverage_Size beverage_size;
    }

    //Beverages
    public class Espresso : Beverage
    {
        public Espresso(Beverage_Size beverage_size)
            : base(beverage_size)
        {
            description = "Espresso(" + beverage_size.ToString() + ")";
        }

        public override double cost()
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return 1.99;
                case Beverage_Size.GRANDE:
                    return 2.99;
                case Beverage_Size.VENTI:
                    return 3.99;
                default:
                    return 0;
            }
        }
    }

    public class HouseBlend : Beverage
    {
        public HouseBlend(Beverage_Size beverage_size)
            : base(beverage_size)
        {
            description = "House Blend Coffee(" + beverage_size.ToString() + ")";
        }
        public override double cost()
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return 0.76;
                case Beverage_Size.GRANDE:
                    return 1.76;
                case Beverage_Size.VENTI:
                    return 2.76;
                default:
                    return 0;
            }
        }
    }

    public class DarkRoast : Beverage
    {
        public DarkRoast(Beverage_Size beverage_size)
            : base(beverage_size)
        {
            description = "DarkRoast with(" + beverage_size.ToString() + ")";
        }

        public override double cost()
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return 1.12;
                case Beverage_Size.GRANDE:
                    return 2.12;
                case Beverage_Size.VENTI:
                    return 3.12;
                default:
                    return 0;
            }
        }
    }

    //Condiments
    public class Mocha : Beverage
    {
        Beverage beverage;

        public Mocha(Beverage beverage, Beverage_Size beverage_size)
            : base(beverage_size)
        {
            this.beverage = beverage;
        }
        public override string getDescription() { return (beverage.getDescription() + ", Mocha(" + beverage_size.ToString() + ")"); }
        public override double cost() 
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return beverage.cost() + 0.29;
                case Beverage_Size.GRANDE:
                    return beverage.cost() + 0.49;
                case Beverage_Size.VENTI:
                    return beverage.cost() + 0.69;
                default:
                    return 0;
            }
        }
    }

    public class Soy : Beverage
    {
        Beverage beverage;

        public Soy(Beverage beverage, Beverage_Size beverage_size)
            : base(beverage_size)
        {
            this.beverage = beverage;
        }

        public override string getDescription() { return (beverage.getDescription() + ", Soy(" + beverage_size.ToString() + ")"); }
        public override double cost() 
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return beverage.cost() + 0.45;
                case Beverage_Size.GRANDE:
                    return beverage.cost() + 0.65;
                case Beverage_Size.VENTI:
                    return beverage.cost() + 0.85;
                default:
                    return 0;
            }
        }
    }

    public class Whip : Beverage
    {
        Beverage beverage;

        public Whip(Beverage beverage, Beverage_Size beverage_size)
            : base(beverage_size)
        {
            this.beverage = beverage;
        }

        public override string getDescription() { return (beverage.getDescription() + ", Whip(" + beverage_size.ToString() + ")"); }
        public override double cost()
        {
            switch (beverage_size)
            {
                case Beverage_Size.TALL:
                    return beverage.cost() + 0.15;
                case Beverage_Size.GRANDE:
                    return beverage.cost() + 0.3;
                case Beverage_Size.VENTI:
                    return beverage.cost() + 0.45;
                default:
                    return 0;
            }
        }
    }

    public class LowerCase_StreamReader : System.IO.StreamReader
    {
        public LowerCase_StreamReader(string File_Directory) : base(File_Directory) { }

        public override string ReadLine()
        {
            
            string result = base.ReadLine();
            return result.ToLower();
        }
    }

    //--------- Design Pattern 4-1(encapsulating object creation by composition) ---------
    public abstract class Pizza
    {
        Form1 f1 = (Form1)Application.OpenForms["Form1"];
        public string name;
        public string dough;
        public string sauce;
        public List<string> toppings = new List<string>();

        public void prepare()
        {
            
            f1.GB_Status_AppendText_Nextline("Preparing " + name, Color.Blue);
            f1.GB_Status_AppendText_Nextline("Tossing dough " + dough, Color.Blue);
            f1.GB_Status_AppendText_Nextline("Adding sauce " + sauce, Color.Blue);
            foreach (string topping in toppings) f1.GB_Status_AppendText_Nextline("Topping " + topping, Color.Blue);
        }

        public void bake() { f1.GB_Status_AppendText_Nextline("Bake for 25minutes at 350", Color.Blue); }
        public void cut() { f1.GB_Status_AppendText_Nextline("Cutting the pizza into diagonal slices", Color.Blue); }
        public void box() { f1.GB_Status_AppendText_Nextline("place the pizza in the pizza box", Color.Blue); }
    }

    public class CheesePizza : Pizza
    {
        public CheesePizza()
        {
            name = "cheese pizza";
            dough = "thin dough";
            sauce = "BBQ sauce";
            toppings.Add("cheeses");
        }
    }

    public class  PepperoiPizza : Pizza
    {
        public PepperoiPizza()
        {
            name = "pepperoni pizza";
            dough = "crispy dough";
            sauce = "hot sauce";
            toppings.Add("pepperonies");
            toppings.Add("hams");
        }
    }

    public class VeggiePizza : Pizza
    {
        public VeggiePizza()
        {
            name = "veggie pizza";
            dough = "thick dough";
            sauce = "crab sauce";
            toppings.Add("mushrooms");
            toppings.Add("pineapples");
            toppings.Add("peppers");
            toppings.Add("blueberries");
        }
    }
    public class SimplePizzaFactory
    {
        public Pizza createPizza(string type)
        {
            Pizza pizza = null;

            if (type.Equals("cheese")) pizza = new CheesePizza();
            else if (type.Equals("peperoni")) pizza = new PepperoiPizza();
            else if (type.Equals("veggie")) pizza = new VeggiePizza();

            return pizza;
        }
    }

    public class Simple_PizzaStore
    {
        SimplePizzaFactory factory;
        public Simple_PizzaStore(SimplePizzaFactory factory) { this.factory = factory; }
        public Pizza orderPizza(string type)
        {
            Pizza pizza = factory.createPizza(type);

            pizza.prepare();
            pizza.bake();
            pizza.cut();
            pizza.box();

            return pizza;
        }
    }

    //--------- Design Pattern 4-2(Pizza Factory_Method (encapsulating object creation by inheritance)) ---------
    public class Korean_CheesePizza : Pizza
    {
        public Korean_CheesePizza()
        {
            name = "korean cheese pizza";
            dough = "thin dough";
            sauce = "BBQ sauce";
            toppings.Add("cheeses");
        }
    }

    public class Korean_PepperoiPizza : Pizza
    {
        public Korean_PepperoiPizza()
        {
            name = "korean pepperoni pizza";
            dough = "crispy dough";
            sauce = "hot sauce";
            toppings.Add("Korean pepperonies");
            toppings.Add("Korean hams");
        }
    }

    public class NY_CheesePizza : Pizza
    {
        public NY_CheesePizza()
        {
            name = "NY cheese pizza";
            dough = "thin crispy dough";
            sauce = "sweet veggie sauce";
            toppings.Add("cheda cheeses");
            toppings.Add("NY hams");
        }
    }

    public class NY_PepperoiPizza : Pizza
    {
        public NY_PepperoiPizza()
        {
            name = "NY pepperoni pizza";
            dough = "very thick dough";
            sauce = "hot oyster sauce";
            toppings.Add("NY pepperonies");
            toppings.Add("NY meets");
        }
    }

   
    
    
    public abstract class PizzaStore
    {
        public Pizza orderPizza(string type)
        {
            Pizza pizza = createPizza(type);

            pizza.prepare();
            pizza.bake();
            pizza.cut();
            pizza.box();

            return pizza;
        }

        protected abstract Pizza createPizza(string type);
    }

    public class Korean_PizzaStore : PizzaStore
    {
        protected override Pizza createPizza(string type)
        {
            Pizza pizza = null;

            if (type.Equals("cheese")) pizza = new Korean_CheesePizza();
            else if (type.Equals("peperoni")) pizza = new Korean_PepperoiPizza();
            
            return pizza;
        }
    }

    public class NY_PizzaStore : PizzaStore
    {
        protected override Pizza createPizza(string type)
        {
            Pizza pizza = null;

            if (type.Equals("cheese")) pizza = new NY_CheesePizza();
            else if (type.Equals("peperoni")) pizza = new NY_PepperoiPizza();

            return pizza;
        }
    }

    //--------- Design Pattern 4-2(Pizza Abstract Factory (encapsulating object creation by inheritance)) ---------

    public class Korean_PizzaIngredientFactory : PizzaIngredientFactory
    {
        public string createDough()
        {
            return "Korean very thin Dough";
        }

        public string createSauce()
        {
            return "Korean BBQ Sauce";
        }

        public string createCheese()
        {
            return "Korean Cheese";
        }

        public string createPepperoni()
        {
            return "Korean Pepperoni";
        }
    }

    public class NY_PizzaIngredientFactory : PizzaIngredientFactory
    {
        public string createDough()
        {
            return "NY Crispy Dough";
        }

        public string createSauce()
        {
            return "NY Hot Sauce";
        }

        public string createCheese()
        {
            return "NY Tasty Cheese";
        }

        public string createPepperoni()
        {
            return "NY Jucy Pepperoni";
        }
    }

    public abstract class Pizza2
    {
        Form1 f1 = (Form1)Application.OpenForms["Form1"];
        public string name;
        public string dough;
        public string sauce;
        public string pepperoni;
        public string cheese;

        public abstract void prepare();

        public void bake() { f1.GB_Status_AppendText_Nextline("Bake for 25minutes at 350", Color.Blue); }
        public void cut() { f1.GB_Status_AppendText_Nextline("Cutting the pizza into diagonal slices", Color.Blue); }
        public void box() { f1.GB_Status_AppendText_Nextline("place the pizza in the pizza box", Color.Blue); }
    }

    public class CheesePizza2 : Pizza2
    {
        PizzaIngredientFactory ingredientfactory;
        public CheesePizza2(PizzaIngredientFactory ingredientfactory) { this.ingredientfactory = ingredientfactory; }
        public override void prepare()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            dough = ingredientfactory.createDough();
            sauce = ingredientfactory.createSauce();
            cheese = ingredientfactory.createCheese();

            f1.GB_Status_AppendText_Nextline(dough, Color.Blue);
            f1.GB_Status_AppendText_Nextline(sauce, Color.Blue);
            f1.GB_Status_AppendText_Nextline(cheese, Color.Blue); 
        }
    }

    public class PeperoniPizza2 : Pizza2
    {
        PizzaIngredientFactory ingredientfactory;
        public PeperoniPizza2(PizzaIngredientFactory ingredientfactory) { this.ingredientfactory = ingredientfactory; }
        public override void prepare()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            dough = ingredientfactory.createDough();
            sauce = ingredientfactory.createSauce();
            pepperoni = ingredientfactory.createPepperoni();

            f1.GB_Status_AppendText_Nextline(dough, Color.Blue);
            f1.GB_Status_AppendText_Nextline(sauce, Color.Blue);
            f1.GB_Status_AppendText_Nextline(pepperoni, Color.Blue); 
            
        }
    }

    public abstract class PizzaStore2
    {
        public Pizza2 orderPizza(string type)
        {
            Pizza2 pizza = createPizza(type);

            pizza.prepare();
            pizza.bake();
            pizza.cut();
            pizza.box();

            return pizza;
        }

        protected abstract Pizza2 createPizza(string type);
    }


    public class NY_PizzaStore2 : PizzaStore2
    {
        protected override Pizza2 createPizza(string type)
        {
            Pizza2 pizza = null;
            PizzaIngredientFactory ingredientfactory = new NY_PizzaIngredientFactory();

            if (type.Equals("cheese"))
            {
                pizza = new CheesePizza2(ingredientfactory);
                pizza.name = "NY CheesePizza";
            }
            else if (type.Equals("peperoni"))
            {
                pizza = new PeperoniPizza2(ingredientfactory);
                pizza.name = "NY peperoni Pizza";
            }

            return pizza;
        }
    }

    public class Korean_PizzaStore2 : PizzaStore2
    {
        protected override Pizza2 createPizza(string type)
        {
            Pizza2 pizza = null;
            PizzaIngredientFactory ingredientfactory = new Korean_PizzaIngredientFactory();

            if (type.Equals("cheese"))
            {
                pizza = new CheesePizza2(ingredientfactory);
                pizza.name = "Korean CheesePizza";
            }
            else if (type.Equals("peperoni"))
            {
                pizza = new PeperoniPizza2(ingredientfactory);
                pizza.name = "Korean peperoni Pizza";
            }
            return pizza;
        }
    }


    //--------- Design Pattern 9(Well Managed Collections) ---------
    public class MenuItem
    {
        string name;
        string description;
        bool vegetarian;
        double price;

        public MenuItem(string name, string description, bool vegetarian, double price)
        {
            this.name = name;
            this.description = description;
            this.vegetarian = vegetarian;
            this.price = price;
        }

        public string getName() { return name; }
        public string getDescription() { return description; }
        public double getPrice() { return price; }
        public bool isVegetarian() { return vegetarian; }
    }


    public interface Menu
    {
        string getMenuname();
        List<MenuItem> getMenuItems();
    }

    public class PancakeHouseMenu : Menu
    {
        List<MenuItem> menuItems;

        public PancakeHouseMenu()
        {
            menuItems = new List<MenuItem>();
            addItem("A Pancake", "Pancake with A Sauce", true, 2.29);
            addItem("B Pancake", "Pancake with B Sauce and without meets", false, 2.49);
            addItem("C Pancake", "Pancake with C Sauce", true, 2.69);
            addItem("D Pancake", "Pancake with D Sauce and without meets", false, 2.89);
        }

        public void addItem(string name, string description, bool vegetarian, double price)
        {
            menuItems.Add(new MenuItem(name, description, vegetarian, price));
        }

        
        public List<MenuItem> getMenuItems()
        {
            return menuItems;
        }

        public IEnumerator creatIEnumerator()
        {
            return menuItems.GetEnumerator();
        }

        public string getMenuname()
        {
            return "PancakeHouseMenu";
        }
    }

    public class DinerMenu : Menu
    {
        const int MAX_ITEMS = 6;
        int numberOfItems = 0;
        MenuItem[] menuItems;

        public DinerMenu()
        {
            menuItems = new MenuItem[MAX_ITEMS];
            addItem("A Set Menu", "Steak with A Sauce", true, 2.29);
            addItem("B Set Menu", "Sea foods with B Sauce and without meets", false, 2.49);
            addItem("C Set Menu", "Steak with C Sauce", true, 2.69);
            addItem("D Set Menu", "Sea foods with D Sauce and without meets", false, 2.89);
        }

        public void addItem(string name, string description, bool vegetarian, double price)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (numberOfItems >= MAX_ITEMS)
            {
                f1.GB_Status_AppendText_Nextline("Sorry, Menu is full! Can't add item to menu", Color.Blue);
            }
            else
            {
                menuItems[numberOfItems++] = new MenuItem(name, description, vegetarian, price);
            }
        }

        public List<MenuItem> getMenuItems()
        {
            List<MenuItem> List_Menu_Items = new List<MenuItem>();
            for (int i = 0; i < numberOfItems; i++) List_Menu_Items.Add(menuItems[i]);
            return List_Menu_Items;
        }

        public string getMenuname()
        {
            return "DinerMenu";
        }
    }

    public class CafeMenu : Menu
    {
        Dictionary<string, MenuItem> menuItems = new Dictionary<string, MenuItem>();

        public CafeMenu()
        {
            addItem("Cafe Menu A", "Americano", true, 2.29);
            addItem("Cafe Menu B", "Latte", false, 2.49);
            addItem("Cafe Menu C", "Dessert Cake", true, 2.69);
            addItem("Cafe Menu D", "Cheese", false, 2.89);
        }

        public void addItem(string name, string description, bool vegetarian, double price)
        {
            menuItems.Add(name, new MenuItem(name, description, vegetarian, price));
        }

        public List<MenuItem> getMenuItems()
        {
            List<MenuItem> List_Menu_Items = new List<MenuItem>();
            foreach (KeyValuePair<string, MenuItem> menu_item_pair in menuItems) List_Menu_Items.Add(menu_item_pair.Value);
            return List_Menu_Items;
        }

        public string getMenuname()
        {
            return "CafeMenu";
        }
    }


    public class Waitress
    {
        List<Menu> menus;

        //public Waitress(PancakeHouseMenu pancakeHouseMenu, DinerMenu dinerMenu)
        public Waitress(List<Menu> menus)    
        {
            this.menus = menus;
        }

        public void printMenu()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            foreach (Menu menu in menus)
            {
                f1.GB_Status_AppendText_Nextline("-----------" + menu.getMenuname() + "-----------", Color.Black);
                foreach (MenuItem menuItem in menu.getMenuItems())
                   f1.GB_Status_AppendText_Nextline(menuItem.getName() + "," + menuItem.getPrice().ToString() + " -- " + menuItem.getDescription(), Color.Blue);
            }
        }
    }



    //--------- Chapter 12 (Many Patterns with Quackables ---------
    public class A_Duck : Quackable
    {
        string name;
        public string get_name() { return name; }
        QuackObservable observable;
        public A_Duck(string name)
        {
            this.name = name;
            observable = new QuackObservable(this);
        }
        public void registerObserver(IQuackObserver observer)
        {
            observable.registerObserver(observer);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Observer " + observer.ToString() + " was registered by a " + this.get_name(), Color.Black);
        }

        public void notifyObservers()
        {
            observable.notifyObservers();
        }

        public void quack()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("A_Duck quack !", Color.Blue);
            notifyObservers();
        }
    }

    public class B_Duck : Quackable
    {
        string name;
        public string get_name() { return name; }
        QuackObservable observable;
        public B_Duck(string name)
        {
            this.name = name;
            observable = new QuackObservable(this);
        }
        public void registerObserver(IQuackObserver observer)
        {
            observable.registerObserver(observer);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Observer " + observer.ToString() + " was registered by a " + this.get_name(), Color.Black);
        }

        public void notifyObservers()
        {
            observable.notifyObservers();
        }

        public void quack()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("B_Duck quack !", Color.Blue);
            notifyObservers();
        }
    }

    public class C_Duck : Quackable
    {
        string name;
        public string get_name() { return name; }
        QuackObservable observable;
        public C_Duck(string name)
        {
            this.name = name;
            observable = new QuackObservable(this);
        }
        public void registerObserver(IQuackObserver observer)
        {
            observable.registerObserver(observer);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Observer " + observer.ToString() + " was registered by a " + this.get_name(), Color.Black);
        }

        public void notifyObservers()
        {
            observable.notifyObservers();
        }

        public void quack()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("C_Duck quack !", Color.Blue);
            notifyObservers();
        }
    }

    public class D_Duck : Quackable
    {
        string name;
        public string get_name() { return name; }

        QuackObservable observable;
        public D_Duck(string name)
        {
            this.name = name;
            observable = new QuackObservable(this);
        }
        public void registerObserver(IQuackObserver observer)
        {
            observable.registerObserver(observer);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Observer " + observer.ToString() + " was registered by a " + this.get_name(), Color.Black);
        }

        public void notifyObservers()
        {
            observable.notifyObservers();
        }

        public void quack()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("D_Duck quack !", Color.Blue);
            notifyObservers();
        }
    }

    public class Goose
    {
        string name;
        public Goose(string name)
        { this.name = name; }
        public string get_name() { return name; }

        public void honk()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Goose honk !", Color.Blue);
        }
    }

    //Adapter : Goose --> Quackable(Duck)
    public class GooseAdapter : Quackable
    {
        QuackObservable observable;
        Goose goose;
        public GooseAdapter(Goose goose)
        {
            this.goose = goose;
            observable = new QuackObservable(this);
        }
        public void quack()
        {
            goose.honk();
            notifyObservers();
        }

        public void registerObserver(IQuackObserver observer)
        {
            observable.registerObserver(observer);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Observer " + observer.ToString() + " was registered by a " + goose.get_name(), Color.Black);
        }

        public void notifyObservers()
        {
            observable.notifyObservers();
        }

        public string get_name()
        {
            return goose.get_name();
        }
    }

    //Decorator
    public class QuackCounter : Quackable
    {
        Quackable duck;
        static int numberofQuack;

        public QuackCounter(Quackable duck)
        {
            this.duck = duck;
        }

        public void quack()
        {
            duck.quack();
            numberofQuack++;
        }
        public static int get_numberofQuack() { return numberofQuack; }
        public void reset_numberofQuack() { numberofQuack = 0; }

        public void registerObserver(IQuackObserver observer)
        {
            duck.registerObserver(observer);
        }

        public void notifyObservers()
        {
            duck.notifyObservers();
        }

        public string get_name()
        {
            return duck.get_name();
        }
    }

    //Factory
    public class DuckFactory : IQuackable_Factory
    {
        public Quackable Create_A_Duck(string name)
        {
            return new QuackCounter(new A_Duck(name));
        }

        public Quackable Create_B_Duck(string name)
        {
            return new QuackCounter(new B_Duck(name));
        }

        public Quackable Create_C_Duck(string name)
        {
            return new QuackCounter(new C_Duck(name));
        }

        public Quackable Create_D_Duck(string name)
        {
            return new QuackCounter(new D_Duck(name));
        }

        public Quackable Create_Goose(string name)
        {
            return new QuackCounter(new GooseAdapter(new Goose(name)));
        }
    }

    public class Quackable_Flock : Quackable
    {
        List<Quackable> ducks = new List<Quackable>();
        public void Add(Quackable duck)
        {
            ducks.Add(duck);
        }
        public void Add(List<Quackable> ducks)
        {
            foreach (Quackable duck in ducks) this.ducks.Add(duck);
        }
        
        public void quack()
        {
            foreach (Quackable duck in ducks)
            {
                duck.quack();
            }
        }

        public void registerObserver(IQuackObserver observer)
        {
            foreach (Quackable duck in ducks) duck.registerObserver(observer);
        }

        public void notifyObservers()
        {
            foreach (Quackable duck in ducks) duck.notifyObservers();
        }

        public string get_name()
        {
            throw new NotImplementedException();
        }
    }

    public class QuackObservable : IQuackObservable
    {
        List<IQuackObserver> observers = new List<IQuackObserver>();
        IQuackObservable duck;

        public QuackObservable(IQuackObservable duck)
        {
            this.duck = duck;
        }

        public void registerObserver(IQuackObserver observer)
        {
            observers.Add(observer);
        }

        public void notifyObservers()
        {
            foreach (IQuackObserver observer in observers) observer.update(duck);
        }

        public string get_name()
        {
            return duck.get_name();
        }
    }

    public class Quackologist : IQuackObserver
    {
        public void update(IQuackObservable duck)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("Quackologist was notified that the " + duck.get_name() + " just quacked", Color.Black);
        }
    }

    public class DuckSimulator
    {
        public void simulate()
        {
            Quackable_Flock ducks = new Quackable_Flock();
            DuckFactory duckfactory = new DuckFactory();
            ducks.Add(duckfactory.Create_A_Duck("A1"));
            ducks.Add(duckfactory.Create_B_Duck("B1"));
            ducks.Add(duckfactory.Create_C_Duck("C1"));
            ducks.Add(duckfactory.Create_D_Duck("D1"));
            ducks.Add(duckfactory.Create_Goose("Goose1"));
            show_numberofQuack();
            
            ducks.quack();
            show_numberofQuack();

            List<Quackable> D_ducks = new List<Quackable>();
            for (int i = 0; i < 6; i++) D_ducks.Add(duckfactory.Create_D_Duck("Duck D" + i.ToString()));
            ducks.Add(D_ducks);
            ducks.quack();
            show_numberofQuack();

            IQuackObserver duck_expert = new Quackologist();
            ducks.registerObserver(duck_expert);
            ducks.quack();
            show_numberofQuack();
        }

        private void show_numberofQuack()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.GB_Status_AppendText_Nextline("numberofQuack : " + QuackCounter.get_numberofQuack().ToString(), Color.Black);
        }
    }

   





}
