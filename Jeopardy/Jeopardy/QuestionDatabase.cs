using System;
using System.Collections.Generic;

namespace Jeopardy
{
    /// <summary>
    /// Represents a single clue/question
    /// </summary>
    public class ClueData
    {
        public int Value { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }

        public ClueData(int value, string question, string answer)
        {
            Value = value;
            Question = question ?? "";
            Answer = answer ?? "";
            IsAnswered = false;
        }
    }

    /// <summary>
    /// Represents a set of question topics/categories
    /// </summary>
    public class QuestionSet
    {
        public string SetName { get; set; }
        public string Description { get; set; }
        public Dictionary<string, List<ClueData>> Categories { get; set; }

        public QuestionSet(string name, string description = "")
        {
            SetName = name;
            Description = description;
            Categories = new Dictionary<string, List<ClueData>>();
        }

        /// <summary>
        /// Add a category with 5 clues (200, 400, 600, 800, 1000)
        /// </summary>
        public void AddCategory(string categoryName, List<ClueData> clues)
        {
            if (clues.Count == 5)
            {
                Categories[categoryName] = clues;
            }
        }

        public List<string> GetCategoryNames()
        {
            return new List<string>(Categories.Keys);
        }
    }

    /// <summary>
    /// Database of predefined question sets
    /// </summary>
    public static class QuestionDatabase
    {
        public static List<QuestionSet> GetAvailableQuestionSets()
        {
            return new List<QuestionSet>
            {
                CreateClassicSet(),
                CreateScienceSet(),
                CreateHistorySet(),
                CreateMoviesSet(),
                CreateSportsSet()
            };
        }

        private static QuestionSet CreateClassicSet()
        {
            QuestionSet set = new QuestionSet("Classic Mix", "A mix of various categories");

            set.AddCategory("Science", new List<ClueData>
            {
                new ClueData(200, "This gas makes up most of Earth's atmosphere.", "what is nitrogen"),
                new ClueData(400, "H2O is the chemical formula for this.", "what is water"),
                new ClueData(600, "The force that pulls objects toward Earth.", "what is gravity"),
                new ClueData(800, "The planet known as the 'Red Planet'.", "what is mars"),
                new ClueData(1000, "The unit of electrical resistance.", "what is ohm")
            });

            set.AddCategory("History", new List<ClueData>
            {
                new ClueData(200, "Who was the first President of the United States?", "who is george washington"),
                new ClueData(400, "This ancient wonder was located at Alexandria.", "what is the lighthouse of alexandria"),
                new ClueData(600, "The year World War II ended.", "what is 1945"),
                new ClueData(800, "The name of the ship Charles Darwin sailed on.", "what is the beagle"),
                new ClueData(1000, "This empire fell in 1453 with the fall of Constantinople.", "what is the ottoman empire")
            });

            set.AddCategory("Movies", new List<ClueData>
            {
                new ClueData(200, "In 'The Shawshank Redemption', this actor played Andy Dufresne.", "who is tim robbins"),
                new ClueData(400, "Which movie featured a character named 'Forrest Gump'?", "what is forrest gump"),
                new ClueData(600, "Who played Jack Dawson in 'Titanic'?", "who is leonardo dicaprio"),
                new ClueData(800, "Which film won the Oscar for Best Picture in 1994?", "what is forrest gump"),
                new ClueData(1000, "Who directed 'Jurassic Park'?", "who is steven spielberg")
            });

            set.AddCategory("Sports", new List<ClueData>
            {
                new ClueData(200, "This country won the FIFA World Cup in 2018.", "what is france"),
                new ClueData(400, "Michael Jordan wore this number for most of his career.", "what is 23"),
                new ClueData(600, "The sport known as 'the beautiful game'.", "what is soccer"),
                new ClueData(800, "The athlete with the most Olympic gold medals.", "who is michael phelps"),
                new ClueData(1000, "The team that won the first Super Bowl.", "what is the green bay packers")
            });

            set.AddCategory("Geography", new List<ClueData>
            {
                new ClueData(200, "The longest river in Africa.", "what is the nile"),
                new ClueData(400, "The capital of Japan.", "what is tokyo"),
                new ClueData(600, "The driest desert on Earth (excluding poles).", "what is the atacama"),
                new ClueData(800, "The smallest country in the world.", "what is vatican city"),
                new ClueData(1000, "The largest ocean on Earth.", "what is the pacific")
            });

            return set;
        }

