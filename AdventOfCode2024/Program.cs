﻿// See https://aka.ms/new-console-template for more information
using AdventOfCode2024;

Console.WriteLine("Hello, World!");
var days = new IAdventDay[]
{
    new AdventDay()
};

var date = DateTime.Today;
var day = date.Day - 1;
var currentAdventDay = days[day];
var result1 = await currentAdventDay.ExecuteTask1();
Console.WriteLine(result1);
var result2 = await currentAdventDay.ExecuteTask2();
Console.WriteLine(result2);
Console.ReadLine();
