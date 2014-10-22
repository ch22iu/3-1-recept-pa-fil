using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {
        //Tar emot ett receptobjekt och skriver ut det.
        public virtual void Show(IRecipe recipe)
        {
            ShowHeader(recipe.Name);
            Console.WriteLine();
            Console.WriteLine("Ingredienser");
            Console.WriteLine("============");
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                Console.WriteLine(ingredient.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Instruktioner");
            Console.WriteLine("=============");
            foreach (string instruction in recipe.Instructions)
            {
                Console.WriteLine(instruction);
            }
            Console.WriteLine();
        }
        public virtual void Show(IEnumerable<IRecipe> recipes)
        {
            foreach (IRecipe recipe in recipes)
            {
                if (recipe != null)
                {
                    Show(recipe);
                }
            }
        }
        // Skriver ut själva objektets rubrik.
        public void ShowHeader(string name)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(name);
            Console.WriteLine("-------------------------------------");
            Console.ResetColor();
        }
    }
}