        private static QuestionSet CreateScienceSet()
        {
            QuestionSet set = new QuestionSet("Science", "Science and nature questions");

            set.AddCategory("Biology", new List<ClueData>
            {
                new ClueData(200, "The basic unit of life.", "what is a cell"),
                new ClueData(400, "The powerhouse of the cell.", "what is a mitochondrion"),
                new ClueData(600, "This process produces oxygen in plants.", "what is photosynthesis"),
                new ClueData(800, "The study of heredity and genes.", "what is genetics"),
                new ClueData(1000, "The number of chromosomes in a human cell.", "what is 46")
            });

            set.AddCategory("Physics", new List<ClueData>
            {
                new ClueData(200, "The SI unit of force.", "what is a newton"),
                new ClueData(400, "The speed of this in vacuum is constant at 299,792,458 m/s.", "what is light"),
                new ClueData(600, "Energy equals mass times this squared.", "what is the speed of light"),
                new ClueData(800, "This principle states two objects cannot occupy the same space.", "what is pauli exclusion principle"),
                new ClueData(1000, "The theoretical physicist who developed general relativity.", "who is albert einstein")
            });

            set.AddCategory("Chemistry", new List<ClueData>
            {
                new ClueData(200, "The atomic number of Carbon.", "what is 6"),
                new ClueData(400, "This element is essential for human respiration.", "what is oxygen"),
                new ClueData(600, "The pH value of pure water at 25°C.", "what is 7"),
                new ClueData(800, "The process of converting liquid to gas.", "what is evaporation"),
                new ClueData(1000, "The noble gas with the highest atomic number in nature.", "what is xenon")
            });

            set.AddCategory("Astronomy", new List<ClueData>
            {
                new ClueData(200, "Our sun is this type of star.", "what is a yellow dwarf"),
                new ClueData(400, "The number of planets in our solar system.", "what is 8"),
                new ClueData(600, "The closest star to Earth besides the sun.", "what is proxima centauri"),
                new ClueData(800, "This is the most massive object in our solar system.", "what is the sun"),
                new ClueData(1000, "The term for the point of no return around a black hole.", "what is the event horizon")
            });

            set.AddCategory("Technology", new List<ClueData>
            {
                new ClueData(200, "The inventor of the telephone.", "who is alexander graham bell"),
                new ClueData(400, "This computer scientist is known for developing the web.", "who is tim berners-lee"),
                new ClueData(600, "The first computer programmed by Augusta Ada Lovelace.", "what is the analytical engine"),
                new ClueData(800, "This programming language is known as the 'language of the web'.", "what is javascript"),
                new ClueData(1000, "The name of the quantum computing algorithm for factoring.", "what is shor's algorithm")
            });

            return set;
        }

