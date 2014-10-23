using FiledRecipes.Domain;
using FiledRecipes.App.Mvp;
using FiledRecipes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace FiledRecipes.Views
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeView : ViewBase, IRecipeView
    {
        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "Ingredienser";
        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "Instruktioner";
        //Tar emot ett receptobjekt och skriver ut det.
        public virtual void Show(IRecipe recipe)
        {
            ShowHeader(recipe.Name);
            Console.WriteLine();
            Console.WriteLine(SectionIngredients);
            Console.WriteLine("============");

            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                Console.WriteLine(ingredient.ToString());
            }

            Console.WriteLine();
            Console.WriteLine(SectionInstructions);
            Console.WriteLine("=============");

            foreach (string instruction in recipe.Instructions)
            {
                Console.WriteLine(instruction);
            }

            Console.WriteLine();
        }

        // Visa alla lister om inget annat finns.
        public virtual void Show(IEnumerable<IRecipe> recipes)
        {
            foreach (IRecipe recipe in recipes)
            {
                if (recipe != null)
                {
                    Show(recipe);
                }

                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ╔══════════════════════════════════════╗ ");
                Console.WriteLine(" ║     Press anykey to next recipe      ║ ");
                Console.WriteLine(" ╚══════════════════════════════════════╝ ");
                Console.ResetColor();
                Console.ReadLine();
                Console.ResetColor();
            }
            
        }
        // Först så tar den en sträng sen skriver ut själva objektets rubrik.
        public virtual void ShowHeader(string name)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ╔══════════════════════════════════════╗ ");
            Console.WriteLine(" ║           " + name + "            ");
            Console.WriteLine(" ╚══════════════════════════════════════╝ ");
            Console.ResetColor();
        }
        public static void breakNow()
        {
            System.Threading.Thread.Sleep(4000);
        }
    }
}