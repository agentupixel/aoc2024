// See https://aka.ms/new-console-template for more information
using AdventOfCode2024;

Console.WriteLine("Hello, World!");
var days = new IAdventDay[]
{
    new AdventDay1(),
    new AdventDay2(),
    new AdventDay3(),
    new AdventDay4(),
    new AdventDay5(),
    new AdventDay6(),
    new AdventDay7(),
    new AdventDay8(),
    new AdventDay9(),
    new AdventDay10(),
    new AdventDay11(),
    new AdventDay12(),
    new AdventDay13(),
    new AdventDay14(),
    new AdventDay15(),
};

var date = DateTime.Today;
var day = date.Day - 1;
var currentAdventDay = days[day];
var result1 = await currentAdventDay.ExecuteTask1();
Console.WriteLine(result1);
var result2 = await currentAdventDay.ExecuteTask2();
Console.WriteLine(result2);
Console.ReadLine();