        private static QuestionSet CreateHistorySet()
        {
            QuestionSet set = new QuestionSet("History", "Historical events and figures");

            set.AddCategory("Ancient History", new List<ClueData>
            {
                new ClueData(200, "The city where the Hanging Gardens were built.", "what is babylon"),
                new ClueData(400, "The Roman empire lasted this many centuries before the fall of Rome.", "what is 5"),
                new ClueData(600, "This philosopher taught Aristotle.", "who is plato"),
                new ClueData(800, "The year the Roman Empire fell.", "what is 476"),
                new ClueData(1000, "This ancient Egyptian queen had a famous affair with Mark Antony.", "who is cleopatra")
            });

            set.AddCategory("Medieval Period", new List<ClueData>
            {
                new ClueData(200, "The year William the Conqueror invaded England.", "what is 1066"),
                new ClueData(400, "This war lasted 100 years between England and France.", "what is the hundred years' war"),
                new ClueData(600, "The medieval period is also called the Dark Ages or this.", "what is the middle ages"),
                new ClueData(800, "Joan of Arc was from this country.", "what is france"),
                new ClueData(1000, "The year the Byzantine Empire fell to the Ottomans.", "what is 1453")
            });

            set.AddCategory("Modern Era", new List<ClueData>
            {
                new ClueData(200, "The year of the French Revolution.", "what is 1789"),
                new ClueData(400, "Napoleon's final exile was to this island.", "what is saint helena"),
                new ClueData(600, "The year the United States declared independence.", "what is 1776"),
                new ClueData(800, "The year World War I ended.", "what is 1918"),
                new ClueData(1000, "The year the Berlin Wall fell.", "what is 1989")
            });

            set.AddCategory("Great Leaders", new List<ClueData>
            {
                new ClueData(200, "This British Prime Minister led during World War II.", "who is winston churchill"),
                new ClueData(400, "This US President ended slavery with the Emancipation Proclamation.", "who is abraham lincoln"),
                new ClueData(600, "This Indian leader is known for non-violent resistance.", "who is mahatma gandhi"),
                new ClueData(800, "This Soviet leader initiated the policy of Glasnost.", "who is mikhail gorbachev"),
                new ClueData(1000, "This Chinese leader established the People's Republic of China.", "who is mao zedong")
            });

            set.AddCategory("Revolutions", new List<ClueData>
            {
                new ClueData(200, "The revolution that overthrew the Bastille.", "what is the french revolution"),
                new ClueData(400, "This revolution aimed to transform Russia's government in 1917.", "what is the russian revolution"),
                new ClueData(600, "The technological revolution that began in 18th century Britain.", "what is the industrial revolution"),
                new ClueData(800, "This 20th century revolution occurred in Cuba.", "what is the cuban revolution"),
                new ClueData(1000, "The revolution that resulted in the establishment of the United States.", "what is the american revolution")
            });

            return set;
        }

        private static QuestionSet CreateMoviesSet()
        {
            QuestionSet set = new QuestionSet("Movies", "Films and cinema");

            set.AddCategory("Classics", new List<ClueData>
            {
                new ClueData(200, "The year 'Citizen Kane' was released.", "what is 1941"),
                new ClueData(400, "This 1939 film asked 'Frankly, my dear, do you give a damn?'", "what is gone with the wind"),
                new ClueData(600, "The director of 'Singin' in the Rain'.", "who is gene kelly"),
                new ClueData(800, "This 1954 film features Marlon Brando and is based on a novel.", "what is on the waterfront"),
                new ClueData(1000, "The first animated feature film by Disney.", "what is snow white and the seven dwarfs")
            });

            set.AddCategory("Action", new List<ClueData>
            {
                new ClueData(200, "The actor who plays James Bond in most recent films.", "who is daniel craig"),
                new ClueData(400, "The 'Matrix' films explore this philosophical concept.", "what is reality"),
                new ClueData(600, "The superhero who first appeared in 'Iron Man'.", "who is tony stark"),
                new ClueData(800, "This Christopher Nolan film involves dreams within dreams.", "what is inception"),
                new ClueData(1000, "The highest-grossing action film of all time (non-inflation adjusted).", "what is avengers endgame")
            });

            set.AddCategory("Comedy", new List<ClueData>
            {
                new ClueData(200, "The main character in 'Forrest Gump' played by Tom Hanks.", "who is forrest gump"),
                new ClueData(400, "Jim Carrey's character in 'The Truman Show'.", "who is truman burbank"),
                new ClueData(600, "The Marx Brothers' famous 'A Night at the' film series.", "what is the opera"),
                new ClueData(800, "This Wes Anderson film features the Grand Hotel Budapest.", "what is the grand budapest hotel"),
                new ClueData(1000, "The film that won Best Picture at the Oscars in 2020.", "what is parasite")
            });

            set.AddCategory("Drama", new List<ClueData>
            {
                new ClueData(200, "The prison shown in 'The Shawshank Redemption'.", "what is shawshank"),
                new ClueData(400, "The year 'Schindler's List' was released.", "what is 1993"),
                new ClueData(600, "This film depicts the life of Stephen Hawking.", "what is the theory of everything"),
                new ClueData(800, "The composer who scored 'The Piano'.", "who is michael nyman"),
                new ClueData(1000, "This film follows the lives of four elderly women in Miami.", "what is the golden girls")
            });

            set.AddCategory("Sci-Fi", new List<ClueData>
            {
                new ClueData(200, "The year the original 'Star Wars' film was released.", "what is 1977"),
                new ClueData(400, "The computer antagonist in '2001: A Space Odyssey'.", "what is hal 9000"),
                new ClueData(600, "The primary setting of the 'Alien' franchise.", "what is the nostromo"),
                new ClueData(800, "In 'The Fifth Element', this is the ultimate weapon.", "what is leeloo"),
                new ClueData(1000, "The year 'Blade Runner' takes place.", "what is 2049")
            });

            return set;
        }

