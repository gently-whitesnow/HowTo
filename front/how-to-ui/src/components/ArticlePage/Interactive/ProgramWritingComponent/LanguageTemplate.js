import { LanguageType } from "../../../../entities/LanguageType.js";

export const LanguageTemplate = {
  [LanguageType.Cshapr]: `using System;
namespace Program
{
    class Program {         
        static void Main(string[] args)
        {
            WriteLine("Hello World!");
        }
    }
}`,
  [LanguageType.Cpp]: `#include <iostream>

int main() {
    std::cout << "Hello World!";
    return 0;
}`,
  [LanguageType.Python]: `print("hello world!")`,
};
