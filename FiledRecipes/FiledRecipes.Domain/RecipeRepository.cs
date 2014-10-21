using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiledRecipes.Domain
{
    /// <summary>
    /// Holder for recipes.
    /// </summary>
    public class RecipeRepository : IRecipeRepository
    {
        /// <summary>
        /// Represents the recipe section.
        /// </summary>
        private const string SectionRecipe = "[Recept]";
        /// <summary>
        /// Represents the ingredients section.
        /// </summary>
        private const string SectionIngredients = "[Ingredienser]";
        /// <summary>
        /// Represents the instructions section.
        /// </summary>
        private const string SectionInstructions = "[Instruktioner]";
        /// <summary>
        /// Occurs after changes to the underlying collection of recipes.
        /// </summary>
        public event EventHandler RecipesChangedEvent;
        /// <summary>
        /// Specifies how the next line read from the file will be interpreted.
        /// </summary>
        private enum RecipeReadStatus { Indefinite, New, Ingredient, Instruction };
        /// <summary>
        /// Collection of recipes.
        /// </summary>
        private List<IRecipe> _recipes;
        /// <summary>
        /// The fully qualified path and name of the file with recipes.
        /// </summary>
        private string _path;
        /// <summary>
        /// Indicates whether the collection of recipes has been modified since it was last saved.
        /// </summary>
        public bool IsModified { get; protected set; }
        /// <summary>
        /// Initializes a new instance of the RecipeRepository class.
        /// </summary>
        /// <param name="path">The path and name of the file with recipes.</param>
        public RecipeRepository(string path)
        {
            // Throws an exception if the path is invalid.
            _path = Path.GetFullPath(path);
            _recipes = new List<IRecipe>();
        }
        /// <summary>
        /// Returns a collection of recipes.
        /// </summary>
        /// <returns>A IEnumerable&lt;Recipe&gt; containing all the recipes.</returns>
        public virtual IEnumerable<IRecipe> GetAll()
        {
            // Deep copy the objects to avoid privacy leaks.
            return _recipes.Select(r => (IRecipe)r.Clone());
        }
        /// <summary>
        /// Returns a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to get.</param>
        /// <returns>The recipe at the specified index.</returns>
        public virtual IRecipe GetAt(int index)
        {
            // Deep copy the object to avoid privacy leak.
            return (IRecipe)_recipes[index].Clone();
        }
        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete. The value can be null.</param>
        public virtual void Delete(IRecipe recipe)
        {
            // If it's a copy of a recipe...
            if (!_recipes.Contains(recipe))
            {
                // ...try to find the original!
                recipe = _recipes.Find(r => r.Equals(recipe));
            }
            _recipes.Remove(recipe);
            IsModified = true;
            OnRecipesChanged(EventArgs.Empty);
        }
        /// <summary>
        /// Deletes a recipe.
        /// </summary>
        /// <param name="index">The zero-based index of the recipe to delete.</param>
        public virtual void Delete(int index)
        {
            Delete(_recipes[index]);
        }
        /// <summary>
        /// Raises the RecipesChanged event.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected virtual void OnRecipesChanged(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = RecipesChangedEvent;
            // Event will be null if there are no subscribers.
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
        public virtual void Load()
        {
            List<IRecipe> recipeList = new List<IRecipe>();

            try
            {
                Recipe recipe = null;
                int rowCount = 0;
                RecipeReadStatus status = RecipeReadStatus.Indefinite;

                using (StreamReader sr = new StreamReader(this._path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        rowCount++;
                        try
                        {
                            if (line == String.Empty)
                            {
                                status = RecipeReadStatus.Indefinite;
                            }
                            else if (line == "[Recept]")
                            {
                                status = RecipeReadStatus.New;
                            }
                            else if (line == "[Ingredienser]")
                            {
                                status = RecipeReadStatus.Ingredient;
                            }
                            else if (line == "[Instruktioner]")
                            {
                                status = RecipeReadStatus.Instruction;
                            }
                            else
                            {
                                if (status == RecipeReadStatus.New)
                                {
                                    recipe = new Recipe(line);
                                    status = RecipeReadStatus.Indefinite;
                                }
                                else if (status == RecipeReadStatus.Ingredient)
                                {
                                    string[] ingredientsContainer = line.Split(';');
                                    try
                                    {
                                        Ingredient ingredient = new Ingredient();
                                        ingredient.Amount = ingredientsContainer[0];
                                        ingredient.Measure = ingredientsContainer[1];
                                        ingredient.Name = ingredientsContainer[2];
                                        recipe.Add(ingredient);
                                    }
                                    catch
                                    {
                                        throw new ArgumentException("Kunde inte läsa raderna ur filen.");
                                    }
                                }
                                else if (status == RecipeReadStatus.Instruction)
                                {
                                    recipe.Add(line);
                                }
                            }
                            if ((status == RecipeReadStatus.New && rowCount != 1) || sr.Peek() < 0)
                            {
                                recipeList.Add(recipe);
                            }

                        }
                        catch
                        {
                            throw new ArgumentException("Någonting blev fel");
                        }
                    }
                }
                _recipes = recipeList.OrderBy(recipestatus => recipestatus.Name).ToList();
                IsModified = true;
                OnRecipesChanged(EventArgs.Empty);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
        public virtual void Save()
        {

            try
            {
                using (StreamWriter sw = new StreamWriter(this._path, false))
                {
                    
                    foreach (Recipe recipe in _recipes)
                    {
                        sw.WriteLine("[Recept]");
                        sw.WriteLine(recipe.Name);
                        sw.WriteLine("[Ingredienser]");
                        foreach (Ingredient ingredient in recipe.Ingredients)
                        {
                            sw.WriteLine(ingredient.Amount + ";" + ingredient.Measure + ";" + ingredient.Name);
                        }
                        sw.WriteLine("[Instruktioner]");
                        foreach (string direction in recipe.Instructions)
                        {
                            sw.WriteLine(direction);
                        }

                    }

                }
            }
            catch
            {
                throw new ApplicationException("Filen kunde inte sparas");
            }
        }
    }
}