        private static QuestionSet CreateSportsSet()
        {
            QuestionSet set = new QuestionSet("Sports", "Sports and athletics");

            set.AddCategory("Football", new List<ClueData>
            {
                new ClueData(200, "The team that won Super Bowl LVI.", "what is the los angeles rams"),
                new ClueData(400, "The city where the Green Bay Packers play.", "what is green bay"),
                new ClueData(600, "Tom Brady's primary team for most of his career.", "what is the new england patriots"),
                new ClueData(800, "The year the NFL was founded.", "what is 1920"),
                new ClueData(1000, "The only player to win the Heisman Trophy and Super Bowl in same year.", "what is desmond howard")
            });

            set.AddCategory("Basketball", new List<ClueData>
            {
                new ClueData(200, "The team Michael Jordan played for.", "what is the chicago bulls"),
                new ClueData(400, "The number of championships the Lakers won in the 80s.", "what is 5"),
                new ClueData(600, "LeBron James' birth name.", "what is lebron raymone james"),
                new ClueData(800, "The year basketball was invented.", "what is 1891"),
                new ClueData(1000, "The first player to score 100 points in a single game.", "who is wilt chamberlain")
            });

            set.AddCategory("Soccer", new List<ClueData>
            {
                new ClueData(200, "The team Lionel Messi won the World Cup with.", "what is argentina"),
                new ClueData(400, "The country that hosted the 2022 World Cup.", "what is qatar"),
                new ClueData(600, "Pelé's birth name.", "what is edson arantes do nascimento"),
                new ClueData(800, "The number of players on a soccer field per team.", "what is 11"),
                new ClueData(1000, "The year the FIFA World Cup began.", "what is 1930")
            });

            set.AddCategory("Tennis", new List<ClueData>
            {
                new ClueData(200, "The player with the most Grand Slam titles (men's).", "who is novak djokovic"),
                new ClueData(400, "The French Grand Slam tournament is played on this surface.", "what is clay"),
                new ClueData(600, "The number of sets in a men's Grand Slam match.", "what is 5"),
                new ClueData(800, "Serena Williams won this many Grand Slam titles.", "what is 23"),
                new ClueData(1000, "The oldest Grand Slam tournament.", "what is wimbledon")
            });

            set.AddCategory("Olympics", new List<ClueData>
            {
                new ClueData(200, "The city that hosted the first modern Olympics.", "what is athens"),
                new ClueData(400, "The number of rings in the Olympic symbol.", "what is 5"),
                new ClueData(600, "Usain Bolt's 100m Olympic record time.", "what is 9.63 seconds"),
                new ClueData(800, "The year Olympics are held.", "what is every four years"),
                new ClueData(1000, "The number of countries that participated in the first Olympics.", "what is 14")
            });

            return set;
        }
    }
}
