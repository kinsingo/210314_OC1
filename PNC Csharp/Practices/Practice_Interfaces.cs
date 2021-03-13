using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNC_Csharp
{
    //-----------C#-------------
    interface ICrown
    {
        string FunnyThingIHave { get; }
        string Honk();
    }

    interface IScaryClown : ICrown
    {
        string ScraryThingIHave { get; }
    }

    //-----------Design Pattern 1-------------
    public interface FlyBehavior
    {
        string fly();
    }

    public interface QuackBehavior
    {
        string quack();
    }

    //-----------Design Pattern 2(Observer Pattern)-------------
    public interface Subject
    {
        void registerObserver(Observer o);
        void removeObserver(Observer o);
        void notifyObservers();
    }

    public interface Observer
    {
        void update(float temp, float humidity, float pressure);
    }

    public interface DisplayElement
    {
        void display();
    }

    //--------- Design Pattern 4-2(Pizza Abstract Factory (encapsulating object creation by inheritance)) ---------
    public interface PizzaIngredientFactory
    {
        string createDough();
        string createSauce();
        string createCheese();
        string createPepperoni();
    }


    //--------- Chapter 12 (Many Patterns with Quackables ---------
    public interface Quackable : IQuackObservable
    {
        void quack();
    }

    //Objects' Factory
    public interface IQuackable_Factory
    {
        Quackable Create_A_Duck(string name);
        Quackable Create_B_Duck(string name);
        Quackable Create_C_Duck(string name);
        Quackable Create_D_Duck(string name);
    }

    public interface IQuackObserver
    {
        void update(IQuackObservable duck);
    }

    public interface IQuackObservable
    {
        string get_name();
        void registerObserver(IQuackObserver observer);
        void notifyObservers();
    }

}
