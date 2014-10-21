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
        public virtual void Show(IRecipe recipe)
        {
            RenderHeader(recipe.Name);
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
            foreach (string direction in recipe.Instructions)
            {
                Console.WriteLine(direction);
            }
            Console.WriteLine();
        }
        public virtual void Show(IEnumerable<IRecipe> recipes)
        {
            foreach (Recipe recipe in recipes)
            {
                if (recipe != null)
                {
                    Show(recipe);
                }
            }
        }
        public void RenderHeader(string name)
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